using System;
using System.Reflection;

namespace WifiKit32Common
{
    static public class OnBoardOled
    {
        public const int ScreenWidth = 128;
        public const int ScrenHeight = 64;
        public const byte I2CAddress = 0x3c;
        public const int Data = OnBoardDevicePortNumber.OledSDA;
        public const int Clock = OnBoardDevicePortNumber.OledSCL;
        public const int Reset = OnBoardDevicePortNumber.OledRST;
        public const int VExt = OnBoardDevicePortNumber.OledVExt;
    }
}
