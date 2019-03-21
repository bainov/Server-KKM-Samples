using ClientPrint.PrintServiceRef;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ClientPrint.Документы
{
    ///  <summary>
    /// Представляет задание печати Чека коррекции
    /// </summary>
    /// <example>
    /// Пример кода
    /// <code language="1С" title="1C">
    /// ОбъектДрайвера= ПолучитьОбьектДрайвера(компонентаIFr2001, Адрес, Порт, ИмяУстройства);
    /// ЧекКор = ОбъектДрайвера.СоздатьЧекКоррекции();
    /// // Касса по умолчанию используется заданная при инициализации объекта драйвера
    /// // Чтобы другую кассу отличную от
    /// // ЧекКор.ИмяКассы = "Viki";
    ///
    /// ЧекКор.Кассир = Кассир;
    /// ЧекКор.ИннКассира = ИннКасира;
    ///
    /// // 1 - приход,
    /// // 2 - возврат прихода,
    /// // 4 - расход,
    /// // 5 - возврат расхода
    /// ЧекКор.ТипЧека = 1;
    ///
    /// // 0 - Общая
    /// // 1 - Упрощенная Доход
    /// // 2 - Упрощенная Доход минус Расход
    /// // 3 - Единый налог на вмененный доход
    /// // 4 - Единый сельскохозяйственный налог
    /// // 5 - Патентная система налогообложения
    /// // 999 - использовать настройки устройства
    /// ЧекКор.ТипНалогообложения = 0;
    ///
    ///
    /// ЧекКор.ДатаДокументаОснованияДляКоррекции = ТекущаяДата();
    /// ЧекКор.НомерДокументаОснованияДляКоррекции = "17-15Ф";
    /// ЧекКор.ОписаниеКоррекции  = "Продали воды, мучающимся от жары, гражданам";
    /// // 0 - самостоятельно, 1 - по предписанию
    /// ЧекКор.ТипКоррекции = 0;
    ///
    /// ЧекКор.СуммаНалогаБезНдс = 3200;
    /// ЧекКор.СуммаНалога0 = 0;
    /// ЧекКор.СуммаНалога10 = 0;
    /// ЧекКор.СуммаНалога18 = 0;
    /// ЧекКор.СуммаНалога110 = 0;
    /// ЧекКор.СуммаНалога118 = 0;
    ///
    /// ЧекКор.ДобавитьОплатуНаличными (3200);
    /// ЧекКор.ДобавитьОплатуКартой (0);
    /// ЧекКор.ДобавитьОплатуКредит (0);
    /// ЧекКор.ДобавитьОплатуПредоплатой (0);
    /// ЧекКор.ДобавитьОплатуПредставлением (0);
    ///
    /// Результат = ОбъектДрайвера.РаспечататьЧекКоррекции(ЧекКор);
    /// Если Результат.КодОшибки&lt;&gt;0 ТОгда
    ///     Сообщить("Ошибка "+Результат.КодОшибки+" Описание "+Результат.ОписаниеОшибки);
    ///     Возврат
    /// КонецЕсли;
    ///
    /// Сообщить("Чек номер = "+	Результат.НомерЧека);
    /// Сообщить("Чек смены = "+	Результат.НомерСмены);
    /// Сообщить("Фискальный признак = "+	Результат.ФискальныйПризнак );
    /// </code>
    /// </example>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ЧекКоррекции : DockBase
    {
        /// <summary>
        /// Предоставляет доступ к базовому документу
        /// </summary>
        public CheckCorrection Check
        {
            get { return Doc as CheckCorrection; }
            set { Doc = value; }
        }

#pragma warning disable 1591

        public ЧекКоррекции()
#pragma warning restore 1591
        {
            Doc = new CheckCorrection
            {
                DockId = Guid.NewGuid(),
                CheckType = CheckTypes.ЧекКоррекцииРасхода,
                DeviceId = Guid.Empty,
                TaxType = TaxVariants.None
            };
        }

        /// <summary>
        /// Задает тип задания чека коррекции
        /// </summary>
        ///<remarks>
        /// 1 - Приход
        /// 2 - Возврат прихода
        /// 3 - Расход
        /// 4 - Возврат расхода
        /// </remarks>
        public int ТипЧека
        {
            get { return (int)Check.CheckType - 6; }
            set { Check.CheckType = (CheckTypes)(value + 6); }
        }

        /// <summary>
        /// Задает тип коррекции задания
        /// </summary>
        /// <remarks>
        /// 0 - самостоятельно,
        /// 1 - по предписанию
        /// </remarks>>
        public int ТипКоррекции
        {
            get { return (int)Check.СorrectionType; }
            set
            {
                try
                {
                    Check.СorrectionType = (CorrectionTypes)value;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Тип коррекции может быть только: 0 - самостоятельно, 1 - по предписанию." + ex.Message);
                }
            }
        }

        /// <summary>
        /// Задает дату основания коррекции
        /// </summary>
        public DateTime ДатаДокументаОснованияДляКоррекции
        {
            get { return Check.CorrectionBaseDate; }
            set { Check.CorrectionBaseDate = value; }
        }

        /// <summary>
        /// Задает номер документа основания коррекции
        /// </summary>
        public string НомерДокументаОснованияДляКоррекции
        {
            get { return Check.CorrectionBaseNumber; }
            set { Check.CorrectionBaseNumber = value; }
        }

        /// <summary>
        /// Задает описание коррекции
        /// </summary>
        public string ОписаниеКоррекции
        {
            get { return Check.CorrectionBaseName; }
            set { Check.CorrectionBaseName = value; }
        }

        /// <summary>
        /// Задает сумму налогов по ставке 18%
        /// </summary>
        public double СуммаНалога18
        {
            get { return (double)Check.TaxSum18; }
            set { Check.TaxSum18 = (decimal)value; }
        }

        /// <summary>
        /// Задает сумму налогов по ставке 10%
        /// </summary>
        public double СуммаНалога10
        {
            get { return (double)Check.TaxSum10; }
            set { Check.TaxSum10 = (decimal)value; }
        }

        /// <summary>
        /// Задает сумму налогов по ставке 0%
        /// </summary>
        public double СуммаНалога0
        {
            get { return (double)Check.TaxSum0; }
            set { Check.TaxSum0 = (decimal)value; }
        }

        /// <summary>
        /// Задает сумму налогов Без НДС
        /// </summary>
        public double СуммаНалогаБезНдс
        {
            get { return (double)Check.TaxSumNone; }
            set { Check.TaxSumNone = (decimal)value; }
        }

        /// <summary>
        /// Задает сумму налогов по ставке 110%
        /// </summary>
        public double СуммаНалога110
        {
            get { return (double)Check.TaxSum110; }
            set { Check.TaxSum110 = (decimal)value; }
        }

        /// <summary>
        /// Задает сумму налогов по ставке 118%
        /// </summary>
        public double СуммаНалога118
        {
            get { return (double)Check.TaxSum118; }
            set { Check.TaxSum118 = (decimal)value; }
        }

        /// <summary>
        /// Устанавливает оплату наличными
        /// </summary>
        /// <param name="Сумма">Сумма оплаты</param>
        public void ДобавитьОплатуНаличными(double Сумма)
        {
            var pay = new Payment();
            pay.Summ = (decimal)Сумма;
            pay.TypeClose = PayTypes.Наличные;
            Check.CashPayment = pay;
        }

        /// <summary>
        /// Устанавливает оплату электронными
        /// </summary>
        /// <param name="Сумма">Сумма оплаты</param>
        public void ДобавитьОплатуКартой(double Сумма)
        {
            Payment pay = new Payment();
            pay.Summ = (decimal)Сумма;
            pay.TypeClose = PayTypes.Электронными;
            Check.ElectronicPayment = pay;
        }

        /// <summary>
        /// Устанавливает оплату кредитом
        /// </summary>
        /// <param name="Сумма">Сумма оплаты</param>
        public void ДобавитьОплатуКредит(double Сумма)
        {
            Payment pay = new Payment();
            pay.Summ = (decimal)Сумма;
            pay.TypeClose = PayTypes.Кредит;
            Check.CreditPayment = pay;
        }

        /// <summary>
        /// Устанавливает оплату предоплатой
        /// </summary>
        /// <param name="Сумма">Сумма оплаты</param>
        public void ДобавитьОплатуПредоплатой(double Сумма)
        {
            Payment pay = new Payment();
            pay.Summ = (decimal)Сумма;
            pay.TypeClose = PayTypes.Предоплата;
            Check.AdvancePayment = pay;
        }

        /// <summary>
        /// Устанавливает оплату представлением
        /// </summary>
        /// <param name="Сумма">Сумма оплаты</param>
        public void ДобавитьОплатуПредставлением(double Сумма)
        {
            Payment pay = new Payment();
            pay.Summ = (decimal)Сумма;
            pay.TypeClose = PayTypes.Представление;
            Check.CashProvisionPayment = pay;
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
    }
}