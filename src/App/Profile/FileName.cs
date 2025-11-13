using Microsoft.Win32;

namespace Launcher.App.Profile
{
    public class fileName
    {
        public string NicknameDir;
        public string config_path;
        public string ItemPresetsConfig;
        public string NewKart_LoadFile;
        public string Favorite_LoadFile;
        public string FavoriteTrack_LoadFile;
        public string AI_LoadFile;
        public string TuneData_LoadFile;
        public string PlantData_LoadFile;
        public string LevelData_LoadFile;
        public string PartsData_LoadFile;
        public string Parts12Data_LoadFile;
        public string Level12Data_LoadFile;
        public string Competitive_LoadFile;
        public string TrainingMission_LoadFile;
    }

    public static class FileName
    {
        #region Constants

        public const string KartRider = "KartRider.exe";
        public const string PinFile = "KartRider.pin";
        public const string PinFileBak = "KartRider-bak.pin";

        public const string TCGKartRegPath = @"HKEY_CURRENT_USER\SOFTWARE\TCGame\kart";

        #endregion

        public static string AppDir = AppDomain.CurrentDomain.BaseDirectory; // 应用程序所在目录

        public static readonly string TCGKartGamePath = Path.GetFullPath((string)(Registry.GetValue(TCGKartRegPath, "gamepath", AppDir) ?? AppDir));

        public static readonly string Update_File = Path.GetFullPath(Path.Combine(AppDir, @"Update.bat"));
        public static readonly string Update_Folder = Path.GetFullPath(Path.Combine(AppDir, @"Update\"));

        public static string ProfileDir = Path.GetFullPath(Path.Combine(AppDir, @"Profile"));
        public static readonly string LogDir = Path.GetFullPath(Path.Combine(ProfileDir, @"Logs\"));
        public static readonly string TimeAttackLog = Path.GetFullPath(Path.Combine(LogDir, @"TimeAttack.log"));
        public static readonly string ConfigFile = Path.GetFullPath(Path.Combine(ProfileDir, @"Config.json"));
        public static string Load_Settings = Path.GetFullPath(Path.Combine(ProfileDir, @"Settings.json"));
        public static string NewKart_LoadFile = Path.GetFullPath(Path.Combine(ProfileDir, @"NewKart.json"));
        public static string ModelMax_LoadFile = Path.GetFullPath(Path.Combine(ProfileDir, @"ModelMax.xml"));
        public static string SpecialKartConfig = Path.GetFullPath(Path.Combine(ProfileDir, @"SpecialKartConfig.json"));

        public static Dictionary<string, fileName> FileNames = new();

        public static void Load(string nickname)
        {
            var filename = new fileName();
            filename.NicknameDir = Path.GetFullPath(Path.Combine(ProfileDir, nickname));
            filename.config_path = Path.GetFullPath(Path.Combine(filename.NicknameDir, @"Launcher.json"));
            filename.ItemPresetsConfig = Path.GetFullPath(Path.Combine(filename.NicknameDir, @"ItemPresetsConfig.json"));
            filename.Favorite_LoadFile = Path.GetFullPath(Path.Combine(filename.NicknameDir, @"Favorite.json"));
            filename.FavoriteTrack_LoadFile = Path.GetFullPath(Path.Combine(filename.NicknameDir, @"FavoriteTrack.json"));
            filename.AI_LoadFile = Path.GetFullPath(Path.Combine(filename.NicknameDir, @"AI.xml"));
            filename.TuneData_LoadFile = Path.GetFullPath(Path.Combine(filename.NicknameDir, @"TuneData.json"));
            filename.PlantData_LoadFile = Path.GetFullPath(Path.Combine(filename.NicknameDir, @"PlantData.json"));
            filename.LevelData_LoadFile = Path.GetFullPath(Path.Combine(filename.NicknameDir, @"LevelData.json"));
            filename.PartsData_LoadFile = Path.GetFullPath(Path.Combine(filename.NicknameDir, @"PartsData.json"));
            filename.Parts12Data_LoadFile = Path.GetFullPath(Path.Combine(filename.NicknameDir, @"Parts12Data.json"));
            filename.Level12Data_LoadFile = Path.GetFullPath(Path.Combine(filename.NicknameDir, @"Level12Data.json"));
            filename.Competitive_LoadFile = Path.GetFullPath(Path.Combine(filename.NicknameDir, @"Competitive.json"));
            filename.TrainingMission_LoadFile = Path.GetFullPath(Path.Combine(ProfileDir, @"TrainingMission.json"));
            FileNames.TryAdd(nickname, filename);
            if (!Directory.Exists(filename.NicknameDir))
            {
                Directory.CreateDirectory(filename.NicknameDir);
            }
            ProfileService.Load(nickname);
        }
    }
}
