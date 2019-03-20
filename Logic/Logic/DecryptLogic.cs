using System.IO;
using System.Threading.Tasks;
using Interfaces.Logic;

namespace Logic.Logic
{
    public class DecryptLogic : IDecryptLogic
    {
        public async Task<string> Decrypt(string filePath)
        {
            return await Task.Run(() =>
            {
                var fileBytes = File.ReadAllBytes(filePath);

                var result = Decrypt(fileBytes);

                return result;
            });
        }

        private string Decrypt(byte[] fileBytes)
        {
            return "";
        }
    }
}