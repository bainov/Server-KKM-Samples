﻿using ClientPrint.Документы;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ClientPrint
{
    /// <summary>
    /// Дополнительные методы РБ-Софт
    /// </summary>
    /// <remarks>
    /// Возможности:
    /// Позволяют работать с документами как с объектами
    /// Позволяют работать с ФФД 1.0.5
    /// Позволяют работать из 1С 7.7.
    /// </remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface IRbsoftExtension
    {
        /// <summary>
        /// Производит инициализацию внешней компоненты, т.е. пытается подключится к Серверу ККМ по заданным свойствам адреса и порта.
        /// </summary>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool Инициализация();

        /// <summary>
        /// Используется в 1С 7.7
        /// </summary>
        [Obsolete("Оставлен для совместимости обработок для 1С 7.7 младше 10.08.18")]
        string ИдентификаторОбъекта { get; set; }

        /// <summary>
        /// Получить текущее состояние расчетов с ОФД
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <param name="КоличествоНеотправленных"></param>
        /// <param name="НомерПервогоНеотправленного"></param>
        /// <param name="ДатаПервогоНеотправленного"></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ПолучитьТекущееСостояниеРасчетов(string ИдУстройства, out int КоличествоНеотправленных,
            out int НомерПервогоНеотправленного, out DateTime ДатаПервогоНеотправленного);

        /// <summary>
        /// Получить текущее состояние ККТ
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <param name="НомерЧека"></param>
        /// <param name="НомерСмены">НомерСмены</param>
        /// <param name="СтатусСмены">1 - Закрыта; 2 - Открыта; 3 - Истекла</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ПолучитьСостояниеКассы(string ИдУстройства, out int НомерЧека, out int НомерСмены, out int СтатусСмены);

        /// <summary>
        /// Проверка печатался ли ранее документ на Сервере ККМ
        /// </summary>
        /// <param name="ИдЧека"></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ДокументПечаталсяРанее(string ИдЧека);

        /// <summary>
        /// Печать копии последнего чека
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool НапечататьКопиюЧека(string ИдУстройства);

        /// <summary>
        /// Выводит произвольную текстовую строку на чековую ленту.
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <param name="Строка"></param>
        /// <returns>Значение = 1, если метод успешно выполнен; в противном случае — значение = 0.</returns>
        /// <remarks>Применяется в 1С 7.7, так как в этой платформе нет булевого типа</remarks>
        int НапечататьНефискСтроку77(string ИдУстройства, string Строка);

        /// <summary>
        /// Печатает отчет за смену без гашения (не закрывает кассовую смену)
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <returns>Значение = 1, если метод успешно выполнен; в противном случае — значение = 0.</returns>
        /// <remarks>Применяется в 1С 7.7, так как в этой платформе нет булевого типа</remarks>
        int НапечататьОтчетБезГашения77(string ИдУстройства);

        /// <summary>
        /// Печатает отчет за смену с гашением (закрывает кассовую смену).
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <returns>Значение = 1, если метод успешно выполнен; в противном случае — значение = 0.</returns>
        /// <remarks>Применяется в 1С 7.7, так как в этой платформе нет булевого типа</remarks>
        int НапечататьОтчетСГашением77(string ИдУстройства);

        /// <summary>
        /// Печатает строку товарной позиции с переданными реквизитами с учетом признака расчета и предмета расчета
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <param name="Наименование"></param>
        /// <param name="Количество"></param>
        /// <param name="Цена"></param>
        /// <param name="Сумма"></param>
        /// <param name="Отдел"></param>
        /// <param name="НДС"></param>
        /// <param name="ПризнакСпособаРасчета"></param>
        /// <param name="ПризнакПредметаРасчета"></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool НапечататьФискСтроку105(string ИдУстройства, string Наименование, double Количество, double Цена,
            double Сумма, long Отдел, double НДС, int ПризнакСпособаРасчета, int ПризнакПредметаРасчета);

        /// <summary>
        /// Печатает строку товарной позиции с переданными реквизитами
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <param name="Наименование"></param>
        /// <param name="Количество"></param>
        /// <param name="Цена"></param>
        /// <param name="Сумма"></param>
        /// <param name="Отдел"></param>
        /// <param name="НДС"></param>
        /// <returns>Значение = 1, если метод успешно выполнен; в противном случае — значение = 0.</returns>
        /// <remarks>Применяется в 1С 7.7, так как в этой платформе нет булевого типа</remarks>
        int НапечататьФискСтроку77(string ИдУстройства, string Наименование, double Количество, double Цена,
            double Сумма, int Отдел, double НДС);

        /// <summary>
        /// Отключает оборудование
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <returns>Значение = 1, если метод успешно выполнен; в противном случае — значение = 0.</returns>
        /// <remarks>Применяется в 1С 7.7, так как в этой платформе нет булевого типа</remarks>
        int Отключить77(string ИдУстройства);

        /// <summary>
        /// Открывает новый чек.
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <param name="ФискальныйЧек">Признак фискального чека</param>
        /// <param name="ЧекВозврата">Признак чека возврата</param>
        /// <param name="АнулироватьОткрытыйЧек">Признак автоматического аннулирования ранее открытого чека</param>
        /// <param name="НомерЧека">Номер нового чека</param>
        /// <param name="НомерСмены">Номер открытой смены</param>
        /// <returns>Значение = 1, если метод успешно выполнен; в противном случае — значение = 0.</returns>
        /// <remarks>Применяется в 1С 7.7, так как в этой платформе нет булевого типа</remarks>
#warning Номер чека и смены могут отличаться

        int ОткрытьЧек77(string ИдУстройства, int ФискальныйЧек, int ЧекВозврата, int АнулироватьОткрытыйЧек,
            out int НомерЧека, out int НомерСмены);

        /// <summary>
        /// Производит открытие денежного ящика, подключенного к фискальному регистратору.
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ОткрытьЯщик(string ИдУстройства);

        /// <summary>
        /// Производит отрезку чековой ленты на ККМ
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool Отрезка(string ИдУстройства);

        /// <summary>
        /// Печатает отчет о текущем состоянии расчетов
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ОтчетОТекущемСостоянииРасчетов(string ИдУстройства);

        /// <summary>
        /// Печатает отчет по кассирам
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ОтчетПоКассирам(string ИдУстройства);

        /// <summary>
        /// Печатает отчет по секциям
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ОтчетПоСекциям(string ИдУстройства);

        /// <summary>
        /// Печатаем на ККМ клише
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <returns>Значение = 1, если метод успешно выполнен; в противном случае — значение = 0.</returns>
        [Obsolete]
        int ПечатьКлише(string ИдУстройства);

        /// <summary>
        /// Подключает оборудование с текущими значениями параметров, установленных функцией «УстановитьПараметр».
        /// </summary>
        /// <param name="ИдУстройства">Возвращает идентификатор подключенного экземпляра устройства</param>
        /// <returns>Значение = 1, если метод успешно выполнен; в противном случае — значение = 0.</returns>
        /// <remarks>Применяется в 1С 7.7, так как в этой платформе нет булевого типа</remarks>
        int Подключить77(out string ИдУстройства);

        /// <summary>
        /// Получаем идентификатор устройства
        /// </summary>
        /// <param name="ИмяУстройства">Имя устройства, зарегистрированное на Сервере ККМ</param>
        /// <returns>идентификатор в формате GUID</returns>
        string ПолучитьИдентификаторПоИмени(string ИмяУстройства);

        /// <summary>
        /// Получить список всех устройств добавленных на Сервер ККМ.
        /// </summary>
        /// <returns>Массив имен устройств.
        /// Имя - это псевдоним устройства заполняемый пользователем при добавлении устройства
        /// </returns>
        [Obsolete]
        string[] ПолучитьИменаУстройств();

        /// <summary>
        /// Получить данные чека и смены последнего распечатанного документа
        /// </summary>
        /// <param name="НомерЧека"></param>
        /// <param name="НомерСмены"></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        [Obsolete]
        bool ПолучитьИнфоПоследнийДокумент(out int НомерЧека, out int НомерСмены);

        /// <summary>
        /// Получить номер версии Сервера ККМ
        /// </summary>
        /// <returns>Номер версии Сервера ККМ</returns>
        string ПолучитьНомерВерсииСервера();

        /// <summary>
        /// Получить список всех идентификаторов устройств добавленных на Сервер ККМ.
        /// </summary>/// <returns>Значение = 1, если метод успешно выполнен; в противном случае — значение = 0.</returns>
        /// <returns>Массив идентификаторов устройств в формате GUID</returns>
        ///<remarks>
        /// Используется тип маршализации SafeArray
        /// SafeArray - самоописывающий массив
        /// </remarks>
        /// <returns>Список GUID (идентификаторов)</returns>
        [Obsolete]
        string[] ПолучитьСписокИдентификаторовУстройств();

        /// <summary>
        /// Получить сумму денег в ККМ
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <returns>Сумма денег в ККМ</returns>
        double ПолучитьСуммуНаличных(string ИдУстройства);

        /// <summary>
        /// Получить чек из базы данных по идентификатору
        /// </summary>
        /// <param name="ИдЧека"></param>
        Чек ПолучитьЧек(string ИдЧека);

        /// <summary>
        /// Распечатать чека внесения/выемки
        /// </summary>
        /// <param name="Документ"></param>
        /// <returns> = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        ВнесениеВыемка РаспечататьВнесениеВыемка(ВнесениеВыемка Документ);

        /// <summary>
        /// Распечатать заказ
        /// </summary>
        /// <param name="ДокументЗаказ"></param>
        Заказ РаспечататьЗаказ(Заказ ДокументЗаказ);

        /// <summary>
        /// Закрывает чек.
        /// </summary>
        /// <remarks>Сумма всех видов оплат должна быть больше суммы открытого чека.</remarks>
        [Obsolete]
        bool РаспечататьИЗакрытьЧек();

        /// <summary>
        /// Распечатывает отчет
        /// </summary>
        /// <param name="ДокументОтчет"></param>
        Отчет РаспечататьОтчет(Отчет ДокументОтчет);

        /// <summary>
        /// Распечатывает текст
        /// </summary>
        /// <param name="ДокументТекст"></param>
        ТекстовыйФайл РаспечататьТекст(ТекстовыйФайл ДокументТекст);

        /// <summary>
        /// Распечатывает чек
        /// </summary>
        /// <param name="ДокументЧек"></param>
        Чек РаспечататьЧек(Чек ДокументЧек);

        /// <summary>
        /// Распечатывает чек коррекции
        /// </summary>
        /// <param name="ДокументЧекКоррекции"></param>
        ЧекКоррекции РаспечататьЧекКоррекции(ЧекКоррекции ДокументЧекКоррекции);

        /// <summary>
        /// Создает объект документа чек внесения/выемки
        /// </summary>
        ВнесениеВыемка СоздатьВнесениеВыемка();

        /// <summary>
        /// Создает объект документа заказ
        /// </summary>
        Заказ СоздатьЗаказ();

        /// <summary>
        /// Создает объект документа отчета
        /// </summary>
        Отчет СоздатьОтчет();

        /// <summary>
        /// Создает объект документа текст
        /// </summary>
        ТекстовыйФайл СоздатьТекст();

        /// <summary>
        /// Создает объект документа чек
        /// </summary>
        Чек СоздатьЧек();

        /// <summary>
        /// Создает объект документа чек коррекции
        /// </summary>
        ЧекКоррекции СоздатьЧекКоррекции();

        /// <summary>
        /// Устанавливает имя кассира в открытом чеке
        /// </summary>
        /// <param name="Кассир"></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool УстановитьИмяКассира(string Кассир);

        /// <summary>
        /// Устанавливает ИНН кассира в открытом чеке
        /// </summary>
        /// <param name="ИннКассира">ИНН кассира</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool УстановитьИннКассира(string ИннКассира);

        /// <summary>
        /// Устанавливает пароль кассира в открытом чеке
        /// </summary>
        /// <param name="Пароль"></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool УстановитьПарольКассира(string Пароль);

        /// <summary>
        /// Устанавливает имя кассира в открытом чеке
        /// </summary>
        /// <param name="Кассир"></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        /// <remarks>Применяется в 1С 7.7, так как в этой платформе нет булевого типа</remarks>
        int УстановитьИмяКассира77(string Кассир);

        /// <summary>
        /// Установить контакт клиента в открытом чеке
        /// </summary>
        /// <param name="ТелефонИлиEmail"></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool УстановитьКонтактКлиента(string ТелефонИлиEmail);

        /// <summary>
        /// Установить контакт клиента в открытом чеке
        /// </summary>
        /// <param name="ТелефонИлиEmail"></param>
        /// <returns>Значение = 1, если метод успешно выполнен; в противном случае — значение = 0.</returns>
        /// <remarks>Применяется в 1С 7.7, так как в этой платформе нет булевого типа</remarks>
        int УстановитьКонтактКлиента77(string ТелефонИлиEmail);

        /// <summary>
        /// Изменяет размер штрих-кода
        /// </summary>
        /// <param name="size"></param>
        void УстановитьРазмерШтрихКода(int size);

        /// <summary>
        /// Установить систему налогообложения в открытом чеке
        /// </summary>
        /// <param name="TaxVariant"></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool УстановитьСистемуНалогообложения(int TaxVariant);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <param name="ИмяКассира"></param>
        /// <param name="ИннКассира"></param>
        /// <param name="НомерДокумента"></param>
        /// <param name="НомерСессии"></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ОткрытьСмену105(string ИдУстройства, string ИмяКассира, string ИннКассира, out int НомерДокумента,
            out int НомерСессии);

        #region ЧекКоррекции

        /// <summary>
        /// Открывает новый чек коррекции
        /// </summary>
        void ОткрытьЧекКоррекции();

        /// <summary>
        /// Устанавливает дату документа основания для коррекции
        /// </summary>
        /// <param name="ДатаДокумента"></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool УстановитьДатуДокументаОснованияДляКоррекции(DateTime ДатаДокумента);

        /// <summary>
        /// Устанавливает номер документа основания для коррекции
        /// </summary>
        /// <param name="НомерДокумента"></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool УстановитьНомерДокументаОснованияДляКоррекции(string НомерДокумента);

        /// <summary>
        /// Устанавливает описание коррекции
        /// </summary>
        /// <param name="ОписаниеКоррекции"></param>
        bool УстановитьОписаниеКоррекции(string ОписаниеКоррекции);

        /// <summary>
        /// Установление суммы налогов для коррекции
        /// </summary>
        /// <param name="taxNone"></param>
        /// <param name="tax0"></param>
        /// <param name="tax10"></param>
        /// <param name="tax18"></param>
        /// <param name="tax110"></param>
        /// <param name="tax118"></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool УстановитьДляКоррекцииСуммыНалогов(double taxNone, double tax0, double tax10, double tax18, double tax110,
            double tax118);

        /// <summary>
        /// Устанавливает тип коррекции
        /// </summary>
        /// <param name="ТипКоррекции">Самостоятельно = 0,  ПоПредписанию = 1</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool УстановитьТипКоррекции(int ТипКоррекции);

        /// <summary>
        /// Установить тип налогообложения для коррекции
        /// </summary>
        /// <param name="ТипСистемыНалогообложнения">Номер перечисления системы налогообложения</param>
        /// <remarks> ОСН = 0, УСН = 1, УСНД_Р = 2, ЕНВД = 3, ЕСН = 4, ПСН = 5, None = 999 </remarks>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool УстановитьДляКоррекцииТипНалогообложения(int ТипСистемыНалогообложнения);

        /// <summary>
        /// Установить для коррекции тип чека
        /// </summary>
        /// <param name="typeCorrection">  1 - Приход,  3 - Расход</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool УстановитьДляКоррекцииТипЧека(int typeCorrection);

        /// <summary>
        /// Устанавливает оплату для коррекции
        /// </summary>
        /// <param name="НаличнаяОплата"></param>
        /// <param name="ОплатаЭлектронными"></param>
        /// <param name="ОплатаКредитом"></param>
        /// <param name="Предоплатаой"></param>
        /// <param name="Представлением"></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ДобавитьОплатыЧекаКоррекции(double НаличнаяОплата, double ОплатаЭлектронными, double ОплатаКредитом,
            double Предоплатаой, double Представлением);

        /// <summary>
        /// Распечатывает чек коррекции на ККМ
        /// </summary>
        /// <param name="ИдУстрйоства"></param>
        /// <param name="Кассир"></param>
        /// <param name="ИннКассира"></param>
        /// <param name="НомерЧека"></param>
        /// <param name="НомерСесси"></param>
        /// <param name="ФискальынйПризнак"></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ЗакрытьЧекКоррекции(string ИдУстрйоства, string Кассир, string ИннКассира, out int НомерЧека,
            out int НомерСесси, out string ФискальынйПризнак);

        #endregion ЧекКоррекции

        /// <summary>
        /// Расширенный метод оплаты чека ФФД 105
        /// </summary>
        /// <param name="НаличнаяОплата"></param>
        /// <param name="ОплатаЭлектронными"></param>
        /// <param name="ОплатаКредитом"></param>
        /// <param name="Предоплатаой"></param>
        /// <param name="Представлением"></param>
        /// <returns></returns>
        bool ДобавитьОплаты105(double НаличнаяОплата, double ОплатаЭлектронными, double ОплатаКредитом,
            double Предоплатаой, double Представлением);

        /// <summary>
        /// Расширенный метод закрытия чека ФФД 105 с получением фискальных данных и данных кассира
        /// </summary>
        /// <param name="ИдУстрйоства"></param>
        /// <param name="Кассир"></param>
        /// <param name="ИннКассира"></param>
        /// <param name="НомерЧека"></param>
        /// <param name="НомерСессии"></param>
        /// <param name="ФискальныйПризнак"></param>
        /// <returns></returns>
        bool ЗакрытьЧек105(string ИдУстрйоства, string Кассир, string ИннКассира, out int НомерЧека,
          out int НомерСессии, out string ФискальныйПризнак);
    }
}