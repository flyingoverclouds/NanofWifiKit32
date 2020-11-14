
namespace OledFonts
{
    /// <summary>
    /// Encapsulate font information for SSD1306 driver
    /// </summary>
    public class Font
    {
        /// <summary>
        /// Legac byte array (same structure as the original SSD1306 Fonts)
        /// </summary>
        public byte[] LegacyFont { get; internal set; }
    }



}
