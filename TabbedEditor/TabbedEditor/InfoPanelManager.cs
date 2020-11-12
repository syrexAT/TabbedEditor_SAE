using SimpleWorld.ToolDevelopment_SAE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TabbedEditor
{
    public static class InfoPanelManager
    {
        static StackPanel stackPanel;

        public static void Update(TileType type, int enemyCount)
        {
            stackPanel.Children.Clear();
            stackPanel.Children.Add(new Label() {Content = "Tile Type: " + type.ToString()});
            stackPanel.Children.Add(new Label() {Content = "Enemy Count: " + enemyCount});
            stackPanel.Width = 100;
        }

        public static void Init(StackPanel panel)
        {
            stackPanel = panel;
        }

        public static void Hide()
        {
            stackPanel.Width = 0;
            stackPanel.Children.Clear();
        }


    }
}
