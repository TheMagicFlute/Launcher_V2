using System;
using System.Collections.Generic;
using KartRider;
using Profile;

namespace ExcData
{
    public class SpeedType
    {
        public static Dictionary<string, byte> speedNames = new Dictionary<string, byte>
        {
            { "统合S1.5", 7 },
            { "慢速S0", 3 },
            { "普通S1", 0 },
            { "快速S2", 1 },
            { "高速S3", 2 },
            { "复古初级", 10 },
            { "复古L3/老S1", 11 },
            { "复古L2/老S2", 12 },
            { "复古L1/老S3", 13 },
            { "复古Pro", 14 }
        };

        public static float AddSpec_TransAccelFactor = 0f;
        public static float AddSpec_SteerConstraint = 0f;
        public static float AddSpec_DriftEscapeForce = 0f;

        public static float DragFactor = 0f;
        public static float ForwardAccelForce = 0f;
        public static float BackwardAccelForce = 0f;
        public static float GripBrakeForce = 0f;
        public static float SlipBrakeForce = 0f;
        public static float SteerConstraint = 0f;
        public static float DriftEscapeForce = 0f;
        public static float CornerDrawFactor = 0f;
        public static float DriftMaxGauge = 0f;
        public static float TransAccelFactor = 0f;
        public static float BoostAccelFactor = 0f;
        public static float StartForwardAccelForceItem = 0f;
        public static float StartForwardAccelForceSpeed = 0f;

        public static void SpeedTypeData()
        {
            if (ProfileService.ProfileConfig.GameOption.SpeedType == 3) // S0 慢速 보통
            {
                SpeedType.AddSpec_SteerConstraint = -0.3f;
                SpeedType.AddSpec_DriftEscapeForce = -350f;
                SpeedType.AddSpec_TransAccelFactor = -0.015f;
                SpeedType.DragFactor = -0.05f;
                SpeedType.ForwardAccelForce = -530f;
                SpeedType.BackwardAccelForce = -225f;
                SpeedType.GripBrakeForce = -570f;
                SpeedType.SlipBrakeForce = -215f;
                SpeedType.SteerConstraint = -2.25f;
                SpeedType.DriftEscapeForce = -750f;
                SpeedType.CornerDrawFactor = -0.05f;
                SpeedType.DriftMaxGauge = 750f;
                SpeedType.TransAccelFactor = -0.2155f;
                SpeedType.BoostAccelFactor = 0.006f;
                SpeedType.StartForwardAccelForceItem = -530f;
                SpeedType.StartForwardAccelForceSpeed = -950f;
                Console.WriteLine("SpeedType: 低速 S0");
            }
            else if (ProfileService.ProfileConfig.GameOption.SpeedType == 0) // S1 普通 빠름
            {
                SpeedType.AddSpec_SteerConstraint = 1.7f;
                SpeedType.AddSpec_DriftEscapeForce = 150f;
                SpeedType.AddSpec_TransAccelFactor = 0.199f;
                SpeedType.DragFactor = -0.015f;
                SpeedType.ForwardAccelForce = -200f;
                SpeedType.BackwardAccelForce = -225f;
                SpeedType.GripBrakeForce = -270f;
                SpeedType.SlipBrakeForce = -165f;
                SpeedType.SteerConstraint = -0.25f;
                SpeedType.DriftEscapeForce = -250f;
                SpeedType.CornerDrawFactor = -0.03f;
                SpeedType.DriftMaxGauge = -330f;
                SpeedType.TransAccelFactor = -0.0015f;
                SpeedType.BoostAccelFactor = 0.006f;
                SpeedType.StartForwardAccelForceItem = -200f;
                SpeedType.StartForwardAccelForceSpeed = -360f;
                Console.WriteLine("SpeedType: 普通 S1");
            }
            else if (ProfileService.ProfileConfig.GameOption.SpeedType == 1) // S2 快速 매우빠름
            {
                SpeedType.AddSpec_SteerConstraint = 2.2f;
                SpeedType.AddSpec_DriftEscapeForce = 1100f;
                SpeedType.AddSpec_TransAccelFactor = 0.202f;
                SpeedType.DragFactor = 0.0121f;
                SpeedType.ForwardAccelForce = 200f;
                SpeedType.BackwardAccelForce = 225f;
                SpeedType.GripBrakeForce = 270f;
                SpeedType.SlipBrakeForce = 165f;
                SpeedType.SteerConstraint = 0.25f;
                SpeedType.DriftEscapeForce = 700f;
                SpeedType.CornerDrawFactor = 0f;
                SpeedType.DriftMaxGauge = 580f;
                SpeedType.TransAccelFactor = 0.0015f;
                SpeedType.BoostAccelFactor = 0.006f;
                SpeedType.StartForwardAccelForceItem = 200f;
                SpeedType.StartForwardAccelForceSpeed = 360f;
                Console.WriteLine("SpeedType: 快速 S2");
            }
            else if (ProfileService.ProfileConfig.GameOption.SpeedType == 2) // S3 高速 가장빠름
            {
                SpeedType.AddSpec_SteerConstraint = 2.7f;
                SpeedType.AddSpec_DriftEscapeForce = 1500f;
                SpeedType.AddSpec_TransAccelFactor = 0.2f;
                SpeedType.DragFactor = 0.04f;
                SpeedType.ForwardAccelForce = 750f;
                SpeedType.BackwardAccelForce = 450f;
                SpeedType.GripBrakeForce = 540f;
                SpeedType.SlipBrakeForce = 325f;
                SpeedType.SteerConstraint = 0.75f;
                SpeedType.DriftEscapeForce = 1100f;
                SpeedType.CornerDrawFactor = -0.02f;
                SpeedType.DriftMaxGauge = 1700f;
                SpeedType.TransAccelFactor = -0.0005f;
                SpeedType.BoostAccelFactor = 0.006f;
                SpeedType.StartForwardAccelForceItem = 750f;
                SpeedType.StartForwardAccelForceSpeed = 1350f;
                Console.WriteLine("SpeedType: 高速 S3");
            }
            else if (ProfileService.ProfileConfig.GameOption.SpeedType == 4
                  || ProfileService.ProfileConfig.GameOption.SpeedType == 6
                  || ProfileService.ProfileConfig.GameOption.SpeedType == 7) // S1.5 统合 무부, 통합
            {
                SpeedType.AddSpec_SteerConstraint = 1.95f;
                SpeedType.AddSpec_DriftEscapeForce = 400f;
                SpeedType.AddSpec_TransAccelFactor = 0.2005f;
                SpeedType.DragFactor = 0f;
                SpeedType.ForwardAccelForce = 0f;
                SpeedType.BackwardAccelForce = 0f;
                SpeedType.GripBrakeForce = 0f;
                SpeedType.SlipBrakeForce = 0f;
                SpeedType.SteerConstraint = 0f;
                SpeedType.DriftEscapeForce = 0f;
                SpeedType.CornerDrawFactor = 0f;
                SpeedType.DriftMaxGauge = 0f;
                SpeedType.TransAccelFactor = 0f;
                SpeedType.BoostAccelFactor = 0f;
                SpeedType.StartForwardAccelForceItem = 0f;
                SpeedType.StartForwardAccelForceSpeed = 0f;
                Console.WriteLine("SpeedType: 统合 S1.5");
            }
            else if (ProfileService.ProfileConfig.GameOption.SpeedType == 10) // 初级
            {
                SpeedType.AddSpec_SteerConstraint = 0f;
                SpeedType.AddSpec_DriftEscapeForce = 0f;
                SpeedType.AddSpec_TransAccelFactor = 0f;
                SpeedType.DragFactor = -0.01f;
                SpeedType.ForwardAccelForce = -150f;
                SpeedType.BackwardAccelForce = -225f;
                SpeedType.GripBrakeForce = -270f;
                SpeedType.SlipBrakeForce = -215f;
                SpeedType.SteerConstraint = -0.25f;
                SpeedType.DriftEscapeForce = -100f;
                SpeedType.CornerDrawFactor = 0.02f;
                SpeedType.DriftMaxGauge = -300f;
                SpeedType.TransAccelFactor = 0.0045f;
                SpeedType.BoostAccelFactor = 0.006f;
                SpeedType.StartForwardAccelForceItem = -270f;
                SpeedType.StartForwardAccelForceSpeed = -270f;
                Console.WriteLine("SpeedType: 初级");
            }
            else if (ProfileService.ProfileConfig.GameOption.SpeedType == 11) // 老S1/L3
            {
                SpeedType.AddSpec_SteerConstraint = 0f;
                SpeedType.AddSpec_DriftEscapeForce = 0f;
                SpeedType.AddSpec_TransAccelFactor = 0f;
                SpeedType.DragFactor = 0.013f;
                SpeedType.ForwardAccelForce = 250f;
                SpeedType.BackwardAccelForce = 225f;
                SpeedType.GripBrakeForce = 270f;
                SpeedType.SlipBrakeForce = 145f;
                SpeedType.SteerConstraint = 0.55f;
                SpeedType.DriftEscapeForce = 700f;
                SpeedType.CornerDrawFactor = 0.02f;
                SpeedType.DriftMaxGauge = 700f;
                SpeedType.TransAccelFactor = 0.0045f;
                SpeedType.BoostAccelFactor = 0.006f;
                SpeedType.StartForwardAccelForceItem = 450f;
                SpeedType.StartForwardAccelForceSpeed = 450f;
                Console.WriteLine("SpeedType: 老S1/L3");
            }
            else if (ProfileService.ProfileConfig.GameOption.SpeedType == 12) // 老S2/L2
            {
                SpeedType.AddSpec_SteerConstraint = 0f;
                SpeedType.AddSpec_DriftEscapeForce = 0f;
                SpeedType.AddSpec_TransAccelFactor = 0f;
                SpeedType.DragFactor = -0.007f;
                SpeedType.ForwardAccelForce = 350f;
                SpeedType.BackwardAccelForce = 375f;
                SpeedType.GripBrakeForce = 330f;
                SpeedType.SlipBrakeForce = 195f;
                SpeedType.SteerConstraint = 0.57f;
                SpeedType.DriftEscapeForce = 800f;
                SpeedType.CornerDrawFactor = 0.02f;
                SpeedType.DriftMaxGauge = 800f;
                SpeedType.TransAccelFactor = 0.0045f;
                SpeedType.BoostAccelFactor = 0.006f;
                SpeedType.StartForwardAccelForceItem = 400f;
                SpeedType.StartForwardAccelForceSpeed = 400f;
                Console.WriteLine("SpeedType: 老S2/L2");
            }
            else if (ProfileService.ProfileConfig.GameOption.SpeedType == 13) // 老S3/L1
            {
                SpeedType.AddSpec_SteerConstraint = 0f;
                SpeedType.AddSpec_DriftEscapeForce = 0f;
                SpeedType.AddSpec_TransAccelFactor = 0f;
                SpeedType.DragFactor = 0.051f;
                SpeedType.ForwardAccelForce = 750f;
                SpeedType.BackwardAccelForce = 450f;
                SpeedType.GripBrakeForce = 540f;
                SpeedType.SlipBrakeForce = 325f;
                SpeedType.SteerConstraint = 0.75f;
                SpeedType.DriftEscapeForce = 1100f;
                SpeedType.CornerDrawFactor = 0.02f;
                SpeedType.DriftMaxGauge = 1700f;
                SpeedType.TransAccelFactor = 0.0045f;
                SpeedType.BoostAccelFactor = 0.006f;
                SpeedType.StartForwardAccelForceItem = 1350f;
                SpeedType.StartForwardAccelForceSpeed = 1350f;
                Console.WriteLine("SpeedType: 老S3/L1");
            }
            else if (ProfileService.ProfileConfig.GameOption.SpeedType == 14) // Pro
            {
                SpeedType.AddSpec_SteerConstraint = 0f;
                SpeedType.AddSpec_DriftEscapeForce = 0f;
                SpeedType.AddSpec_TransAccelFactor = 0f;
                SpeedType.DragFactor = 0.06f;
                SpeedType.ForwardAccelForce = 1650f;
                SpeedType.BackwardAccelForce = 1125f;
                SpeedType.GripBrakeForce = 1350f;
                SpeedType.SlipBrakeForce = 865f;
                SpeedType.SteerConstraint = 1.15f;
                SpeedType.DriftEscapeForce = 2100f;
                SpeedType.CornerDrawFactor = 0.02f;
                SpeedType.DriftMaxGauge = 3700f;
                SpeedType.TransAccelFactor = 0.0045f;
                SpeedType.BoostAccelFactor = 0.006f;
                SpeedType.StartForwardAccelForceItem = 1800f;
                SpeedType.StartForwardAccelForceSpeed = 1800f;
                Console.WriteLine("SpeedType: Pro");
            }
            else // 错误的/未知的速度频道
            {
                GameSupport.OnDisconnect();
                Console.WriteLine("SpeedType: Null/Error/UnknownSpeedType");
            }
            FlyingPet.FlyingPet_Spec();
        }
    }
}
