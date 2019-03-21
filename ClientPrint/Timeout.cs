using System;

namespace ClientPrint
{
    /// <summary>
    /// Предоставляет статические методы для работы с настройками тайм-аута соединения с Сервером ККМ.
    /// </summary>
    public static class TimeOut
    {
        private const int Min = 0;
        private const int Sec = 30;
        private static TimeSpan _timeOut;

        /// <summary>
        /// Получить настойки тайм-аута
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetTimeOut()
        {
            _timeOut = Properties.Settings.Default.TimeOut;
            if (Check(_timeOut))
                return _timeOut;
            else
            {
                SetDefaultSettings();
                return _timeOut;
            }
        }

        /// <summary>
        /// Сохранить настойки тайм-аута
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static bool SetTimeOut(TimeSpan timeout)
        {
            if (!Check(timeout))
                return false;
            try
            {
                _timeOut = timeout;
                SaveSettings();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool Check(TimeSpan timeOut)
        {
            if (timeOut.Minutes < 0 || (timeOut.Seconds < 0))
                return false;
            else if (timeOut.Minutes == 0 && timeOut.Seconds < Sec)
                return false;
            else
                return true;
        }

        private static void SaveSettings()
        {
            Properties.Settings.Default.TimeOut = _timeOut;
            Properties.Settings.Default.Save();
        }

        private static void SetDefaultSettings()
        {
            _timeOut = new TimeSpan(0, Min, Sec);
        }
    }
}