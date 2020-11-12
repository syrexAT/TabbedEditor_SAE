using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TabbedEditor.TextEditor
{
    /// <summary>
    /// Interaktionslogik für TextEditorControl.xaml
    /// </summary>
    public partial class TextEditorControl : UserControl, IEditorControl //TextEditorControl kann man jetzt auch immmer von IEditorControl usen
    {
        public event Action<string> TitleChanged; //damit wir die Tab Titles verändert können
        //Action ist ein callback zu dem man sihc registrieren kann, ein string wird zurückgelifert, quasi das selbe wie bei einer Liste aufgebaut
        public string FilePath => file.Path;

        private TextFile file = new TextFile(); //Member von MainWindow und wir holen und das TextFile, es is direkt ein neues file und kann somit nicht null sein
        public TextEditorControl()
        {
            InitializeComponent();

            TextEditor.TextChanged += TextEditor_TextChanged; //händische registrierung, immer wenn sich der text ändert, ändere ich auch den text im file
        }

        //Diese methode ist jetzt dem Event TextChanged hinzugefügt
        //es gibt immer ein sender, object is das niedrigste, davon erbt alles, das kann alles sein, sender ist das was das event ausgelöst hat
        //TextEditor ist der sender! brauchen wir später
        //das zweite sind Argumente die definieren was in dem Event gerade passiert ist (z.B. in welche form sich der Text geändert hat !)
        private void TextEditor_TextChanged(object sender, TextChangedEventArgs e) //automatische methode hat er geschrieben, beim TextChanged sagt er zum TextChanged registriere mit die TextEditor_TExtChanged hinzu
        {
            //e.Changes sagt was sich geändert hat
            file.Data = TextEditor.Text; //jetzt fügt er das der variable file.Data hinzu -- mitändern!

            //moment der text hat sich geändert! also direkt UnsavedChanges auf true ändern!
            file.UnsavedChanges = true;
            SetTitle(); //title updaten das der tabtitle aktuell is, sternchen dazukommt usw.

            //wir wollen das jetzt irgendwo hinspeichern also brauchne wi rein menu
        }

        //eigene Open/Save/SaveAs methoden schreiben, weil für nachher müssen wir es flexibler gestalten
        public void Open(string path)
        {
            file.Path = path;
            //direkt den text reinalden
            file.Data = File.ReadAllText(file.Path); //text aus file rauslesen und in data speichern
            //der text der im textwindow angezeigt wird auch updaten!
            TextEditor.Text = file.Data; //dann da reinspeichern was wir ausgelesen haben
            file.UnsavedChanges = false; //gibt keine ungespeicherten ädnerugnen weil wir es ja gerade geöffnet haben
            SetTitle();
        }

        //Titel zum korrekten titel setzen
        //er sagt jetzt nur das sein name sich geändert hat
        private void SetTitle()
        {
            //man kann von sich selbst, die eigenschaften mit this setzen
            //gibts ein file name?
            string title = "";
            if (string.IsNullOrEmpty(file.Path)) // also es gibt noch keinen pfad
            {

                title += $"New File"; //Dollar zeichen jetzt kann man properties in geschwungene klammern dazuschreiben, also einen string zusammenbauen
            }
            else //gibt einen pfad
            {
                title += $"{file.Name}"; //ich möchte anzheigen ob wir schon gespeichert haben oder nicht 
            }
            //If else in einer zeile, erstes ist If --> wenn true dann * sonst ""
            title += (file.UnsavedChanges ? "*" : "");

            TitleChanged?.Invoke(title); //title ist der parameter bei dem der zuhört ausgeführt wird

            //event auslösen das was passiert also das sich der name geändert at
            //wir bieten an das uns wer zuhört bis wir sagen bitte es is was passiert!


        }

        public void Save()
        {
            //is es schonmal gespeichert worden?
            if (!string.IsNullOrEmpty(file.Path))
            {
                SaveFileToDisk(); //wenn ja speicher es auf den pfad(festplatte)
            }
            else
            {
                SaveAs(); //wenn nicht öffne saveas -> Speichern unter dialog öffnen
            }
        }

        public void SaveAs()
        {
            //Speichern unter
            //dann soll sich wohin möchtest du die Datei speichern kommen, wird nicht selber geschrieben obviously, gibt ein fertiges dialogsystem
            SaveFileDialog saveDialog = new SaveFileDialog();
            //wenn ich so ein SaveFileDialog aufmach, dann gibts unten immer ein DropDown mit als was will mans speichern (wir wollen .txt)
            //string besteht aus sich wiederholenden 2 teilen, das was angezeigt wird im dropdown, und das was er wirklich im filesystem filtert
            saveDialog.Filter = "Text File (*.txt)|*.txt";

            //wir wollen wissen wann der dialog fertig ist, das ist dann wenn er uns ein event gibt
            saveDialog.FileOk += SaveDialog_FileOk; 
            //anzeigen
            saveDialog.ShowDialog();
        }

        private void SaveDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //der user hat geklickt --> Ok
            //aus dem SaveFileDialog rausholen was eigentlich eingegeben wurde! es ist aber oben eine lokale variable im SaveAs Methode
            //man kann ihn natürlich irgendwie als member speichern vom MainWinodw
            //ABER wir können diesen sender benutzen, der ist immer das object das die methode auslöst, also das SaveFileDialog ist unser Sender
            //wir müssen ihn nur richtig casten
            SaveFileDialog saveDialog = sender as SaveFileDialog; //damit hab ich den SaveDialog auch in der Methode lokal zur verfügung
            //FileName ist der komplette Pfad!
            file.Path = saveDialog.FileName; //jetzt methode schreiben die das file irgendwo hinspeichert, werden wir uach beim normalen Save benutzen!
            SaveFileToDisk();
        }



        private void SaveFileToDisk() //speichert es tatsächlich auf die festplatte
        {
            //Theoretisch noch überprüfen ob er da überhaupt hinspeichern DARF in den path aber das machen wir jetzt nicht
            File.WriteAllText(file.Path, file.Data); //Path und Data(Content)
            file.UnsavedChanges = false;
            SetTitle();
        }

        public bool UnsavedChanges //das man von aussenhin fragen kann ob der tab sachen hat die nciht gespeichert sind
        {
            get
            {
                return file.UnsavedChanges;
            }
        }



    }
}
