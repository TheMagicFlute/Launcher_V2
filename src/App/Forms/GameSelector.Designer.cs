namespace Launcher.App.Forms
{
    partial class GameSelector
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
            label_Prompt_which_to_load = new Label();
            Game_Path = new ComboBox();
            label_Prompt_manual_select = new Label();
            ManualSelect = new Button();
            Confirm = new Button();
            Prompt_Status = new Label();
            Cancel = new Button();
            SuspendLayout();
            // 
            // label_Prompt_which_to_load
            // 
            label_Prompt_which_to_load.AutoSize = true;
            label_Prompt_which_to_load.Location = new Point(12, 9);
            label_Prompt_which_to_load.Name = "label_Prompt_which_to_load";
            label_Prompt_which_to_load.Size = new Size(140, 17);
            label_Prompt_which_to_load.TabIndex = 0;
            label_Prompt_which_to_load.Text = "请问想要加载哪个游戏？";
            // 
            // Game_Path
            // 
            Game_Path.FormattingEnabled = true;
            Game_Path.Location = new Point(12, 29);
            Game_Path.Name = "Game_Path";
            Game_Path.Size = new Size(480, 25);
            Game_Path.TabIndex = 1;
            Game_Path.SelectedIndexChanged += Game_Path_SelectedIndexChanged;
            // 
            // label_Prompt_manual_select
            // 
            label_Prompt_manual_select.AutoSize = true;
            label_Prompt_manual_select.Location = new Point(12, 85);
            label_Prompt_manual_select.Name = "label_Prompt_manual_select";
            label_Prompt_manual_select.Size = new Size(192, 17);
            label_Prompt_manual_select.TabIndex = 2;
            label_Prompt_manual_select.Text = "没有想要的游戏? 试试手动选择 ->";
            // 
            // ManualSelect
            // 
            ManualSelect.Location = new Point(208, 81);
            ManualSelect.Name = "ManualSelect";
            ManualSelect.Size = new Size(80, 25);
            ManualSelect.TabIndex = 3;
            ManualSelect.Text = "手动选择";
            ManualSelect.UseVisualStyleBackColor = true;
            ManualSelect.Click += ManualSelect_Click;
            // 
            // Confirm
            // 
            Confirm.Location = new Point(412, 81);
            Confirm.Name = "Confirm";
            Confirm.Size = new Size(80, 25);
            Confirm.TabIndex = 4;
            Confirm.Text = "确定并继续";
            Confirm.UseVisualStyleBackColor = true;
            Confirm.Click += Confirm_Click;
            // 
            // Prompt_Status
            // 
            Prompt_Status.AutoSize = true;
            Prompt_Status.Location = new Point(12, 57);
            Prompt_Status.Name = "Prompt_Status";
            Prompt_Status.Size = new Size(95, 17);
            Prompt_Status.TabIndex = 5;
            Prompt_Status.Text = "目前未选择游戏.";
            // 
            // Cancel
            // 
            Cancel.Location = new Point(326, 81);
            Cancel.Name = "Cancel";
            Cancel.Size = new Size(80, 25);
            Cancel.TabIndex = 6;
            Cancel.Text = "取消";
            Cancel.UseVisualStyleBackColor = true;
            Cancel.Click += Cancel_Click;
            // 
            // GameSelector
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(504, 111);
            ControlBox = false;
            Controls.Add(Cancel);
            Controls.Add(Prompt_Status);
            Controls.Add(Confirm);
            Controls.Add(ManualSelect);
            Controls.Add(label_Prompt_manual_select);
            Controls.Add(Game_Path);
            Controls.Add(label_Prompt_which_to_load);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GameSelector";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "选择游戏";
            Load += OnLoad;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_Prompt_which_to_load;
        private ComboBox Game_Path;
        private Label label_Prompt_manual_select;
        private Button ManualSelect;
        private Button Confirm;
        private Label Prompt_Status;
        private Button Cancel;
    }
}