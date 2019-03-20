using System.Threading.Tasks;

namespace Interfaces.Logic
{
    public interface IDecryptLogic
    {
        Task<string> Decrypt(string filePath);
    }
}