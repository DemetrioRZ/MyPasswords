using System.Security;
using System.Windows;
using System.Windows.Input;
using Interfaces.Views;
using Model;
using Views.Common;

namespace Views
{
    public class CreateMasterPasswordViewModel : ViewModelBase
    {
        private const int MinPasswordLength = 12;

        private SecureString _masterPassword;

        private SecureString _repeatMasterPassword;

        private bool _showPassword;

        public CreateMasterPasswordViewModel()
        {
            InitializeCommands();
        }

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

        public bool ShowPassword
        {
            [SecurityCritical] get => _showPassword;
            [SecurityCritical] set
            {
                _showPassword = value;
                OnPropertyChanged(nameof(ShowPassword));
            }
        }

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

        public bool PasswordsMismatch
        {
            [SecurityCritical] get => MasterPassword?.ToUnsecure() != RepeatMasterPassword?.ToUnsecure();
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
                   && RepeatMasterPassword.Length == MasterPassword.Length;
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