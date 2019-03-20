using System.IO;
using System.Threading.Tasks;
using Interfaces.Logic;

namespace Logic
{
    public class EncryptLogic : IEncryptLogic
    {
        public async Task EncryptAsync(string filePath, string json)
        {
            // todo: encrypt and write bytes to file
            await Task.Run(() =>
            {
                File.WriteAllText(filePath, json);
            });
        }
    }
}