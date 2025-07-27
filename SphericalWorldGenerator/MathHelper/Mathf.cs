using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SphericalWorldGenerator.MathHelper
{
    public static class Mathf
    {
        // constants
        public const float PI = 3.141592653589793238462643383f;
        public const float Deg2Rad = PI / 180f;
        public const float Rad2Deg = 180f / PI;
        public const float Epsilon = 1.401298E-45f;              // smallest positive float value
        public static readonly float Infinity = float.PositiveInfinity;
        public static readonly float NegativeInfinity = float.NegativeInfinity;

        // --- trigonometry & exponentials ---
        public static float Sin(float f) => (float)Math.Sin(f);
        public static float Cos(float f) => (float)Math.Cos(f);
        public static float Tan(float f) => (float)Math.Tan(f);
        public static float Asin(float f) => (float)Math.Asin(f);
        public static float Acos(float f) => (float)Math.Acos(f);
        public static float Atan(float f) => (float)Math.Atan(f);
        public static float Atan2(float y, float x) => (float)Math.Atan2(y, x);
        public static float Sqrt(float f) => (float)Math.Sqrt(f);
        public static float Pow(float f, float p) => (float)Math.Pow(f, p);
        public static float Exp(float power) => (float)Math.Exp(power);
        public static float Log(float f, float p) => (float)Math.Log(f, p);
        public static float Log(float f) => (float)Math.Log(f);
        public static float Log10(float f) => (float)Math.Log10(f);

        // --- absolute / sign / min / max ---
        public static float Abs(float f) => Math.Abs(f);
        public static int Abs(int v) => Math.Abs(v);
        public static float Min(float a, float b) => a < b ? a : b;
        public static float Max(float a, float b) => a > b ? a : b;
        public static float Sign(float f) => f >= 0f ? 1f : -1f;

        // --- rounding ---
        public static float Ceil(float f) => (float)Math.Ceiling(f);
        public static float Floor(float f) => (float)Math.Floor(f);
        public static float Round(float f) => (float)Math.Round(f);
        public static int CeilToInt(float f) => (int)Math.Ceiling(f);
        public static int FloorToInt(float f) => (int)Math.Floor(f);
        public static int RoundToInt(float f) => (int)Math.Round(f);

        // --- clamp & interpolate ---
        public static float Clamp(float v, float min, float max)
            => v < min ? min : v > max ? max : v;
        public static int Clamp(int v, int min, int max)
            => v < min ? min : v > max ? max : v;
        public static float Clamp01(float v)
            => v < 0f ? 0f : v > 1f ? 1f : v;

        public static float Lerp(float a, float b, float t)
            => a + (b - a) * Clamp01(t);
        public static float LerpUnclamped(float a, float b, float t)
            => a + (b - a) * t;
        public static float LerpAngle(float a, float b, float t)
        {
            float delta = DeltaAngle(a, b);
            return a + delta * Clamp01(t);
        }

        // --- movement helpers ---
        public static float MoveTowards(float current, float target, float maxDelta)
        {
            float diff = target - current;
            if (Math.Abs(diff) <= maxDelta) return target;
            return current + Sign(diff) * maxDelta;
        }

        public static float MoveTowardsAngle(float current, float target, float maxDelta)
        {
            float delta = DeltaAngle(current, target);
            if (-maxDelta < delta && delta < maxDelta)
                return target;
            return MoveTowards(current, current + delta, maxDelta);
        }

        public static float SmoothStep(float from, float to, float t)
        {
            t = Clamp01(t);
            // hermite: (−2t³ + 3t²)
            t = t * t * (3f - 2f * t);
            return from + (to - from) * t;
        }

        // match Unity’s gamma / linear transitions
        public static float Gamma(float value, float absmax, float gamma)
        {
            bool neg = value < 0f;
            if (neg) value = -value;
            if (value > absmax) return neg ? -value : value;
            float res = (float)Math.Pow(value / absmax, gamma) * absmax;
            return neg ? -res : res;
        }

        // true if within tolerance
        public static bool Approximately(float a, float b)
        {
            return Abs(b - a) <
                   Math.Max(1E-06f * Math.Max(Abs(a), Abs(b)), Epsilon * 8f);
        }

        // critically damped spring smoothing
        public static float SmoothDamp(
            float current,
            float target,
            ref float currentVelocity,
            float smoothTime,
            float maxSpeed,
            float deltaTime)
        {
            smoothTime = Math.Max(0.0001f, smoothTime);
            float omega = 2f / smoothTime;
            float x = omega * deltaTime;
            float exp = 1f / (1f + x + 0.48f * x * x + 0.235f * x * x * x);
            float change = current - target;
            float origTgt = target;

            // clamp max speed
            float maxChange = maxSpeed * smoothTime;
            change = Clamp(change, -maxChange, maxChange);
            target = current - change;

            float temp = (currentVelocity + omega * change) * deltaTime;
            currentVelocity = (currentVelocity - omega * temp) * exp;

            float output = target + (change + temp) * exp;

            // ensure no overshoot
            if ((origTgt - current > 0f) == (output > origTgt))
            {
                output = origTgt;
                currentVelocity = (output - origTgt) / deltaTime;
            }

            return output;
        }

        public static float SmoothDamp(
            float current,
            float target,
            ref float currentVelocity,
            float smoothTime,
            float deltaTime)
        {
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, float.PositiveInfinity, deltaTime);
        }

        // wrap & ping‐pong
        public static float Repeat(float t, float length)
        {
            return Clamp(t - (float)Math.Floor(t / length) * length, 0f, length);
        }

        public static float PingPong(float t, float length)
        {
            t = Repeat(t, length * 2f);
            return length - Abs(t - length);
        }

        // inverse of Lerp
        public static float InverseLerp(float a, float b, float value)
        {
            if (a != b)
                return Clamp01((value - a) / (b - a));
            return 0f;
        }

        // shortest difference between two angles (in degrees)
        public static float DeltaAngle(float current, float target)
        {
            float delta = Repeat((target - current), 360f);
            if (delta > 180f) delta -= 360f;
            return delta;
        }
    }
}
