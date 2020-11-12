using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TabbedEditor.TargaViewer;
using TabbedEditor.TextEditor;
using TabbedEditor.WorldEditor;

namespace TabbedEditor //Ganz normaler C# code
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Dictionary<string, Type> EndingToType = new Dictionary<string, Type>() //EndingToType kann man jetzt überall benutzen und sagen welchen type er erzeugen soll bei welcher dateiendung
        {
            //gleich die elemetne hinzufügen
            {"tga", typeof(TargaViewerControl) },
            {"txt", typeof(TextEditorControl) },//mit typeof liefert er mir ein objekt/instanz zurück von texteditorcontrol
            {"json", typeof(WorldEditorControl) },
            {"world", typeof(WorldEditorControl) }
        };


        public MainWindow()
        {
            //Wir wollen das der text den ich in dem TextEditor eingeb genau der text is der in textfile liegt, also immer direkt ins textFile hieninschreiben

            InitializeComponent();
            InfoPanelManager.Init(InfoPanel);
            TabController.SelectionChanged += TabController_SelectionChanged;
            FileHistory.FileHistoryChanged += UpdateRecentFiles;
            FileHistory.Load();

            //Ich will ein eveent haben wo ich erfahr das wer ins textfeld was eingegeben hat und was er eingeben hat
            //ans TextFeld als event registrieren
            //ich will mal ans TextFeld zugreifen! --> in XAML bei TextBox --> Name="TextEditor" dazuschreiben, TextEditor ist jetzt eine variable/member
            //gibt mega viele events !
            //Registrieren geht mit +=, das heisst alle leute die schon zuhören und wir fügen uns selbst hinzu und speicherns in TextChanged

            //Zumachen das fenster und fragen

            //RecentFileList hooking the MenuClick
            //RecentFileList.MenuClick += (s, e) => (e.Filepath);

        }

        private void TabController_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InfoPanelManager.Hide(); //um die infopanel zu verstecken wenn wir tab wechseln
        }

        //eigene Open/Save/SaveAs methoden schreiben, weil für nachher müssen wir es flexibler gestalten
        private void Open() //Open file dialog herzeigen, dann registrier mich für das event FileOk
        {
            //Umschreiben weil der kann bis jetzt nur ein txt file öffnen, aber er muss uach für WorldEditor eine JSON datei öffnen

            //Das selbe wie Save nur in gegenrichtung
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Targa File (*.tga)|*.tga|World File (*.json, *.world)|*.json;*.world|Text File (*.txt)|*.txt";
            openDialog.FileOk += OpenDialog_FileOk;
            openDialog.ShowDialog();
        }

        private void OpenDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e) //neues file aufmachen und dann neuen tab aufmachen
        {
            OpenFileDialog openDialog = sender as OpenFileDialog;

            //Path geteilt in lauter strings geteilt und gesplittet an jedem punkt? 
            string[] splittedPath = openDialog.FileName.Split('.'); //liefer array an strings zurück und kommt drauf an wie viele punkte vorkommen, uns interessiert das aller letzte element! wegen dateieendung
            string ending = splittedPath[splittedPath.Length - 1]; //liefert jetzt entweder .json oder .txt zurück

            Type type = EndingToType[ending];
            var tab = FindTab(openDialog.FileName, type);
            if (!(tab is null)) //schauen ob der tab offen ist dann selektireren wir ihn einfach
            {
                TabController.SelectedValue = tab;
                return;
            }
            IEditorControl editorControl = OpenTab(type); //IEditorControl statt TextEditorControl, weil der hat ja die OpenMethode
            try
            {
                editorControl.Open(openDialog.FileName);
            }
            catch (Exception exc)
            {
                TabController.Items.RemoveAt(TabController.Items.Count - 1);
                MessageBox.Show("Could not open file:\n" + exc.Message, "Error");
            }
            FileHistory.Add(openDialog.FileName, type);
            //FileHistory.SaveInHistory(openDialog.FileName, editorControl.GetType());

            //RecentFileList.InsertFile(openDialog.FileName);
        }

        private IEditorControl OpenTab(Type editorType) //neuen tab aufmachen //Type übergeben jetzt damit er weiss was er erzeugen soll
        {
            //zuerst ein Tab ding hinzufügen
            TabItem tab = new TabItem();
            tab.Header = "New";
            //jetzt neuen texteditorcontrol erzeugen
            //casten auf IEditorControl weil er weiss ja nicht was Type sein soll!
            IEditorControl editorControl = Activator.CreateInstance(editorType) as IEditorControl; //Beim new muss man aber je nach dem was wir öffnen unterschiedliche tabs öffnen, also nicht new IEditorControl sondern spezifischer 
            //Im OpenTab sagen welcher typ er erzeugen soll 
            //Reflection!


            //Lambda die ich genau hier definier, nicht methode automatisch erzeugen lassen!
            editorControl.TitleChanged += (string title) =>
            {
                tab.Header = title;//header auf title setzen beim tab
            };

            //Content von dem Tab setzen
            tab.Content = editorControl;
            //Dem tab einen TabController geben
            TabController.Items.Add(tab); //jetzt wird der neue tab hinzugefügt, TabController hält alle tabs
            //der aktive tab is der letzte tab in der liste
            TabController.SelectedIndex = TabController.Items.Count - 1; //immer -1 weil index wenn man das letzet haben will 3 tabs --> 0,1,2 deswegen -1, LETZTEN tab als aktiven machen
            return editorControl;
        }

        private void Save()
        {
            //Den aktuellen selektierten Content holen
            (TabController.SelectedContent as IEditorControl).Save(); //als textEditorControl casten

        }

        private void SaveAs()
        {
            (TabController.SelectedContent as IEditorControl).SaveAs();
        }

        private void HelpClicked(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();
        }

        private void CommandBinding_Open(object sender, ExecutedRoutedEventArgs e) //die 3 werden asugeführt wenn jeweils dass Command Open/Save/SaveAS ausgeführt wrd
        {
            Open();
        }

        private void CommandBinding_Save(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
        }

        private void CommandBinding_SaveAs(object sender, ExecutedRoutedEventArgs e)
        {
            SaveAs();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //wir überprüfen nicht da 1 file sondern laufen über alle tabs rüber und schauen ob einer dieser tabs unsaved changes hat !
            bool unsavedChanges = false;
            foreach (TabItem tab in TabController.Items)
            {
                //content rausholen und in den Editor casten
                IEditorControl editorControl = tab.Content as IEditorControl;
                unsavedChanges = unsavedChanges || editorControl.UnsavedChanges; //wenn 1 von den beiden true ist dann wirds true, wenn ein einziges unsavedChanges hat dann soll unsavedChanges true sein!

            }

            if (unsavedChanges)
            {
                //wenn es das gibt dann fragen wir den User, gibt standardding ist aber veraltet sollte man nicht benutzen, werdens aber trd benutzen
                //das ist die einzige ausnahme wo man das programm stoppen kann
                MessageBoxResult result = MessageBox.Show("There are unsaved changes! Do you really want to close?", "Unsaved Changes", MessageBoxButton.YesNo); //das was zurückkommt speichern wir uns in einer variable, das blockiert aber auch den UIThread
                if (result == MessageBoxResult.No) //nicht zumachen
                {
                    e.Cancel = true;//Ja ich möchte das event abbrechen!, weil unsavedChanges gibt
                }

            }
        }

        private void UpdateRecentFiles()
        {
            RecentFileList.Items.Clear(); //ansonsten bleiben die alten auch dort
            EditorFile[] history = FileHistory.GenerateList();
            if (history.Length == 0)
            {
                RecentFileList.Items.Add(new MenuItem() { Header = "Empty", IsEnabled = false });
                return;
            }

            foreach (EditorFile entry in history)
            {
                MenuItem listEntry = new MenuItem();
                listEntry.Header = $"[{FileHistory.GetEditorName(entry.Editor)}]\t    {entry.Path.Split('\\').Last()}";
                listEntry.ToolTip = entry.Path;
                listEntry.Click += RecentFilesEntryOnClick;
                listEntry.DataContext = entry;
                RecentFileList.Items.Add(listEntry);
            }

            RecentFileList.Items.Add(new Separator());
            MenuItem clearHistory = new MenuItem();
            clearHistory.Header = "Clear History";
            clearHistory.ToolTip = "Clear recent file history";
            clearHistory.Click += ClearHistoryOnClick;
            RecentFileList.Items.Add(clearHistory);
        }

        private void ClearHistoryOnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("You are about to clear the whole file history! Do you want to continue?", "Clear History", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                FileHistory.ResetHistory();
            }
        }

        private void RecentFilesEntryOnClick(object sender, RoutedEventArgs e) //zum öffnen der datei wenn der eintrag angeklickt wird
        {
            var fileHistoryEntry = (sender as MenuItem)?.DataContext as EditorFile; //weil jeder entry als datacontext eingespeichert ist oben
            var type = fileHistoryEntry.GetEditorType();

            var tab = FindTab(fileHistoryEntry.Path, type);
            if (!(tab is null)) //schauen ob der tab offen ist dann selektireren wir ihn einfach
            {
                TabController.SelectedValue = tab;
                return;
            }

            if (!typeof(IEditorControl).IsAssignableFrom(type)) //schauen ob der type der uns übergeben wurde vom IEditorControl abstammt
            {
                if (MessageBox.Show("Invalid entry! Do you want to remove it from the recent file list?", "Invalid Entry", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    FileHistory.Remove(fileHistoryEntry);
                }
                return;
            }

            if (!File.Exists(fileHistoryEntry.Path)) //falls datei verschoben/gelöscht wurde
            {
                if (MessageBox.Show("The file does not exist! Do you want to remove it from the recent file list?", "Missing file", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    FileHistory.Remove(fileHistoryEntry);
                }
                return;
            }

            IEditorControl editorControl = OpenTab(type);
            bool loadSuccess = false;

            try
            {
                editorControl.Open(fileHistoryEntry.Path);
                loadSuccess = true;
            }
            catch (Exception except)
            {
                TabController.Items.RemoveAt(TabController.Items.Count - 1);
                MessageBox.Show("Failed to load file.");
            }
            if (loadSuccess)
            {
                FileHistory.Add(fileHistoryEntry.Path, type);
            }

        }

        private TabItem FindTab(string path, Type type)
        {
            foreach (TabItem item in TabController.Items)
            {
                if (item.Content is IEditorControl child && child.GetType() == type && child.FilePath == path)
                {
                    return item;
                }
            }

            return null;
        }



    }
}
