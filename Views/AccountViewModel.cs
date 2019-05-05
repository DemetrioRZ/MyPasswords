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
        /// Название ресурса.
        /// </summary>
        public string ResourceName
        {
            get => _account.ResourceName;
            set
            {
                _account.ResourceName = value;
                OnPropertyChanged(nameof(ResourceName));
            }
        }

        /// <summary>
        /// Тип аккаунта, произвольное значение, почта, музыкальный сервис, игровой аккаунт и т д.
        /// </summary>
        public string AccountType
        {
            get => _account.AccountType;
            set
            {
                _account.AccountType = value;
                OnPropertyChanged(AccountType);
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