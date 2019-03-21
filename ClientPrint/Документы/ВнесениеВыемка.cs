using ClientPrint.PrintServiceRef;
using System;
using System.Runtime.InteropServices;

namespace ClientPrint.Документы
{
    /// <summary>
    /// Представляет задание печати Внесения/Выемки
    /// </summary>
    /// <example>
    /// Пример кода
    /// <code language="1С" title="1C">
    /// ОбъектДрайвера= ПолучитьОбьектДрайвера(компонентаIFr2001, Адрес, Порт, ИмяУстройства);
    /// внесениеВыемка = ОбъектДрайвера.СоздатьВнесениеВыемка();
    /// // Касса по умолчанию используется заданная при инициализации объекта драйвера
    /// // Чтобы другую кассу отличную от
    /// // внесениеВыемка.ИмяКассы = "Atol";
    ///
    /// // Выемка = 0, Внесение = 1,
    /// внесениеВыемка.ВидОперации   = 1;
    /// внесениеВыемка.Сумма = 50;
    ///
    /// Результат = ОбъектДрайвера.РаспечататьВнесениеВыемка(внесениеВыемка);
    /// Если Результат.КодОшибки&lt;&gt;0 ТОгда
    ///      Сообщить("Ошибка "+Результат.КодОшибки+" Описание "+Результат.ОписаниеОшибки);
    ///      Возврат
    /// КонецЕсли;
    ///
    /// Сообщить("Успешно");
    /// </code>
    /// </example>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class ВнесениеВыемка : DockBase
    {
        /// <summary>
        /// Предоставляет доступ к базовому документу
        /// </summary>
        public Cash Cash
        {
            get { return Doc as Cash; }
            set { Doc = value; }
        }

#pragma warning disable 1591

        public ВнесениеВыемка()
#pragma warning restore 1591
        {
            Doc = new Cash
            {
                DockId = Guid.NewGuid(),
                DocType = DocTypes.ВнесениеВыемка,
                CashType = CashType.Внесение
            };
        }

        /// <summary>
        /// Задает вид операции
        /// </summary>
        /// <remarks>Выемка = 0, Внесение = 1,</remarks>
        public CashType ВидОперации
        {
            get
            {
                return Cash.CashType;
            }
            set
            {
                Cash.CashType = value;
            }
        }

        /// <summary>
        /// Сумма операции
        /// </summary>
        public double Сумма
        {
            get { return Cash.Summ; }
            set { Cash.Summ = value; }
        }

        /// <summary>
        /// Пароль кассира
        /// </summary>
        /// <remarks>Не обязательное свойство</remarks>
        public string ПарольКассира
        {
            get
            {
                return Cash.Password;
            }
            set
            {
                Cash.Password = value;
            }
        }
    }
}