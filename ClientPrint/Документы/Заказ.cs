using ClientPrint.PrintServiceRef;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ClientPrint.Документы
{
    /// <summary>
    /// Представляет задание печати Заказ
    /// </summary>
    /// <remarks>Не фискальный документ</remarks>
    /// <example>
    /// Пример кода
    /// <code language="1С" title="1C">
    /// ОбъектДрайвера= ПолучитьОбьектДрайвера(компонентаIFr2001, Адрес, Порт, ИмяУстройства);
    /// заказ = ОбъектДрайвера.СоздатьЗаказ();
    /// // Касса по умолчанию используется заданная при инициализации объекта драйвера
    /// // Чтобы другую кассу отличную от
    /// // текст.ИмяКассы = "Atol";
    /// заказ.НомерЗаказа = 12;
    ///
    /// заказ.ДобавитьПозицию(
    /// "Мясо Бурятия",
    /// 240.50,
    /// 4,
    /// 10,
    /// 1);
    ///
    /// Результат = ОбъектДрайвера.РаспечататьЗаказ(заказ);
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
    public class Заказ : DockBase
    {
        private readonly List<CheckItem> _items = new List<CheckItem>();

        /// <summary>
        /// Предоставляет доступ к базовому документу
        /// </summary>
        public Order Order
        {
            get { return Doc as Order; }
            set { Doc = value; }
        }

#pragma warning disable 1591

        public Заказ()
#pragma warning restore 1591
        {
            Doc = new Order
            {
                DockId = Guid.NewGuid(),
                DeviceId = Guid.Empty,
                DocType = DocTypes.ЗаказПоТовару
            };
        }

        /// <summary>
        /// Задает номер заказа
        /// </summary>
        public int НомерЗаказа
        {
            get { return Order.Number; }
            set { Order.Number = value; }
        }

        /// <summary>
        /// Добавляет позицию в заказ
        /// </summary>
        /// <param name="Наименование">Наименование</param>
        /// <param name="Цена">Цена</param>
        /// <param name="Количество">Количество</param>
        /// <param name="Скидка">Скидка</param>
        /// <param name="Отдел">Отдел</param>
        public void ДобавитьПозицию(string Наименование, double Цена, double Количество, double Скидка, int Отдел)
        {
            var item = new CheckItem
            {
                Name = Наименование,
                Price = (decimal)Цена,
                Quantity = (decimal)Количество,
                DiscountValue = (decimal)Скидка,
                Department = Отдел
            };
            _items.Add(item);
            Order.Items = _items.ToArray();
        }
    }
}