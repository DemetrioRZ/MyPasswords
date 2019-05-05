using System;
using System.Security;
using System.Windows.Input;
using Interfaces.Views;
using Views.Common;

namespace Views
{
    /// <summary>
    /// Модель представления вида окна редактирования аккаунта.
    /// </summary>
    public class EditAccountViewModel : ViewModelBase
    {
        /// <summary>
        /// Редактируемый аккаунт.
        /// </summary>
        private AccountViewModel _account;

        /// <summary>
        /// Логин.
        /// </summary>
        private string _login;

        /// <summary>
        /// Пароль.
        /// </summary>
        private SecureString _password;

        /// <summary>
        /// Название ресурса.
        /// </summary>
        private string _resourceName;

        /// <summary>
        /// Тип аккаунта.
        /// </summary>
        private string _accountType;

        /// <summary>
        /// Комментарий.
        /// </summary>
        private string _comment;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public EditAccountViewModel()
        {
            InitializeCommands();
        }

        /// <summary>
        /// Инициализация команд.
        /// </summary>
        private void InitializeCommands()
        {
            OkCommand = new Command(Ok, CanOk);
            CancelCommand = new Command(Cancel);
        }

        /// <summary>
        /// Команда подтверждения.
        /// </summary>
        public ICommand OkCommand { get; private set; }

        /// <summary>
        /// Команда отмены.
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        /// <summary>
        /// Редактируемый аккаунт.
        /// </summary>
        public AccountViewModel EditingAccount
        {
            get => _account;
            set
            {
                _account = value;

                Login = _account.Login;
                Password = _account.Password.Copy();
                ResourceName = _account.ResourceName;
                AccountType = _account.AccountType;
                Comment = _account.Comment;
            }
        }

        /// <summary>
        /// Метод загрузки пароля в PasswordBox при первой загрузке.
        /// </summary>
        public Action<SecureString> LoadPassword { get; set; } 

        /// <summary>
        /// Логин.
        /// </summary>
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        /// <summary>
        /// Пароль.
        /// </summary>
        public SecureString Password
        {
            [SecurityCritical] get => _password;
            [SecurityCritical] set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        /// <summary>
        /// Название ресурса.
        /// </summary>
        public string ResourceName
        {
            get => _resourceName;
            set
            {
                _resourceName = value;
                OnPropertyChanged(nameof(ResourceName));
            }
        }

        /// <summary>
        /// Тип аккаунта.
        /// </summary>
        public string AccountType
        {
            get => _accountType;
            set
            {
                _accountType = value;
                OnPropertyChanged(AccountType);
            }
        }

        /// <summary>
        /// Комментарий.
        /// </summary>
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

        /// <summary>
        /// Задаёт пароль для модели.
        /// </summary>
        /// <param name="password">новое значение пароля</param>
        public void SetPasswordSecure(SecureString password)
        {
            Password = password.Copy();
        }

        /// <summary>
        /// Вызывается при загрузке окна редактора, передаёт пароль в PasswordBox.
        /// </summary>
        public void OnLoaded()
        {
            LoadPassword?.Invoke(Password.Copy());
        }

        /// <summary>
        /// Подтверждение.
        /// </summary>
        private void Ok(object param)
        {
            if (!(param is IView window))
                return;

            EditingAccount.Login = Login;
            EditingAccount.Password = Password.Copy();
            EditingAccount.ResourceName = ResourceName;
            EditingAccount.AccountType = AccountType;
            EditingAccount.Comment = Comment;
            
            window.DialogResult = true;
        }

        /// <summary>
        /// CanExecute для подтверждения.
        /// </summary>
        private bool CanOk(object param)
        {
            return Password.Length > 0
                   && !string.IsNullOrWhiteSpace(Login)
                   && !string.IsNullOrWhiteSpace(ResourceName);
        }

        /// <summary>
        /// Отмена.
        /// </summary>
        private void Cancel(object param)
        {
            if (!(param is IView window))
                return;

            window.DialogResult = false;
        }
    }
}