using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Interfaces.Logic;
using Interfaces.ViewModels;
using Microsoft.Win32;
using Views.Common;

namespace Views
{
    public class MainWindowViewModel : INotifyPropertyChanged, IMainWindowViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public ICommand OpenFileCommand => _openFileCommand ?? (_openFileCommand = new Command(OpenFile));

        public MainWindowViewModel(IDecryptLogic decryptLogic)
        {
            _decryptLogic = decryptLogic;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void OpenFile(object param)
        {
            var ofd = new OpenFileDialog {Multiselect = false, Filter = "All files (*.*)|*.*"};
            
            if (ofd.ShowDialog() != true)
                return;

            _filePath = ofd.FileName;

            if (string.IsNullOrWhiteSpace(_filePath))
                return;

            var decrypted = await _decryptLogic.Decrypt(_filePath);
        }

        private ICommand _openFileCommand;

        private string _filePath;

        private readonly IDecryptLogic _decryptLogic;
    }
}