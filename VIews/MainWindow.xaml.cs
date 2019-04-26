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
        /// Модель представления вида основного окна.
        /// </summary>
        private readonly MainWindowViewModel _mainWindowViewModel;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="mainWindowViewModel">Модель представления вида основного окна</param>
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            
            _mainWindowViewModel = mainWindowViewModel;
            DataContext = _mainWindowViewModel;
        }
    }
}
