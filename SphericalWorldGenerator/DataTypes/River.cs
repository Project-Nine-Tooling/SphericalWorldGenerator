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
        #region Properties
        public int Length;
        public readonly List<Tile> Tiles;
        public int ID;

        public int Intersections;
        public float TurnCount;
        public Direction CurrentDirection;
        #endregion

        #region Construction
        public River(int id)
        {
            ID = id;
            Tiles = [];
        }
        #endregion

        #region Methods
        public void AddTile(Tile tile)
        {
            tile.SetRiverPath(this);
            Tiles.Add(tile);
        }
        #endregion
    }

    public class RiverGroup
    {
        public List<River> Rivers = [];
    }
}