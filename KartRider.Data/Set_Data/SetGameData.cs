﻿using System;
using System.IO;
using KartRider;
using ExcData;
using System.Linq;

namespace Set_Data
{
	public static class SetGameData
	{
		// 存昵称
		public static void Save_Nickname()
		{
			using (StreamWriter streamWriter = new StreamWriter(FileName.SetRider_LoadFile + FileName.SetRider_Nickname + FileName.Extension, false))
			{
				streamWriter.Write(SetRider.Nickname);
			}
		}

		// 存简介
		public static void Save_RiderIntro()
		{
			using (StreamWriter streamWriter = new StreamWriter(FileName.SetRider_LoadFile + FileName.SetRider_RiderIntro + FileName.Extension, false))
			{
				streamWriter.Write(SetRider.RiderIntro);
			}
		}

		// 存徽章
		public static void Save_Emblem()
		{
			using (StreamWriter streamWriter = new StreamWriter(FileName.SetRider_LoadFile + FileName.SetRider_Emblem1 + FileName.Extension, false))
			{
				streamWriter.Write(SetRider.Emblem1);
			}
			using (StreamWriter streamWriter = new StreamWriter(FileName.SetRider_LoadFile + FileName.SetRider_Emblem2 + FileName.Extension, false))
			{
				streamWriter.Write(SetRider.Emblem2);
			}
		}

		// 存计时数据
		public static void Save_RecordTimeAttack()
		{
			string trackName = StartGameData.StartTimeAttack_Track.ToString();
			if (KartExcData.track.ContainsKey(StartGameData.StartTimeAttack_Track))
			{
				trackName = KartExcData.track[StartGameData.StartTimeAttack_Track];
			}
			using (StreamWriter streamWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"Profile/TimeAttack.log", true))
			{
				streamWriter.WriteLine("[{0}] SpeedType:{1}, Infinite:{2}, GameType:{3}, Kart:{4}, FlyingPet:{5}, ResultType:{6}, RP:{7}, Lucci:{8}, Track:{9}, Record:{10}:{11}:{12}", DateTime.Now, SpeedType.speedNames.FirstOrDefault(pair => pair.Value == config.SpeedType).Key, StartGameData.StartTimeAttack_SpeedType == 4 ? 1 : 0, StartGameData.StartTimeAttack_GameType, StartGameData.Kart_id, StartGameData.FlyingPet_id, GameType.RewardType, GameType.TimeAttack_RP, GameType.TimeAttack_Lucci, trackName, GameType.min, GameType.sec, GameType.mil);
			}
		}

		// 存Lucci和经验
		public static void Save_RewardTimeAttack()
		{
			using (StreamWriter streamWriter = new StreamWriter(FileName.SetRider_LoadFile + FileName.SetRider_Lucci + FileName.Extension, false))
			{
				streamWriter.Write(SetRider.Lucci);
			}
			using (StreamWriter streamWriter = new StreamWriter(FileName.SetRider_LoadFile + FileName.SetRider_RP + FileName.Extension, false))
			{
				streamWriter.Write(SetRider.RP);
			}
		}

		// 存计时得到的Lucci
		public static void Save_TimeAttackDecLucci()
		{
			using (StreamWriter streamWriter = new StreamWriter(FileName.SetRider_LoadFile + FileName.SetRider_Lucci + FileName.Extension, false))
			{
				streamWriter.Write(SetRider.Lucci);
			}
		}

		// 存道具变更卡
		public static void Save_SlotChanger()
		{
			using (StreamWriter streamWriter = new StreamWriter(FileName.SetRider_LoadFile + FileName.SetRider_SlotChanger + FileName.Extension, false))
			{
				streamWriter.Write(SetRider.SlotChanger);
			}
		}

		// 存卡
		public static void Save_Card()
		{
			using (StreamWriter streamWriter = new StreamWriter(FileName.SetRider_LoadFile + FileName.SetRider_Card + FileName.Extension, false))
			{
				streamWriter.Write(SetRider.Card);
			}
		}
	}
}
