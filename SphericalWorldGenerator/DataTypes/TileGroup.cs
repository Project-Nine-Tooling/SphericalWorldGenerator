using System.Collections.Generic;

namespace SphericalWorldGenerator.DataTypes
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
            Tiles = [];
        }
    }
}