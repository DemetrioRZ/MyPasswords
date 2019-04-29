using System.Collections.Generic;
using System.IO;
using System.Security;
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
        /// Gzip архиватор.
        /// </summary>
        private readonly IGzipArchiver _gzipArchiver;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="encryptDecryptLogic">Логика шифрования</param>
        /// <param name="accountsSerializer">Логика сериализации</param>
        /// <param name="gzipArchiver">Gzip архиватор</param>
        public AccountsLogic(
            IEncryptDecryptLogic encryptDecryptLogic, 
            IAccountsSerializer accountsSerializer, 
            IGzipArchiver gzipArchiver)
        {
            _encryptDecryptLogic = encryptDecryptLogic;
            _accountsSerializer = accountsSerializer;
            _gzipArchiver = gzipArchiver;
        }

        /// <summary>
        /// Загружает аккаунты из файла.
        /// </summary>
        /// <param name="filePath">путь к файлу</param>
        /// <param name="masterPassword">мастер пароль</param>
        /// <returns>аккаунты асинхронно</returns>
        public async Task<ICollection<Account>> GetAccounts(string filePath, SecureString masterPassword)
        {
            var fileBytes = await Task.Run(() => File.ReadAllBytes(filePath));

            var encryptedJson = await _gzipArchiver.Decompress(fileBytes);

            var decryptedJson = await _encryptDecryptLogic.DecryptAsync(encryptedJson, masterPassword);

            var result = await _accountsSerializer.DeserializeAsync(decryptedJson);

            return result;
        }

        /// <summary>
        /// Сохраняет аккаунты в файл.
        /// </summary>
        /// <param name="accounts">Аккаунты</param>
        /// <param name="filePath">путь к сохраняемому файлу</param>
        /// <param name="masterPassword">мастер пароль</param>
        /// <returns>Task сохранения файла</returns>
        public async Task SaveAccounts(ICollection<Account> accounts, string filePath, SecureString masterPassword)
        {
            var json = await _accountsSerializer.SerializeAsync(accounts);

            var encryptedJson = await _encryptDecryptLogic.EncryptAsync(json, masterPassword);

            var fileBytes = await _gzipArchiver.Compress(encryptedJson);

            await Task.Run(() => { File.WriteAllBytes(filePath, fileBytes); });
        }
    }
}