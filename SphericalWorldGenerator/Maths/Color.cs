using System;

namespace SphericalWorldGenerator.Maths
{
    /// <summary>
    /// RGBA color with float precision (0–1).
    /// </summary>
    public struct Color : IEquatable<Color>
    {
        public float R;
        public float G;
        public float B;
        public float A;

        /// <summary>
        /// Create a color. Alpha defaults to 1.
        /// </summary>
        public Color(float r, float g, float b, float a = 1f)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        /// <summary>
        /// Access component by index: 0→r, 1→g, 2→b, 3→a.
        /// </summary>
        public float this[int index]
        {
            get
            {
                return index switch
                {
                    0 => R,
                    1 => G,
                    2 => B,
                    3 => A,
                    _ => throw new IndexOutOfRangeException("Invalid Color index!")
                };
            }
            set
            {
                switch (index)
                {
                    case 0: R = value; break;
                    case 1: G = value; break;
                    case 2: B = value; break;
                    case 3: A = value; break;
                    default: throw new IndexOutOfRangeException("Invalid Color index!");
                }
            }
        }

        /// <summary>
        /// Linearly interpolate between colors, clamped 0–1.
        /// </summary>
        public static Color Lerp(Color a, Color b, float t)
        {
            t = Math.Clamp(t, 0f, 1f);
            return new Color(
                a.R + (b.R - a.R) * t,
                a.G + (b.G - a.G) * t,
                a.B + (b.B - a.B) * t,
                a.A + (b.A - a.A) * t
            );
        }

        /// <summary>
        /// Linearly interpolate without clamping t.
        /// </summary>
        public static Color LerpUnclamped(Color a, Color b, float t)
        {
            return new Color(
                a.R + (b.R - a.R) * t,
                a.G + (b.G - a.G) * t,
                a.B + (b.B - a.B) * t,
                a.A + (b.A - a.A) * t
            );
        }

        /// <summary>
        /// Multiply RGB channels by a scalar; alpha unchanged.
        /// </summary>
        public Color RGBMultiplied(float multiplier)
            => new(R * multiplier, G * multiplier, B * multiplier, A);

        /// <summary>
        /// Brightness approximation: 0.299*r + 0.587*g + 0.114*b.
        /// </summary>
        public float grayscale
            => R * 0.299f + G * 0.587f + B * 0.114f;

        /// <summary>
        /// The largest of r, g, b, a.
        /// </summary>
        public float maxColorComponent
            => Math.Max(Math.Max(R, G), Math.Max(B, A));

        // Common presets
        public static Color red => new(1f, 0f, 0f, 1f);
        public static Color green => new(0f, 1f, 0f, 1f);
        public static Color blue => new(0f, 0f, 1f, 1f);
        public static Color white => new(1f, 1f, 1f, 1f);
        public static Color black => new(0f, 0f, 0f, 1f);
        public static Color yellow => new(1f, 0.9215686f, 0.01568628f, 1f);
        public static Color cyan => new(0f, 1f, 1f, 1f);
        public static Color magenta => new(1f, 0f, 1f, 1f);
        public static Color gray => new(0.5f, 0.5f, 0.5f, 1f);
        public static Color grey => gray;
        public static Color clear => new(0f, 0f, 0f, 0f);

        // Operator overloads
        public static Color operator +(Color a, Color b)
            => new(a.R + b.R, a.G + b.G, a.B + b.B, a.A + b.A);

        public static Color operator -(Color a, Color b)
            => new(a.R - b.R, a.G - b.G, a.B - b.B, a.A - b.A);

        public static Color operator *(Color a, float b)
            => new(a.R * b, a.G * b, a.B * b, a.A * b);

        public static Color operator *(float b, Color a)
            => a * b;

        public static Color operator *(Color a, Color b)
            => new(a.R * b.R, a.G * b.G, a.B * b.B, a.A * b.A);

        public static Color operator /(Color a, float b)
        {
            if (b == 0f) throw new DivideByZeroException("Divide Color by zero.");
            return new Color(a.R / b, a.G / b, a.B / b, a.A / b);
        }

        public static bool operator ==(Color a, Color b)
            => a.Equals(b);

        public static bool operator !=(Color a, Color b)
            => !a.Equals(b);

        public override bool Equals(object obj)
            => obj is Color other && Equals(other);

        public bool Equals(Color other)
            => Math.Abs(R - other.R) < 1e-5f
            && Math.Abs(G - other.G) < 1e-5f
            && Math.Abs(B - other.B) < 1e-5f
            && Math.Abs(A - other.A) < 1e-5f;

        public override int GetHashCode()
            => HashCode.Combine(R, G, B, A);

        public override string ToString()
            => $"RGBA({R:F3}, {G:F3}, {B:F3}, {A:F3})";
    }
}
