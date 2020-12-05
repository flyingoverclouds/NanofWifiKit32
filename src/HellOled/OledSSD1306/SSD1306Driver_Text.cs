using System;
using System.Diagnostics;

namespace sablefin.nf.OledDisplay1306
{

    public partial class SSD1306Driver
    {
        const int JUMPTABLE_BYTES = 4;
        const int JUMPTABLE_LSB = 1;
        const int JUMPTABLE_SIZE = 2;
        const int JUMPTABLE_WIDTH = 3;
        const int JUMPTABLE_START = 4;
        const int WIDTH_POS = 0;
        const int HEIGHT_POS = 1;
        const int FIRST_CHAR_POS = 2;
        const int CHAR_NUM_POS = 3;

        Font _currentFont = null;
        byte[] _currentFontData = null;

        TextAlignment _currentTextAlignement;
        public TextAlignment CurrentTextAlignement { 
            get=>_currentTextAlignement; 
            set { _currentTextAlignement = value; } 
        }


        /// <summary>
        /// get/set the current font data
        /// </summary>
        public Font CurrentFont
        {
            get { return _currentFont; }
            set { 
                _currentFont = value;
                _currentFontData = _currentFont.LegacyFont;
            }
        }


        /// <summary>
        /// quick & dirty unicode to ascii converter.
        /// support the € symbol. 
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        byte UnicodeToAscii(char ch) 
        {
            // TODO: replace by an char extension method ? 
            if (ch < 127)
                return (byte)ch;
            if ((((ushort)ch) & 0xFF00) == 0xC2)
                return (byte)ch;
            if (((ushort)ch) == 0x82AC) // special case for € symbol
                return 0x80;
            return 32; // all other char ignored and replaced by a char
        }

        /// <summary>
        /// transcode an UTF8 encoded string to an array of byte (ascii encoding)
        /// </summary>
        /// <param name="str">string to ascii-ize</param>
        /// <returns>byte array of ascii encoded character</returns>
        byte[] utf8ascii(string str)
        {
            var l = str.Length;
            var s = new byte[l];

            for (int i = 0; i < l; i++)
                s[i] = UnicodeToAscii(str[i]);
            return s;
        }


        /// <summary>
        /// Draw one char from the font data on a specified position
        /// </summary>
        /// <param name="xMove">top left X of origin</param>
        /// <param name="yMove">top left Y of origin</param>
        /// <param name="width">width of char</param>
        /// <param name="height">height of char</param>
        /// <param name="data">font data to display</param>
        /// <param name="offset">offset of char to print</param>
        /// <param name="bytesInData">size of data char to display</param>
        private void DrawFontChar(int xMove, int yMove, int width, int height, byte[] data, int offset, int bytesInData) 
        {
            if (width< 0 || height< 0) return;
            if (yMove + height< 0 || yMove> _displayHeight)  return;
            if (xMove + width< 0 || xMove> _displayWidth)   return;

            byte rasterHeight = (byte)(1 + ((height - 1) >> 3)); // fast ceil(height / 8.0)
            int yOffset = (byte)(yMove & 7);

            bytesInData = bytesInData == 0 ? width* rasterHeight : bytesInData;

            int initYMove = yMove;
            int initYOffset = yOffset;


            for (int i = 0; i<bytesInData; i++) 
            {
                // Reset if next horizontal drawing phase is started.
                if (i % rasterHeight == 0) 
                {
                  yMove   = initYMove;
                  yOffset = initYOffset;
                }

                byte currentByte = data[offset+i];// pgm_read_byte(data + offset + i);

                int xPos = xMove + (i / rasterHeight);
                int yPos = ((yMove >> 3) + (i % rasterHeight)) * _displayWidth;

                int dataPos = xPos + yPos;

                if (dataPos >=  0  && dataPos<_displayBufferSize && xPos    >=  0  && xPos< _displayWidth ) 
                {
                    if (yOffset >= 0) 
                    {
                        switch (_currentColor) 
                        {
                            case OledColor.White: displayBuffer[dataPos] |= (byte)(currentByte << yOffset); break;
                            case OledColor.Black : displayBuffer[dataPos] &= (byte)(~(currentByte << yOffset)); break;
                            case OledColor.Inverse: displayBuffer[dataPos] ^= (byte)(currentByte << yOffset); break;
                        }
                        if (dataPos < (_displayBufferSize - _displayWidth)) 
                        {
                            switch (_currentColor)
                            {
                                case OledColor.White: displayBuffer[dataPos + _displayWidth] |= (byte)(currentByte >> (8 - yOffset)); break;
                                case OledColor.Black: displayBuffer[dataPos + _displayWidth] &= (byte)(~(currentByte >> (8 - yOffset))); break;
                                case OledColor.Inverse: displayBuffer[dataPos + _displayWidth] ^= (byte)(currentByte >> (8 - yOffset)); break;
                            }
                        }
                    } 
                    else 
                    {
                        // Make new offset position
                        yOffset = -yOffset;
                        switch (_currentColor) 
                        {
                            case OledColor.White: displayBuffer[dataPos] |= (byte)(currentByte >> yOffset); break;
                            case OledColor.Black: displayBuffer[dataPos] &= (byte)(~(currentByte >> yOffset)); break;
                            case OledColor.Inverse: displayBuffer[dataPos] ^= (byte)(currentByte >> yOffset); break;
                        }

                        // Prepare for next iteration by moving one block up
                        yMove -= 8;

                        // and setting the new yOffset
                        yOffset = 8 - yOffset;
                    }

                }
            }
        }


        /// <summary>
        /// Draw a char on a specificposition. 
        /// The function return the width of the char in pixel 
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position </param>
        /// <param name="c">Char to draw</param>
        /// <returns>Width of the drawn char.</returns>
        public int DrawChar(int x, int y, char c)
        {
            if (_currentFontData==null)
            {
                Debug.WriteLine("ERROR: No current font selected !");
                return 0; // nothing drawn
            }
            int textHeight = _currentFontData[HEIGHT_POS];//pgm_read_byte(fontData + HEIGHT_POS);
            byte firstChar = _currentFontData[FIRST_CHAR_POS];// pgm_read_byte(fontData + FIRST_CHAR_POS);
            int sizeOfJumpTable = _currentFontData[CHAR_NUM_POS] * JUMPTABLE_BYTES; //pgm_read_byte(fontData + CHAR_NUM_POS) * JUMPTABLE_BYTES;

            byte ascii = UnicodeToAscii(c);

            if (ascii >= firstChar)
            {
                byte charCode = (byte)(ascii - firstChar);

                // 4 Bytes per char code
                byte msbJumpToChar = _currentFontData[JUMPTABLE_START + charCode * JUMPTABLE_BYTES]; // pgm_read_byte(fontData + JUMPTABLE_START + charCode * JUMPTABLE_BYTES);                  // MSB  \ JumpAddress
                byte lsbJumpToChar = _currentFontData[JUMPTABLE_START + charCode * JUMPTABLE_BYTES + JUMPTABLE_LSB]; // pgm_read_byte(fontData + JUMPTABLE_START + charCode * JUMPTABLE_BYTES + JUMPTABLE_LSB);   // LSB /
                byte charByteSize = _currentFontData[JUMPTABLE_START + charCode * JUMPTABLE_BYTES + JUMPTABLE_SIZE]; // pgm_read_byte(fontData + JUMPTABLE_START + charCode * JUMPTABLE_BYTES + JUMPTABLE_SIZE);  // Size
                byte currentCharWidth = _currentFontData[JUMPTABLE_START + charCode * JUMPTABLE_BYTES + JUMPTABLE_WIDTH]; // pgm_read_byte(fontData + JUMPTABLE_START + charCode * JUMPTABLE_BYTES + JUMPTABLE_WIDTH); // Width

                // Test if the char is drawable
                if (!(msbJumpToChar == 255 && lsbJumpToChar == 255))
                {
                    // Get the position of the char data
                    int charDataPosition = JUMPTABLE_START + sizeOfJumpTable + ((msbJumpToChar << 8) + lsbJumpToChar);
                    DrawFontChar(x, y, currentCharWidth, textHeight, _currentFontData, charDataPosition, charByteSize);
               }
                return currentCharWidth;
            }
            return 0; // nothing to draw
        }

        /// <summary>
        /// Return the width (in pixel) of a char in the current font.
        /// </summary>
        /// <param name="c">char</param>
        /// <returns>width in pixel</returns>
        private int GetCharWidth(char c)
        {
            if (_currentFontData == null)
            {
                Debug.WriteLine("ERROR: No current font selected !");
                return 0; // nothing drawn
            }
            byte firstChar = _currentFontData[FIRST_CHAR_POS];// pgm_read_byte(fontData + FIRST_CHAR_POS);
            byte ascii = UnicodeToAscii(c);
            if (ascii >= firstChar)
            {
                byte charCode = (byte)(ascii - firstChar);
                // 4 Bytes per char code
                byte currentCharWidth = _currentFontData[JUMPTABLE_START + charCode * JUMPTABLE_BYTES + JUMPTABLE_WIDTH]; 
                return currentCharWidth;
            }
            return 0; // nothin drawn
        }



        /// <summary>
        /// Draw a string on a specific position. 
        /// ONLY LEFT ALIGNMENT SUPPORTED
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="txt">test to print</param>
        public void DrawString(int x, int y, string txt)
        {
            if (this.CurrentTextAlignement == TextAlignment.Left)
            {
                DrawString_LeftAlignment(x, y, txt);
                return;
            }

            int origX = x;
            int txtLength = txt.Length; // cached value to avoid multiple call the the property
            int start = 0, end = 0; // cursor to browse the string 
            int width = 0; // width of line to display
            int offset = 0; // offset to respect requested alignment
            char c;
            while (start<txtLength-1)
            {
                // extracting end of current line from the current start position && update width on each iteration
                while( end<txtLength && !(txt[end]=='\n' || txt[end]=='\r'))
                {
                    width += GetCharWidth(txt[end]);
                    end++;
                }

                // Compute left offset for current line
                if (this.CurrentTextAlignement==TextAlignment.Center)
                {
                    offset = width / 2;
                }
                else if (this.CurrentTextAlignement==TextAlignment.Right)
                {
                    offset = width;
                }

                // Draw the current ligne
                for (int i = start; i < end; i++)
                {
                    c = txt[i];
                    x+= DrawChar(x-offset, y, c);
                }

                end++; // to go after the current new line
                width = 0;
                start = end;
                x = origX;
                y += this.CurrentFont.LegacyFont[HEIGHT_POS];
            }


        }

        /// <summary>
        /// This is a private optimized version of DrawString() for left Alignment.
        /// </summary>
        /// <param name="x">X origin</param>
        /// <param name="y">Y origin</param>
        /// <param name="txt">text to print</param>
        private void DrawString_LeftAlignment(int x, int y, string txt)
        {
            int origX = x;
            int wChar = 0;

            //foreach (var c in txt) // HACK: generate a compiler error due to the lake of IEnumerable implementation on String class.
            char c;
            for(int i=0;i<txt.Length;i++)
            {
                c = txt[i];
                if (c == '\n' || c == '\r')
                {
                    x = origX;
                    y += this.CurrentFont.LegacyFont[HEIGHT_POS];
                    continue;
                }
                wChar = DrawChar(x, y, c);
                x += wChar;
            }
        }

        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="maxLineWidth"></param>
        /// <param name="text"></param>
        public void DrawStringMaxWidth(UInt16 x, UInt16 y, UInt16 maxLineWidth, string text)
        {
            throw new NotImplementedException();
        }

        public int GetStringWidth(string txt, int length)
        {
            int wChar = 0;
            int size = 0;
            char c;
            for (int i = 0; i < txt.Length; i++)
            {
                if (i >= length)
                    break;
                c = txt[i];
                size += GetCharWidth(c);
            }

            return size;
        }

        public int GetStringWidth(string txt)
        {
            return GetStringWidth(txt, txt.Length);
        }

    }
}
