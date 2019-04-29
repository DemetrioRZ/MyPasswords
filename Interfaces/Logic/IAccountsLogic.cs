using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using Model;

namespace Interfaces.Logic
{
    /// <summary>
    /// Интерфейс логики работы с аккаунтами.
    /// </summary>
    public interface IAccountsLogic
    {
        /// <summary>
        /// Загружает аккаунты из файла.
        /// </summary>
        /// <param name="filePath">путь к файлу</param>
        /// <param name="masterPassword">мастер пароль</param>
        /// <returns>аккаунты асинхронно</returns>
        Task<ICollection<Account>> GetAccounts(string filePath, SecureString masterPassword);


        /// <summary>
        /// Сохраняет аккаунты в файл.
        /// </summary>
        /// <param name="accounts">Аккаунты</param>
        /// <param name="filePath">путь к сохраняемому файлу</param>
        /// <param name="masterPassword">мастер пароль</param>
        /// <returns>Task сохранения файла</returns>
        Task SaveAccounts(ICollection<Account> accounts, string filePath, SecureString masterPassword);
    }
}