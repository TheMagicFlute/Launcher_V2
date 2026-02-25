using Launcher.App.Constant;
using Launcher.App.Logger;
using Launcher.App.Profile;
using Launcher.App.Server;
using Launcher.App.Utility;
using Launcher.Library.Data;
using System.Diagnostics;
using System.Text;

namespace Launcher.App.Forms
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 游戏所在的目录
        /// </summary>
        public static string GameDir { get; set; } = string.Empty;

        /// <summary>
        /// KartRider.exe Path
        /// </summary>
        public static string? KartRider { get; set; }

        /// <summary>
        /// PinFile Path
        /// </summary>
        /// </summary>
        public static string? PinFile { get; set; }

        /// <summary>
        /// Backup PinFile Path
        /// </summary>
        public static string? PinFileBak { get; set; }

        /// <summary>
        /// Whether the game is selected
        /// </summary>
        public static bool GameIsReady { get; set; } = false;

        /// <summary>
        /// The PinFile Object
        /// </summary>
        public static PINFile? PinFileData { get; set; }

        public MainForm()
        {
            // Initialize Component
            InitializeComponent();

            ClientVersion.Location = new Point(label_Client.Location.X + 70, label_Client.Location.Y);
            VersionLabel.Location = new Point(Launcher_label.Location.X + 70, Launcher_label.Location.Y);

            StartPosition = FormStartPosition.Manual;
            Rectangle screen = Screen.PrimaryScreen is not null ? Screen.PrimaryScreen.WorkingArea : new Rectangle(0, 0, Width, Height);
            Location = new Point(screen.Width - Width, screen.Height - Height);

            VersionLabel.Text = Constants.VERSION;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!PreQuit())
            {
                e.Cancel = true;
                return;
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            // 加载配置文件
            new Loader().ShowDialog();

            ClientVersion.Text = (GameIsReady
                ? $"P{ProfileService.ProfileConfigs[ProfileService.SettingConfig.Name].GameOption.Version}"
                : "游戏不可用.");

            Text = $"{Text} {Constants.ARCHITECTURE} [{Constants.VERSION}, {ThisAssembly.Git.Commit}]";

            // 启动监听
            if (!RouterListener.Start())
            {
                Utils.MsgMultiInstance();
            }
        }

        private static bool PreQuit()
        {
            if (Process.GetProcessesByName("KartRider").Length != 0)
            {
                MessageBox.Show("跑跑卡丁车正在运行!\n为保证游戏文件不被损坏, 请结束跑跑卡丁车后再退出该程序!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (File.Exists(PinFileBak)) // restore PinFile
            {
                File.Delete(PinFile);
                File.Move(PinFileBak, PinFile);
            }
            ProfileService.Save(ProfileService.SettingConfig.Name);
            RouterListener.Stop();
            return true;
        }

        private async Task<bool> PreLaunch()
        {
            if (Process.GetProcessesByName("KartRider").Length != 0)
            {
                Utils.MsgKartIsRunning();
                return false;
            }
            if (!Utils.CheckGameAvailability(GameDir))
            {
                Utils.MsgFileNotFound();
                ClientVersion.Text = "游戏不可用.";
                return false;
            }
            if (RouterListener.IsRunning == false)
            {
                MessageBox.Show("路由监听服务未启动!\n请检查网络设置或重启监听后重试!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Disable Buttons
            Start_Button.Enabled = false;
            GetKart_Button.Enabled = false;

            await Task.Run(async () =>
            {
                try
                {
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
                        await new MemoryModifier().LaunchAndModifyMemory(GameDir);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error when launch: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] PreLaunch failed: {ex.Message}");
                    MessageBox.Show($"启动前置操作失败!\n错误信息: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });

            // Restore Buttons
            Start_Button.Enabled = true;
            GetKart_Button.Enabled = true;

            // Send notification

            Utils.Send_Notification("启动器提示", "跑跑卡丁车启动中...");

            return true;
        }

        private void Start_Button_Click(object sender, EventArgs e)
        {
            PreLaunch();
        }

        private void GetKart_Button_Click(object sender, EventArgs e)
        {
            Program.GetKartDlg = new();
            Program.GetKartDlg.ShowDialog();
        }

        private void VersionLabel_MouseEnter(object sender, EventArgs e)
        {
            new ToolTip().SetToolTip(VersionLabel, "点击前往GitHub Release");
        }

        private void GitHub_Release_Click(object sender, EventArgs e)
        {
            Utils.TryOpenUrl(Constants.GH_LATEST_RELEASE);
        }

        private void label_Client_Click(object sender, EventArgs e)
        {
            Utils.TryOpenUrl(Constants.KRARCHIVE_URL);
        }

        private void ClientVersion_MouseEnter(object sender, EventArgs e)
        {
            new ToolTip().SetToolTip(ClientVersion, "点击前往BrownSugar的跑跑卡丁车存档");
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

        private void Export_Log_Click(object sender, EventArgs e)
        {
            string logFileName = CachedConsoleWriter.SaveToFile();
            CachedConsoleWriter.cachedWriter.ClearCache();
            if (logFileName == string.Empty)
                MessageBox.Show("日志写入失败!", "写入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show($"日志已经写入了 {logFileName}", "写入完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Online_Settings_Click(object sender, EventArgs e)
        {
            Program.SettingDlg = new();
            Program.SettingDlg.ShowDialog();
        }

        private void More_Options_Click(object sender, EventArgs e)
        {
            Program.OptionsDlg = new();
            Program.OptionsDlg.ShowDialog();
        }

        private void About_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"跑跑卡丁车单机启动器+模拟服务器, 版本: {Constants.VERSION}\n\n" +
                $"在 {Constants.LICENSE} 许可证下提供该软件, 使用及开发时请遵守许可.\n" +
                $"所有者: {Constants.OWNER}\n" +
                $"GitHub仓库: {Constants.GH_REPO_URL}\n" +
                $"在线文档: {Constants.DOCS_URL}\n\n" +
                $"在这里反馈漏洞或请求一个新功能 {Constants.GH_ISSUE_URL}\n\n" +
                $"感谢你的使用! 以及感谢所有为此项目做出贡献的玩家们! (贡献者名单详见GitHub)\n\n" +
                $"KartRider forever!",

                "关于"
            );
        }

        private void Online_Docs_Click(object sender, EventArgs e)
        {
            Utils.TryOpenUrl(Constants.DOCS_URL);
        }

        private void KartInfo_Click(object sender, EventArgs e)
        {
            Utils.TryOpenUrl(Constants.KARTINFO_URL);
        }

        private void Github_Repo_Click(object sender, EventArgs e)
        {
            Utils.TryOpenUrl(Constants.GH_REPO_URL);
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            if (!PreQuit())
                return;
            Environment.Exit(0);
        }

        private void Launch_Game_Click(object sender, EventArgs e)
        {
            _ = PreLaunch();
        }

        private void SelectGame_Click(object sender, EventArgs e)
        {
            new GameSelector().Show();
        }

        private void ManageServer_Click(object sender, EventArgs e)
        {
            // TODO
        }
    }
}
