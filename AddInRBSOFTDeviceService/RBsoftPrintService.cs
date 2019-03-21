using ClientPrint;
using ClientPrint.Components;
using ClientPrint.Документы;
using System;
using System.ComponentModel;
using System.EnterpriseServices;
using System.Runtime.InteropServices;

#pragma warning disable 1591

// Перегрузка любого метода вызовет ошибку в 1С
// Не перегружайте методы

namespace AddinRBsoft
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    //   "1С:Предприятие" определяет ProgID COM—объекта компоненты следующим образом:
    //ProgID имеет вид<Vendor>.<Component>;
    //в качестве первой части(<Vendor>) используется строка "AddIn";
    //в качестве второй части(<Component>) используется строка с ID 100 из таблицы строк компоненты.Строка может иметь вид "Name1|Name2|...|NameN", и в этом случае будут созданы все объекты с ProgID вида "AddIn.NameX". Если такая строка отсутствует, то используется имя файла внешней компоненты без расширения.
    // https://its.1c.ru/db/metod8dev#content:3221:hdoc:_com_comp

    [ProgId("Addin." + nameof(RBsoftPrintServiceV3))]
    [System.EnterpriseServices.Description(nameof(RBsoftPrintServiceV3) + ".Component class")]
    // ReSharper disable once InconsistentNaming
    public class RBsoftPrintServiceV3 : ClientPrint.Components.Component, IConfigure, ICommon, IFr2001, IRbsoftExtension, IKostanova
    {
        #region default

        private IAsyncEvent _asyncEvent;
        private IStatusLine _statusLine;

        #endregion default

        protected override void Init(object connection)
        {
            _asyncEvent = (IAsyncEvent)connection;
            _statusLine = (IStatusLine)connection;
        }

        private readonly ClientPr _c;

        public RBsoftPrintServiceV3()
        {
            _c = new ClientPr();
        }

        #region Extension

        public string ИдентификаторОбъекта
        {
            get
            {
                return _c.ИдентификаторОбъекта;
            }

            set
            {
                _c.ИдентификаторОбъекта = value;
            }
        }

        public bool УстановитьИннКассира(string иннКассира)
        {
            return _c.УстановитьИннКассира(иннКассира);
        }

        public bool УстановитьПарольКассира(string password)
        {
            return _c.УстановитьПарольКассира(password);
        }

        #region ЧекКоррекции

        public void ОткрытьЧекКоррекции()
        {
            _c.ОткрытьЧекКоррекции();
        }

        public bool УстановитьДатуДокументаОснованияДляКоррекции(DateTime date)
        {
            return _c.УстановитьДатуДокументаОснованияДляКоррекции(date);
        }

        public bool УстановитьНомерДокументаОснованияДляКоррекции(string number)
        {
            return _c.УстановитьНомерДокументаОснованияДляКоррекции(number);
        }

        public bool УстановитьОписаниеКоррекции(string description)
        {
            return _c.УстановитьОписаниеКоррекции(description);
        }

        public bool УстановитьДляКоррекцииСуммыНалогов(double taxNone, double tax0, double tax10, double tax18, double tax110, double tax118)
        {
            return _c.УстановитьДляКоррекцииСуммыНалогов(taxNone, tax0, tax10, tax18, tax110, tax118);
        }

        public bool УстановитьТипКоррекции(int corretionType)
        {
            return _c.УстановитьТипКоррекции(corretionType);
        }

        public bool УстановитьДляКоррекцииТипНалогообложения(int snoType)
        {
            return _c.УстановитьДляКоррекцииТипНалогообложения(snoType);
        }

        public bool УстановитьДляКоррекцииТипЧека(int checkType)
        {
            return _c.УстановитьДляКоррекцииТипЧека(checkType);
        }

        public bool ДобавитьОплатыЧекаКоррекции(double НаличнаяОплата, double ОплатаЭлектронными, double ОплатаКредитом, double Предоплатаой, double Представлением)
        {
            return _c.ДобавитьОплатыЧекаКоррекции(НаличнаяОплата, ОплатаЭлектронными, ОплатаКредитом, Предоплатаой, Представлением);
        }

        public bool ЗакрытьЧекКоррекции(string deviceId, string кассир, string vatin, out int checkNumber, out int sessionNumber, out string fiscalSign)
        {
            return _c.ЗакрытьЧекКоррекции(deviceId, кассир, vatin, out checkNumber, out sessionNumber, out fiscalSign);
        }

        #endregion ЧекКоррекции

        public bool ПолучитьТекущееСостояниеРасчетов(string deviceId, out int backlogDocumentsCounter, out int backlogDocumentFirstNumber, out DateTime BacklogDocumentFirstDateTime)
        {
            return _c.ПолучитьТекущееСостояниеРасчетов(deviceId, out backlogDocumentsCounter, out backlogDocumentFirstNumber, out BacklogDocumentFirstDateTime);
        }

        public bool ПолучитьСостояниеКассы(string ИДУстройтва, out int НомерЧека, out int НомерСмены, out int СтатусСмены)
        {
            return _c.ПолучитьСостояниеКассы(ИДУстройтва, out НомерЧека, out НомерСмены, out СтатусСмены);
        }

        public bool ОткрытьСмену105(string DeviceID, string CashierName, string CashierVatin, out int DocumentNumber, out int SessionNumber)
        {
            return _c.ОткрытьСмену105(DeviceID, CashierName, CashierVatin, out DocumentNumber, out SessionNumber);
        }

        public ЧекКоррекции СоздатьЧекКоррекции()
        {
            return _c.СоздатьЧекКоррекции();
        }

        public ЧекКоррекции РаспечататьЧекКоррекции(ЧекКоррекции чек)
        {
            return _c.РаспечататьЧекКоррекции(чек);
        }

        public bool УстановитьСистемуНалогообложения(int TaxVariant)
        {
            return _c.УстановитьСистемуНалогообложения(TaxVariant);
        }

        public void УстановитьРазмерШтрихКода(int size)
        {
            _c.УстановитьРазмерШтрихКода(size);
        }

        public bool ОткрытьЯщик(string id)
        {
            return _c.ОткрытьЯщик(id);
        }

        public int ПечатьКлише(string id)
        {
            return _c.ПечатьКлише(id);
        }

        public string ПолучитьНомерВерсииСервера()
        {
            return _c.ПолучитьНомерВерсииСервера();
        }

        public bool ДокументПечаталсяРанее(string Id)
        {
            return _c.ДокументПечаталсяРанее(Id);
        }

        public bool НапечататьКопиюЧека(string DeviceID)
        {
            return _c.НапечататьКопиюЧека(DeviceID);
        }

        public int НапечататьНефискСтроку77(string DeviceId, string TextString)
        {
            return Convert.ToInt32(_c.НапечататьНефискСтроку(DeviceId, TextString));
        }

        public int НапечататьОтчетБезГашения77(string DeviceId)
        {
            return Convert.ToInt32(_c.НапечататьОтчетБезГашения(DeviceId));
        }

        public int НапечататьОтчетСГашением77(string DeviceId)
        {
            return Convert.ToInt32(_c.НапечататьОтчетСГашением(DeviceId));
        }

        public int НапечататьФискСтроку77(string ИДУстойства, string Наименование, double Количество, double Цена, double Сумма, int Отдел, double НДС)
        {
            return Convert.ToInt32(_c.НапечататьФискСтроку(ИДУстойства, Наименование, Количество, Цена, Сумма, Отдел, НДС));
        }

        public int Отключить77(string DeviceId)
        {
            return Convert.ToInt32(_c.Отключить(DeviceId));
        }

        public int ОткрытьЧек77(string DeviceId, int isFiscal, int isReturn, int CanceleOpenedCheck, out int CheckNumber, out int SessionNumber)
        {
            return Convert.ToInt32(_c.ОткрытьЧек(DeviceId, Convert.ToBoolean(isFiscal), Convert.ToBoolean(isReturn), Convert.ToBoolean(CanceleOpenedCheck), out CheckNumber, out SessionNumber));
        }

        public bool Отрезка(string deviceID)
        {
            return _c.Отрезка(deviceID);
        }

        public string ПолучитьИдентификаторПоИмени(string ИмяУстройства)
        {
            return _c.ПолучитьИдентификаторПоИмени(ИмяУстройства);
        }

        public string[] ПолучитьИменаУстройств()
        {
            return _c.ПолучитьИменаУстройств();
        }

        public bool ПолучитьИнфоПоследнийДокумент(out int Number, out int Session)
        {
            return _c.ПолучитьИнфоПоследнийДокумент(out Number, out Session);
        }

        public string[] ПолучитьСписокИдентификаторовУстройств()
        {
            return _c.ПолучитьСписокИдентификаторовУстройств();
        }

        public ВнесениеВыемка РаспечататьВнесениеВыемка(ВнесениеВыемка выемка)
        {
            return _c.РаспечататьВнесениеВыемка(выемка);
        }

        public Заказ РаспечататьЗаказ(Заказ заказ)
        {
            return _c.РаспечататьЗаказ(заказ);
        }

        [Obsolete]
        public bool РаспечататьИЗакрытьЧек()
        {
            return _c.РаспечататьИЗакрытьЧек();
        }

        public Отчет РаспечататьОтчет(Отчет отчет)
        {
            return _c.РаспечататьОтчет(отчет);
        }

        public ТекстовыйФайл РаспечататьТекст(ТекстовыйФайл текст)
        {
            return _c.РаспечататьТекст(текст);
        }

        public Чек РаспечататьЧек(Чек чек)
        {
            return _c.РаспечататьЧек(чек);
        }

        public ВнесениеВыемка СоздатьВнесениеВыемка()
        {
            return _c.СоздатьВнесениеВыемка();
        }

        public Заказ СоздатьЗаказ()
        {
            return _c.СоздатьЗаказ();
        }

        public Отчет СоздатьОтчет()
        {
            return _c.СоздатьОтчет();
        }

        public ТекстовыйФайл СоздатьТекст()
        {
            return _c.СоздатьТекст();
        }

        public Чек СоздатьЧек()
        {
            return _c.СоздатьЧек();
        }

        public int УстановитьИмяКассира77(string Cashier)
        {
            return Convert.ToInt32(_c.УстановитьИмяКассира(Cashier));
        }

        public Чек ПолучитьЧек(string Id)
        {
            return _c.ПолучитьЧек(Id);
        }

        public double ПолучитьСуммуНаличных(string DeviceId)
        {
            return _c.ПолучитьСуммуНаличных(DeviceId);
        }

        public int Подключить77(out string DeviceId)
        {
            return _c.Подключить77(out DeviceId);
        }

        public bool УстановитьИмяКассира(string Cashier)
        {
            return _c.УстановитьИмяКассира(Cashier);
        }

        public bool УстановитьКонтактКлиента(string contact)
        {
            return _c.УстановитьКонтактКлиента(contact);
        }

        public int УстановитьКонтактКлиента77(string contact)
        {
            return _c.УстановитьКонтактКлиента77(contact);
        }

        public bool ОтчетОТекущемСостоянииРасчетов(string DeviceID)
        {
            return _c.ОтчетОТекущемСостоянииРасчетов(DeviceID);
        }

        public bool ОтчетПоКассирам(string DeviceID)
        {
            return _c.ОтчетПоКассирам(DeviceID);
        }

        public bool ОтчетПоСекциям(string DeviceID)
        {
            return _c.ОтчетПоСекциям(DeviceID);
        }

        public bool НапечататьФискСтроку105(string ИДУстойства, string Наименование, double Количество, double Цена, double Сумма, long Отдел, double НДС, int ПризнакСпособаРасчета, int ПризнакПредметаРасчета)
        {
            return _c.НапечататьФискСтроку105(ИДУстойства, Наименование, Количество, Цена, Сумма, Отдел, НДС, ПризнакСпособаРасчета, ПризнакПредметаРасчета);
        }

        public bool ДобавитьОплаты105(double НаличнаяОплата, double ОплатаЭлектронными, double ОплатаКредитом,
           double Предоплатаой, double Представлением)
        {
            return _c.ДобавитьОплаты105(НаличнаяОплата, ОплатаЭлектронными, ОплатаКредитом, Предоплатаой, Представлением);
        }

        public bool ЗакрытьЧек105(string ИдУстрйоства, string Кассир, string ИннКассира, out int НомерЧека,
          out int НомерСесси, out string ФискальынйПризнак)
        {
            return _c.ЗакрытьЧек105(ИдУстрйоства, Кассир, ИннКассира, out НомерЧека,
          out НомерСесси, out ФискальынйПризнак);
        }

        #endregion Extension

        #region Config

        public string Адрес
        {
            get
            {
                return _c.Адрес;
            }

            set
            {
                _c.Адрес = value;
            }
        }

        public string ИдентификаторУстройства
        {
            get
            {
                return _c.ИдентификаторУстройства;
            }

            set
            {
                _c.ИдентификаторУстройства = value;
            }
        }

        public string Кассир
        {
            get
            {
                return _c.Кассир;
            }

            set
            {
                _c.Кассир = value;
            }
        }

        public int Порт
        {
            get
            {
                return _c.Порт;
            }

            set
            {
                _c.Порт = value;
            }
        }

        public bool СтатусПодключения
        {
            get
            {
                return _c.СтатусПодключения;
            }

            set
            {
                _c.СтатусПодключения = value;
            }
        }

        #endregion Config

        #region Common

        public bool ПолучитьОписание(out string Наименование, out string Описание, out string ТипОборудования, out int РевизияИнтерфейса, out bool ИнтеграционнаяБиблиотека, out bool ОсновнойДрайверУстановлен, out string URLCкачивания)
        {
            Наименование = "AddInRBSoftPrintService";
            Описание = "Внешняя компонента для 1С Сервис печати РБ-Софт";
            ТипОборудования = "ФискальныйРегистратор";
            РевизияИнтерфейса = 2001;
            ИнтеграционнаяБиблиотека = false;
            ОсновнойДрайверУстановлен = true;
            URLCкачивания = "http://www.rbsoft.ru/product/devicenet/download";
            return true;
        }

        public bool Инициализация()
        {
            return _c.Инициализация();
        }

        public bool ВыполнитьДополнительноеДействие(string ИмяДействия)
        {
            return _c.ВыполнитьДополнительноеДействие(ИмяДействия);
        }

        public bool Отключить(string ИДУстройства)
        {
            return _c.Отключить(ИДУстройства);
        }

        public bool Подключить(out string ИдУстройства)
        {
            return _c.Подключить(out ИдУстройства);
        }

        public bool ПолучитьДополнительныеДействия(out string ДополнительныеДействия)
        {
            return _c.ПолучитьДополнительныеДействия(out ДополнительныеДействия);
        }

        public string ПолучитьНомерВерсии()
        {
            return _c.ПолучитьНомерВерсии();
        }

        public long ПолучитьОшибку(out string ОписаниеОшибки)
        {
            return _c.ПолучитьОшибку(out ОписаниеОшибки);
        }

        public bool ПолучитьПараметры(out string ПараметрыДрайвера)
        {
            return _c.ПолучитьПараметры(out ПараметрыДрайвера);
        }

        public bool ТестУстройства(out string РезультатТеста, out string АктивированДемоРежим)
        {
            return _c.ТестУстройства(out РезультатТеста, out АктивированДемоРежим);
        }

        public bool УстановитьПараметр(string ИмяПараметра, object ЗначениеПараметра)
        {
            return _c.УстановитьПараметр(ИмяПараметра, ЗначениеПараметра);
        }

        #endregion Common

        #region fr2001

        public bool ЗакрытьЧек(string ИДУстойства, double НаличнаяОплата, double ОплатаКартой, double ОплатаКредитом, double ОплатаСертификатом)
        {
            return _c.ЗакрытьЧек(ИДУстойства, НаличнаяОплата, ОплатаКартой, ОплатаКредитом, ОплатаСертификатом);
        }

        public bool НапечататьНефискСтроку(string ИДУстойства, string СтрокаТекста)
        {
            return _c.НапечататьНефискСтроку(ИДУстойства, СтрокаТекста);
        }

        public bool НапечататьОтчетБезГашения(string ИДУстойства)
        {
            return _c.НапечататьОтчетБезГашения(ИДУстойства);
        }

        public bool НапечататьОтчетСГашением(string ИДУстойства)
        {
            return _c.НапечататьОтчетСГашением(ИДУстойства);
        }

        public bool НапечататьФискСтроку(string ИДУстойства, string Наименование, double Количество, double Цена, double Сумма, long Отдел, double НДС)
        {
            return _c.НапечататьФискСтроку(ИДУстойства, Наименование, Количество, Цена, Сумма, Отдел, НДС);
        }

        public bool НапечататьЧекВнесенияВыемки(string ИДУстойства, double Сумма)
        {
            return _c.НапечататьЧекВнесенияВыемки(ИДУстойства, Сумма);
        }

        public bool НапечататьШтрихКод(string ИДУстойства, string ТипШтрихкода, string Штрихкод)
        {
            return _c.НапечататьШтрихКод(ИДУстойства, ТипШтрихкода, Штрихкод);
        }

        public bool ОткрытьДенежныйЯщик(string ИДУстойства)
        {
            return _c.ОткрытьДенежныйЯщик(ИДУстойства);
        }

        public bool ОткрытьСмену(string ИДУстойства)
        {
            return _c.ОткрытьСмену(ИДУстойства);
        }

        public bool ОткрытьЧек(string ИДУстойства, bool ФискальныйЧек, bool ЧекВозврата, bool АнулироватьОткрытыйЧек, out int НомерЧека, out int НомерСмены)
        {
            return _c.ОткрытьЧек(ИДУстойства, ФискальныйЧек, ЧекВозврата, АнулироватьОткрытыйЧек, out НомерЧека, out НомерСмены);
        }

        public bool ОтменитьЧек(string ИДУстойства)
        {
            return _c.ОтменитьЧек(ИДУстойства);
        }

        public bool ПолучитьШиринуСтроки(string ИДУстойства, out int ШиринаСтроки)
        {
            return _c.ПолучитьШиринуСтроки(ИДУстойства, out ШиринаСтроки);
        }

        #endregion fr2001

        #region Kostanova

        public int ПолучитьКодОшибки()
        {
            return _c.ПолучитьКодОшибки();
        }

        public string ПолучитьТекстОшибки()
        {
            return _c.ПолучитьТекстОшибки();
        }

        public int РаспечататьЧекАсинхронно(Чек чек)
        {
            return _c.РаспечататьЧекАсинхронно(чек);
        }

        public int УстройствоИсправно(string deviceId)
        {
            return _c.УстройствоИсправно(deviceId);
        }

        public int ПроверитьЧек(string docId)
        {
            return _c.ПроверитьЧек(docId);
        }

        #endregion Kostanova
    }
}