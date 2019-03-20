namespace Interfaces.Views
{
    public interface IView
    {
        object DataContext { get; }

        void Show();

        bool? ShowDialog();
    }
}