using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using KartLibrary.Consts;
using KartLibrary.Data;
using KartLibrary.File;
using KartLibrary.Xml;
using KartRider.IO.Packet;
using Microsoft.Win32;
using RHOParser;
using Set_Data;

namespace KartRider
{
    internal static class Program
    {
#if DEBUG
        public const bool DBG = true;
#elif RELEASE
        public const bool DBG = false;
#endif

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;

        public static int consoleStatus = SW_SHOW;
        public static IntPtr consoleHandle;
        public static Launcher LauncherDlg;
        public static GetKart GetKartDlg;
        public static bool SpeedPatch;
        public static bool PreventItem;
        public static string RootDirectory;
        public static CountryCode CC = CountryCode.CN;

        // 当前系统架构 小写字符串 目前仅有 x64 x86 arm64
        public static string architecture = RuntimeInformation.ProcessArchitecture.ToString().ToLower();

        [STAThread]
        private static async Task Main(string[] args)
        {
            string input = "";
            string output = "";

            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            AllocConsole();
            consoleHandle = Process.GetCurrentProcess().MainWindowHandle;

            Console.Write($"中国跑跑卡丁车单机启动器 | {architecture} | ");
            if (DBG) Console.Write("[DEBUG]");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");

            // delete updater
            string Update_File = AppDomain.CurrentDomain.BaseDirectory + "Update.bat";
            string Update_Folder = AppDomain.CurrentDomain.BaseDirectory + "Update";
            if (File.Exists(Update_File))
            {
                File.Delete(Update_File);
            }
            if (Directory.Exists(Update_Folder))
            {
                Directory.Delete(Update_Folder, true);
            }
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Profile"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Profile");
            }

            // get country code
            string CountryCode = await Update.GetCountryAsync();
            if (CountryCode != "") // available country code
            {
                // change country code & write to file
                CC = ((CountryCode)Enum.Parse(typeof(CountryCode), CountryCode));
                using (StreamWriter streamWriter = new StreamWriter(FileName.Load_CC + FileName.Extension, false))
                {
                    streamWriter.Write(CC.ToString());
                }
            }
            else if (!File.Exists(FileName.Load_CC + FileName.Extension)) // no country code file, create default
            {
                // default country code is CN (China)
                using (StreamWriter streamWriter = new StreamWriter(FileName.Load_CC + FileName.Extension, false))
                {
                    streamWriter.Write(CC.ToString());
                }
            }

            if (File.Exists(FileName.Load_CC + FileName.Extension)) // load country code from file
            {
                string textValue = System.IO.File.ReadAllText(FileName.Load_CC + FileName.Extension);
                CC = (CountryCode)Enum.Parse(typeof(CountryCode), textValue);
            }
            Console.WriteLine($"最后一次打开于: {CC.ToString()}");

            // check for update
            if (await Update.UpdateDataAsync()) return;

            if (args == null || args.Length == 0)
            {
                string regPth = @"HKEY_CURRENT_USER\SOFTWARE\TCGame\kart";
                RootDirectory = (string)Registry.GetValue(regPth, "gamepath", null);
                if (CheckGameAvailability(AppDomain.CurrentDomain.BaseDirectory))
                {
                    // working directory
                    RootDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    Console.WriteLine("使用当前目录下的游戏.");
                }
                else if (CheckGameAvailability(RootDirectory))
                {
                    // TCGame registered directory
                    Console.WriteLine("使用TCGame注册的游戏目录下的游戏.");
                }
                else
                {
                    // game not found
                    MsgErrorFileNotFound();
                    return;
                }
                if (string.IsNullOrEmpty(RootDirectory))
                {
                    Console.WriteLine("Error: 游戏目录为空目录!"); return;
                }

                // load Data files
                try
                {
                    Console.WriteLine("读取Data文件...");
                    KartRhoFile.Dump(RootDirectory + @"Data\aaa.pk");
                    KartRhoFile.packFolderManager.Reset();
                    Console.WriteLine("Data文件读取完成!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"读取Data文件时出错: {ex.Message}");
                }

                // auto hide console window if not in debug mode
                if (!DBG) ShowWindow(consoleHandle, SW_HIDE);

                // open launcher form
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                LauncherDlg = new Launcher();
                LauncherDlg.kartRiderDirectory = RootDirectory;
                Application.Run(LauncherDlg);
                
            }
            else if (args.Length == 1)
            {
                input = args[0];
                output = args[0];
            }
            else
            {
                if (args.Length != 2)
                    return;
                input = args[0];
                output = args[1];
            }

            if (input.EndsWith(".rho") || input.EndsWith(".rho5"))
            {
                Program.decode(input, output);
            }
            else if (input.EndsWith("aaa.xml"))
            {
                AAAD(input);
            }
            else if (input.EndsWith(".xml"))
            {
                XtoB(input);
            }
            else if (input.EndsWith(".bml"))
            {
                BtoX(input);
            }
            else if (input.EndsWith(".pk"))
            {
                AAAR(input);
            }
            else
            {
                if (!Directory.Exists(input))
                    return;
                if (input.Contains("_0"))
                {
                    encode(input, output);
                }
                else
                {
                    string[] files = Directory.GetFiles(input, "*.rho");
                    if (files.Length > 0)
                    {
                        AAAC(input, files);
                    }
                    else
                    {
                        encodea(input, output);
                    }
                }
            }
        }

        public static bool CheckGameAvailability(string gamePath)
        {
            return File.Exists(gamePath + Launcher.KartRider) && File.Exists(gamePath + Launcher.PinFile);
        }

        public static void MsgErrorFileNotFound()
        {
            Console.WriteLine("Error: 未找到游戏!");
            MessageBox.Show($"找不到 {Launcher.KartRider} 或 {Launcher.PinFile} !\n请检查游戏是否正确安装.\n如使用特定版本游戏, 请检查启动器处在的位置是否与该版本安装在同一目录下. ", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(1);
        }

        private static void encodea(string input, string output)
        {
            RhoArchive rhoArchive = new RhoArchive();
            if (!output.EndsWith(".rho"))
                output += ".rho";

            Program.SaveFolder(input, output);
        }

        private static void SaveFolder(string intput, string output)
        {
            RhoArchive rhoArchive = new RhoArchive();
            string lastFolderName = Path.GetFileName(intput);
            string array = lastFolderName.Replace('_', '\\'); ;
            GetAllFiles(intput + "\\" + array, new List<string>(), rhoArchive.RootFolder);

            rhoArchive.SaveTo(output);
        }

        private static void GetAllFiles(string folderPath, List<string> fileList, RhoFolder folder)
        {
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                string extension = Path.GetExtension(file);
                RhoFile item = new RhoFile();
                item.DataSource = new FileDataSource(file);
                item.Name = Path.GetFileName(file);
                if (extension == ".bml" || extension == ".bmh" || extension == ".bmx" || extension == ".kap" || extension == ".ksv" || extension == ".1s" || extension == ".dds")
                {
                    item.FileEncryptionProperty = RhoFileProperty.Compressed;
                }
                else if (extension == ".xml")
                {
                    item.FileEncryptionProperty = RhoFileProperty.Encrypted;
                }
                else if (extension == ".kml")
                {
                    item.FileEncryptionProperty = RhoFileProperty.None;
                }
                else
                {
                    item.FileEncryptionProperty = RhoFileProperty.PartialEncrypted;
                }
                folder.AddFile(item);
            }
            string[] subdirectories = Directory.GetDirectories(folderPath);
            foreach (string subdirectory in subdirectories)
            {
                RhoFolder folder2 = new RhoFolder
                {
                    Name = Path.GetFileName(subdirectory)
                };
                folder.AddFolder(folder2);
                GetAllFiles(subdirectory, fileList, folder2);
            }
        }

        private static void encode(string input, string output)
        {
            Rho5Archive rho5Archive = new Rho5Archive();
            if (!output.EndsWith(".rho5"))
                output += ".rho5";
            var fileInfo = new FileInfo(output);
            string fullName = "";
            if (fileInfo.Directory != null)
            {
                fullName = fileInfo.Directory.FullName;
                if (!Directory.Exists(fullName))
                    Directory.CreateDirectory(fullName);
                string[] strArray = fileInfo.Name.Replace(".rho5", "").Split("_", StringSplitOptions.None);
                string dataPackName = strArray[0];
                int dataPackID = 0;
                if (strArray.Length == 2)
                    dataPackID = Convert.ToInt32(strArray[1]);
                input = input.Replace("\\", "/");
                if (!input.EndsWith("/"))
                    input += "/";
                rho5Archive.SaveFolder(input, dataPackName, fullName, CC, dataPackID);
            }
            else
            {
                Console.WriteLine($"路径不存在：{output}");
            }
        }

        private static void decode(string input, string output)
        {
            if (output.EndsWith(".rho"))
                output = output.Replace(".rho", "");
            if (output.EndsWith(".rho5"))
                output = output.Replace(".rho5", "");
            PackFolderManager packFolderManager = new PackFolderManager();
            packFolderManager.OpenSingleFile(input, CC);
            Queue<PackFolderInfo> packFolderInfoQueue = new Queue<PackFolderInfo>();
            packFolderInfoQueue.Enqueue(packFolderManager.GetRootFolder());
            packFolderManager.GetRootFolder();
            while (packFolderInfoQueue.Count > 0)
            {
                PackFolderInfo packFolderInfos = packFolderInfoQueue.Dequeue();
                foreach (PackFolderInfo packFolderInfo in packFolderInfos.GetFoldersInfo())
                {
                    string fileName = Path.GetFileNameWithoutExtension(packFolderInfo.FolderName);
                    RhoFolders(output, output + "/" + fileName, packFolderInfo);
                }
            }
        }

        private static void RhoFolders(string input, string output, PackFolderInfo rhoFolders)
        {
            if (rhoFolders.GetFilesInfo() != null)
            {
                foreach (var item in rhoFolders.GetFilesInfo())
                {
                    string fullName = input + "/" + ReplacePath(item.FullName);
                    string Name = Path.GetDirectoryName(fullName);
                    if (!Directory.Exists(Name))
                        Directory.CreateDirectory(Name);
                    byte[] data = item.GetData();
                    using (FileStream fileStream = new FileStream(fullName, FileMode.OpenOrCreate))
                    {
                        fileStream.Write(data, 0, data.Length);
                    }
                }
            }
            if (rhoFolders.Folders != null)
            {
                foreach (var rhoFolder in rhoFolders.Folders)
                {
                    string Folder = output + "/" + rhoFolder.FolderName;
                    if (!Directory.Exists(Folder))
                        Directory.CreateDirectory(Folder);
                    RhoFolders(input, Folder, rhoFolder);
                }
            }
        }

        private static string ReplacePath(string file)
        {
            return file.IndexOf(".rho") > -1 ? file.Substring(0, file.IndexOf(".rho")).Replace("_", "/") + file.Substring(file.IndexOf(".rho") + 4) : file;
        }

        private static void BtoX(string input)
        {
            byte[] data = File.ReadAllBytes(input);
            BinaryXmlDocument bxd = new BinaryXmlDocument();
            bxd.Read(Encoding.GetEncoding("UTF-16"), data);
            string output_bml = bxd.RootTag.ToString();
            byte[] output_data = Encoding.GetEncoding("UTF-16").GetBytes(output_bml);
            string filePath = System.IO.Path.ChangeExtension(input, "xml");
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(output_data, 0, output_data.Length);
            }
        }

        private static void XtoB(string input)
        {
            XDocument xdoc = XDocument.Load(input);
            if (xdoc.Root == null)
                return;
            List<int> childCounts = CountChildren(xdoc.Root, 0, new List<int>());
            using (XmlReader reader = XmlReader.Create(input))
            {
                using (OutPacket outPacket = new OutPacket())
                {
                    int Count = 0;
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            string elementName = reader.Name;
                            int attCount = reader.AttributeCount;
                            outPacket.WriteString(elementName);
                            outPacket.WriteInt(0);
                            outPacket.WriteInt(attCount);
                            for (int i = 0; i < attCount; i++)
                            {
                                reader.MoveToAttribute(i);
                                string attName = reader.Name;
                                outPacket.WriteString(attName);
                                string attValue = reader.Value;
                                outPacket.WriteString(attValue);
                            }
                            outPacket.WriteInt(childCounts[Count]);
                            Count++;
                            reader.MoveToElement();
                        }
                    }
                    byte[] byteArray = outPacket.ToArray();
                    string filePath = System.IO.Path.ChangeExtension(input, "bml");
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(byteArray, 0, byteArray.Length);
                    }
                }
            }
        }

        public static List<int> CountChildren(XElement element, int level, List<int> childCounts)
        {
            int childCount = element.Elements().Count();
            childCounts.Add(childCount);
            foreach (XElement child in element.Elements())
            {
                CountChildren(child, level + 1, childCounts);
            }
            return childCounts;
        }

        private static void AAAR(string input)
        {
            using FileStream fileStream = new FileStream(input, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            int totalLength = binaryReader.ReadInt32();
            byte[] array = binaryReader.ReadKRData(totalLength);
            fileStream.Close();
            BinaryXmlDocument bxd = new BinaryXmlDocument();
            bxd.Read(Encoding.GetEncoding("UTF-16"), array);
            string output_bml = bxd.RootTag.ToString();
            byte[] output_data = Encoding.GetEncoding("UTF-16").GetBytes(output_bml);
            string filePath = System.IO.Path.ChangeExtension(input, "xml");
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(output_data, 0, output_data.Length);
            }
        }

        private static void AAAD(string input)
        {
            XDocument xdoc = XDocument.Load(input);
            if (xdoc.Root == null)
                return;
            List<int> childCounts = CountChildren(xdoc.Root, 0, new List<int>());
            byte[] byteArray;
            using (XmlReader reader = XmlReader.Create(input))
            {
                using (OutPacket outPacket = new OutPacket())
                {
                    int Count = 0;
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            string elementName = reader.Name;
                            int attCount = reader.AttributeCount;
                            outPacket.WriteString(elementName);
                            outPacket.WriteInt(0);
                            outPacket.WriteInt(attCount);
                            for (int i = 0; i < attCount; i++)
                            {
                                reader.MoveToAttribute(i);
                                string attName = reader.Name;
                                outPacket.WriteString(attName);
                                string attValue = reader.Value;
                                outPacket.WriteString(attValue);
                            }
                            outPacket.WriteInt(childCounts[Count]);
                            Count++;
                            reader.MoveToElement();
                        }
                    }
                    byteArray = outPacket.ToArray();
                }
            }

            string filePath = System.IO.Path.ChangeExtension(input, "pk");
            using FileStream fileStream = new FileStream(filePath, FileMode.Create);
            {
                BinaryWriter binaryWriter = new BinaryWriter(fileStream);
                binaryWriter.Write((int)0);
                int KRDataLength = binaryWriter.WriteKRData(byteArray, false, true);
                binaryWriter.BaseStream.Seek(0, SeekOrigin.Begin);
                binaryWriter.Write(KRDataLength);
            }
        }

        private static void AAAC(string input, string[] files)
        {
            string[] whitelist =
            {
                "_I04_sn", "_I05_sn",
                "_R01_sn", "_R02_sn",
                "_I02_sn", "_I01_sn", "_I03_sn",
                "_L01_", "_L02_", "_L03_03_", "_L03_", "_L04_",
                "bazzi_", "arthur_", "bero_", "brodi_", "camilla_", "chris_", "contender_", "crowdr_",
                "CSO_", "dao_", "dizni_", "erini_", "ethi_", "Guazi_", "halloween_", "homrunDao_",
                "innerWearSonogong_", "innerWearWonwon_",
                "Jianbing_", "kephi_", "kero_", "kwanwoo_", "Lingling_", "lodumani_", "mabi_", "Mahua_",
                "marid_", "mobi_", "mos_", "narin_", "neoul_", "neo_", "nymph_", "olympos_", "panda_",
                "referee_", "ren_", "Reto_", "run_", "zombie_", "santa_", "sophi_", "taki_", "tiera_",
                "tutu_", "twoTop_", "twotop_", "uni_", "wonwon_", "zhindaru_", "zombie_",
                "flyingBook_", "flyingMechanic_", "flyingRedlight_",
                "crow_", "dragonBoat_", "GiLin_",
                "maple_", "beach_", "village_", "china_", "factory_", "ice_", "mine_", "nemo_", "world_", "forest_",
                "_I", "_R", "_S", "_F", "_P", "_K", "_D", "_jp"
            };
            string[] blacklist = { "character_" };
            string Whitelist = AppDomain.CurrentDomain.BaseDirectory + "Profile\\Whitelist.ini";
            string Blacklist = AppDomain.CurrentDomain.BaseDirectory + "Profile\\Blacklist.ini";
            if (File.Exists(Whitelist))
            {
                whitelist = File.ReadAllLines(Whitelist);
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(Whitelist))
                {
                    foreach (string white in whitelist)
                    {
                        writer.WriteLine(white);
                    }
                }
            }
            if (File.Exists(Blacklist))
            {
                blacklist = File.ReadAllLines(Blacklist);
            }
            else
            {
                using (StreamWriter blackr = new StreamWriter(Blacklist))
                {
                    foreach (string black in blacklist)
                    {
                        blackr.WriteLine(black);
                    }
                }
            }
            XElement root = new XElement("PackFolder", new XAttribute("name", "KartRider"));
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string result = fileName;
                foreach (string white in whitelist)
                {
                    result = result.Replace(white, white.Replace("_", "!"));
                }
                foreach (string black in blacklist)
                {
                    result = result.Replace(black.Replace("_", "!"), black);
                }
                string[] splitParts = result.Split('_');
                XElement currentFolder = root;
                for (int i = 0; i < splitParts.Length - 1; i++)
                {
                    string folderName = splitParts[i];
                    XElement? subFolder = currentFolder.Elements("PackFolder")
                                                     .FirstOrDefault(f => (string?)f.Attribute("name") == folderName);
                    if (subFolder == null)
                    {
                        if (folderName == "character" || folderName == "flyingPet" || folderName == "pet" || folderName == "track")
                        {
                            subFolder = new XElement("PackFolder", new XAttribute("name", folderName), new XAttribute("loadPass", "1"));
                        }
                        else
                        {
                            subFolder = new XElement("PackFolder", new XAttribute("name", folderName));
                        }
                        currentFolder.Add(subFolder);
                    }
                    currentFolder = subFolder;
                }
                Rho rho = new Rho(file);
                uint rhoKey = rho.GetFileKey();
                uint dataHash = rho.GetDataHash();
                long size = rho.baseStream.Length;
                string rhoFolderName = splitParts.Length > 0 ? Path.ChangeExtension(splitParts[splitParts.Length - 1], null) : "";
                XElement rhoFolder = new XElement("RhoFolder",
                    new XAttribute("name", rhoFolderName.Replace('!', '_')),
                    new XAttribute("fileName", fileName),
                    new XAttribute("key", rhoKey.ToString()),
                    new XAttribute("dataHash", dataHash.ToString()),
                    new XAttribute("mediaSize", size.ToString()));
                currentFolder.Add(rhoFolder);
            }

            root.Save(input + "\\aaa.xml");
        }
    }
}
