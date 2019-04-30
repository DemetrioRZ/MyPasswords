using System.ComponentModel;
using System.Security;
using System.Windows;
using Interfaces.Views;

namespace Views
{
    /// <summary>
    /// Interaction logic for EditAccountWindow.xaml
    /// </summary>
    public partial class EditAccountWindow : Window, IEditAccountWindowView
    {
        /// <summary>
        /// Модель представления вида окна.
        /// </summary>
        private readonly EditAccountWindowViewModel _editAccountWindowViewModel;

        /// <summary>
        /// Класс для безопасного восстановления и сохранения размеров окон в файле конфигурации приложения.
        /// </summary>
        private readonly WindowSizeRestorer _windowSizeRestorer;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="editAccountWindowViewModel">модель представления вида окна редактора аккаунта</param>
        /// <param name="windowSizeRestorer"></param>
        public EditAccountWindow(
            EditAccountWindowViewModel editAccountWindowViewModel,
            WindowSizeRestorer windowSizeRestorer)
        {
            InitializeComponent();

            _editAccountWindowViewModel = editAccountWindowViewModel;
            _editAccountWindowViewModel.LoadPassword = LoadPassword;
            this.DataContext = _editAccountWindowViewModel;

            _windowSizeRestorer = windowSizeRestorer;
            _windowSizeRestorer.TryRestore(this);
        }

        /// <summary>
        /// Обработчик события загрузки окна.
        /// Можно при помощи System.Windows.Interactivity привязать команду, но пока решил не добавлять ради этого зависимость.
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _editAccountWindowViewModel.OnLoaded();
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
            _editAccountWindowViewModel.SetPasswordSecure(PasswordControl.SecurePassword);
        }
    }
}
