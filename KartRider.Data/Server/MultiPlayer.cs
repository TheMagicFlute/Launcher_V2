using ExcData;
using KartRider.Common.Utilities;
using KartRider.IO.Packet;
using KartRider_PacketName;
using Set_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace KartRider
{
    public static class MultiPlayer
    {
        static string RoomName;
        static byte[] RoomUnkBytes;
        static uint ArrivalTicks, EndTicks, SettleTicks;
        static int channeldata2 = 0;
        static uint track = Adler32Helper.GenerateAdler32_UNICODE("village_R01", 0); // First = 
        public static uint BootTicksPrev, BootTicksNow;
        public static uint StartTicks = 0;
        static uint FinishTime = 0;
        static string AiXmlFile = AppDomain.CurrentDomain.BaseDirectory + @"Profile\AI.xml";
        public static Dictionary<int, uint> AiTimeData = new Dictionary<int, uint>();

        [DllImport("kernel32")]
        extern static UInt32 GetTickCount();

        public static void milTime(int time)
        {
            GameType.min = time / 60000;
            int sec = time - GameType.min * 60000;
            GameType.sec = sec / 1000;
            GameType.mil = time % 1000;
        }

        public static uint GetUpTime()
        {
            return (uint)(TimeSpan.FromMilliseconds(GetTickCount()).Ticks / 10000);
        }

        static void Set_settleTrigger()
        {
            var onceTimer = new System.Timers.Timer();
            onceTimer.Interval = 10000;
            onceTimer.Elapsed += new System.Timers.ElapsedEventHandler((s, _event) => settleTrigger(s, _event));
            onceTimer.AutoReset = false;
            onceTimer.Start();
        }

        static void settleTrigger(object sender, System.Timers.ElapsedEventArgs e)
        {
            SettleTicks = EndTicks + 3100;
            using (OutPacket outPacket = new OutPacket("GameNextStagePacket"))
            {
                outPacket.WriteByte(1);
                outPacket.WriteInt();
                outPacket.WriteInt();
                RouterListener.MySession.Client.Send(outPacket);
            }
            using (OutPacket outPacket = new OutPacket("GameResultPacket"))
            {
                outPacket.WriteByte();
                outPacket.WriteInt(1);
                outPacket.WriteInt();
                if (FinishTime == 0)
                {
                    outPacket.WriteHexString("FFFFFFFF");
                }
                else
                {
                    outPacket.WriteUInt(FinishTime);
                }
                outPacket.WriteByte();
                outPacket.WriteShort(SetRiderItem.Set_Kart);
                outPacket.WriteShort();
                outPacket.WriteShort();
                outPacket.WriteShort();
                outPacket.WriteByte();
                outPacket.WriteUInt(SetRider.RP += 10000);
                outPacket.WriteInt(10000); // Earned RP
                outPacket.WriteInt(10000); // Earned Lucci
                outPacket.WriteUInt(SetRider.Lucci += 10000);
                outPacket.WriteBytes(new byte[46]);
                outPacket.WriteInt(1);
                outPacket.WriteBytes(new byte[52]);
                outPacket.WriteHexString("FF");
                outPacket.WriteHexString("00 00 00 00 00 00 00 E3 23 07 40 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A8");
                outPacket.WriteInt(1);
                outPacket.WriteBytes(new byte[38]);
                // 加载 XML 文件
                XDocument doc = XDocument.Load(AiXmlFile);
                var aiNodes = doc.Root.Elements().Where(e => e.Name.LocalName.StartsWith("Ai") && !e.Name.LocalName.Equals("AiData")).OrderBy(e => e.Name.LocalName);
                outPacket.WriteInt(aiNodes.Count()); // AI count
                foreach (var node in aiNodes)
                {
                    // 提取 Ai 后的数值部分（例如："Ai2" → "2"）
                    string nodeName = node.Name.LocalName;
                    int numberPart = int.Parse(nodeName.Substring(2)); // 从索引2开始截取（跳过"Ai"）
                    outPacket.WriteInt(numberPart);

                    if (AiTimeData.ContainsKey(numberPart) && AiTimeData[numberPart] > 0)
                    {
                        outPacket.WriteUInt(AiTimeData[numberPart]);
                    }
                    else
                    {
                        outPacket.WriteHexString("FFFFFFFF");
                    }
                    outPacket.WriteByte();

                    // 获取 kart 属性值
                    short kart = ParseShort(node.Attribute("kart"));
                    outPacket.WriteShort(kart);
                    outPacket.WriteShort();
                    outPacket.WriteShort();
                    outPacket.WriteHexString("A0 60");
                    outPacket.WriteByte();
                    outPacket.WriteInt();
                }
                outPacket.WriteBytes(new byte[34]);
                outPacket.WriteHexString("FF FF FF FF 00 00 00 00 00");
                RouterListener.MySession.Client.Send(outPacket);
            }
            using (OutPacket outPacket = new OutPacket("GameControlPacket"))
            {
                outPacket.WriteInt(4);
                outPacket.WriteByte(0);
                outPacket.WriteUInt(SettleTicks);
                RouterListener.MySession.Client.Send(outPacket);
            }
            Console.WriteLine("GameSlotPacket, Settle. Ticks = {0}", SettleTicks);
        }

        static short ParseShort(XAttribute attribute)
        {
            if (attribute == null || !short.TryParse(attribute.Value, out short result))
            {
                return 0; // 默认值或错误处理
            }
            return result;
        }

        public static void Clientsession(uint hash, InPacket iPacket)
        {
            if (hash == Adler32Helper.GenerateAdler32_ASCII("GameSlotPacket", 0))
            {
                iPacket.ReadInt();
                iPacket.ReadInt();
                iPacket.ReadInt();
                var nextpacketlenth = iPacket.ReadInt();
                var nextpackethash = iPacket.ReadUInt();
                if (nextpackethash == Adler32Helper.GenerateAdler32_ASCII("GopCourse", 0))
                {
                    iPacket.ReadBytes(nextpacketlenth - 4 - 4);
                    ArrivalTicks = iPacket.ReadUInt();
                }
                Console.WriteLine("GameSlotPacket, Arrivaled. Ticks = {0}", ArrivalTicks);
                return;
            }
            else if (hash == Adler32Helper.GenerateAdler32_ASCII("GameControlPacket"))
            {
                var state = iPacket.ReadByte();
                // start
                if (state == 0)
                {
                    BootTicksNow = GetUpTime();
                    StartTicks += (StartTicks == 0) ? (BootTicksNow + 8000) : (BootTicksNow - BootTicksPrev);
                    BootTicksPrev = BootTicksNow;
                    using (OutPacket oPacket = new OutPacket("GameAiMasterSlotNoticePacket"))
                    {
                        oPacket.WriteInt();
                        RouterListener.MySession.Client.Send(oPacket);
                    }
                    using (OutPacket oPacket = new OutPacket("GameControlPacket"))
                    {
                        oPacket.WriteInt(1);
                        oPacket.WriteByte(0);
                        oPacket.WriteUInt(StartTicks);
                        RouterListener.MySession.Client.Send(oPacket);
                    }
                    AiTimeData = new Dictionary<int, uint>();
                    FinishTime = 0;
                    Console.WriteLine("GameControlPacket, Start. Ticks = {0}", StartTicks);
                }
                // finish
                else if (state == 2)
                {
                    iPacket.ReadInt();
                    FinishTime = iPacket.ReadUInt();
                    using (OutPacket oPacket = new OutPacket("GameRaceTimePacket"))
                    {
                        oPacket.WriteInt();
                        oPacket.WriteUInt(FinishTime);
                        RouterListener.MySession.Client.Send(oPacket);
                    }
                    using (OutPacket oPacket = new OutPacket("GameControlPacket"))
                    {
                        EndTicks = ArrivalTicks + 8000;
                        oPacket.WriteInt(3);
                        oPacket.WriteByte(0);
                        oPacket.WriteUInt(EndTicks);
                    }
                    Console.Write("GameControlPacket, Finish. Finish Time = {0}", FinishTime);
                    Console.WriteLine(" , End - Start Ticks : {0}", EndTicks - StartTicks - 8000);
                    if (AiTimeData.Count == 0) Set_settleTrigger();
                }
                return;
            }
            else if (hash == Adler32Helper.GenerateAdler32_ASCII("ChGetRoomListRequestPacket"))
            {
                using (OutPacket oPacket = new OutPacket("ChGetRoomListReplyPacket"))
                {
                    oPacket.WriteInt(0);
                    oPacket.WriteInt(0);
                    RouterListener.MySession.Client.Send(oPacket);
                }
                return;
            }
            else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqChannelSwitch", 0))
            {
                Console.WriteLine("Channel Switch, avaliable = {0}", iPacket.Available);
                // Console.WriteLine(BitConverter.ToString(iPacket.ReadBytes(iPacket.Available)).Replace("-", " "));
                // iPacket.ReadInt();
                // iPacket.ReadBytes(14);
                byte[] DateTime1 = iPacket.ReadBytes(18);
                byte channel = iPacket.ReadByte();
                Console.WriteLine("Channel Switch, channel = {0}", channel);
                int channeldata1 = 0;
                channeldata1 = 1;
                channeldata2 = 4;
                // StartGameRacing.GameRacing_SpeedType = 4;
                if (channel == 72 || channel == 55)
                {
                    using (OutPacket oPacket = new OutPacket("PrChannelSwitch"))
                    {
                        oPacket.WriteInt(0);
                        // oPacket.WriteInt(channeldata1);
                        oPacket.WriteInt(4);
                        oPacket.WriteEndPoint(IPAddress.Parse("127.0.0.1"), (ushort)RouterListener.port);
                        //RouterListener.Listener.BeginAcceptSocket(new AsyncCallback(RouterListener.OnAcceptSocket), null);
                        RouterListener.MySession.Client.Send(oPacket);
                    }
                    //GameSupport.OnDisconnect();
                    StartGameData.StartTimeAttack_SpeedType = 7;
                }
                else if (channel == 25)
                {
                    using (OutPacket oPacket = new OutPacket("PrChannelSwitch"))
                    {
                        oPacket.WriteInt(0);
                        oPacket.WriteInt(4);
                        oPacket.WriteEndPoint(IPAddress.Parse("127.0.0.1"), (ushort)RouterListener.port);
                        //RouterListener.Listener.BeginAcceptSocket(new AsyncCallback(RouterListener.OnAcceptSocket), null);
                        RouterListener.MySession.Client.Send(oPacket);
                    }
                    //GameSupport.OnDisconnect();
                    StartGameData.StartTimeAttack_SpeedType = 4;
                }
                else
                {
                    using (OutPacket outPacket = new OutPacket("ChGetCurrentGpReplyPacket"))
                    {
                        outPacket.WriteInt(0);
                        outPacket.WriteInt(0);
                        outPacket.WriteInt(0);
                        outPacket.WriteInt(0);
                        outPacket.WriteInt(0);
                        outPacket.WriteByte(0);
                        RouterListener.MySession.Client.Send(outPacket);
                    }
                }
                //GameSupport.OnDisconnect();
                return;
            }
            else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqChannelMovein", 0))
            {
                using (OutPacket oPacket = new OutPacket("PrChannelMoveIn"))
                {
                    // oPacket.WriteHexString("01 3d a4 3d 49 8f 99 3d a4 3d 49 90 99");
                    oPacket.WriteByte(1);
                    oPacket.WriteEndPoint(IPAddress.Parse(RouterListener.sIP), 39311);
                    oPacket.WriteEndPoint(IPAddress.Parse(RouterListener.sIP), 39312);
                    RouterListener.MySession.Client.Send(oPacket);
                }
                return;
            }
            else if (hash == Adler32Helper.GenerateAdler32_ASCII("PqMissionAttendPacket", 0))
            {
                using (OutPacket oPacket = new OutPacket("PrMissionAttendPacket"))
                {
                    oPacket.WriteInt(3);
                    oPacket.WriteInt(0);
                    oPacket.WriteInt(15);
                    oPacket.WriteInt(0);
                    oPacket.WriteInt(-1);
                    oPacket.WriteInt(0);
                    oPacket.WriteInt(0);
                    oPacket.WriteInt(0);
                    oPacket.WriteInt(0);
                    oPacket.WriteInt(109);
                    RouterListener.MySession.Client.Send(oPacket);
                }
                return;
            }
            else if (hash == Adler32Helper.GenerateAdler32_ASCII("ChCreateRoomRequestPacket", 0))
            {
                Console.Write("Avaiable = {0}", iPacket.Available);
                RoomName = iPacket.ReadString();    // room name
                Console.WriteLine(" RoomName = {0}, len = {1}", RoomName, RoomName.Length);
                iPacket.ReadInt();
                var unk1 = iPacket.ReadByte(); // 7c
                iPacket.ReadInt();
                var Playernum = iPacket.ReadInt();
                iPacket.ReadInt();
                iPacket.ReadInt();
                RoomUnkBytes = iPacket.ReadBytes(32);
                var unk2 = iPacket.ReadBytes(29);
                byte AiSwitch = iPacket.ReadByte();
                using (OutPacket oPacket = new OutPacket("ChCreateRoomReplyPacket"))
                {
                    oPacket.WriteShort(1);
                    oPacket.WriteByte((byte)Playernum);
                    oPacket.WriteByte(unk1);
                    RouterListener.MySession.Client.Send(oPacket);
                }
                if (Playernum > 0 && AiSwitch == 6)
                {
                    // 读取 XML 文件
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(AiXmlFile);

                    // 获取根节点
                    XmlNode rootNode = xmlDoc.DocumentElement;

                    // 清空所有 Ai* 节点（保留 AiData）
                    RemoveAiNodes(rootNode);

                    // 新增 AI 节点数量
                    AddAiNodes(rootNode, Playernum - 1);

                    // 保存修改后的 XML 文件
                    xmlDoc.Save(AiXmlFile);
                }
                return;
            }
            else if (hash == Adler32Helper.GenerateAdler32_ASCII("GrFirstRequestPacket"))
            {
                GrSessionDataPacket();
                // Thread.Sleep(10);
                GrSlotDataPacket();
                return;
            }
            else if (hash == Adler32Helper.GenerateAdler32_ASCII("GrChangeTrackPacket"))
            {
                track = iPacket.ReadUInt();
                iPacket.ReadInt();
                RoomUnkBytes = iPacket.ReadBytes(32);
                Console.WriteLine("Gr Track Changed : {0}", RandomTrack.GetTrackName(track));
                return;
            }
            else if (hash == Adler32Helper.GenerateAdler32_ASCII("GrRequestSetSlotStatePacket"))
            {
                int Data = iPacket.ReadInt();
                GrSlotDataPacket();
                // GrSlotStatePacket(Data);
                GrReplySetSlotStatePacket(Data);
                return;
            }
            else if (hash == Adler32Helper.GenerateAdler32_ASCII("GrRequestClosePacket"))
            {
                using (OutPacket oPacket = new OutPacket("GrReplyClosePacket"))
                {
                    // oPacket.WriteHexString("ff 76 05 5d 01");
                    oPacket.WriteUInt(SetRider.UserNO);
                    oPacket.WriteByte(1);
                    oPacket.WriteInt(7);
                    oPacket.WriteInt(7);
                    oPacket.WriteInt(0);
                    oPacket.WriteInt(0);
                    RouterListener.MySession.Client.Send(oPacket);
                }
                return;
            }
            else if (hash == Adler32Helper.GenerateAdler32_ASCII("GrRequestStartPacket"))
            {
                using (OutPacket oPacket = new OutPacket("GrReplyStartPacket"))
                {
                    oPacket.WriteInt(0);
                    RouterListener.MySession.Client.Send(oPacket);
                }
                using (OutPacket oPacket = new OutPacket("GrCommandStartPacket"))
                {
                    StartGameData.StartTimeAttack_RandomTrackGameType = 0;
                    StartGameData.StartTimeAttack_Track = track;
                    RandomTrack.SetGameType();

                    oPacket.WriteUInt(Adler32Helper.GenerateAdler32(Encoding.ASCII.GetBytes("GrSessionDataPacket")));
                    GrSessionDataPacket(oPacket);

                    oPacket.WriteUInt(Adler32Helper.GenerateAdler32(Encoding.ASCII.GetBytes("GrSlotDataPacket")));
                    GrSlotDataPacket(oPacket);

                    // kart data
                    StartGameData.GetKartSpac(oPacket);

                    // AI data
                    XmlDocument doc = new XmlDocument();
                    doc.Load(@"Profile\AI.xml");
                    int listCount = 0;
                    XmlNodeList lis = doc.SelectNodes("//*[starts-with(name(), 'Ai') and contains(translate(name(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'ai') and not(contains(name(), 'data'))]");
                    if (lis.Count > 0)
                    {
                        listCount = lis.Count;
                    }
                    oPacket.WriteInt(listCount); // AI count
                    XmlNodeList Data = doc.GetElementsByTagName("AiData");
                    if (Data.Count > 0)
                    {
                        for (int i = 0; i < listCount; i++)
                        {
                            oPacket.WriteEncFloat(float.Parse(Data[0].Attributes[0].Value));
                            oPacket.WriteEncFloat(float.Parse(Data[0].Attributes[1].Value));
                            oPacket.WriteEncFloat(float.Parse(Data[0].Attributes[2].Value));
                            oPacket.WriteEncFloat(float.Parse(Data[0].Attributes[3].Value));
                            oPacket.WriteEncFloat(float.Parse(Data[0].Attributes[4].Value));
                            oPacket.WriteEncFloat(float.Parse(Data[0].Attributes[5].Value));
                        }
                    }
                    oPacket.WriteUInt(StartGameData.StartTimeAttack_Track); // track name hash
                    oPacket.WriteInt(10000);

                    oPacket.WriteInt();
                    oPacket.WriteUInt(Adler32Helper.GenerateAdler32(Encoding.ASCII.GetBytes("MissionInfo")));
                    oPacket.WriteHexString("00000000000000000000FFFFFFFF000000000000000000");
                    // oPacket.WriteString("[applied param]\r\ntransAccelFactor='1.8555' driftEscapeForce='4720' steerConstraint='24.95' normalBoosterTime='3860' \r\npartsBoosterLock='1' \r\n\r\n[equipped / default parts param]\r\ntransAccelFactor='1.86' driftEscapeForce='2120' steerConstraint='2.7' normalBoosterTime='860' \r\n\r\n\r\n[gamespeed param]\r\ntransAccelFactor='-0.0045' driftEscapeForce='2600' steerConstraint='22.25' normalBoosterTime='3000' \r\n\r\n\r\n[factory enchant param]\r\n");
                    RouterListener.MySession.Client.Send(oPacket);
                }
                // StartGameRacing.KartSpecLog();
                Console.WriteLine("Track : {0}", RandomTrack.GetTrackName(StartGameData.StartTimeAttack_Track));
                return;
            }
            else if (hash == Adler32Helper.GenerateAdler32_ASCII("PcReportStateInGame", 0))
            {
                return;
            }
            else if (hash == Adler32Helper.GenerateAdler32_ASCII("ChLeaveRoomRequestPacket"))
            {
                using (OutPacket oPacket = new OutPacket("ChLeaveRoomReplyPacket"))
                {
                    oPacket.WriteByte(1);
                    RouterListener.MySession.Client.Send(oPacket);
                }
                return;
            }
            else if (hash == Adler32Helper.GenerateAdler32_ASCII("GrRequestBasicAiPacket"))
            {
                int pos = iPacket.ReadInt();
                Console.WriteLine("GrRequestBasicAiPacket, position = {0}", pos);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AiXmlFile);
                XmlNode ai = xmlDoc.SelectSingleNode("//Ai" + pos.ToString());
                if (ai != null)
                {
                    using (OutPacket oPacket = new OutPacket("GrSlotDataBasicAi"))
                    {
                        oPacket.WriteInt(1);
                        oPacket.WriteByte(1);
                        oPacket.WriteInt(pos);
                        oPacket.WriteHexString("0000000000000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
                        RouterListener.MySession.Client.Send(oPacket);
                    }
                    XmlNode parentNode = ai.ParentNode;
                    if (parentNode != null)
                    {
                        parentNode.RemoveChild(ai);
                    }
                    xmlDoc.Save(AiXmlFile);
                }
                else
                {
                    using (OutPacket oPacket = new OutPacket("GrSlotDataBasicAi"))
                    {
                        oPacket.WriteInt(0);
                        oPacket.WriteByte(1);
                        oPacket.WriteInt(pos);
                        oPacket.WriteShort(1);
                        oPacket.WriteShort((short)pos);
                        oPacket.WriteShort(1508);
                        oPacket.WriteShort(0);
                        oPacket.WriteShort(0);
                        oPacket.WriteShort(0);
                        oPacket.WriteShort(0);
                        oPacket.WriteShort(0);
                        oPacket.WriteByte(0);
                        oPacket.WriteHexString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
                        RouterListener.MySession.Client.Send(oPacket);
                    }
                    XmlElement element = xmlDoc.CreateElement("Ai" + pos.ToString());
                    element.SetAttribute("character", "1");
                    element.SetAttribute("rid", pos.ToString());
                    element.SetAttribute("kart", "1508");
                    element.SetAttribute("balloon", "0");
                    element.SetAttribute("headBand", "0");
                    element.SetAttribute("goggle", "0");
                    XmlNode rootNode = xmlDoc.DocumentElement;
                    if (rootNode != null)
                    {
                        rootNode.AppendChild(element);
                    }
                    xmlDoc.Save(AiXmlFile);
                }
                using (OutPacket oPacket = new OutPacket("GrReplyBasicAiPacket"))
                {
                    oPacket.WriteByte(1);
                    oPacket.WriteHexString("2CFB6605");
                    RouterListener.MySession.Client.Send(oPacket);
                }
                return;
            }
            else if (hash == Adler32Helper.GenerateAdler32_ASCII("GameAiGoalinPacket"))
            {
                var AiNum = iPacket.ReadInt();
                var AiTime = iPacket.ReadUInt();
                using (OutPacket oPacket = new OutPacket("GameRaceTimePacket"))
                {
                    oPacket.WriteInt(AiNum);
                    oPacket.WriteUInt(AiTime);
                    RouterListener.MySession.Client.Send(oPacket);
                }
                if (AiTimeData.Count == 0 && FinishTime == 0)
                {
                    using (OutPacket oPacket = new OutPacket("GameControlPacket"))
                    {
                        EndTicks = AiTime + 8000;
                        oPacket.WriteInt(3);
                        oPacket.WriteByte(0);
                        oPacket.WriteUInt(EndTicks);
                    }
                    Console.Write("GameControlPacket, Finish. Finish Time = {0}", AiTime);
                    Console.WriteLine(" , End - Start Ticks : {0}", EndTicks - StartTicks - 8000);
                    Set_settleTrigger();
                }
                AiTimeData.Add(AiNum, AiTime);
                return;
            }
        }

        static void GrSlotDataPacket()
        {
            using (OutPacket oPacket = new OutPacket("GrSlotDataPacket"))
            {
                GrSlotDataPacket(oPacket);
                RouterListener.MySession.Client.Send(oPacket);
            }
        }

        static void GrSlotDataPacket(OutPacket outPacket)
        {
            outPacket.WriteUInt(track); //track name hash
            outPacket.WriteInt(0);
            outPacket.WriteBytes(RoomUnkBytes);
            outPacket.WriteInt(0); //RoomMaster 
            outPacket.WriteInt(2);
            outPacket.WriteInt(0); // outPacket.WriteShort(); outPacket.WriteShort(3);
            outPacket.WriteShort(0); // 797
            outPacket.WriteByte(0);
            var unk1 = 0;
            outPacket.WriteInt(unk1);
            // for (int i = 0; i < unk1; i++) outPacket.WriteByte();
            for (int i = 0; i < 4; i++) outPacket.WriteInt();

            /* ---- One/First player ---- */
            outPacket.WriteInt(2); // Player Type, 2 = RoomMaster, 3 = AutoReady, 4 = Observer, 5 = ? , 7 = AI
            outPacket.WriteUInt(SetRider.UserNO);

            outPacket.WriteEndPoint(IPAddress.Parse(RouterListener.client.Address.ToString()), (ushort)RouterListener.client.Port);
            // outPacket.WriteEndPoint(IPAddress.Parse(RouterListener.forceConnect), 39311);
            // outPacket.WriteHexString("3a 16 01 31 7d 48");
            outPacket.WriteInt();
            outPacket.WriteShort();

            outPacket.WriteString(SetRider.Nickname);
            outPacket.WriteShort(SetRider.Emblem1);
            outPacket.WriteShort(SetRider.Emblem2);
            outPacket.WriteShort(0);
            GameSupport.GetRider(outPacket);
            outPacket.WriteShort(0);
            outPacket.WriteString(SetRider.Card);
            outPacket.WriteUInt(SetRider.RP);
            outPacket.WriteByte();
            outPacket.WriteByte();
            outPacket.WriteByte();
            for (int i = 0; i < 8; i++) outPacket.WriteInt();
            outPacket.WriteInt(1500); // outPacket.WriteInt(1500);
            outPacket.WriteInt(2512); // outPacket.WriteInt(2000);
            outPacket.WriteInt(162); // outPacket.WriteInt();
            outPacket.WriteInt(2000); // outPacket.WriteInt(2000);
            outPacket.WriteInt(5); // outPacket.WriteInt(5);
            outPacket.WriteByte(255);
            outPacket.WriteByte(0);
            outPacket.WriteByte(0);
            outPacket.WriteByte(0);

            outPacket.WriteByte(3); // 3
            outPacket.WriteString("");
            outPacket.WriteInt();
            outPacket.WriteInt();
            outPacket.WriteInt();
            outPacket.WriteInt();
            outPacket.WriteByte();
            outPacket.WriteInt();

            /*---- One/First player ----*/
            /*
            outPacket.WriteInt(0);
            outPacket.WriteInt(0);
            outPacket.WriteInt(0);
            outPacket.WriteInt(0);
            outPacket.WriteInt(0);
            outPacket.WriteInt(0);
            outPacket.WriteInt(0);
            outPacket.WriteBytes(new byte[38]);
            outPacket.WriteHexString("FFFFFFFF");
            outPacket.WriteHexString("FFFFFFFF");
            outPacket.WriteHexString("FFFFFFFF");
            outPacket.WriteHexString("FFFFFFFF");
            outPacket.WriteHexString("FFFFFFFF");
            outPacket.WriteHexString("FFFFFFFF");
            outPacket.WriteHexString("FFFFFFFF");
            outPacket.WriteInt(0);
            */
            // outPacket.WriteHexString("030000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000");

            // process AI Data
            XmlDocument doc = new XmlDocument();
            outPacket.WriteShort(0);
            doc.Load(AiXmlFile);
            XmlNode[] aiData = new XmlNode[8];
            for (int i = 1; i <= 7; i++)
            {
                aiData[i] = doc.SelectSingleNode($"//Ai{i}");
                if (aiData[i] == null)
                {
                    outPacket.WriteInt(0);
                    continue;
                }
                outPacket.WriteInt(7);
                XmlElement xe = (XmlElement)aiData[i];
                outPacket.WriteShort(short.Parse(xe.GetAttribute("character")));
                outPacket.WriteShort(short.Parse(xe.GetAttribute("rid")));
                outPacket.WriteShort(short.Parse(xe.GetAttribute("kart")));
                outPacket.WriteShort(short.Parse(xe.GetAttribute("balloon")));
                outPacket.WriteShort(short.Parse(xe.GetAttribute("headBand")));
                outPacket.WriteShort(short.Parse(xe.GetAttribute("goggle")));
                outPacket.WriteByte(0);
            }
            outPacket.WriteBytes(new byte[36]);
            for (int i = 1; i <= 7; i++)
            {
                if (aiData[i] == null)
                {
                    outPacket.WriteHexString("FFFFFFFF");
                }
                else
                {
                    outPacket.WriteInt(i);
                }
            }
            outPacket.WriteInt(0);
        }

        static void GrSlotStatePacket(int Data)
        {
            using (OutPacket oPacket = new OutPacket("GrSlotStatePacket"))
            {
                oPacket.WriteInt(Data);
                oPacket.WriteBytes(new byte[60]);
                RouterListener.MySession.Client.Send(oPacket);
            }
        }

        static void GrReplySetSlotStatePacket(int Data)
        {
            using (OutPacket oPacket = new OutPacket("GrReplySetSlotStatePacket"))
            {
                oPacket.WriteUInt(SetRider.UserNO);
                oPacket.WriteInt(1);
                oPacket.WriteByte(0);
                oPacket.WriteInt(Data);
                RouterListener.MySession.Client.Send(oPacket);
            }
        }

        static void GrSessionDataPacket()
        {
            using (OutPacket oPacket = new OutPacket("GrSessionDataPacket"))
            {
                GrSessionDataPacket(oPacket);
                RouterListener.MySession.Client.Send(oPacket);
            }
        }

        static void GrSessionDataPacket(OutPacket outPacket)
        {
            outPacket.WriteString(RoomName);
            outPacket.WriteInt(0);
            outPacket.WriteByte(1);
            outPacket.WriteByte(7); // (byte)channeldata2
            outPacket.WriteInt(0);
            outPacket.WriteHexString("083483D162"); // 08 24 72 F5 9E
            outPacket.WriteInt(0);
            outPacket.WriteByte(0);
            outPacket.WriteByte(0);
            outPacket.WriteByte(0);
        }

        // 移除所有 Ai* 节点 (保留 AiData）
        static void RemoveAiNodes(XmlNode rootNode)
        {
            // 创建一个临时列表来存储要删除的节点
            System.Collections.Generic.List<XmlNode> nodesToRemove = new System.Collections.Generic.List<XmlNode>();

            // 收集所有需要删除的节点
            foreach (XmlNode node in rootNode.ChildNodes)
            {
                if (node.Name.StartsWith("Ai") && node.Name != "AiData")
                {
                    nodesToRemove.Add(node);
                }
            }

            // 从 XML 文档中删除收集的节点
            foreach (XmlNode node in nodesToRemove)
            {
                rootNode.RemoveChild(node);
            }
        }

        // 添加指定数量的 Ai 节点
        static void AddAiNodes(XmlNode rootNode, int count)
        {
            XmlDocument xmlDoc = rootNode.OwnerDocument;

            for (int i = 1; i <= count; i++)
            {
                // 创建新的 Ai 节点
                string nodeName = i == 1 ? "Ai1" : $"Ai{i}";
                XmlElement aiElement = xmlDoc.CreateElement(nodeName);

                // 添加属性
                aiElement.SetAttribute("character", "1");
                aiElement.SetAttribute("rid", i.ToString());
                aiElement.SetAttribute("kart", "1508");
                aiElement.SetAttribute("balloon", "0");
                aiElement.SetAttribute("headBand", "0");
                aiElement.SetAttribute("goggle", "0");

                // 添加到根节点
                rootNode.AppendChild(aiElement);
            }
        }
    }
}
