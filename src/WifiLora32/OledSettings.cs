
namespace sablefin.nf.WifiLora32
{
    /// <summary>
    /// Constant for onboard Oled on Heltec WifiLora 32 v2 (compatible with Wifikit32)
    /// </summary>
    static public class OnBoardOled
    {
        /// <summary>
        /// Onboard oled screen width (in pixel)
        /// </summary>
        public const int ScreenWidth = 128;

        /// <summary>
        /// Onboard oled screen height (in pixel)
        /// </summary>
        public const int ScrenHeight = 64;

        /// <summary>
        /// Onboar oled screen i2C address
        /// </summary>
        public const byte I2CAddress = 0x3c;

        /// <summary>
        /// Onboard oled Data gpio pin
        /// </summary>
        public const int Data = OnBoardDevicePortNumber.OledSDA;

        /// <summary>
        /// Onboard oled Clock gpio pin
        /// </summary>
        public const int Clock = OnBoardDevicePortNumber.OledSCL;

        /// <summary>
        /// Onboard oled Reset gpio pin
        /// </summary>
        public const int Reset = OnBoardDevicePortNumber.OledRST;

        /// <summary>
        /// Onboard oled power (VExt) gpio pin.
        /// Used to electrically switching on/off the oled screen.
        /// </summary>
        public const int VExt = OnBoardDevicePortNumber.OledVExt;
    }
}
