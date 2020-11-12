using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TabbedEditor.TargaViewer;
using TabbedEditor.TextEditor;
using TabbedEditor.WorldEditor;

namespace TabbedEditor
{
    internal class SerializableFileHistory //damit ich ins JsonConvert laden und speichern kann, um die filehistory zu ladne/speichern weil die eine json ist
    {
        public EditorFile[] Entries;

        [JsonConstructor]
        public SerializableFileHistory(EditorFile[] entries)
        {
            Entries = entries;
        }
    }


    public static class FileHistory
    {
        private static readonly string AppDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/TabbedEditor/";
        private static readonly string HistoryFile = AppDataDir + "files.history"; //nur falls man sich vertippt
        private static List<EditorFile> history = new List<EditorFile>();

        public static event Action FileHistoryChanged; // damit sich das fenster subscriben kann, damit sich die recent file list updatet und das zuletzt geöffnete ganz oben ist


        private static Dictionary<string, string> EditorName = new Dictionary<string, string>()
        {
            {typeof(TargaViewerControl).ToString(), "Targa" },
            {typeof(TextEditorControl).ToString(), "Text" },
            {typeof(WorldEditorControl).ToString(), "World" },
        };

        public static string GetEditorName(string type) //gibt den eintrag ausm dictionary zurück und wenns es nciht gibt gibt es null zurück
        {
            return EditorName.ContainsKey(type) ? EditorName[type] : "NULL"; //schaut obs den type im dictionary gibt, dann gibt ers zurück wenns nicht existiert dann gibt er null zurück, falls man einen neuen editortyp hinzufügt
        }

        public static void Load()
        {
            try
            {
                if (File.Exists(HistoryFile))
                {
                    history = JsonConvert.DeserializeObject<SerializableFileHistory>(File.ReadAllText(HistoryFile)).Entries.ToList();
                }
            }
            catch (Exception e)
            {
                if (MessageBox.Show("Could not load file history!\nDo you want to reset the file history?", "Tabbed Editor - History Error", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ResetHistory();
                }
            }
            FileHistoryChanged?.Invoke();
        }

        public static void ResetHistory()
        {
            File.Delete(HistoryFile);
            history.Clear(); //array clearen
            FileHistoryChanged.Invoke(); //zum updaten
        }

        public static void Save()
        {


            try
            {
                Directory.CreateDirectory(AppDataDir);
                File.WriteAllText(HistoryFile, JsonConvert.SerializeObject(new SerializableFileHistory(history.ToArray())));
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not save file history!");
            }
        }

        public static void Add(string path, Type type)//wird ausgeführt wenn ein tab erfolgreich geöffnet wurde
        {
            EditorFile editorFile = history.FirstOrDefault(e => e.Path == path); //schaut ob es bereits exisitiert


            if (editorFile is null)
            {
                history.Insert(0, new EditorFile(path, type));
                while (history.Count > 10)
                {
                    history.RemoveAt(10); //ältesten eintrag löschen
                }
            }
            else
            {
                history.Remove(editorFile); //bereits existierenden löschen und ganz noch oben geben das es an erster stelle ist
                history.Insert(0, editorFile); //ganz nach oben geben
            }

            Save();
            FileHistoryChanged?.Invoke();
        }

        public static void Remove(EditorFile editorFile)
        {
            history.Remove(editorFile);
            Save();
            FileHistoryChanged?.Invoke();
        }

        public static void RemoveAt(int index)
        {
            //checken ob index der gelöscht werden soll ob der größer ist als die gesamte anzahl
            //wenn ja wirds nicht ausgeführt
            //wenn nicht dann entferne ich im array an dieser stelle
            if (index >= history.Count)
            {
                return;
            }

            history.RemoveAt(index);
            Save();
            FileHistoryChanged?.Invoke();
        }

        public static EditorFile[] GenerateList()
        {
            return history.ToArray();
        }

    }
}
