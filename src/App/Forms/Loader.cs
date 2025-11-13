using Launcher.App.Profile;
using Launcher.App.Server;
using Launcher.App.Utility;
using Launcher.Library.Constant;
using Launcher.Library.File.OldImplements;
using Launcher.Properties;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Launcher.App.Forms
{
    public partial class Loader : Form
    {
        private readonly System.Windows.Forms.Timer animationTimer;

        public Loader()
        {
            InitializeComponent();

            // 在构造函数中初始化定时器（在 UI 线程运行）
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 50; // 25ms 每次 tick，可根据需要调整
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            StartAnimation();

            Task.Run(() =>
            {
                try
                {
                    Load_Main();
                    PromptMsg.Text = "读取Data文件...";
                    Load_Data();
                    PromptMsg.Text = "加载特殊赛车配置...";
                    Load_Kart_Data();
                }
                catch { }
                finally
                {
                    // 操作完成后停止动画并进行 UI 收尾并关闭窗体
                    try
                    {
                        this.Invoke(() =>
                        {
                            try
                            {
                                animationTimer.Stop();
                                animationTimer.Dispose();
                                ProgressBar.Value = ProgressBar.Minimum;
                            }
                            catch { }

                            Utils.PrintDivLine();
                            Dispose();
                        });
                    }
                    catch { }
                }
            });
        }

        private void Load_Main()
        {
            if (Process.GetProcessesByName("KartRider").Length != 0)
            {
                Utils.MsgKartIsRunning();
                return;
            }

            // find game directory
            if (Utils.CheckGameAvailability(FileName.AppDir))
            {
                // working directory
                Program.RootDirectory = FileName.AppDir;
                Console.WriteLine("使用当前目录下的游戏.");
            }
            else if (Utils.CheckGameAvailability(FileName.TCGKartGamePath))
            {
                // TCGame registered directory
                Program.RootDirectory = FileName.TCGKartGamePath;
                Console.WriteLine("使用TCGame注册的游戏目录下的游戏.");
            }
            else
            {
                // game not found
                Utils.MsgFileNotFound();
            }
            Console.WriteLine($"游戏目录: {Program.RootDirectory}");
            Utils.PrintDivLine();
        }

        private void Load_Data()
        {
            Console.WriteLine("读取Data文件...");
            try
            {
                var packFolderManager = KartRhoFile.Dump(Path.GetFullPath(Path.Combine(Program.RootDirectory, @"Data\aaa.pk")));
                if (packFolderManager == null)
                {
                    // MsgErrorReadData 可能会弹窗，必须回到 UI 线程调用
                    this.Invoke(() => Utils.MsgErrorReadData());
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

        private void Load_Kart_Data()
        {
            Console.WriteLine("加载特殊赛车配置...");
            string ModelMax = Resources.ModelMax;
            if (!File.Exists(FileName.ModelMax_LoadFile))
            {
                using (StreamWriter streamWriter = new StreamWriter(FileName.ModelMax_LoadFile, false))
                {
                    streamWriter.Write(ModelMax);
                }
            }
            XmlUpdater updater = new();
            updater.UpdateLocalXmlWithResource(FileName.ModelMax_LoadFile, ModelMax);

            SpecialKartConfig.SaveConfigToFile(FileName.SpecialKartConfig);
            MultiPlayer.kartConfig = SpecialKartConfig.LoadConfigFromFile(FileName.SpecialKartConfig);
        }

        private void StartAnimation()
        {
            // 启动定时器（UI 线程），Tick 时安全更新 ProgressBar
            if (!animationTimer.Enabled)
                animationTimer.Start();
        }

        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                // 循环进度条：不断增加 Value，超过 Maximum 时回到 Minimum（可保留超出的步长或直接回到 Minimum）
                int step = 5; // 每次增加的步长，可按需调整
                int next = ProgressBar.Value + step;
                if (next > ProgressBar.Maximum)
                {
                    // 超出最大值后从 Minimum 重新开始
                    ProgressBar.Value = ProgressBar.Minimum;
                }
                else
                {
                    ProgressBar.Value = next;
                }
            }
            catch { }
        }
    }
}
