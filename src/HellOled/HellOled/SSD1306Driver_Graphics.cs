using System;

namespace nanoframework.OledDisplay1306
{
    public partial class SSD1306Driver
    {
        public void SetPixel(UInt16 x,UInt16 y)
        {
            //if (x >= 0 && x < this->width() && y >= 0 && y < this->height())
            if (x>=0 && x<_displayWidth && y>=0 && y< _displayHeight)
            {
                switch (_currentColor)
                {
                    case OledColor.White:
                        //        case WHITE: buffer[x + (y / 8) * this->width()] |= (1 << (y & 7)); break;
                         displayBuffer[x + (y / 8) * _displayWidth] |= (byte)(1 << (y & 7));
                        break;
                    case OledColor.Black:
                        //        case BLACK: buffer[x + (y / 8) * this->width()] &= ~(1 << (y & 7)); break;
                        displayBuffer[x + (y / 8) * _displayWidth] &= (byte)~(1 << (y & 7));
                        break;
                    case OledColor.Inverse:
                        //        case INVERSE: buffer[x + (y / 8) * this->width()] ^= (1 << (y & 7)); break;
                        displayBuffer[x + (y / 8) * _displayWidth] ^= (byte)(1 << (y & 7));
                        break;
                }
            }
            if(x==0&&y==0)
            {
                displayBuffer[x + (y / 8) * _displayWidth] |= 128;
            }
        }

        public void DrawLine(UInt16 x0, UInt16 y0,UInt16 x1, UInt16 y1)
        {
            throw new NotImplementedException();
        }

        public void DrawRect(UInt16 x0, UInt16 y0, UInt16 x1, UInt16 y1)
        {
            throw new NotImplementedException();
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

        public void DrawHorizontalLine(UInt16 x0, UInt16 y0, UInt16 length)
        {
            throw new NotImplementedException();
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
