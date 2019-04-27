using System.Windows;
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
            this.DataContext = _editAccountWindowViewModel;
        }

        /// <summary>
        /// Редактируемый аккаунт.
        /// </summary>
        public Account EditingAccount
        {
            get => _editAccountWindowViewModel.EditingAccount;
            set => _editAccountWindowViewModel.EditingAccount = value;
        }
    }
}
