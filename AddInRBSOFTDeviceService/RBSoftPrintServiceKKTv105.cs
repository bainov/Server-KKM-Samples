using ClientPrint;
using ClientPrint.Components;
using System.EnterpriseServices;
using System.Runtime.InteropServices;

#pragma warning disable 1591

namespace AddInRBSoft
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    //   "1С:Предприятие" определяет ProgID COM—объекта компоненты следующим образом:
    //ProgID имеет вид<Vendor>.<Component>;
    //в качестве первой части(<Vendor>) используется строка "AddIn";
    //в качестве второй части(<Component>) используется строка с ID 100 из таблицы строк компоненты.Строка может иметь вид "Name1|Name2|...|NameN", и в этом случае будут созданы все объекты с ProgID вида "AddIn.NameX". Если такая строка отсутствует, то используется имя файла внешней компоненты без расширения.
    // https://its.1c.ru/db/metod8dev#content:3221:hdoc:_com_comp

    [ProgId("Addin." + nameof(RBSoftPrintServiceKKTv105))]
    [Description(nameof(RBSoftPrintServiceKKTv105) + ".Component class")]
    public class RBSoftPrintServiceKKTv105 : Component, IConfigure, ICommon, IKkt2002//, IRbsoftExtension
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

        public RBSoftPrintServiceKKTv105()
        {
            _c = new ClientPr();
        }

        #region Kkt105

        public bool ЗакрытьСмену(string ИДУстройства, string ПараметрыОперации, out string ПараметрыСостояния, out int НомерСмены, out int НомерДокумента)
        {
            return _c.ЗакрытьСмену(ИДУстройства, ПараметрыОперации, out ПараметрыСостояния, out НомерСмены, out НомерДокумента);
        }

        public bool НапечататьОтчетБезГашения(string ИДУстройства, string ПараметрыОперации)
        {
            return _c.НапечататьОтчетБезГашения(ИДУстройства, ПараметрыОперации);
        }

        public bool НапечататьТекстовыйДокумент(string ИДУстройтва, string ДанныеДокумента)
        {
            return _c.НапечататьТекстовыйДокумент(ИДУстройтва, ДанныеДокумента);
        }

        public bool НапечататьЧекВнесенияВыемки(string ИДУстройства, string ПараметрыОперации, double Сумма)
        {
            return _c.НапечататьЧекВнесенияВыемки(ИДУстройства, ПараметрыОперации, Сумма);
        }

        public bool ОперацияФН(string ИДУстройства, long ТипОперации, string ПараметрыФискализацииXML)
        {
            return _c.ОперацияФН(ИДУстройства, ТипОперации, ПараметрыФискализацииXML);
        }

        public bool ОткрытьДенежныйЯщик(string ИДУстройтва)
        {
            return _c.ОткрытьДенежныйЯщик(ИДУстройтва);
        }

        public bool ОткрытьСмену(string ИДУстройства, string ПараметрыОперации,out  string ПараметрыСостояния, out int НомерСмены, out int НомерДокумента)
        {
            return _c.ОткрытьСмену(ИДУстройства, ПараметрыОперации, out ПараметрыСостояния, out НомерСмены, out НомерДокумента);
        }

        public bool ОтчетОТекущемСостоянииРасчетов(string ИДУстройства, string ПараметрыОперации, out string ПараметрыСостояния)
        {
            return _c.ОтчетОТекущемСостоянииРасчетов(ИДУстройства, ПараметрыОперации, out ПараметрыСостояния);
        }

        public bool ПолучитьПараметрыККТ(string ИДУстройства, out string ТаблицаПараметровККТ)
        {
            return _c.ПолучитьПараметрыККТ(ИДУстройства, out ТаблицаПараметровККТ);
        }

        public bool ПолучитьТекущееСостояние(string ИДУстройтва, out int НомерДокумента, out int НомерСмены, out int СтатусСмены, out string ПараметрыСостояния)
        {
            return _c.ПолучитьТекущееСостояние(ИДУстройтва, out НомерДокумента, out НомерСмены, out СтатусСмены, out ПараметрыСостояния);
        }

        public bool ПолучитьШиринуСтроки(string ИДУстройтва, out int ШиринаСтроки)
        {
            return _c.ПолучитьШиринуСтроки(ИДУстройтва, out ШиринаСтроки);
        }

        public bool СформироватьЧек(string ИДУстройства, bool Электронно, string ДанныеЧека, out int НомерДокумента, out int НомерСмены, out string ФискальныйПризнак, out string АдресСайтаПроверки)
        {
            return _c.СформироватьЧек(ИДУстройства, Электронно, ДанныеЧека, out НомерДокумента, out НомерСмены, out ФискальныйПризнак, out АдресСайтаПроверки);
        }

        public bool СформироватьЧекКоррекции(string ИДУстройства, string ДанныеЧека, out int НомерДокумента, out int НомерСмены, out string ФискальныйПризнак, out string АдресСайтаПроверки)
        {
            return _c.СформироватьЧекКоррекции(ИДУстройства, ДанныеЧека, out НомерДокумента, out НомерСмены, out ФискальныйПризнак, out АдресСайтаПроверки);
        }

        #endregion Kkt105

        //#region Extension

        //public void УстановитьРазмерШтрихКода(int size)
        //{
        //    c.УстановитьРазмерШтрихКода(size);
        //}

        //public bool ОткрытьЯщик(string id)
        //{
        //    return c.ОткрытьЯщик(id);
        //}

        //public int ПечатьКлише(string id)
        //{
        //    return c.ПечатьКлише(id);
        //}

        //public string ПолучитьНомерВерсииСервера()
        //{
        //    return c.ПолучитьНомерВерсииСервера();
        //}

        //public bool ДокументПечаталсяРанее(string Id)
        //{
        //    return c.ДокументПечаталсяРанее(Id);
        //}

        //public bool НапечататьКопиюЧека(string DeviceID)
        //{
        //    return c.НапечататьКопиюЧека(DeviceID);
        //}

        //public int НапечататьНефискСтроку77(string ИдУстройства, string TextString)
        //{
        //    return Convert.ToInt32(c.НапечататьНефискСтроку(ИдУстройства, TextString));
        //}

        //public int НапечататьОтчетБезГашения77(string ИдУстройства)
        //{
        //    return Convert.ToInt32(c.НапечататьОтчетБезГашения(ИдУстройства));
        //}

        //public int НапечататьОтчетСГашением77(string ИдУстройства)
        //{
        //    return Convert.ToInt32(c.НапечататьОтчетСГашением(ИдУстройства));
        //}

        //public int НапечататьФискСтроку77(string ИДУстойства, string Наименование, double Количество, double Цена, double Сумма, int Отдел, double НДС)
        //{
        //    return Convert.ToInt32(c.НапечататьФискСтроку(ИДУстойства, Наименование, Количество, Цена, Сумма, Отдел, НДС));
        //}

        //public int Отключить77(string ИдУстройства)
        //{
        //    return Convert.ToInt32(c.Отключить(ИдУстройства));
        //}

        //public int ОткрытьЧек77(string ИдУстройства, int isFiscal, int isReturn, int CanceleOpenedCheck, out int CheckNumber, out int SessionNumber)
        //{
        //    return Convert.ToInt32(c.ОткрытьЧек(ИдУстройства, Convert.ToBoolean(isFiscal), Convert.ToBoolean(isReturn), Convert.ToBoolean(CanceleOpenedCheck), out CheckNumber, out SessionNumber));
        //}

        //public bool Отрезка(string deviceID)
        //{
        //    return c.Отрезка(deviceID);
        //}

        //public string ПолучитьИдентификаторПоИмени(string ИмяУстройства)
        //{
        //    return c.ПолучитьИдентификаторПоИмени(ИмяУстройства);
        //}

        //public string[] ПолучитьИменаУстройств()
        //{
        //    return c.ПолучитьИменаУстройств();
        //}

        //public bool ПолучитьИнфоПоследнийДокумент(out int Number, out int Session)
        //{
        //    return c.ПолучитьИнфоПоследнийДокумент(out Number, out Session);
        //}

        //public string[] ПолучитьСписокИдентификаторовУстройств()
        //{
        //    return c.ПолучитьСписокИдентификаторовУстройств();
        //}

        //public ВнесениеВыемка РаспечататьВнесениеВыемка(ВнесениеВыемка выемка)
        //{
        //    return c.РаспечататьВнесениеВыемка(выемка);
        //}

        //public Заказ РаспечататьЗаказ(Заказ заказ)
        //{
        //    return c.РаспечататьЗаказ(заказ);
        //}

        //public bool РаспечататьИЗакрытьЧек()
        //{
        //    return c.РаспечататьИЗакрытьЧек();
        //}

        //public Отчет РаспечататьОтчет(Отчет отчет)
        //{
        //    return c.РаспечататьОтчет(отчет);
        //}

        //public ТекстовыйФайл РаспечататьТекст(ТекстовыйФайл текст)
        //{
        //    return c.РаспечататьТекст(текст);
        //}

        //public Чек РаспечататьЧек(Чек чек)
        //{
        //    return c.РаспечататьЧек(чек);
        //}

        //public ВнесениеВыемка СоздатьВнесениеВыемка()
        //{
        //    return c.СоздатьВнесениеВыемка();
        //}

        //public Заказ СоздатьЗаказ()
        //{
        //    return c.СоздатьЗаказ();
        //}

        //public ОтменаЧека СоздатьОтменуЧека()
        //{
        //    return c.СоздатьОтменуЧека();
        //}

        //public Отчет СоздатьОтчет()
        //{
        //    return c.СоздатьОтчет();
        //}

        //public ТекстовыйФайл СоздатьТекст()
        //{
        //    return c.СоздатьТекст();
        //}

        //public Чек СоздатьЧек()
        //{
        //    return c.СоздатьЧек();
        //}

        //public bool СравнитьДокументы(Чек check1, Чек check2)
        //{
        //    return c.СравнитьДокументы(check1, check2);
        //}

        //public int УстановитьИмяКассира77(string Cashier)
        //{
        //    return Convert.ToInt32(c.УстановитьИмяКассира(Cashier));
        //}

        //public Чек ПолучитьЧек(string Id)
        //{
        //    return c.ПолучитьЧек(Id);
        //}

        //public double ПолучитьСуммуНаличных(string ИдУстройства)
        //{
        //    return c.ПолучитьСуммуНаличных(ИдУстройства);
        //}

        //public int Подключить77(out string ИдУстройства)
        //{
        //    return c.Подключить77(out ИдУстройства);
        //}

        //public string ИдентификаторОбъекта
        //{
        //    get
        //    {
        //        return c.ИдентификаторОбъекта;
        //    }

        //    set
        //    {
        //        c.ИдентификаторОбъекта = value;
        //    }
        //}

        //public bool УстановитьИмяКассира(string Cashier)
        //{
        //    return c.УстановитьИмяКассира(Cashier);
        //}

        //public bool УстановитьКонтактКлиента(string contact)
        //{
        //    return c.УстановитьКонтактКлиента(contact);
        // }
        //public int УстановитьКонтактКлиента77(string contact)
        //{
        //    return c.УстановитьКонтактКлиента77(contact);
        //}

        //public bool ОтчетОТекущемСостоянииРасчетов(string DeviceID)
        //{
        //    return c.ОтчетОТекущемСостоянииРасчетов(DeviceID);
        //}

        //public bool ОтчетПоКассирам(string DeviceID)
        //{
        //    return c.ОтчетПоКассирам(DeviceID);
        //}

        //public bool ОтчетПоСекциям(string DeviceID)
        //{
        //    return c.ОтчетПоСекциям(DeviceID);
        //}

        //public bool НапечататьФискСтроку105(string ИДУстойства, string Наименование, double Количество, double Цена, double Сумма, long Отдел, double НДС, int ПризнакСпособаРасчета, int ПризнакПредметаРасчета)
        //{
        //    return c.НапечататьФискСтроку105(ИДУстойства, Наименование, Количество, Цена, Сумма, Отдел, НДС, ПризнакСпособаРасчета, ПризнакПредметаРасчета);
        //}
        //#endregion Extension

        #region Configure

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

        #endregion Configure

        #region Common

        public bool ПолучитьОписание(out string Наименование, out string Описание, out string ТипОборудования, out int РевизияИнтерфейса, out bool ИнтеграционнаяБиблиотека, out bool ОсновнойДрайверУстановлен, out string URLCкачивания)
        {
            Наименование = "AddInRBSoftPrintServiceKKTv105";
            Описание = "Внешняя компонента для 1С Сервис печати РБ-Софт";
            ТипОборудования = "ККТ";
            // версии 2.4 соответствует число 2004.
            РевизияИнтерфейса = 2004;
            ИнтеграционнаяБиблиотека = false;
            ОсновнойДрайверУстановлен = true;
            URLCкачивания = "http://www.rbsoft.ru/product/devicenet/download";
            return true;
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
    }
}