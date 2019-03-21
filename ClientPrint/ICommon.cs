﻿using System.Diagnostics.CodeAnalysis;

namespace ClientPrint
{
    /// <summary>
    /// Обязательные функции и методы, связанные с использованием драйвера подключаемого оборудования в системе <a href="https://its.1c.ru/db/content/metod8dev/src/developers/additional/guides/i8104829.htm?_=1530716605#chapter22">Источник</a>
    /// </summary>
    /// <remarks> Требования к разработке драйверов подключаемого оборудования (версия 2.1)</remarks>>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface ICommon
    {
        /// <summary>
        /// Возвращает номер версии драйвера.
        /// </summary>
        /// <returns>Номер версии драйвера</returns>
        string ПолучитьНомерВерсии();

        // GetVersion

        /// <summary>
        /// Возвращает информацию о драйвере, такую как название и описание, поддерживаемый тип оборудования.
        /// </summary>
        /// <param name="Наименование">Наименование драйвера</param>
        /// <param name="Описание">Описание драйвера></param>
        /// <param name="ТипОборудования">Строка, определяющая тип оборудования*</param>
        /// <param name="РевизияИнтерфейса">Поддерживаемая версия требований** для данного типа оборудования</param>
        /// <param name="ИнтеграционнаяБиблиотека">	Флаг возвращает, является ли компонент интеграционной библиотекой драйвера или самостоятельным драйвером</param>
        /// <param name="ОсновнойДрайверУстановлен">Для интеграционной библиотеки возвращает флаг установки основной поставки драйвера</param>
        /// <param name="URLCкачивания">Возвращает пустую строку или адрес страницы сайта производителя, по которому доступна ссылка для скачивания основной поставки драйвера или иная информация о драйвере. При возвращении пустой строки функционал установки основной поставки драйвера не активизируется.</param>
        ///<remarks>* - Строка, определяющая тип оборудования, имеет одно из значений: “СканерШтрихкода“, “СчитывательМагнитныхКарт“, “ФискальныйРегистратор“, “ПринтерЧеков“, “ПринтерЭтикеток“, “ДисплейПокупателя“, “ТерминалСбораДанных“, “ЭквайринговыйТерминал“, “ЭлектронныеВесы“, “ВесыСПечатьюЭтикеток“, “СчитывательRFID“. ** - Версия требований – версия текущего документа(Версии 1.00 соответствует число 1000. Версии 1.2 соответствует число 1002.</remarks>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ПолучитьОписание(out string Наименование,
                        out string Описание,
                        out string ТипОборудования,
                        out int РевизияИнтерфейса,
                        out bool ИнтеграционнаяБиблиотека,
                        out bool ОсновнойДрайверУстановлен,
                        out string URLCкачивания);

        // GetDescription

        /// <summary>
        /// Возвращает код и описание последней произошедшей ошибки.
        /// </summary>
        /// <param name="ОписаниеОшибки"></param>
        /// <returns>Код ошибки</returns>
        long ПолучитьОшибку(out string ОписаниеОшибки);

        // GetLastError

        /// <summary>
        /// Возвращает список параметров настройки драйвера и их типы, значения по умолчанию и возможные значения.
        /// </summary>
        /// <param name="ПараметрыДрайвера">Список параметров <a href ="https://its.1c.ru/db/metod8dev#content:4829:hdoc:table_1">Подробнее</a></param>
        bool ПолучитьПараметры(out string ПараметрыДрайвера);

        // GetParameters

        /// <summary>
        /// Установка значения параметра по имени
        /// </summary>
        /// <param name="ИмяПараметра">Имя параметра</param>
        /// <param name="ЗначениеПараметра">Значение параметра</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool УстановитьПараметр(string ИмяПараметра, object ЗначениеПараметра);

        // SetParameter

        /// <summary>
        /// Подключает оборудование с текущими значениями параметров, установленных функцией «УстановитьПараметр». Возвращает идентификатор подключенного экземпляра устройства
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool Подключить(out string ИдУстройства);

        // Open
        /// <summary>
        /// Отключает оборудование
        /// </summary>
        /// <param name="ИдУстройства">Идентификатор устройства</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool Отключить(string ИдУстройства);

        // Close

        /// <summary>
        /// Выполняет пробное подключение и опрос устройства с текущими значениями параметров, установленными функцией «УстановитьПараметр». При успешном выполнении подключения в описании возвращается информация об устройстве
        /// </summary>
        /// <param name="РезультатТеста">Описание результата выполнения теста</param>
        /// <param name="АктивированДемоРежим">	Возвращает описание ограничений демонстрационного режима при его наличии и пустой результат при его отсутствии. Пример: драйвер является платным, и для полноценной работы нужен ключ защиты.</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ТестУстройства(out string РезультатТеста, out string АктивированДемоРежим);

        // DeviceTest

        /// <summary>
        /// Получает список действий, которые будут отображаться как дополнительные пункты меню в форме настройки оборудования, доступной администратору. Если действий не предусмотрено, возвращает пустую строку.
        /// </summary>
        /// <param name="ДополнительныеДействия">Список дополнительных действий <a href ="https://its.1c.ru/db/content/metod8dev/src/developers/additional/guides/i8104829.htm?_=1530716605#table_1_4">Подробнее</a></param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ПолучитьДополнительныеДействия(out string ДополнительныеДействия);

        // GetAdditionalActions

        /// <summary>
        /// Команда на выполнение дополнительного действия с определенными именем
        /// </summary>
        /// <param name="ИмяДействия">Имя действия</param>
        /// <returns>Значение = Истина, если метод успешно выполнен; в противном случае — значение = Ложь.</returns>
        bool ВыполнитьДополнительноеДействие(string ИмяДействия);

        // DoAdditionalAction
    }
}