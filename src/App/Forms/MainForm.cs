using Launcher.App;
using Launcher.App.Constant;
using Launcher.App.ExcData;
using Launcher.App.Forms;
using Launcher.App.Logger;
using Launcher.App.Profile;
using Launcher.App.Server;
using Launcher.App.Utility;
using Launcher.Library.Data;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Compression;
using static Launcher.App.Program;
using static Launcher.App.Utility.Utils;

namespace Launcher.App.Forms
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// KartRider.exe Path
        /// </summary>
        public static string KartRider = Path.GetFullPath(Path.Combine(RootDirectory, FileName.KartRider));

        /// <summary>
        /// PinFile Path
        /// </summary>
        /// </summary>
        public static string PinFile = Path.GetFullPath(Path.Combine(RootDirectory, FileName.PinFile));

        /// <summary>
        /// Backup PinFile Path
        /// </summary>
        public static string PinFileBak = Path.GetFullPath(Path.Combine(RootDirectory, FileName.PinFileBak));

        /// <summary>
        /// The PinFile Object
        /// </summary>
        private static PINFile PinFileData = new(PinFile);

        public MainForm()
        {
            // Initialize Component
            InitializeComponent();

            ClientVersion.Location = new Point(label_Client.Location.X + 70, label_Client.Location.Y);
            VersionLabel.Location = new Point(Launcher_label.Location.X + 70, Launcher_label.Location.Y);

            StartPosition = FormStartPosition.Manual;
            Rectangle screen = Screen.PrimaryScreen != null ? Screen.PrimaryScreen.WorkingArea : new Rectangle(0, 0, Width, Height);
            Location = new Point(screen.Width - Width, screen.Height - Height);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (Process.GetProcessesByName("KartRider").Length != 0)
            {
                MessageBox.Show("跑跑卡丁车正在运行!\n为保证游戏文件不被损坏, 请结束跑跑卡丁车后再退出该程序!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
                return;
            }
            if (File.Exists(PinFileBak)) // restore PinFile
            {
                File.Delete(PinFile);
                File.Move(PinFileBak, PinFile);
            }
            ProfileService.Save(ProfileService.SettingConfig.Name);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            new Loader().ShowDialog();

            ProfileService.SettingConfig.ClientVersion = PinFileData.Header.MinorVersion;
            ProfileService.ProfileConfigs[ProfileService.SettingConfig.Name].GameOption.Version = PinFileData.Header.MinorVersion;
            ProfileService.Save(ProfileService.SettingConfig.Name);

            ClientVersion.Text = $"P{ProfileService.ProfileConfigs[ProfileService.SettingConfig.Name].GameOption.Version}";
            VersionLabel.Text = Constants.Version;

            Console.WriteLine($"[INFO] Game Client Version: {PinFileData.Header.MinorVersion}");
            Console.WriteLine($"[INFO] Launcher Version: {Constants.Version}");

            if (Constants.DBG)
                Console.WriteLine($"Config:\n{JsonConvert.SerializeObject(ProfileService.ProfileConfigs[ProfileService.SettingConfig.Name], Newtonsoft.Json.Formatting.Indented)}");

            Console.WriteLine($"[INFO] Process: {KartRider}");

            try
            {
                RouterListener.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                if (ex is System.Net.Sockets.SocketException)
                {
                    MsgMultiInstance();
                }
            }
        }


        private void Start_Button_Click(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName("KartRider").Length != 0)
            {
                MsgKartIsRunning();
                return;
            }
            if (!CheckGameAvailability(Program.RootDirectory))
            {
                MsgFileNotFound();
                return;
            }
            new Thread(() =>
            {
                Start_Button.Enabled = false;
                GetKart_Button.Enabled = false;

                Console.WriteLine("Backing up old PinFile...");
                if (File.Exists(PinFileBak))
                {
                    File.Delete(PinFile);
                    File.Move(PinFileBak, PinFile);
                }
                File.Copy(PinFile, PinFileBak);
                Console.WriteLine($"Backup PinFile: {PinFileBak}");

                PinFileData = new(PinFile);
                foreach (PINFile.AuthMethod authMethod in PinFileData.AuthMethods)
                {
                    Console.WriteLine($"Changing IP to your server... {authMethod.Name}");
                    foreach (PINFile.IPEndPoint loginServer in authMethod.LoginServers)
                    {
                        Console.WriteLine($"{loginServer} -> {ProfileService.SettingConfig.ServerIP}:{ProfileService.SettingConfig.ServerPort}");
                    }
                    authMethod.LoginServers.Clear();
                    authMethod.LoginServers.Add(new PINFile.IPEndPoint
                    {
                        IP = ProfileService.SettingConfig.ServerIP,
                        Port = ProfileService.SettingConfig.ServerPort
                    });
                    Console.WriteLine($"All Changed to {authMethod.LoginServers[0]} \n");
                }
                Console.WriteLine("All IP has been Changed to your server");

                Console.WriteLine("Scanning Bml Objects in PinFile...");
                foreach (BmlObject bml in PinFileData.BmlObjects)
                {
                    for (int i = bml.SubObjects.Count - 1; i >= 0; i--)
                    {
                        Console.WriteLine($"Found {bml.SubObjects[i].Item1} in {bml.Name}");
                        if (bml.SubObjects[i].Item1 != "NgsOn")
                            continue;
                        Console.WriteLine($"Removing {bml.SubObjects[i].Item1}");
                        bml.SubObjects.RemoveAt(i);
                    }
                }
                Console.WriteLine();

                File.WriteAllBytes(PinFile, PinFileData.GetEncryptedData());
                try
                {
                    new MemoryModifier().LaunchAndModifyMemory(RootDirectory);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when launch: {ex.Message}");
                }
                finally
                {
                    Start_Button.Enabled = true;
                    GetKart_Button.Enabled = true;
                }
            }).Start();
        }

        private void GetKart_Button_Click(object sender, EventArgs e)
        {
            GetKartDlg = new();
            GetKartDlg.ShowDialog();
        }

        private void GitHub_Click(object sender, EventArgs e)
        {
            string url = $"https://github.com/{Constants.Owner}/{Constants.Repo}";
            TryOpenUrl(url);
        }
        private void VersionLabel_MouseEnter(object sender, EventArgs e)
        {
            new ToolTip().SetToolTip(VersionLabel, "点击前往GitHub Release");
        }

        private void GitHub_Release_Click(object sender, EventArgs e)
        {
            string url = $"https://github.com/{Constants.Owner}/{Constants.Repo}/releases";
            TryOpenUrl(url);
        }

        private void GitHub_MouseEnter(object sender, EventArgs e)
        {
            new ToolTip().SetToolTip(GitHub, "点击前往GitHub仓库");
        }

        private void KartInfo_Click(object sender, EventArgs e)
        {
            string url = "https://kartinfo.me/thread-9369-1-1.html";
            TryOpenUrl(url);
        }

        private void KartInfo_MouseEnter(object sender, EventArgs e)
        {
            new ToolTip().SetToolTip(KartInfo, "点击前往KartInfo论坛");
        }

        private void label_Client_Click(object sender, EventArgs e)
        {
            string url = "https://github.com/brownsugar/popkart-client-archive/releases";
            TryOpenUrl(url);
        }

        private void ClientVersion_MouseEnter(object sender, EventArgs e)
        {
            new ToolTip().SetToolTip(ClientVersion, "点击前往BrownSugar的跑跑卡丁车存档");
        }

        private void label_Docs_Click(object sender, EventArgs e)
        {
            string url = "https://themagicflute.github.io/Launcher_V2/";
            TryOpenUrl(url);
        }

        private void label_TimeAttackLog_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(FileName.TimeAttackLog) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                if (ex is System.ComponentModel.Win32Exception)
                {
                    Console.WriteLine("计时日志文件未找到, 请进行计时后再查看!");
                }
                else
                {
                    Console.WriteLine($"查找计时日志时发生错误: {ex.Message}");
                }
            }
        }

        private void button_More_Options_Click(object sender, EventArgs e)
        {
            OptionsDlg = new();
            OptionsDlg.ShowDialog();
        }

        private void ConsoleLogger_Click(object sender, EventArgs e)
        {
            string logFileName = CachedConsoleWriter.SaveToFile();
            CachedConsoleWriter.cachedWriter.ClearCache();
            if (logFileName == string.Empty)
                MessageBox.Show("日志写入失败!", "写入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show($"日志已经写入了{logFileName}", "写入完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Settings_Click(object sender, EventArgs e)
        {
            SettingDlg = new();
            SettingDlg.ShowDialog();
        }
    }
}
