using System.Drawing;
using System.Windows.Forms;

namespace Launcher.KartRider.Data.Forms
{
    partial class DebugForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugForm));
            X = new Label();
            Y = new Label();
            Z = new Label();
            Track = new Label();
            Time = new Label();
            Booster_used = new Label();
            Hit = new Label();
            N2O = new Label();
            Ex_V1 = new Label();
            Ex_XUN = new Label();
            Player = new Label();
            SuspendLayout();
            // 
            // X
            // 
            X.AutoSize = true;
            X.Location = new Point(12, 23);
            X.Name = "X";
            X.Size = new Size(19, 17);
            X.TabIndex = 0;
            X.Text = "X:";
            // 
            // Y
            // 
            Y.AutoSize = true;
            Y.Location = new Point(12, 40);
            Y.Name = "Y";
            Y.Size = new Size(18, 17);
            Y.TabIndex = 1;
            Y.Text = "Y:";
            // 
            // Z
            // 
            Z.AutoSize = true;
            Z.Location = new Point(12, 57);
            Z.Name = "Z";
            Z.Size = new Size(18, 17);
            Z.TabIndex = 2;
            Z.Text = "Z:";
            // 
            // Track
            // 
            Track.AllowDrop = true;
            Track.AutoSize = true;
            Track.Location = new Point(12, 74);
            Track.Name = "Track";
            Track.Size = new Size(43, 17);
            Track.TabIndex = 3;
            Track.Text = "Track:";
            // 
            // Time
            // 
            Time.AutoSize = true;
            Time.Location = new Point(12, 91);
            Time.Name = "Time";
            Time.Size = new Size(39, 17);
            Time.TabIndex = 5;
            Time.Text = "Time:";
            // 
            // Booster_used
            // 
            Booster_used.AutoSize = true;
            Booster_used.Location = new Point(12, 108);
            Booster_used.Name = "Booster_used";
            Booster_used.Size = new Size(91, 17);
            Booster_used.TabIndex = 6;
            Booster_used.Text = "Used Booster:";
            // 
            // Hit
            // 
            Hit.AutoSize = true;
            Hit.Location = new Point(12, 125);
            Hit.Name = "Hit";
            Hit.Size = new Size(27, 17);
            Hit.TabIndex = 8;
            Hit.Text = "Hit:";
            // 
            // N2O
            // 
            N2O.AutoSize = true;
            N2O.Location = new Point(12, 142);
            N2O.Name = "N2O";
            N2O.Size = new Size(38, 17);
            N2O.TabIndex = 9;
            N2O.Text = "N2O:";
            // 
            // Ex_V1
            // 
            Ex_V1.AutoSize = true;
            Ex_V1.Location = new Point(12, 159);
            Ex_V1.Name = "Ex_V1";
            Ex_V1.Size = new Size(52, 17);
            Ex_V1.TabIndex = 10;
            Ex_V1.Text = "Exceed:";
            // 
            // Ex_XUN
            // 
            Ex_XUN.AutoSize = true;
            Ex_XUN.Location = new Point(12, 176);
            Ex_XUN.Name = "Ex_XUN";
            Ex_XUN.Size = new Size(55, 17);
            Ex_XUN.TabIndex = 11;
            Ex_XUN.Text = "XUN Ex:";
            // 
            // Player
            // 
            Player.AutoSize = true;
            Player.Location = new Point(12, 9);
            Player.Name = "Player";
            Player.Size = new Size(46, 17);
            Player.TabIndex = 12;
            Player.Text = "Player:";
            // 
            // DebugForm
            // 
            ClientSize = new Size(232, 211);
            Controls.Add(Player);
            Controls.Add(Ex_XUN);
            Controls.Add(Ex_V1);
            Controls.Add(N2O);
            Controls.Add(Hit);
            Controls.Add(Booster_used);
            Controls.Add(Time);
            Controls.Add(Track);
            Controls.Add(Z);
            Controls.Add(Y);
            Controls.Add(X);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "DebugForm";
            Text = "Debug Info";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Time;
        private Label X;
        private Label Y;
        private Label Z;
        private Label Track;
        private Label Booster_used;
        private Label Hit;
        private Label N2O;
        private Label Ex_V1;
        private Label Ex_XUN;
        private Label Player;
    }
}