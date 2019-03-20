using System.Threading.Tasks;
using Interfaces.Logic;

namespace Logic
{
    public class EncryptDecryptLogic : IEncryptDecryptLogic
    {
        private readonly IEncryptLogic _encryptLogic;

        private readonly IDecryptLogic _decryptLogic;

        public EncryptDecryptLogic(
            IEncryptLogic encryptLogic, 
            IDecryptLogic decryptLogic)
        {
            _encryptLogic = encryptLogic;
            _decryptLogic = decryptLogic;
        }

        public Task<string> DecryptAsync(string filePath)
        {
            return _decryptLogic.DecryptAsync(filePath);
        }

        public async Task EncryptAsync(string filePath, string json)
        {
            await _encryptLogic.EncryptAsync(filePath, json);
        }
    }
}