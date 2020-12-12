
namespace sablefin.nf.WifiLora32
{
    /// <summary>
    /// Contains constant for using the Onbard LORA SX1276 chipset on Heltec WifiLora32 v2 board
    /// </summary>
    static public class LoRaSettings
    {
        public const int CS = OnBoardDevicePortNumber.LoRaCS;
        public const int RST = OnBoardDevicePortNumber.LoRaRST;
        public const int Miso = OnBoardDevicePortNumber.LoRaMISO;
        public const int Mosi  = OnBoardDevicePortNumber.LoRaRST;
        public const int Dio0 = OnBoardDevicePortNumber.LoRaDIO0;
        public const int Dio1 = OnBoardDevicePortNumber.LoRaDIO1;
        public const int Dio2 = OnBoardDevicePortNumber.LoRaDIO2;
        public const int SCK = OnBoardDevicePortNumber.LoRaSCK;
    }
}
