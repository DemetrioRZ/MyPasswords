using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Logic;

namespace Logic
{
    /// <summary>
    /// Архиватор Gzip.
    /// </summary>
    public class GzipArchiver : IGzipArchiver
    {
        /// <summary>
        /// Сжимает строку.
        /// </summary>
        /// <param name="uncompressedString">несжатая исходная строка UTF8</param>
        /// <returns>байты gz архива</returns>
        public async Task<byte[]> Compress(string uncompressedString)
        {
            return await Task.Run(() =>
            {
                using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
                {
                    using (var compressedStream = new MemoryStream())
                    { 
                        using (var compressStream = new GZipStream(compressedStream, CompressionMode.Compress))
                        {
                            uncompressedStream.CopyTo(compressStream);
                        }

                        return compressedStream.ToArray();
                    }
                }
            });
        }

        /// <summary>
        /// Распаковывает сжатую строку.
        /// </summary>
        /// <param name="compressedBytes">байты gz архива</param>
        /// <returns>распакованная строка UTF8</returns>
        public async Task<string> Decompress(byte[] compressedBytes)
        {
            return await Task.Run(() =>
            {
                using (var compressedStream = new MemoryStream(compressedBytes))
                {
                    using (var decompressStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                    {
                        using (var decompressedStream = new MemoryStream())
                        {
                            decompressStream.CopyTo(decompressedStream);

                            var decompressedBytes = decompressedStream.ToArray();

                            return Encoding.UTF8.GetString(decompressedBytes);
                        }
                    }   
                }
            });
        }
    }
}