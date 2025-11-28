using Launcher.App.Profile;
using Launcher.App.Server;
using Launcher.Library.IO;
using static Launcher.App.Program;
using static Launcher.App.Utility.Utils;

namespace Launcher.App.Forms
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
            checkBox_AutoUpdate.Checked = ProfileService.SettingConfig.AutoUpdate;
        }

        #region Launcher Options

        private void button_ToggleConsole_Click(object sender, EventArgs e)
        {
            ProfileService.SettingConfig.ConsoleVisibility = !IsWindowVisible(consoleHandle);
            ShowWindow(consoleHandle, ProfileService.SettingConfig.ConsoleVisibility ? SW_SHOW : SW_HIDE);
            ProfileService.Save(ProfileService.SettingConfig.Name);
        }

        private void button_ToggleConsole_MouseEnter(object sender, EventArgs e)
        {
            new ToolTip().SetToolTip(button_ToggleConsole, "显示/隐藏控制台");
        }

        private void button_ToggleShowPacketDetail_Click(object sender, EventArgs e)
        {
            ProfileService.SettingConfig.ShowPacketDetail = !ProfileService.SettingConfig.ShowPacketDetail;
            Console.WriteLine($"输出数据包细节: {(ProfileService.SettingConfig.ShowPacketDetail ? "开" : "关")}");
            ProfileService.Save(ProfileService.SettingConfig.Name);
        }

        private void button_ToggleShowPacketDetail_MouseEnter(object sender, EventArgs e)
        {
            new ToolTip().SetToolTip(button_ToggleShowPacketDetail, "显示/隐藏数据包的内容");
        }

        private void button_CheckUpdate_Click(object sender, EventArgs e)
        {
            new Updater().ShowDialog();
        }

        private void checkBox_AutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            ProfileService.SettingConfig.AutoUpdate = checkBox_AutoUpdate.Checked;
            ProfileService.Save(ProfileService.SettingConfig.Name);
            Console.WriteLine($"自动检查更新: {(ProfileService.SettingConfig.AutoUpdate ? "ON" : "OFF")}");
        }

        private void checkBox_AutoUpdate_MouseEnter(object sender, EventArgs e)
        {
            new ToolTip().SetToolTip(checkBox_AutoUpdate,
                "勾选此项，则会在每次启动时检查更新。\n" +
                "如果你想同步官服，推荐勾选此项；如果不想，则不建议勾选。\n" +
                "最新启动器只保证支持最新游戏客户端。");
        }

        #endregion

        #region InGame Options

        private void button_KillGameProcesses_Click(object sender, EventArgs e)
        {
            TryKillKart();
        }

        private void button_ForceLeaveRoom_Click(object sender, EventArgs e)
        {
            using (OutPacket oPacket = new OutPacket("ChLeaveRoomReplyPacket"))
            {
                oPacket.WriteByte(1);
                RouterListener.MySession.Client.Send(oPacket);
            }
        }

        #endregion
    }
}
