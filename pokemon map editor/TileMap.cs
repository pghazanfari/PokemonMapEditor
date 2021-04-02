using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonMapEditor
{
    public class TileMap
    {
        public Bitmap Image { get; }
        public IReadOnlyList<Tile> Tiles { get; }
        public PropertiedTile[,] Map { get; }

        public int TileWidth { get; }
        public int TileHeight { get; }

        public TileMap(Bitmap image, int tileWidth, int tileHeight)
        {
            Image = image;
            Map = new PropertiedTile[image.Height / tileHeight, image.Width / tileWidth];
            TileWidth = tileWidth;
            TileHeight = tileHeight;

            Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();

            for (int y = 0; y < image.Height; y += tileHeight)
            {
                for (int x = 0; x < image.Width; x += tileWidth)
                {
                    Rectangle rect = new Rectangle(x, y, tileWidth, tileHeight);
                    Tile tile = new Tile(image, rect);
                    if (!tiles.ContainsKey(tile.Hash)) { tiles[tile.Hash] = tile; }

                    Map[y / tileHeight, x / tileWidth] = new PropertiedTile(tiles[tile.Hash]);
                }
            }

            Tiles = new List<Tile>(tiles.Values.ToList()).AsReadOnly();
        }

        public void Save(string filename)
        {
            int ylen = Map.GetLength(0);
            int xlen = Map.GetLength(1);

            Dictionary<string, string>[,] data = new Dictionary<string, string>[ylen, xlen];
            for (int y = 0; y < ylen; y++)
            {
                for (int x = 0; x < xlen; x++)
                {
                    data[y, x] = Map[y, x].Properties;
                }
            }

            Dictionary<string, object> output = new Dictionary<string, object>();
            output["shape"] = new int[] { ylen, xlen };
            output["data"] = data;

            using (StreamWriter file = new StreamWriter(new FileStream(filename, FileMode.Create), Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, output);
            }
        }
    }
}