using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbedEditor
{
    interface IEditorControl
    {
        //Sie haben alle gemein das man ein event hat wo man sich registriert das sich der Title gechanged hat
        //Interface kann nur public methoden haben, aber C# hat properties, die sind so in der mitte zwischen methodne und variablen
        //2 Methoden in einer zusammengefasst getter, setter methode --> properties kann interface auch haben!

        //Getter/Setter für so eine Action wo man sich registrieren kann das sich der title gechagned hat
        event Action<string> TitleChanged; //doch ohne getter und setter sonst geht event nicht

        string FilePath { get; }

        //TextChanged interresiert unds nicht weil das wird im WorldEditor nicht sein

        //Open --> Interessiert uns!
        void Open(string path); //ich sag dieses interface soll eine open methode haben

        //SetTitle --> Ist nur eine interne methode (private), interen definition für den TextEditorControl

        //Save brauchen wir
        void Save();

        //SaveAs bruachen wir auch
        void SaveAs(); //jeder tab kümmert sich selbst drum was für ein savefiledialog angezeigt wird

        //SaveFileToDisk muss jeder für sich selber machen also auch nicht im Interface
        //UnsavedChanges bruachen wir aber wiederrum
        bool UnsavedChanges { get; }

    }
}
