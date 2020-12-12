using System;

namespace sablefin.nf.WifiLora32
{

    /// <summary>
    /// This class expose const that describe available GPIO port on Heltec WifiLora32 v2.
    /// Somme GPIO port can be hardwired to onbard device (look at comments/intellisense for detailed informations per pin.)
    /// Based on http://resource.heltec.cn/download/WiFi_LoRa_32/WIFI_LoRa_32_V2.pdf (16-nov-2020)
    /// </summary>
    public static class GpioPortNumber
    {
        /// <summary>
        /// HARDWIRED pin to PRG Button
        /// supported capabilities : ADC2_1, Touch 1, Button
        /// WIFIKIT32 Compatible.
        /// </summary>
        public const int Gpio0 = 0;


        /// <summary>
        /// HARDWIRED With TX
        /// CAPABILITIES : TX, U0_TXD
        /// WIFIKIT32 Compatible.
        /// </summary>
        public const int Gpio1 = 1;

        /// <summary>
        /// Supported capabilities : CS, ADC2_2, HSPI_WP, Touch2
        /// WIFIKIT32 Compatible.
        /// </summary>
        public const int Gpio2 = 2;

        /// <summary>
        /// HARDWIRED with RX
        /// CAPABILITIES : RX, U0_RXD
        /// WIFIKIT32 Compatible.
        /// </summary>
        public const int Gpio3 = 3;  

        /// <summary>
        /// HARDWIRED pin to Oled screen (SDA)
        /// UNAVAILABLE capabilities : ADC2_0, HSPI_HD, Touch0
        /// WIKIT32 Compatible.
        /// </summary>
        public const int Gpio4 = 4;  // wired to OLED_SDA

        /// <summary>
        /// LoRa SCK (SPI Clock)
        /// UNAVAILBLE capabilities : V_SPI_CS0
        /// </summary>
        public const int Gpio5 = 5;

        /// <summary>
        /// Supported capabilities : Touch5, ADC2_5
        /// WifiKit32 compatible.
        /// </summary>
        public const int Gpio12 = 12;

        /// <summary>
        /// Supported capabilities : ADC2_4, Touch4
        /// </summary>
        public const int Gpio13 = 13; 

        /// <summary>
        /// LoRa_RST (Reset)
        /// UNAVAILABLE capabilities : Touch6, ADC2_6
        /// </summary>
        public const int Gpio14 = 14;

        /// <summary>
        /// Hardwired pin to Oled screen (SCL)
        /// UNAVAILABLE capabilities : ADC2_3, HSPI_CS0, Touch 3
        /// WifiKit32 compatible
        /// </summary>
        public const int Gpio15 = 15; // wired to OLED_SCL
        
        /// <summary>
        /// Hardwired pin to Oled screen (RST)
        /// UNAVAILABLE capabilities : U2_RXD
        /// Wifikit32 compatible
        /// </summary>
        public const int Gpio16 = 16; // wired to OLED_RST

        /// <summary>
        /// Supported capabilities : U2_TXD
        /// Wifikit32 compatible
        /// </summary>
        public const int Gpio17 = 17;

        /// <summary>
        /// LoRa_CS (SPI Select)
        /// UNAVAILABLE capabilities : V_SPI_CLK
        /// </summary>
        public const int Gpio18 = 18;

        /// <summary>
        /// LoRa_MISO  (SPI MISO)
        /// UNAVAILABLE capabilities : MISO, V_SPI_Q, U0_CTS
        /// </summary>
        public const int Gpio19 = 19;
        
        /// <summary>
        /// Hardwired pin for VExtControl
        /// UNAVAILABLE capabilities : SDA, V_SPI_HD
        /// Wifikit32 compatible
        /// </summary>
        public const int Gpio21 = 21;

        /// <summary>
        /// Supported capabilities : SCL, V_SPI_WP, U0_RTS
        /// Wifikit32 compatible
        /// </summary>
        public const int Gpio22 = 22;

        /// <summary>
        /// Supported capabilities : V_SPI_D
        /// Wifikit32 compatible
        /// </summary>
        public const int Gpio23 = 23;

        /// <summary>
        /// Hardwired pin to onboardLED1
        /// UNAVAILABLE capabilities : DAC2, ADC2_8
        /// Wifikit32 compatible
        /// </summary>
        public const int Gpio25 = 25;

        /// <summary>
        /// Hardwired pin to LoRa_DIO0 (Interrupt 0)
        /// UNAVAILBLE capabilities : DAC1, ADC2_9
        /// </summary>
        public const int Gpio26 = 26;

        /// <summary>
        /// Hardwired pin to LoRa_MOSI (SPI MOSI)
        /// UNAVAILABLE capabilities : Touch7, ADC2_7
        /// </summary>
        public const int Gpio27 = 27;

        /// <summary>
        /// Hardwired pin to LoRa_DIO2 (Interrupt 2)
        /// UNAVAILABLE capabilities : Touch9, ADC1_4, XTAL32
        /// </summary>
        public const int Gpio32 = 32;

        /// <summary>
        /// Hardwired pin to LoRa_DIO1 (Interrupt 1)
        /// UNAVAILABLE capabilities : Touch8, ADC1_5, XTAL32
        /// </summary>
        public const int Gpio33 = 33;
        
        /// <summary>
        /// INPUT ONLY
        /// Supported capabilities :  ADC1_6
        /// Wifikit32 compatible
        /// </summary>
        public const int Gpio34 = 34;

        /// <summary>
        /// INPUT ONLY
        /// Supported capabilities : ADC1_7
        /// Wifikit32 compatible
        /// </summary>
        public const int Gpio35 = 35;


        /// <summary>
        /// INPUT ONLY 
        /// Supported capabilities : ADC1_0
        /// Wifikit32 compatible
        /// </summary>
        public const int Gpio36 = 36;

        /// <summary>
        /// INPUT ONLY 
        /// Supported capabilities : ADC1_1
        /// Wifikit32 compatible
        /// </summary>
        public const int Gpio37 = 37;

        /// <summary>
        /// INPUT ONLY 
        /// Supported capabilities : ADC1_2
        /// Wifikit32 compatible
        /// </summary>
        public const int Gpio38 = 38;

        /// <summary>
        /// INPUT ONLY
        /// Supported capabilities : ADC1_3
        /// Wifikit32 compatible
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
        /// Shared with onboard PRG button (GPIO 0)
        /// </summary>
        public const int Touch1 = GpioPortNumber.Gpio0; 

        /// <summary>
        /// Touch 2 (GPIO 2)
        /// </summary>
        public const int Touch2 = GpioPortNumber.Gpio2;

        //public const int Touch3 = GpioPortNumber.Gpio15; //NOT AVAILABLE : hardwired to Oled SCL

        /// <summary>
        /// Touch 4 (GPIO 13)
        /// </summary>
        public const int Touch4 = GpioPortNumber.Gpio13; 

        /// <summary>
        /// Touch 5 (GPIO 12)
        /// </summary>
        public const int Touch5 = GpioPortNumber.Gpio12;

        ///// <summary>
        ///// Touch 6 (GPIO 14)
        ///// </summary>
        //public const int Touch6 = GpioPortNumber.Gpio14; //NOT AVAILABLE : hardwired to LoRa_RST

        ///// <summary>
        ///// Touch 7 (GPIO 27)
        ///// </summary>
        //public const int Touch7 = GpioPortNumber.Gpio27; //NOT AVAILABLE : hardwired to LoRa_MOSI

        ///// <summary>
        ///// Touch 8 (GPIO 33)
        ///// </summary>
        //public const int Touch8 = GpioPortNumber.Gpio33; //NOT AVAILABLE : hardwired to LoRa_DIO1

        ///// <summary>
        ///// Touch 9 (GPIO 32)
        ///// </summary>
        //public const int Touch9 = GpioPortNumber.Gpio32; //NOT AVAILABLE : hardwired to LoRa_DIO2
    }

    /// <summary>
    /// This class expose GPIO port number for HeltecWifikit32 onboard device.
    /// (look at comments/intellisense for detailed informations per pin.)
    /// </summary>
    public static class OnBoardDevicePortNumber
    {
        /// <summary>
        /// Onbard LED (GPIO 25)
        /// WifiKit32 compatible
        /// </summary>
        public const int Led = GpioPortNumber.Gpio25;

        /// <summary>
        /// Onboad Oled 0.96' : SCL line (GPIO 15)
        /// WifiKit32 compatible
        /// </summary>
        public const int OledSCL = GpioPortNumber.Gpio15;

        /// <summary>
        /// Onboad Oled 0.96' : SDA line (GPIO 4)
        /// WifiKit32 compatible
        /// </summary>
        public const int OledSDA = GpioPortNumber.Gpio4;

        /// <summary>
        /// Onboad Oled 0.96' : RST line (GPIO 16)
        /// WifiKit32 compatible
        /// </summary>
        public const int OledRST = GpioPortNumber.Gpio16;

        /// <summary>
        /// Onboard Button PRG (GPIO 0)
        /// WifiKit32 compatible
        /// </summary>
        public const int Button = GpioPortNumber.Gpio0;

        /// <summary>
        /// Onboad VExt Control (GPIO 21)
        /// WifiKit32 compatible
        /// </summary>
        public const int OledVExt = GpioPortNumber.Gpio21;


        /// <summary>
        /// Onboad LORA MISO (GPIO 19)
        /// </summary>
        public const int LoRaMISO = GpioPortNumber.Gpio19;

        /// <summary>
        /// Onboad LORA CS(GPIO 18)
        /// </summary>
        public const int LoRaCS = GpioPortNumber.Gpio18;

        /// <summary>
        /// Onboad LORA SCK (GPIO 5)
        /// </summary>
        public const int LoRaSCK = GpioPortNumber.Gpio5;

        /// <summary>
        /// Onboad LORA DIO 2(GPIO 32)
        /// </summary>
        public const int LoRaDIO2 = GpioPortNumber.Gpio32;

        /// <summary>
        /// Onboad LORA DIO 1 (GPIO 33)
        /// </summary>
        public const int LoRaDIO1 = GpioPortNumber.Gpio33;

        /// <summary>
        /// Onboad LORA DIO 0 (GPIO 26)
        /// </summary>
        public const int LoRaDIO0 = GpioPortNumber.Gpio26;

        /// <summary>
        /// Onboad LORA MOSI (GPIO 27)
        /// </summary>
        public const int LoRaMOSI = GpioPortNumber.Gpio27;

        /// <summary>
        /// Onboad LORA RST (GPIO 14)
        /// </summary>
        public const int LoRaRST = GpioPortNumber.Gpio14;
    }

}
