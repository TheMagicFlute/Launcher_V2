using System.Windows.Forms;
using Launcher.Properties;

namespace Launcher.App.Forms
{
    partial class Setting
    {
        private void InitializeComponent()
        {
            Speed_comboBox = new ComboBox();
            Speed_label = new Label();
            PlayerName = new TextBox();
            Name_label = new Label();
            ServerIP = new TextBox();
            IP_label = new Label();
            ServerPort = new TextBox();
            Port_label = new Label();
            NgsOn = new CheckBox();
            Save = new Button();
            SuspendLayout();
            // 
            // Speed_comboBox
            // 
            Speed_comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            Speed_comboBox.ForeColor = Color.Red;
            Speed_comboBox.FormattingEnabled = true;
            Speed_comboBox.Location = new Point(54, 107);
            Speed_comboBox.Name = "Speed_comboBox";
            Speed_comboBox.Size = new Size(114, 20);
            Speed_comboBox.TabIndex = 4;
            Speed_comboBox.SelectedIndexChanged += Speed_comboBox_SelectedIndexChanged;
            // 
            // Speed_label
            // 
            Speed_label.AutoSize = true;
            Speed_label.ForeColor = Color.Blue;
            Speed_label.Location = new Point(19, 111);
            Speed_label.Name = "Speed_label";
            Speed_label.Size = new Size(35, 12);
            Speed_label.TabIndex = 5;
            Speed_label.Text = "速度:";
            // 
            // PlayerName
            // 
            PlayerName.Location = new Point(54, 20);
            PlayerName.Name = "PlayerName";
            PlayerName.Size = new Size(114, 21);
            PlayerName.TabIndex = 1;
            PlayerName.TextChanged += Changed;
            // 
            // Name_label
            // 
            Name_label.AutoSize = true;
            Name_label.ForeColor = Color.Blue;
            Name_label.Location = new Point(19, 24);
            Name_label.Name = "Name_label";
            Name_label.Size = new Size(35, 12);
            Name_label.TabIndex = 2;
            Name_label.Text = "昵称:";
            // 
            // ServerIP
            // 
            ServerIP.Location = new Point(54, 49);
            ServerIP.Name = "ServerIP";
            ServerIP.Size = new Size(114, 21);
            ServerIP.TabIndex = 2;
            ServerIP.Text = "127.0.0.1";
            ServerIP.TextChanged += Changed;
            // 
            // IP_label
            // 
            IP_label.AutoSize = true;
            IP_label.ForeColor = Color.Blue;
            IP_label.Location = new Point(19, 53);
            IP_label.Name = "IP_label";
            IP_label.Size = new Size(23, 12);
            IP_label.TabIndex = 3;
            IP_label.Text = "IP:";
            // 
            // ServerPort
            // 
            ServerPort.Location = new Point(54, 78);
            ServerPort.Name = "ServerPort";
            ServerPort.Size = new Size(114, 21);
            ServerPort.TabIndex = 3;
            ServerPort.Text = "39312";
            ServerPort.TextChanged += Changed;
            // 
            // Port_label
            // 
            Port_label.AutoSize = true;
            Port_label.ForeColor = Color.Blue;
            Port_label.Location = new Point(19, 82);
            Port_label.Name = "Port_label";
            Port_label.Size = new Size(35, 12);
            Port_label.TabIndex = 4;
            Port_label.Text = "端口:";
            // 
            // NgsOn
            // 
            NgsOn.AutoSize = true;
            NgsOn.ForeColor = Color.Blue;
            NgsOn.Location = new Point(180, 78);
            NgsOn.Name = "NgsOn";
            NgsOn.Size = new Size(54, 16);
            NgsOn.TabIndex = 5;
            NgsOn.Text = "NgsOn";
            NgsOn.UseVisualStyleBackColor = true;
            NgsOn.CheckedChanged += Changed;
            // 
            // Save
            // 
            Save.Location = new Point(180, 107);
            Save.Name = "Save";
            Save.Size = new Size(75, 23);
            Save.TabIndex = 6;
            Save.Text = "保存";
            Save.UseVisualStyleBackColor = true;
            Save.Click += Save_Click;
            // 
            // Setting
            // 
            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(273, 150);
            Controls.Add(PlayerName);
            Controls.Add(Name_label);
            Controls.Add(ServerIP);
            Controls.Add(IP_label);
            Controls.Add(ServerPort);
            Controls.Add(Port_label);
            Controls.Add(Speed_comboBox);
            Controls.Add(Speed_label);
            Controls.Add(NgsOn);
            Controls.Add(Save);
            Font = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Setting";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "设置";
            TopMost = true;
            Activated += OnActivated;
            FormClosing += OnFormClosing;
            Load += OnLoad;
            ResumeLayout(false);
            PerformLayout();
        }

        private TextBox PlayerName;
        private TextBox ServerIP;
        private TextBox ServerPort;
        private ComboBox Speed_comboBox;
        private CheckBox NgsOn;
        private Button Save;
        private Label Name_label;
        private Label IP_label;
        private Label Port_label;
        private Label Speed_label;
    }
}
