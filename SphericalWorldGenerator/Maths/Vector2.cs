using System;

namespace SphericalWorldGenerator.Maths
{
    /// <summary>
    /// A 2D vector type analogous to UnityEngine.Vector2.
    /// </summary>
    [Serializable]
    public struct Vector2 : IEquatable<Vector2>
    {
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        // Indexer (0 ⇒ x, 1 ⇒ y)
        public float this[int index]
        {
            get
            {
                return index == 0 ? x
                     : index == 1 ? y
                     : throw new IndexOutOfRangeException("Invalid Vector2 index");
            }
            set
            {
                if (index == 0) x = value;
                else if (index == 1) y = value;
                else throw new IndexOutOfRangeException("Invalid Vector2 index");
            }
        }

        // Common constants
        public static Vector2 zero => new(0f, 0f);
        public static Vector2 one => new(1f, 1f);
        public static Vector2 up => new(0f, 1f);
        public static Vector2 down => new(0f, -1f);
        public static Vector2 left => new(-1f, 0f);
        public static Vector2 right => new(1f, 0f);

        // Magnitude and normalization
        public float magnitude => MathF.Sqrt(x * x + y * y);
        public float sqrMagnitude => x * x + y * y;
        public Vector2 normalized
        {
            get
            {
                float mag = magnitude;
                return mag > 1e-5f
                    ? this / mag
                    : zero;
            }
        }

        /// <summary>
        /// Normalizes this vector in place.
        /// </summary>
        public void Normalize()
        {
            float mag = magnitude;
            if (mag > 1e-5f)
            {
                x /= mag;
                y /= mag;
            }
            else
            {
                x = y = 0f;
            }
        }

        // Linear interpolation
        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            t = Math.Clamp(t, 0f, 1f);
            return new Vector2(
                a.x + (b.x - a.x) * t,
                a.y + (b.y - a.y) * t
            );
        }

        // Dot product
        public static float Dot(Vector2 a, Vector2 b) => a.x * b.x + a.y * b.y;

        // Distance between two points
        public static float Distance(Vector2 a, Vector2 b) => (a - b).magnitude;

        // Scale (component‐wise multiplication)
        public static Vector2 Scale(Vector2 a, Vector2 b) => new(a.x * b.x, a.y * b.y);

        // Reflect vector off a surface with normal n
        public static Vector2 Reflect(Vector2 direction, Vector2 normal)
        {
            // r = d - 2*(d·n)*n
            float dot = Dot(direction, normal);
            return direction - 2f * dot * normal;
        }

        // Operators
        public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.x + b.x, a.y + b.y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.x - b.x, a.y - b.y);
        public static Vector2 operator -(Vector2 v) => new(-v.x, -v.y);
        public static Vector2 operator *(Vector2 v, float d) => new(v.x * d, v.y * d);
        public static Vector2 operator *(float d, Vector2 v) => new(v.x * d, v.y * d);
        public static Vector2 operator /(Vector2 v, float d) => new(v.x / d, v.y / d);

        public static bool operator ==(Vector2 a, Vector2 b) => a.Equals(b);
        public static bool operator !=(Vector2 a, Vector2 b) => !a.Equals(b);

        // IEquatable implementation
        public bool Equals(Vector2 other)
            => x == other.x && y == other.y;

        public override bool Equals(object? obj)
            => obj is Vector2 v && Equals(v);

        public override int GetHashCode()
            => HashCode.Combine(x, y);

        public override string ToString()
            => $"({x:F3}, {y:F3})";
    }
}
