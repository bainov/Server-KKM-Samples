using System;
using System.IO;
using System.Text;

namespace ClientPrint
{
    internal class Settings
    {
        public Settings(string IniPath)
        {
            if (string.IsNullOrWhiteSpace(IniPath))
                throw new ArgumentException("Путь до файла настроек не может быть пустым");
            _path = Path.Combine(IniPath, settingFile);
            UpdateWriteLog();
            StartChangeFileWatcher();
        }

        private void UpdateWriteLog()
        {
            _writeLog = GetValue();
        }

        private bool GetValue()
        {
            if (!File.Exists(_path)) return false;

            try
            {
                if (bool.TryParse(File.ReadAllText(_path), out bool val)) return val;
            }
            catch (IOException)
            {
                return false; // <- File failed to open
            }

            return false;
        }

        /// <summary>
        /// Начать отслеживание изменение в файле
        /// </summary>
        private void StartChangeFileWatcher()
        {
            // Вызывает иногда исключение
            try
            {
                watcher = new FileSystemWatcher
                {
                    Path = Path.GetDirectoryName(_path),
                    NotifyFilter = NotifyFilters.LastWrite,
                    Filter = Path.GetFileName(_path)
                };
                watcher.Changed += OnChanged;
                watcher.EnableRaisingEvents = true;
            }
            catch
            {
                //ignore
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            UpdateWriteLog();
        }

        private const string settingFile = "config.txt";

        private string _path;
        private FileSystemWatcher watcher;

        private bool _writeLog;

        /// <summary>
        /// Записывать лог
        /// </summary>
        public bool WriteLog
        {
            get
            {
                return _writeLog;
            }
            set
            {
                try
                {
                    File.WriteAllText(_path, value.ToString());
                }
                catch (IOException)
                {
                    // ignore
                }
            }
        }
    }
}