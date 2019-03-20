using System.Threading.Tasks;

namespace Interfaces.Logic
{
    public interface IEncryptLogic
    {
        Task EncryptAsync(string filePath, string json);
    }
}