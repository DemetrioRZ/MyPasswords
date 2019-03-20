using System.Windows.Input;
using Interfaces.Logic;
using Microsoft.Win32;
using Views.Common;

namespace Views
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand OpenFileCommand => _openFileCommand ?? (_openFileCommand = new Command(OpenFile));

        public MainWindowViewModel(IDecryptLogic decryptLogic)
        {
            _decryptLogic = decryptLogic;
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