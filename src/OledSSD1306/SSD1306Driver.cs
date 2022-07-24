/*********************************************************************************************
 * SSD1306 library - 
 * This C# library is orignally based on the original C/C++ Thingpulse Arduino SSD1306 library.
 * It has been redesigned to respect C#/.Net/Nanoframework best practices, constraint & optimization.
 * 
 * (c) Nicolas Clerc - Novembre 2020
 * 
 * 
 *-------- LICENSE
 * The MIT License (MIT)
 *
 * Original C library Copyright (c) 2020 by ThingPulse, Daniel Eichhorn
 * Copyright (c) 2020 by Nicolas CLERC for the C# port
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 *
 */


using System;
using System.Device.Gpio;
using System.Threading;
using System.Device.I2c;

namespace sablefin.nf.OledDisplay1306
{
    /// <summary>
    /// Constant for supperted 'color' on Oled screen
    /// </summary>
    public enum OledColor
    {
        Black = 0,
        White = 1,
        Inverse= 2
    };

    /// <summary>
    /// Define alignement of text for Text oriented method
    /// </summary>
    public enum TextAlignment
    {
        /// <summary>
        /// Text aligned on LEFT
        /// </summary>
        Left = 0,

        /// <summary>
        /// Text aligned on RIGHT
        /// </summary>
        Right = 1,

        /// <summary>
        /// Text CENTERED on the line
        /// </summary>
        Center = 2,

        /// <summary>
        /// Text vertically AND horizontally CENTERED - NOT SUPPORTED
        /// </summary>
        //CenterBoth = 3
    };


    public partial class SSD1306Driver : IDisposable
    {

        public byte DisplayWidth => _displayWidth;
        readonly byte _displayWidth = 128;

        public byte DisplayHeight => _displayHeight;
        readonly byte _displayHeight = 64;

        readonly int _displayBufferSize = 1024;

        readonly int _i2cPostCommandSleep=50; // Default in thingpulse code is 50ms. Heltec with onboard screen works fine with 0ms. 
        public int I2CPostCommandSleep { get { return _i2cPostCommandSleep; } }


        //public TextAlignment CurrentTextAlignement { get => _currentTextAlignement; set => _currentTextAlignement = value; }
        //TextAlignment _currentTextAlignement = TextAlignment.Left;


        public OledColor CurrentColor { get => _currentColor; set => _currentColor = value; }
        OledColor _currentColor = OledColor.White;

        /// <summary>
        /// I2C commands value used by the SSD1306 libraray
        /// </summary>
      
        byte[] displayBuffer = null;

        private I2cDevice i2cbus = null;
        private GpioPin resetPin = null;

        public SSD1306Driver(I2cDevice i2cbus, GpioPin resetPin, int i2cPostCommandSleep=50)
        {
            if (i2cbus == null)
                throw new ArgumentException("I2cDevice instance cannot be null.",nameof(i2cbus));
            this.i2cbus = i2cbus;
            this.resetPin = resetPin;
        }


        public void Init() 
        {
            if (displayBuffer == null)
                displayBuffer = new byte[_displayBufferSize];
            Clear();
            ResetDisplay(); 
            SendInitCommand();
            RefreshDisplay();
            DisplayOn();
        }
        
        
        /// <summary>
        /// This test methid fill the display buffer with random value
        /// 0: random pixel fill 
        /// 1: alternate vertical line
        /// 2: alternate thick vertical line
        /// 3:thin horizontal line
        /// 4: first 1288 byte of display buffer set to 128+16 (usefull for origin align test)
        /// </summary>
        public void TestFill(int pattern=0)
        {
            Clear();
            switch (pattern)
            {
                case 0: // random fill of display buffer
                    Random rnd = new Random((int)DateTime.UtcNow.Ticks);
                    rnd.NextBytes(displayBuffer);
                    break;
                case 1: // alternate vertical line 
                    for (int i = 0; i < displayBuffer.Length;)
                    {
                        displayBuffer[i++] = 0;
                        displayBuffer[i++] = 255;
                    }
                    break;
                case 2: // alternate thick vertical line
                    for (int i = 0; i < displayBuffer.Length;)
                    {
                        displayBuffer[i++] = 0; displayBuffer[i++] = 0; displayBuffer[i++] = 0; displayBuffer[i++] = 0;
                        displayBuffer[i++] = 255; displayBuffer[i++] = 255; displayBuffer[i++] = 255; displayBuffer[i++] = 255;
                    }
                    break;
                case 3: // thin horizontal line
                    for (int i = 0; i < displayBuffer.Length;i++)
                    {
                        displayBuffer[i++] = 16; 
                    }
                    break;
                case 4: // first 128 byte set to same value
                    for (int i = 0; i < 128; i++)
                    {
                        //displayBuffer[1023 - i] = 255;
                        displayBuffer[i] = 128+16;
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid pattern value", "pattern");
            }
        }

        /// <summary>
        /// Switching off/on eletrical power of oled screen. 
        /// It gpio reset pin not provider, nothing happens.
        /// </summary>
        public void Reset()
        {
            resetPin?.Write(PinValue.Low);
            Thread.Sleep(100);
            resetPin?.Write(PinValue.High);
            Thread.Sleep(100);
        }


        /// <summary>
        /// Clear the display buffer (set all pixel to 0)
        /// </summary>
        public void Clear()
        {
            for (int n = 0; n < displayBuffer.Length; n++)
                displayBuffer[n] = 0;
        }
        /// <summary>
        /// Send the current display buffer to oled.
        /// </summary>
        public void RefreshDisplay()
        {
            byte x_offset = (byte)((128 - _displayWidth) / 2);

            SendI2CCommand(Commands.ColumnAddress);
            SendI2CCommand(x_offset);
            SendI2CCommand((byte)(x_offset + (_displayWidth - 1 )));
            

            SendI2CCommand(Commands.PageAddress);
            SendI2CCommand(0x00);
            SendI2CCommand((byte)((_displayHeight/8)-1));

            //if (geometry == GEOMETRY_128_64)
            //{
            //    SendI2CCommand(0x7); // BUGGY ON WIFIKIT32 !!!!
            //}
            //else if (geometry == GEOMETRY_128_32)
            //{
            //    SendI2CCommand(0x3);
            //}
            SendI2CCommand(0x0); // VALID VALUE FOR WIFIKIT32


            byte[] img = new byte[displayBuffer.Length+1];
            img[0] = (Byte)Commands.SetStartLine;
            Array.Copy(displayBuffer, 0, img, 1, displayBuffer.Length);
            Thread.Sleep(_i2cPostCommandSleep);
            i2cbus.Write(img); // Send the display buffer to the screen
        }


        private void ResetDisplay()
        {
            resetPin?.Write(PinValue.Low);
            Thread.Sleep(_i2cPostCommandSleep);
            resetPin?.Write(PinValue.High);
            Thread.Sleep(_i2cPostCommandSleep);
        }



        /// <summary>
        /// Send I2C command to the current i2c bus
        /// TO OPTIMIZE TO AVOID MEMORY ALLOCATION ON EACH CALL !!!
        /// </summary>
        /// <param name="cmd"></param>
        private void SendI2CCommand(byte cmd)
        {
            i2cbus.Write(new byte[] { 0x00, cmd }); // TODO: replace with a preallocated buffer
            Thread.Sleep(_i2cPostCommandSleep);
        }



        /// <summary>
        /// Send the initialization sequence of command to active the oled screen.
        /// Display remain OFF (you need to explicitly call DisplayOn() )
        /// </summary>
        private void SendInitCommand()
        {
            SendI2CCommand(Commands.DisplayOff);
            SendI2CCommand(Commands.SetDisplayClockDiv);
            SendI2CCommand(0xF0); // Increase speed of the display max ~96Hz
            SendI2CCommand(Commands.SetMultiplex);
            SendI2CCommand((byte)(_displayHeight - 1));
            SendI2CCommand(Commands.SetDisplayOffSet);
            SendI2CCommand(0x00);
            SendI2CCommand(Commands.SetStartLine);
            SendI2CCommand(Commands.ChargePump);
            SendI2CCommand(0x14);
            SendI2CCommand(Commands.MemoryMode);
            SendI2CCommand(0x00);
            SendI2CCommand(Commands.SegRemap);
            SendI2CCommand(Commands.ComScanInc);
            SendI2CCommand(Commands.SetComPins);
            //if ((geometry == GEOMETRY_128_64) || (geometry == GEOMETRY_64_32)) {
            SendI2CCommand(0x12);
            // } else if (geometry == GEOMETRY_128_32) {
            //SendI2CCommand(0x02);
            //}

            SendI2CCommand(Commands.SetContrast);

            //if ((geometry == GEOMETRY_128_64) || (geometry == GEOMETRY_64_32)){
            //    sendCommand(0xCF);
            SendI2CCommand(0xCF);
            //} else if (geometry == GEOMETRY_128_32){
            //    SendI2CCommand(0x8F);
            //}

            SendI2CCommand(Commands.SetPreCharge);
            SendI2CCommand(0xF1);

            SendI2CCommand(Commands.SetVComDetect);
            SendI2CCommand(0x40);    //0x40 default, to lower the contrast, put 0

            SendI2CCommand(Commands.DisplayAllOnResume);
            SendI2CCommand(Commands.NormalDisplay);
            SendI2CCommand(0x2e); // Stop scroll
            //SendI2CCommand(Commands.DisplayOn); // commented to avoid screen on with jam filling
        }

        /// <summary>
        /// sleep display ???
        /// </summary>
        public void Sleep()
        {
            SendI2CCommand(0x8D);
            SendI2CCommand(0x10);
            SendI2CCommand(0xAE);
        }

        /// <summary>
        /// Wakeup display ???
        /// </summary>
        public void WakeUp()
        {
            SendI2CCommand(0x8D);
            SendI2CCommand(0x14);
            SendI2CCommand(0xAF);
        }

        /// <summary>
        /// Activate display
        /// </summary>
        public void DisplayOn()
        {
            SendI2CCommand(Commands.DisplayOn);
        }

        /// <summary>
        /// Deactivate display 
        /// </summary>
        public void DisplayOff()
        {
            SendI2CCommand(Commands.DisplayOff);
        }


        /// <summary>
        /// Invert display color (pixel set to 0 = light on)
        /// </summary>
        public void InvertDisplay()
        {
            SendI2CCommand(Commands.InvertDisplay);
        }

        /// <summary>
        /// Set normal display color (pixel set to 0 = light off)
        /// </summary>
        public void NormalDisplay()
        {
            SendI2CCommand(Commands.NormalDisplay);
        }



        /// <summary>
        /// Define the oled crontrast & brightness
        /// Really low brightness & contrast: contrast = 10, precharge = 5, comdetect = 0
        /// normal brightness & contrast:  contrast = 100
        /// To reset contrast : call the function with all parameters default value.
        /// </summary>
        /// <param name="contrastValue">0 to 255. Default is 0xCF</param>
        /// <param name="precharge">1 to 0x1F for low contrast, 0xF1 default. </param>
        /// <param name="comdetect">0 for low contract, Default is 0x40</param>
        public void SetContrast(byte contrastValue=0xCF,byte precharge=0xF1,byte comdetect=0x40)
        {
            SendI2CCommand(Commands.SetPreCharge);
            SendI2CCommand(precharge); //0xF1 default, to lower the contrast, put 1-1F
            SendI2CCommand(Commands.SetContrast);
            SendI2CCommand(contrastValue); // 0-255
            SendI2CCommand(Commands.SetVComDetect); //(additionally needed to lower the contrast)
            SendI2CCommand(comdetect); //0x40 default, to lower the contrast, put 0
        }

        /// <summary>
        /// Define brightness
        /// </summary>
        /// <param name="brightness"></param>
        public void SetBrightness(byte brightness)
        {
            byte contrast;
            if (brightness < 128)
            {
                // Magic values to get a smooth/ step-free transition
                contrast = (byte)(brightness * 1.171);
            }
            else
                contrast = (byte)(brightness * 1.171 - 43);

            SetContrast(contrast, (brightness==0)? (byte)0 : (byte)241,(byte)(brightness / 8));
        }

        /// <summary>
        /// Restore native screen orientation/morroring 
        /// </summary>
        public void ResetOrientation()
        {
            SendI2CCommand(Commands.SegRemap);
            SendI2CCommand(Commands.ComScanInc); //Reset screen rotation or mirroring
        }

        /// <summary>
        /// Flip screen vertically
        /// </summary>
        public void FlipScreenVertically()
        {
            SendI2CCommand(Commands.SegRemap | 0x01);
            SendI2CCommand(Commands.ComScanDec);  //Rotate screen 180 Deg
        }

        public void MirrorScreen()
        {
            SendI2CCommand(Commands.SegRemap);
            SendI2CCommand(Commands.ComScanDec); //Mirror screen
        }

        public void SetLogBuffer(UInt16 lines,UInt16 chars)
        {
            throw new NotImplementedException();
        }

        public void DrawLogBuffer(UInt16 x, UInt16 y)
        {
            throw new NotImplementedException();
        }



        public void Dispose()
        {
            //TODO: TO IMPLEMENT !!!
        }
    }
}
