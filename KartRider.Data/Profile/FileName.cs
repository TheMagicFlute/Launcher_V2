using Microsoft.Win32;

namespace Profile
{
    /// <summary>
    /// A static class containing file names and paths.
    /// </summary>
    public static class FileName
    {
        #region Constants

        public const string KartRider = "KartRider.exe";
        public const string PinFile = "KartRider.pin";
        public const string PinFileBak = "KartRider-bak.pin";

        public const string TCGKartRegPath = @"HKEY_CURRENT_USER\SOFTWARE\TCGame\kart";

        #endregion

        public readonly static string AppDir = AppDomain.CurrentDomain.BaseDirectory;

        public readonly static string TCGKartGamePath = Path.GetFullPath((string)(Registry.GetValue(TCGKartRegPath, "gamepath", AppDir) ?? AppDir));

        public readonly static string ProfileDir = Path.GetFullPath(Path.Combine(AppDir, @"Profile\"));
        public readonly static string LogDir = Path.GetFullPath(Path.Combine(ProfileDir, @"Logs\"));
        public readonly static string ConfigFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Config.json"));
        public readonly static string SpecialKartConfig = Path.GetFullPath(Path.Combine(AppDir, @"Profile\SpecialKartConfig.json"));

        public readonly static string Update_File = Path.GetFullPath(Path.Combine(AppDir, @"Update.bat"));
        public readonly static string Update_Folder = Path.GetFullPath(Path.Combine(AppDir, @"Update\"));

        public readonly static string Whitelist = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Whitelist.ini"));
        public readonly static string Blacklist = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Blacklist.ini"));

        public readonly static string NewKart_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\NewKart.xml"));
        public readonly static string ModelMax_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\ModelMax.xml"));
        public readonly static string Favorite_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Favorite.xml"));
        public readonly static string FavoriteTrack_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\FavoriteTrack.xml"));
        public readonly static string TrainingMission_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\TrainingMission.xml"));
        public readonly static string Competitive_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Competitive.xml"));
        public readonly static string AI_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\AI.xml"));
        public readonly static string TuneData_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\TuneData.xml"));
        public readonly static string PlantData_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\PlantData.xml"));
        public readonly static string LevelData_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\LevelData.xml"));
        public readonly static string PartsData_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\PartsData.xml"));
        public readonly static string Parts12Data_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Parts12Data.xml"));
        public readonly static string Level12Data_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Level12Data.xml"));
    }
}
