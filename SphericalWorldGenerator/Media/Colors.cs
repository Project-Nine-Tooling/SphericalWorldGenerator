using SphericalWorldGenerator.Maths;

namespace SphericalWorldGenerator.Media
{
    /// <summary>
    /// Common RGBA color presets (0–1 range).
    /// </summary>
    public static class Colors
    {
        // Basic
        public static readonly Color Black = new(0f, 0f, 0f, 1f);
        public static readonly Color White = new(1f, 1f, 1f, 1f);
        public static readonly Color Red = new(1f, 0f, 0f, 1f);
        public static readonly Color Green = new(0f, 1f, 0f, 1f);
        public static readonly Color Blue = new(0f, 0f, 1f, 1f);
        public static readonly Color Yellow = new(1f, 0.9215686f, 0.01568628f, 1f);
        public static readonly Color Cyan = new(0f, 1f, 1f, 1f);
        public static readonly Color Magenta = new(1f, 0f, 1f, 1f);
        public static readonly Color Gray = new(0.5f, 0.5f, 0.5f, 1f);
        public static readonly Color Grey = Gray;
        public static readonly Color Clear = new(0f, 0f, 0f, 0f);

        // Terrain‑mapping examples (just copy the ones you need)
        public static readonly Color DeepColor = new(15 / 255f, 30 / 255f, 80 / 255f, 1f);
        public static readonly Color ShallowColor = new(15 / 255f, 40 / 255f, 90 / 255f, 1f);
        public static readonly Color RiverColor = new(30 / 255f, 120 / 255f, 200 / 255f, 1f);
        public static readonly Color SandColor = new(198 / 255f, 190 / 255f, 31 / 255f, 1f);
        public static readonly Color GrassColor = new(50 / 255f, 220 / 255f, 20 / 255f, 1f);
        public static readonly Color ForestColor = new(16 / 255f, 160 / 255f, 0f, 1f);
        public static readonly Color RockColor = new(0.5f, 0.5f, 0.5f, 1f);
        public static readonly Color SnowColor = new(1f, 1f, 1f, 1f);

        // You can keep adding any named palettes you like:
        // public static readonly Color IceWater  = new Color(210/255f, 255/255f, 252/255f, 1f);
        // public static readonly Color ColdWater = new Color(119/255f, 156/255f, 213/255f, 1f);
        // …etc.
    }
}
