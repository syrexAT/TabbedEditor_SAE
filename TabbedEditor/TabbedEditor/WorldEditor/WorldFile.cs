using SimpleWorld.ToolDevelopment_SAE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbedEditor.WorldEditor
{
    class WorldFile
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
        public WorldData Data;
        public bool UnsavedChanges = false;
    }
}
