using System;

namespace nanoframework.OledDisplay1306
{
    public enum TextAlignment
    {
        Left = 0,
        Right = 1,
        Center = 2,
        CenterBoth = 3
    };


    public partial class SSD1306Driver
    {
        public void DrawString(UInt16 x,UInt16 y,string text)
        {
            throw new NotImplementedException();
        }
        public void DrawStringMaxWidth(UInt16 x, UInt16 y, UInt16 maxLineWidth, string text)
        {
            throw new NotImplementedException();
        }

        public UInt16 GetStringWidth(string text,UInt16 length)
        {
            throw new NotImplementedException();
        }

        public UInt16 GetStringWidth(string text)
        {
            throw new NotImplementedException();
        }

        public void SetFont(/* object font struct */)
        {
            throw new NotImplementedException();
        }


    public void SetFontTableLookupFunnction(/* tablelookupfunction */)
        {
            // Convert UTF8 --> font table index
            throw new NotImplementedException();
        }




    }
}
