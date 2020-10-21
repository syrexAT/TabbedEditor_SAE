using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TabbedEditor.WorldEditor.Tools
{
    class ChangeEnemyCountTool : IWorldEditorTool
    {
        private WorldEditorControl editor;
        private int enemyDelta; //wollen wir enemies hinzufügen oder weggnehemn?, entweder +1 oder -1

        //Constructor
        public ChangeEnemyCountTool(WorldEditorControl editor, int enemyDelta)
        {
            this.editor = editor;
            this.enemyDelta = enemyDelta;
        }

        public void OnClick(WorldTileControl tileControl, MouseButtonEventArgs e)
        {
            if (tileControl.EnemyCount + enemyDelta < 0)
            {
                return;
                //Vielleicht noch warnung anzeigen und es blinkt oderso 
            }
            tileControl.EnemyCount += enemyDelta; //enemydelta is entweder +1 oder -1, also entweder fügen wir da gegner hinzu oder nehmen sie herraus
        }
    }
}
