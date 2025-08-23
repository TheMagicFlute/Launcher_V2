using System;
using KartRider;

namespace ExcData
{
    public class SpeedPatch
    {
        public static float DragFactor = 0f;                    // 最高速度 최고 속도
        public static float ForwardAccelForce = 0f;             // 前进加速度 전진 가속도
        public static float DriftEscapeForce = 0f;              // 甩尾脱离力 드리프트 탈출력
        public static float CornerDrawFactor = 0f;              // 弯道加速 코너 가속
        public static float DriftMaxGauge = 0f;                 // 氮气储存量 게이지 충전량
        public static float TransAccelFactor = 0f;              // 变形加速器加速力 변신 부스터 가속력
        public static float BoostAccelFactor = 0f;              // 加速器加速力 부스터 가속력
        public static float StartForwardAccelForceItem = 0f;    // 道具启动加速器加速力 출발 부스터 가속 아이템
        public static float StartForwardAccelForceSpeed = 0f;   // 竞速启动加速器加速力 출발 부스터 가속 스피드

        public static void SpeedPatcData()
        {
            if (Program.SpeedPatch)
            {
                SpeedPatch.DragFactor = -0.003f;
                SpeedPatch.ForwardAccelForce = 30f;
                SpeedPatch.DriftEscapeForce = 200f;
                SpeedPatch.CornerDrawFactor = 0.0015f;
                SpeedPatch.DriftMaxGauge = -70f;
                SpeedPatch.TransAccelFactor = 0.005f;
                SpeedPatch.BoostAccelFactor = 0.005f;
                SpeedPatch.StartForwardAccelForceItem = 100f;
                SpeedPatch.StartForwardAccelForceSpeed = 100f;
            }
            else
            {
                SpeedPatch.DragFactor = 0f;
                SpeedPatch.ForwardAccelForce = 0f;
                SpeedPatch.DriftEscapeForce = 0f;
                SpeedPatch.CornerDrawFactor = 0f;
                SpeedPatch.DriftMaxGauge = 0f;
                SpeedPatch.TransAccelFactor = 0f;
                SpeedPatch.BoostAccelFactor = 0f;
                SpeedPatch.StartForwardAccelForceItem = 0f;
                SpeedPatch.StartForwardAccelForceSpeed = 0f;
            }
        }
    }
}
