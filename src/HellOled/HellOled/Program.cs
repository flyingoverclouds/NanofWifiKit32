using System;
using System.Threading;
using System.Diagnostics;
using System.Device.Gpio;
using nanoFramework.Hardware.Esp32;
using Windows.Devices.I2c;

namespace HellOled
{
    public class Program
    {

        public static void Main()
        {
            Debug.WriteLine("[HellOled] : a hello word with the embedded OLED screen.");

            // Heltec oled TEST
            var heltec = new HeltecOled();
            heltec.Begin();
            heltec.Display.SetContrast(0);

            // BLINK LED TO WAIT
            int counter = 0;
            GpioController gpioc = new GpioController();
            GpioPin led = gpioc.OpenPin(WifiKit32Common.OnBoardDevicePortNumber.Led, PinMode.Output);
            led.Write(PinValue.Low);
            
            while (true)
            {
                if (counter%2==0)
                    heltec.Display.TestFill(3);
                else
                    heltec.Display.TestFill(2);
                heltec.Display.RefreshDisplay();

                led.Toggle();
                if (counter > 10000)
                    counter = 0;

                //Debug.WriteLine(counter.ToString());
                Thread.Sleep(2000);
                counter++;
            }
        }
    }
}
