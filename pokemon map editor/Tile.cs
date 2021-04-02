using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PokemonMapEditor
{
    public class Tile
    {
        private static readonly MD5 md5 = System.Security.Cryptography.MD5.Create();

        public string Hash { get; }

        public unsafe Tile(Bitmap image, Rectangle rect)
        {
            BitmapData bitmapData = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);
            int bpp = bitmapData.Stride / image.Width;

            byte* ptr = (byte*)bitmapData.Scan0;

            byte[] pixels = new byte[rect.Width * rect.Height * bpp];

            try
            {

                int pi = 0;

                for (int y = 0; y < bitmapData.Height; y++)
                {
                    // This is why real scan-width is important to have!
                    byte* row = ptr + (y * bitmapData.Stride);

                    for (int x = 0; x < bitmapData.Width; x++)
                    {
                        byte* pixel = row + x * bpp;
                        for (int bit = 0; bit < bpp; bit++)
                        {
                            pixels[pi++] = pixel[bit];
                        }
                    }
                }
            }
            finally
            {
                image.UnlockBits(bitmapData);
            }

            Hash = CalculateMD5Hash(pixels);
        }

        public override bool Equals(object obj)
        {
            if (obj is Tile) { return (obj as Tile).Hash.Equals(Hash); }
            return false;
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode();
        }

        private static string CalculateMD5Hash(byte[] inputBytes)
        {
            // step 1, calculate MD5 hash from input

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
