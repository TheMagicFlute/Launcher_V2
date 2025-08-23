using System;
using System.Xml;
using ExcData;
using KartRider;
using KartRider.IO.Packet;

namespace RiderData
{
    public static class Emblem
    {
        public static void RmOwnerEmblemPacket()
        {
            int All_Emblem = KartExcData.emblem.Count;
            using (OutPacket outPacket = new OutPacket("RmOwnerEmblemPacket"))
            {
                outPacket.WriteInt(1);
                outPacket.WriteInt(1);
                outPacket.WriteInt(All_Emblem);
                for (int i = 0; i < KartExcData.emblem.Count; i++)
                {
                    outPacket.WriteShort(KartExcData.emblem[i]);
                }
                RouterListener.MySession.Client.Send(outPacket);
            }
        }
    }
}
