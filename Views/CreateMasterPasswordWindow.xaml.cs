using System.ComponentModel;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using Interfaces.Views;
using Views.Common;

namespace Views
{
    /// <summary>
    /// Interaction logic for CreateMasterPasswordWindow.xaml
    /// </summary>
    public partial class CreateMasterPasswordWindow : Window, ICreateMasterPasswordView
    {
        private readonly CreateMasterPasswordViewModel _createMasterPasswordViewModel;

        private readonly WindowSizeRestorer _windowSizeRestorer;

        public CreateMasterPasswordWindow(
            CreateMasterPasswordViewModel createMasterPasswordViewModel, 
            WindowSizeRestorer windowSizeRestorer)
        {
            InitializeComponent();

            _createMasterPasswordViewModel = createMasterPasswordViewModel;
            DataContext = _createMasterPasswordViewModel;

            _windowSizeRestorer = windowSizeRestorer;
            _windowSizeRestorer.TryRestore(this);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            PasswordBox.Focus();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            _windowSizeRestorer.TryStore(this);
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _createMasterPasswordViewModel.SetMasterPasswordSecure(PasswordBox.SecurePassword);
        }

        [SecurityCritical]
        private void OnUnsecuredPasswordChanged(object sender, TextChangedEventArgs e)
        {
            PasswordBox.Password = UnsecuredPasswordBox.Text;
        }

        private void OnRepeatPasswordChanged(object sender, RoutedEventArgs e)
        {
            _createMasterPasswordViewModel.SetRepeatMasterPasswordSecure(RepeatPasswordBox.SecurePassword);
        }

        [SecurityCritical]
        private void OnShowPasswordCheckBoxClick(object sender, RoutedEventArgs e)
        {
            ForceUnsecuredPasswordTextBox(ShowPasswordCheckBox.IsChecked == true ? PasswordBox.Password : string.Empty);
        }

        /// <summary>
        /// Меняет значение UnsecuredPasswordBox и не генерирует событие TextChanged. 
        /// </summary>
        /// <param name="forceValue">устанавливаемое значение</param>
        [SecurityCritical]
        private void ForceUnsecuredPasswordTextBox(string forceValue)
        {
            UnsecuredPasswordBox.TextChanged -= OnUnsecuredPasswordChanged;
            UnsecuredPasswordBox.Text = forceValue;
            UnsecuredPasswordBox.TextChanged += OnUnsecuredPasswordChanged;
        }
    }
}
