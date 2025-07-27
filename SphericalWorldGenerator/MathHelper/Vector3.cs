using System;

namespace SphericalWorldGenerator.MathHelper
{
    /// <summary>
    /// A 3D vector with float precision.
    /// </summary>
    public struct Vector3 : IEquatable<Vector3>
    {
        public float x;
        public float y;
        public float z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>Length of the vector.</summary>
        public float magnitude => (float)global::System.Math.Sqrt(x * x + y * y + z * z);

        /// <summary>Squared length of the vector (avoids a sqrt).</summary>
        public float sqrMagnitude => x * x + y * y + z * z;

        /// <summary>Normalized copy of this vector.</summary>
        public Vector3 normalized
        {
            get
            {
                float mag = magnitude;
                return mag > 1e-5f
                    ? this / mag
                    : zero;
            }
        }

        public static Vector3 zero => new Vector3(0f, 0f, 0f);
        public static Vector3 one => new Vector3(1f, 1f, 1f);
        public static Vector3 up => new Vector3(0f, 1f, 0f);
        public static Vector3 down => new Vector3(0f, -1f, 0f);
        public static Vector3 left => new Vector3(-1f, 0f, 0f);
        public static Vector3 right => new Vector3(1f, 0f, 0f);
        public static Vector3 forward => new Vector3(0f, 0f, 1f);
        public static Vector3 back => new Vector3(0f, 0f, -1f);

        /// <summary>Dot product of two vectors.</summary>
        public static float Dot(Vector3 a, Vector3 b)
            => a.x * b.x + a.y * b.y + a.z * b.z;

        /// <summary>Cross product of two vectors.</summary>
        public static Vector3 Cross(Vector3 a, Vector3 b)
            => new Vector3(
                   a.y * b.z - a.z * b.y,
                   a.z * b.x - a.x * b.z,
                   a.x * b.y - a.y * b.x
               );

        /// <summary>Linearly interpolates between a and b by t (clamped 0–1).</summary>
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            t = global::System.Math.Clamp(t, 0f, 1f);
            return new Vector3(
                a.x + (b.x - a.x) * t,
                a.y + (b.y - a.y) * t,
                a.z + (b.z - a.z) * t
            );
        }

        // Operator overloads
        public static Vector3 operator +(Vector3 a, Vector3 b)
            => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);

        public static Vector3 operator -(Vector3 a, Vector3 b)
            => new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);

        public static Vector3 operator -(Vector3 v)
            => new Vector3(-v.x, -v.y, -v.z);

        public static Vector3 operator *(Vector3 v, float scalar)
            => new Vector3(v.x * scalar, v.y * scalar, v.z * scalar);

        public static Vector3 operator *(float scalar, Vector3 v)
            => v * scalar;

        public static Vector3 operator /(Vector3 v, float scalar)
        {
            if (scalar == 0f) throw new DivideByZeroException("Divide Vector3 by zero.");
            return new Vector3(v.x / scalar, v.y / scalar, v.z / scalar);
        }

        public static bool operator ==(Vector3 a, Vector3 b)
            => a.Equals(b);

        public static bool operator !=(Vector3 a, Vector3 b)
            => !a.Equals(b);

        public override bool Equals(object obj)
            => obj is Vector3 other && Equals(other);

        public bool Equals(Vector3 other)
            => global::System.Math.Abs(x - other.x) < 1e-5f
            && global::System.Math.Abs(y - other.y) < 1e-5f
            && global::System.Math.Abs(z - other.z) < 1e-5f;

        public override int GetHashCode()
            => HashCode.Combine(x, y, z);

        public override string ToString()
            => $"({x:F3}, {y:F3}, {z:F3})";
    }
}
