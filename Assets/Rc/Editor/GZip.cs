using System.Text;
using System.IO;
using System.IO.Compression;
using UnityEngine;

namespace rc
{
    static public class GZip
    {
        static public byte[] Compress(string text)
        {
            return Compress(Encoding.UTF8.GetBytes(text));
        }

        static public byte[] Compress(byte[] bytes)
        {
            var outStream = new MemoryStream();
            using (var gzipStream = new GZipStream(outStream, CompressionMode.Compress))
            {
                gzipStream.Write(bytes, 0, bytes.Length);
            }

            byte[] compressed = new byte[outStream.Length];
            outStream.Read(compressed, 0, compressed.Length);
            return compressed;
        }

        static public byte[] Decompress(string text)
        {
            return Decompress(Encoding.UTF8.GetBytes(text));
        }

        static public byte[] Decompress(byte[] bytes)
        {
            var outStream = new MemoryStream();
            using (var gzipStream = new GZipStream(outStream, CompressionMode.Decompress))
            {
                gzipStream.Write(bytes, 0, bytes.Length);
            }

            byte[] compressed = new byte[outStream.Length];
            outStream.Read(compressed, 0, compressed.Length);
            return compressed;
        }
    }
}
