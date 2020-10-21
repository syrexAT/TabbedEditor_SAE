using Microsoft.Win32;
using SimpleWorld.ToolDevelopment_SAE.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TabbedEditor.WorldEditor.Tools;

namespace TabbedEditor.WorldEditor
{
    /// <summary>
    /// Interaktionslogik für WorldEditorControl.xaml
    /// </summary>
    public partial class WorldEditorControl : UserControl, IEditorControl
    {
        //datei bearbeiten wollen und das selbe machen wie beim textfile -> Worldfile anlegen das nur die aktuellen daten beeinhaltet
        private WorldFile file = new WorldFile();

        private Dictionary<WorldEditorTool, IWorldEditorTool> tools = new Dictionary<WorldEditorTool, IWorldEditorTool>();

        //was is grad das aktive tool was benutzt wird?
        private WorldEditorTool activeTool = WorldEditorTool.LandBrush; //landbrush bei default

        public WorldEditorControl()
        {
            InitializeComponent();

            //Für jedes enum element, fügen wir 1 element in das DropDown einfügen
            //über alle enums loopen
            foreach (TileType tileType in Enum.GetValues(typeof(TileType)))
            {
                //füg jeweils dieses tileType in dieses Dropdown hinzu
                TileTypeSelector.Items.Add(tileType); //TileTypeSelector ist die ComboBox im XAML
            }
            TileTypeSelector.SelectedIndex = 0;

            tools.Add(WorldEditorTool.LandBrush, new LandBrushTool(this));
            tools.Add(WorldEditorTool.AddEnemy, new ChangeEnemyCountTool(this, 1));
            tools.Add(WorldEditorTool.RemoveEnemy, new ChangeEnemyCountTool(this, -1));

            //über die buttons drüberlaufen
            foreach (ToggleButton toolButton in ToolsToolBar.Items)//über alle kinder drüberlaufen die togglebutton sind 
            {
                toolButton.Click += ToolButton_Click;
            }

            //standardmäßig eines active setzen
            ToggleButton activeToolButton = ToolsToolBar.Items[0] as ToggleButton; 
            activeToolButton.IsChecked = true;
            activeTool = (WorldEditorTool)activeToolButton.DataContext;
        }

        private void ToolButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (ToggleButton otherToolButton in ToolsToolBar.Items) //alle deaktivieren die wir nicht geklickt haben
            {
                otherToolButton.IsChecked = false;
            }

            //sender zurückcasten in togglebutton
            ToggleButton toolButton = sender as ToggleButton;
            toolButton.IsChecked = true;
            activeTool = (WorldEditorTool)toolButton.DataContext; //in das enum reincasten, activeTool ändern, weil ihc das ActiveTool geändert hab wird genau das ausgeführt was das Tool auf Click macht
        }

        public bool UnsavedChanges { get { return false; } } //

        public event Action<string> TitleChanged;




        public void Open(string path)
        {
            file.Path = path;
            //datei öffnen und inhalt in die file schreiben
            file.Data = WorldUtils.LoadWorldData(path);
            GenerateViews(); //Show UI
            file.UnsavedChanges = false; //weil es ja grad geöffnet wurde
            SetTitle();
        }

        //Methode die unser UI aufbaut
        private void GenerateViews()
        {
            //Elemente in diesem Grid an elementen erzeugen
            //Für alle zeilen und spalten die in unserem worldFile drin sind, müssen wir row und column definitions anlegen
            //danna lles durchlaufen und fügen in unser grid jeweisl ein element hinzu

            //Row und Column Definitions
            TileData[,] tileArray = file.Data.TileArray; // fürs lesbarere

            for (int y = 0; y < tileArray.GetLength(1); y++)
            {
                RowDefinition rowDef = new RowDefinition();
                //angeben wie groß die Row werden soll
                rowDef.Height = new GridLength(30); //nicht nur 30 sondern mit GridLength
                WorldGrid.RowDefinitions.Add(rowDef);
            }
            for (int x = 0; x < tileArray.GetLength(0); x++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = new GridLength(30); //30x30 großes Element pro tile in unseren Daten
                WorldGrid.ColumnDefinitions.Add(colDef);
            }

            for (int y = 0; y < tileArray.GetLength(1); y++)
            {
                for (int x = 0; x < tileArray.GetLength(0); x++)
                {
                    TileData tileData = tileArray[x, y];
                    //Elemente in unser worldgrid hinzufügen, erstmals einfach einen button
                    WorldTileControl tileControl = new WorldTileControl(tileData); 
                    //sagen in welche row und columns es kommt, button kann also nicht irgendwie SetGridPosition methode haben
                    //also ich muss von außen sagen was das attribut vom button ist auf welche position es hinmuss
                    Grid.SetColumn(tileControl, x);
                    Grid.SetRow(tileControl, y);
                    WorldGrid.Children.Add(tileControl); //Children weil das is in der hierachie drinnen

                    //immer wenn ich auf TileControl draufdrick das sich data im hintergrund 
                    //ich will wissen wenn jemand draufgeklickt hat -> event
                    tileControl.MouseDown += TileControl_MouseDown;
                    //tileControl.MouseEnter += TileControl_MouseEnter;

                    //Die kleinen tools wollen wissen wann wir auf ein Tile geklickt haben

                }
            }
        }

        //Unpraktisch für später, aber kann man dann machen 
        ////private void TileControl_MouseEnter(object sender, MouseEventArgs e)
        ////{
        ////    if (e.LeftButton == MouseButtonState.Pressed) //wenn der linke knopf runtergedrückt ist
        ////    {
        ////        WorldTileControl tileControl = sender as WorldTileControl;
        ////        tileControl.TileType = (TileType)TileTypeSelector.SelectedValue;
        ////    }
        ////}

        private void TileControl_MouseDown(object sender, MouseButtonEventArgs e) //object sender is das element auf das wir gerade geklickt haben, weil immer als sender kommt das mit der das event auslöst -> also tileControl
        {
            //sender herholen, als worldtilecontrol casten, dann in worldtilecontrol es auf ein anderers TileType setzen
            WorldTileControl tileControl = sender as WorldTileControl;
            /*tileControl.TileType = TileType.Grass;*/ //jetzt sollt sich alles auf grass umwandelnw enn ich draufklick

            //Anstatt das ich Grass zuordne, nehm ich vom DropDown die ausgewählte Value
            //tileControl.TileType = (TileType)TileTypeSelector.SelectedValue;

            //Click auf tool weiterleiten und der kümmert sich dann darum , Worldeditorcontrol weiss nicht was passiert, das einzige was jetzt alles macht ist das LandBrush tool ! , er sgat nur : tool es wurde geklickt mach wasauchimmer du machst wenn geklickt wird 
            tools[activeTool].OnClick(tileControl, e);
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(file.Path))
            {
                SaveAs();
            }
            else //wenn wir schon einen path haben
            {
                SaveFileToDisk();
            }

        }

        public void SaveAs()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "World File (*.json)|*.json|World File (*.world)|*.world"; //können es eh nur als json file speichern
            //auf fileOK registriren
            saveDialog.FileOk += SaveDialog_FileOk;
            saveDialog.ShowDialog();

        }

        private void SaveDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //was passiert wenn der dialog fertig ist?
            SaveFileDialog saveDialog = sender as SaveFileDialog;
            //in dem worldfile den pfad richtig setzen
            file.Path = saveDialog.FileName;
            SaveFileToDisk(); // auf die festplatte speichern
        }

        private void SaveFileToDisk()
        {
            WorldUtils.SaveWorldData(file.Data, file.Path);
            file.UnsavedChanges = false;
            SetTitle();
        }


        private void SetTitle() //kann aber z.B. IEditorControl auch zu einer abstrakten klasse machen und nicht INterface dann hat es vorimplementierte methoden wie SetTitle
        {
            string title = "";
            if (string.IsNullOrEmpty(file.Path)) 
            {

                title += $"New File"; 
            }
            else //gibt einen pfad
            {
                title += $"{file.Path}"; 
            }
            title += (file.UnsavedChanges ? "*" : "");

            TitleChanged?.Invoke(title);
        }
    }
}
