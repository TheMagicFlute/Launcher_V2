using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using ExcData;
using KartLibrary.File;
using KartRider;
using KartRider.Common.Data;
using KartRider.Common.Utilities;
using KartRider.IO.Packet;
using Launcher.Properties;
using RHOParser;
using Set_Data;
using static KartRider.Common.Data.PINFile;
using static KartRider.Program;
using static KartRider.Update;

namespace KartRider
{
    public class Launcher : Form
    {
        public static bool GetKart = true;
        public static short KartSN = 0;
        public string kartRiderDirectory = null;
        public const string KartRider = "KartRider.exe";
        public const string PinFile = "KartRider.pin";
        public const string PinFileBak = "KartRider-bak.pin";
        public static string executablePath;

        private Button Start_Button;
        private Button GetKart_Button;
        private Label label_Client;
        private ComboBox Speed_comboBox;
        private Label Speed_label;
        private Label GitHub;
        private Label KartInfo;
        private Label Launcher_label;
        private Label ClientVersion;
        private Label label_Docs;
        private Label label_TimeAttackLog;
        private Button button_KillGameProcesses;
        private Button button_ToggleConsole;
        private Button button_More_Options;
        private Label VersionLabel;

        public Launcher()
        {
            InitializeComponent();
            // ----------
            SetGameOption.Load_SetGameOption();
            foreach (string key in SpeedType.speedNames.Keys)
            {
                Speed_comboBox.Items.Add(key);
            }
            KeyValuePair<string, byte> speed = SpeedType.speedNames.FirstOrDefault(a => a.Value == SetGameOption.SpeedType);
            if (!String.IsNullOrEmpty(speed.Key))
            {
                Speed_comboBox.Text = speed.Key;
            }
            ClientVersion.Text = $"P{SetGameOption.Version.ToString()}";
            VersionLabel.Text = currentVersion;
            VersionLabel.Location = new Point(Launcher_label.Location.X + 70, Launcher_label.Location.Y);
            ClientVersion.Location = new Point(label_Client.Location.X + 70, label_Client.Location.Y);

            StartPosition = FormStartPosition.Manual;
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            Location = new Point(screen.Width - Width, screen.Height - Height);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Launcher));
            Start_Button = new Button();
            GetKart_Button = new Button();
            label_Client = new Label();
            ClientVersion = new Label();
            VersionLabel = new Label();
            Launcher_label = new Label();
            Speed_comboBox = new ComboBox();
            Speed_label = new Label();
            GitHub = new Label();
            KartInfo = new Label();
            label_Docs = new Label();
            label_TimeAttackLog = new Label();
            button_KillGameProcesses = new Button();
            button_ToggleConsole = new Button();
            button_More_Options = new Button();
            SuspendLayout();
            // 
            // Start_Button
            // 
            Start_Button.Location = new Point(12, 12);
            Start_Button.Name = "Start_Button";
            Start_Button.Size = new Size(200, 25);
            Start_Button.TabIndex = 364;
            Start_Button.Text = "启动游戏";
            Start_Button.UseVisualStyleBackColor = true;
            Start_Button.Click += Start_Button_Click;
            // 
            // GetKart_Button
            // 
            GetKart_Button.Location = new Point(12, 105);
            GetKart_Button.Name = "GetKart_Button";
            GetKart_Button.Size = new Size(200, 25);
            GetKart_Button.TabIndex = 365;
            GetKart_Button.Text = "添加道具";
            GetKart_Button.UseVisualStyleBackColor = true;
            GetKart_Button.Click += GetKart_Button_Click;
            // 
            // label_Client
            // 
            label_Client.AutoSize = true;
            label_Client.BackColor = SystemColors.Control;
            label_Client.Font = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_Client.ForeColor = Color.Blue;
            label_Client.Location = new Point(2, 239);
            label_Client.Name = "label_Client";
            label_Client.Size = new Size(71, 12);
            label_Client.TabIndex = 367;
            label_Client.Text = "游戏版本  :";
            label_Client.Click += label_Client_Click;
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
            VersionLabel.Click += GitHub_Click;
            // 
            // Launcher_label
            // 
            Launcher_label.AutoSize = true;
            Launcher_label.ForeColor = Color.Blue;
            Launcher_label.Location = new Point(2, 251);
            Launcher_label.Name = "Launcher_label";
            Launcher_label.Size = new Size(71, 12);
            Launcher_label.TabIndex = 373;
            Launcher_label.Text = "启动器版本:";
            Launcher_label.Click += GitHub_Click;
            // 
            // Speed_comboBox
            // 
            Speed_comboBox.ForeColor = Color.Red;
            Speed_comboBox.FormattingEnabled = true;
            Speed_comboBox.Location = new Point(13, 159);
            Speed_comboBox.Name = "Speed_comboBox";
            Speed_comboBox.Size = new Size(199, 20);
            Speed_comboBox.TabIndex = 368;
            Speed_comboBox.Text = "统合S1.5";
            Speed_comboBox.SelectedIndexChanged += Speed_comboBox_SelectedIndexChanged;
            // 
            // Speed_label
            // 
            Speed_label.AutoSize = true;
            Speed_label.Font = new Font("宋体", 9F);
            Speed_label.ForeColor = Color.Blue;
            Speed_label.Location = new Point(12, 144);
            Speed_label.Name = "Speed_label";
            Speed_label.Size = new Size(65, 12);
            Speed_label.TabIndex = 369;
            Speed_label.Text = "速度选择: ";
            // 
            // GitHub
            // 
            GitHub.AutoSize = true;
            GitHub.ForeColor = Color.Blue;
            GitHub.Location = new Point(171, 239);
            GitHub.Name = "GitHub";
            GitHub.Size = new Size(41, 12);
            GitHub.TabIndex = 371;
            GitHub.Text = "GitHub";
            GitHub.Click += GitHub_Click;
            // 
            // KartInfo
            // 
            KartInfo.AutoSize = true;
            KartInfo.ForeColor = Color.Blue;
            KartInfo.Location = new Point(159, 251);
            KartInfo.Name = "KartInfo";
            KartInfo.Size = new Size(53, 12);
            KartInfo.TabIndex = 372;
            KartInfo.Text = "KartInfo";
            KartInfo.Click += KartInfo_Click;
            // 
            // label_Docs
            // 
            label_Docs.AutoSize = true;
            label_Docs.ForeColor = Color.Blue;
            label_Docs.Location = new Point(135, 227);
            label_Docs.Name = "label_Docs";
            label_Docs.Size = new Size(77, 12);
            label_Docs.TabIndex = 374;
            label_Docs.Text = "线上说明文档";
            label_Docs.Click += label_Docs_Click;
            // 
            // label_TimeAttackLog
            // 
            label_TimeAttackLog.AutoSize = true;
            label_TimeAttackLog.ForeColor = Color.Blue;
            label_TimeAttackLog.Location = new Point(135, 215);
            label_TimeAttackLog.Name = "label_TimeAttackLog";
            label_TimeAttackLog.Size = new Size(77, 12);
            label_TimeAttackLog.TabIndex = 375;
            label_TimeAttackLog.Text = "查看计时记录";
            label_TimeAttackLog.Click += label_TimeAttackLog_Click;
            // 
            // button_KillGameProcesses
            // 
            button_KillGameProcesses.Location = new Point(12, 43);
            button_KillGameProcesses.Name = "button_KillGameProcesses";
            button_KillGameProcesses.Size = new Size(200, 25);
            button_KillGameProcesses.TabIndex = 376;
            button_KillGameProcesses.Text = "强制关闭游戏";
            button_KillGameProcesses.UseVisualStyleBackColor = true;
            button_KillGameProcesses.Click += button_KillGameProcesses_Click;
            // 
            // button_ToggleConsole
            // 
            button_ToggleConsole.Location = new Point(12, 74);
            button_ToggleConsole.Name = "button_ToggleConsole";
            button_ToggleConsole.Size = new Size(200, 25);
            button_ToggleConsole.TabIndex = 377;
            button_ToggleConsole.Text = "切换终端";
            button_ToggleConsole.UseVisualStyleBackColor = true;
            button_ToggleConsole.Click += button_ToggleConsole_Click;
            // 
            // button_More_Options
            // 
            button_More_Options.Location = new Point(12, 185);
            button_More_Options.Name = "button_More_Options";
            button_More_Options.Size = new Size(200, 25);
            button_More_Options.TabIndex = 378;
            button_More_Options.Text = "更多选项";
            button_More_Options.UseVisualStyleBackColor = true;
            button_More_Options.Click += button_More_Options_Click;
            // 
            // Launcher
            // 
            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(224, 271);
            Controls.Add(button_More_Options);
            Controls.Add(button_ToggleConsole);
            Controls.Add(button_KillGameProcesses);
            Controls.Add(label_TimeAttackLog);
            Controls.Add(label_Docs);
            Controls.Add(VersionLabel);
            Controls.Add(Launcher_label);
            Controls.Add(KartInfo);
            Controls.Add(GitHub);
            Controls.Add(Speed_comboBox);
            Controls.Add(Speed_label);
            Controls.Add(ClientVersion);
            Controls.Add(label_Client);
            Controls.Add(GetKart_Button);
            Controls.Add(Start_Button);
            Font = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Launcher";
            Text = "Launcher";
            TopMost = true;
            FormClosing += OnFormClosing;
            Load += OnLoad;
            ResumeLayout(false);
            PerformLayout();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (Process.GetProcessesByName("KartRider").Length != 0)
            {
                MessageBox.Show("跑跑卡丁车正在运行，请结束跑跑卡丁车后再退出该程序！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
                return;
            }
            if (File.Exists(this.kartRiderDirectory + PinFileBak))
            {
                File.Delete(this.kartRiderDirectory + PinFile);
                File.Move(this.kartRiderDirectory + PinFileBak, this.kartRiderDirectory + PinFile);
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            executablePath = Process.GetCurrentProcess().MainModule.FileName;
            Load_KartExcData();
            StartingLoad_ALL.StartingLoad();
            PINFile val = new PINFile(this.kartRiderDirectory + PinFile);
            SetGameOption.Version = val.Header.MinorVersion;
            SetGameOption.Save_SetGameOption();

            Console.WriteLine($"Process: {this.kartRiderDirectory + KartRider}");
            try
            {
                RouterListener.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                if (ex is System.Net.Sockets.SocketException)
                {
                    LauncherSystem.MsgMultiInstance();
                }
            }
        }

        private void Start_Button_Click(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName("KartRider").Length != 0)
            {
                LauncherSystem.MsgKartIsRunning();
            }
            if (!CheckGameAvailability(this.kartRiderDirectory))
            {
                LauncherSystem.MsgFileNotFound();
            }
            (new Thread(() =>
            {
                Console.WriteLine("Backing up old PinFile...");
                Console.WriteLine(this.kartRiderDirectory + PinFileBak);
                if (File.Exists(this.kartRiderDirectory + PinFileBak))
                {
                    File.Delete(this.kartRiderDirectory + PinFile);
                    File.Move(this.kartRiderDirectory + PinFileBak, this.kartRiderDirectory + PinFile);
                }
                File.Copy(this.kartRiderDirectory + PinFile, this.kartRiderDirectory + PinFileBak);
                PINFile val = new PINFile(this.kartRiderDirectory + PinFile);
                foreach (AuthMethod authMethod in val.AuthMethods)
                {
                    Console.WriteLine($"Changing IP Address to local... {authMethod.Name}");
                    foreach (IPEndPoint loginServer in authMethod.LoginServers)
                    {
                        Console.WriteLine($"Found {loginServer.ToString()}");
                    }
                    authMethod.LoginServers.Clear();
                    authMethod.LoginServers.Add(new PINFile.IPEndPoint
                    {
                        IP = "127.0.0.1",
                        Port = 39312
                    });
                    Console.WriteLine($"All Changed to {authMethod.LoginServers[0].ToString()} \n");
                }

                Console.WriteLine("Scanning Bml Objects in PinFile...");
                foreach (BmlObject bml in val.BmlObjects)
                {
                    for (int i = bml.SubObjects.Count - 1; i >= 0; i--)
                    {
                        Console.WriteLine($"Found {bml.SubObjects[i].Item1} in {bml.Name}");
                        if (bml.SubObjects[i].Item1 != "NgsOn") continue;
                        Console.WriteLine($"Removing {bml.SubObjects[i].Item1}");
                        bml.SubObjects.RemoveAt(i);
                    }
                }
                Console.WriteLine();

                File.WriteAllBytes(this.kartRiderDirectory + PinFile, val.GetEncryptedData());
                Launcher.GetKart = false;
                // origin passport:aHR0cHM6Ly9naXRodWIuY29tL3lhbnlnbS9MYXVuY2hlcl9WMi9yZWxlYXNlcw==
                ProcessStartInfo startInfo = new ProcessStartInfo(Launcher.KartRider, "TGC -region:3 -passport:OFFLINE")
                {
                    WorkingDirectory = this.kartRiderDirectory,
                    UseShellExecute = true,
                    Verb = "runas"
                };
                try
                {
                    Process.Start(startInfo);
                    Thread.Sleep(1000);
                    Launcher.GetKart = true;
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    // 用户取消了UAC提示或没有权限
                    // Console.WriteLine(ex.Message);
                    Console.WriteLine("用户积极取消操作, 或者没有Administer权限.");
                }
            })).Start();
        }

        private void GetKart_Button_Click(object sender, EventArgs e)
        {
            if (!GetKart) return;
            Program.GetKartDlg = new GetKart();
            Program.GetKartDlg.ShowDialog();
        }

        public void Load_KartExcData()
        {
            Console.WriteLine("正在读取配置文件...");
            string ModelMaxPath = AppDomain.CurrentDomain.BaseDirectory + @"Profile\ModelMax.xml";
            string ModelMax = Resources.ModelMax;
            if (!File.Exists(ModelMaxPath))
            {
                using (StreamWriter streamWriter = new StreamWriter(ModelMaxPath, false))
                {
                    streamWriter.Write(ModelMax);
                }
            }
            XmlFileUpdater.XmlUpdater updater = new XmlFileUpdater.XmlUpdater();
            updater.UpdateLocalXmlWithResource(ModelMaxPath, ModelMax);

            EnsureDefaultDataFileExists(AppDomain.CurrentDomain.BaseDirectory + @"Profile\AI.xml", CreateAIDefaultData);

            KartExcData.NewKart = LoadKartData(AppDomain.CurrentDomain.BaseDirectory + @"Profile\NewKart.xml", LoadNewKart);
            KartExcData.TuneList = LoadKartData(AppDomain.CurrentDomain.BaseDirectory + @"Profile\TuneData.xml", LoadTuneData);
            KartExcData.PlantList = LoadKartData(AppDomain.CurrentDomain.BaseDirectory + @"Profile\PlantData.xml", LoadPlantData);
            KartExcData.LevelList = LoadKartData(AppDomain.CurrentDomain.BaseDirectory + @"Profile\LevelData.xml", LoadLevelData);
            KartExcData.PartsList = LoadKartData(AppDomain.CurrentDomain.BaseDirectory + @"Profile\PartsData.xml", LoadPartsData);
            KartExcData.Parts12List = LoadKartData(AppDomain.CurrentDomain.BaseDirectory + @"Profile\Parts12Data.xml", LoadParts12Data);
            KartExcData.Level12List = LoadKartData(AppDomain.CurrentDomain.BaseDirectory + @"Profile\Level12Data.xml", LoadLevel12Data);
            Console.WriteLine("配置文件读取完成!");
        }

        private void EnsureDefaultDataFileExists(string filePath, Action<string> createDefaultData)
        {
            if (!File.Exists(filePath))
            {
                createDefaultData(filePath);
            }
            else
            {
                try
                {
                    XDocument doc = XDocument.Load(filePath); // 解析XML内容

                    // 处理SpeedAI和SpeedSpec
                    XElement speedAI = doc.Root.Element("SpeedAI");
                    // 如果SpeedAI不存在，则创建它
                    if (speedAI == null)
                    {
                        speedAI = new XElement("SpeedAI");
                        doc.Root.Add(speedAI);
                    }

                    // 检查是否存在SpeedSpec，不存在则添加
                    bool hasSpeedSpec = speedAI.Elements("SpeedSpec").Any();
                    if (!hasSpeedSpec)
                    {
                        XElement speedSpecElement = new XElement("SpeedSpec");
                        speedSpecElement.SetAttributeValue("a", "1");
                        speedSpecElement.SetAttributeValue("b", "2500");
                        speedSpecElement.SetAttributeValue("c", "2970");
                        speedSpecElement.SetAttributeValue("d", "1.5");
                        speedSpecElement.SetAttributeValue("e", "1000");
                        speedSpecElement.SetAttributeValue("f", "1500");
                        speedAI.Add(speedSpecElement);
                    }

                    // 处理ItemAI和ItemSpec
                    XElement itemAI = doc.Root.Element("ItemAI");
                    // 如果ItemAI不存在，则创建它
                    if (itemAI == null)
                    {
                        itemAI = new XElement("ItemAI");
                        doc.Root.Add(itemAI);
                    }

                    // 检查是否存在ItemSpec，不存在则添加
                    bool hasItemSpec = itemAI.Elements("ItemSpec").Any();
                    if (!hasItemSpec)
                    {
                        XElement itemSpecElement = new XElement("ItemSpec");
                        itemSpecElement.SetAttributeValue("a", "0.8");
                        itemSpecElement.SetAttributeValue("b", "2500");
                        itemSpecElement.SetAttributeValue("c", "2970");
                        itemSpecElement.SetAttributeValue("d", "1.5");
                        itemSpecElement.SetAttributeValue("e", "1000");
                        itemSpecElement.SetAttributeValue("f", "1500");
                        itemAI.Add(itemSpecElement);
                    }

                    doc.Save(filePath); // 保存修改后的XML内容
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"处理XML时出错: {ex.Message}");
                }
            }
        }

        private void CreateAIDefaultData(string filePath)
        {
            try
            {
                // 创建XML文档
                XDocument doc = new XDocument(
                    // XML声明
                    new XDeclaration("1.0", "utf-8", null),
                    // 根元素AI
                    new XElement("AI",
                        // SpeedAI元素及其内容
                        new XElement("SpeedAI",
                            new XElement("SpeedSpec",
                                new XAttribute("a", "1"),
                                new XAttribute("b", "2500"),
                                new XAttribute("c", "2970"),
                                new XAttribute("d", "1.5"),
                                new XAttribute("e", "1000"),
                                new XAttribute("f", "1500")
                            )
                        ),
                        // ItemAI元素及其内容
                        new XElement("ItemAI",
                            new XElement("ItemSpec",
                                new XAttribute("a", "0.8"),
                                new XAttribute("b", "2500"),
                                new XAttribute("c", "2970"),
                                new XAttribute("d", "1.5"),
                                new XAttribute("e", "1000"),
                                new XAttribute("f", "1500")
                            )
                        )
                    )
                );
                doc.Save(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"生成 XML 文件时出错: {ex.Message}");
            }
        }

        private List<List<short>> LoadKartData(string filePath, Func<XmlNodeList, List<List<short>>> parseDataFunction)
        {
            if (!File.Exists(filePath)) return new List<List<short>>();

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNodeList lis = doc.GetElementsByTagName("Kart");
            return parseDataFunction(lis);
        }

        private List<List<short>> LoadNewKart(XmlNodeList lis)
        {
            var result = new List<List<short>>();
            foreach (XmlNode xn in lis)
            {
                XmlElement xe = (XmlElement)xn;
                if (short.TryParse(xe.GetAttribute("id"), out short id) &&
                    short.TryParse(xe.GetAttribute("sn"), out short sn))
                {
                    result.Add(new List<short> { id, sn });
                }
                else
                {
                    Console.WriteLine(@"读取 Profile\NewKart.xml 文件失败, 建议删除文件后重试!");
                }
            }
            return result;
        }

        private List<List<short>> LoadTuneData(XmlNodeList lis)
        {
            var result = new List<List<short>>();
            foreach (XmlNode xn in lis)
            {
                XmlElement xe = (XmlElement)xn;
                if (short.TryParse(xe.GetAttribute("id"), out short id) &&
                    short.TryParse(xe.GetAttribute("sn"), out short sn) &&
                    short.TryParse(xe.GetAttribute("tune1"), out short tune1) &&
                    short.TryParse(xe.GetAttribute("tune2"), out short tune2) &&
                    short.TryParse(xe.GetAttribute("tune3"), out short tune3) &&
                    short.TryParse(xe.GetAttribute("slot1"), out short slot1) &&
                    short.TryParse(xe.GetAttribute("count1"), out short count1) &&
                    short.TryParse(xe.GetAttribute("slot2"), out short slot2) &&
                    short.TryParse(xe.GetAttribute("count2"), out short count2))
                {
                    result.Add(new List<short> { id, sn, tune1, tune2, tune3, slot1, count1, slot2, count2 });
                }
                else
                {
                    Console.WriteLine(@"读取 Profile\TuneData.xml 文件失败, 建议删除文件后重试!");
                }
            }
            return result;
        }

        private List<List<short>> LoadPlantData(XmlNodeList lis)
        {
            var result = new List<List<short>>();
            foreach (XmlNode xn in lis)
            {
                XmlElement xe = (XmlElement)xn;
                if (short.TryParse(xe.GetAttribute("id"), out short id) &&
                    short.TryParse(xe.GetAttribute("sn"), out short sn) &&
                    short.TryParse(xe.GetAttribute("Engine"), out short Engine) &&
                    short.TryParse(xe.GetAttribute("Engine_id"), out short Engine_id) &&
                    short.TryParse(xe.GetAttribute("Handle"), out short Handle) &&
                    short.TryParse(xe.GetAttribute("Handle_id"), out short Handle_id) &&
                    short.TryParse(xe.GetAttribute("Wheel"), out short Wheel) &&
                    short.TryParse(xe.GetAttribute("Wheel_id"), out short Wheel_id) &&
                    short.TryParse(xe.GetAttribute("Kit"), out short Kit) &&
                    short.TryParse(xe.GetAttribute("Kit_id"), out short Kit_id))
                {
                    result.Add(new List<short> { id, sn, Engine, Engine_id, Handle, Handle_id, Wheel, Wheel_id, Kit, Kit_id });
                }
                else
                {
                    Console.WriteLine(@"读取 Profile\PlantData.xml 文件失败, 建议删除文件后重试!");
                }
            }
            return result;
        }

        private List<List<short>> LoadLevelData(XmlNodeList lis)
        {
            var result = new List<List<short>>();
            foreach (XmlNode xn in lis)
            {
                XmlElement xe = (XmlElement)xn;
                if (short.TryParse(xe.GetAttribute("id"), out short id) &&
                    short.TryParse(xe.GetAttribute("sn"), out short sn) &&
                    short.TryParse(xe.GetAttribute("level"), out short level) &&
                    short.TryParse(xe.GetAttribute("point"), out short point) &&
                    short.TryParse(xe.GetAttribute("v1"), out short v1) &&
                    short.TryParse(xe.GetAttribute("v2"), out short v2) &&
                    short.TryParse(xe.GetAttribute("v3"), out short v3) &&
                    short.TryParse(xe.GetAttribute("v4"), out short v4) &&
                    short.TryParse(xe.GetAttribute("Effect"), out short Effect))
                {
                    result.Add(new List<short> { id, sn, level, point, v1, v2, v3, v4, Effect });
                }
                else
                {
                    Console.WriteLine(@"读取 Profile\LevelData.xml 文件失败, 建议删除文件后重试!");
                }
            }
            return result;
        }

        private List<List<short>> LoadPartsData(XmlNodeList lis)
        {
            var result = new List<List<short>>();
            foreach (XmlNode xn in lis)
            {
                XmlElement xe = (XmlElement)xn;
                if (short.TryParse(xe.GetAttribute("id"), out short id) &&
                    short.TryParse(xe.GetAttribute("sn"), out short sn) &&
                    short.TryParse(xe.GetAttribute("Engine"), out short Engine) &&
                    short.TryParse(xe.GetAttribute("EngineGrade"), out short EngineGrade) &&
                    short.TryParse(xe.GetAttribute("EngineValue"), out short EngineValue) &&
                    short.TryParse(xe.GetAttribute("Handle"), out short Handle) &&
                    short.TryParse(xe.GetAttribute("HandleGrade"), out short HandleGrade) &&
                    short.TryParse(xe.GetAttribute("HandleValue"), out short HandleValue) &&
                    short.TryParse(xe.GetAttribute("Wheel"), out short Wheel) &&
                    short.TryParse(xe.GetAttribute("WheelGrade"), out short WheelGrade) &&
                    short.TryParse(xe.GetAttribute("WheelValue"), out short WheelValue) &&
                    short.TryParse(xe.GetAttribute("Booster"), out short Booster) &&
                    short.TryParse(xe.GetAttribute("BoosterGrade"), out short BoosterGrade) &&
                    short.TryParse(xe.GetAttribute("BoosterValue"), out short BoosterValue) &&
                    short.TryParse(xe.GetAttribute("Coating"), out short Coating) &&
                    short.TryParse(xe.GetAttribute("TailLamp"), out short TailLamp))
                {
                    result.Add(new List<short> { id, sn, Engine, EngineGrade, EngineValue, Handle, HandleGrade, HandleValue, Wheel, WheelGrade, WheelValue, Booster, BoosterGrade, BoosterValue, Coating, TailLamp });
                }
                else
                {
                    Console.WriteLine(@"读取 Profile\PartsData.xml 文件失败, 建议删除文件后重试!");
                }
            }
            return result;
        }

        private List<List<short>> LoadParts12Data(XmlNodeList lis)
        {
            var result = new List<List<short>>();
            foreach (XmlNode xn in lis)
            {
                XmlElement xe = (XmlElement)xn;
                if (short.TryParse(xe.GetAttribute("id"), out short id) &&
                    short.TryParse(xe.GetAttribute("sn"), out short sn) &&
                    short.TryParse(xe.GetAttribute("Engine"), out short Engine) &&
                    short.TryParse(xe.GetAttribute("defaultEngine"), out short defaultEngine) &&
                    short.TryParse(xe.GetAttribute("EngineValue"), out short EngineValue) &&
                    short.TryParse(xe.GetAttribute("Handle"), out short Handle) &&
                    short.TryParse(xe.GetAttribute("defaultHandle"), out short defaultHandle) &&
                    short.TryParse(xe.GetAttribute("HandleValue"), out short HandleValue) &&
                    short.TryParse(xe.GetAttribute("Wheel"), out short Wheel) &&
                    short.TryParse(xe.GetAttribute("defaultWheel"), out short defaultWheel) &&
                    short.TryParse(xe.GetAttribute("WheelValue"), out short WheelValue) &&
                    short.TryParse(xe.GetAttribute("Booster"), out short Booster) &&
                    short.TryParse(xe.GetAttribute("defaultBooster"), out short defaultBooster) &&
                    short.TryParse(xe.GetAttribute("BoosterValue"), out short BoosterValue) &&
                    short.TryParse(xe.GetAttribute("Coating"), out short Coating) &&
                    short.TryParse(xe.GetAttribute("TailLamp"), out short TailLamp) &&
                    short.TryParse(xe.GetAttribute("BoosterEffect"), out short BoosterEffect) &&
                    short.TryParse(xe.GetAttribute("ExceedType"), out short ExceedType))
                {
                    result.Add(new List<short> { id, sn, Engine, defaultEngine, EngineValue, Handle, defaultHandle, HandleValue, Wheel, defaultWheel, WheelValue, Booster, defaultBooster, BoosterValue, Coating, TailLamp, BoosterEffect, ExceedType });
                }
                else
                {
                    Console.WriteLine(@"读取 Profile\Parts12Data.xml 文件失败, 建议删除文件后重试!");
                }
            }
            return result;
        }

        private List<List<short>> LoadLevel12Data(XmlNodeList lis)
        {
            var result = new List<List<short>>();
            foreach (XmlNode xn in lis)
            {
                XmlElement xe = (XmlElement)xn;
                if (short.TryParse(xe.GetAttribute("id"), out short id) &&
                    short.TryParse(xe.GetAttribute("sn"), out short sn) &&
                    short.TryParse(xe.GetAttribute("Level"), out short Level) &&
                    short.TryParse(xe.GetAttribute("Skill1"), out short Skill1) &&
                    short.TryParse(xe.GetAttribute("SkillLevel1"), out short SkillLevel1) &&
                    short.TryParse(xe.GetAttribute("Skill2"), out short Skill2) &&
                    short.TryParse(xe.GetAttribute("SkillLevel2"), out short SkillLevel2) &&
                    short.TryParse(xe.GetAttribute("Skill3"), out short Skill3) &&
                    short.TryParse(xe.GetAttribute("SkillLevel3"), out short SkillLevel3) &&
                    short.TryParse(xe.GetAttribute("Point"), out short Point))
                {
                    result.Add(new List<short> { id, sn, Level, Skill1, SkillLevel1, Skill2, SkillLevel2, Skill3, SkillLevel3, Point });
                }
                else
                {
                    Console.WriteLine(@"读取 Profile\Level12Data.xml 文件失败, 建议删除文件后重试!");
                }
            }
            return result;
        }

        private void Speed_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Speed_comboBox.SelectedItem != null)
            {
                string selectedSpeed = Speed_comboBox.SelectedItem.ToString();
                if (SpeedType.speedNames.ContainsKey(selectedSpeed))
                {
                    Config.SpeedType = SpeedType.speedNames[selectedSpeed];
                    Console.Write($"速度更改为: {selectedSpeed}...");
                    SetGameOption.SpeedType = Config.SpeedType;
                    SetGameOption.Save_SetGameOption();
                    Console.WriteLine("已保存.");
                }
                else
                {
                    Console.WriteLine("未知/错误的速度选项");
                }
            }
        }

        private void GitHub_Click(object sender, EventArgs e)
        {
            string url = $"https://github.com/{owner}/{repo}/releases";
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"打开超链接时发生错误: {ex.Message}");
            }
        }

        private void KartInfo_Click(object sender, EventArgs e)
        {
            string url = "https://kartinfo.me/thread-9369-1-1.html";
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"打开超链接时发生错误: {ex.Message}");
            }
        }

        private void label_Client_Click(object sender, EventArgs e)
        {
            string url = "https://github.com/brownsugar/popkart-client-archive/releases";
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"打开超链接时发生错误: {ex.Message}");
            }
        }

        private void label_Docs_Click(object sender, EventArgs e)
        {
            string url = "https://themagicflute.github.io/Launcher_V2/";
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"打开超链接时发生错误: {ex.Message}");
            }
        }

        private void label_TimeAttackLog_Click(object sender, EventArgs e)
        {
            string cmd = AppDomain.CurrentDomain.BaseDirectory + @"Profile\TimeAttack.log";
            try
            {
                Process.Start(new ProcessStartInfo(cmd) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                if (ex is System.ComponentModel.Win32Exception)
                {
                    Console.WriteLine("计时日志文件未找到, 请进行计时后再查看!");
                }
                else
                {
                    Console.WriteLine($"错误: {ex.Message}");
                }
            }
        }

        private void button_KillGameProcesses_Click(object sender, EventArgs e)
        {
            LauncherSystem.TryKillKart();
        }

        private void button_More_Options_Click(object sender, EventArgs e)
        {
            MessageBox.Show("更多选项功能尚未实现!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_ToggleConsole_Click(object sender, EventArgs e)
        {
            bool isConsoleVisible = IsWindowVisible(consoleHandle);
            isConsoleVisible = !isConsoleVisible;
            ShowWindow(consoleHandle, isConsoleVisible ? SW_SHOW : SW_HIDE);
            using (StreamWriter streamWriter = new StreamWriter(FileName.Load_ConsoleVisibility, false))
            {
                streamWriter.Write((isConsoleVisible ? "1" : "0"));
            }
        }
    }
}
