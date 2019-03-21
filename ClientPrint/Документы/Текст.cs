using ClientPrint.PrintServiceRef;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ClientPrint.Документы
{
    /// <summary>
    /// Представляет задание печати Текста
    /// </summary>
    /// <remarks>Не фискальный документ</remarks>
    /// /// <example>
    /// Пример кода
    /// <code language="1С" title="1C">
    /// ОбъектДрайвера= ПолучитьОбьектДрайвера(компонентаIFr2001, Адрес, Порт, ИмяУстройства);
    /// текст = ОбъектДрайвера.СоздатьТекст();
    /// // Касса по умолчанию используется заданная при инициализации объекта драйвера
    /// // Чтобы другую кассу отличную от
    /// // текст.ИмяКассы = "Atol";
    ///
    /// текст.ДобавитьСтроку("В наше время осуществилось волшебство мифа и легенды. С клавиатуры вводится верное заклинание, и экран монитора оживает, показывая то, чего никогда не было и не могло быть.");
    /// текст.ДобавитьСтроку("Фредерик Брукс");
    ///
    /// Результат = ОбъектДрайвера.РаспечататьТекст(текст);
    /// Если Результат.КодОшибки&lt;&gt;0 ТОгда
    ///      Сообщить("Ошибка "+Результат.КодОшибки+" Описание "+Результат.ОписаниеОшибки);
    ///      Возврат;
    /// КонецЕсли ;
    ///
    /// Сообщить("Успешно");
    /// </code>
    /// </example>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ТекстовыйФайл : DockBase
    {
        /// <summary>
        /// Предоставляет доступ к базовому документу
        /// </summary>
        public TextFile TextFile
        {
            get { return Doc as TextFile; }
            set { Doc = value; }
        }

#pragma warning disable 1591

        public ТекстовыйФайл()
#pragma warning restore 1591
        {
            Doc = new TextFile
            {
                DockId = Guid.NewGuid(),
                DocType = DocTypes.Текст,
                DeviceId = Guid.Empty
            };
            _lines = new List<PrintLine>();
        }

        private List<PrintLine> _lines;

        /// <summary>
        /// Добавляет строку текста в задание
        /// </summary>
        /// <param name="Текст">Строка текста</param>
        public void ДобавитьСтроку(string Текст)
        {
            var line = new PrintLine
            {
                Alignment = Alignment.AlignmentLeft,
                Font = PrintFont.Normal,
                Line = Текст,
                Wrap = true
            };
            //TODO Вызывает сомнение эта строчка кода, может ее удалить Очир Дармаев 110119
            //Before.Add(line);

            Add(line);
        }

        private void Add(PrintLine line)
        {
            _lines.Add(line);
            TextFile.Lines = _lines.ToArray();
        }

        internal void ДобавитьШтрихКод(string barCodeType, string barCode)
        {
            if (string.IsNullOrWhiteSpace(barCodeType)) return;
            if (string.IsNullOrWhiteSpace(barCode)) return;

            var line = new PrintLine
            {
                Barcode = new Barcode()
                {
                    BarcodeText = barCode,
                    BarcodeType = barCodeType
                }
            };

            Add(line);
        }
    }
}