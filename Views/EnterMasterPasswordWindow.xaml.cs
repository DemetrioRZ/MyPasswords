using System.ComponentModel;
using System.Security;
using System.Windows;
using Interfaces.Views;

namespace Views
{
    /// <summary>
    /// Interaction logic for EnterMasterPasswordWindow.xaml
    /// </summary>
    public partial class EnterMasterPasswordWindow : Window, IEnterMasterPasswordWindowView
    {
        /// <summary>
        /// Модель представления вида окна ввода мастер пароля.
        /// </summary>
        private readonly EnterMasterPasswordWindowViewModel _enterMasterPasswordWindowViewModel;

        /// <summary>
        /// Класс для безопасного восстановления и сохранения размеров окон в файле конфигурации приложения.
        /// </summary>
        private readonly WindowSizeRestorer _windowSizeRestorer;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="enterMasterPasswordWindowViewModel">Модель представления вида окна ввода мастер пароля.</param>
        /// <param name="windowSizeRestorer">Класс для безопасного восстановления и сохранения размеров окон в файле конфигурации приложения.</param>
        public EnterMasterPasswordWindow(
            EnterMasterPasswordWindowViewModel enterMasterPasswordWindowViewModel, 
            WindowSizeRestorer windowSizeRestorer)
        {
            InitializeComponent();

            _enterMasterPasswordWindowViewModel = enterMasterPasswordWindowViewModel;
            DataContext = _enterMasterPasswordWindowViewModel;

            _windowSizeRestorer = windowSizeRestorer;
            _windowSizeRestorer.TryRestore(this);
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
            _enterMasterPasswordWindowViewModel.SetMasterPasswordSecure(PasswordControl.SecurePassword);
        }
    }
}
