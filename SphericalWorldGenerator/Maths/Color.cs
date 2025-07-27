using System;

namespace SphericalWorldGenerator.Maths
{
    /// <summary>
    /// RGBA color with float precision (0–1).
    /// </summary>
    public struct Color : IEquatable<Color>
    {
        public float r;
        public float g;
        public float b;
        public float a;

        /// <summary>
        /// Create a color. Alpha defaults to 1.
        /// </summary>
        public Color(float r, float g, float b, float a = 1f)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
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
                    0 => r,
                    1 => g,
                    2 => b,
                    3 => a,
                    _ => throw new IndexOutOfRangeException("Invalid Color index!")
                };
            }
            set
            {
                switch (index)
                {
                    case 0: r = value; break;
                    case 1: g = value; break;
                    case 2: b = value; break;
                    case 3: a = value; break;
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
                a.r + (b.r - a.r) * t,
                a.g + (b.g - a.g) * t,
                a.b + (b.b - a.b) * t,
                a.a + (b.a - a.a) * t
            );
        }

        /// <summary>
        /// Linearly interpolate without clamping t.
        /// </summary>
        public static Color LerpUnclamped(Color a, Color b, float t)
        {
            return new Color(
                a.r + (b.r - a.r) * t,
                a.g + (b.g - a.g) * t,
                a.b + (b.b - a.b) * t,
                a.a + (b.a - a.a) * t
            );
        }

        /// <summary>
        /// Multiply RGB channels by a scalar; alpha unchanged.
        /// </summary>
        public Color RGBMultiplied(float multiplier)
            => new(r * multiplier, g * multiplier, b * multiplier, a);

        /// <summary>
        /// Brightness approximation: 0.299*r + 0.587*g + 0.114*b.
        /// </summary>
        public float grayscale
            => r * 0.299f + g * 0.587f + b * 0.114f;

        /// <summary>
        /// The largest of r, g, b, a.
        /// </summary>
        public float maxColorComponent
            => Math.Max(Math.Max(r, g), Math.Max(b, a));

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
            => new(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);

        public static Color operator -(Color a, Color b)
            => new(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);

        public static Color operator *(Color a, float b)
            => new(a.r * b, a.g * b, a.b * b, a.a * b);

        public static Color operator *(float b, Color a)
            => a * b;

        public static Color operator *(Color a, Color b)
            => new(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);

        public static Color operator /(Color a, float b)
        {
            if (b == 0f) throw new DivideByZeroException("Divide Color by zero.");
            return new Color(a.r / b, a.g / b, a.b / b, a.a / b);
        }

        public static bool operator ==(Color a, Color b)
            => a.Equals(b);

        public static bool operator !=(Color a, Color b)
            => !a.Equals(b);

        public override bool Equals(object obj)
            => obj is Color other && Equals(other);

        public bool Equals(Color other)
            => Math.Abs(r - other.r) < 1e-5f
            && Math.Abs(g - other.g) < 1e-5f
            && Math.Abs(b - other.b) < 1e-5f
            && Math.Abs(a - other.a) < 1e-5f;

        public override int GetHashCode()
            => HashCode.Combine(r, g, b, a);

        public override string ToString()
            => $"RGBA({r:F3}, {g:F3}, {b:F3}, {a:F3})";
    }
}
