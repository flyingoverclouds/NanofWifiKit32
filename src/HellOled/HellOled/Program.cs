/* Demo program for the Oled1306 & heltec helper library
 * 
 ***************************************************************************************
 * 
 * MIT License
 *
 * Copyright (c) 2020 Nicolas CLERC
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
using System.Threading;
using System.Diagnostics;
using System.Device.Gpio;
using HeltecHelper;

using sablefin.nf.OledDisplay1306;
using sablefin.nf.OledFonts;

//using sablefin.nf.WifiKit32Common; // comment/uncomment this using when targeting Heltec WifiKit32 v2
using sablefin.nf.WifiLora32; // comment/uncomment this using when targeting Heltec WifiLORA32 v2

namespace HellOled
{
    public class Program
    {
        static XbmImage wifiLogo = null;
        static XbmImage nanofLogo = null;

        static void DemoGeometry(SSD1306Driver oledScreen)
        {
            oledScreen.DrawLine(0, 0, oledScreen.DisplayWidth - 1, oledScreen.DisplayHeight - 1);
            oledScreen.DrawLine(0, oledScreen.DisplayHeight - 1, oledScreen.DisplayWidth - 1, 0);
            oledScreen.DrawHorizontalLine(20, 32, 88);
            oledScreen.DrawVerticalLine(64, 10, 44);
            oledScreen.DrawRect(30, 15, 68, 34);
            oledScreen.CurrentColor = OledColor.Inverse;
            oledScreen.FillRect(2, 15, 20, 34);
            oledScreen.FillRect(106, 15, 20, 34);
            oledScreen.DrawCircleQuads(30, 0, 30, 0b0100);
            oledScreen.DrawCircleQuads(98, 0, 30, 0b1000);
            oledScreen.DrawCircleQuads(30, 64, 30, 0b0010);
            oledScreen.DrawCircleQuads(98, 64, 30, 0b0001);
            oledScreen.FillCircle(64, 31, 12);
            oledScreen.CurrentColor = OledColor.White;
            oledScreen.DrawCircle(64, 31, 30);
        }

        static void DemoScreen2(SSD1306Driver oledScreen)
        {
            Random rnd = new Random();
            oledScreen.DrawProgressBar(5, 5, 118, 10, rnd.Next(100));
            oledScreen.DrawXbm(64, 22, wifiLogo.Width, wifiLogo.Height, wifiLogo.Datas); // legacy lib signature
            oledScreen.DrawXbm(20, 22, nanofLogo); // modern dotnet signature
        }

        static void DemoScreen3(SSD1306Driver oledScreen)
        {
            oledScreen.Clear();
            oledScreen.CurrentTextAlignement = TextAlignment.Left;

            oledScreen.CurrentFont = FontArialMTPlain10.GetFont();
            oledScreen.DrawString(1, 0, "NiCo");

            oledScreen.CurrentFont = FontArialMTPlain16.GetFont();
            oledScreen.DrawString(1, 11, "nIc0");

            oledScreen.CurrentFont = FontArialMTPlain24.GetFont();
            oledScreen.DrawString(1, 30, "N1co");
        }

        static void DemoScreen4(SSD1306Driver oledScreen)
        {
            oledScreen.Clear();
            oledScreen.CurrentFont = FontArialMTPlain10.GetFont();
            oledScreen.DrawHorizontalLine(0, 32, 128);
            oledScreen.DrawVerticalLine(64, 0, 64);
            oledScreen.CurrentTextAlignement = TextAlignment.Left;
            oledScreen.DrawString(0,0,"DrawString()\rLine 1\nLine 2\r\nLine 3");
            oledScreen.CurrentTextAlignement = TextAlignment.Right;
            oledScreen.DrawString(128, 0, "DrawString()\rLine 1\nLine 2\r\nLine 3");
            oledScreen.CurrentTextAlignement = TextAlignment.Center;
            oledScreen.DrawString(64, 0, "< >\r-< >-\roO0[]0Oo\r/ \\\r\\_ _/");
        }

        public static void Main()
        {
            Debug.WriteLine("[HellOled] : a advanced hello word with the embedded OLED screen.");

            var heltec = new HeltecOled();
            heltec.Begin();
            heltec.Display.SetBrightness(180);
            heltec.Display.FlipScreenVertically();
            heltec.Display.CurrentColor = OledColor.White;


            int counter = 0;
            GpioController gpioc = new GpioController();
            GpioPin led = gpioc.OpenPin(OnBoardDevicePortNumber.Led, PinMode.Output);
            led.Write(PinValue.Low);

            wifiLogo = XBMSamples.GetWifiLogoXBM();
            nanofLogo = XBMSamples.GetNanoFrameworkXBM();

            heltec.Display.CurrentFont = FontArialMTPlain10.GetFont();

            while (true)
            {
                switch(counter)
                {
                    case 0:
                        heltec.Display.Clear();
                        heltec.Display.TestFill(0);
                        break;
                    case 1:
                        heltec.Display.Clear();
                        DemoGeometry(heltec.Display);
                        break;
                    case 2:
                        heltec.Display.Clear();
                        DemoScreen2(heltec.Display);
                        break;
                    case 3:
                        DemoScreen3(heltec.Display);
                        break;
                    case 4:
                        DemoScreen4(heltec.Display);
                        break;
                    default:
                        counter = -1; // there is the ++ at the end of loop
                        break;
                }

                heltec.Display.RefreshDisplay();
                led.Toggle();
                Thread.Sleep(1000);
                counter++;
            }
        }
    }
}
