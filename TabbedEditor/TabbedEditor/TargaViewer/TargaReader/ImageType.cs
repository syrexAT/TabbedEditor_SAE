using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbedEditor.TargaViewer.TargaReader
{
    enum ImageType
    {
        UncompressedTrueColor, //die kompletten farbdaten
        UncompressedColorMap,
        RunLengthTrueColor,
        RunLengthColorMap

    }
}
