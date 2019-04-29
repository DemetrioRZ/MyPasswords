using System.Security;
using System.Threading.Tasks;

namespace Interfaces.Logic
{
    public interface IEncryptDecryptLogic
    {
        Task<string> DecryptAsync(string encryptedJson, SecureString key);

        Task<string> EncryptAsync(string json, SecureString key);
    }
}