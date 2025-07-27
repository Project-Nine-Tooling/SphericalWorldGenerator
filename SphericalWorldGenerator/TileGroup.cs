using System.Collections.Generic;

namespace SphericalWorldGenerator
{
    public enum TileGroupType
    {
        Water,
        Land
    }

    public class TileGroup
    {

        public TileGroupType Type;
        public List<Tile> Tiles;

        public TileGroup()
        {
            Tiles = new List<Tile>();
        }
    }
}