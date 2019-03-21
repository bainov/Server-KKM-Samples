using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace ClientPrint
{
    internal class IniFile
    {
        /// <summary>
        /// Имя файла.
        /// </summary>
        private string PathFile;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        /// <summary>
        /// С помощью конструктора записываем пусть до файла и его имя.
        /// </summary>
        public IniFile(string IniPath)
        {
            if (!Directory.Exists(IniPath))
                Directory.CreateDirectory(IniPath);
            PathFile = Path.Combine(IniPath, "config.ini");
        }

        /// <summary>
        /// Ипользует файл config.ini
        /// </summary>
        public IniFile()
        {
            var currentDir = Assembly.GetExecutingAssembly().Location;

            PathFile = Path.Combine(Path.GetDirectoryName(currentDir), "config.ini");
        }

        /// <summary>
        /// Читает ini-файл и возвращаем значение указного ключа из заданной секции.
        /// При отсутсвии ключа записывает значение по умолчаниюи возвращает значение по умолчанию
        /// </summary>
        public string ReadINI(string section, string key, string defultValue = null)
        {
            if (KeyExists(key, section))
            {
                var RetVal = new StringBuilder(255);
                GetPrivateProfileString(section, key, defultValue, RetVal, 255, PathFile);
                return RetVal.ToString();
            }
            else
            {
                if (defultValue != null)
                {
                    Write(section, key, defultValue);
                }
                return defultValue;
            }
        }

        /// <summary>
        /// Записывает в выбранную секцию в выбранный ключ.
        /// </summary>
        public void Write(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, PathFile);
        }

        /// <summary>
        /// Удаляет ключ из выбранной секции.
        /// </summary>
        public void DeleteKey(string Key, string Section = null)
        {
            Write(Section, Key, null);
        }

        /// <summary>
        /// Удаляет выбранную секцию
        /// </summary>
        /// <param name="Section"></param>
        public void DeleteSection(string Section = null)
        {
            Write(Section, null, null);
        }

        /// <summary>
        /// Проверяет, есть ли такой ключ, в этой секции
        /// </summary>
        /// <param name="key"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public bool KeyExists(string key, string section)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", RetVal, 255, PathFile);
            return RetVal.ToString().Length > 0;
        }
    }
}