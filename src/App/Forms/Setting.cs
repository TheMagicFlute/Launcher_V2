using Launcher.App.ExcData;
using Launcher.App.Profile;

namespace Launcher.App.Forms
{
    public partial class Setting : Form
    {
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
            var result = MessageBox.Show("确定要舍弃未保存的更改吗?", "确认关闭", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
            Speed_comboBox.Text = (SpeedType.speedNames.FirstOrDefault(x => x.Value == ProfileService.SettingConfig.SpeedType).Key);
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
                    Console.WriteLine("未知的/不可用的速度类型");
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
            ProfileService.SaveSettings();
            Console.WriteLine("已保存联机设置!");
            MessageBox.Show("已保存联机设置!", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
