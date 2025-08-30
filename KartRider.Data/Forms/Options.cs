using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KartRider;
using KartRider.IO.Packet;
using Profile;
using static KartRider.LauncherSystem;
using static KartRider.Program;
using static KartRider.Update;

namespace KartRider
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }

        #region Launcher Options

        private void button_ToggleConsole_Click(object sender, EventArgs e)
        {
            ProfileService.ProfileConfig.ServerSetting.ConsoleVisibility = !IsWindowVisible(consoleHandle);
            ShowWindow(consoleHandle, ProfileService.ProfileConfig.ServerSetting.ConsoleVisibility ? SW_SHOW : SW_HIDE);
            ProfileService.Save();
        }

        private void button_ToggleConsole_MouseEnter(object sender, EventArgs e)
        {
            new ToolTip().SetToolTip(button_ToggleConsole, "显示/隐藏控制台");
        }

        private void button_ToggleShowPacketDetail_Click(object sender, EventArgs e)
        {
            ProfileService.ProfileConfig.ServerSetting.ShowPacketDetail = !ProfileService.ProfileConfig.ServerSetting.ShowPacketDetail;
            Console.WriteLine($"输出数据包细节: {(ProfileService.ProfileConfig.ServerSetting.ShowPacketDetail ? "开" : "关")}");
            ProfileService.Save();
        }

        private void button_ToggleShowPacketDetail_MouseEnter(object sender, EventArgs e)
        {
            new ToolTip().SetToolTip(button_ToggleShowPacketDetail, "显示/隐藏数据包的内容");
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
