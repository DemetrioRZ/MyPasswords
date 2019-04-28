using System;
using System.Security;
using System.Windows.Input;
using Interfaces.Views;
using Model;
using Views.Common;

namespace Views
{
    /// <summary>
    /// Модель представления вида окна редактирования аккаунта.
    /// </summary>
    public class EditAccountWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Редактируемый аккаунт.
        /// </summary>
        private Account _account;

        /// <summary>
        /// Логин.
        /// </summary>
        private string _login;

        /// <summary>
        /// Пароль.
        /// </summary>
        private SecureString _password;

        /// <summary>
        /// Сайт ресурса.
        /// </summary>
        private string _website;

        /// <summary>
        /// Комментарий.
        /// </summary>
        private string _comment;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public EditAccountWindowViewModel()
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
        public Account EditingAccount
        {
            get => _account;
            set
            {
                _account = value;

                Login = _account.Login;
                Password = _account.Password;
                Website = _account.WebSite;
                Comment = _account.Comment;
            }
        }

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
        /// Логин.
        /// </summary>
        public SecureString Password
        {
            get => _password ?? (_password = new SecureString());
            [SecurityCritical] set
            {
                if (_password == null)
                    _password = new SecureString();

                _password.Clear();
                foreach (var ch in value.ToUnsecure())
                    _password.AppendChar(ch);
                OnPropertyChanged(nameof(Password));
            }
        }

        /// <summary>
        /// Сайт ресурса.
        /// </summary>
        public string Website
        {
            get => _website;
            set
            {
                _website = value;
                OnPropertyChanged(nameof(Website));
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

        public void SetPasswordSecure(SecureString password)
        {
            Password = password;
        }

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
            EditingAccount.Password = Password;
            EditingAccount.WebSite = Website;
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
                   && !string.IsNullOrWhiteSpace(Website);
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