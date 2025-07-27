using AccidentalNoise;
using SphericalWorldGenerator.DataTypes;
using SphericalWorldGenerator.Maths;
using SphericalWorldGenerator.Media;
using SphericalWorldGenerator.Texturing;
using System.Collections.Generic;

namespace SphericalWorldGenerator.Framework
{
    public abstract class Generator
    {
        #region Configuration
        protected int Seed;
        #endregion

        #region Generator Values
        protected int Width = 16000;
        protected int Height = 16000;
        #endregion

        #region Height Map
        protected int TerrainOctaves = 6;
        protected double TerrainFrequency = 1.25;
        protected float DeepWater = 0.2f;
        protected float ShallowWater = 0.4f;
        protected float Sand = 0.5f;
        protected float Grass = 0.7f;
        protected float Forest = 0.8f;
        protected float Rock = 0.9f;
        #endregion

        #region Heat Map
        protected int HeatOctaves = 4;
        protected double HeatFrequency = 3.0;
        protected float ColdestValue = 0.05f;
        protected float ColderValue = 0.18f;
        protected float ColdValue = 0.4f;
        protected float WarmValue = 0.6f;
        protected float WarmerValue = 0.8f;
        #endregion

        #region Moisture Map
        protected int MoistureOctaves = 4;
        protected double MoistureFrequency = 3.0;
        protected float DryerValue = 0.27f;
        protected float DryValue = 0.4f;
        protected float WetValue = 0.6f;
        protected float WetterValue = 0.8f;
        protected float WettestValue = 0.9f;
        #endregion

        #region Rivers
        protected int RiverCount = 40;
        protected float MinRiverHeight = 0.6f;
        protected int MaxRiverAttempts = 1000;
        protected int MinRiverTurns = 18;
        protected int MinRiverLength = 20;
        protected int MaxRiverIntersections = 2;
        #endregion

        #region Data
        protected MapData HeightData;
        protected MapData HeatData;
        protected MapData MoistureData;
        protected MapData Clouds1;
        protected MapData Clouds2;

        /// <remarks>
        /// TODO: Replace with jagged array instead of 2D array, and in all enumerators, change enumeration order to be row-first (row-major) for access locality.
        /// </remarks>
        protected Tile[,] Tiles;
        #endregion

        #region Data Groups
        /// <remarks>
        /// Break down connected pieces of water into TileGroup collections.
        /// Remark-cz-20250727: Doesn't seem to be used in current algorithm.
        /// </remarks>
        protected List<TileGroup> Waters = [];
        /// <remarks>
        /// Break down connected pieces of land into TileGroup collections.
        /// Remark-cz-20250727: Doesn't seem to be used in current algorithm.
        /// </remarks>
        protected List<TileGroup> Lands = [];

        /// <summary>
        /// Paths on land leading to water.
        /// </summary>
        protected List<River> Rivers = [];
        /// <summary>
        /// Groups of rivers, that intersect and leads to water.
        /// </summary>
        protected List<RiverGroup> RiverGroups = [];
        #endregion

        #region Texture Outputs
        public Texture2D TerrainTypeMapRenderer;
        public Texture2D HeatMapRenderer;
        public Texture2D MoistureMapRenderer;
        public Texture2D BiomeMapRenderer;
        #endregion

        #region Configurations
        /// <summary>
        /// Per Whittaker’s model.
        /// </summary>
        protected BiomeType[,] BiomeTable = new BiomeType[6, 6] {   
		    //COLDEST        //COLDER          //COLD                  //HOT                          //HOTTER                       //HOTTEST
		    { BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert },              //DRYEST
		    { BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert },              //DRYER
		    { BiomeType.Ice, BiomeType.Tundra, BiomeType.Woodland,     BiomeType.Woodland,            BiomeType.Savanna,             BiomeType.Savanna },             //DRY
		    { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.Woodland,            BiomeType.Savanna,             BiomeType.Savanna },             //WET
		    { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.SeasonalForest,      BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest },  //WETTER
		    { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.TemperateRainforest, BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest }   //WETTEST
	    };
        #endregion

        #region Framework
        public void Start()
        {
            Instantiate();
            Generate();
        }
        protected virtual void Instantiate()
        {
            Seed = Random.Range(0, int.MaxValue);
            Initialize();
        }
        protected abstract void Initialize();
        protected abstract void PopulateData();
        protected virtual void Generate()
        {
            PopulateData();
            BuildTiles();
            UpdateNeighbors();

            GenerateRivers();
            BuildRiverGroups();
            DigRiverGroups();
            AdjustMoistureMap();

            UpdateBitmasks();
            FloodFill();

            GenerateBiomeMap();
            UpdateBiomeBitmask();

            TerrainTypeMapRenderer = TextureGenerator.GetTerrainTypeTexture(Width, Height, Tiles);
            HeatMapRenderer = TextureGenerator.GetHeatMapTexture(Width, Height, Tiles);
            MoistureMapRenderer = TextureGenerator.GetMoistureMapTexture(Width, Height, Tiles);
            BiomeMapRenderer = TextureGenerator.GetBiomeMapTexture(Width, Height, Tiles, ColdestValue, ColderValue, ColdValue);
        }
        #endregion

        #region Methods
        protected abstract Tile GetTop(Tile tile);
        protected abstract Tile GetBottom(Tile tile);
        protected abstract Tile GetLeft(Tile tile);
        protected abstract Tile GetRight(Tile tile);
        public BiomeType GetBiomeType(Tile tile)
            => BiomeTable[(int)tile.MoistureType, (int)tile.HeatType];
        public float GetHeightScale(Tile tile)
        {
            if (tile == null)
                return int.MaxValue;
            else
                return tile.HeightRatio;
        }
        public Texture2D GetHeightMap(bool normalizeHeights = false)
        {
            if (normalizeHeights)
                throw new System.NotImplementedException();
            else
                return TextureGenerator.GetHeightMapTexture(Width, Height, Tiles);
        }
        #endregion

        #region Gameplay
        public void Update()
        {
            // Refresh with new seed value
            // Remark: This also takes into effect any parameter updates
            Seed = Random.Range(0, int.MaxValue);
            Initialize();
            Generate();
        }
        #endregion

        #region Routines
        private void UpdateBiomeBitmask()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Tiles[x, y].UpdateBiomeBitmask();
                }
            }
        }
        private void GenerateBiomeMap()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (!Tiles[x, y].Collidable) 
                        continue;

                    Tile t = Tiles[x, y];
                    t.BiomeType = GetBiomeType(t);
                }
            }
        }
        private void AddMoisture(Tile t, int radius)
        {
            int startx = MathHelper.Mod(t.X - radius, Width);
            int endx = MathHelper.Mod(t.X + radius, Width);
            Vector2 center = new(t.X, t.Y);
            int curr = radius;

            while (curr > 0)
            {
                int x1 = MathHelper.Mod(t.X - curr, Width);
                int x2 = MathHelper.Mod(t.X + curr, Width);
                int y = t.Y;

                AddMoisture(Tiles[x1, y], 0.025f / (center - new Vector2(x1, y)).magnitude);

                for (int i = 0; i < curr; i++)
                {
                    AddMoisture(Tiles[x1, MathHelper.Mod(y + i + 1, Height)], 0.025f / (center - new Vector2(x1, MathHelper.Mod(y + i + 1, Height))).magnitude);
                    AddMoisture(Tiles[x1, MathHelper.Mod(y - (i + 1), Height)], 0.025f / (center - new Vector2(x1, MathHelper.Mod(y - (i + 1), Height))).magnitude);

                    AddMoisture(Tiles[x2, MathHelper.Mod(y + i + 1, Height)], 0.025f / (center - new Vector2(x2, MathHelper.Mod(y + i + 1, Height))).magnitude);
                    AddMoisture(Tiles[x2, MathHelper.Mod(y - (i + 1), Height)], 0.025f / (center - new Vector2(x2, MathHelper.Mod(y - (i + 1), Height))).magnitude);
                }
                curr--;
            }
        }
        private void AddMoisture(Tile t, float amount)
        {
            MoistureData.Data[t.X, t.Y] += amount;
            t.MoistureValue += amount;
            if (t.MoistureValue > 1)
                t.MoistureValue = 1;

            // Set moisture type
            if (t.MoistureValue < DryerValue) t.MoistureType = MoistureType.Dryest;
            else if (t.MoistureValue < DryValue) t.MoistureType = MoistureType.Dryer;
            else if (t.MoistureValue < WetValue) t.MoistureType = MoistureType.Dry;
            else if (t.MoistureValue < WetterValue) t.MoistureType = MoistureType.Wet;
            else if (t.MoistureValue < WettestValue) t.MoistureType = MoistureType.Wetter;
            else t.MoistureType = MoistureType.Wettest;
        }
        /// <summary>
        /// Adjusts moisture map to incorporate rivers.
        /// </summary>
        private void AdjustMoistureMap()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Tile t = Tiles[x, y];
                    if (t.TerrainType == TerrainType.River)
                        AddMoisture(t, 60);
                }
            }
        }
        private void DigRiverGroups()
        {
            for (int i = 0; i < RiverGroups.Count; i++)
            {
                RiverGroup group = RiverGroups[i];
                River longest = null;

                // Find longest river in this group
                for (int j = 0; j < group.Rivers.Count; j++)
                {
                    River river = group.Rivers[j];
                    if (longest == null)
                        longest = river;
                    else if (longest.Tiles.Count < river.Tiles.Count)
                        longest = river;
                }

                if (longest != null)
                {
                    // Dig out longest path first
                    DigRiver(longest);

                    for (int j = 0; j < group.Rivers.Count; j++)
                    {
                        River river = group.Rivers[j];
                        if (river != longest)
                            DigRiver(river, longest);
                    }
                }
            }
        }
        private void BuildRiverGroups()
        {
            // Loop each tile, checking if it belongs to multiple rivers
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Tile t = Tiles[x, y];

                    if (t.Rivers.Count > 1)
                    {
                        // Multiple rivers == intersection
                        RiverGroup group = null;

                        // Does a rivergroup already exist for this group?
                        for (int n = 0; n < t.Rivers.Count; n++)
                        {
                            River tileriver = t.Rivers[n];
                            for (int i = 0; i < RiverGroups.Count; i++)
                            {
                                for (int j = 0; j < RiverGroups[i].Rivers.Count; j++)
                                {
                                    River river = RiverGroups[i].Rivers[j];
                                    if (river.ID == tileriver.ID)
                                        group = RiverGroups[i];
                                    if (group != null) 
                                        break;
                                }
                                if (group != null) 
                                    break;
                            }
                            if (group != null) 
                                break;
                        }

                        // existing group found -- add to it
                        if (group != null)
                        {
                            for (int n = 0; n < t.Rivers.Count; n++)
                            {
                                if (!group.Rivers.Contains(t.Rivers[n]))
                                    group.Rivers.Add(t.Rivers[n]);
                            }
                        }
                        else   //No existing group found - create a new one
                        {
                            group = new RiverGroup();
                            for (int n = 0; n < t.Rivers.Count; n++)
                                group.Rivers.Add(t.Rivers[n]);
                            RiverGroups.Add(group);
                        }
                    }
                }
            }
        }
        /// <remarks>
        /// This is an agent based approach. The first step of the algorithm, is to select a random tile on the map. The selected tile must be land, and must also have a height value that is over a specified threshold. From this tile, we determine which neighboring tile is the lowest, and navigate towards it. We create a path in this fashion, until a water tile is reached.
        /// </remarks>
        private void GenerateRivers()
        {
            int attempts = 0;
            int rivercount = RiverCount;
            Rivers = [];

            // Generate some rivers
            while (rivercount > 0 && attempts < MaxRiverAttempts)
            {

                // Get a random tile
                int x = Random.Range(0, Width);
                int y = Random.Range(0, Height);
                Tile tile = Tiles[x, y];

                // validate the tile
                if (!tile.Collidable) continue;
                if (tile.Rivers.Count > 0) continue;

                if (tile.HeightRatio > MinRiverHeight)
                {
                    // Tile is good to start river from
                    River river = new(rivercount);

                    // Figure out the direction this river will try to flow
                    river.CurrentDirection = tile.GetLowestNeighbor(this);

                    // Recursively find a path to water
                    FindPathToWater(tile, river.CurrentDirection, ref river);

                    // Validate the generated river 
                    if (river.TurnCount < MinRiverTurns || river.Tiles.Count < MinRiverLength || river.Intersections > MaxRiverIntersections)
                    {
                        //Validation failed - remove this river
                        for (int i = 0; i < river.Tiles.Count; i++)
                        {
                            Tile t = river.Tiles[i];
                            t.Rivers.Remove(river);
                        }
                    }
                    else if (river.Tiles.Count >= MinRiverLength)
                    {
                        // Validation passed - Add river to list
                        Rivers.Add(river);
                        tile.Rivers.Add(river);
                        rivercount--;
                    }
                }
                attempts++;
            }
        }
        /// <summary>
        /// Dig river based on a parent river vein
        /// </summary>
        private void DigRiver(River river, River parent)
        {
            int intersectionID = 0;
            int intersectionSize = 0;

            // determine point of intersection
            for (int i = 0; i < river.Tiles.Count; i++)
            {
                Tile t1 = river.Tiles[i];
                for (int j = 0; j < parent.Tiles.Count; j++)
                {
                    Tile t2 = parent.Tiles[j];
                    if (t1 == t2)
                    {
                        intersectionID = i;
                        intersectionSize = t2.RiverSize;
                    }
                }
            }

            int counter = 0;
            int intersectionCount = river.Tiles.Count - intersectionID;
            int size = Random.Range(intersectionSize, 5);
            river.Length = river.Tiles.Count;

            // randomize size change
            int two = river.Length / 2;
            int three = two / 2;
            int four = three / 2;
            int five = four / 2;

            int twomin = two / 3;
            int threemin = three / 3;
            int fourmin = four / 3;
            int fivemin = five / 3;

            // randomize length of each size
            int count1 = Random.Range(fivemin, five);
            if (size < 4)
            {
                count1 = 0;
            }
            int count2 = count1 + Random.Range(fourmin, four);
            if (size < 3)
            {
                count2 = 0;
                count1 = 0;
            }
            int count3 = count2 + Random.Range(threemin, three);
            if (size < 2)
            {
                count3 = 0;
                count2 = 0;
                count1 = 0;
            }
            int count4 = count3 + Random.Range(twomin, two);

            // Make sure we are not digging past the river path
            if (count4 > river.Length)
            {
                int extra = count4 - river.Length;
                while (extra > 0)
                {
                    if (count1 > 0) { count1--; count2--; count3--; count4--; extra--; }
                    else if (count2 > 0) { count2--; count3--; count4--; extra--; }
                    else if (count3 > 0) { count3--; count4--; extra--; }
                    else if (count4 > 0) { count4--; extra--; }
                }
            }

            // adjust size of river at intersection point
            if (intersectionSize == 1)
            {
                count4 = intersectionCount;
                count1 = 0;
                count2 = 0;
                count3 = 0;
            }
            else if (intersectionSize == 2)
            {
                count3 = intersectionCount;
                count1 = 0;
                count2 = 0;
            }
            else if (intersectionSize == 3)
            {
                count2 = intersectionCount;
                count1 = 0;
            }
            else if (intersectionSize == 4)
            {
                count1 = intersectionCount;
            }
            else
            {
                count1 = 0;
                count2 = 0;
                count3 = 0;
                count4 = 0;
            }

            // dig out the river
            for (int i = river.Tiles.Count - 1; i >= 0; i--)
            {
                Tile t = river.Tiles[i];

                if (counter < count1)
                    t.DigRiver(river, 4);
                else if (counter < count2)
                    t.DigRiver(river, 3);
                else if (counter < count3)
                    t.DigRiver(river, 2);
                else if (counter < count4)
                    t.DigRiver(river, 1);
                else
                    t.DigRiver(river, 0);
                counter++;
            }
        }

        // Dig river
        private void DigRiver(River river)
        {
            int counter = 0;

            // How wide are we digging this river?
            int size = Random.Range(1, 5);
            river.Length = river.Tiles.Count;

            // randomize size change
            int two = river.Length / 2;
            int three = two / 2;
            int four = three / 2;
            int five = four / 2;

            int twomin = two / 3;
            int threemin = three / 3;
            int fourmin = four / 3;
            int fivemin = five / 3;

            // randomize lenght of each size
            int count1 = Random.Range(fivemin, five);
            if (size < 4)
            {
                count1 = 0;
            }
            int count2 = count1 + Random.Range(fourmin, four);
            if (size < 3)
            {
                count2 = 0;
                count1 = 0;
            }
            int count3 = count2 + Random.Range(threemin, three);
            if (size < 2)
            {
                count3 = 0;
                count2 = 0;
                count1 = 0;
            }
            int count4 = count3 + Random.Range(twomin, two);

            // Make sure we are not digging past the river path
            if (count4 > river.Length)
            {
                int extra = count4 - river.Length;
                while (extra > 0)
                {
                    if (count1 > 0) { count1--; count2--; count3--; count4--; extra--; }
                    else if (count2 > 0) { count2--; count3--; count4--; extra--; }
                    else if (count3 > 0) { count3--; count4--; extra--; }
                    else if (count4 > 0) { count4--; extra--; }
                }
            }

            // Dig it out
            for (int i = river.Tiles.Count - 1; i >= 0; i--)
            {
                Tile t = river.Tiles[i];

                if (counter < count1)
                    t.DigRiver(river, 4);
                else if (counter < count2)
                    t.DigRiver(river, 3);
                else if (counter < count3)
                    t.DigRiver(river, 2);
                else if (counter < count4)
                    t.DigRiver(river, 1);
                else
                    t.DigRiver(river, 0);
                counter++;
            }
        }
        #endregion

        #region Helpers
        private void FindPathToWater(Tile tile, Direction direction, ref River river)
        {
            if (tile.Rivers.Contains(river))
                return;

            // Check if there is already a river on this tile
            if (tile.Rivers.Count > 0)
                river.Intersections++;

            river.AddTile(tile);

            // Get neighbors
            Tile left = GetLeft(tile);
            Tile right = GetRight(tile);
            Tile top = GetTop(tile);
            Tile bottom = GetBottom(tile);

            float leftValue = int.MaxValue;
            float rightValue = int.MaxValue;
            float topValue = int.MaxValue;
            float bottomValue = int.MaxValue;

            // Query height values of neighbors
            if (left != null && left.GetRiverNeighborCount(river) < 2 && !river.Tiles.Contains(left))
                leftValue = left.HeightRatio;
            if (right != null && right.GetRiverNeighborCount(river) < 2 && !river.Tiles.Contains(right))
                rightValue = right.HeightRatio;
            if (top != null && top.GetRiverNeighborCount(river) < 2 && !river.Tiles.Contains(top))
                topValue = top.HeightRatio;
            if (bottom != null && bottom.GetRiverNeighborCount(river) < 2 && !river.Tiles.Contains(bottom))
                bottomValue = bottom.HeightRatio;

            // If neighbor is existing river that is not this one, flow into it
            if (bottom != null && bottom.Rivers.Count == 0 && !bottom.Collidable)
                bottomValue = 0;
            if (top != null && top.Rivers.Count == 0 && !top.Collidable)
                topValue = 0;
            if (left != null && left.Rivers.Count == 0 && !left.Collidable)
                leftValue = 0;
            if (right != null && right.Rivers.Count == 0 && !right.Collidable)
                rightValue = 0;

            // Override flow direction if a tile is significantly lower
            if (direction == Direction.Left)
                if (Mathf.Abs(rightValue - leftValue) < 0.1f)
                    rightValue = int.MaxValue;
            if (direction == Direction.Right)
                if (Mathf.Abs(rightValue - leftValue) < 0.1f)
                    leftValue = int.MaxValue;
            if (direction == Direction.Top)
                if (Mathf.Abs(topValue - bottomValue) < 0.1f)
                    bottomValue = int.MaxValue;
            if (direction == Direction.Bottom)
                if (Mathf.Abs(topValue - bottomValue) < 0.1f)
                    topValue = int.MaxValue;

            // Find mininum
            float min = Mathf.Min(Mathf.Min(Mathf.Min(leftValue, rightValue), topValue), bottomValue);

            // If no minimum found - exit
            if (min == int.MaxValue)
                return;

            // Move to next neighbor
            if (min == leftValue)
            {
                if (left != null && left.Collidable)
                {
                    if (river.CurrentDirection != Direction.Left)
                    {
                        river.TurnCount++;
                        river.CurrentDirection = Direction.Left;
                    }
                    FindPathToWater(left, direction, ref river);
                }
            }
            else if (min == rightValue)
            {
                if (right != null && right.Collidable)
                {
                    if (river.CurrentDirection != Direction.Right)
                    {
                        river.TurnCount++;
                        river.CurrentDirection = Direction.Right;
                    }
                    FindPathToWater(right, direction, ref river);
                }
            }
            else if (min == bottomValue)
            {
                if (bottom != null && bottom.Collidable)
                {
                    if (river.CurrentDirection != Direction.Bottom)
                    {
                        river.TurnCount++;
                        river.CurrentDirection = Direction.Bottom;
                    }
                    FindPathToWater(bottom, direction, ref river);
                }
            }
            else if (min == topValue)
            {
                if (top != null && top.Collidable)
                {
                    if (river.CurrentDirection != Direction.Top)
                    {
                        river.TurnCount++;
                        river.CurrentDirection = Direction.Top;
                    }
                    FindPathToWater(top, direction, ref river);
                }
            }
        }
        /// <summary>
        /// Build a Tile array from our data
        /// </summary>
        private void BuildTiles()
        {
            Tiles = new Tile[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Tile t = new()
                    {
                        X = x,
                        Y = y
                    };

                    // Set heightmap value (normalized)
                    float physicalHeight = HeightData.Data[x, y];
                    float normalizedHeight = (physicalHeight - HeightData.Min) / (HeightData.Max - HeightData.Min);
                    t.HeightRatio = normalizedHeight;
                    t.PhysicalHeight = physicalHeight;

                    // Decide terrain type
                    if (normalizedHeight < DeepWater)
                    {
                        t.TerrainType = TerrainType.DeepWater;
                        t.Collidable = false;
                    }
                    else if (normalizedHeight < ShallowWater)
                    {
                        t.TerrainType = TerrainType.ShallowWater;
                        t.Collidable = false;
                    }
                    else if (normalizedHeight < Sand)
                    {
                        t.TerrainType = TerrainType.Sand;
                        t.Collidable = true;
                    }
                    else if (normalizedHeight < Grass)
                    {
                        t.TerrainType = TerrainType.Grass;
                        t.Collidable = true;
                    }
                    else if (normalizedHeight < Forest)
                    {
                        t.TerrainType = TerrainType.Forest;
                        t.Collidable = true;
                    }
                    else if (normalizedHeight < Rock)
                    {
                        t.TerrainType = TerrainType.Rock;
                        t.Collidable = true;
                    }
                    else
                    {
                        t.TerrainType = TerrainType.Snow;
                        t.Collidable = true;
                    }

                    // Adjust moisture based on height
                    if (t.TerrainType == TerrainType.DeepWater)
                        MoistureData.Data[t.X, t.Y] += 8f * t.HeightRatio;
                    else if (t.TerrainType == TerrainType.ShallowWater)
                        MoistureData.Data[t.X, t.Y] += 3f * t.HeightRatio;
                    else if (t.TerrainType == TerrainType.Shore)
                        MoistureData.Data[t.X, t.Y] += 1f * t.HeightRatio;
                    else if (t.TerrainType == TerrainType.Sand)
                        MoistureData.Data[t.X, t.Y] += 0.2f * t.HeightRatio;

                    // Moisture Map Analyze	
                    float moistureValue = MoistureData.Data[x, y];
                    moistureValue = (moistureValue - MoistureData.Min) / (MoistureData.Max - MoistureData.Min);
                    t.MoistureValue = moistureValue;

                    // Set moisture type
                    if (moistureValue < DryerValue) t.MoistureType = MoistureType.Dryest;
                    else if (moistureValue < DryValue) t.MoistureType = MoistureType.Dryer;
                    else if (moistureValue < WetValue) t.MoistureType = MoistureType.Dry;
                    else if (moistureValue < WetterValue) t.MoistureType = MoistureType.Wet;
                    else if (moistureValue < WettestValue) t.MoistureType = MoistureType.Wetter;
                    else t.MoistureType = MoistureType.Wettest;

                    // Adjust Heat Map based on Height - Higher == colder
                    if (t.TerrainType == TerrainType.Forest)
                        HeatData.Data[t.X, t.Y] -= 0.1f * t.HeightRatio;
                    else if (t.TerrainType == TerrainType.Rock)
                        HeatData.Data[t.X, t.Y] -= 0.25f * t.HeightRatio;
                    else if (t.TerrainType == TerrainType.Snow)
                        HeatData.Data[t.X, t.Y] -= 0.4f * t.HeightRatio;
                    else
                        HeatData.Data[t.X, t.Y] += 0.01f * t.HeightRatio;

                    // Set heat value
                    float heatValue = HeatData.Data[x, y];
                    heatValue = (heatValue - HeatData.Min) / (HeatData.Max - HeatData.Min);
                    t.HeatValue = heatValue;

                    // Set heat type
                    if (heatValue < ColdestValue) t.HeatType = HeatType.Coldest;
                    else if (heatValue < ColderValue) t.HeatType = HeatType.Colder;
                    else if (heatValue < ColdValue) t.HeatType = HeatType.Cold;
                    else if (heatValue < WarmValue) t.HeatType = HeatType.Warm;
                    else if (heatValue < WarmerValue) t.HeatType = HeatType.Warmer;
                    else t.HeatType = HeatType.Warmest;

                    if (Clouds1 != null)
                    {
                        t.Cloud1Value = Clouds1.Data[x, y];
                        t.Cloud1Value = (t.Cloud1Value - Clouds1.Min) / (Clouds1.Max - Clouds1.Min);
                    }

                    if (Clouds2 != null)
                    {
                        t.Cloud2Value = Clouds2.Data[x, y];
                        t.Cloud2Value = (t.Cloud2Value - Clouds2.Min) / (Clouds2.Max - Clouds2.Min);
                    }

                    Tiles[x, y] = t;
                }
            }
        }
        private void UpdateNeighbors()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Tile t = Tiles[x, y];

                    t.Top = GetTop(t);
                    t.Bottom = GetBottom(t);
                    t.Left = GetLeft(t);
                    t.Right = GetRight(t);
                }
            }
        }
        private void UpdateBitmasks()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    Tiles[x, y].UpdateBitmask();
        }
        /// <summary>
        /// Find lands vs water bodies.
        /// </summary>
        private void FloodFill()
        {
            // Use a stack instead of recursion for performance with large maps
            Stack<Tile> stack = new();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Tile t = Tiles[x, y];

                    // Tile already flood filled, skip
                    if (t.FloodFilled)
                        continue;

                    // Land
                    if (t.Collidable)
                    {
                        TileGroup group = new();
                        group.Type = TileGroupType.Land;
                        stack.Push(t);

                        while (stack.Count > 0)
                            FloodFill(stack.Pop(), ref group, ref stack);

                        if (group.Tiles.Count > 0)
                            Lands.Add(group);
                    }
                    // Water
                    else
                    {
                        TileGroup group = new();
                        group.Type = TileGroupType.Water;
                        stack.Push(t);

                        while (stack.Count > 0)
                            FloodFill(stack.Pop(), ref group, ref stack);

                        if (group.Tiles.Count > 0)
                            Waters.Add(group);
                    }
                }
            }
        }
        private void FloodFill(Tile tile, ref TileGroup tiles, ref Stack<Tile> stack)
        {
            // Validate
            if (tile == null)
                return;
            if (tile.FloodFilled)
                return;
            if (tiles.Type == TileGroupType.Land && !tile.Collidable)
                return;
            if (tiles.Type == TileGroupType.Water && tile.Collidable)
                return;

            // Add to TileGroup
            tiles.Tiles.Add(tile);
            tile.FloodFilled = true;

            // floodfill into neighbors
            Tile t = GetTop(tile);
            if (t != null && !t.FloodFilled && tile.Collidable == t.Collidable)
                stack.Push(t);
            t = GetBottom(tile);
            if (t != null && !t.FloodFilled && tile.Collidable == t.Collidable)
                stack.Push(t);
            t = GetLeft(tile);
            if (t != null && !t.FloodFilled && tile.Collidable == t.Collidable)
                stack.Push(t);
            t = GetRight(tile);
            if (t != null && !t.FloodFilled && tile.Collidable == t.Collidable)
                stack.Push(t);
        }
        #endregion
    }
}