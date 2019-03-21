using ClientPrint.PrintServiceRef;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

#pragma warning disable CS0108

namespace ClientPrint.Документы
{
    /// <summary>
    /// Представляет задание печати Чека
    /// </summary>
    /// <example>
    /// Пример кода
    /// <code language="1С" title="1C">
    /// ОбъектДрайвера= ПолучитьОбьектДрайвера(компонентаIFr2001,Адрес,Порт,ИмяУстройства);
    /// Чек = ОбъектДрайвера.СоздатьЧек();
    /// // Касса по умолчанию используется заданная при инициализации объекта драйвера
    /// // Чтобы другую кассу отличную от
    /// // Чек.ИмяКассы = "Atol";
    ///
    /// Чек.Кассир = Кассир;
    /// Чек.ИннКассира = ИннКасира;
    /// Фискальный = Истина;
    /// Чек.Фискальный = Фискальный;
    /// // 1 - приход,
    /// // 2 - возврат прихода,
    /// // 4 - расход,
    /// // 5 - возврат расхода
    /// Чек.ТипЧека = 1;
    ///
    /// // 0 - Общая
    /// // 1 - Упрощенная Доход
    /// // 2 - Упрощенная Доход минус Расход
    /// // 3 - Единый налог на вмененный доход
    /// // 4 - Единый сельскохозяйственный налог
    /// // 5 - Патентная система налогообложения
    /// // 999 - использовать настройки устройства
    /// Чек.ТипНалогообложения = 0;
    ///
    /// Чек.Электронный = Ложь;
    /// // Задает телефон или email покупателя
    /// Чек.Контакт = "";
    /// // Задает номер отдела
    /// Чек.Департамент =1;
    ///
    /// // Если не задать этот параметр, будет использоваться пароль указанный в настройках кассы на Сервере ККМ
    /// // Чек.ПарольКассира = "30";
    ///
    /// // ДобавитьПозицию105(
    /// Чек.ДобавитьПозицию105(
    ///     "Спички", // string Наименование,
    ///     0.60 ,    // double Цена,
    ///     2,        // double Количество,
    ///     0.20,     // double Скидка,
    ///     1,        // int Отдел,
    ///     10,       // double НДС,
    ///     2,        // int ПризнакСпособаРасчета,
    ///     1);       // int ПризнакПредметаРасчета
    ///
    /// //НДС:
    /// // -1  - БезНДС
    /// // 0   - 0% НДС
    /// // 10  - 10% НДС
    /// // 18  - 18% НДС
    /// // 20  - 20% НДС
    /// // 110 - 110% НДС
    /// // 118 - 118% НДС
    /// // 120 - 120% НДС
    ///
    /// //ПризнакСпособаРасчета:
    /// // 0 - Не применяется
    /// // 1 - Предоплата полная
    /// // 2 - Предоплата частичная
    /// // 3 - Аванс
    /// // 4 - Передача с полной оплатой
    /// // 5 - Передач с частичной оплатой
    /// // 6 - Передача без оплаты
    /// // 7 - Оплата кредита
    ///
    /// //ПризнакПредметаРасчета:
    /// // 0  - Не применяется
    /// // 1  - Товар
    /// // 2  - Подакцизный товар
    /// // 3  - Работа
    /// // 3  - Услуга
    /// // 4  - Прием ставок
    /// // 5  - Выплата выигрышей в азартных играх
    /// // 6  - Реализация лотерейных билетов
    /// // 7  - Выплата выигрышей в лотереях
    /// // 8  - Права на использование интеллектуальной деятельности
    /// // 9  - Аванс задаток предоплата кредит
    /// // 10 - Предмет расчета
    /// // 11 - Предмет расчета не относящийся к предметам расчета
    ///
    /// Чек.ДобавитьОплатуНаличными(0.10);
    /// Чек.ДобавитьОплатуКартой(0.90);
    /// // Чек.ДобавитьОплатуКредит(0);
    /// // Чек.ДобавитьОплатуПредоплатой(0);
    /// // Чек.ДобавитьОплатуПредставлением(0);
    ///
    /// Результат = ОбъектДрайвера.РаспечататьЧек(Чек);
    /// Если Результат.КодОшибки&lt;&gt;0 ТОгда
    ///      Сообщить("Ошибка "+Результат.КодОшибки+" Описание "+Результат.ОписаниеОшибки);
    ///      Возврат
    /// КонецЕсли;
    ///
    ///
    /// Сообщить("Успех");
    ///
    /// Если Фискальный Тогда
    ///     Сообщить("Чек номер = "+	Результат.НомерЧека);
    ///     Сообщить("Чек смены = "+	Результат.НомерСмены);
    ///     Сообщить("Фискальный признак = "+	Результат.ФискальныйПризнак );
    /// КонецЕсли;
    /// </code>
    /// </example>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ParameterHidesMember")]
    // ReSharper disable once InheritdocConsiderUsage

    public class Чек : DockBase
    {
        private readonly List<CheckItem> _items;

        /// <summary>
        /// Предоставляет доступ к базовому документу
        /// </summary>
        public Check Check
        {
            get { return Doc as Check; }
            set { Doc = value; }
        }

#pragma warning disable 1591

        public Чек()
#pragma warning restore 1591
        {
            _items = new List<CheckItem>();

            Doc = new Check
            {
                DockId = Guid.NewGuid(),
                Bonus = 0,
                BonusLeft = 0,
                CheckNumber = 0,
                CheckType = CheckTypes.Продажа,
                DeviceId = Guid.Empty,
                Summ = 0,
                Paid = 0,
                InjectSumm = 0,
                IsFiscal = false,
                TaxType = TaxVariants.None,
                // CheckMode - 0 электронный
                // CheckMode - 1 не электронный
                CheckMode = 1
            };

            //Doc=Check;
        }

        /// <summary>
        /// Задает тип чека
        /// </summary>
        /// <remarks>
        /// 1 - приход,
        /// 2 - возврат прихода,
        /// 4 - расход,
        /// 5 - возврат расхода
        ///</remarks>
        public CheckTypes ТипЧека
        {
            get { return Check.CheckType; }
            set { Check.CheckType = value; }
        }

        /// <summary>
        /// Добавляет строку текста перед фискальными позициями чека в 0 позицию
        /// </summary>
        /// <param name="Текст">Строка текста</param>
        public void ДобавитьВНачало(string Текст)
        {
            var line = new PrintLine
            {
                Alignment = Alignment.AlignmentLeft,
                Font = PrintFont.Normal,
                Line = Текст
            };
            Before.Insert(0, line);
            Check.Before = Before.ToArray();
        }

        /// <summary>
        /// Добавляет строку текста перед фискальными позициями чека
        /// </summary>
        /// <param name="Текст">Строка текста</param>
        public void ДобавитьПередЧеком(string Текст)
        {
            var line = new PrintLine
            {
                Alignment = Alignment.AlignmentLeft,
                Font = PrintFont.Normal,
                Line = Текст
            };
            Before.Add(line);
            Check.Before = Before.ToArray();
        }

        /// <summary>
        /// Добавляет строку текста после фискальных позиций чека
        /// </summary>
        /// <param name="Текст">Строка текста</param>
        public void ДобавитьПослеЧека(string Текст)
        {
            var line = new PrintLine
            {
                Alignment = Alignment.AlignmentLeft,
                Font = PrintFont.Normal,
                Line = Текст
            };
            After.Add(line);
            Check.After = After.ToArray();
        }

        /// <summary>
        /// Добавляет фискальную позицию в чек
        /// </summary>
        /// <param name="Наименование">Наименование позиции</param>
        /// <param name="Цена">Цена</param>
        /// <param name="Количество">Количество</param>
        /// <param name="Скидка">Скидка в деньгах</param>
        /// <param name="Отдел">Отдел (Департамент)</param>
        /// <param name="НДС">НДС</param>
        public void ДобавитьПозицию(string Наименование, double Цена, double Количество, double Скидка, int Отдел, double НДС)
        {
            ДобавитьПозицию105(Наименование, Цена, Количество, Скидка, Отдел, НДС, (int)ItemTypes.НеПрименяется, (int)PaymentModes.НеПрименяется);
        }

        /// <summary>
        /// Добавляет фискальную позицию в чек ФФД 1.0.5
        /// </summary>
        /// <param name="Наименование">Наименование позиции</param>
        /// <param name="Цена">Цена</param>
        /// <param name="Количество">Количество</param>
        /// <param name="Скидка">Скидка в деньгах</param>
        /// <param name="Отдел">Отдел (Департамент)</param>
        /// <param name="НДС">НДС</param>
        /// <param name="ПризнакСпособаРасчета">Признак способа расчета</param>
        /// <param name="ПризнакПредметаРасчета">Признак предмета расчета</param>
        /// <remarks>
        ///<para>
        /// НДС
        /// -1 -  Без НДС
        /// 0 - 0 % НДС
        /// 10 - 10 % НДС
        /// 18 - 18 % НДС
        /// 20 - 20 % НДС
        /// 110 - 110 % НДС
        /// 118 - 118 % НДС
        /// 120 - 120 % НДС
        /// </para>
        /// <para>
        /// Признак предмета расчета
        /// НеПрименяется = 0,
        /// ПредоплатаПолная = 1,
        /// ПредоплатаЧастичная = 2,
        /// Аванс = 3,
        /// ПередачаСПолнойОплатой = 4,
        /// ПередачСЧастичнойОплатой = 5,
        /// ПередачабезОплаты = 6,
        /// ОплатаКредита = 7,
        /// НеПрименяется = 0
        /// </para>
        /// <para>
        /// Признак способа расчета
        /// Возможные значения
        /// Товар = 1,
        /// ПодакцизныйТовар = 2,
        /// Работа = 3,
        /// Услуга = 4,
        /// ПриемСтавок = 5,
        /// ВыплатаВыигрышейВАзартныхИграх = 6,
        /// РеализацияЛотерейныхБилетов = 7,
        /// ВыплатаВыигрышейВЛотереях = 8,
        /// ПравНаИспользованиеИнтелектуальнойДеятельности = 9,
        /// АвансЗадатокПредоплатаКредит = 10,
        /// ПредметРасчета = 11,
        /// ПредметРасчетаНеОтносящийсяКПредметамРасчета = 12,
        /// НеПрименяется13 = 13
        /// </para>>
        /// </remarks>>
        public void ДобавитьПозицию105(string Наименование, double Цена, double Количество, double Скидка, int Отдел, double НДС, int ПризнакСпособаРасчета, int ПризнакПредметаРасчета)
        {
            var item = new CheckItem
            {
                isFiscal = true,
                After = new PrintLine(),
                Before = new PrintLine(),
                Quantity = (decimal)Количество,
                Name = Наименование,
                Price = (decimal)Цена,
                Department = Отдел,
                DiscountValue = 0,
                DiscountInfoValue = (decimal)Скидка,
                ItemType = (ItemTypes)ПризнакПредметаРасчета,
                PaymentMode = (PaymentModes)ПризнакСпособаРасчета
            };
            decimal sum = Math.Round(item.Quantity * item.Price, 2, MidpointRounding.AwayFromZero);
            item.Summ = sum - (decimal)Скидка;
            // Если скидка полностью покрывает сумму оплаты
            if (sum == Math.Round((decimal)Скидка, 2, MidpointRounding.AwayFromZero))
            {
                // обнуляем цену позиции,
                // чтобы пройти валидацию на сервере в FRbase.CheckData()
                item.Price = 0;
            }
            ConvertNDS(НДС, item);
            _items.Add(item);
            Check.CheckItems = _items.ToArray();
        }

        private static void ConvertNDS(double НДС, CheckItem item)
        {
            item.TaxValue = Math.Abs(НДС - 0.10) < 0.001 ? 10 : Math.Abs(НДС - 0.18) < 0.001 ? 18 : Math.Abs(НДС - 0.20) < 0.001 ? 20 : Convert.ToInt32(НДС);
        }

        /// <summary>
        /// Добавляет фискальную позицию в чек ФФД 1.0.5 с учетом скидки
        /// </summary>
        /// <param name="Наименование">Наименование позиции</param>
        /// <param name="ЦенаБезСкидки">Цена без скидки</param>
        /// <param name="Количество">Количество</param>
        /// <param name="СуммаСоСкидкой">Сумма со скидкой</param>
        /// <param name="Отдел">Отдел (Департамент)</param>
        /// <param name="НДС">НДС</param>
        /// <param name="ПризнакСпособаРасчета">Признак способа расчета</param>
        /// <param name="ПризнакПредметаРасчета">Признак предмета расчета</param>
        /// <remarks>
        /// Когда скидка уже учетна в сумме, не возникает ошибкок связаных с копейками при расчете скидок.
        ///<para>
        /// НДС
        /// -1 -  Без НДС
        /// 0 - 0 % НДС
        /// 10 - 10 % НДС
        /// 18 - 18 % НДС
        /// 20 - 20 % НДС
        /// 110 - 110 % НДС
        /// 118 - 118 % НДС
        /// 120 - 120 % НДС
        /// </para>
        /// <para>
        /// Признак предмета расчета
        /// НеПрименяется = 0,
        /// ПредоплатаПолная = 1,
        /// ПредоплатаЧастичная = 2,
        /// Аванс = 3,
        /// ПередачаСПолнойОплатой = 4,
        /// ПередачСЧастичнойОплатой = 5,
        /// ПередачабезОплаты = 6,
        /// ОплатаКредита = 7,
        /// НеПрименяется = 0
        /// </para>
        /// <para>
        /// Признак способа расчета
        /// Возможные значения
        /// Товар = 1,
        /// ПодакцизныйТовар = 2,
        /// Работа = 3,
        /// Услуга = 4,
        /// ПриемСтавок = 5,
        /// ВыплатаВыигрышейВАзартныхИграх = 6,
        /// РеализацияЛотерейныхБилетов = 7,
        /// ВыплатаВыигрышейВЛотереях = 8,
        /// ПравНаИспользованиеИнтелектуальнойДеятельности = 9,
        /// АвансЗадатокПредоплатаКредит = 10,
        /// ПредметРасчета = 11,
        /// ПредметРасчетаНеОтносящийсяКПредметамРасчета = 12,
        /// НеПрименяется13 = 13
        /// </para>>
        /// </remarks>>
        public void ДобавитьПозициюСУчетомСкидки(string Наименование, double ЦенаБезСкидки, double Количество, double СуммаСоСкидкой, int Отдел, double НДС, int ПризнакСпособаРасчета, int ПризнакПредметаРасчета)
        {
            decimal скидка = (decimal)(Math.Round(ЦенаБезСкидки * Количество, 2, MidpointRounding.AwayFromZero) - СуммаСоСкидкой);
            var item = new CheckItem
            {
                isFiscal = true,
                After = new PrintLine(),
                Before = new PrintLine(),
                Quantity = (decimal)Количество,
                Name = Наименование,
                Price = (decimal)ЦенаБезСкидки,
                Department = Отдел,
                DiscountValue = 0,
                DiscountInfoValue = скидка,
                ItemType = (ItemTypes)ПризнакПредметаРасчета,
                PaymentMode = (PaymentModes)ПризнакСпособаРасчета
            };
            item.Summ = (decimal)СуммаСоСкидкой;

            // Если скидка полностью покрывает сумму оплаты
            if (item.Summ == Math.Round(скидка, 2, MidpointRounding.AwayFromZero))
            {
                // обнуляем цену позиции,
                // чтобы пройти валидацию на сервере в FRbase.CheckData()
                item.Price = 0;
            }

            ConvertNDS(НДС, item);

            _items.Add(item);
            Check.CheckItems = _items.ToArray();
        }

        internal bool ДобавитьШтрихКод(string barCodeType, string barCode)
        {
            if (string.IsNullOrWhiteSpace(barCodeType)) return false;
            if (string.IsNullOrWhiteSpace(barCode)) return false;

            var item = new CheckItem
            {
                isFiscal = false,
                Barcode = new Barcode()
                {
                    BarcodeText = barCode,
                    BarcodeType = barCodeType
                }
            };
            _items.Add(item);
            Check.CheckItems = _items.ToArray();
            return true;
        }

        /// <summary>
        /// Добавляет не фискальную позицию в чек как текст
        /// </summary>
        /// <param name="Текст">Строка текста</param>
        public void ДобавитьНеФискальнуюПозицию(string Текст)
        {
            var item = new CheckItem
            {
                isFiscal = false,
                Name = Текст
            };
            _items.Add(item);
            Check.CheckItems = _items.ToArray();
        }

        /// <summary>
        /// Добавляет фискальную позицию в чек
        /// </summary>
        /// <param name="Наименование">Наименование позиции</param>
        /// <param name="Цена">Цена</param>
        /// <param name="Количество">Количество</param>
        /// <param name="Скидка">Скидка в деньгах</param>
        /// <param name="Отдел">Отдел (Департамент)</param>
        /// <param name="ТекстДо">Не фискальный текст до позиции</param>
        /// <param name="ТекстПосле">Не фискальный текст после позиции</param>
        [Obsolete("Нет возможности указать НДС")]
        public void ДобавитьПозицию(string Наименование, double Цена, double Количество, double Скидка, int Отдел, string ТекстДо, string ТекстПосле)
        {
            var item = new CheckItem
            {
                isFiscal = true,
                After = new PrintLine() { Line = ТекстПосле },
                Before = new PrintLine() { Line = ТекстДо },
                Quantity = (decimal)Количество,
                DiscountValue = (decimal)Скидка,
                Name = Наименование,
                Price = (decimal)Цена,
                Department = Отдел
            };
            _items.Add(item);
            Check.CheckItems = _items.ToArray();
        }

        /// <summary>
        /// Устанавливает оплату наличными
        /// </summary>
        /// <param name="Сумма">Сумма оплаты</param>
        /// <param name="Скидка">Скидка. Больше не учитывается. (Является необязательным параметром)</param>
        public void ДобавитьОплатуНаличными(double Сумма, double Скидка = 0)
        {
            Check.CashPayment = new Payment
            {
                Summ = (decimal)Сумма,
                Discount = (decimal)Скидка,
                TypeClose = PayTypes.Наличные
            };
        }

        /// <summary>
        /// Устанавливает оплату электронными
        /// </summary>
        /// <param name="Сумма">Сумма оплаты</param>
        /// <param name="Скидка">Скидка. Больше не учитывается. (Является необязательным параметром)</param>
        public void ДобавитьОплатуКартой(double Сумма, double Скидка = 0)
        {
            Check.ElectronicPayment = new Payment
            {
                Summ = (decimal)Сумма,
                Discount = (decimal)Скидка,
                TypeClose = PayTypes.Электронными
            };
        }

        /// <summary>
        /// Устанавливает оплату кредитом
        /// </summary>
        /// <param name="Сумма">Сумма оплаты</param>
        /// <param name="Скидка">Скидка. Больше не учитывается. (Является необязательным параметром)</param>
        public void ДобавитьОплатуКредит(double Сумма, double Скидка = 0)
        {
            Check.CreditPayment = new Payment
            {
                Summ = (decimal)Сумма,
                Discount = (decimal)Скидка,
                TypeClose = PayTypes.Кредит
            };
        }

        /// <summary>
        /// Устанавливает оплату предоплатой
        /// </summary>
        /// <param name="Сумма">Сумма оплаты</param>
        /// <param name="Скидка">Скидка. Больше не учитывается. (Является необязательным параметром)</param>
        public void ДобавитьОплатуПредоплатой(double Сумма, double Скидка = 0)
        {
            Check.AdvancePayment = new Payment
            {
                Summ = (decimal)Сумма,
                Discount = (decimal)Скидка,
                TypeClose = PayTypes.Предоплата
            };
        }

        /// <summary>
        /// Устанавливает оплату представлением
        /// </summary>
        /// <param name="Сумма">Сумма оплаты</param>
        /// <param name="Скидка">Скидка. Больше не учитывается. (Является необязательным параметром)</param>
        public void ДобавитьОплатуПредставлением(double Сумма, double Скидка = 0)
        {
            Check.CashProvisionPayment = new Payment
            {
                Summ = (decimal)Сумма,
                Discount = (decimal)Скидка,
                TypeClose = PayTypes.Представление
            };
        }

        /// <summary>
        /// Сумма документа
        /// </summary>
        [Obsolete("Не используется")]
        public decimal Сумма
        {
            get { return Check.Summ; }
            set { Check.Summ = value; }
        }

        /// <summary>
        /// Задает номер отдела
        /// </summary>
        public int Департамент
        {
            get { return Check.Department; }
            set { Check.Department = value; }
        }

        /// <summary>
        /// Задает пароль кассира
        /// </summary>
        /// <remarks>Если не задать этот параметр, будет использоваться пароль указанный в настройках кассы на Сервере ККМ</remarks>
        public string ПарольКассира
        {
            get
            {
                return Check.Password;
            }
            set
            {
                Check.Password = value;
            }
        }

        /// <summary>
        /// Возвращает строковое представление всех позиций чека
        /// </summary>
        [Obsolete]
        public string Позиции
        {
            get
            {
                var str = "";
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var item in Check.CheckItems)
                {
                    str += ItemToString(item);
                }
                return str;
            }
        }

        [Obsolete]
        private static string ItemToString(CheckItem item)
        {
            string str = item.Name + " " + item.Price + "*" + item.Quantity;
            if (item.DiscountValue != 0)
                str += "-" + item.DiscountValue;
            str += "=" + item.Summ;
            str += "\n";
            return str;
        }

        /// <summary>
        /// Задает параметр - является ли это задание чека фискальным
        /// </summary>
        [Obsolete("Следует использовать кириллическое имя " + nameof(Фискальный))]
        public bool IsFiscal
        {
            get
            {
                return Check.IsFiscal;
            }
            set
            {
                Check.IsFiscal = value;
            }
        }

        /// <summary>
        /// Задает параметр - является ли это задание чека фискальным
        /// </summary>
        public bool Фискальный
        {
            get
            {
                return Check.IsFiscal;
            }
            set
            {
                Check.IsFiscal = value;
            }
        }

        /// <summary>
        /// Задает параметр - является ли это задание чека электронным
        /// </summary>
        /// <remarks>Не печатается на чековой ленте. На электронном чеке обязательно надо указать Контакт, что-бы документ не печатался на чековой ленте</remarks>
        public bool Электронный
        {
            get
            {
                return Check.CheckMode == 0;
            }
            set { Check.CheckMode = value ? 0 : 1; }
        }

        /// <summary>
        /// Задает систему налогообложения задания
        /// </summary>
        /// <remarks>
        /// 0 - Общая
        /// 1 - Упрощенная Доход
        /// 2 - Упрощенная Доход минус Расход
        /// 3 - Единый налог на вмененный доход
        /// 4 - Единый сельскохозяйственный налог
        /// 5 - Патентная система налогообложения
        /// 999 - использовать настройки устройства
        /// </remarks>>
        public int ТипНалогообложения
        {
            get
            {
                return (int)Check.TaxType;
            }
            set
            {
                try
                {
                    Check.TaxType = (TaxVariants)value;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Неверный тип налогообложения " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Задает телефон или email покупателя
        /// </summary>
        public string Контакт
        {
            get
            {
                return Check.ClientContact;
            }
            set
            {
                Check.ClientContact = value;
            }
        }
    }
}