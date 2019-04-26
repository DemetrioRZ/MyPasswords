using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Interfaces.Logic;
using Microsoft.Win32;
using Model;
using Views.Common;

namespace Views
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Логика работы с аккаунтами
        /// </summary>
        private readonly IAccountsLogic _accountsLogic;

        /// <summary>
        /// Путь к сериализованному файлу с аккаунтами.
        /// </summary>
        private string _serializedAccountsFilePath;

        /// <summary>
        /// Коллекция отображаемых аккаунтов
        /// </summary>
        private ObservableCollection<Account> _accounts;

        /// <summary>
        /// Выбранная модель аккаунта.
        /// </summary>
        private Account _selectedAccount;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="accountsLogic">Логика работы с аккаунтами</param>
        public MainWindowViewModel(IAccountsLogic accountsLogic)
        {
            _accountsLogic = accountsLogic;

            InitializeCommands();
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
        /// Команда создания нового аккаунта для хранения.
        /// </summary>
        public ICommand CreateAccountCommand { get; private set; }

        /// <summary>
        /// Команда редактирования выбранного аккаунта.
        /// </summary>
        public ICommand EditAccountCommand { get; private set; }

        /// <summary>
        /// Коллекция отображаемых аккаунтов
        /// </summary>
        public ObservableCollection<Account> Accounts
        {
            get => _accounts;
            private set
            {
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }

        /// <summary>
        /// Выбранный аккаунт.
        /// </summary>
        public Account SelectedAccount
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
            SaveFileCommand = new Command(SaveFileAsync, x => Accounts != null);
            CreateAccountCommand = new Command(CreateAccount, x => Accounts != null);
            EditAccountCommand = new Command(EditAccount, x => Accounts != null && Accounts.Any() && SelectedAccount != null);
        }

        /// <summary>
        /// Создаёт новый для хранения аккаунтов.
        /// </summary>
        private void NewFile(object param)
        {
            Accounts = new ObservableCollection<Account>(new List<Account>());
        }

        /// <summary>
        /// Команда открытия файла с аккаунтами.
        /// </summary>
        private async void OpenFileAsync(object param)
        {
            var ofd = new OpenFileDialog {Multiselect = false, Filter = "All files (*.*)|*.*"};
            
            if (ofd.ShowDialog() != true)
                return;

            _serializedAccountsFilePath = ofd.FileName;

            if (string.IsNullOrWhiteSpace(_serializedAccountsFilePath))
                return;

            var accounts = await _accountsLogic.GetAccounts(_serializedAccountsFilePath);

            Accounts = new ObservableCollection<Account>(accounts);
        }

        /// <summary>
        /// Сохраняет файл с аккаунтами на диск.
        /// </summary>
        private async void SaveFileAsync(object param)
        {
            var sfd = new SaveFileDialog { Filter = "All files (*.*)|*.*" };
            if (!string.IsNullOrWhiteSpace(_serializedAccountsFilePath))
                sfd.FileName = _serializedAccountsFilePath;

            if (sfd.ShowDialog() != true)
                return;

            _serializedAccountsFilePath = sfd.FileName;

            if (string.IsNullOrWhiteSpace(_serializedAccountsFilePath))
                return;

            await _accountsLogic.SaveAccounts(Accounts, _serializedAccountsFilePath);
        }

        /// <summary>
        /// Создаёт новый аккаунт.
        /// </summary>
        private void CreateAccount(object param)
        {
            Accounts.Add(new Account { Id = Guid.NewGuid(), Login = "Login", Password = "password", WebSite = "website", Comment = "comment"});
        }

        /// <summary>
        /// Редактирует выбранный аккаунт.
        /// </summary>
        private void EditAccount(object param)
        {
            if (!(param is Account account))
                return;

            // todo: открыть в окне на редактирование

        }
    }
}