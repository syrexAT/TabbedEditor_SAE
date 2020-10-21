using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbedEditor
{
    static class FileHistory
    {
        public static List<string> recentFileList = new List<string>();

        public static void SaveInHistory(string fileName, Type editorType) //rückgabe? string? IEditorControl?
        {
            recentFileList.Add(fileName);

            string json = JsonConvert.SerializeObject(editorType);
            File.WriteAllText(fileName, json);
        }

        //public static string LoadHistory(string path)
    }
}
