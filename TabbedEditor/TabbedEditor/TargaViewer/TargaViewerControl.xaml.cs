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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TabbedEditor.TargaViewer.TargaReader;

namespace TabbedEditor.TargaViewer
{
    /// <summary>
    /// Interaktionslogik für TargaViewerControl.xaml
    /// </summary>
    public partial class TargaViewerControl : UserControl, IEditorControl
    {
        public string FilePath { get; private set; }

        public TargaViewerControl()
        {
            InitializeComponent();
        }

        public bool UnsavedChanges { get { return false; } } //immer false weil wir viewen nur was

        public event Action<string> TitleChanged;

        public void Open(string path)
        {
            FilePath = path;
            TitleChanged?.Invoke(path.Split('\\').Last());
            TargaFile targaFile = TargaFile.Read(path);

            //das macht es nur leihcter zum lesen
            int width = targaFile.Header.Width;
            int height = targaFile.Header.Height;

            WriteableBitmap bitmap = new WriteableBitmap(width, height, 90, 90, PixelFormats.Bgra32, null); //)0 is standard

            //Über alles drüberlaufen und die informationen in das ding reinschreiebn
            //die information die ich reinschreib ist ein array an bytes
            //Color is auch nur 4 bytes hintereinander
            //1 dimesniuonales byte array
            byte[] buffer = new byte[width * height * 4]; //1 pixel is ja 4 bytes hoch also * 4, 4 bytes pro pixel

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //auf welcher position ist es dann auf der x und y position?
                    int pos = (x + y * width) * 4; //mit width multiiplizeiren, weil mit jeder zeile die wir nachunten gehen .. ehm ? 
                    //mit width multipliziert weil -->  x 1 y 2 da müsst 9 rauskommen, x 1 + die 2 zeilend rüber dazuzählen, 2 * 4   also 8 dazuzählen

                    //X spiegeln weil WPF und targa
                    var lookUpX = width - x - 1;
                    //Hier die farben den pixels zuweisen, jedem kanal
                    buffer[pos] = targaFile.Pixels[lookUpX, y].B;
                    buffer[pos + 1] = targaFile.Pixels[lookUpX, y].G;
                    buffer[pos + 2] = targaFile.Pixels[lookUpX, y].R;
                    buffer[pos + 3] = targaFile.Pixels[lookUpX, y].A;
                }
            }

            //wohin soll ers laden
            Int32Rect rect = new Int32Rect(0, 0, width, height); //kein offset und so breit und groß wie das bild

            bitmap.WritePixels(rect, buffer, width * 4, 0); //wie viele bytes hat 1 zeile im bild? 4 ! offset 0 

            OutputImage.Source = bitmap;

        }

        public void Save()
        {
        }

        public void SaveAs()
        {
        }
    }
}
