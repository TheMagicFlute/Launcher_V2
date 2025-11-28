using Launcher.App.Utility;

namespace Launcher.App.Forms
{
    public partial class StaticInfo : Form
    {
        public StaticInfo()
        {
            InitializeComponent();
        }

        public StaticInfo(string prompt, string text = "")
            : this()
        {
            Text = prompt;
            Prompt_Text.Text = prompt;
            Info_Box.Text = text;
        }

        private void Clear_Info_Click(object sender, EventArgs e)
        {
            Info_Box.Text = string.Empty;
        }

        private void Copy_Info_Click(object sender, EventArgs e)
        {
            Utils.CopyToClipboard(Info_Box.Text);
        }
    }
}
