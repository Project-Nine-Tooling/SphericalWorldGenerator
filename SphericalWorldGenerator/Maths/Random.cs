using System;
using System.Numerics;

namespace SphericalWorldGenerator.Maths
{
    /// <summary>
    /// Drop‑in replacement for Random.
    /// </summary>
    public static class Random
    {
        // ──── Internal state ────────────────────────────────────────────────────
        private static int _seed = Environment.TickCount;
        private static System.Random _rng = new(_seed);

        /// <summary>
        /// (Write‑only) seed for reproducible sequences. Calls InitState().
        /// </summary>
        public static int seed
        {
            set => InitState(value);
        }

        /// <summary>
        /// Initialize the generator with the given seed.
        /// </summary>
        public static void InitState(int seed)
        {
            _seed = seed;
            _rng = new System.Random(seed);
        }

        // ──── Basic values ──────────────────────────────────────────────────────

        /// <summary>
        /// Random float in [0.0, 1.0).
        /// </summary>
        public static float value => (float)_rng.NextDouble();

        /// <summary>
        /// Random integer in [minInclusive, maxExclusive).
        /// </summary>
        public static int Range(int minInclusive, int maxExclusive)
            => _rng.Next(minInclusive, maxExclusive);

        /// <summary>
        /// Random float in [minInclusive, maxInclusive].
        /// </summary>
        public static float Range(float minInclusive, float maxInclusive)
            => (float)(_rng.NextDouble() * (maxInclusive - minInclusive) + minInclusive);

        // ──── 2D/3D sampling ────────────────────────────────────────────────────

        /// <summary>
        /// Random point inside the unit circle (uniform).
        /// </summary>
        public static Vector2 insideUnitCircle
        {
            get
            {
                while (true)
                {
                    float x = Range(-1f, 1f);
                    float y = Range(-1f, 1f);
                    if (x * x + y * y <= 1f)
                        return new Vector2(x, y);
                }
            }
        }

        /// <summary>
        /// Random point inside the unit sphere (uniform).
        /// </summary>
        public static Vector3 insideUnitSphere
        {
            get
            {
                while (true)
                {
                    float x = Range(-1f, 1f);
                    float y = Range(-1f, 1f);
                    float z = Range(-1f, 1f);
                    if (x * x + y * y + z * z <= 1f)
                        return new Vector3(x, y, z);
                }
            }
        }

        /// <summary>
        /// Random unit vector (on the surface of the unit sphere).
        /// </summary>
        public static Vector3 onUnitSphere
        {
            get
            {
                // Uniform sampling via spherical coordinates
                double u = _rng.NextDouble();             // in [0,1)
                double v = _rng.NextDouble();             // in [0,1)
                double theta = 2.0 * Math.PI * v;
                double z = 2.0 * u - 1.0;                  // in [-1,1]
                double r = Math.Sqrt(1 - z * z);
                return new Vector3(
                    (float)(r * Math.Cos(theta)),
                    (float)(r * Math.Sin(theta)),
                    (float)z
                );
            }
        }

        // ──── Color sampling ───────────────────────────────────────────────────

        /// <summary>
        /// Random color in HSV space (hue,sat,val ∈ [0,1], alpha = 1).
        /// </summary>
        public static Color ColorHSV()
            => ColorHSV(0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f);

        /// <summary>
        /// Random color with each HSV component in its own range, plus alpha.
        /// </summary>
        public static Color ColorHSV(
            float hueMin, float hueMax,
            float satMin, float satMax,
            float valMin, float valMax,
            float alphaMin, float alphaMax
        )
        {
            float h = Range(hueMin, hueMax);
            float s = Range(satMin, satMax);
            float v = Range(valMin, valMax);
            float a = Range(alphaMin, alphaMax);
            return HSVToRGB(h, s, v, a);
        }

        // ──── Internal helpers ─────────────────────────────────────────────────

        // Converts HSV [0,1] + alpha to an RGBA Color.
        private static Color HSVToRGB(float H, float S, float V, float A)
        {
            // Unity’s algorithm: divide H into six sectors
            float r, g, b;
            if (S == 0f)
            {
                r = g = b = V; // achromatic
            }
            else
            {
                float h = H * 6f;
                int i = (int)Math.Floor(h);
                float f = h - i;
                float p = V * (1f - S);
                float q = V * (1f - S * f);
                float t = V * (1f - S * (1f - f));
                switch (i % 6)
                {
                    case 0: r = V; g = t; b = p; break;
                    case 1: r = q; g = V; b = p; break;
                    case 2: r = p; g = V; b = t; break;
                    case 3: r = p; g = q; b = V; break;
                    case 4: r = t; g = p; b = V; break;
                    default: r = V; g = p; b = q; break;
                }
            }
            return new Color(r, g, b, A);
        }
    }
}
