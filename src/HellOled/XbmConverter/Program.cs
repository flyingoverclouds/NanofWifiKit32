using System;
using System.Collections.Generic;
using System.Text;

namespace XbmConverter
{

    // SAMPLE. 
    // USE https://www.online-utility.org/image/convert/to/XBM that generate directly C code.
    class Program
    {
        // This simple program is used to convert a AsciiArt ( 1 & 0 char only) to a c# byte[]
        // ascciArt can be generated using an online Image to AsciiArt converer
        // i try the following : https://cloudapps.herokuapp.com/imagetoascii/ with the setting : FontSize 10, CustomCharSet:10

        /* Image sample :  
        000000000000001111111110000000000000000000000000
        000000000000111111111111000000000000000000000000
        000000000001111000000111100000000000000000000000
        000000000011111000000111110000000000000000000000
        000000000001111111111111110000000000000000000000
        000000000000011111111111111000000000000000000000
        000000000000000000000000111111000000000000000000
        000000000111100000000000001111111111111100000000
        000001111111111100000000000001111111111111100000
        000111111000011111110000000001111110000111111000
        000111100000001111111111111111111100000011111000
        000111111000011111110000000001111110001111110000
        000001111111111110000000000011111111111111100000
        000000000111100000000000001111111100000000000000
        000000000000000000000001111111000000000000000000
        000000000000011111111111111000000000000000000000
        000000000000111111111111110000000000000000000000
        000000000001111000000011110000000000000000000000
        000000000001111100000011110000000000000000000000
        000000000000111111111111100000000000000000000000
        000000000000000111111110000000000000000000000000
        */
        static void Main(string[] args)
        {
            Console.WriteLine("Enter ASCI ART line per line. Finish with and Empty line");
            // Read Ascii art from console until an empty line
            List<string> lst = new List<string>();
            string s = string.Empty;
            
            do
            {
                Console.Write(">");
                s = Console.ReadLine();
                if (s != "")
                    lst.Add(s);
            } while (s != "");

            //Convert ascii art to byte[]
            List<byte> bytes = new List<byte>();
            foreach (var l in lst)
            {
                var currentPos = 0;
                while(currentPos<l.Length)
                {
                    var charBits = l.Substring(currentPos, 8);

                    byte b = (byte)(((charBits[0] == '1') ? 128 : 0 )
                        + ((charBits[1] == '1') ? 64 : 0)
                        + ((charBits[2] == '1') ? 32 : 0)
                        + ((charBits[3] == '1') ? 16 : 0)
                        + ((charBits[4] == '1') ? 8 : 0)
                        + ((charBits[5] == '1') ? 4 : 0)
                        + ((charBits[6] == '1') ? 2 : 0)
                        + ((charBits[7] == '1') ? 1 : 0));

                    bytes.Add(b);

                    currentPos += 8;
                }
            }

            Console.WriteLine("byte[] ready to copy paste : ");
            // generate C# byte[] initialisation string from the byte[]
            StringBuilder sbCS = new StringBuilder();
            foreach(var b in bytes)
            {
                Console.Write($"0x{b:X2},");
            }

        }
       
    }
}
