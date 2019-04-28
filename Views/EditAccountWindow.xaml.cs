using System.Security;
using System.Windows;
using System.Windows.Controls;
using Interfaces.Views;
using Model;

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
        /// Конструктор.
        /// </summary>
        /// <param name="editAccountWindowViewModel">модель представления вида окна редактора аккаунта</param>
        public EditAccountWindow(EditAccountWindowViewModel editAccountWindowViewModel)
        {
            InitializeComponent();

            _editAccountWindowViewModel = editAccountWindowViewModel;
            _editAccountWindowViewModel.LoadPassword = LoadPassword;

            this.DataContext = _editAccountWindowViewModel;
        }

        [SecurityCritical]
        private void LoadPassword(SecureString securePassword)
        {
            PasswordBox.Password = securePassword.ToUnsecure();
        }

        /// <summary>
        /// Редактируемый аккаунт.
        /// </summary>
        public Account EditingAccount
        {
            get => _editAccountWindowViewModel.EditingAccount;
            set => _editAccountWindowViewModel.EditingAccount = value;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _editAccountWindowViewModel.SetPasswordSecure(PasswordBox.SecurePassword);
        }

        private void OnUnsecuredPasswordChanged(object sender, TextChangedEventArgs e)
        {
            PasswordBox.Password = UnsecuredPasswordBox.Text;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _editAccountWindowViewModel.OnLoaded();
        }

        private void OnShowPasswordClick(object sender, RoutedEventArgs e)
        {
            if (ShowPasswordButton.IsChecked == true)
            {
                ForceUnsecurePasswordTextBox(PasswordBox.Password);
                PasswordBox.Visibility = Visibility.Collapsed;
                UnsecuredPasswordBox.Visibility = Visibility.Visible;
            }
            else
            {
                ForceUnsecurePasswordTextBox(string.Empty);
                UnsecuredPasswordBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;
            }
        }

        private void ForceUnsecurePasswordTextBox(string forceValue)
        {
            UnsecuredPasswordBox.TextChanged -= OnUnsecuredPasswordChanged;
            UnsecuredPasswordBox.Text = forceValue;
            UnsecuredPasswordBox.TextChanged += OnUnsecuredPasswordChanged;
        }
    }
}
