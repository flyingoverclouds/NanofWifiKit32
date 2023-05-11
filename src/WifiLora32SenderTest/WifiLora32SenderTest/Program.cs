/* Demo program for the Lora capabilities of Heltec LoraWifikit32
 * 
 ***************************************************************************************
 * 
 * MIT License
 *
 * Copyright (c) 2021 Nicolas CLERC
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
using System.Diagnostics;
using System.Threading;
using sablefin.nf.WifiLora32;
using sablefin.nf.OledDisplay1306;
using sablefin.nf.OledFonts;
//using sablefin.nf.WifiKit32Common;
using HeltecHelper;
using System.Device.Gpio;

using nanoFramework.Hardware.Esp32;
using Windows.Devices.Spi;
using devMobile.IoT.Rfm9x;

namespace WifiLora32SenderTest
{
    public class Program
    {
        static void DemoOLED_FOR_SAMPLESONLY(SSD1306Driver oledScreen)
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
 
            Random rnd = new Random();
            oledScreen.DrawProgressBar(5, 5, 118, 10, rnd.Next(100));
            //oledScreen.DrawXbm(64, 22, wifiLogo.Width, wifiLogo.Height, wifiLogo.Datas); // legacy lib signature
            //oledScreen.DrawXbm(20, 22, nanofLogo); // modern dotnet signature

            oledScreen.Clear();
            oledScreen.CurrentTextAlignement = TextAlignment.Left;

            oledScreen.CurrentFont = FontArialMTPlain10.GetFont();
            oledScreen.DrawString(1, 0, "NiCo");

            oledScreen.CurrentFont = FontArialMTPlain16.GetFont();
            oledScreen.DrawString(1, 11, "nIc0");

            oledScreen.CurrentFont = FontArialMTPlain24.GetFont();
            oledScreen.DrawString(1, 30, "N1co");

            oledScreen.Clear();
            oledScreen.CurrentFont = FontArialMTPlain10.GetFont();
            oledScreen.DrawHorizontalLine(0, 32, 128);
            oledScreen.DrawVerticalLine(64, 0, 64);
            oledScreen.CurrentTextAlignement = TextAlignment.Left;
            oledScreen.DrawString(0, 0, "DrawString()\rLine 1\nLine 2\r\nLine 3");
            oledScreen.CurrentTextAlignement = TextAlignment.Right;
            oledScreen.DrawString(128, 0, "DrawString()\rLine 1\nLine 2\r\nLine 3");
            oledScreen.CurrentTextAlignement = TextAlignment.Center;
            oledScreen.DrawString(64, 0, "< >\r-< >-\roO0[]0Oo\r/ \\\r\\_ _/");
        }


        static void DisplayScreen(SSD1306Driver oledScreen,int dataContext)
        {
            oledScreen.Clear();
            oledScreen.CurrentFont = FontArialMTPlain10.GetFont();
            oledScreen.CurrentTextAlignement = TextAlignment.Center;
            oledScreen.DrawString(64, 0,"Lora Message Sender");
            oledScreen.CurrentColor = OledColor.Inverse;
            oledScreen.FillRect(0, 0, 128, 14);
            oledScreen.CurrentColor = OledColor.White;

            oledScreen.CurrentTextAlignement = TextAlignment.Left;
            oledScreen.DrawString(0, 52, $"MSG#{dataContext}");

            oledScreen.CurrentFont = FontArialMTPlain16.GetFont();
            oledScreen.DrawString(0, 15, $"LAT:");
            oledScreen.DrawString(40, 15, $"0\"{dataContext % 43}'{dataContext % 91}");
            oledScreen.DrawString(0,30,$"LON:");
            oledScreen.DrawString(40, 30, $"0\"{dataContext % 17}'{dataContext % 87}");
        }


       

        public static void LoraKit32Specific()
        {
            const string SpiBusId = "SPI1";
            const int chipSelectPinNumber = sablefin.nf.WifiLora32.LoRaSettings.CS;
            int SendCount = 0;


            try
            {
                Configuration.SetPinFunction(sablefin.nf.WifiLora32.LoRaSettings.Miso, DeviceFunction.SPI1_MISO);
                Configuration.SetPinFunction(sablefin.nf.WifiLora32.LoRaSettings.Mosi, DeviceFunction.SPI1_MOSI);
                Configuration.SetPinFunction(sablefin.nf.WifiLora32.LoRaSettings.SCK, DeviceFunction.SPI1_CLOCK);
                Rfm9XDevice rfm9XDevice = new Rfm9XDevice(SpiBusId, chipSelectPinNumber);
                Thread.Sleep(500);

                // Put device into LoRa + Standby mode
                rfm9XDevice.RegisterWriteByte(0x01, 0b10000000); // RegOpMode 

                // Set the frequency to 915MHz
                byte[] frequencyWriteBytes = { 0xE4, 0xC0, 0x00 }; // RegFrMsb, RegFrMid, RegFrLsb
                rfm9XDevice.RegisterWrite(0x06, frequencyWriteBytes);

                // More power PA Boost
                rfm9XDevice.RegisterWriteByte(0x09, 0b10000000); // RegPaConfig

                rfm9XDevice.RegisterDump();
                while (true)
                {
                    rfm9XDevice.RegisterWriteByte(0x0E, 0x0); // RegFifoTxBaseAddress 

                    // Set the Register Fifo address pointer
                    rfm9XDevice.RegisterWriteByte(0x0D, 0x0); // RegFifoAddrPtr 

                    string messageText = $"Hello LoRa {SendCount += 1}!";

                    // load the message into the fifo
                    // FLOCS: byte[] messageBytes = UTF8Encoding.UTF8.GetBytes(messageText);
                    byte[] messageBytes = SSD1306Driver.Utf8StringToAsciiString(messageText);

                    rfm9XDevice.RegisterWrite(0x0, messageBytes); // RegFifo

                    // Set the length of the message in the fifo
                    rfm9XDevice.RegisterWriteByte(0x22, (byte)messageBytes.Length); // RegPayloadLength

                    Debug.WriteLine($"Sending {messageBytes.Length} bytes message {messageText}");
                    /// Set the mode to LoRa + Transmit
                    rfm9XDevice.RegisterWriteByte(0x01, 0b10000011); // RegOpMode 

                    // Wait until send done, no timeouts in PoC
                    Debug.WriteLine("Send-wait");
                    byte IrqFlags = rfm9XDevice.RegisterReadByte(0x12); // RegIrqFlags
                    while ((IrqFlags & 0b00001000) == 0)  // wait until TxDone cleared
                    {
                        Thread.Sleep(10);
                        IrqFlags = rfm9XDevice.RegisterReadByte(0x12); // RegIrqFlags
                        Debug.Write(".");
                    }
                    Debug.WriteLine("");
                    rfm9XDevice.RegisterWriteByte(0x12, 0b00001000); // clear TxDone bit
                    Debug.WriteLine("Send-Done");

                    Thread.Sleep(10000);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"EXCEPTION : {ex.Message}");
            }
        }
    

        public static void Main()
        {
            Debug.WriteLine("[WifiLora32SenderTest] : send lora message (counter value) while displaying loranet technical information.");

            var heltec = new HeltecOled();
            heltec.Begin();
            heltec.Display.SetBrightness(180);
            heltec.Display.FlipScreenVertically();
            heltec.Display.CurrentColor = OledColor.White;


            int counter = 0;
            while (true)
            {
                switch (counter)
                {
                    default:
                        //counter = -1;
                        DisplayScreen(heltec.Display,counter);
                        break;
                }
                Debug.WriteLine($"Counter = {counter}");
                heltec.Display.RefreshDisplay();
                Thread.Sleep(1000);
                counter++;
            }
        
        }
    }
}
