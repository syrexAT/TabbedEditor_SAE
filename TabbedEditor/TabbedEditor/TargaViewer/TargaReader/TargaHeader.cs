using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbedEditor.TargaViewer.TargaReader
{
    class TargaHeader
    {
        public byte IDLength;
        public ImageType ImageType;
        public int XOrigin; //2 bytes
        public int YOrigin;
        public int Width;
        public int Height;
        public byte PixelDepth;

        //Für Farbpallette Color map
        public int ColorMapSize;
        public byte ColorMapPixelDepth;


        //Zum laden wieder public static methode
        public static TargaHeader Read(BinaryReader reader)
        {
            TargaHeader header = new TargaHeader();
            header.IDLength = reader.ReadByte(); //1 byte einlesen für die ID Length --> also fertig
            reader.ReadByte(); //Lies es ein aber micht nichts damit, Skip Color Map Type, is redundant information

            //enum einlesen und dann interpretieren wie wirs wollen
            byte ImageTypeByte = reader.ReadByte();
            switch (ImageTypeByte)
            {
                case 1:
                    header.ImageType = ImageType.UncompressedColorMap;
                    break;
                case 2: //weil das black and white image unnötig ist zu unterscheiden deswegen macht einfach case 2 das selbe wie case 3 und wir lassens quasi aus
                case 3:
                    header.ImageType = ImageType.UncompressedTrueColor;
                    break;
                case 9:
                    header.ImageType = ImageType.RunLengthColorMap;
                    break;
                case 10:
                case 11:
                    header.ImageType = ImageType.RunLengthTrueColor;
                    break;
            }

            reader.ReadBytes(2); //Skip Color Map Offset
            header.ColorMapSize = reader.ReadInt16();
            header.ColorMapPixelDepth = reader.ReadByte();
            /*reader.ReadBytes(5);*/ // 5byte sladen wegen Color Map Specification; we skip colormap specification for now! also lesens ein und machen damit mal nichts noch

            header.XOrigin = reader.ReadInt16();
            header.YOrigin = reader.ReadInt16();
            header.Width = reader.ReadInt16();
            header.Height = reader.ReadInt16();
            header.PixelDepth = reader.ReadByte();

            reader.ReadByte(); //wird ignoriert aber muss wieder eingelesen werden, das ist der Image Descriptor

            return header;
        }
    }
}
