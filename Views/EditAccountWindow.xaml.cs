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
        /// Конструктор.
        /// </summary>
        /// <param name="editAccountWindowViewModel">модель представления вида окна редактора аккаунта</param>
        public EditAccountWindow(EditAccountWindowViewModel editAccountWindowViewModel)
        {
            InitializeComponent();

            this.DataContext = editAccountWindowViewModel;
        }
    }
}
