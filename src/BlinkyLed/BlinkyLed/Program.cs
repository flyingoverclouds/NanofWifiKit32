using System;
using System.Threading;
using System.Diagnostics;

using System.Device.Gpio;


namespace BlinkyLed
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("WifiKit32 NF Blinky");
            int counter = 0;
            
            GpioController gpioc = new GpioController();
            GpioPin led = gpioc.OpenPin(WifiKit32Common.OnBoardDevicePortNumber.Led,PinMode.Output); 
            led.Write(PinValue.Low);

            while(true)
            {
                led.Toggle();
                if (counter > 10000)
                    counter = 0;

                Debug.WriteLine(counter.ToString());
                Thread.Sleep(1000);
                counter++;
            }
        }
    }
}
