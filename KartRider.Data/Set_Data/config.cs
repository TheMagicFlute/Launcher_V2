using System;
using System.IO;
using KartRider;

namespace Set_Data
{
    public static class Config
    {
        public static byte PreventItem_Use = 0;
        public static byte SpeedPatch_Use = 0;
        public static byte SpeedType = 7;

        public static void Load_PreventItem()
        {
            string Load_PreventItem = FileName.config_LoadFile + FileName.config_PreventItem + FileName.Extension;
            if (File.Exists(Load_PreventItem))
            {
                string textValue = System.IO.File.ReadAllText(Load_PreventItem);
                Config.PreventItem_Use = byte.Parse(textValue);
            }
            else
            {
                using (StreamWriter streamWriter = new StreamWriter(Load_PreventItem, false))
                {
                    streamWriter.Write(Config.PreventItem_Use);
                }
            }
            Config.Check_PreventItem();
        }

        public static void Load_SpeedPatch()
        {
            string Load_SpeedPatch = FileName.config_LoadFile + FileName.config_SpeedPatch + FileName.Extension;
            if (File.Exists(Load_SpeedPatch))
            {
                string textValue = System.IO.File.ReadAllText(Load_SpeedPatch);
                Config.SpeedPatch_Use = byte.Parse(textValue);
            }
            else
            {
                using (StreamWriter streamWriter = new StreamWriter(Load_SpeedPatch, false))
                {
                    streamWriter.Write(Config.SpeedPatch_Use);
                }
            }
            Config.Check_SpeedPatch();
        }

        public static void Check_PreventItem()
        {
            if (Config.PreventItem_Use == 0)
            {
                Program.PreventItem = false;
            }
            else
            {
                Program.PreventItem = true;
            }
        }

        public static void Check_SpeedPatch()
        {
            if (Config.SpeedPatch_Use == 0)
            {
                Program.SpeedPatch = false;
                Program.LauncherDlg.Text = "Launcher";
            }
            else
            {
                Program.SpeedPatch = true;
                Program.LauncherDlg.Text = "Launcher (Speed Patch)"; // 속도 패치 Speed Patch
            }
        }

        public static void Load_ALL()
        {
            Config.Load_PreventItem();
            Config.Load_SpeedPatch();
        }
    }
}
