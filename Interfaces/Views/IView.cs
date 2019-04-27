namespace Interfaces.Views
{
    public interface IView
    {
        object DataContext { get; }

        void Show();

        bool? ShowDialog();

        void Close();

        bool? DialogResult { get; set; }
    }
}