using Launcher.App.Server;
using Launcher.Library.IO;

namespace Launcher.App.Event
{
    public static class EventBuyCount
    {
        public static int BuyCount = 0;
        public static int[] ShopItem = [];

        public static void PrEventBuyCount(SessionGroup Parent)
        {
            using (OutPacket outPacket = new OutPacket("PrEventBuyCount"))
            {
                outPacket.WriteInt(BuyCount);
                for (int i = 0; i < ShopItem.Length; i++)
                {
                    outPacket.WriteInt(ShopItem[i]);
                    outPacket.WriteInt(0);
                }
                Parent.Client.Send(outPacket);
            }
        }
    }
}
