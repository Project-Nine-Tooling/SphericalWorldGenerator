using Divooka.Multimedia.Image;
using SphericalWorldGenerator.Maths;
using System;

namespace SphericalWorldGenerator.Media
{
    /// <summary>
    /// Matches UnityEngine.TextureFormat for your needs.
    /// </summary>
    public enum TextureFormat
    {
        ARGB32
        // add more formats here if you actually need them
    }

    /// <summary>
    /// Matches UnityEngine.TextureWrapMode for your needs.
    /// </summary>
    public enum TextureWrapMode
    {
        Clamp,
        Repeat
    }

    /// <summary>
    /// Minimal Texture2D stub so your standalone code compiles.
    /// </summary>
    public class Texture2D
    {
        #region Settings
        public int Width { get; }
        public int Height { get; }
        public TextureFormat Format { get; }
        /// <remarks>
        /// Ignored on CPU implementation here.
        /// </remarks>
        public bool MipChain { get; }
        public TextureWrapMode WrapMode { get; set; }
        #endregion

        #region Data
        public PixelImage Data { get; private set; }
        #endregion

        #region Construction
        /// <summary>
        /// Simple ctor: defaults to ARGB32, no mip‑chain.
        /// </summary>
        public Texture2D(int width, int height)
            : this(width, height, TextureFormat.ARGB32, false) { }
        /// <summary>
        /// Full ctor so you can request mipChain if you like.
        /// </summary>
        public Texture2D(int width, int height, TextureFormat format, bool mipChain)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Texture dimensions must be positive");
            Width = width;
            Height = height;
            Format = format;
            MipChain = mipChain;
            WrapMode = TextureWrapMode.Clamp;

            // Initialize blank image (transparent black)
            Data = new PixelImage(width, height, new Divooka.Multimedia.Image.Color(0, 0, 0, 0));
        }
        #endregion

        #region Methods
        /// <summary>
        /// Bulk‑set pixels (must be exactly width*height long).
        /// </summary>
        public void SetPixels(SphericalWorldGenerator.Maths.Color[] newPixels)
        {
            if (newPixels == null)
                throw new ArgumentNullException(nameof(newPixels));
            if (newPixels.Length != Width * Height)
                throw new ArgumentException($"Expected {Width * Height} pixels, got {newPixels.Length}");

            // Convert your Math.Color[] → backend Pixel[]
            Pixel[] flat = new Pixel[newPixels.Length];
            for (int i = 0; i < flat.Length; i++)
            {
                Maths.Color c = newPixels[i];
                // Remark: c.R/G/B/A are floats in [0..1]
                byte r = (byte)(System.Math.Clamp(c.R, 0f, 1f) * 255);
                byte g = (byte)(System.Math.Clamp(c.G, 0f, 1f) * 255);
                byte b = (byte)(System.Math.Clamp(c.B, 0f, 1f) * 255);
                byte a = (byte)(System.Math.Clamp(c.A, 0f, 1f) * 255);
                flat[i] = new Pixel(r, g, b, a);
            }

            // re‑create the PixelImage from flat array
            Data = new PixelImage(Width, Height, flat);
        }
        /// <summary>
        /// Read a single pixel, obeying wrapMode (Clamp or Repeat).
        /// </summary>
        public SphericalWorldGenerator.Maths.Color GetPixel(int x, int y)
        {
            // Apply wrap or clamp
            if (WrapMode == TextureWrapMode.Repeat)
            {
                x %= Width; if (x < 0) x += Width;
                y %= Height; if (y < 0) y += Height;
            }
            else // Clamp
            {
                x = System.Math.Clamp(x, 0, Width - 1);
                y = System.Math.Clamp(y, 0, Height - 1);
            }

            var p = Data.Pixels![y][x];
            // Convert backend Pixel → Maths.Color (floats [0..1])
            return new SphericalWorldGenerator.Maths.Color(
                p.Red / 255f,
                p.Green / 255f,
                p.Blue / 255f,
                p.Alpha / 255f
            );
        }
        /// <summary>
        /// "Uploads" all SetPixels changes.  No‑op in standalone.
        /// </summary>
        public void Apply()
        {
            // nothing to do in CPU‑only context
        }
        #endregion
    }
}
