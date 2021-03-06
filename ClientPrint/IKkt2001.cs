﻿using System.Diagnostics.CodeAnalysis;

namespace ClientPrint
{
    /// <summary>
    /// Требования к разработке драйверов для ККТ с функцией передачи в ОФД 2.1 <a href="https://its.1c.ru/db/metod8dev#content:4829:hdoc:chapter270">Источник</a>
    /// </summary>
    /// <remarks> ВНИМАНИЕ: Данный стандарт определяет требования к ККТ на основании форматов фискальных документов версии «1.0»</remarks>

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface IKkt2001
    {
        /// <summary>
        /// Получение данных из ККТ для регистрации фискального накопителя и последующей работы
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <param name="ТаблицаПараметровККТ">Регистрационные данные фискального накопителя <a href="https://its.1c.ru/db/metod8dev#content:4829:hdoc:tableparameterskkt">Подробнее</a> </param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ПолучитьПараметрыККТ(string ИдУстройства, out string ТаблицаПараметровККТ);

        //GetDataKKT

        /// <summary>
        /// Операция с фискальным накопителем. После проведения операции происходит печать отчета о проведении соответствующей операции.
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <param name="ТипОперации">Тип операции: 1 - Регистрация 2 - Изменение параметров регистрации 3 - Закрытие ФН</param>
        /// <param name="Кассир">ФИО уполномоченного лица для проведения операции</param>
        /// <param name="ПараметрыФискализации">Данные для фискализации фискального накопителя <a href="https://its.1c.ru/db/content/metod8dev/src/developers/additional/guides/i8104829.htm?_=1530716605#parametersfiscal">Подробнее</a></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ОперацияФН(string ИдУстройства, long ТипОперации, string Кассир, string ПараметрыФискализации);

        //OperationFN
        /// <summary>
        /// Открывает новую смену и печатает на ККТ отчет об открытии смены.
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <param name="Кассир">ФИО уполномоченного лица для проведения операции</param>
        /// <param name="НомерСмены">Номер смены</param>
        /// <param name="НомерДокумента">Номер отчета об открытии смены</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ОткрытьСмену(string ИдУстройства, string Кассир, out int НомерСмены, out int НомерДокумента);

        //OpenShift

        /// <summary>
        /// Закрывает открытую ранее смену и печатает на ККТ отчет о закрытии смены.
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <param name="Кассир">ФИО уполномоченного лица для проведения операции</param>
        /// <param name="НомерСмены">Номер смены</param>
        /// <param name="НомерДокумента">Номер отчета об закрытии смены</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ЗакрытьСмену(string ИдУстройства, string Кассир, out int НомерСмены, out int НомерДокумента);

        //CloseShift

        /// <summary>
        /// Формирование чека в пакетном режиме. Передается структура, описывающая тип открываемого чека, фискальные и текстовые строки, штрих-коды, которые будут напечатаны. Также передаются суммы оплат для закрытия чека.
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <param name="Кассир">ФИО уполномоченного лица для проведения операции</param>
        /// <param name="Электронно">Формирование чека в только электроном виде.Печать чека не осуществляется.</param>
        /// <param name="ДанныеЧека">XML структура - описание чека .<a href="https://its.1c.ru/db/content/metod8dev/src/developers/additional/guides/i8104829.htm?_=1530716605#checkpackage">Подробнее</a></param>
        /// <param name="НомерЧека">Номер фискального чека возвращаемый ФН</param>
        /// <param name="НомерСмены">Номер открытой смены</param>
        /// <param name="ФискальныйПризнак">Фискальный признак</param>
        /// <param name="АдресСайтаПроверки">Адрес сайта проверки</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool СформироватьЧек(string ИдУстройства, string Кассир, bool Электронно, string ДанныеЧека, out int НомерЧека, out int НомерСмены, out string ФискальныйПризнак, out string АдресСайтаПроверки);

        //ProcessCheck

        /// <summary>
        /// Формирование чека коррекции в пакетном режиме. Передается структура, описывающая тип открываемого чека и атрибуты чека.
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <param name="Кассир">ФИО уполномоченного лица для проведения операции</param>
        /// <param name="ДанныеЧекаКоррекции">XML структура - описание чека коррекции. <a href="https://its.1c.ru/db/content/metod8dev/src/developers/additional/guides/i8104829.htm?_=1530716605#checkcorrectionpackage">Подробнее</a></param>
        /// <param name="НомерЧека">Номер фискального чека возвращаемый ФН</param>
        /// <param name="НомерСмены">Номер открытой смены</param>
        /// <param name="ФискальныйПризнак">Фискальный признак</param>
        /// <param name="АдресСайтаПроверки">Адрес сайта проверки</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool СформироватьЧекКоррекции(string ИдУстройства, string Кассир, string ДанныеЧекаКоррекции, out int НомерЧека, out int НомерСмены, out string ФискальныйПризнак, out string АдресСайтаПроверки);

        //ProcessCorrectionCheck

        /// <summary>
        /// Печать текстового документа (текстовый слип-чек, информационная квитанция)
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <param name="ДанныеДокумента">	XML структура - описание текстового документа. <a href="https://its.1c.ru/db/content/metod8dev/src/developers/additional/guides/i8104829.htm?_=1530716605#documentpackage">Подробнее</a></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool НапечататьТекстовыйДокумент(string ИдУстройства, string ДанныеДокумента);

        //PrintTextDocument

        /// <summary>
        /// Печатает на чек внесения/ выемки
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <param name="Сумма">Сумма внесения /выемки</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        /// <remarks> (зависит от переданной суммы). Сумма &gt;= 0 - внесение, Сумма &lt; 0 - выемка.</remarks>
        bool НапечататьЧекВнесенияВыемки(string ИдУстройства, double Сумма);

        //CashInOutcome

        /// <summary>
        /// Печатает на отчет за смену без закрытия кассовой смены
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool НапечататьОтчетБезГашения(string ИдУстройства);

        //PrintXReport

        /// <summary>
        /// Получение текущего состояние ККТ.
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <param name="НомерЧека">Номер последнего пробитого фискального чека</param>
        /// <param name="НомерСмены">Номер смены</param>
        /// <param name="СтатусСмены">Состояние смены 1 - Закрыта 2 - Открыта 3 - Истекла</param>
        /// <param name="ПараметрыСостояния">XML структура - описание параметров состояния. <a href="https://its.1c.ru/db/content/metod8dev/src/developers/additional/guides/i8104829.htm?_=1530716605#statusparameters">Подробнее</a> </param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ПолучитьТекущееСостояние(string ИдУстройства, out int НомерЧека, out int НомерСмены, out int СтатусСмены, out string ПараметрыСостояния);

        //GetCurrentStatus

        /// <summary>
        /// Формирует отчет о текущем состоянии расчетов
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ОтчетОТекущемСостоянииРасчетов(string ИдУстройства);

        //ReportCurrentStatusOfSettlements
        /// <summary>
        /// Производит открытие денежного ящика, подключенного к фискальному регистратору.
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ОткрытьДенежныйЯщик(string ИдУстройства);

        //OpenCashDrawer

        /// <summary>
        /// Получить ширину строки чека в символах.
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <param name="ШиринаСтроки">	Ширина строки в символах</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ПолучитьШиринуСтроки(string ИдУстройства, out int ШиринаСтроки);

        //GetLineLength
    }
}