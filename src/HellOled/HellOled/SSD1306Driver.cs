using System;
using System.Device.Gpio;
using System.Threading;
using Windows.Devices.I2c;

namespace HellOled
{
    class SSD1306Driver
    {
        const byte DisplayWidth = 128;
        const byte DisplayHeight = 64;
        const int DisplayBufferSize = 1024;

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
            //NoOffSet = 0x0,
            //VCCState = 0x14,
            //LowColumn = 0x0,
            //DisableLRRemap = 0x12,
            //NoExternalVcc = 0xCF,
            //InternalDC = 0xF1,
            //ComDetect = 0xD8,
            //SetComDetect = 0x40,
            //DeactivateScroll = 0x2E,
            //Reset = 0x0,
            //PageEndAddress = 0x37
        }

        byte[] displayBuffer =null;

        private I2cDevice i2cbus = null;
        private GpioPin resetPin=null;

        public SSD1306Driver(I2cDevice i2cbus,GpioPin resetPin)
        {
            if (i2cbus == null)
                throw new ArgumentException("I2cDevice instance cannot be null.",nameof(i2cbus));
            this.i2cbus = i2cbus;
            this.resetPin = resetPin;
        }


        public void Init() 
        {
            if (displayBuffer == null)
                displayBuffer = new byte[DisplayBufferSize];
            Clear();
            ResetDisplay(); 
            SendInitCommand();
            RefreshDisplay();
            DisplayOn();
        }
        
        public void FlipScreenVertically()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This test methid fill the display buffer with random value
        /// </summary>
        public void TestFill(int pattern=0)
        {
            Clear();
            switch (pattern)
            {
                case 0:
                    Random rnd = new Random((int)DateTime.UtcNow.Ticks);
                    rnd.NextBytes(displayBuffer);
                    break;
                case 1:
                    for (int i = 0; i < displayBuffer.Length;)
                    {
                        displayBuffer[i++] = 0;
                        displayBuffer[i++] = 255;
                    }
                    break;
                case 2:
                    for (int i = 0; i < displayBuffer.Length;)
                    {
                        displayBuffer[i++] = 0; displayBuffer[i++] = 0; displayBuffer[i++] = 0; displayBuffer[i++] = 0;
                        displayBuffer[i++] = 255; displayBuffer[i++] = 255; displayBuffer[i++] = 255; displayBuffer[i++] = 255;
                    }
                    break;
                case 3:
                    for (int i = 0; i < displayBuffer.Length;i++)
                    {
                        displayBuffer[i++] = 16; 
                    }
                    break;

                default:
                    throw new ArgumentException("Pattern value is : 0 --> 3", "pattern");
            }
        }

        /// <summary>
        /// Clear the display buffer.
        /// </summary>
        public void Clear()
        {
            // TODO : implement a optimized fillin
            for (int n = 0; n < displayBuffer.Length; n++)
                displayBuffer[n] = 0;
        }

        public void DisplayOn()
        {
            SendI2CCommand(Commands.DisplayOn);
        }

        public void DisplayOff()
        {
            SendI2CCommand(Commands.DisplayOff);
        }

        /// <summary>
        /// Send the current display buffer to oled.
        /// </summary>
        public void RefreshDisplay()
        {
            byte x_offset = (128 - DisplayWidth) / 2;

            SendI2CCommand(Commands.ColumnAddress);
            SendI2CCommand(x_offset);
            SendI2CCommand((byte)(x_offset + (DisplayWidth - 1 )));

            SendI2CCommand(Commands.PageAddress);
            SendI2CCommand(0x00);
            SendI2CCommand((DisplayHeight/8)-1);

            //if (geometry == GEOMETRY_128_64)
            //{
            SendI2CCommand(0x7);
            //}
            //else if (geometry == GEOMETRY_128_32)
            //{
            //    sendCommand(0x3);
            //}

            byte[] img = new byte[displayBuffer.Length+1];
            img[0] = (Byte)Commands.SetStartLine;
            Array.Copy(displayBuffer, 0, img, 1, displayBuffer.Length);
            //Thread.Sleep(50);
            i2cbus.Write(img); // Send the display buffer to the screen
        }


        public void SetContrast(byte contrastValue)
        {
            if (contrastValue > 0x40)
                throw new ArgumentException("Invalid contrast value : must be between 0(low) and 0x40(high)", nameof(contrastValue));
            SendI2CCommand(Commands.SetVComDetect);
            SendI2CCommand(contrastValue);    
        }

            private void ResetDisplay()
        {
            resetPin?.Write(PinValue.Low);
            Thread.Sleep(50);
            resetPin?.Write(PinValue.High);
            Thread.Sleep(50);
        }



        /// <summary>
        /// Send I2C command to the current i2c bus
        /// TO OPTIMIZE TO AVOID MEMORY ALLOCATION ON EACH CALL !!!
        /// </summary>
        /// <param name="cmd"></param>
        private void SendI2CCommand(byte cmd)
        {
            i2cbus.Write(new byte[] { 0x00, cmd }); // TODO: replace with a preallocated buffer
            Thread.Sleep(50);
        }



        /// <summary>
        /// Send the initialization sequence of command to active the oled screen.
        /// </summary>
        private void SendInitCommand()
        {
            //sendCommand(DISPLAYOFF);
            SendI2CCommand(Commands.DisplayOff);

            //sendCommand(SETDISPLAYCLOCKDIV);
            SendI2CCommand(Commands.SetDisplayClockDiv);
            //sendCommand(0xF0); // Increase speed of the display max ~96Hz
            SendI2CCommand(0xF0); // Increase speed of the display max ~96Hz

            //sendCommand(SETMULTIPLEX);
            SendI2CCommand(Commands.SetMultiplex);
            //sendCommand(this->height() - 1);
            SendI2CCommand(DisplayHeight - 1);

            //sendCommand(SETDISPLAYOFFSET);
            SendI2CCommand(Commands.SetDisplayOffSet);
            //sendCommand(0x00);
            SendI2CCommand(0x00);

            //sendCommand(SETSTARTLINE);
            SendI2CCommand(Commands.SetStartLine);

            //sendCommand(CHARGEPUMP);
            SendI2CCommand(Commands.ChargePump);
            //sendCommand(0x14);
            SendI2CCommand(0x14);

            //sendCommand(MEMORYMODE);
            SendI2CCommand(Commands.MemoryMode);
            //sendCommand(0x00);
            SendI2CCommand(0x00);

            //sendCommand(SEGREMAP);
            SendI2CCommand(Commands.SegRemap);

            //sendCommand(COMSCANINC);
            SendI2CCommand(Commands.ComScanInc);

            //sendCommand(SETCOMPINS);
            SendI2CCommand(Commands.SetComPins);
            //if ((geometry == GEOMETRY_128_64) || (geometry == GEOMETRY_64_32))
            //    sendCommand(0x12);
            SendI2CCommand(0x12);

            //sendCommand(SETCONTRAST);
            SendI2CCommand(Commands.SetContrast);
            //if ((geometry == GEOMETRY_128_64) || (geometry == GEOMETRY_64_32))
            //{
            //    sendCommand(0xCF);
            SendI2CCommand(0xCF);
            //}
            //else if (geometry == GEOMETRY_128_32)
            //{
            //    sendCommand(0x8F);
            //}

            //sendCommand(SETPRECHARGE);
            SendI2CCommand(Commands.SetPreCharge);
            //sendCommand(0xF1);
            SendI2CCommand(0xF1);

            //sendCommand(SETVCOMDETECT); //0xDB, (additionally needed to lower the contrast)
            SendI2CCommand(Commands.SetVComDetect);
            //sendCommand(0x40);          
            SendI2CCommand(0x40);    //0x40 default, to lower the contrast, put 0

            //sendCommand(DISPLAYALLON_RESUME);
            SendI2CCommand(Commands.DisplayAllOnResume);
            //sendCommand(NORMALDISPLAY);
            SendI2CCommand(Commands.NormalDisplay);
            //sendCommand(0x2e);            // stop scroll
            SendI2CCommand(0x2e); // Stop scroll
            //sendCommand(DISPLAYON);
            //SendI2CCommand(Commands.DisplayOn);
        }


    }
}
