namespace SphericalWorldGenerator.Framework
{
    public class MapData
    {
        #region Construction
        public MapData(int width, int height)
        {
            Data = new float[width, height];
            Min = float.MaxValue;
            Max = float.MinValue;
        }
        #endregion

        #region Properties
        public float[,] Data;
        public float Min { get; set; }
        public float Max { get; set; }
        #endregion
    }
}