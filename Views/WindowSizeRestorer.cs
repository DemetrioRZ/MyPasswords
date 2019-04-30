using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace Views
{
    /// <summary>
    /// Класс для безопасного восстановления и сохранения размеров окон в файле конфигурации приложения.
    /// </summary>
    public class WindowSizeRestorer
    {
        /// <summary>
        /// Попытка восстановления размеров окна.
        /// </summary>
        /// <param name="window">окно</param>
        public void TryRestore(Window window)
        {
            try
            {
                var windowSizeSettings = Properties.Settings.Default[$"{window.GetType().Name}Size"];

                if (!(windowSizeSettings is string windowSizeSettingsStr) || string.IsNullOrWhiteSpace(windowSizeSettingsStr))
                    return;
                
                var tokens = windowSizeSettingsStr.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Length != 5)
                    return;

                window.Top = double.Parse(tokens[0]);
                window.Left = double.Parse(tokens[1]);
                window.Height = double.Parse(tokens[2]);
                window.Width = double.Parse(tokens[3]);

                if (tokens[4] == WindowState.Maximized.ToString())
                    window.WindowState = WindowState.Maximized;
            }
            catch (ConfigurationErrorsException ex) 
            {
                var messageBuilder = new StringBuilder();
                messageBuilder.Append("MyPasswords has detected that your user settings file has become corrupted. ");
                messageBuilder.Append("This may be due to a crash or improper exiting of the program. ");
                messageBuilder.Append("MyPasswords will try to reset your user settings.");
                
                MessageBox.Show(messageBuilder.ToString(), "Corrupt user settings", MessageBoxButton.OK, MessageBoxImage.Error);
                
                var filename = (ex.InnerException as ConfigurationErrorsException)?.Filename;
                if (string.IsNullOrWhiteSpace(filename))
                {
                    var failedMessage = "Failed to reset your user settings. Please remove setting file manually. Typical user's setting location %appdata%\\local\\MyPasswords";
                    MessageBox.Show(failedMessage, "Corrupt user settings", MessageBoxButton.OK, MessageBoxImage.Error);
                    Process.GetCurrentProcess().Kill();
                    return;
                }
                File.Delete(filename);

                MessageBox.Show("MyPasswords successfully reset your user setting. Application will be restarted", "Settings successfully reset", MessageBoxButton.OK, MessageBoxImage.Information);

                Process.Start(Application.ResourceAssembly.Location);
                Process.GetCurrentProcess().Kill();
            }
        }

        /// <summary>
        /// Попытка сохранить настройки размеров окна.
        /// </summary>
        /// <param name="window">Окно.</param>
        public void TryStore(Window window)
        {
            try
            {
                var settingsBuilder = new StringBuilder();

                if (window.WindowState == WindowState.Maximized)
                {
                    settingsBuilder.Append($"{window.RestoreBounds.Top};");
                    settingsBuilder.Append($"{window.RestoreBounds.Left};");
                    settingsBuilder.Append($"{window.RestoreBounds.Height};");
                    settingsBuilder.Append($"{window.RestoreBounds.Width};");
                    
                }
                else
                {
                    settingsBuilder.Append($"{window.Top};");
                    settingsBuilder.Append($"{window.Left};");
                    settingsBuilder.Append($"{window.Height};");
                    settingsBuilder.Append($"{window.Width};");
                }

                settingsBuilder.Append(window.WindowState.ToString());

                Properties.Settings.Default[$"{window.GetType().Name}Size"] = settingsBuilder.ToString();

                Properties.Settings.Default.Save();
            }
            catch (ConfigurationErrorsException)
            {
                // При перезапуске настройки будут сброшены, т к файл настроек повреждён
                MessageBox.Show("Failed to save window position in user settings", "User settings", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}