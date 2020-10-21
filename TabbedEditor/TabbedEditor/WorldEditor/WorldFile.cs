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
        public string Path;
        public WorldData Data;
        public bool UnsavedChanges = false;
    }
}
