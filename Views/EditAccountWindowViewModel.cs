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
        private string _password;

        /// <summary>
        /// Повторно введенный пароль.
        /// </summary>
        private string _repeatPassword;

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
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        /// <summary>
        /// Повторно введённый пароль.
        /// </summary>
        public string RepeatPassword
        {
            get => _repeatPassword;
            set
            {
                _repeatPassword = value;
                OnPropertyChanged(nameof(RepeatPassword));
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
            return !string.IsNullOrWhiteSpace(Password)
                   && !string.IsNullOrWhiteSpace(RepeatPassword)
                   && !string.IsNullOrWhiteSpace(Login)
                   && !string.IsNullOrWhiteSpace(Website)
                   && Password == RepeatPassword;
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