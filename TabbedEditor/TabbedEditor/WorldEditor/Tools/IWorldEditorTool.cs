using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TabbedEditor.WorldEditor.Tools
{
    interface IWorldEditorTool
    {
        void OnClick(WorldTileControl tileControl, MouseButtonEventArgs e);
    }
}
