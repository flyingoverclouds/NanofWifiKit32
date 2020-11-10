using System;

namespace nanoframework.OledDisplay1306
{
    public partial class SSD1306Driver
    {

        /// <summary>
        /// set a pixel to the currentcolor.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPixel(int x, int y)
        {
            if (x>=0 && x<_displayWidth && y>=0 && y< _displayHeight)
            {
                switch (_currentColor)
                {
                    case OledColor.White:
                         displayBuffer[x + (y / 8) * _displayWidth] |= (byte)(1 << (y & 7));
                        break;
                    case OledColor.Black:
                        displayBuffer[x + (y / 8) * _displayWidth] &= (byte)~(1 << (y & 7));
                        break;
                    case OledColor.Inverse:
                        displayBuffer[x + (y / 8) * _displayWidth] ^= (byte)(1 << (y & 7));
                        break;
                }
            }
        }


        private int abs(int v)
        {
            // localimplementation of Abs() to avoid including a full Math package
            return (v < 0) ? -v : v;
        }

        private void swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }


        /// <summary>
        /// Draw a line from (x0,y0) to (x1,y1) using the current color 
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        public void DrawLine(int x0, int y0, int x1, int y1)
        {
            var steep = abs(y1 - y0) > abs(x1 - x0);
            if (steep)
            {
                swap(ref x0, ref y0);
                swap(ref x1, ref y1);
            }

            if (x0 > x1)
            {
                swap(ref x0, ref x1);
                swap(ref y0, ref y1);
            }

            var dx = (x1 - x0);
            var dy = abs(y1 - y0);

            var err = (dx / 2);
            int ystep;

            if (y0 < y1)
            {
                ystep = 1;
            }
            else
            {
                ystep = -1;
            }

            for (; x0 <= x1; x0++)
            {
                if (steep)
                {
                    SetPixel(y0,x0);
                }
                else
                {
                    SetPixel(x0, y0);
                }
                err -= dy;
                if (err < 0)
                {
                    y0 +=  ystep;
                    err += dx;
                }
            }
        }


        /// <summary>
        /// Draw a transparent rectangle box, using currentcolor for border.
        /// </summary>
        /// <param name="x">Top Left X coordinate</param>
        /// <param name="y">Top Left Y coordinate</param>
        /// <param name="width">Width of rectangle (including bounding box)</param>
        /// <param name="height">Height of rectangle (including bounding box)</param>
        public void DrawRect(int x, int y, int width, int height)
        {
            DrawHorizontalLine(x, y, width);
            DrawVerticalLine(x, y, height);
            DrawVerticalLine(x + width - 1, y, height);
            DrawHorizontalLine(x, y + height - 1, width);
        }

        /// <summary>
        /// Draw a filled rectangle box using the currentcolor for border and filler.
        /// </summary>
        /// <param name="x">Top Left X coordinate</param>
        /// <param name="y">Top Left Y coordinate</param>
        /// <param name="width">Width of rectangle </param>
        /// <param name="height">Height of rectangle</param>
        public void FillRect(int x, int y, int width, int height)
        {
            for (int xm = x; xm < x + width; xm++)
            {
                DrawVerticalLine(xm, y, height);
            }
        }

        public void DrawCircle(UInt16 x0, UInt16 y0, UInt16 radius)
        {
            throw new NotImplementedException();
        }

        public void DrawCircleQuad(UInt16 x0, UInt16 y0, UInt16 radius, UInt16 quads)
        {
            throw new NotImplementedException();
        }

        public void FillCircle(UInt16 x0, UInt16 y0, UInt16 radius)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Draw an hoizontal line 
        /// </summary>
        /// <param name="x">X origine</param>
        /// <param name="y">Y origine</param>
        /// <param name="length">size of line (in pixel)</param>
        public void DrawHorizontalLine(int x, int y, int length)
        {
            if (y < 0 || y >= _displayHeight) 
                return; 

            if (x < 0)
            {
                length += x;
                x = 0;
            }

            if ((x + length) > _displayWidth)
                length = (_displayWidth - x);

            if (length <= 0) { return; }

            int bufferNdx = (y >> 3) * _displayWidth;
            bufferNdx += x;

            byte drawBit = (byte)(1 << (y & 7));

            switch (CurrentColor)
            {
                case OledColor.White:
                    while (length-- != 0)
                        displayBuffer[bufferNdx++] |= drawBit;
                    break;
                case OledColor.Black:
                    drawBit = (byte)(~drawBit); 
                    while (length--!=0)
                        displayBuffer[bufferNdx++] &= drawBit;
                    break;
                case OledColor.Inverse:
                    while (length-- != 0)
                        displayBuffer[bufferNdx++] ^= drawBit;
                    break;
            }
        }

        /// <summary>
        /// Draw a vertical line using an optimized algorithme
        /// </summary>
        /// <param name="x">X origin</param>
        /// <param name="y">Y origin</param>
        /// <param name="length">Length in pixel</param>
        public void DrawVerticalLine(int x, int y, int length)
        {
            if (x < 0 || x >= _displayWidth || length==0) return;

            if (y < 0)
            {
                length += y;
                y = 0;
            }

            if ((y + length) > _displayHeight)
                length = (_displayHeight - y);

            if (length <= 0) 
                return;


            int yOffset = (byte)(y & 7);
            byte drawBit;
            int bufferNdx = 0;

            bufferNdx += (y >> 3) * _displayWidth;
            bufferNdx += x;

            if (yOffset!=0)
            {
                yOffset = 8 - yOffset;
                drawBit = (byte)~(0xFF >> (yOffset));

                if (length < yOffset)
                {
                    drawBit &= (byte) (0xFF >> (yOffset - length));
                }

                switch (CurrentColor)
                {
                    case OledColor.White:
                        displayBuffer[bufferNdx] |= drawBit;
                        break;
                    case OledColor.Black:
                        displayBuffer[bufferNdx] &= (byte)~drawBit;
                        break;
                    case OledColor.Inverse:
                        displayBuffer[bufferNdx] ^= drawBit;
                        break;
                }

                if (length < yOffset) 
                    return;

                length -= yOffset;
                bufferNdx += _displayWidth;
            }

            if (length >= 8)
            {
                switch (CurrentColor)
                {
                    case OledColor.White:
                    case OledColor.Black:
                        drawBit =(byte) ((CurrentColor==OledColor.White) ? 0xFF : 0x00);
                        do
                        {
                            displayBuffer[bufferNdx] = drawBit;
                            bufferNdx += _displayWidth;
                            length -= 8;
                        } while (length >= 8);
                        break;
                    case OledColor.Inverse:
                        do
                        {
                            displayBuffer[bufferNdx] = (byte)~displayBuffer[bufferNdx];
                            bufferNdx += _displayWidth;
                            length -= 8;
                        } while (length >= 8);
                        break;
                }
            }

            if (length > 0)
            {
                drawBit =(byte)( (1 << (length & 7)) - 1);
                switch (CurrentColor)
                {
                    case OledColor.White:
                        displayBuffer[bufferNdx] |= drawBit;
                        break;
                    case OledColor.Black:
                        displayBuffer[bufferNdx] &= (byte)~drawBit;
                        break;
                    case OledColor.Inverse:
                        displayBuffer[bufferNdx] ^= drawBit;
                        break;
                }
            }

        }

        public void DrawProgressBar(UInt16 x0, UInt16 y0, UInt16 width, UInt16 height, UInt16 progress)
        {
            throw new NotImplementedException();
        }

        public void DrawFastImage(UInt16 x0, UInt16 y0, UInt16 width, UInt16 height, byte[] image )
        {
            throw new NotImplementedException();
        }

        public void DrawXbm(UInt16 x0, UInt16 y0, UInt16 width, UInt16 height, byte[] image)
        {
            throw new NotImplementedException();
        }


    }
}
