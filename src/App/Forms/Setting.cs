using Launcher.App.ExcData;
using Launcher.App.Profile;
using Launcher.App.Server;
using Launcher.App.Utility;
using System.Net;
using System.Text;

namespace Launcher.App.Forms
{
    public partial class Setting : Form
    {
        public string[] AiSpeed = ["简单", "困难", "地狱"];

        private bool Modified = false;

        private bool IsIPv4Available = false;
        private bool IsPortAvailable = false;

        public Setting()
        {
            InitializeComponent();
            ServerIP.TextChanged += Change;
            ServerPort.TextChanged += Change;
            Speed_comboBox.SelectedIndexChanged += Change;
            AiSpeed_comboBox.SelectedIndexChanged += Change;
        }

        private void Modify()
        {
            Modified = true;
            Text = "设置*";
        }

        private void Restore()
        {
            Modified = false;
            Text = "设置";
        }

        private void Change(object sender, EventArgs e)
        {
            Modify();
        }

        private void ServerIP_TextChanged(object sender, EventArgs e)
        {
            if (Utils.IsValidIPv4(ServerIP.Text))
            {
                ServerIP.ForeColor = Color.Green;
                IsIPv4Available = true;
            }
            else
            {
                ServerIP.ForeColor = Color.Red;
                IsIPv4Available = false;
            }
        }

        private void ServerIP_LostFocus(object sender, EventArgs e)
        {
            if (IsIPv4Available)
                ServerIP.Text = IPAddress.Parse(ServerIP.Text).ToString();
        }

        private void ServerPort_TextChanged(object sender, EventArgs e)
        {
            if (Utils.IsValidPort(ServerPort.Text))
            {
                ServerPort.ForeColor = Color.Green;
                IsPortAvailable = true;
            }
            else
            {
                ServerPort.ForeColor = Color.Red;
                IsPortAvailable = false;
            }
        }

        private void OnActivated(object sender, EventArgs e)
        {
            Save.Focus();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Modified)
            {
                return;
            }
            DialogResult result = MessageBox.Show("确定要舍弃更改吗?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            PlayerName.Text = ProfileService.SettingConfig.Name;
            ServerIP.Text = ProfileService.SettingConfig.ServerIP;
            ServerPort.Text = ProfileService.SettingConfig.ServerPort.ToString();
            NgsOn.Checked = ProfileService.SettingConfig.NgsOn;
            ProfileService.SaveSettings();
            foreach (string key in SpeedType.speedNames.Keys)
            {
                Speed_comboBox.Items.Add(key);
            }
            foreach (string key in AiSpeed)
            {
                AiSpeed_comboBox.Items.Add(key);
            }
            Speed_comboBox.Text = (SpeedType.speedNames.FirstOrDefault(x => x.Value == ProfileService.SettingConfig.SpeedType).Key);
            AiSpeed_comboBox.Text = ProfileService.SettingConfig.AiSpeedType;
            Restore();
        }

        private void Speed_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Speed_comboBox.SelectedItem is not null)
            {
                string selectedSpeed = Speed_comboBox.SelectedItem.ToString();
                if (SpeedType.speedNames.ContainsKey(selectedSpeed))
                {
                    ProfileService.SettingConfig.SpeedType = SpeedType.speedNames[selectedSpeed];
                    ProfileService.SaveSettings();
                    Console.WriteLine($"速度更改为: {selectedSpeed}");
                }
                else
                {
                    Console.WriteLine("未知的/无效的速度类型");
                }
            }
        }

        private void AiSpeed_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AiSpeed_comboBox.SelectedItem is not null)
            {
                string selectedAiSpeed = AiSpeed_comboBox.SelectedItem.ToString();
                if (AiSpeed.Contains(selectedAiSpeed))
                {
                    ProfileService.SettingConfig.AiSpeedType = selectedAiSpeed;
                    ProfileService.SaveSettings();
                    Console.WriteLine($"AI速度更改为: {selectedAiSpeed}");
                }
                else
                {
                    Console.WriteLine("未知的/无效的AI速度类型");
                }
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if ((!IsIPv4Available)
             && (!IsPortAvailable))
            {
                MessageBox.Show("IPv4与端口不正确, 请修改后重试!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (!IsIPv4Available)
            {
                MessageBox.Show("IPv4不正确, 请修改后重试!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (!IsPortAvailable)
            {
                MessageBox.Show("端口不正确, 请修改后重试!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ProfileService.SettingConfig.Name = PlayerName.Text;
            ProfileService.SettingConfig.ServerIP = ServerIP.Text;
            ProfileService.SettingConfig.ServerPort = ushort.Parse(ServerPort.Text);
            ProfileService.SettingConfig.NgsOn = NgsOn.Checked;
            ProfileService.SettingConfig.SpeedType = SpeedType.speedNames[Speed_comboBox.Text];
            ProfileService.SettingConfig.AiSpeedType = AiSpeed_comboBox.Text;
            ProfileService.SaveSettings();
            Console.WriteLine("已保存设置.");
            MessageBox.Show("设置成功保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Restore();
        }

        private void Show_My_IP_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new();
            List<string> RouterIPList = LanIpGetter.GetAllLocalLanIps();
            foreach (var ip in RouterIPList)
            {
                sb.AppendLine($"{ip}:{ProfileService.SettingConfig.ServerPort}");
            }
            new StaticInfo().Show("本机IP", sb.ToString());
        }

        private void Restore_IP_Click(object sender, EventArgs e)
        {
            ServerIP.Text = "127.0.0.1";
            ServerPort.Text = "39312";
        }
    }
}
