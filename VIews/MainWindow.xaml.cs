using System.Windows;
using Interfaces.Views;

namespace Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindowView
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="mainWindowViewModel">Модель представления вида основного окна</param>
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();

            DataContext = mainWindowViewModel;
        }
    }
}
