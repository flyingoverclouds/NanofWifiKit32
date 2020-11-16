using System;
using System.Threading;
using System.Diagnostics;
using System.Device.Gpio;
using HeltecLib;
using sablefin.nf.OledDisplay1306;
using sablefin.nf.OledFonts;

using sablefin.nf.WifiKit32Common; // comment/uncomment this using when targeting Heltec WifiKit32 v2
//using sablefin.nf.WifiLora32; // comment/uncomment this using when targeting Heltec WifiLORA32 v2

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
            oledScreen.CurrentFont = FontArialMTPlain10.GetFont();
            int w = 0;
            int x = 1;
            w = oledScreen.DrawChar(x += w, 1, 'N');
            w = oledScreen.DrawChar(x += w, 1, 'i');
            w = oledScreen.DrawChar(x += w, 1, 'C');
            w = oledScreen.DrawChar(x += w, 1, 'o');

            oledScreen.CurrentFont = FontArialMTPlain16.GetFont();
            w = 0;
            x = 1;
            w = oledScreen.DrawChar(x += w, 11, 'n');
            w = oledScreen.DrawChar(x += w, 11, 'I');
            w = oledScreen.DrawChar(x += w, 11, 'c');
            w = oledScreen.DrawChar(x += w, 11, '0');

            oledScreen.CurrentFont = FontArialMTPlain24.GetFont();
            w = 0;
            x = 1;
            w = oledScreen.DrawChar(x += w, 30, 'N');
            w = oledScreen.DrawChar(x += w, 30, '1');
            w = oledScreen.DrawChar(x += w, 30, 'c');
            w = oledScreen.DrawChar(x += w, 30, 'o');


        }
        public static void Main()
        {
            
            Debug.WriteLine("[HellOled] : a hello word with the embedded OLED screen.");

            // Heltec oled TEST
            var heltec = new HeltecOled();
            heltec.Begin();
            //heltec.Display.SetContrast(20);
            heltec.Display.SetBrightness(180);

            //heltec.Display.InvertDisplay();
            heltec.Display.FlipScreenVertically();
            heltec.Display.CurrentColor = OledColor.White;


            // BLINK LED TO WAIT
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
                        //heltec.Display.TestFill(4);
                        heltec.Display.TestFill(0);
                        break;
                    //case 1:
                    //    heltec.Display.Clear();
                    //    heltec.Display.TestFill(1);
                    //    break;
                    //case 2:
                    //    heltec.Display.Clear();
                    //    heltec.Display.TestFill(2);
                    //    break;
                    //case 3:
                    //    heltec.Display.Clear();
                    //    heltec.Display.TestFill(3);
                    //    break;
                    case 1:
                        heltec.Display.Clear();
                        DemoGeometry(heltec.Display);
                        break;
                    case 2:
                        heltec.Display.Clear();
                        DemoScreen2(heltec.Display);
                        break;
                    case 3:
                        heltec.Display.Clear();
                        DemoScreen3(heltec.Display);
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
