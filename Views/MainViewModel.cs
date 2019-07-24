using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Interfaces.Logic;
using Interfaces.Views;
using Microsoft.Win32;
using Model;
using Views.Common;

namespace Views
{
    /// <summary>
    /// Модель представления вида основного окна.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Логика работы с аккаунтами
        /// </summary>
        private readonly IAccountsLogic _accountsLogic;

        /// <summary>
        /// Функтор получения окна редактора аккаунта
        /// </summary>
        private readonly Func<IEditAccountView> _getEditAccountWindowView;

        /// <summary>
        /// Функтор получения окна ввода мастер пароля.
        /// </summary>
        private readonly Func<IEnterMasterPasswordView> _getEnterMasterPasswordView;

        /// <summary>
        /// Функтор получения окна создания мастер пароля.
        /// </summary>
        private readonly Func<ICreateMasterPasswordView> _getCreateMasterPasswordView;

        /// <summary>
        /// Создатель документов для печати.
        /// </summary>
        private readonly FlowDocumentCreator _flowDocumentCreator;

        /// <summary>
        /// Путь к сериализованному файлу с аккаунтами.
        /// </summary>
        private string _serializedAccountsFilePath;

        /// <summary>
        /// Заголовок окна.
        /// </summary>
        private string _title;

        /// <summary>
        /// Название приложения для заголовка.
        /// </summary>
        private const string AppName = "MyPasswords";

        /// <summary>
        /// Мастер пароль.
        /// </summary>
        [SecurityCritical]
        private SecureString _masterPassword;

        /// <summary>
        /// Коллекция отображаемых аккаунтов
        /// </summary>
        private ObservableCollection<AccountViewModel> _accounts;

        /// <summary>
        /// Выбранная модель аккаунта.
        /// </summary>
        private AccountViewModel _selectedAccount;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="accountsLogic">Логика работы с аккаунтами</param>
        /// <param name="getEditAccountWindowView">Функтор получения окна редактора аккаунта</param>
        /// <param name="getEnterMasterPasswordView">Функтор получения окна ввода мастер пароля</param>
        /// <param name="getCreateMasterPasswordView">Функтор получения окна создания мастер пароля</param>
        public MainViewModel(
            IAccountsLogic accountsLogic, 
            Func<IEditAccountView> getEditAccountWindowView, 
            Func<IEnterMasterPasswordView> getEnterMasterPasswordView, 
            Func<ICreateMasterPasswordView> getCreateMasterPasswordView, FlowDocumentCreator flowDocumentCreator)
        {
            _accountsLogic = accountsLogic;
            _getEditAccountWindowView = getEditAccountWindowView;
            _getEnterMasterPasswordView = getEnterMasterPasswordView;
            _getCreateMasterPasswordView = getCreateMasterPasswordView;
            _flowDocumentCreator = flowDocumentCreator;

            InitializeCommands();
            _title = AppName;
        }

        /// <summary>
        /// Команда создания нового файла с аккаунтами.
        /// </summary>
        public ICommand NewFileCommand { get; private set; }

        /// <summary>
        /// Команда открытия файла с аккаунтами.
        /// </summary>
        public ICommand OpenFileCommand { get; private set; }

        /// <summary>
        /// Команда сохранения файла с аккаунтами.
        /// </summary>
        public ICommand SaveFileCommand { get; private set; }

        /// <summary>
        /// Команда сохранить как для файла с аккаунтами.
        /// </summary>
        public ICommand SaveFileAsCommand { get; private set; }

        /// <summary>
        /// Команда создания нового аккаунта для хранения.
        /// </summary>
        public ICommand CreateAccountCommand { get; private set; }

        /// <summary>
        /// Команда редактирования выбранного аккаунта.
        /// </summary>
        public ICommand EditAccountCommand { get; private set; }

        /// <summary>
        /// Команда удаления выбранного аккаунта.
        /// </summary>
        public ICommand DeleteAccountCommand { get; private set; }

        /// <summary>
        /// Команда печати файла с аккаунтами.
        /// </summary>
        public ICommand PrintCommand { get; private set; }

        /// <summary>
        /// Коллекция отображаемых аккаунтов
        /// </summary>
        public ObservableCollection<AccountViewModel> Accounts
        {
            get => _accounts;
            private set
            {
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
                OnPropertyChanged(nameof(CanShowGrid));
                OnPropertyChanged(nameof(CanShowHintLabel));
            }
        }

        /// <summary>
        /// Проверка наличия открытого или созданного списка аккаунтов.
        /// </summary>
        public bool CanShowGrid => _accounts != null;

        /// <summary>
        /// Проверка показывать ли подсказку с чего начать работу.
        /// </summary>
        public bool CanShowHintLabel => _accounts == null;

        /// <summary>
        /// Заголовок окна.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                _title = $"{AppName} - {value}";
                OnPropertyChanged(nameof(Title));
            }
        }

        /// <summary>
        /// Выбранный аккаунт.
        /// </summary>
        public AccountViewModel SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
            }
        }

        /// <summary>
        /// Инициализация команд.
        /// </summary>
        private void InitializeCommands()
        {
            NewFileCommand = new Command(NewFile);
            OpenFileCommand = new Command(OpenFileAsync);
            SaveFileCommand = new Command(SaveFile, x => Accounts != null);
            SaveFileAsCommand = new Command(SaveFileAs, x => Accounts != null);
            CreateAccountCommand = new Command(CreateAccount, x => Accounts != null);
            EditAccountCommand = new Command(EditAccount, x => Accounts != null && Accounts.Any() && SelectedAccount != null);
            DeleteAccountCommand = new Command(DeleteAccount, x => Accounts != null && Accounts.Any() && SelectedAccount != null);
            PrintCommand = new Command(PrintAccounts, x => Accounts != null && Accounts.Any());
        }

        /// <summary>
        /// Создаёт новый для хранения аккаунтов.
        /// </summary>
        private void NewFile(object param)
        {
            if (Accounts != null && MessageBox.Show("Create new file?", "New file", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                return;

            _serializedAccountsFilePath = null;
            _masterPassword?.Dispose();
            _masterPassword = null;

            Accounts = new ObservableCollection<AccountViewModel>(new List<AccountViewModel>());
            Title = "New file";
        }

        /// <summary>
        /// Открывает новый файл с аккаунтами.
        /// </summary>
        private async void OpenFileAsync(object param)
        {
            if (Accounts != null && MessageBox.Show("Open existing file?", "Open file", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                return;

            var ofd = new OpenFileDialog {Multiselect = false, Filter = "All files (*.*)|*.*"};
            
            if (ofd.ShowDialog() != true)
                return;

            _serializedAccountsFilePath = ofd.FileName;

            if (string.IsNullOrWhiteSpace(_serializedAccountsFilePath))
                return;

            var enterMasterPasswordView = _getEnterMasterPasswordView();

            if (enterMasterPasswordView.ShowDialog() != true)
            {
                _serializedAccountsFilePath = null;
                return;
            }

            if (!(enterMasterPasswordView.DataContext is EnterMasterPasswordViewModel enterMasterPasswordViewModel))
            {
                _serializedAccountsFilePath = null;
                return;
            }

            _masterPassword = enterMasterPasswordViewModel.MasterPassword.Copy();

            try
            {
                var accounts = await _accountsLogic.GetAccounts(_serializedAccountsFilePath, _masterPassword.Copy());
                var accountViewModels = accounts.Select(x => new AccountViewModel().For(x)).ToList();

                Accounts = new ObservableCollection<AccountViewModel>(accountViewModels);
                Title = _serializedAccountsFilePath;
            }
            catch (DecryptException)
            {
                MessageBox.Show("Decrypt file error, wrong password", "Decryption error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Сохраняет файл с аккаунтами на диск.
        /// </summary>
        private async void SaveFile(object param)
        {
            await SaveFileAsync(param);
        }

        /// <summary>
        /// Сохраняет файл с аккаунтами на диск как новый.
        /// </summary>
        private async void SaveFileAs(object param)
        {
            _serializedAccountsFilePath = null;
            _masterPassword?.Dispose();
            _masterPassword = null;
            
            await SaveFileAsync(param);
        }

        /// <summary>
        /// Асинхронно сохраняет файл с аккаунтами.
        /// </summary>
        private async Task SaveFileAsync(object param)
        {
            if (string.IsNullOrWhiteSpace(_serializedAccountsFilePath))
            {
                var sfd = new SaveFileDialog { Filter = "Gzip files (*.gz)|*.gz" };
                
                if (sfd.ShowDialog() != true)
                    return;

                _serializedAccountsFilePath = sfd.FileName;

                if (string.IsNullOrWhiteSpace(_serializedAccountsFilePath))
                    return;
            }
            
            if (_masterPassword == null)
            {
                var createMasterPasswordView = _getCreateMasterPasswordView();

                
                if (createMasterPasswordView.ShowDialog() != true)
                {
                    _serializedAccountsFilePath = null;
                    return;
                }

                if (!(createMasterPasswordView.DataContext is CreateMasterPasswordViewModel createMasterPasswordViewModel))
                {
                    _serializedAccountsFilePath = null;
                    return;
                }

                _masterPassword = createMasterPasswordViewModel.MasterPassword.Copy();
            }

            try
            {
                var accounts = Accounts.Select(x => x.GetModel()).ToList();
                await _accountsLogic.SaveAccounts(accounts, _serializedAccountsFilePath, _masterPassword.Copy());
                Title = _serializedAccountsFilePath;
            }
            catch (EncryptException)
            {
                MessageBox.Show("Encrypt file error, please contact developer", "Encryption error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Создаёт новый аккаунт.
        /// </summary>
        private void CreateAccount(object param)
        {
            var account = new AccountViewModel().For(new Account());
            var editWindow = _getEditAccountWindowView();
            if (!(editWindow.DataContext is EditAccountViewModel editWindowViewModel))
                return;

            editWindowViewModel.EditingAccount = account;

            if (editWindow.ShowDialog() != true)
                return;

            Accounts.Add(editWindowViewModel.EditingAccount);
        }

        /// <summary>
        /// Редактирует выбранный аккаунт.
        /// </summary>
        /// <param name="param">модель представления вида выбранного аккаунта</param>
        private void EditAccount(object param)
        {
            if (!(param is AccountViewModel account))
                return;

            var editWindow = _getEditAccountWindowView();
            if (!(editWindow.DataContext is EditAccountViewModel editWindowViewModel))
                return;

            editWindowViewModel.EditingAccount = account;

            editWindow.ShowDialog();
        }

        /// <summary>
        /// Удаляет выбранный аккаунт.
        /// </summary>
        /// <param name="param">модель представления вида выбранного аккаунта</param>
        private void DeleteAccount(object param)
        {
            if (!(param is AccountViewModel account))
                return;

            if (MessageBox.Show($"Are you sure you want to delete account {account.Login}?", "Delete account", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                return;

            Accounts.Remove(account);
            account.Password.Dispose();
        }

        /// <summary>
        /// Запускает диалог печати открытого файла с аккаунтами.
        /// </summary>
        private void PrintAccounts(object param)
        {
            var flowDocument = _flowDocumentCreator.Create(Accounts.Select(x => x.GetModel()).ToList());
            
            var printDialog = new PrintDialog();
            var paginatorSource = (IDocumentPaginatorSource) flowDocument;

            if (printDialog.ShowDialog() == true)
                printDialog.PrintDocument(paginatorSource.DocumentPaginator, _serializedAccountsFilePath);

            // todo: удалить flowDocument
        }
    }
}