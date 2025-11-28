using Launcher.App.ExcData;
using Launcher.App.Server;

namespace Launcher.App.Profile
{
    public class GameDataReset
    {
        public static void DataReset(string Nickname)
        {
            if (ProfileService.ProfileConfigs[Nickname].Rider.Lucci > uint.MaxValue)
            {
                ProfileService.ProfileConfigs[Nickname].Rider.Lucci = SessionGroup.LucciMax;
            }
            ProfileService.Save(Nickname);
            SpeedPatch.SpeedPatchData();
        }
    }
}
