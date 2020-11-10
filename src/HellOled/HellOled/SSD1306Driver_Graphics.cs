﻿using System;

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


        int abs(int v)
        {
            // localimplementation of Abs() to avoid including a full Math package
            return (v < 0) ? -v : v;
        }

        void swap<T>(ref T a, ref T b)
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
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        public void DrawRect(UInt16 x0, UInt16 y0, UInt16 x1, UInt16 y1)
        {
            //drawHorizontalLine(x, y, width);
            //drawVerticalLine(x, y, height);
            //drawVerticalLine(x + width - 1, y, height);
            //drawHorizontalLine(x, y + height - 1, width);
        }

        public void FillRect(UInt16 x0, UInt16 y0, UInt16 x1, UInt16 y1)
        {
            throw new NotImplementedException();
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
            if (y < 0 || y >= _displayHeight) { return; }

            if (x < 0)
            {
                length += x;
                x = 0;
            }

            if ((x + length) > _displayWidth)
            {
                length = (_displayWidth - x);
            }

            if (length <= 0) { return; }

            //uint8_t* bufferPtr = buffer;
            //bufferPtr += (y >> 3) * this->width();
            //bufferPtr += x;
            int bufferNdx = (y >> 3) * _displayWidth;
            bufferNdx += x;

            byte drawBit = (byte)(1 << (y & 7));

            switch (CurrentColor)
            {
                case OledColor.White:
                    while (length-- != 0)
                    {
                        //            *bufferPtr++ |= drawBit;
                        displayBuffer[bufferNdx++] |= drawBit;
                    }; 
                    break;
                case OledColor.Black:
                    drawBit = (byte)(~drawBit); 
                    while (length--!=0)
                    {
                        //*bufferPtr++ &= drawBit;
                        displayBuffer[bufferNdx++] &= drawBit;

                    }; 
                    break;
                case OledColor.Inverse:
                    while (length-- != 0)
                    {
                        //            *bufferPtr++ ^= drawBit;
                        displayBuffer[bufferNdx++] ^= drawBit;
                    }; 
                    break;
            }
        }

        public void DrawVerticalLine(UInt16 x0, UInt16 y0, UInt16 length)
        {
            throw new NotImplementedException();
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
