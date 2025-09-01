using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using KartLibrary.Consts;
using KartLibrary.Data;
using KartLibrary.File;
using KartLibrary.Xml;
using KartRider.IO.Packet;
using Microsoft.Win32;
using Profile;
using RHOParser;
using static KartRider.LauncherSystem;

namespace KartRider
{
    internal static class Program
    {
#if DEBUG
        public const bool DBG = true;
#else
        public const bool DBG = false;
#endif

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;

        public static IntPtr consoleHandle;
        public static Launcher LauncherDlg;
        public static GetKart GetKartDlg;
        public static Options OptionsDlg;
        public static string RootDirectory;

        // 当前系统架构 小写字符串 目前仅有 x64 x86 arm64
        public static string architecture = RuntimeInformation.ProcessArchitecture.ToString().ToLower();

        [STAThread]
        private static async Task Main(string[] args)
        {
            AllocConsole();
            consoleHandle = Process.GetCurrentProcess().MainWindowHandle;

            // 初始化自适应编码
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            SetAdaptiveConsoleEncoding();

            if (args != null && args.Length > 1)
            {
                ProcessPack((new ArraySegment<string>(args, 1, args.Length - 1)).ToArray());
            }

            ProfileService.Load();

            if (DBG) Console.Write("[DEBUG]");
            Console.WriteLine($"中国跑跑卡丁车单机启动器 {Update.currentVersion} for {architecture}" +
                $" - built on {File.GetLastWriteTime(Process.GetCurrentProcess().MainModule.FileName).ToString("yyyy/MM/dd HH:mm:ss K")}, #{ThisAssembly.Git.Commit}");
            Console.WriteLine("--------------------------------------------------");

            // check & create app directory
            if (!Directory.Exists(FileName.ProfileDir))
                Directory.CreateDirectory(FileName.ProfileDir);

            // delete updater
            if (File.Exists(FileName.Update_File))
                File.Delete(FileName.Update_File);
            if (Directory.Exists(FileName.Update_Folder))
                Directory.Delete(FileName.Update_Folder, true);

            // get country code
            string CountryCode = await Update.GetCountryAsync();
            if (CountryCode != "") // available country code
            {
                // change country code & write to file
                ProfileService.ProfileConfig.ServerSetting.CC = ((CountryCode)Enum.Parse(typeof(CountryCode), CountryCode));
                ProfileService.Save();
            }
            Console.WriteLine($"最后一次打开于: {ProfileService.ProfileConfig.ServerSetting.CC.ToString()}");

            // check for update
            if (await Update.UpdateDataAsync()) return;

            if (Process.GetProcessesByName("KartRider").Length != 0)
            {
                MsgKartIsRunning();
                return;
            }

            // find game directory
            if (CheckGameAvailability(FileName.AppDir))
            {
                // working directory
                RootDirectory = FileName.AppDir;
                Console.WriteLine("使用当前目录下的游戏.");
            }
            else if (CheckGameAvailability(FileName.TCGKartGamePath))
            {
                // TCGame registered directory
                RootDirectory = FileName.TCGKartGamePath;
                Console.WriteLine("使用TCGame注册的游戏目录下的游戏.");
            }
            else
            {
                // game not found
                MsgFileNotFound();
            }

            // load Data files
            try
            {
                Console.WriteLine("读取Data文件...");
                KartRhoFile.Dump(Path.GetFullPath(Path.Combine(RootDirectory, @"Data\aaa.pk")));
                KartRhoFile.packFolderManager.Reset();
                Console.WriteLine("Data文件读取完成!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"读取Data文件时出错: {ex.Message}");
            }

            // auto hide console window if not in debug mode
            if (!ProfileService.ProfileConfig.ServerSetting.ConsoleVisibility)
                ShowWindow(consoleHandle, SW_HIDE);

            // open launcher form
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LauncherDlg = new();
            Application.Run(LauncherDlg);

            return;
        }

        public static void SetAdaptiveConsoleEncoding()
        {
            try
            {
                // 1. 检测操作系统类型
                bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

                // 2. 优先尝试 UTF-8（跨平台通用）
                Encoding targetEncoding = Encoding.UTF8;

                // 3. Windows 中文环境特殊处理（部分终端默认 GBK）
                if (isWindows)
                {
                    // 获取系统默认区域语言
                    var systemCulture = CultureInfo.InstalledUICulture;
                    bool isChineseCulture = systemCulture.Name.StartsWith("zh-");

                    // 中文系统尝试使用 GBK（CP936），避免 UTF-8 在部分老终端乱码
                    if (isChineseCulture)
                    {
                        try
                        {
                            targetEncoding = Encoding.GetEncoding(936); // 936 对应 GBK
                        }
                        catch (ArgumentException)
                        {
                            // 极少数情况系统不支持 GBK，则 fallback 到 UTF-8
                            targetEncoding = Encoding.UTF8;
                        }
                    }
                }

                // 4. 应用编码设置（输出/输入保持一致）
                Console.OutputEncoding = targetEncoding;
                Console.InputEncoding = targetEncoding;

                // 5. 验证编码是否生效（可选）
                Console.WriteLine($"已适配编码: {targetEncoding.EncodingName}");
            }
            catch (Exception ex)
            {
                // 异常时使用系统默认编码作为最后保障
                Console.WriteLine($"编码设置失败，使用默认编码: {ex.Message}");
            }
        }
    }
}
