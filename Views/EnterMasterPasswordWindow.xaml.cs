using System.ComponentModel;
using System.Security;
using System.Windows;
using Interfaces.Views;
using Views.Common;

namespace Views
{
    /// <summary>
    /// Interaction logic for EnterMasterPasswordWindow.xaml
    /// </summary>
    public partial class EnterMasterPasswordWindow : Window, IEnterMasterPasswordView
    {
        /// <summary>
        /// Модель представления вида окна ввода мастер пароля.
        /// </summary>
        private readonly EnterMasterPasswordViewModel _enterMasterPasswordViewModel;

        /// <summary>
        /// Класс для безопасного восстановления и сохранения размеров окон в файле конфигурации приложения.
        /// </summary>
        private readonly WindowSizeRestorer _windowSizeRestorer;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="enterMasterPasswordViewModel">Модель представления вида окна ввода мастер пароля.</param>
        /// <param name="windowSizeRestorer">Класс для безопасного восстановления и сохранения размеров окон в файле конфигурации приложения.</param>
        public EnterMasterPasswordWindow(
            EnterMasterPasswordViewModel enterMasterPasswordViewModel, 
            WindowSizeRestorer windowSizeRestorer)
        {
            InitializeComponent();

            _enterMasterPasswordViewModel = enterMasterPasswordViewModel;
            DataContext = _enterMasterPasswordViewModel;

            _windowSizeRestorer = windowSizeRestorer;
            _windowSizeRestorer.TryRestore(this);
        }

        /// <summary>
        /// Обработчик загрузки окна.
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            PasswordControl.Focus();
        }

        /// <summary>
        /// Обработчик закрытия окна.
        /// </summary>
        private void OnClosing(object sender, CancelEventArgs e)
        {
            _windowSizeRestorer.TryStore(this);
        }

        /// <summary>
        /// Обработчик смены вводимого пароля.
        /// </summary>
        [SecurityCritical]
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _enterMasterPasswordViewModel.SetMasterPasswordSecure(PasswordControl.SecurePassword);
        }
    }
}
