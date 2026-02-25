namespace Launcher.App.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            Start_Button = new Button();
            GetKart_Button = new Button();
            label_Client = new Label();
            ClientVersion = new Label();
            VersionLabel = new Label();
            Launcher_label = new Label();
            label_TimeAttackLog = new Label();
            label_Note = new Label();
            menuStrip = new MenuStrip();
            MenuBar_File = new ToolStripMenuItem();
            Launch_Game = new ToolStripMenuItem();
            SelectGame = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            Export_Log = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            Quit = new ToolStripMenuItem();
            MenuBar_Settings = new ToolStripMenuItem();
            Online_Settings = new ToolStripMenuItem();
            More_Options = new ToolStripMenuItem();
            MenuBar_Manage = new ToolStripMenuItem();
            ManageServer = new ToolStripMenuItem();
            MenuBar_Help = new ToolStripMenuItem();
            About = new ToolStripMenuItem();
            Online_Docs = new ToolStripMenuItem();
            KartInfo = new ToolStripMenuItem();
            Github_Repo = new ToolStripMenuItem();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // Start_Button
            // 
            Start_Button.BackColor = SystemColors.Control;
            Start_Button.Location = new Point(11, 28);
            Start_Button.Name = "Start_Button";
            Start_Button.Size = new Size(200, 25);
            Start_Button.TabIndex = 364;
            Start_Button.Text = "启动游戏";
            Start_Button.UseVisualStyleBackColor = false;
            Start_Button.Click += Start_Button_Click;
            // 
            // GetKart_Button
            // 
            GetKart_Button.BackColor = SystemColors.Control;
            GetKart_Button.Location = new Point(11, 59);
            GetKart_Button.Name = "GetKart_Button";
            GetKart_Button.Size = new Size(200, 25);
            GetKart_Button.TabIndex = 365;
            GetKart_Button.Text = "添加道具";
            GetKart_Button.UseVisualStyleBackColor = false;
            GetKart_Button.Click += GetKart_Button_Click;
            // 
            // label_Client
            // 
            label_Client.AutoSize = true;
            label_Client.BackColor = SystemColors.Control;
            label_Client.Font = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_Client.ForeColor = Color.Blue;
            label_Client.Location = new Point(2, 112);
            label_Client.Name = "label_Client";
            label_Client.Size = new Size(71, 12);
            label_Client.TabIndex = 367;
            label_Client.Text = "游戏版本  :";
            // 
            // ClientVersion
            // 
            ClientVersion.AutoSize = true;
            ClientVersion.BackColor = SystemColors.Control;
            ClientVersion.Font = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ClientVersion.ForeColor = Color.Red;
            ClientVersion.Location = new Point(0, 0);
            ClientVersion.Name = "ClientVersion";
            ClientVersion.Size = new Size(0, 12);
            ClientVersion.TabIndex = 367;
            ClientVersion.Click += label_Client_Click;
            ClientVersion.MouseEnter += ClientVersion_MouseEnter;
            // 
            // VersionLabel
            // 
            VersionLabel.AutoSize = true;
            VersionLabel.BackColor = SystemColors.Control;
            VersionLabel.Font = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            VersionLabel.ForeColor = Color.Red;
            VersionLabel.Location = new Point(0, 0);
            VersionLabel.Name = "VersionLabel";
            VersionLabel.Size = new Size(0, 12);
            VersionLabel.TabIndex = 373;
            VersionLabel.Click += GitHub_Release_Click;
            VersionLabel.MouseEnter += VersionLabel_MouseEnter;
            // 
            // Launcher_label
            // 
            Launcher_label.AutoSize = true;
            Launcher_label.BackColor = SystemColors.Control;
            Launcher_label.ForeColor = Color.Blue;
            Launcher_label.Location = new Point(2, 124);
            Launcher_label.Name = "Launcher_label";
            Launcher_label.Size = new Size(71, 12);
            Launcher_label.TabIndex = 373;
            Launcher_label.Text = "启动器版本:";
            // 
            // label_TimeAttackLog
            // 
            label_TimeAttackLog.AutoSize = true;
            label_TimeAttackLog.BackColor = SystemColors.Control;
            label_TimeAttackLog.ForeColor = Color.Blue;
            label_TimeAttackLog.Location = new Point(134, 124);
            label_TimeAttackLog.Name = "label_TimeAttackLog";
            label_TimeAttackLog.Size = new Size(77, 12);
            label_TimeAttackLog.TabIndex = 375;
            label_TimeAttackLog.Text = "查看计时记录";
            label_TimeAttackLog.Click += label_TimeAttackLog_Click;
            // 
            // label_Note
            // 
            label_Note.AutoSize = true;
            label_Note.BackColor = Color.Cyan;
            label_Note.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_Note.ForeColor = Color.Red;
            label_Note.Location = new Point(35, 136);
            label_Note.Name = "label_Note";
            label_Note.Size = new Size(151, 16);
            label_Note.TabIndex = 380;
            label_Note.Text = "KartRider Forever!\r\n";
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { MenuBar_File, MenuBar_Settings, MenuBar_Manage, MenuBar_Help });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(214, 25);
            menuStrip.TabIndex = 381;
            menuStrip.Text = "menuStrip1";
            // 
            // MenuBar_File
            // 
            MenuBar_File.DropDownItems.AddRange(new ToolStripItem[] { Launch_Game, SelectGame, toolStripSeparator2, Export_Log, toolStripSeparator1, Quit });
            MenuBar_File.Name = "MenuBar_File";
            MenuBar_File.Size = new Size(44, 21);
            MenuBar_File.Text = "文件";
            // 
            // Launch_Game
            // 
            Launch_Game.Name = "Launch_Game";
            Launch_Game.Size = new Size(148, 22);
            Launch_Game.Text = "启动游戏";
            Launch_Game.Click += Launch_Game_Click;
            // 
            // SelectGame
            // 
            SelectGame.Name = "SelectGame";
            SelectGame.Size = new Size(148, 22);
            SelectGame.Text = "选择其他游戏";
            SelectGame.Click += SelectGame_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(145, 6);
            // 
            // Export_Log
            // 
            Export_Log.Name = "Export_Log";
            Export_Log.Size = new Size(148, 22);
            Export_Log.Text = "导出日志";
            Export_Log.Click += Export_Log_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(145, 6);
            // 
            // Quit
            // 
            Quit.Name = "Quit";
            Quit.Size = new Size(148, 22);
            Quit.Text = "退出";
            Quit.Click += Quit_Click;
            // 
            // MenuBar_Settings
            // 
            MenuBar_Settings.DropDownItems.AddRange(new ToolStripItem[] { Online_Settings, More_Options });
            MenuBar_Settings.Name = "MenuBar_Settings";
            MenuBar_Settings.Size = new Size(44, 21);
            MenuBar_Settings.Text = "设置";
            // 
            // Online_Settings
            // 
            Online_Settings.Name = "Online_Settings";
            Online_Settings.Size = new Size(124, 22);
            Online_Settings.Text = "联机设置";
            Online_Settings.Click += Online_Settings_Click;
            // 
            // More_Options
            // 
            More_Options.Name = "More_Options";
            More_Options.Size = new Size(124, 22);
            More_Options.Text = "更多选项";
            More_Options.Click += More_Options_Click;
            // 
            // MenuBar_Manage
            // 
            MenuBar_Manage.DropDownItems.AddRange(new ToolStripItem[] { ManageServer });
            MenuBar_Manage.Name = "MenuBar_Manage";
            MenuBar_Manage.Size = new Size(44, 21);
            MenuBar_Manage.Text = "管理";
            // 
            // ManageServer
            // 
            ManageServer.Name = "ManageServer";
            ManageServer.Size = new Size(160, 22);
            ManageServer.Text = "服务器管理选项";
            ManageServer.Click += ManageServer_Click;
            // 
            // MenuBar_Help
            // 
            MenuBar_Help.DropDownItems.AddRange(new ToolStripItem[] { About, Online_Docs, KartInfo, Github_Repo });
            MenuBar_Help.Name = "MenuBar_Help";
            MenuBar_Help.Size = new Size(44, 21);
            MenuBar_Help.Text = "帮助";
            // 
            // About
            // 
            About.Name = "About";
            About.Size = new Size(164, 22);
            About.Text = "关于";
            About.Click += About_Click;
            // 
            // Online_Docs
            // 
            Online_Docs.Name = "Online_Docs";
            Online_Docs.Size = new Size(164, 22);
            Online_Docs.Text = "线上说明文档";
            Online_Docs.Click += Online_Docs_Click;
            // 
            // KartInfo
            // 
            KartInfo.Name = "KartInfo";
            KartInfo.Size = new Size(164, 22);
            KartInfo.Text = "KartInfo论坛";
            KartInfo.Click += KartInfo_Click;
            // 
            // Github_Repo
            // 
            Github_Repo.Name = "Github_Repo";
            Github_Repo.Size = new Size(164, 22);
            Github_Repo.Text = "GitHub源码仓库";
            Github_Repo.Click += Github_Repo_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(214, 161);
            Controls.Add(label_Note);
            Controls.Add(label_TimeAttackLog);
            Controls.Add(VersionLabel);
            Controls.Add(Launcher_label);
            Controls.Add(ClientVersion);
            Controls.Add(label_Client);
            Controls.Add(GetKart_Button);
            Controls.Add(Start_Button);
            Controls.Add(menuStrip);
            Font = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "启动器";
            TopMost = true;
            FormClosing += OnFormClosing;
            Load += OnLoad;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_Client;
        private Label Launcher_label;
        private Label ClientVersion;
        private Label label_TimeAttackLog;
        private Label VersionLabel;
        private Button Start_Button;
        private Button GetKart_Button;
        private Label label_Note;
        private MenuStrip menuStrip;
        private ToolStripMenuItem MenuBar_Settings;
        private ToolStripMenuItem More_Options;
        private ToolStripMenuItem MenuBar_Help;
        private ToolStripMenuItem About;
        private ToolStripMenuItem Online_Settings;
        private ToolStripMenuItem Online_Docs;
        private ToolStripMenuItem KartInfo;
        private ToolStripMenuItem Github_Repo;
        private ToolStripMenuItem MenuBar_File;
        private ToolStripMenuItem Export_Log;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem Quit;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem Launch_Game;
        private ToolStripMenuItem MenuBar_Manage;
        private ToolStripMenuItem SelectGame;
        private ToolStripMenuItem ManageServer;
    }
}
