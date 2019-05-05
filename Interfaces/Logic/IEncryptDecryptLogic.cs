using System.Security;
using System.Threading.Tasks;

namespace Interfaces.Logic
{
    public interface IEncryptDecryptLogic
    {
        Task<string> DecryptAsync(string encryptedJson, SecureString masterPassword);

        Task<string> EncryptAsync(string json, SecureString masterPassword);
    }
}