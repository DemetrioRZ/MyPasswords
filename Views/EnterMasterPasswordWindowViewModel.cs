using System.Security;
using System.Windows.Input;
using Interfaces.Views;
using Views.Common;

namespace Views
{
    /// <summary>
    /// Модель представления вида окна ввода мастер пароля.
    /// </summary>
    public class EnterMasterPasswordWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Мастер пароль.
        /// </summary>
        private SecureString _masterPassword;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public EnterMasterPasswordWindowViewModel()
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
        /// Мастер пароль.
        /// </summary>
        public SecureString MasterPassword
        {
            [SecurityCritical] get => _masterPassword;
            [SecurityCritical] set
            {
                _masterPassword = value;
                OnPropertyChanged(nameof(MasterPassword));
            }
        }

        /// <summary>
        /// Задаёт мастер пароль для модели представления вида.
        /// </summary>
        /// <param name="password">новое значение мастер пароля</param>
        public void SetMasterPasswordSecure(SecureString password)
        {
            MasterPassword = password.Copy();
        }

        /// <summary>
        /// Подтверждение.
        /// </summary>
        private void Ok(object param)
        {
            if (!(param is IView window))
                return;

            window.DialogResult = true;
        }

        /// <summary>
        /// CanExecute для подтверждения.
        /// </summary>
        private bool CanOk(object param)
        {
            return MasterPassword != null && MasterPassword.Length > 0;
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