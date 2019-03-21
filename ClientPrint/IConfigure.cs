using System;

namespace ClientPrint
{
    /// <summary>
    /// Описывает настройки
    /// </summary>
    public interface IConfigure
    {
        /// <summary>
        /// Задает сетевой адрес Сервера ККМ
        /// </summary>
        string Адрес { get; set; }

        /// <summary>
        /// Задает имя устройства на Сервере ККМ
        /// </summary>
        string ИдентификаторУстройства { get; set; }

        /// <summary>
        /// Кассир
        /// </summary>
        /// <remarks>Если кассир в чек не задан будет использоваться этот параметр</remarks>
        [Obsolete]
        string Кассир { get; set; }

        /// <summary>
        /// Задает номер порта используемый для подключения к Серверу ККМ
        /// </summary>
        int Порт { get; set; }

        /// <summary>
        /// Получает значение, указывающее, что подключен к Серверу ККМ
        /// </summary>
        bool СтатусПодключения { get; set; }
    }
}