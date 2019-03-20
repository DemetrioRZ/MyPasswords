using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Interfaces.Logic
{
    public interface IAccountsSerializer
    {
        Task<string> SerializeAsync(ICollection<Account> accounts);
        
        Task<ICollection<Account>> DeserializeAsync(string json);
    }
}