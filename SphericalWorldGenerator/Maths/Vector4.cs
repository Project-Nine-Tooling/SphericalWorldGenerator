using System;
using System.Runtime.CompilerServices;

namespace SphericalWorldGenerator.Maths
{
    /// <summary>
    /// A 4D vector with float precision.
    /// </summary>
    public struct Vector4 : IEquatable<Vector4>
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>Access component by index: 0→x, 1→y, 2→z, 3→w.</summary>
        public float this[int index]
        {
            get
            {
                return index switch
                {
                    0 => x,
                    1 => y,
                    2 => z,
                    3 => w,
                    _ => throw new IndexOutOfRangeException("Invalid Vector4 index!")
                };
            }
            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    case 3: w = value; break;
                    default: throw new IndexOutOfRangeException("Invalid Vector4 index!");
                }
            }
        }

        /// <summary>Length of the vector.</summary>
        public float magnitude => (float)System.Math.Sqrt(x * x + y * y + z * z + w * w);

        /// <summary>Squared length of the vector (avoids a sqrt).</summary>
        public float sqrMagnitude => x * x + y * y + z * z + w * w;

        /// <summary>Normalized copy of this vector.</summary>
        public Vector4 normalized
        {
            get
            {
                float mag = magnitude;
                return mag > 1e-5f
                    ? this / mag
                    : zero;
            }
        }

        public static Vector4 zero => new(0f, 0f, 0f, 0f);
        public static Vector4 one => new(1f, 1f, 1f, 1f);

        /// <summary>Component‑wise multiply.</summary>
        public static Vector4 Scale(Vector4 a, Vector4 b)
            => new(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);

        /// <summary>Dot product of two Vector4s.</summary>
        public static float Dot(Vector4 a, Vector4 b)
            => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

        /// <summary>Linearly interpolates between a and b by t (clamped 0–1).</summary>
        public static Vector4 Lerp(Vector4 a, Vector4 b, float t)
        {
            t = System.Math.Clamp(t, 0f, 1f);
            return new Vector4(
                a.x + (b.x - a.x) * t,
                a.y + (b.y - a.y) * t,
                a.z + (b.z - a.z) * t,
                a.w + (b.w - a.w) * t
            );
        }

        /// <summary>
        /// Converts a Vector3 to a Vector4.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector4(Vector3 v)
        {
            // Take the x, y, z from the Vector3 and set w to 0
            return new Vector4(v.x, v.y, v.z, 0.0F);
        }

        // Arithmetic operators
        public static Vector4 operator +(Vector4 a, Vector4 b)
            => new(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);

        public static Vector4 operator -(Vector4 a, Vector4 b)
            => new(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);

        public static Vector4 operator -(Vector4 v)
            => new(-v.x, -v.y, -v.z, -v.w);

        public static Vector4 operator *(Vector4 v, float scalar)
            => new(v.x * scalar, v.y * scalar, v.z * scalar, v.w * scalar);

        public static Vector4 operator *(float scalar, Vector4 v)
            => v * scalar;

        public static Vector4 operator /(Vector4 v, float scalar)
        {
            if (scalar == 0f) throw new DivideByZeroException("Divide Vector4 by zero.");
            return new Vector4(v.x / scalar, v.y / scalar, v.z / scalar, v.w / scalar);
        }

        // Equality
        public static bool operator ==(Vector4 a, Vector4 b)
            => a.Equals(b);

        public static bool operator !=(Vector4 a, Vector4 b)
            => !a.Equals(b);

        public override bool Equals(object obj)
            => obj is Vector4 other && Equals(other);

        public bool Equals(Vector4 other)
            => System.Math.Abs(x - other.x) < 1e-5f
            && System.Math.Abs(y - other.y) < 1e-5f
            && System.Math.Abs(z - other.z) < 1e-5f
            && System.Math.Abs(w - other.w) < 1e-5f;

        public override int GetHashCode()
            => HashCode.Combine(x, y, z, w);

        public override string ToString()
            => $"({x:F3}, {y:F3}, {z:F3}, {w:F3})";
    }
}
