using nanoFramework.Hardware.Esp32;
using System;
using System.Device.Gpio;
using System.Threading;
using System.Device.I2c;
using sablefin.nf.OledDisplay1306;
using sablefin.nf.WifiKit32Common;

namespace HeltecHelper
{
    class HeltecOled 
    {
        GpioPin oledVext=null;
        GpioPin oledReset = null;
        I2cDevice i2cBusSSD1306 = null;

        SSD1306Driver ssd1306;
        public SSD1306Driver Display
        {
            get { return ssd1306; }
        }

        public HeltecOled()
        {
            GpioController gpioc = new GpioController();
            // POWER-ON ONBOARD SCREEN
            oledVext = gpioc.OpenPin(OnBoardDevicePortNumber.OledVExt, PinMode.Output);
            oledReset = gpioc.OpenPin(OnBoardOled.Reset, PinMode.Output);
        }


        public void Begin()
        {
            this.PowerON();
            this.Reset();

            // Configuration of I2C1 bus for onboard LED
            Configuration.SetPinFunction(OnBoardOled.Data, DeviceFunction.I2C1_DATA);
            Configuration.SetPinFunction(OnBoardOled.Clock, DeviceFunction.I2C1_CLOCK);


            i2cBusSSD1306 = I2cDevice.Create(new I2cConnectionSettings(1, OnBoardOled.I2CAddress, I2cBusSpeed.FastMode));


            ssd1306 = new SSD1306Driver(i2cBusSSD1306,oledReset,50 /* Heltec onboard oled support 0ms */);

            ssd1306.Init();
            //ssd1306.FlipScreenVertically();
            ssd1306.RefreshDisplay();
        }

        public void PowerON()
        {
            oledVext?.Write(PinValue.Low); // based on Heltec.cpp:Heltec_ESP32::VextON()
            Thread.Sleep(100);
        }

        public void PowerOFF()
        {
            oledVext?.Write(PinValue.Low); // based on Heltec.cpp:Heltec_ESP32::VextON()
        }

        /// <summary>
        /// Reset the onboard oled screen by switching off/on the reset pin.
        /// </summary>
        public void Reset()
        {
            oledReset?.Write(PinValue.Low);
            Thread.Sleep(100);
            oledReset?.Write(PinValue.High);
            Thread.Sleep(100);
        }
    }
}
