using OledFonts;
using System;
using System.Diagnostics;

namespace nanoframework.OledDisplay1306
{
    /// <summary>
    /// Define alignement of text for Text oriented method
    /// </summary>
    public enum TextAlignment
    {
        /// <summary>
        /// Text aligned on LEFT
        /// </summary>
        Left = 0,
        
        /// <summary>
        /// Text aligned on RIGHT
        /// </summary>
        Right = 1,
        
        /// <summary>
        /// Text CENTERED on the line
        /// </summary>
        Center = 2,

        /// <summary>
        /// Text vertically AND horizontally CENTERED
        /// </summary>
        CenterBoth = 3
    };


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



        byte UnicodeToAscii(char ch) // TODO : replace with a extension method on Char type !
        {
            if (ch < 127)
                return (byte)ch;
            if ((((ushort)ch) & 0xFF00) == 0xC2)
                return (byte)ch;
            if (((ushort)ch) == 0x82AC) // special case for € symbol
                return 0x80;
            return 32; // all other char ignored and replaced by a char
        }

        byte[] utf8ascii(string str)
        {
            var l = str.Length;
            var s = new byte[l];

            for (int i = 0; i < l; i++)
                s[i] = UnicodeToAscii(str[i]);
            return s;
        }


        /// <summary>
        /// xMove & yMOve is the origine of drawing. For centered aligned, this is the center of the line
        /// </summary>
        /// <param name="xMove"></param>
        /// <param name="yMove"></param>
        /// <param name="text"></param>
        /// <param name="textWidth"></param>
        private void DrawStringInternal_LEGACY(int xMove, int yMove, byte[] text, int textWidth)
        {
            int textLength = text.Length;

            int textHeight = _currentFontData[HEIGHT_POS];//pgm_read_byte(fontData + HEIGHT_POS);
            byte firstChar = _currentFontData[FIRST_CHAR_POS];// pgm_read_byte(fontData + FIRST_CHAR_POS);
            int sizeOfJumpTable = _currentFontData[CHAR_NUM_POS] * JUMPTABLE_BYTES; //pgm_read_byte(fontData + CHAR_NUM_POS) * JUMPTABLE_BYTES;


            int cursorX = 0;
            int cursorY = 0;

            switch (CurrentTextAlignement)
            {
                case TextAlignment.CenterBoth:
                    yMove -= textHeight >> 1;
                    xMove -= textWidth >> 1; // divide by 2
                    break;
                case TextAlignment.Center:
                    xMove -= textWidth >> 1; // divide by 2
                    break;
                case TextAlignment.Right:
                    xMove -= textWidth;
                    break;
                case TextAlignment.Left:
                    break;
            }

            // Don't draw anything if it is not on the screen.
            if (xMove + textWidth < 0 || xMove > _displayWidth) { return; }
            if (yMove + textHeight < 0 || yMove > _displayWidth) { return; } 
            //TODO: BUG HERE ??? WHy not yMove > _displayHeight ???

            for (int j = 0; j < textLength; j++)
            {
                int xPos = xMove + cursorX;
                int yPos = yMove + cursorY;

                byte code = text[j];
                if (code >= firstChar)
                {
                    byte charCode = (byte)(code - firstChar);

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
                        //drawInternal_LEGACY(xPos, yPos, currentCharWidth, textHeight, _currentFontData, charDataPosition, charByteSize);
                    }

                    cursorX += currentCharWidth;
                }
            }

        }

        public void DrawString_LEGACY(int xMove, int yMove, String strUser)
        {
            int lineHeight = _currentFontData[HEIGHT_POS]; //pgm_read_byte(fontData + HEIGHT_POS);

            //HERE
            var text = utf8ascii(strUser);

            int yOffset = 0;

            // If the string should be centered vertically too
            // we need to now how heigh the string is.
            if (CurrentTextAlignement == TextAlignment.CenterBoth)
            {
                int lb = 0;
                // Find number of linebreaks in text
                for (int i = 0; text[i] != 0; i++)
                {
                    if(text[i] == 10)
                         lb++;
                }
                // Calculate center
                yOffset = (lb * lineHeight) / 2;
            }

            int line = 0;
            //char* textPart = strtok(text, "\n");
            var textPart = text; 

            // TODO : handle multi line text ?

            //while (textPart != NULL)
            //{
            //    uint16_t length = strlen(textPart);
            //    drawStringInternal_LEGACY(xMove, yMove - yOffset + (line++) * lineHeight, textPart, length, getStringWidth(textPart, length));
            //    textPart = strtok(NULL, "\n");
            //}
            //free(text);

        }


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



        public void DrawChar(int x, int y, char c)
        {
            if (_currentFontData==null)
            {
                Debug.WriteLine("ERROR: No current font selected !");
                return;
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

                    //drawInternal_LEGACY(xPos, yPos, currentCharWidth, textHeight, _currentFontData, charDataPosition, charByteSize);
                }

            }
        }




        public void DrawString_NF(int xMove, int yMove, string strUser)
        {
            int lineHeight = _currentFontData[HEIGHT_POS]; //pgm_read_byte(fontData + HEIGHT_POS);

            var lines = strUser.Split('\n');
            

            //HERE
            var text = utf8ascii(strUser);

            int yOffset = 0;

            // If the string should be centered vertically too
            // we need to now how heigh the string is.
            if (CurrentTextAlignement == TextAlignment.CenterBoth)
            {
                int lb = 0;
                // Find number of linebreaks in text
                for (int i = 0; text[i] != 0; i++)
                {
                    if (text[i] == 10)
                        lb++;
                }
                // Calculate center
                yOffset = (lb * lineHeight) / 2;
            }

            int line = 0;
            //char* textPart = strtok(text, "\n");
            var textPart = text;

            // TODO : handle multi line text ?

            //while (textPart != NULL)
            //{
            //    uint16_t length = strlen(textPart);
            //    drawStringInternal_LEGACY(xMove, yMove - yOffset + (line++) * lineHeight, textPart, length, getStringWidth(textPart, length));
            //    textPart = strtok(NULL, "\n");
            //}
            //free(text);

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

    }
}
