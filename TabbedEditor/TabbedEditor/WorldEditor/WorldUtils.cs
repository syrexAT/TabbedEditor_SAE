using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleWorld.ToolDevelopment_SAE.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using worldProto;

namespace TabbedEditor.WorldEditor
{
    public static class WorldUtils //mit static gibt es nichtmal mehr eine instnaz von der klasse, kann hier nichtmal mehr eine nicht static methode reinschreiben
    {
        public static List<int> tiles = new List<int>();
        public static List<int> waterTiles = new List<int>();
        public static bool viable = false;

        public static WorldData LoadWorldData(string path)
        {
            WorldData worldData = null;
            if (path.EndsWith(".json"))
            {
                worldData = LoadJsonData(path);
            }
            else if (path.EndsWith(".world")) //.world sind jetzt die binär daten die haben eig keine dateiendung wir nennens einfach .world
            {
                worldData = LoadProtoData(path);
            }

            return worldData;
        }

        public static void SaveWorldData(WorldData worldData, string path)
        {

            CheckTiles(worldData);
            if (viable)
            {
                if (path.EndsWith(".json"))
                {
                    SaveJsonData(worldData, path);
                }
                else if (path.EndsWith(".world"))
                {
                    SaveProtoData(worldData, path);
                }
            }


        }

        private static void SaveProtoData(WorldData worldData, string path)
        {
            WorldMessage worldMessage = ToWorldMessage(worldData); //worldData in das datenobjekt umwandeln mit dem der proto Serializer umgehen kann
            //in eine datei schreiben mit filestream, also so wie bei den bilddateien
            //wir brauchne nur eienn stream aufmachen und den übergeben wir den serializer und der macht alles
            FileStream stream = File.Open(path, FileMode.Create); //wenns schon da is wirds überschrieben wenns nicht so is wirds erstellt --> asuch bei recent files verwenden!
            ProtoSerializer serializer = new ProtoSerializer(); //dem sag ich jz pl sserializier mir das raus
            serializer.Serialize(stream, worldMessage);
            stream.Close();
        }

        private static WorldData LoadProtoData(string path)
        {
            FileStream stream = File.Open(path, FileMode.Open); //Filemode open weil wir wollen einlesen und nix erzeugen
            ProtoSerializer serializer = new ProtoSerializer();
            //man muss im serializer bereits ein objekt reinstopfen das er dann befüllen wird, also ein worldmessage zuerst einfach erzeugen
            WorldMessage worldMessage = new WorldMessage();
            serializer.Deserialize(stream, worldMessage, typeof(WorldMessage));
            stream.Close();
            //Protocol buffer daten wieder in normale daten(json) umwandeln
            WorldData worldData = ToWorldData(worldMessage);
            return worldData;
        }

        //Lade mir das ding aus einer datei
        //Speicher mir das ding aus einer datei
        private static WorldData LoadJsonData(string path)//static weil dann brauch ich keine instanz von WorldUtils
        {
            //Laden zuerst mit fileLoad die string json rein
            string json = File.ReadAllText(path);
            //Text deserializen in die tatsächliche worlddata
            WorldData worldData = JsonConvert.DeserializeObject<WorldData>(json);
            return worldData;
        }

        private static void SaveJsonData(WorldData worldData, string path)
        {
            string json = JsonConvert.SerializeObject(worldData);
            File.WriteAllText(path, json);
        }


        public static WorldMessage ToWorldMessage(WorldData worldData)
        {
            //wir konvertieren diese WorldData in eine WorldMessage
            WorldMessage worldMessage = new WorldMessage();
            //Durchs tilearary durchlaufen und befüllen
            for (int y = 0; y < worldData.TileArray.GetLength(1); y++)
            {
                //jede zeile in unseren daten eine neue zeile worlddata hinzufügen
                RowMessage rowMessage = new RowMessage();
                worldMessage.tileArray.Add(rowMessage);
                for (int x = 0; x < worldData.TileArray.GetLength(0); x++)
                {
                    TileData tileData = worldData.TileArray[x, y];
                    TileDataMessage tileMessage = new TileDataMessage();
                    tileMessage.tileType = (int)tileData.TileType; //auf int casten weils in protocol buffer ja int32 sind
                    tileMessage.enemyCount = tileData.EnemyCount;
                    rowMessage.row.Add(tileMessage);
                }
            }

            return worldMessage;
        }

        //von protodaten wieder in jsondaten kommen, also worldmessage in worlddata umwandeln
        public static WorldData ToWorldData(WorldMessage worldMessage)
        {
            WorldData worldData = new WorldData();

            //2 dimensionales tilearray also breite und höhe
            //schritt 1 --> 
            int height = worldMessage.tileArray.Count; //wie viele zeilen wir haben
            int width = worldMessage.tileArray[0].row.Count; //erste zeiel holen und schauen wie lange die erste zeile ist

            worldData.TileArray = new TileData[width, height];
            //wieder über x und y drüberlaufen und die daten richtig befüllen
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //als erstes die tiledatamessage holen, di edaten die wir kompilieren werden, also die protocol buffer daten holen
                    TileDataMessage tileMessage = worldMessage.tileArray[y].row[x];
                    TileData tileData = new TileData();
                    tileData.TileType = (TileType)tileMessage.tileType; //zurück casten von int auf unser enum tiletype
                    tileData.EnemyCount = tileMessage.enemyCount;
                    worldData.TileArray[x, y] = tileData;
                }
            }

            return worldData;
        }

        public static void CheckTiles(WorldData worldData)
        {
            for (int y = 0; y < worldData.TileArray.GetLength(1); y++)
            {
                for (int x = 0; x < worldData.TileArray.GetLength(0); x++)
                {
                    TileData tileData = worldData.TileArray[x, y];
                    if (tileData.TileType == TileType.Water)
                    {
                        waterTiles.Add(1);
                        //tiles.Add(1);
                    }
                    else
                    {
                        tiles.Add(1);
                    }
                }
            }

            if (waterTiles.Count > tiles.Count)
            {
                viable = false;
            }
            else
            {
                viable = true;
            }

            //die ans ende !
            waterTiles.Clear();
            tiles.Clear();

        }

    }
}
