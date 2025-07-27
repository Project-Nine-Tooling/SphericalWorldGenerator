using SphericalWorldGenerator.Framework;
using SphericalWorldGenerator.Maths;
using SphericalWorldGenerator.Media;
using System.Collections.Generic;

namespace SphericalWorldGenerator.DataTypes
{
    public enum HeightType
    {
        DeepWater = 1,
        ShallowWater = 2,
        Shore = 3,
        Sand = 4,
        Grass = 5,
        Forest = 6,
        Rock = 7,
        Snow = 8,
        River = 9
    }

    public enum HeatType
    {
        Coldest = 0,
        Colder = 1,
        Cold = 2,
        Warm = 3,
        Warmer = 4,
        Warmest = 5
    }

    public enum MoistureType
    {
        Wettest = 5,
        Wetter = 4,
        Wet = 3,
        Dry = 2,
        Dryer = 1,
        Dryest = 0
    }

    public enum BiomeType
    {
        Desert,
        Savanna,
        TropicalRainforest,
        Grassland,
        Woodland,
        SeasonalForest,
        TemperateRainforest,
        BorealForest,
        Tundra,
        Ice
    }

    public class Tile
    {
        #region Properties
        public HeightType HeightType;
        public HeatType HeatType;
        public MoistureType MoistureType;
        public BiomeType BiomeType;

        public float Cloud1Value { get; set; }
        public float Cloud2Value { get; set; }
        public float HeightValue { get; set; }
        public float HeatValue { get; set; }
        public float MoistureValue { get; set; }
        public int X, Y;
        public int Bitmask;
        public int BiomeBitmask;

        public Tile Left;
        public Tile Right;
        public Tile Top;
        public Tile Bottom;

        public bool Collidable;
        public bool FloodFilled;

        public Color Color = Colors.Black;
        public List<River> Rivers = [];

        public int RiverSize { get; set; }
        #endregion

        #region Construction
        public Tile()
        {
        }
        #endregion

        public void UpdateBiomeBitmask()
        {
            int count = 0;

            if (Collidable && Top != null && Top.BiomeType == BiomeType)
                count += 1;
            if (Collidable && Bottom != null && Bottom.BiomeType == BiomeType)
                count += 4;
            if (Collidable && Left != null && Left.BiomeType == BiomeType)
                count += 8;
            if (Collidable && Right != null && Right.BiomeType == BiomeType)
                count += 2;

            BiomeBitmask = count;
        }

        public void UpdateBitmask()
        {
            int count = 0;

            if (Collidable && Top != null && Top.HeightType == HeightType)
                count += 1;
            if (Collidable && Right != null && Right.HeightType == HeightType)
                count += 2;
            if (Collidable && Bottom != null && Bottom.HeightType == HeightType)
                count += 4;
            if (Collidable && Left != null && Left.HeightType == HeightType)
                count += 8;

            Bitmask = count;
        }

        public int GetRiverNeighborCount(River river)
        {
            int count = 0;
            if (Left != null && Left.Rivers.Count > 0 && Left.Rivers.Contains(river))
                count++;
            if (Right != null && Right.Rivers.Count > 0 && Right.Rivers.Contains(river))
                count++;
            if (Top != null && Top.Rivers.Count > 0 && Top.Rivers.Contains(river))
                count++;
            if (Bottom != null && Bottom.Rivers.Count > 0 && Bottom.Rivers.Contains(river))
                count++;
            return count;
        }

        public Direction GetLowestNeighbor(Generator generator)
        {
            float left = generator.GetHeightValue(Left);
            float right = generator.GetHeightValue(Right);
            float bottom = generator.GetHeightValue(Bottom);
            float top = generator.GetHeightValue(Top);

            if (left < right && left < top && left < bottom)
                return Direction.Left;
            else if (right < left && right < top && right < bottom)
                return Direction.Right;
            else if (top < left && top < right && top < bottom)
                return Direction.Top;
            else if (bottom < top && bottom < right && bottom < left)
                return Direction.Bottom;
            else
                return Direction.Bottom;
        }

        public void SetRiverPath(River river)
        {
            if (!Collidable)
                return;

            if (!Rivers.Contains(river))
            {
                Rivers.Add(river);
            }
        }

        private void SetRiverTile(River river)
        {
            SetRiverPath(river);
            HeightType = HeightType.River;
            HeightValue = 0;
            Collidable = false;
        }

        // This function got messy.  Sorry.
        public void DigRiver(River river, int size)
        {
            SetRiverTile(river);
            RiverSize = size;

            if (size == 1)
            {
                if (Bottom != null)
                {
                    Bottom.SetRiverTile(river);
                    Bottom.Right?.SetRiverTile(river);
                }
                Right?.SetRiverTile(river);
            }

            if (size == 2)
            {
                if (Bottom != null)
                {
                    Bottom.SetRiverTile(river);
                    Bottom.Right?.SetRiverTile(river);
                }
                Right?.SetRiverTile(river);
                if (Top != null)
                {
                    Top.SetRiverTile(river);
                    Top.Left?.SetRiverTile(river);
                    Top.Right?.SetRiverTile(river);
                }
                if (Left != null)
                {
                    Left.SetRiverTile(river);
                    Left.Bottom?.SetRiverTile(river);
                }
            }

            if (size == 3)
            {
                if (Bottom != null)
                {
                    Bottom.SetRiverTile(river);
                    Bottom.Right?.SetRiverTile(river);
                    if (Bottom.Bottom != null)
                    {
                        Bottom.Bottom.SetRiverTile(river);
                        Bottom.Bottom.Right?.SetRiverTile(river);
                    }
                }
                if (Right != null)
                {
                    Right.SetRiverTile(river);
                    if (Right.Right != null)
                    {
                        Right.Right.SetRiverTile(river);
                        Right.Right.Bottom?.SetRiverTile(river);
                    }
                }
                if (Top != null)
                {
                    Top.SetRiverTile(river);
                    Top.Left?.SetRiverTile(river);
                    Top.Right?.SetRiverTile(river);
                }
                if (Left != null)
                {
                    Left.SetRiverTile(river);
                    Left.Bottom?.SetRiverTile(river);
                }
            }

            if (size == 4)
            {

                if (Bottom != null)
                {
                    Bottom.SetRiverTile(river);
                    Bottom.Right?.SetRiverTile(river);
                    if (Bottom.Bottom != null)
                    {
                        Bottom.Bottom.SetRiverTile(river);
                        Bottom.Bottom.Right?.SetRiverTile(river);
                    }
                }
                if (Right != null)
                {
                    Right.SetRiverTile(river);
                    if (Right.Right != null)
                    {
                        Right.Right.SetRiverTile(river);
                        Right.Right.Bottom?.SetRiverTile(river);
                    }
                }
                if (Top != null)
                {
                    Top.SetRiverTile(river);
                    if (Top.Right != null)
                    {
                        Top.Right.SetRiverTile(river);
                        Top.Right.Right?.SetRiverTile(river);
                    }
                    if (Top.Top != null)
                    {
                        Top.Top.SetRiverTile(river);
                        Top.Top.Right?.SetRiverTile(river);
                    }
                }
                if (Left != null)
                {
                    Left.SetRiverTile(river);
                    if (Left.Bottom != null)
                    {
                        Left.Bottom.SetRiverTile(river);
                        Left.Bottom.Bottom?.SetRiverTile(river);
                    }

                    if (Left.Left != null)
                    {
                        Left.Left.SetRiverTile(river);
                        Left.Left.Bottom?.SetRiverTile(river);
                        Left.Left.Top?.SetRiverTile(river);
                    }

                    if (Left.Top != null)
                    {
                        Left.Top.SetRiverTile(river);
                        Left.Top.Top?.SetRiverTile(river);
                    }
                }
            }
        }
    }
}