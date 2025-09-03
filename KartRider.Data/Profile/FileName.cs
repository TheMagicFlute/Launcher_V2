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

        public static string TCGKartGamePath = Path.GetFullPath((string)Registry.GetValue(TCGKartRegPath, "gamepath", null));

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

        #region Legacy Configs

        public const string Extension = ".ini";

        public static string ConfigRoot = AppDomain.CurrentDomain.BaseDirectory + @"Profile\Launcher\";

		public static string LoadFolder = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Launcher\"));
		public static string SetRider_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Launcher\SetRider\"));
		public static string SetRiderItem_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Launcher\SetRider\SetRiderItem\"));
		public static string SetMyRoom_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Launcher\MyRoom\"));
		public static string SetGameOption_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Launcher\GameOption\"));
		public static string config_LoadFile = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Launcher\config\"));

        public static string Load_Console = Path.GetFullPath(Path.Combine(AppDir, @"Profile\Console.ini"));
		public static string Load_CC = Path.GetFullPath(Path.Combine(AppDir, @"Profile\CountryCode.ini"));

        public static string SetRider_Nickname = "Set_Nickname";
        public static string SetRider_RiderIntro = "Set_RiderIntro";
        public static string SetRider_Card = "Set_Card";
        public static string SetRider_Emblem1 = "Set_Emblem1";
        public static string SetRider_Emblem2 = "Set_Emblem2";
        public static string SetRider_Lucci = "Set_Lucci";
        public static string SetRider_RP = "Set_RP";
        public static string SetRider_Koin = "Set_Koin";
        public static string SetRider_Premium = "Set_Premium";
        public static string SetRider_SlotChanger = "Set_SlotChanger";
        public static string SetRider_ClubMark_LOGO = "Set_ClubMark_LOGO";
        public static string SetRider_ClubMark_LINE = "Set_ClubMark_LINE";
        public static string SetRider_ClubName = "Set_ClubName";

        public static string SetRiderItem_Character = "Set_Character";
        public static string SetRiderItem_Paint = "Set_Paint";
        public static string SetRiderItem_Kart = "Set_Kart";
        public static string SetRiderItem_Plate = "Set_Plate";
        public static string SetRiderItem_Goggle = "Set_Goggle";
        public static string SetRiderItem_Balloon = "Set_Balloon";
        public static string SetRiderItem_HeadBand = "Set_HeadBand";
        public static string SetRiderItem_HeadPhone = "Set_HeadPhone";
        public static string SetRiderItem_HandGearL = "Set_HandGearL";
        public static string SetRiderItem_Uniform = "Set_Uniform";
        public static string SetRiderItem_Decal = "Set_Decal";
        public static string SetRiderItem_Pet = "Set_Pet";
        public static string SetRiderItem_FlyingPet = "Set_FlyingPet";
        public static string SetRiderItem_Aura = "Set_Aura";
        public static string SetRiderItem_SkidMark = "Set_SkidMark";
        public static string SetRiderItem_RidColor = "Set_RidColor";
        public static string SetRiderItem_BonusCard = "Set_BonusCard";
        public static string SetRiderItem_Tachometer = "Set_Tachometer";
        public static string SetRiderItem_Dye = "Set_Dye";
        public static string SetRiderItem_KartSN = "Set_KartSN";
        public static string SetRiderItem_slotBg = "Set_slotBg";

        public static string SetMyRoom_MyRoom = "Set_MyRoom";
        public static string SetMyRoom_MyRoomBGM = "Set_MyRoomBGM";
        public static string SetMyRoom_UseRoomPwd = "Set_UseRoomPwd";
        public static string SetMyRoom_UseItemPwd = "Set_UseItemPwd";
        public static string SetMyRoom_TalkLock = "Set_TalkLock";
        public static string SetMyRoom_RoomPwd = "Set_RoomPwd";
        public static string SetMyRoom_ItemPwd = "Set_ItemPwd";
        public static string SetMyRoom_MyRoomKart1 = "Set_MyRoomKart1";
        public static string SetMyRoom_MyRoomKart2 = "Set_MyRoomKart2";

        public static string SetGameOption_Set_BGM = "Set_BGM";
        public static string SetGameOption_Set_Sound = "Set_Sound";
        public static string SetGameOption_Main_BGM = "Main_BGM";
        public static string SetGameOption_Sound_effect = "Sound_effect";
        public static string SetGameOption_Full_screen = "Full_screen";
        public static string SetGameOption_Unk1 = "Unk1";
        public static string SetGameOption_Unk2 = "Unk2";
        public static string SetGameOption_Unk3 = "Unk3";
        public static string SetGameOption_Unk4 = "Unk4";
        public static string SetGameOption_Unk5 = "Unk5";
        public static string SetGameOption_Unk6 = "Unk6";
        public static string SetGameOption_Unk7 = "Unk7";
        public static string SetGameOption_Unk8 = "Unk8";
        public static string SetGameOption_Unk9 = "Unk9";
        public static string SetGameOption_Unk10 = "Unk10";
        public static string SetGameOption_Unk11 = "Unk11";
        public static string SetGameOption_BGM_Check = "BGM_Check";
        public static string SetGameOption_Sound_Check = "Sound_Check";
        public static string SetGameOption_Unk12 = "Unk12";
        public static string SetGameOption_Unk13 = "Unk13";
        public static string SetGameOption_GameType = "GameType";
        public static string SetGameOption_SetGhost = "SetGhost";
        public static string SetGameOption_SpeedType = "SpeedType";
        public static string SetGameOption_Unk14 = "Unk14";
        public static string SetGameOption_Unk15 = "Unk15";
        public static string SetGameOption_Unk16 = "Unk16";
        public static string SetGameOption_Unk17 = "Unk17";
        public static string SetGameOption_Set_screen = "Set_screen";
        public static string SetGameOption_Unk18 = "Unk18";
        public static string SetGameOption_Version = "Version";

        public static string SetETC_KartTune = "ETC_KartTune";
        public static string SetETC_KartPlant = "ETC_KartPlant";
        public static string SetETC_KartLevel = "ETC_KartLevel";
        public static string SetETC_KartParts = "ETC_KartParts";
        public static string SetETC_DataPack = "ETC_DataPack";

        public static string config_FavoriteItem = "FavoriteItem";
        public static string config_PreventItem = "PreventItem";
        public static string config_SpeedPatch = "SP_UpdateTest";

        #endregion
    }
}
