using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Interfaces.Logic
{
    public interface IAccountsLogic
    {
        Task<ICollection<Account>> GetAccounts(string filePath);

        Task SaveAccounts(ICollection<Account> accounts, string filePath);
    }
}