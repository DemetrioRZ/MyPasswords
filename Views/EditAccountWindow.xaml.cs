using System.ComponentModel;
using System.Security;
using System.Windows;
using Interfaces.Views;
using Views.Common;

namespace Views
{
    /// <summary>
    /// Interaction logic for EditAccountWindow.xaml
    /// </summary>
    public partial class EditAccountWindow : Window, IEditAccountView
    {
        /// <summary>
        /// Модель представления вида окна.
        /// </summary>
        private readonly EditAccountViewModel _editAccountViewModel;

        /// <summary>
        /// Класс для безопасного восстановления и сохранения размеров окон в файле конфигурации приложения.
        /// </summary>
        private readonly WindowSizeRestorer _windowSizeRestorer;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="editAccountViewModel">модель представления вида окна редактора аккаунта</param>
        /// <param name="windowSizeRestorer"></param>
        public EditAccountWindow(
            EditAccountViewModel editAccountViewModel,
            WindowSizeRestorer windowSizeRestorer)
        {
            InitializeComponent();

            _editAccountViewModel = editAccountViewModel;
            _editAccountViewModel.LoadPassword = LoadPassword;
            this.DataContext = _editAccountViewModel;

            _windowSizeRestorer = windowSizeRestorer;
            _windowSizeRestorer.TryRestore(this);
        }

        /// <summary>
        /// Обработчик события загрузки окна.
        /// Можно при помощи System.Windows.Interactivity привязать команду, но пока решил не добавлять ради этого зависимость.
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _editAccountViewModel.OnLoaded();
            LoginTextBox.Focus();
        }

        /// <summary>
        /// Обработчик закрытия окна.
        /// </summary>
        private void OnClosing(object sender, CancelEventArgs e)
        {
            _windowSizeRestorer.TryStore(this);
        }

        /// <summary>
        /// Загружает пароль из модели в PasswordControl
        /// </summary>
        /// <param name="securePassword">пароль из модели SecureString</param>
        [SecurityCritical]
        private void LoadPassword(SecureString securePassword)
        {
            PasswordControl.SecurePassword = securePassword;
        }

        /// <summary>
        /// Обработчик изменения пароля.
        /// </summary>
        [SecurityCritical]
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _editAccountViewModel.SetPasswordSecure(PasswordControl.SecurePassword);
        }
    }
}
