using Launcher.App.Constant;
using Launcher.App.Profile;
using Launcher.App.Server;
using Launcher.App.Utility;
using Launcher.Properties;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Launcher.App.Forms
{
    public partial class Loader : Form
    {
        public Loader()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            if (!Check_Game())
            {
                Dispose();
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    PromptMsg.Text = "读取正在加载...";
                    Load_Main();
                    PromptMsg.Text = "读取Data文件...";
                    Load_Data();
                    PromptMsg.Text = "加载特殊赛车配置...";
                    Load_Kart_Data();

                    MainForm.GameIsReady = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when loading: {ex.Message}");
                }
                finally
                {
                    // 操作完成后, 关闭加载窗口
                    try
                    {
                        Invoke(() =>
                        {
                            Utils.PrintDivLine();
                            if (Constants.DBG)
                            {
                                Console.WriteLine($"Config:\n{JsonConvert.SerializeObject(ProfileService.ProfileConfigs[ProfileService.SettingConfig.Name], Formatting.Indented)}");
                                Utils.PrintDivLine();
                            }
                            Console.WriteLine($"[INFO] Game: {MainForm.KartRider}");
                            Console.WriteLine($"[INFO] Game Client Version: P{MainForm.PinFileData.Header.MinorVersion}");
                            Console.WriteLine($"[INFO] Launcher Version: {Constants.VERSION}");
                            Utils.PrintDivLine();
                            Dispose();
                        });
                    }
                    catch { }
                }
            });
        }

        private bool Check_Game()
        {
            if (Process.GetProcessesByName("KartRider").Length != 0)
            {
                Utils.MsgKartIsRunning();
                return false;
            }

            if (!MainForm.GameIsReady)
            {
                // Cheack + set default game path
                // find game directory
                if (Utils.CheckGameAvailability(FileName.AppDir))
                {
                    // working directory
                    MainForm.GameDir = FileName.AppDir;
                    Console.WriteLine("使用当前目录下的游戏.");
                    MainForm.GameIsReady = true;
                }
                else if (Utils.CheckGameAvailability(FileName.TCGKartGamePath))
                {
                    // TCGame registered directory
                    MainForm.GameDir = FileName.TCGKartGamePath;
                    Console.WriteLine("使用TCGame注册的游戏目录下的游戏.");
                    MainForm.GameIsReady = true;
                }
                else
                {
                    // game not found
                    MainForm.GameDir = string.Empty;
                    Utils.MsgFileNotFound();
                    return false;
                }
            }
            Console.WriteLine($"游戏目录: {(MainForm.GameDir != string.Empty ? MainForm.GameDir : "未知")}");
            Utils.PrintDivLine();
            return true;
        }

        private void Load_Main()
        {
            if (MainForm.GameIsReady)
            {
                MainForm.KartRider = Path.GetFullPath(Path.Combine(MainForm.GameDir, FileName.KartRider));
                MainForm.PinFile = Path.GetFullPath(Path.Combine(MainForm.GameDir, FileName.PinFile));
                MainForm.PinFileBak = Path.GetFullPath(Path.Combine(MainForm.GameDir, FileName.PinFileBak));
                MainForm.PinFileData = new(MainForm.PinFile);

                ProfileService.SettingConfig.ClientVersion = MainForm.PinFileData.Header.MinorVersion;
                ProfileService.ProfileConfigs[ProfileService.SettingConfig.Name].GameOption.Version = MainForm.PinFileData.Header.MinorVersion;
                ProfileService.SettingConfig.LocaleID = MainForm.PinFileData.Header.LocaleID;
                ProfileService.SettingConfig.nClientLoc = MainForm.PinFileData.Header.Unk2;
                ProfileService.Save(ProfileService.SettingConfig.Name);
            }
        }

        private async void Load_Data()
        {
            Console.WriteLine("读取Data文件...");
            try
            {
                Library.File.OldImplements.PackFolderManager packFolderManager = KartRhoFile.Dump(Path.GetFullPath(Path.Combine(MainForm.GameDir, @"Data\aaa.pk")));
                if (packFolderManager is null)
                {
                    // MsgErrorReadData 可能会弹窗，必须回到 UI 线程调用
                    Invoke(Utils.MsgErrorReadData);
                    return;
                }
                packFolderManager.Reset();
                Console.WriteLine("Data文件读取完成!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"读取Data文件时出错: {ex.Message}");
            }
        }

        private async void Load_Kart_Data()
        {
            Console.WriteLine("加载特殊赛车配置...");
            string ModelMax = Resources.ModelMax;
            if (!File.Exists(FileName.ModelMax_LoadFile))
            {
                using (StreamWriter streamWriter = new(FileName.ModelMax_LoadFile, false))
                {
                    streamWriter.Write(ModelMax);
                }
            }

            new XmlUpdater().UpdateLocalXmlWithResource(FileName.ModelMax_LoadFile, ModelMax);

            SpecialKartConfig.SaveConfigToFile(FileName.SpecialKartConfig);
            MultiPlayer.kartConfig = SpecialKartConfig.LoadConfigFromFile(FileName.SpecialKartConfig);
        }
    }
}
