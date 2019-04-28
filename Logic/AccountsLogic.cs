using System.Collections.Generic;
using System.Threading.Tasks;
using Interfaces.Logic;
using Model;

namespace Logic
{
    /// <summary>
    /// Логика работы с аккаунтами.
    /// </summary>
    public class AccountsLogic : IAccountsLogic
    {
        /// <summary>
        /// Логика шифрования.
        /// </summary>
        private readonly IEncryptDecryptLogic _encryptDecryptLogic;

        /// <summary>
        /// Логика сериализации.
        /// </summary>
        private readonly IAccountsSerializer _accountsSerializer;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="encryptDecryptLogic">Логика шифрования</param>
        /// <param name="accountsSerializer">Логика сериализации</param>
        public AccountsLogic(
            IEncryptDecryptLogic encryptDecryptLogic, 
            IAccountsSerializer accountsSerializer)
        {
            _encryptDecryptLogic = encryptDecryptLogic;
            _accountsSerializer = accountsSerializer;
        }

        /// <summary>
        /// Загружает аккаунты из файла.
        /// </summary>
        /// <param name="filePath">путь к файлу</param>
        /// <returns>аккаунты асинхронно</returns>
        public async Task<ICollection<Account>> GetAccounts(string filePath)
        {
            var decryptedJson = await _encryptDecryptLogic.DecryptAsync(filePath);

            var result = await _accountsSerializer.DeserializeAsync(decryptedJson);

            return result;
        }

        /// <summary>
        /// Сохраняет аккаунты в файл.
        /// </summary>
        /// <param name="accounts">Аккаунты</param>
        /// <param name="filePath">путь к сохраняемому файлу</param>
        /// <returns>таск сохранения файла</returns>
        public async Task SaveAccounts(ICollection<Account> accounts, string filePath)
        {
            var json = await _accountsSerializer.SerializeAsync(accounts);

            await _encryptDecryptLogic.EncryptAsync(filePath, json);
        }
    }
}