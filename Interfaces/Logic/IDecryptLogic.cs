using System.Threading.Tasks;

namespace Interfaces.Logic
{
    public interface IDecryptLogic
    {
        Task<string> DecryptAsync(string filePath);
    }
}