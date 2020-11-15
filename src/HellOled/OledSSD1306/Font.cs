
namespace sablefin.nf.OledDisplay1306
{
    /// <summary>
    /// Encapsulate font information for SSD1306 driver
    /// </summary>
    public class Font
    {
        /// <summary>
        /// Consctruct a font instance.
        /// Font are using the same binary format as the original Thingpulse/Heltec library
        /// </summary>
        /// <param name="data">byte array containet font data</param>
        public Font(byte[] data)
        {
            this.LegacyFont = data;
        }
        
        /// <summary>
        /// Legacy byte array (same structure as the original SSD1306 Fonts froem thingpulse/heltec library)
        /// </summary>
        public byte[] LegacyFont { get; internal set; }
    }



}
