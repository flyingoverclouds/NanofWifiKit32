using System;
using System.Threading;
using System.Diagnostics;
using System.Device.Gpio;
using nanoFramework.Hardware.Esp32;
using Windows.Devices.I2c;
using HeltecLib;
using nanoframework.OledDisplay1306;

namespace HellOled
{
    public class Program
    {
        static void DemoGeometry(SSD1306Driver oledScreen)
        {
            //for (short i = 20; i < 108; i+=4)
            //    oledScreen.SetPixel(i, 0);
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
            GpioPin led = gpioc.OpenPin(WifiKit32Common.OnBoardDevicePortNumber.Led, PinMode.Output);
            led.Write(PinValue.Low);


            
            while (true)
            {
                switch(counter%2)
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
                }
                heltec.Display.RefreshDisplay();

                led.Toggle();
                if (counter > 10000)
                    counter = 0;

                Thread.Sleep(1000);
                counter++;
            }
        }
    }
}
