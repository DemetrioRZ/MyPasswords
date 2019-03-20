using System.IO;
using System.Threading.Tasks;
using Interfaces.Logic;

namespace Logic
{
    public class DecryptLogic : IDecryptLogic
    {
        public async Task<string> DecryptAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                // todo: read all bytes decode aes 256
                var fileText = File.ReadAllText(filePath);

                return fileText;
            });
        }
    }
}