using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonMapEditor
{
    public class PropertiedTile
    {
        public Tile Tile { get; }
        public Dictionary<string, string> Properties { get; }

        public PropertiedTile(Tile tile, Dictionary<string, string> properties)
        {
            Tile = tile;
            Properties = properties;
        }

        public PropertiedTile(Tile tile) : this(tile, new Dictionary<string, string>()) { }
    }
}
