using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbedEditor.TargaViewer.TargaReader
{
    class BlockHeader
    {
        public bool IsRLE; //Is RunLengthEncoded?
        public byte Length;

        public static BlockHeader Read(BinaryReader reader)
        {
            BlockHeader header = new BlockHeader();
            //vom Reader 1 byte einlesen
            //ich brauch danach ein byteArray deswegen lesen wir jetzt direkt 1 bytearray mit 1 byte drinnen aus
            byte[] headerByte = reader.ReadBytes(1);
            BitArray bitArray = new BitArray(headerByte); //bit array liefert bool mit true false 0 / 1

            header.IsRLE = bitArray[7]; //höchstwertigste bit is das letzte das drinnen sthet (das is beim bitarray so!) und das wwollen wir auslesen
            bitArray[7] = false; //ich will dash ochwertigste wegschneiden weil das darf unsere auswahl nicht beeinflussen

            //bitarray in ein byte umwandeln da gibt so ein trick wie folgt
            byte[] outputArray = new byte[1];
            bitArray.CopyTo(outputArray, 0);
            header.Length = outputArray[0]; //das 1 byte was drin steht
            header.Length++; //weil eine länge von 0 ergibt keinen sinn, alle pakete wird mindestens 1 beeinhalten



            return header;  
        }

    }
}
