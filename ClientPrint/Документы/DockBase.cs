using ClientPrint.PrintServiceRef;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ClientPrint.Документы
{
    /// <summary>
    /// Представляет общие всех заданий печати свойства
    /// </summary>
    [ClassInterface(ClassInterfaceType.None)]
    public abstract class DockBase
    {
        
        /// <summary>
        /// Перемена базового класса для всех документов
        /// </summary>
        protected CheckBase Doc;

        /// <summary>
        /// Буфер для введения в generic
        /// </summary>
        protected List<PrintLine> After = new List<PrintLine>();

        /// <summary>
        /// Буфер для введения в generic
        /// </summary>
        protected List<PrintLine> Before = new List<PrintLine>();

        /// <summary>
        /// Задает или получает имя компьютера на котором сформирован документ
        /// </summary>
        /// <remarks>
        /// При работа идет в терминальном сеансе это отражено в имени
        /// </remarks>>
        public string Терминал
        {
            get { return Doc.TerminalId; }
            set { Doc.TerminalId = value; }
        }

        /// <summary>
        /// Задает или получает идентификатор документа в формате GUID
        /// </summary>
        /// <remarks>Инициализируется при создании чека </remarks>>
        public string Идентификатор
        {
            get { return Doc.DockId.ToString(); }
            set { Doc.DockId = Guid.Parse(value); }
        }

        /// <summary>
        /// Задает или получает имя устройства как на Сервере ККМ
        /// </summary>
        /// <remarks>Если не заданно используется касса указанная в настройках компоненты</remarks>>
        public string ИмяКассы { get; set; }

        #region Результаты печати

        /// <summary>
        /// Получает номер фискального документа после печати
        /// </summary>
        public int НомерЧека => Doc.DocNumber;

        /// <summary>
        /// Получает номер смены после печати
        /// </summary>
        public int НомерСмены => Doc.SessionNumber;

        /// <summary>
        /// Получает код ошибки печати
        /// </summary>
        public int КодОшибки => Doc.ResultCode;

        /// <summary>
        /// Получает описание ошибки печати
        /// </summary>
        public string ОписаниеОшибки => Doc.ResultDescription;

        /// <summary>
        /// Получает номер смены после печати
        /// </summary>
        [Obsolete("Используйте " + nameof(НомерСмены))]
        public int Сессия => Doc.SessionNumber;

        /// <summary>
        /// Получает номер чека после печати
        /// </summary>
        [Obsolete("Используйте " + nameof(НомерЧека))]
        public int Номер => Doc.DocNumber;

        /// <summary>
        /// Получает фискальный признак после печати
        /// </summary>

        public string ФискальныйПризнак
        {
            get
            {
                return Doc.FiscalSign;
            }
            set
            {
                Doc.FiscalSign = value;
            }
        }

        #endregion Результаты печати

        /// <summary>
        /// Задает или получает GUID идентификатор кассы на которой будет распечатано задание
        /// </summary>
        [Obsolete]
        public string УстройствоПечати
        {
            get { return Doc.DeviceId.ToString(); }
            set { Doc.DeviceId = Guid.Parse(value); }
        }

        /// <summary>
        /// Задает или получает имя кассы на которой будет распечатано задание
        /// </summary>
        public string ИмяУстройства { get; set; }

        /// <summary>
        /// ФИО кассира
        /// </summary>
        public string Кассир
        {
            get { return Doc.Cashier; }
            set { Doc.Cashier = value; }
        }

        /// <summary>
        /// ИНН кассира
        /// </summary>
        public string ИннКассира
        {
            get { return Doc.CashierVATIN; }
            set { Doc.CashierVATIN = value; }
        }

        /// <summary>
        /// Версия клиента
        /// </summary>
        public string ВерсияКлиента
        {
            get { return Doc.Ver; }
            set { Doc.Ver = value; }
        }

        /// <summary>
        /// Добавляет данные о количестве оставшихся вызовов
        /// </summary>
        /// <param name="docCount">Количество оставшихся документов</param>
        public void ДобавитьСтрокуДемоЛицензии(int docCount)
        {
            var line1 = new PrintLine
            {
                Alignment = Alignment.AlignmentCenter,
                Font = PrintFont.Normal,
                Line = "ВНИМАНИЕ!"
            };
            var line2 = new PrintLine
            {
                Alignment = Alignment.AlignmentCenter,
                Font = PrintFont.Normal,
                Line = "ДЕМОНСТРАЦИОННЫЙ РЕЖИМ"
            };
            var line3 = new PrintLine
            {
                Alignment = Alignment.AlignmentCenter,
                Font = PrintFont.Normal,
                Line = "Вызовов печати осталось: " + docCount
            };
            var line4 = new PrintLine
            {
                Alignment = Alignment.AlignmentCenter,
                Font = PrintFont.Normal,
                Line = "          "
            };
            Before.Add(line1);
            Before.Add(line2);
            Before.Add(line3);
            Before.Add(line4);
            Doc.Before = Before.ToArray();
        }

        internal void УстановитьОшибку(int код, string описание)
        {
            Doc.ResultCode = код;
            Doc.ResultDescription = описание;
        }
    }
}