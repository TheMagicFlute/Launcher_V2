using Launcher.App.Profile;
using Launcher.App.Utility;
using Launcher.Library.Data;

namespace Launcher.App.Forms
{
    public partial class GameSelector : Form
    {
        private bool _Game_Is_Available = false;

        public GameSelector()
        {
            InitializeComponent();
        }

        /**
         * TODO:
         * 1.  为每个不同的游戏版本添加类似Tag的标识，
         *     以便用户识别他们选择的游戏版本。
         * 2.  添加用户自定义标签功能，允许用户为不同的游戏版本添加自定义标签，
         * 3.  提供对不同游戏目录与Tag的储存（使用JSON）。
         */

        /// <summary>
        /// 刷新游戏可用性结果
        /// </summary>
        private void RefreshResult()
        {
            if (string.IsNullOrEmpty(Game_Path.Text))
            {
                Prompt_Status.ForeColor = Color.Black;
                Prompt_Status.Text = "目前未选择游戏.";
                _Game_Is_Available = false;
            }
            if (Utils.CheckGameAvailability(Game_Path.Text))
            {
                PINFile pin = new(Path.Combine(Game_Path.Text, FileName.PinFile));
                string country = pin.Header.LocaleID.ToString();
                if (pin.Header.LocaleID == 1002) country += "(KR OFFICIAL)";
                else if (pin.Header.LocaleID == 4002) country += "(TW OFFICIAL)";
                else if (pin.Header.LocaleID == 3002) country += "(CN OFFICIAL)";
                else country += "(UNKNOWN)";
                if (pin.Header.LocaleID == 3002)
                {
                    Prompt_Status.ForeColor = Color.Green;
                    Prompt_Status.Text = $"可以使用该游戏! (版本: P{pin.Header.MinorVersion}, 国家/语言: {country})";
                    _Game_Is_Available = true;
                }
                else
                {
                    Prompt_Status.ForeColor = Color.Orange;
                    Prompt_Status.Text = $"该游戏不是国服版本 (版本: P{pin.Header.MinorVersion}, 国家/语言: {country}), 无法正常使用!";
                    _Game_Is_Available = false;
                }
            }
            else
            {
                Prompt_Status.ForeColor = Color.Red;
                Prompt_Status.Text = "该游戏缺少必要的文件, 请尝试修复或重新安装游戏后重试!";
                _Game_Is_Available = false;
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            // TCGame registered directory (优先级最后)
            if (Utils.CheckGameAvailability(FileName.TCGKartGamePath))
            {
                if (!Game_Path.Items.Contains(FileName.TCGKartGamePath))
                    Game_Path.Items.Add(FileName.TCGKartGamePath);
                Game_Path.Text = FileName.TCGKartGamePath;
            }
            // CWD (优先级其次)
            if (Utils.CheckGameAvailability(FileName.AppDir))
            {
                if (!Game_Path.Items.Contains(FileName.AppDir))
                    Game_Path.Items.Add(FileName.AppDir);
                Game_Path.Text = FileName.AppDir;
            }
            // 已经选择的游戏 (优先级最高)
            if (MainForm.GameIsReady)
            {
                if (!Game_Path.Items.Contains(MainForm.GameDir))
                    Game_Path.Items.Add(MainForm.GameDir);
                Game_Path.Text = MainForm.GameDir;
            }

            RefreshResult();
        }

        private void ManualSelect_Click(object sender, EventArgs e)
        {
            // 在单独的 STA 线程上显示 FolderBrowserDialog，避免阻塞 UI 线程或产生 STA/消息循环问题。
            var t = new Thread(() =>
            {
                try
                {
                    using (var folderBrowser = new FolderBrowserDialog
                    {
                        Description = "请选择游戏文件夹 (请注意, 仅可选择国服游戏!)",
                        RootFolder = Environment.SpecialFolder.Desktop,
                        // 可根据需要设置选中路径：SelectedPath = Game_Path.Text
                    })
                    {
                        // 在该 STA 线程内显示模态对话框（不会阻塞主 UI 线程）
                        var result = folderBrowser.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            var selected = folderBrowser.SelectedPath;
                            // 回到 UI 线程更新控件
                            try
                            {
                                this.BeginInvoke(() =>
                                {
                                    if (!Game_Path.Items.Contains(selected))
                                        Game_Path.Items.Add(selected);
                                    Game_Path.Text = selected;
                                    // 选中之后刷新结果（可能会进行耗时检查 - 由 RefreshResult 决定是否要异步化）
                                    RefreshResult();
                                });
                            }
                            catch
                            {
                                // 如果 Invoke 失败则忽略（窗体可能已关闭）
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    try { Console.WriteLine($"ManualSelect 异常: {ex.Message}"); } catch { }
                }
            })
            {
                IsBackground = true
            };
            // 必须设置为 STA
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void Game_Path_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshResult();
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            if (_Game_Is_Available)
            {
                MainForm.GameDir = Game_Path.Text;
                MessageBox.Show($"设置成功! 新游戏目录: {MainForm.GameDir}.", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Dispose();
                new Loader().ShowDialog();
            }
            else
            {
                MessageBox.Show("新游戏目录不可用, 请调整后重试!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
