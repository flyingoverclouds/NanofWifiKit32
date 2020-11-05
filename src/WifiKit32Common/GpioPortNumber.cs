using System;

namespace WifiKit32Common
{

    /// <summary>
    /// This class expose const that describe available GPIO port on Heltec Wifikit32.
    /// Somme GPIO port can be hardwired to onbard device (look at comments/intellisense for detailed informations per pin.)
    /// Based on https://resource.heltec.cn/download/WiFi_Kit_32/WIFI_Kit_32_pinoutDiagram_V2.pdf (5-nov-2020)
    /// </summary>
    public static class GpioPortNumber
    {
        /// <summary>
        /// HARDWIRED pin to Button
        /// supported capabilities : CLK1, ADC2_1, Touch 1, Button
        /// </summary>
        public const int Gpio0 = 0;
        
        
        ///// <summary>
        ///// HARDWIRED With TX
        ///// UNAVAILABLE CAPABILITIES : CLX3, U0_TXD
        ///// </summary>
        //public const int Gpio1 = 1; 
        
        /// <summary>
        /// Supported capabilities : CS, ADC2_2, HSPI_WP, Touch2
        /// </summary>
        public const int Gpio2 = 2;

        ///// <summary>
        ///// HARDWIRED with RX
        ///// UNAVAILABLE CAPABILITIES : CLX2, U0_RXD
        ///// </summary>
        //public const int Gpio3 = 3;  

        /// <summary>
        /// HARDWIRED pin to Oled screen (SDA)
        /// UNAVAILABLE capabilities : ADC2_0, HSPI_HD, Touch0
        /// </summary>
        public const int Gpio4 = 4;  // wired to OLED_SDA

        /// <summary>
        /// Supported capabilities : V_SPI_CS0
        /// </summary>
        public const int Gpio5 = 5;

        /// <summary>
        /// Supported capabilities : Touch5, ADC2_5
        /// </summary>
        public const int Gpio12 = 12;

        /// <summary>
        /// Hardwired pin for power detection
        /// UNAVAILABLE capabilities : ADC2_4, Touch4
        /// </summary>
        public const int Gpio13 = 13; // Wired to POWER DETECTION

        /// <summary>
        /// Supported capabilities : Touch6, ADC2_6
        /// </summary>
        public const int Gpio14 = 14;

        /// <summary>
        /// Hardwired pin to Oled screen (SCL)
        /// UNAVAILABLE capabilities : ADC2_3, HSPI_CS0, Touch 3
        /// </summary>
        public const int Gpio15 = 15; // wired to OLED_SCL
        
        /// <summary>
        /// Hardwired pin to Oled screen (RST)
        /// UNAVAILABLE capabilities : U2_RXD
        /// </summary>
        public const int Gpio16 = 16; // wired to OLED_RST

        /// <summary>
        /// Supported capabilities : U2_TXD
        /// </summary>
        public const int Gpio17 = 17;

        /// <summary>
        /// Supported capabilities : SCK, V_SPI_CLK
        /// </summary>
        public const int Gpio18 = 18;

        /// <summary>
        /// Supported capabilities : MISO, V_SPI_Q, U0_CTS
        /// </summary>
        public const int Gpio19 = 19;
        
        /// <summary>
        /// Hardwired ping for VExtControl
        /// UNAVAILABLE capabilities : SDA, V_SPI_HD
        /// </summary>
        public const int Gpio21 = 21;

        /// <summary>
        /// Supported capabilities : SCL, V_SPI_WP, U0_RTS
        /// </summary>
        public const int Gpio22 = 22;

        /// <summary>
        /// Supported capabilities : MOSI, V_SPI_D
        /// </summary>
        public const int Gpio23 = 23;
        
        /// <summary>
        /// Hardwire pin to onboardLED1
        /// UNAVAILABLE capabilities : DAC2, ADC2_8
        /// </summary>
        public const int Gpio25 = 25;

        /// <summary>
        /// Supported capabilities : DAC1, ADC2_9
        /// </summary>
        public const int Gpio26 = 26;

        /// <summary>
        /// Supported capabilities : Touch7, ADC2_7
        /// </summary>
        public const int Gpio27 = 27;

        /// <summary>
        /// INPUT ONLY
        /// Supported capabilities : Touch9, ADC1_4, XTAL32
        /// </summary>
        public const int Gpio32 = 32;

        /// <summary>
        /// INPUT ONLY
        /// Supported capabilities : Touch8, ADC1_5, XTAL32
        /// </summary>
        public const int Gpio33 = 33;
        
        /// <summary>
        /// INPUT ONLY
        /// Supported capabilities :  ADC1_6
        /// </summary>
        public const int Gpio34 = 34;

        /// <summary>
        /// INPUT ONLY
        /// Supported capabilities : ADC1_7
        /// </summary>
        public const int Gpio35 = 35;


        /// <summary>
        /// INPUT ONLY / ADC_PREAMPLIFIER
        /// Supported capabilities : ADC1_0, SenseVP
        /// </summary>
        public const int Gpio36 = 36;

        /// <summary>
        /// INPUT ONLY / ADC_PREAMPLIFIER
        /// Supported capabilities : ADC1_1, CapVP
        /// </summary>
        public const int Gpio37 = 37;

        /// <summary>
        /// INPUT ONLY / ADC_PREAMPLIFIER
        /// Supported capabilities : ADC1_2, CapVN
        /// </summary>
        public const int Gpio38 = 38;

        /// <summary>
        /// INPUT ONLY / ADC_PREAMPLIFIER
        /// Supported capabilities : ADC1_3, SenseVN
        /// </summary>
        public const int Gpio39 = 39;
    }


    /// <summary>
    /// This class export GPIO port number to ESP32 touch capabilities available on Wifikit32.
    /// </summary>
    public static class TouchPinPortNumber
    {
        //public const int Touch0 = GpioPortNumber.Gpio4; // NOT AVAILABLE : hardwired to Oled SDA
        
        /// <summary>
        /// Shared with onboard button (GPIO 0)
        /// </summary>
        public const int Touch1 = GpioPortNumber.Gpio0; 

        /// <summary>
        /// Touch 2 (GPIO 2)
        /// </summary>
        public const int Touch2 = GpioPortNumber.Gpio2;

        //public const int Touch3 = GpioPortNumber.Gpio15; //NOT AVAILABLE : hardwired to Oled SCL

        //public const int Touch4 = GpioPortNumber.Gpio13; // NOT AVAILABLE : hardwired to Power Detection

        /// <summary>
        /// Touch 5 (GPIO 12)
        /// </summary>
        public const int Touch5 = GpioPortNumber.Gpio12;

        /// <summary>
        /// Touch 6 (GPIO 14)
        /// </summary>
        public const int Touch6 = GpioPortNumber.Gpio14;

        /// <summary>
        /// Touch 7 (GPIO 27)
        /// </summary>
        public const int Touch7 = GpioPortNumber.Gpio27;

        /// <summary>
        /// Touch 8 (GPIO 33)
        /// </summary>
        public const int Touch8 = GpioPortNumber.Gpio33;

        /// <summary>
        /// Touch 9 (GPIO 32)
        /// </summary>
        public const int Touch9 = GpioPortNumber.Gpio32;
    }

    /// <summary>
    /// This class expose GPIO port number for HeltecWifikit32 onboard device.
    /// (look at comments/intellisense for detailed informations per pin.)
    /// </summary>
    public static class OnBoardDevicePortNumber
    {
        /// <summary>
        /// Onbard LED (GPIO 25)
        /// </summary>
        public const int Led = GpioPortNumber.Gpio25;

        /// <summary>
        /// Onboad Oled 0.96' : SCL line (GPIO 15)
        /// </summary>
        public const int OledSCL = GpioPortNumber.Gpio15;
        
        /// <summary>
        /// Onboad Oled 0.96' : SDA line (GPIO 4)
        /// </summary>
        public const int OledSDA = GpioPortNumber.Gpio4;
        
        /// <summary>
        /// Onboad Oled 0.96' : RST line (GPIO 16)
        /// </summary>
        public const int OledRST = GpioPortNumber.Gpio16;

        /// <summary>
        /// Onboard Button (GPIO 0)
        /// </summary>
        public const int Button = GpioPortNumber.Gpio0;

        /// <summary>
        /// Onboad VExt Control (GPIO 21)
        /// </summary>
        public const int VextControl = GpioPortNumber.Gpio21;
    }

}
