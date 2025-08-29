namespace KartRider
{
    partial class Options
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
            tab_Options = new TabControl();
            tabPage_InGameOptions = new TabPage();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            tab_Options.SuspendLayout();
            SuspendLayout();
            // 
            // tab_Options
            // 
            tab_Options.Controls.Add(tabPage_InGameOptions);
            tab_Options.Controls.Add(tabPage2);
            tab_Options.Controls.Add(tabPage3);
            tab_Options.Location = new Point(12, 12);
            tab_Options.Name = "tab_Options";
            tab_Options.SelectedIndex = 0;
            tab_Options.Size = new Size(230, 237);
            tab_Options.TabIndex = 1;
            // 
            // tabPage_InGameOptions
            // 
            tabPage_InGameOptions.Location = new Point(4, 26);
            tabPage_InGameOptions.Name = "tabPage_InGameOptions";
            tabPage_InGameOptions.Padding = new Padding(3);
            tabPage_InGameOptions.Size = new Size(222, 207);
            tabPage_InGameOptions.TabIndex = 0;
            tabPage_InGameOptions.Text = "游戏内设置";
            tabPage_InGameOptions.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 26);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(222, 207);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 26);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(222, 207);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "tabPage3";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(254, 261);
            Controls.Add(tab_Options);
            Name = "Options";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Options";
            TopMost = true;
            tab_Options.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tab_Options;
        private TabPage tabPage_InGameOptions;
        private TabPage tabPage2;
        private TabPage tabPage3;
    }
}