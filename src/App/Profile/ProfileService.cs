using Launcher.App.Utility;

namespace Launcher.App.Profile
{
    public class ProfileService
    {
        public static Dictionary<string, ProfileConfig> ProfileConfigs { get; set; } = new();
        public static Setting SettingConfig { get; set; } = new Setting();

        public static void SaveSettings()
        {
            File.WriteAllText(FileName.Load_Settings, JsonHelper.Serialize(SettingConfig));
        }

        public static bool LoadSettings()
        {
            if (File.Exists(FileName.Load_Settings))
            {
                SettingConfig = JsonHelper.DeserializeNoBom<Setting>(FileName.Load_Settings);
                return true;
            }
            else
            {
                SettingConfig = new Setting();
                SaveSettings();
                return false;
            }
        }

        public static void Save(string Nickname)
        {
            if (!FileName.FileNames.ContainsKey(Nickname))
            {
                FileName.Load(Nickname);
            }
            var filename = FileName.FileNames[Nickname];
            if (ProfileConfigs.ContainsKey(Nickname))
            {
                File.WriteAllText(filename.config_path, JsonHelper.Serialize(ProfileConfigs[Nickname]));
            }
        }

        public static void Load(string Nickname)
        {
            if (!FileName.FileNames.ContainsKey(Nickname))
            {
                FileName.Load(Nickname);
            }
            var filename = FileName.FileNames[Nickname];

            if (File.Exists(filename.config_path))
            {
                ProfileConfigs.TryAdd(Nickname, JsonHelper.DeserializeNoBom<ProfileConfig>(filename.config_path));
            }
            else
            {
                ProfileConfigs.TryAdd(Nickname, new ProfileConfig());
                Save(Nickname);
            }
        }
    }
}
