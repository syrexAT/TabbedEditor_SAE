using SimpleWorld.ToolDevelopment_SAE.Data;
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

namespace TabbedEditor.WorldEditor
{
    /// <summary>
    /// Interaktionslogik für WorldTileControl.xaml
    /// </summary>
    public partial class WorldTileControl : UserControl
    {
        private TileData tile; //wenn ich hier am tile im worldtilecontrol was änder dann speichert es das auch in den Daten weil es nur eine Referenz ist!
        //das habenw ir damit gemacht das TileData TileType enthält und das TileType die enums!! so wie in der ersten stunde was wir besprochen haben  

        private static Dictionary<TileType, Color> TileTypeToColor = new Dictionary<TileType, Color>()
        {
            { TileType.Grass, Colors.DarkGreen },
            { TileType.Sand, Colors.SandyBrown },
            { TileType.Stone, Colors.DarkSlateGray },
            { TileType.Water, Colors.Blue }
        };

        public WorldTileControl(TileData tile)
        {
            this.tile = tile;
            InitializeComponent();
            UpdateData();
        }

        //Methode das ich von außen den landType holen kann also ein getter/setter, wenn ich den von aussen setz das update ich automatisch die data im hintergrund(UpdateData())
        public TileType TileType
        {
            set
            {
                tile.TileType = value; //wenn ichs von aussen setz setz ich hier drin den richtigen tiletype
                UpdateData(); // und update die data
            }
            get
            {
                return tile.TileType;
            }
        }

        public int EnemyCount
        {
            get
            {
                return tile.EnemyCount;
            }
            set
            {
                tile.EnemyCount = value;
                UpdateData();
            }
        }

        //Methode die die richtige Hintergrundfarbe setzt
        private void UpdateData()
        {
            //nicht nur blau sondern auf die jeweilige richtige data, wir könnten swtich case machne aber nein wir wollen das zentral konfigurieren können, also wir machen es so wie mit den fileendings mit dem statischen Dictionary!
            TileBackground.Background = new SolidColorBrush(TileTypeToColor[tile.TileType]); //Solid weil die linie durchgezogen sein soll
            EnemyCountLabel.Content = tile.EnemyCount.ToString(); //wird als content rausgeschrieben
        }



    }
}
