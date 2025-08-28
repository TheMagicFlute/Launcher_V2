using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace KartRider
{
	public static class LauncherSystem
	{
        #region messages

        public static void MsgKartIsRunning()
		{
			MessageBox.Show("跑跑卡丁车已经运行了!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static void MsgMultiInstance()
		{
			MessageBox.Show("已经有一个启动器在运行了, 不可以同时运行多个启动器!\n点击确认退出程序.", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
			Environment.Exit(1);
		}

		public static void MsgFileNotFound()
		{
            Console.WriteLine($"Error: 找不到 {Launcher.KartRider} 或 {Launcher.PinFile}.");
            MessageBox.Show(Launcher.KartRider + " 或 " + Launcher.PinFile + " 找不到文件!\n点击确认退出程序.", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
			Environment.Exit(1);
		}

		public static void TryKillKart()
		{
            Process[] gameProcesses = Process.GetProcessesByName("KartRider");
            if (gameProcesses.Length > 0)
            {

                if ((int)MessageBox.Show("确认要强制停止所有跑跑卡丁车游戏进程吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != 1)
                {
                    return;
                }
                foreach (Process gProcess in gameProcesses)
                {
                    try
                    {
                        gProcess.Kill();
                        gProcess.WaitForExit();
                        Console.WriteLine("成功强制关闭游戏进程!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"无法强制关闭游戏进程: {ex.Message}");
                    }
                }
                gameProcesses = Process.GetProcessesByName("KartRider");
                if (gameProcesses.Length == 0)
                {
                    MessageBox.Show("所有游戏进程已成功关闭!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("部分游戏进程无法关闭, 请尝试使用任务管理器!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                Console.WriteLine("没有找到正在运行的游戏进程!");
                MessageBox.Show("没有找到正在运行的跑跑卡丁车进程!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region utils

        public static bool TryOpenUrl(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"打开超链接时发生错误: {ex.Message}");
                return false;
            }
            return true;
        }

        #endregion
    }
}
