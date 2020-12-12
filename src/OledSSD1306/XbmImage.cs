using System;

namespace sablefin.nf.OledDisplay1306
{
    /// <summary>
    /// XBM image
    /// </summary>
    public class XbmImage
    {
        /// <summary>
        /// construct a new instance of XBM image
        /// </summary>
        /// <param name="width">width of the bitmatp</param>
        /// <param name="height">height of the bitmap</param>
        /// <param name="datas">binaray datas for XBM picture</param>
        public XbmImage(int width, int height, byte[] datas)
        {
            this.Width = width;
            this.Height = height;
            this.Datas = datas;
        }

        /// <summary>
        /// Width of image (in pixel)
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Height of image (in pixel)
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Binary datas of image
        /// </summary>
        public byte[] Datas { get; private set; }
    }
}
