namespace Launcher.App.Forms
{
    partial class StaticInfo
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
            Info_Box = new TextBox();
            Prompt_Text = new Label();
            Clear_Info = new Button();
            Copy_Info = new Button();
            SuspendLayout();
            // 
            // Info_Box
            // 
            Info_Box.Location = new Point(12, 49);
            Info_Box.Multiline = true;
            Info_Box.Name = "Info_Box";
            Info_Box.ReadOnly = true;
            Info_Box.ScrollBars = ScrollBars.Vertical;
            Info_Box.Size = new Size(510, 249);
            Info_Box.TabIndex = 0;
            // 
            // Prompt_Text
            // 
            Prompt_Text.AutoSize = true;
            Prompt_Text.Font = new Font("Microsoft YaHei UI", 20F);
            Prompt_Text.Location = new Point(12, 9);
            Prompt_Text.Name = "Prompt_Text";
            Prompt_Text.Size = new Size(254, 35);
            Prompt_Text.TabIndex = 1;
            Prompt_Text.Text = "Prompt text here...";
            // 
            // Clear_Info
            // 
            Clear_Info.Location = new Point(12, 304);
            Clear_Info.Name = "Clear_Info";
            Clear_Info.Size = new Size(250, 25);
            Clear_Info.TabIndex = 2;
            Clear_Info.Text = "清除所有信息";
            Clear_Info.UseVisualStyleBackColor = true;
            Clear_Info.Click += Clear_Info_Click;
            // 
            // Copy_Info
            // 
            Copy_Info.Location = new Point(272, 304);
            Copy_Info.Name = "Copy_Info";
            Copy_Info.Size = new Size(250, 25);
            Copy_Info.TabIndex = 3;
            Copy_Info.Text = "复制所有信息";
            Copy_Info.UseVisualStyleBackColor = true;
            Copy_Info.Click += Copy_Info_Click;
            // 
            // StaticInfo
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(534, 341);
            Controls.Add(Copy_Info);
            Controls.Add(Clear_Info);
            Controls.Add(Prompt_Text);
            Controls.Add(Info_Box);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "StaticInfo";
            ShowIcon = false;
            Text = "信息";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox Info_Box;
        private Label Prompt_Text;
        private Button Clear_Info;
        private Button Copy_Info;
    }
}