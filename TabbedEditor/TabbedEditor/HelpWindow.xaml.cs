using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TabbedEditor
{
    /// <summary>
    /// Interaktionslogik für HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();

            WorldEditorHelp.Text = "For level editing, you can open and save .world and .json files.\n" +
                "There are 3 tools available for customizing your level\n" +
                "1. Brush Tool: Choose your Tile Type in the Dropdown menu and draw your Tile Type onto the level tiles.\n" +
                "2. Add Enemy: Press on a tile to add an enemy\n" +
                "3. Remove Enemy: Press on a tile to remove an enemy\n";

            TargaViewerHelp.Text = "You can view uncompressed/compressed truecolor/colormap targa files.";

            TextEditorHelp.Text = "You can open a .txt file and view, edit and save it.";

            NavigationHelp.Text = "On the top left of the window, you can open the \"File\" Tab." +
                " There you can choose to \"Open\" a file from a directory or \"Save/Save As\" to save the file";

        }
    }
}
