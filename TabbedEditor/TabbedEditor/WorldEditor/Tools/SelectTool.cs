using SimpleWorld.ToolDevelopment_SAE.Data;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TabbedEditor.WorldEditor.Tools
{
    class SelectTool : IWorldEditorTool
    {
        private WorldEditorControl editor;

        private TileType type;
        private int enemyCount;

        public SelectTool(WorldEditorControl editor)
        {
            this.editor = editor;
        }

        public void OnClick(WorldTileControl tileControl, MouseButtonEventArgs e)
        {
            //tileControl.TileType = type;
            type = tileControl.TileType;
            //tileControl.TileBackground = type;
            //tileControl.EnemyCountLabel = enemyCount;
            //tileControl.EnemyCount = enemyCount;
            enemyCount = tileControl.EnemyCount;
            InfoPanelManager.Update(type, enemyCount);

            
        }

        public void OnDeselect()
        {
            InfoPanelManager.Hide();

        }
    }
}
