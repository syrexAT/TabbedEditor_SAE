using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbedEditor.TextEditor
{
    class TextFile
    {
        public string Path = "";//welche datei wir bearbeiten, pfad zur datei
        public string Data = ""; //den text den wir bearebieten und später uaf die festplatte speichern
        public bool UnsavedChanges = false;//ob es bearbeitet wird, sterndal
    }
}
