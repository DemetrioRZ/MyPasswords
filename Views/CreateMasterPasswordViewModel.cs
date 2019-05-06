using System.Security;
using System.Windows.Input;
using Interfaces.Views;
using Model;
using Views.Common;

namespace Views
{
    /// <summary>
    /// Модель представления вида окна создания пароля.
    /// </summary>
    public class CreateMasterPasswordViewModel : ViewModelBase
    {
        /// <summary>
        /// Минимальная длина пароля.
        /// </summary>
        private const int MinPasswordLength = 12;

        /// <summary>
        /// Мастер пароль.
        /// </summary>
        private SecureString _masterPassword;

        /// <summary>
        /// Повтор мастер пароля.
        /// </summary>
        private SecureString _repeatMasterPassword;

        /// <summary>
        /// Отобразить пароль.
        /// </summary>
        private bool _showPassword;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public CreateMasterPasswordViewModel()
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
            [SecurityCritical]
            set
            {
                _masterPassword = value;
                OnPropertyChanged(nameof(MasterPassword));
                OnPropertyChanged(nameof(SymbolsLeftHint));
                OnPropertyChanged(nameof(PasswordsMismatch));
            }
        }

        /// <summary>
        /// Повтор мастер пароля.
        /// </summary>
        public SecureString RepeatMasterPassword
        {
            [SecurityCritical] get => _repeatMasterPassword;
            [SecurityCritical]
            set
            {
                _repeatMasterPassword = value;
                OnPropertyChanged(nameof(RepeatMasterPassword));
                OnPropertyChanged(nameof(PasswordsMismatch));
            }
        }

        /// <summary>
        /// Показать пароль.
        /// </summary>
        public bool ShowPassword
        {
            [SecurityCritical] get => _showPassword;
            [SecurityCritical] set
            {
                _showPassword = value;
                OnPropertyChanged(nameof(ShowPassword));
                OnPropertyChanged(nameof(PasswordsMismatch));
            }
        }

        /// <summary>
        /// Осталось ввести символов для достаточной длины пароля.
        /// </summary>
        public string SymbolsLeftHint
        {
            get
            {
                if (MasterPassword == null)
                    return $"{MinPasswordLength} symbols left";

                if (MasterPassword.Length < MinPasswordLength)
                    return $"{MinPasswordLength - MasterPassword.Length} symbols left";

                return "password length is enough";
            }
        }

        /// <summary>
        /// Отображать подсказку о несовпадении паролей.
        /// </summary>
        public bool PasswordsMismatch
        {
            [SecurityCritical] get => !ShowPassword && MasterPassword?.ToUnsecure() != RepeatMasterPassword?.ToUnsecure();
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
        /// Задаёт повтор мастер пароля для модели представления вида.
        /// </summary>
        /// <param name="password">новое значение мастер пароля</param>
        public void SetRepeatMasterPasswordSecure(SecureString password)
        {
            RepeatMasterPassword = password.Copy();
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
            if (ShowPassword)
                return MasterPassword != null
                       && MasterPassword.Length >= MinPasswordLength;

            return MasterPassword != null
                   && MasterPassword.Length >= MinPasswordLength
                   && RepeatMasterPassword != null
                   && RepeatMasterPassword.Length == MasterPassword.Length
                   && !PasswordsMismatch;
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