using System.Collections.Generic;
using System.Threading.Tasks;
using Interfaces.Logic;
using Model;

namespace Logic
{
    public class AccountsLogic : IAccountsLogic
    {
        private readonly IEncryptDecryptLogic _encryptDecryptLogic;

        private readonly IAccountsSerializer _accountsSerializer;

        public AccountsLogic(
            IEncryptDecryptLogic encryptDecryptLogic, 
            IAccountsSerializer accountsSerializer)
        {
            _encryptDecryptLogic = encryptDecryptLogic;
            _accountsSerializer = accountsSerializer;
        }

        public async Task<ICollection<Account>> GetAccounts(string filePath)
        {
            var decryptedJson = await _encryptDecryptLogic.DecryptAsync(filePath);

            var result = await _accountsSerializer.DeserializeAsync(decryptedJson);

            return result;
        }

        public async Task SaveAccounts(ICollection<Account> accounts, string filePath)
        {
            var json = await _accountsSerializer.SerializeAsync(accounts);

            await _encryptDecryptLogic.EncryptAsync(filePath, json);
        }
    }
}