using SimpleWorld.ToolDevelopment_SAE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TabbedEditor.WorldEditor.Tools
{
    class LandBrushTool : IWorldEditorTool
    {

        private WorldEditorControl editor;

        //Constructor
        public LandBrushTool(WorldEditorControl editor)
        {
            this.editor = editor;
        }

        public void OnClick(WorldTileControl tileControl, MouseButtonEventArgs e)
        {
            //Referenz auf WorldEditorControl holen, und hol mir von dem die tileControl selber raus
            tileControl.TileType = (TileType)editor.TileTypeSelector.SelectedValue;
        }
    }
}
