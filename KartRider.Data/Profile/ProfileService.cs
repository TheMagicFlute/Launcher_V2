using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using KartRider;
using Newtonsoft.Json;
using RiderData;

namespace Profile
{
    public class ProfileService
    {
        public static ProfileConfig ProfileConfig { get; set; } = new();

        /// <summary>
        /// Clamp values to valid ranges before saving and after loading.
        /// </summary>
        public static void ClampValue()
        {
            ProfileService.ProfileConfig.Rider.Lucci = Math.Min(ProfileService.ProfileConfig.Rider.Lucci, SessionGroup.LucciMax);
            ProfileService.ProfileConfig.GameOption.Set_BGM = Math.Clamp(ProfileService.ProfileConfig.GameOption.Set_BGM, 0f, 1f);
            ProfileService.ProfileConfig.GameOption.Set_Sound = Math.Clamp(ProfileService.ProfileConfig.GameOption.Set_Sound, 0f, 1f);
        }

        public static void Save()
        {
            ClampValue();
            try
            {
                var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                };

                using (StreamWriter streamWriter = new StreamWriter(FileName.ConfigFile, false))
                {
                    streamWriter.Write(Newtonsoft.Json.JsonConvert.SerializeObject(ProfileConfig, jsonSettings));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving config: {ex.Message}");
            }
        }

        public static void Load()
        {
            try
            {
                ProfileConfig = JsonConvert.DeserializeObject<ProfileConfig>(File.ReadAllText(FileName.ConfigFile)) ?? new ProfileConfig();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading config: {ex.Message}");
                ProfileConfig = new ProfileConfig();
            }
            finally
            {
                ClampValue();
                Save();
            }
        }
    }
}
