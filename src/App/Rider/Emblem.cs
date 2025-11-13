using Launcher.App.Server;
using Launcher.Library.IO;

namespace Launcher.App.Rider
{
    public static class Emblem
    {
        public static List<short> emblem = new List<short>();

        public static void RmOwnerEmblemPacket(SessionGroup Parent)
        {
            int All_Emblem = emblem.Count;
            using (OutPacket outPacket = new OutPacket("RmOwnerEmblemPacket"))
            {
                outPacket.WriteInt(1);
                outPacket.WriteInt(1);
                outPacket.WriteInt(All_Emblem);
                foreach (var item in emblem)
                {
                    outPacket.WriteShort(item);
                }
                Parent.Client.Send(outPacket);
            }
        }
    }
}
