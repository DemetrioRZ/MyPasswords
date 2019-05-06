using System.Security;
using System.Windows;
using System.Windows.Controls;
using Model;

namespace Views
{
    /// <summary>
    /// Контрол для работы с паролями.
    /// </summary>
    public sealed partial class PasswordControl : UserControl
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public PasswordControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Событие обновления пароля.
        /// </summary>
        public static readonly RoutedEvent PasswordChangedEvent = EventManager.RegisterRoutedEvent("PasswordChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof (PasswordControl));

        /// <summary>
        /// Событие обновления пароля, подписка.
        /// </summary>
        public event RoutedEventHandler PasswordChanged
        {
            [SecurityCritical] add => AddHandler(PasswordBox.PasswordChangedEvent, value);
            [SecurityCritical] remove => RemoveHandler(PasswordBox.PasswordChangedEvent, value);
        }

        /// <summary>
        /// Защищённый редактируемый пароль.
        /// </summary>
        public SecureString SecurePassword
        {
            [SecurityCritical] get => PasswordBox.SecurePassword;
            [SecurityCritical] set => PasswordBox.Password = value.ToUnsecure();
        }

        /// <summary>
        /// Обработчик изменения пароля в PasswordBox.
        /// </summary>
        [SecurityCritical]
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(PasswordChangedEvent));
        }

        /// <summary>
        /// Обработчик изменения пароля в UnsecuredPasswordBox.
        /// </summary>
        [SecurityCritical]
        private void OnUnsecuredPasswordChanged(object sender, TextChangedEventArgs e)
        {
            PasswordBox.Password = UnsecuredPasswordBox.Text;
        }

        /// <summary>
        /// Обработчик переключения режима ввода пароля отобразить/скрыть.
        /// </summary>
        [SecurityCritical]
        private void OnShowPasswordButtonClick(object sender, RoutedEventArgs e)
        {
            if (ShowPasswordButton.IsChecked == true)
            {
                ForceUnsecuredPasswordTextBox(PasswordBox.Password);
                PasswordBox.Visibility = Visibility.Collapsed;
                UnsecuredPasswordBox.Visibility = Visibility.Visible;
            }
            else
            {
                ForceUnsecuredPasswordTextBox(string.Empty);
                UnsecuredPasswordBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;
            }
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

        /// <summary>
        /// Переводит фокус в активное поле ввода.
        /// </summary>
        public new void Focus()
        {
            if (ShowPasswordButton.IsChecked == true)
                UnsecuredPasswordBox.Focus();
            else
                PasswordBox.Focus();
        }
    }
}
