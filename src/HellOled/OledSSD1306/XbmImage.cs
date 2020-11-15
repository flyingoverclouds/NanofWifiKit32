using System;

namespace HellOled
{
    /// <summary>
    /// XBM image
    /// </summary>
    public class XbmImage
    {
        public XbmImage(int width, int height, byte[] datas)
        {
            this.Width = width;
            this.Height = height;
            this.Datas = datas;
        }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public byte[] Datas { get; private set; }
    }
}
