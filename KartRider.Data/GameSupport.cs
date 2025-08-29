using System;
using System.Collections.Generic;
using System.Linq;
using ExcData;
using KartRider.IO.Packet;
using Profile;

namespace KartRider
{
    public static class GameSupport
    {
        public static void PcFirstMessage()
        {
            uint first_val = 418454259;
            uint second_val = 3259911425;
            using (OutPacket outPacket = new OutPacket("PcFirstMessage"))
            {
                outPacket.WriteUShort(SessionGroup.usLocale);
                outPacket.WriteUShort(1);
                outPacket.WriteUShort(ProfileService.ProfileConfig.GameOption.Version);
                outPacket.WriteString("http://kartupdate.tiancity.cn/patch/JDVDDSVTJVLGHXJ");
                outPacket.WriteUInt(first_val);
                outPacket.WriteUInt(second_val);
                outPacket.WriteByte(SessionGroup.nClientLoc);
                outPacket.WriteString("9wk/NSpInbhNJGTCHOvYH76HjtBwlUA7QKaxZlqwWu0=");
                outPacket.WriteBytes(new byte[31]);
                outPacket.WriteString("92Jw/2KaOSER68ywYfQoploG2FJgmhqCCBTSXaob5e8=");
                RouterListener.MySession.Client.Send(outPacket);
            }
            RouterListener.MySession.Client._RIV = first_val ^ second_val;
            RouterListener.MySession.Client._SIV = first_val ^ second_val;
            return;
        }

        public static void OnDisconnect()
        {
            RouterListener.MySession.Client.Disconnect();
        }

        public static void SpRpLotteryPacket()
        {
            using (OutPacket outPacket = new OutPacket("SpRpLotteryPacket"))
            {
                outPacket.WriteHexString("05 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                RouterListener.MySession.Client.Send(outPacket);
            }
        }

        public static void PrGetGameOption()
        {
            using (OutPacket outPacket = new OutPacket("PrGetGameOption"))
            {
                outPacket.WriteFloat(ProfileService.ProfileConfig.GameOption.Set_BGM);
                outPacket.WriteFloat(ProfileService.ProfileConfig.GameOption.Set_Sound);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.Main_BGM);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.Sound_effect);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.Full_screen);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.ShowMirror);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.ShowOtherPlayerNames);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.ShowOutlines);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.ShowShadows);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.HighLevelEffect);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.MotionBlurEffect);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.MotionDistortionEffect);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.HighEndOptimization);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.AutoReady);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.PropDescription);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.VideoQuality);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.BGM_Check);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.Sound_Check);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.ShowHitInfo);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.AutoBoost);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.GameType);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.SetGhost);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.SpeedType);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.RoomChat);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.DrivingChat);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.ShowAllPlayerHitInfo);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.ShowTeamColor);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.Set_screen);
                outPacket.WriteByte(ProfileService.ProfileConfig.GameOption.HideCompetitiveRank);
                outPacket.WriteBytes(new byte[79]);
                RouterListener.MySession.Client.Send(outPacket);
            }
        }

        public static void ChRpEnterMyRoomPacket()
        {
            if (GameType.EnterMyRoomType == 0)
            {
                using (OutPacket outPacket = new OutPacket("ChRpEnterMyRoomPacket"))
                {
                    outPacket.WriteString(ProfileService.ProfileConfig.Rider.Nickname);
                    outPacket.WriteByte(0);
                    outPacket.WriteShort(ProfileService.ProfileConfig.MyRoom.MyRoom);
                    outPacket.WriteByte(ProfileService.ProfileConfig.MyRoom.MyRoomBGM);
                    outPacket.WriteByte(ProfileService.ProfileConfig.MyRoom.UseRoomPwd);
                    outPacket.WriteByte(0);
                    outPacket.WriteByte(ProfileService.ProfileConfig.MyRoom.UseItemPwd);
                    outPacket.WriteByte(ProfileService.ProfileConfig.MyRoom.TalkLock);
                    outPacket.WriteString(ProfileService.ProfileConfig.MyRoom.RoomPwd);
                    outPacket.WriteString("");
                    outPacket.WriteString(ProfileService.ProfileConfig.MyRoom.ItemPwd);
                    outPacket.WriteShort(ProfileService.ProfileConfig.MyRoom.MyRoomKart1);
                    outPacket.WriteShort(ProfileService.ProfileConfig.MyRoom.MyRoomKart2);
                    RouterListener.MySession.Client.Send(outPacket);
                }
            }
            else
            {
                using (OutPacket outPacket = new OutPacket("ChRpEnterMyRoomPacket"))
                {
                    outPacket.WriteString("");
                    outPacket.WriteByte(GameType.EnterMyRoomType);
                    outPacket.WriteShort(0);
                    outPacket.WriteByte(0);
                    outPacket.WriteByte(0);
                    outPacket.WriteByte(0);
                    outPacket.WriteByte(0);
                    outPacket.WriteByte(1);
                    outPacket.WriteString(""); // RoomPwd
                    outPacket.WriteString("");
                    outPacket.WriteString(""); // ItemPwd
                    outPacket.WriteShort(0);
                    outPacket.WriteShort(0);
                    RouterListener.MySession.Client.Send(outPacket);
                }
            }
        }

        public static void RmNotiMyRoomInfoPacket()
        {
            using (OutPacket outPacket = new OutPacket("RmNotiMyRoomInfoPacket"))
            {
                outPacket.WriteShort(ProfileService.ProfileConfig.MyRoom.MyRoom);
                outPacket.WriteByte(ProfileService.ProfileConfig.MyRoom.MyRoomBGM);
                outPacket.WriteByte(ProfileService.ProfileConfig.MyRoom.UseRoomPwd);
                outPacket.WriteByte(0);
                outPacket.WriteByte(ProfileService.ProfileConfig.MyRoom.UseItemPwd);
                outPacket.WriteByte(ProfileService.ProfileConfig.MyRoom.TalkLock);
                outPacket.WriteString(ProfileService.ProfileConfig.MyRoom.RoomPwd);
                outPacket.WriteString("");
                outPacket.WriteString(ProfileService.ProfileConfig.MyRoom.ItemPwd);
                outPacket.WriteShort(ProfileService.ProfileConfig.MyRoom.MyRoomKart1);
                outPacket.WriteShort(ProfileService.ProfileConfig.MyRoom.MyRoomKart2);
                RouterListener.MySession.Client.Send(outPacket);
            }
        }

        public static void PrCheckMyClubStatePacket()
        {
            using (OutPacket outPacket = new OutPacket("PrCheckMyClubStatePacket"))
            {
                if (ProfileService.ProfileConfig.Rider.ClubMark_LOGO == 0)
                {
                    outPacket.WriteInt(0);
                    outPacket.WriteString("");
                    outPacket.WriteInt(0);
                    outPacket.WriteInt(0);
                }
                else
                {
                    outPacket.WriteInt(ProfileService.ProfileConfig.Rider.ClubCode);
                    outPacket.WriteString(ProfileService.ProfileConfig.Rider.ClubName);
                    outPacket.WriteInt(ProfileService.ProfileConfig.Rider.ClubMark_LOGO);
                    outPacket.WriteInt(ProfileService.ProfileConfig.Rider.ClubMark_LINE);
                }
                outPacket.WriteShort(5); // Grade
                outPacket.WriteString(ProfileService.ProfileConfig.Rider.Nickname);
                outPacket.WriteInt(1); // ClubMember
                outPacket.WriteByte(5); // Level
                outPacket.WriteHexString("A2 0E 90 AB 9A 99");
                RouterListener.MySession.Client.Send(outPacket);
            }
        }

		public static void ChRequestChStaticReplyPacket()
		{
			using (OutPacket outPacket = new OutPacket("ChRequestChStaticReplyPacket"))
			{
                outPacket.WriteHexString("01550300005301FBA2778C320F000078DAA556F957D34010FEE46E699BA60797788B27A2E2817881DC55391EF0BB8FD2803C03E5B555F4BF7766D3B54919B25B7C792FD9DDF9E6DCC9CC6C02F8E4D0AB862AF650C1027C5AEDE307BEA18E5378F454904813A4B52DE184DE475821963A1AE8620947B4F0708C0D7A9F1124D8EDD27B8FBEDD1743B4B49EBC518A56D89B374AD3D0BE44C4742DA93F217A349016D15A58321B4BF6D47B301B1BAC0094722D244D23ED5AC89A46A6680C5D705A269407A7680C5F189E75445BC310D711ED0C43721C9643FCA48DAF8892FA7C32026A492C6488B08FEF4AF32991ABA4A28E6DFAFEA1331FC53E029469D9A0C7279E213E3820FA3189F389303CA00E7879F8CF8991F0A1367B54BE41FE1ED0F744193F26E742147435198AB486AC12D3B82B1096892BF0F184280CBB36282A61D2752736831872236791640CBC995161AF29CD153AA8916DBFA904680B6F3917005AD26FE78D101DC83B66A8B6F5EE98B5D468E427460D2EC9797FAF33366DE67D57655F95EA6659A55BB06E97FE602416269BF4305EB636E1D1A808DB31C4E9B1894D4EECC94E3CD1229EE42C4D9CCA591AF554AE3B3BEA27AA10D3199EC9FF4918F27CC82825EAFDF490516694E1453F31F82A1C5AFACBD691E67F950A1585F526B3C6BF96889A73860D6AA83A5B232257412E240C3A147F9637B60C5AC3EC782C43FC5FF5F632CC5AF3BB78665FC1B867784226BD673F7F29D63A41EA24A04A044F690D208BAA4794D5DEC307F702862DDA05676CEEC71ED58F7CDA963197096DB6C99A7DA5A015C4F9B408D07E2F307F45F9D120CF5A3EEA102C0639DC505B5F81DA43BC943D07391F8DE59CD0754AAADB78CDAAD7500C2BE171AD24066AD5116445216B721F8D824A721F8D823E172C3A32DF548DE85F0A169D5983BF66426E2CD10555DB54AF47EB8704D94809D1DA6DF6D7CDE0EE79C0AED3536D2697866D312F8F310D15FC60B0D139C773DA159803BD4A0824609ED338D6E882F9765718D80D9BBB2931B4073677BDC6D05ED88C57DC96D0079B218B4704F4A3B31998AF1D03E86C12E6B82109F3B0CB450483689F49591B529006D16126A5112E2B1FF92803537199639883F81233CFA02C3A6D1333CCE6E27F6AFF2C8BC8212ECDB71892876DD39F6278019799402699B588CBCD8613CCFC179BA9A2FC");
				RouterListener.MySession.Client.Send(outPacket);
			}
		}

        public static void PrDynamicCommand()
        {
            using (OutPacket outPacket = new OutPacket("PrDynamicCommand"))
            {
                outPacket.WriteByte(0);
                RouterListener.MySession.Client.Send(outPacket);
            }
        }

        public static void PrQuestUX2ndPacket()
        {
            int All_Quest = KartExcData.quest.Count;
            using (OutPacket outPacket = new OutPacket("PrQuestUX2ndPacket"))
            {
                outPacket.WriteInt(1);
                outPacket.WriteInt(1);
                outPacket.WriteInt(All_Quest);
                foreach (var item in KartExcData.quest)
                {
                    outPacket.WriteInt(item);
                    outPacket.WriteInt(item);
                    outPacket.WriteInt(0);
                    outPacket.WriteShort(-1);
                    outPacket.WriteShort(0);
                    outPacket.WriteInt(0);
                    outPacket.WriteInt(0);
                    outPacket.WriteInt(1);
                    outPacket.WriteInt(0);
                    outPacket.WriteByte(0);
                }
                RouterListener.MySession.Client.Send(outPacket);
            }
        }

        public static void GetRider(OutPacket outPacket)
        {
            var KartAndSN = new { Kart = ProfileService.ProfileConfig.RiderItem.Set_Kart, SN = ProfileService.ProfileConfig.RiderItem.Set_KartSN };
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_Character);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_Paint);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_Kart);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_Plate);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_Goggle);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_Balloon);
            outPacket.WriteShort(0);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_HeadBand);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_HeadPhone);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_HandGearL);
            outPacket.WriteShort(0);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_Uniform);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_Decal);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_Pet);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_FlyingPet);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_Aura);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_SkidMark);
            outPacket.WriteShort(0);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_RidColor);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_BonusCard);
            outPacket.WriteShort(0); // bossModeCard
            var existingPlant = KartExcData.PlantList.FirstOrDefault(list => list[0] == KartAndSN.Kart && list[1] == KartAndSN.SN);
            if (existingPlant != null)
            {
                outPacket.WriteShort(existingPlant[3]);
                outPacket.WriteShort(existingPlant[7]);
                outPacket.WriteShort(existingPlant[5]);
                outPacket.WriteShort(existingPlant[9]);
            }
            else
            {
                outPacket.WriteShort(0);
                outPacket.WriteShort(0);
                outPacket.WriteShort(0);
                outPacket.WriteShort(0);
            }
            outPacket.WriteShort(0);
            outPacket.WriteShort(0);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_Tachometer);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_Dye);
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_KartSN);
            outPacket.WriteByte(0);
            var existingParts = KartExcData.PartsList.FirstOrDefault(list => list[0] == KartAndSN.Kart && list[1] == KartAndSN.SN);
            if (existingParts != null)
            {
                outPacket.WriteShort(existingParts[14]);
                outPacket.WriteShort(existingParts[15]);
            }
            else
            {
                outPacket.WriteShort(0);
                outPacket.WriteShort(0);
            }
            outPacket.WriteShort(ProfileService.ProfileConfig.RiderItem.Set_slotBg);
            var existingParts12 = KartExcData.Parts12List.FirstOrDefault(list => list[0] == KartAndSN.Kart && list[1] == KartAndSN.SN);
            if (existingParts12 != null)
            {
                outPacket.WriteShort(existingParts12[14]);
                outPacket.WriteShort(existingParts12[15]);
                outPacket.WriteShort(existingParts12[16]);
            }
            else
            {
                outPacket.WriteShort(0);
                outPacket.WriteShort(0);
                outPacket.WriteShort(0);
            }
        }

        public static short GetItemSkill(short skill)
        {
            List<short> skills = V2Spec.GetSkills();
            for (int i = 0; i < skills.Count; i++)
            {
                if (V2Spec.itemSkill.TryGetValue(skills[i], out var Level) &&
                    Level.TryGetValue(skill, out var LevelSkill))
                {
                    return LevelSkill;
                }
            }
            if (MultiPlayer.skillChange.TryGetValue(ProfileService.ProfileConfig.RiderItem.Set_Kart, out var changes) &&
                changes.TryGetValue(skill, out var changesSkill))
            {
                return changesSkill;
            }
            return skill;
        }

        public static void AddItemSkill(short skill)
        {
            skill = GameSupport.GetItemSkill(skill);
            using (OutPacket oPacket = new OutPacket("GameSlotPacket"))
            {
                oPacket.WriteInt();
                oPacket.WriteUInt(4294967295);
                oPacket.WriteByte(10);
                oPacket.WriteHexString("001000");
                oPacket.WriteShort(skill);
                oPacket.WriteByte(1);
                oPacket.WriteBytes(new byte[3]);
                oPacket.WriteByte(2);
                oPacket.WriteShort(skill);
                oPacket.WriteBytes(new byte[5]);
                RouterListener.MySession.Client.Send(oPacket);
            }
        }

        public static void AttackedSkill(byte type, byte uni, short skill)
        {
            skill = GameSupport.GetItemSkill(skill);
            using (OutPacket oPacket = new OutPacket("GameSlotPacket"))
            {
                oPacket.WriteInt();
                oPacket.WriteUInt();
                oPacket.WriteByte(type);
                oPacket.WriteByte(uni);
                oPacket.WriteShort(skill);
                oPacket.WriteByte(1);
                oPacket.WriteShort();
                oPacket.WriteByte(2);
                oPacket.WriteShort(skill);
                oPacket.WriteBytes(new byte[5]);
                RouterListener.MySession.Client.Send(oPacket);
            }
        }
    }
}
