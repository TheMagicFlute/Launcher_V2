using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace KartLibrary.Record
{
    public struct RecordStamp
    {
        public RecordStamp()
        {
            Angle = new Quaternion();
        }

        public int Time { get; set; } = -1; // Unit: ms
        public float X { get; set; } = 0f;
        public float Y { get; set; } = 0f;
        public float Z { get; set; } = 0f;
        public Quaternion Angle { get; set; }
        public ushort Status { get; set; } = 0;
        public bool IsInitialObject => Time == -1;
        public string[] GetCarStatus()
        {
            string[] gasStatus = ["", "红气", "蓝气", "短喷", "开前喷", "gas(101)", "gas(110)", "开喷火"];
            string[] characterStatus = ["", "左摆头", "右摆头", "闪到头", "倒退头", "倒左头", "倒右头", "撞到头"];
            string[] effectStatus = ["", "加速", "漂移", "加速&漂移"];
            List<string> output = new List<string>();
            if (gasStatus[Status & 7] != "")
                output.Add(gasStatus[Status & 7]);
            if (characterStatus[(Status >> 3) & 7] != "")
                output.Add(characterStatus[(Status >> 3 & 7)]);
            if (effectStatus[(Status >> 6) & 3] != "")
                output.Add(effectStatus[(Status >> 6 & 3)]);
            return output.ToArray();
        }
    }
}
