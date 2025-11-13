using Launcher.App.ExcData;
using Launcher.App.Profile;

namespace Launcher.App.Forms
{
    public partial class Setting : Form
    {
        public string[] AiSpeed = ["简单", "困难", "地狱"];

        public Setting()
        {
            InitializeComponent();
        }

        private bool changed = false;

        private void OnActivated(object sender, EventArgs e)
        {
            Save.Focus();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!changed)
            {
                return;
            }
            var result = MessageBox.Show("确定要舍弃更改吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
            changed = false;
            Text = "设置";
        }

        private void Speed_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            changed = true;
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
            changed = true;
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
            Console.WriteLine("已保存设置");
            MessageBox.Show("设置成功保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            changed = false;
            Dispose();
        }

        private void Changed(object sender, EventArgs e)
        {
            changed = true;
            Text = "设置*";
        }
    }
}
