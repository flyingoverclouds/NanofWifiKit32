using System;

using System.Diagnostics;
using System.Threading;

using System.Device.Gpio;
using Windows.Devices.Spi;
using nanoFramework.Hardware.Esp32;

namespace devMobile.IoT.Rfm9x
{
    public sealed class Rfm9XDevice
    {
        private readonly SpiDevice rfm9XLoraModem;
        private const byte RegisterAddressReadMask = 0X7f;
        private const byte RegisterAddressWriteMask = 0x80;

        public Rfm9XDevice(string spiPort, int chipSelectPin, int resetPin)
        {
            var settings = new SpiConnectionSettings(chipSelectPin)
            {
                ClockFrequency = 1000000,
                //DataBitLength = 8,
                Mode = SpiMode.Mode0,// From SemTech docs pg 80 CPOL=0, CPHA=0
                SharingMode = SpiSharingMode.Shared,
            };

            rfm9XLoraModem = SpiDevice.FromId(spiPort, settings);

            // Factory reset pin configuration
            GpioController gpioController = new GpioController();
            GpioPin resetGpioPin = gpioController.OpenPin(resetPin);
            resetGpioPin.SetPinMode(PinMode.Output);
            resetGpioPin.Write(PinValue.Low);
            Thread.Sleep(10);
            resetGpioPin.Write(PinValue.High);
            Thread.Sleep(10);
        }

        public Rfm9XDevice(string spiPort, int chipSelectPin)
        {
            var settings = new SpiConnectionSettings(chipSelectPin)
            {
                ClockFrequency = 1000000,
                Mode = SpiMode.Mode0,// From SemTech docs pg 80 CPOL=0, CPHA=0
                SharingMode = SpiSharingMode.Shared,
            };

            rfm9XLoraModem = SpiDevice.FromId(spiPort, settings);
        }

        public Byte RegisterReadByte(byte registerAddress)
        {
            byte[] writeBuffer = new byte[] { registerAddress &= RegisterAddressReadMask, 0x0 };
            byte[] readBuffer = new byte[writeBuffer.Length];

            rfm9XLoraModem.TransferFullDuplex(writeBuffer, readBuffer);

            return readBuffer[1];
        }

        public ushort RegisterReadWord(byte address)
        {
            byte[] writeBuffer = new byte[] { address &= RegisterAddressReadMask, 0x0, 0x0 };
            byte[] readBuffer = new byte[writeBuffer.Length];

            rfm9XLoraModem.TransferFullDuplex(writeBuffer, readBuffer);

            return (ushort)(readBuffer[2] + (readBuffer[1] << 8));
        }

        public byte[] RegisterRead(byte address, int length)
        {
            byte[] writeBuffer = new byte[length + 1];
            byte[] readBuffer = new byte[writeBuffer.Length];
            byte[] repyBuffer = new byte[length];

            writeBuffer[0] = address &= RegisterAddressReadMask;

            rfm9XLoraModem.TransferFullDuplex(writeBuffer, readBuffer);

            Array.Copy(readBuffer, 1, repyBuffer, 0, length);

            return repyBuffer;
        }

        public void RegisterWriteByte(byte address, byte value)
        {
            byte[] writeBuffer = new byte[] { address |= RegisterAddressWriteMask, value };
            byte[] readBuffer = new byte[writeBuffer.Length];

            rfm9XLoraModem.TransferFullDuplex(writeBuffer, readBuffer);
        }

        public void RegisterWriteWord(byte address, ushort value)
        {
            byte[] valueBytes = BitConverter.GetBytes(value);
            byte[] writeBuffer = new byte[] { address |= RegisterAddressWriteMask, valueBytes[0], valueBytes[1] };
            byte[] readBuffer = new byte[writeBuffer.Length];

            rfm9XLoraModem.TransferFullDuplex(writeBuffer, readBuffer);
        }

        public void RegisterWrite(byte address, byte[] bytes)
        {
            byte[] writeBuffer = new byte[1 + bytes.Length];
            byte[] readBuffer = new byte[writeBuffer.Length];

            Array.Copy(bytes, 0, writeBuffer, 1, bytes.Length);
            writeBuffer[0] = address |= RegisterAddressWriteMask;

            rfm9XLoraModem.TransferFullDuplex(writeBuffer, readBuffer);
        }

        public void RegisterDump()
        {
            Debug.WriteLine("Register dump");
            for (byte registerIndex = 0; registerIndex <= 0x42; registerIndex++)
            {
                byte registerValue = this.RegisterReadByte(registerIndex);

                Debug.WriteLine($"Register 0x{registerIndex:x2} - Value 0X{registerValue:x2}");
            }
        }
    }

}
