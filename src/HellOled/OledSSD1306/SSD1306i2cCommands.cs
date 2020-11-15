using System;

namespace nanoframework.OledDisplay1306
{
    class Commands
    {
        public const byte ChargePump = 0x8D;
        public const byte ColumnAddress = 0x21;
        public const byte ComScanDec = 0xC8;
        public const byte ComScanInc = 0xC0;
        public const byte DisplayAllOn = 0xA5;
        public const byte DisplayAllOnResume = 0xA4;
        public const byte DisplayOff = 0xAE;
        public const byte DisplayOn = 0xAF;
        public const byte ExternalVCC = 0x01;
        public const byte InvertDisplay = 0xA7;
        public const byte MemoryMode = 0x20;
        public const byte NormalDisplay = 0xA6;
        public const byte PageAddress = 0x22;
        public const byte SegRemap = 0xA0;
        public const byte SetComPins = 0xDA;
        public const byte SetContrast = 0x81;
        public const byte SetDisplayClockDiv = 0xD5;
        public const byte SetDisplayOffSet = 0xD3;
        public const byte SetHighColumn = 0x10;
        public const byte SetLowColumn = 0x00;
        public const byte SetMultiplex = 0xA8;
        public const byte SetPreCharge = 0xD9;
        public const byte SetSegmentRemap = 0xA1;
        public const byte SetStartLine = 0x40;
        public const byte SetVComDetect = 0xDB;
        public const byte SetSwitchCapVCC = 0x02;


        //DisplayRatio = 0x80,
        //NoOffSet = 0x00,
        //VCCState = 0x14,
        //LowColumn = 0x00,
        //DisableLRRemap = 0x12,
        //NoExternalVcc = 0xCF,
        //InternalDC = 0xF1,
        //ComDetect = 0xD8,
        //SetComDetect = 0x40,
        //DeactivateScroll = 0x2E,
        //Reset = 0x00,
        //PageEndAddress = 0x37
    }
}
