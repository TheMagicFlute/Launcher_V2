using Launcher.App.ExcData;
using Launcher.App.Profile;
using Launcher.App.Server;
using System.Text;

namespace Launcher.App.Forms
{
    public partial class Setting : Form
    {
        public string[] AiSpeed = ["简单", "困难", "地狱"];

        public Setting()
        {
            InitializeComponent();
            NgsOn.CheckedChanged += Change;
            PlayerName.TextChanged += Change;
            ServerIP.TextChanged += Change;
            ServerPort.TextChanged += Change;
        }

        private bool Modified = false;

        private void Change(object sender, EventArgs e)
        {
            Modify();
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
            Modify();
            if (Speed_comboBox.SelectedItem != null)
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
            Modify();
            if (AiSpeed_comboBox.SelectedItem != null)
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
            new StaticInfo("本机IP", sb.ToString()).ShowDialog();
        }
    }
}
