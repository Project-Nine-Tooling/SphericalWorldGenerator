using SphericalWorldGenerator.MathHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int width { get; }
        public int height { get; }
        public TextureFormat format { get; }
        public bool mipChain { get; }
        public TextureWrapMode wrapMode { get; set; }

        // internal pixel buffer
        private Color[] pixels;

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
            this.width = width;
            this.height = height;
            this.format = format;
            this.mipChain = mipChain;
            this.pixels = new Color[width * height];
            this.wrapMode = TextureWrapMode.Clamp;
        }

        /// <summary>
        /// Bulk‑set pixels (must be exactly width*height long).
        /// </summary>
        public void SetPixels(Color[] newPixels)
        {
            if (newPixels == null)
                throw new ArgumentNullException(nameof(newPixels));
            if (newPixels.Length != width * height)
                throw new ArgumentException($"Expected {width * height} pixels, got {newPixels.Length}");
            // clone to avoid external mutations
            Array.Copy(newPixels, pixels, newPixels.Length);
        }

        /// <summary>
        /// Read a single pixel, obeying wrapMode (Clamp or Repeat).
        /// </summary>
        public Color GetPixel(int x, int y)
        {
            if (wrapMode == TextureWrapMode.Repeat)
            {
                x %= width; if (x < 0) x += width;
                y %= height; if (y < 0) y += height;
            }
            else // Clamp
            {
                x = Math.Clamp(x, 0, width - 1);
                y = Math.Clamp(y, 0, height - 1);
            }

            return pixels[x + y * width];
        }

        /// <summary>
        /// “Uploads” all SetPixels changes.  No‑op in standalone.
        /// </summary>
        public void Apply()
        {
            // nothing to do in CPU‑only context
        }
    }
}
