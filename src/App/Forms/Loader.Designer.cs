namespace Launcher.App.Forms
{
    partial class Loader
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
            PromptMsg = new Label();
            SuspendLayout();
            // 
            // PromptMsg
            // 
            PromptMsg.AutoSize = true;
            PromptMsg.Location = new Point(12, 18);
            PromptMsg.Name = "PromptMsg";
            PromptMsg.Size = new Size(136, 17);
            PromptMsg.TabIndex = 1;
            PromptMsg.Text = "Loading, please wait...";
            // 
            // Loader
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(380, 57);
            ControlBox = false;
            Controls.Add(PromptMsg);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Loader";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "加载中";
            Load += OnLoad;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label PromptMsg;
    }
}
