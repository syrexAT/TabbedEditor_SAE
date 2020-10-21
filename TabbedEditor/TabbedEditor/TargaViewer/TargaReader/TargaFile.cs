using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace TabbedEditor.TargaViewer.TargaReader
{        //utility klasse die kümmert scih darum die targa file einzuläsen und usn eine bitmap zurückliefert
    class TargaFile
    {
        public TargaHeader Header;
        public Color[,] Pixels;

        private void Read(BinaryReader reader)
        {
            //Die soll daraus unsere targa file befüllen
            //https://en.wikipedia.org/wiki/Truevision_TGA
            Header = TargaHeader.Read(reader);

            Pixels = new Color[Header.Width, Header.Height];

            reader.ReadBytes(Header.IDLength); //So lange wie die ID length ist lies es ein und hau es weg also wieder ignorieren --> ID Field

            if (Header.ImageType == ImageType.UncompressedTrueColor)
            {
                for (int y = 0; y < Header.Height; y++)
                {
                    for (int x = 0; x < Header.Width; x++)
                    {
                        //hier die farbe einlsen dafür schreiben wir aber eine utility, weil wir pixel/farbe einlesen exterme oft brauchen
                        Pixels[x, y] = reader.ReadColor(Header.PixelDepth); //jetzt haben wir ein fertig eingelesenes bild
                    }
                }
            }
            else if (Header.ImageType == ImageType.UncompressedColorMap)
            {
                Color[] colorMap = new Color[Header.ColorMapSize];
                for (int i = 0; i < Header.ColorMapSize; i++)
                {
                    colorMap[i] = reader.ReadColor(Header.ColorMapPixelDepth);
                }
                //jetzt index auslesen und in der color map auslesen was für eine farbe das ist
                for (int y = 0; y < Header.Height; y++)
                {
                    for (int x = 0; x < Header.Width; x++)
                    {
                        //wenn die pixeldepth an einem pixel 1 byte ist dann kann ihc ja nur 256 farben speichern, wenn es aber 2 bytes groß is dann mehr 
                        //hat aber nix damit zu tun wie groß die color map ist
                        int index = 0;
                        if (Header.PixelDepth == 8) //also 1 byte
                        {
                            index = reader.ReadByte();
                        }
                        else if (Header.PixelDepth == 16) //also 2 bytes
                        {
                            index = reader.ReadInt16(); //ReadInt16 ist auch ein "short"
                        }
                        Pixels[x, y] = colorMap[index]; //das pixel an der stelle x,y ist was an der colormap and er stelle index steht (aus x und y die zahl holen und ah, was soll das da für eine farbe sein?)

                    }
                }

            }
            else if (Header.ImageType == ImageType.RunLengthTrueColor)
            {
                //wieder über x und y drüberlaufen, dabe immer einen block aktiv haben, xy aus aktuellen block füllen bis er leer is
                //dann neuen block laden und wieder weiterladen solange der block da ist
                //der pixel wird immer gesetzt auf die farbe die aktuell im block da ist
                //das heisst wir brauchen einen aktiven block, wenn der leer is dann laden wir einen neuen BlockHeader
                BlockHeader blockHeader = new BlockHeader();
                //wir brauchen die farbe die wir immer wieder brauchen
                Color color = Colors.White;

                for (int y = 0; y < Header.Height; y++)
                {
                    for (int x = 0; x < Header.Width; x++)
                    {
                        //wir wollen wissen wann der blcok leer ist
                        if (blockHeader.Length == 0)
                        {
                            blockHeader = BlockHeader.Read(reader); //einen ganzen block lesen
                            if (blockHeader.IsRLE)
                            {
                                color = reader.ReadColor(Header.PixelDepth); //wenns RLE ist dann einfach der color variable die color geben die iwederholt wird
                            }
                        }
                        if (blockHeader.IsRLE) //wenn wir in einem RLE bit sind ->1 mal farbe laden und mehrmals widerholen
                        {
                            Pixels[x, y] = color; //die color werden wir spoäter noch ladenw
                        }
                        else
                        {
                            //wenn nicht RLE -> für jeden pixel farbe laden einfach normal
                            Pixels[x, y] = reader.ReadColor(Header.PixelDepth);

                        }
                       
                        blockHeader.Length--; //mit jeden pixel den wir auslesen reduzieren wie die blockheader Length, bis er leer is, dann den neuen laden
                    }
                }
            }
            else if (Header.ImageType == ImageType.RunLengthColorMap)
            {
                Color[] colorMap = new Color[Header.ColorMapSize];
                for (int i = 0; i < Header.ColorMapSize; i++)
                {
                    colorMap[i] = reader.ReadColor(Header.ColorMapPixelDepth);
                }

                BlockHeader blockHeader = new BlockHeader();
                //wir brauchen die farbe die wir immer wieder brauchen
                int index = 0;

                for (int y = 0; y < Header.Height; y++)
                {
                    for (int x = 0; x < Header.Width; x++)
                    {
                        //wir wollen wissen wann der blcok leer ist
                        if (blockHeader.Length == 0)
                        {
                            blockHeader = BlockHeader.Read(reader); //einen ganzen block lesen
                            if (blockHeader.IsRLE)
                            {
                                index = reader.ReadIndex(Header.PixelDepth);
                            }
                        }
                        if (blockHeader.IsRLE) //wenn wir in einem RLE bit sind ->1 mal farbe laden und mehrmals widerholen
                        {
                            Pixels[x, y] = colorMap[index]; //die color werden wir spoäter noch ladenw
                        }
                        else
                        {
                            //wenn nicht RLE -> für jeden pixel farbe laden einfach normal
                            int pixelIndex = reader.ReadIndex(Header.PixelDepth);
                            Pixels[x, y] = colorMap[pixelIndex];

                        }

                        blockHeader.Length--; //mit jeden pixel den wir auslesen reduzieren wie die blockheader Length, bis er leer is, dann den neuen laden
                    }
                }
            }
        }

        //Static klasse die das wichtigste tut, wir habend ann 1 teil der ladet und 1 teil damit es ein praktisches util ist
        public static TargaFile Read(string path)
        {
            //neues TargaFile Object erstellen
            TargaFile targaFile = new TargaFile();

            //Datei öffnen
            //Stream ist immer dann wen von irgendwo langsam daten herkommen, lange datenwurscht aufeinmal herkommt, es ist egal ob es von Festplatte kommt/Internet whatever --> das ist ein Stream
            //und ein Zeiger der auf eine bestimme stelle in den Daten zeigt und den kan man immer weiter schreiben
            //Reader übergibt man den Stream und die sind das praktische ding die können mit diesen stream umgehen und sinnvoll umgehen

            //Stream erstellen
            var stream = File.OpenRead(path); //nur zum einlesen, wir wollen nämlcih nichts writen!, wir können da alles reinwerfen was mit streams umgehen kann

            //BinaryReader
            BinaryReader reader = new BinaryReader(stream);

            targaFile.Read(reader);

            //damit kann man jetzt alles machen
            //im targe file komplexe daten laden, der reader knan aber nur mit simplen daten umgehen(bytes)
            reader.Close(); //Zumachen!!! wenn man es nicht zumacht dann ist man in der situation wo windows sagt das er die datei nicht beendet werden kann usw. weil wie von einem anderen Editor geöffnet ist
            stream.Close();

            return targaFile;
        }
    }
}
