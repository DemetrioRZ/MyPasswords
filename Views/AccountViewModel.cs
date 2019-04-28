using System.Security;
using Model;
using Views.Common;

namespace Views
{
    /// <summary>
    /// Модель представления вида аккаунта
    /// </summary>
    public class AccountViewModel : ViewModelBase
    {
        /// <summary>
        /// Модель аккаунта.
        /// </summary>
        private Account _account;

        /// <summary>
        /// Логин.
        /// </summary>
        public string Login
        {
            get => _account.Login;
            set
            {
                _account.Login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        /// <summary>
        /// Пароль.
        /// </summary>
        public SecureString Password
        {
            get => _account.Password;
            set => _account.Password = value;
        }

        /// <summary>
        /// Сайт ресурса.
        /// </summary>
        public string WebSite
        {
            get => _account.WebSite;
            set
            {
                _account.WebSite = value;
                OnPropertyChanged(nameof(WebSite));
            }
        }

        /// <summary>
        /// Комментарий к ресурсу.
        /// </summary>
        public string Comment
        {
            get => _account.Comment;
            set
            {
                _account.Comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

        /// <summary>
        /// Создает модель представления вида для модели аккаунта.
        /// </summary>
        /// <param name="account">модель аккаунта</param>
        /// <returns>этот же объект</returns>
        public AccountViewModel For(Account account)
        {
            _account = account;

            return this;
        }

        /// <summary>
        /// Возвращает модель.
        /// </summary>
        /// <returns>модель аккаунта</returns>
        public Account GetModel()
        {
            return _account;
        }
    }
}