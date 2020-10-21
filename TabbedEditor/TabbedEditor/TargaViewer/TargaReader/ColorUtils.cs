using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TabbedEditor.TargaViewer.TargaReader
{
    static class ColorUtils
    {
        //Static methode die 1 pixel einliest
        //wenn ich vor BinaryReader this schrieben würd dann wärs eine Extension method! würd auch gehen
        public static Color ReadColor(this BinaryReader reader, byte pixelDepth) //Pixel depth wie viele kanäle wir haben - 1 kanal schwarz weiss, 3 kanaläe RGB, oder 4 Kanäle ARGB
        {
            //Die Color die wir zurückliefern hat natürlich immer ARGB
            //wir werden immer mindestens 1 kanal haben
            byte r = reader.ReadByte(); //1. byte reinladen
            byte g = r; //g und b auf den selben wert setzen --> dasnn hat man grau stufen --> schwarz weiss
            byte b = r;
            byte a = 255;

            //Wenn es mehr als einen kanal gibt
            if (pixelDepth > 8) //größer als 8 bits
            {
                g = reader.ReadByte();
                b = reader.ReadByte();
            }

            if (pixelDepth > 24) //32 bit das heisst wir haben 4 kanäle also alpha auch dazu(other attribute)
            {
                a = reader.ReadByte();
            }

            return Color.FromArgb(a, r, g, b); //das ladet mir jetzt einen pixel ein

        }

        public static int ReadIndex(this BinaryReader reader, byte pixelDepth)
        {
            int index = 0;
            if (pixelDepth == 8) //also 1 byte
            {
                index = reader.ReadByte();
            }
            else if (pixelDepth == 16) //also 2 bytes
            {
                index = reader.ReadInt16(); //ReadInt16 ist auch ein "short"
            }
            return index;
        }
    }
}
