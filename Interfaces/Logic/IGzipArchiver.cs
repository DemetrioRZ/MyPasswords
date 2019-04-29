using System.Threading.Tasks;

namespace Interfaces.Logic
{
    /// <summary>
    /// Интерфейс Gzip архиватора.
    /// </summary>
    public interface IGzipArchiver
    {
        /// <summary>
        /// Сжимает строку.
        /// </summary>
        /// <param name="uncompressedString">несжатая исходная строка UTF8</param>
        /// <returns>байты gz архива</returns>
        Task<byte[]> Compress(string uncompressedString);


        /// <summary>
        /// Распаковывает сжатую строку.
        /// </summary>
        /// <param name="compressedBytes">байты gz архива</param>
        /// <returns>распакованная строка UTF8</returns>
        Task<string> Decompress(byte[] compressedBytes);
    }
}