using System.Collections.Generic;

namespace SphericalWorldGenerator.DataTypes
{
    public enum TileGroupType
    {
        Water,
        Land
    }

    /// <summary>
    /// Represents connected "strips" of lands.
    /// </summary>
    public class TileGroup
    {
        public TileGroupType Type;
        public List<Tile> Tiles;
        public int Size => Tiles.Count;

        public TileGroup()
        {
            Tiles = [];
        }
    }
}