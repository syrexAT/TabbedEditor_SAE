using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbedEditor.TextEditor
{
    class TextFile
    {
        private string _path = "";
        public string Name { get; private set; } = "";

        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                Name = value.Split('\\').Last();
            }
        }
        public string Data = ""; //den text den wir bearebieten und später uaf die festplatte speichern
        public bool UnsavedChanges = false;//ob es bearbeitet wird, sterndal
    }
}
