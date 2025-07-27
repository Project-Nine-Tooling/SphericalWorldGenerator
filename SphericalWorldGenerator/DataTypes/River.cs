using System.Collections.Generic;

namespace SphericalWorldGenerator.DataTypes
{
    public enum Direction
    {
        Left,
        Right,
        Top,
        Bottom
    }

    public class River
    {

        public int Length;
        public List<Tile> Tiles;
        public int ID;

        public int Intersections;
        public float TurnCount;
        public Direction CurrentDirection;

        public River(int id)
        {
            ID = id;
            Tiles = [];
        }

        public void AddTile(Tile tile)
        {
            tile.SetRiverPath(this);
            Tiles.Add(tile);
        }
    }

    public class RiverGroup
    {
        public List<River> Rivers = [];
    }
}