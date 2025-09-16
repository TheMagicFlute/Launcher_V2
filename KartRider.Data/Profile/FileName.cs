using System;
using System.IO;
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

        public static string AppDir = AppDomain.CurrentDomain.BaseDirectory;

        public static string TCGKartGamePath = Path.GetFullPath((string)Registry.GetValue(TCGKartRegPath, "gamepath", AppDir));

        public static string ProfileDir = Path.GetFullPath(Path.Combine(AppDir, @"Profile\"));
        public static string ConfigFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Config.json"));
        public static string SpecialKartConfig = Path.GetFullPath(Path.Combine(AppDir, @"Profile\SpecialKartConfig.json"));

        public static string Update_File = Path.GetFullPath(Path.Combine(AppDir, @"Update.bat"));
        public static string Update_Folder = Path.GetFullPath(Path.Combine(AppDir, @"Update\"));

        public static string Whitelist = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Whitelist.ini"));
        public static string Blacklist = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Blacklist.ini"));

        public static string NewKart_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\NewKart.xml"));
        public static string ModelMax_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\ModelMax.xml"));
        public static string Favorite_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Favorite.xml"));
        public static string FavoriteTrack_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\FavoriteTrack.xml"));
        public static string TrainingMission_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\TrainingMission.xml"));
        public static string Competitive_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Competitive.xml"));
        public static string AI_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\AI.xml"));
        public static string TuneData_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\TuneData.xml"));
        public static string PlantData_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\PlantData.xml"));
        public static string LevelData_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\LevelData.xml"));
        public static string PartsData_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\PartsData.xml"));
        public static string Parts12Data_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Parts12Data.xml"));
        public static string Level12Data_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Level12Data.xml"));
    }
}
