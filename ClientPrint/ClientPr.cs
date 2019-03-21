#define FFD105

using ClientPrint.Forms;
using ClientPrint.PrintServiceRef;
using ClientPrint.Документы;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using ClientPrint.Errors;
using static System.String;

#pragma warning disable 1591

namespace ClientPrint
{
    public class ClientPr : IConfigure, IRbsoftExtension, ICommon, IKkt2001, IFr2001, IKkt2002, IKostanova
    {
        private int _barcodeSize;
        private long _errorCode;
        private string _errorString;
        private Settings settings;
        private PrintServiceClient _client;
        private DockBase _lastDocument;
        private Чек _mainCheck;
        private bool _mainCheckIsOpen;
        private string _terminalId;
        private TimeSpan _timeout;

        public bool Log
        {
            get
            {
                return settings.WriteLog;
            }
            set
            {
                settings.WriteLog = value;
            }
        }

        private ЧекКоррекции _corectionCheck;

        private static string VersionLib = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public ClientPr()
        {
            _errorCode = 0;
            _barcodeSize = 0;
            _errorString = "";
            settings = new Settings(PathCommon);
        }

        public string Адрес { get; set; }
        public string ИдентификаторУстройства { get; set; }
        public string Кассир { get; set; } = Empty;

        public string ИдентификаторОбъекта { get; set; }

        public int Порт { get; set; }

        public bool СтатусПодключения { get; set; }

        public static string PathCommon { get; private set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "RB-soft", "Client Server KKM");
        public static string PathLogs { get; private set; } = Path.Combine(PathCommon, "Logs");

        public bool УстановитьСистемуНалогообложения(int taxVariant)
        {
            try
            {
                _mainCheck.ТипНалогообложения = taxVariant;
                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Ошибка_установки_кода_СНО, ex.Message);
                return false;
            }
        }

        public bool Close(string deviceId)
        {
            return Disconnect();
        }

        public bool ПолучитьТекущееСостояниеРасчетов(string deviceId, out int backlogDocumentsCounter,
            out int backlogDocumentFirstNumber, out DateTime backlogDocumentFirstDateTime)
        {
            try
            {
                Guid g = Guid.Parse(ПолучитьИдентификаторПоИмени(deviceId));
                var sp = _client.GetStatusParameters(g);
                backlogDocumentsCounter = (int)sp.Item1;
                backlogDocumentFirstNumber = (int)sp.Item2;
                backlogDocumentFirstDateTime = sp.Item3;
                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                backlogDocumentsCounter = -1;
                backlogDocumentFirstNumber = -1;
                backlogDocumentFirstDateTime = DateTime.MinValue;
                return false;
            }
        }

        public bool PrintReport(Отчет rep)
        {
            // Обертка возвращающая логический результат
            var res = РаспечататьОтчет(rep);

            if (res.КодОшибки == 0) return true;
            else
                SetError(res.КодОшибки, res.ОписаниеОшибки);
            return false;
        }

        public bool ВыполнитьДополнительноеДействие(string actionName)
        {
            // Не предусмотрено дополнительных действий
            // TableActions = "";
            SetError(Error.Не_предусмотрено_дополнительных_действий);
            return false;
        }

        public bool ЗакрытьЧек(string deviceId, double cash, double payByCard, double payByCredit,
            double payByCertificate, double payCashProvision = 0)
        {
            try
            {
                if (!_mainCheckIsOpen)
                {
                    SetError(Error.Чек_не_открыт);
                    return false;
                }

                _mainCheck.ДобавитьОплатуНаличными(cash);
                _mainCheck.ДобавитьОплатуКартой(payByCard);
                _mainCheck.ДобавитьОплатуКредит(payByCredit);
                _mainCheck.ДобавитьОплатуПредоплатой(payByCertificate);
                _mainCheck.ДобавитьОплатуПредставлением(payCashProvision);

                Guid g = Guid.Parse(ПолучитьИдентификаторПоИмени(deviceId));
                var deviceSetting = _client.GetDeviceSettings(g);
                if (deviceSetting.IsElectronically)
                {
                    using (var f = new DialogContact())
                    {
                        f.ClientData = _mainCheck.Check.ClientContact;
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            _mainCheck.Check.ClientContact = f.ClientData;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }

            _mainCheck = РаспечататьЧек(_mainCheck);
            _mainCheckIsOpen = false;
            SetError(_mainCheck);

            return _mainCheck.КодОшибки == 0;
        }

        public bool НапечататьКопиюЧека(string deviceId)
        {
            try
            {
                Guid g = Guid.Parse(ПолучитьИдентификаторПоИмени(deviceId));
                var res = _client.PrintCopy(g);
                if (res.Item1 == 0)
                    return true;
                else
                {
                    SetError(res.Item1, res.Item2);
                    return false;
                }
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool НапечататьНефискСтроку(string deviceId, string textString)
        {
            if (!_mainCheckIsOpen)
            {
                SetError(Error.Чек_не_открыт);
                return false;
            }

            _mainCheck.ДобавитьНеФискальнуюПозицию(textString);
            return true;
        }

        public int НапечататьНефискСтроку77(string deviceId, string textString)
        {
            return Convert.ToInt32(НапечататьНефискСтроку(deviceId, textString));
        }

        public int НапечататьОтчетБезГашения77(string deviceId)
        {
            return Convert.ToInt32(НапечататьОтчетБезГашения(deviceId));
        }

        public bool НапечататьОтчетСГашением(string deviceId)
        {
            var zReport = СоздатьОтчет();
            zReport.Report.ReportType = ReportType.ZОтчет;
            zReport.ИмяКассы = deviceId;

            return PrintReport(zReport);
        }

        public int НапечататьОтчетСГашением77(string deviceId)
        {
            return Convert.ToInt32(НапечататьОтчетСГашением(deviceId));
        }

        public bool НапечататьТекстовыйДокумент(string deviceId, string documentPackage)
        {
            if (Log)
                WriteLog(new List<DataParam>()
                {
                    SetNameMetod(MethodBase.GetCurrentMethod().Name),
                    Set(nameof(deviceId), deviceId),
                    Set(nameof(documentPackage), documentPackage)
                });

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(documentPackage);

                //XmlNode xmlNode = doc.SelectSingleNode("Document/Positions/Barcode");
                //var barCodeType = ParseData.Parse(xmlNode, "BarcodeType");
                //var barCode = ParseData.Parse(xmlNode, "Barcode");
                //if (!string.IsNullOrEmpty(barCodeType) && !string.IsNullOrEmpty(barCode))
                //{
                //    var res = НапечататьШтрихКод(deviceId, barCodeType, barCode);
                //    if (res == false)
                //    {
                //        return false;
                //    }
                //}

                var text = СоздатьТекст();
                var docx = doc.DocumentElement.SelectNodes("Positions");

                var items = new List<CheckItem>();
                foreach (XmlNode nd in docx[0].ChildNodes)
                {
                    if (nd.Name == "TextString")
                    {
                        var strParse = ParseData.Parse(nd, "Text");
                        if (!string.IsNullOrEmpty(strParse))
                        {
                            text.ДобавитьСтроку(strParse);
                        }
                    }
                    else if (nd.Name == "Barcode")
                    {
                        var barcodeType = ParseData.Parse(nd, "BarcodeType");
                        var barcode = ParseData.Parse(nd, "Barcode");

                        if (string.IsNullOrWhiteSpace(barcodeType) ||
                            string.IsNullOrWhiteSpace(barcode))
                            continue;

                        text.ДобавитьШтрихКод(barcodeType, barcode);
                    }
                }

                text.ИмяКассы = deviceId;
                text = РаспечататьТекст(text);
                if (text.TextFile.ResultCode != 0)
                {
                    SetError(text.TextFile.ResultCode, text.TextFile.ResultDescription);
                    return false;
                }

                //  ПечатьКлише(deviceID);
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }

            return true;
        }

        public bool НапечататьФискСтроку(string deviceId, string name, double quantity, double price, double amount,
            long department, double tax)
        {
            return НапечататьФискСтроку105(deviceId, name, quantity, price, amount, department, tax, 0, 0);
        }

        public int НапечататьФискСтроку77(string deviceId, string name, double quantity, double price, double amount,
            int department, double tax)
        {
            return Convert.ToInt32(НапечататьФискСтроку(deviceId, name, quantity, price, amount, department, tax));
        }

        public bool НапечататьШтрихКод(string deviceId, string barCodeType, string barCode)
        {
            if (!_mainCheckIsOpen)
            {
                SetError(Error.Чек_не_открыт);
                return false;
            }

            var res = _mainCheck.ДобавитьШтрихКод(barCodeType, barCode);
            if (!res)
                SetError(Error.Пустые_данные_штрих_кода);

            return res;
        }

        public bool ОперацияФН(string deviceId, long operationType, string parametersFiscal)
        {
            if (Log)
                WriteLog(new List<DataParam>()
                {
                    SetNameMetod(MethodBase.GetCurrentMethod().Name),
                    Set(nameof(deviceId), deviceId),
                    Set(nameof(operationType), operationType),
                    Set(nameof(parametersFiscal), parametersFiscal)
                });
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(parametersFiscal);

                ПараметрыФискализации paramFiscal = new ПараметрыФискализации();
                paramFiscal.ParamFiscal.OperationType = (int)operationType;
                XmlNode node = doc.DocumentElement;
                paramFiscal.ParamFiscal.Cashier = ParseData.Parse(node, "CashierName");
                paramFiscal.ParamFiscal.CashierVATIN = ParseData.Parse(node, "CashierVATIN");
                paramFiscal.ParamFiscal.KKTNumber = ParseData.Parse(node, "KKTNumber");
                paramFiscal.ParamFiscal.OrganizationName = ParseData.Parse(node, "OrganizationName");
                paramFiscal.ParamFiscal.VATIN = ParseData.Parse(node, "VATIN");
                paramFiscal.ParamFiscal.AddressSettle = ParseData.Parse(node, "AddressSettle");
                paramFiscal.ParamFiscal.PlaceSettle = ParseData.Parse(node, "PlaceSettle");
                paramFiscal.ParamFiscal.TaxVariant = ParseData.Parse(node, "TaxVariant");
                paramFiscal.ParamFiscal.OfflineMode = ParseData.ParseBool(node, "OfflineMode");
                paramFiscal.ParamFiscal.DataEncryption = ParseData.ParseBool(node, "DataEncryption");
                paramFiscal.ParamFiscal.ServiceSign = ParseData.ParseBool(node, "ServiceSign");
                paramFiscal.ParamFiscal.SaleExcisableGoods = ParseData.ParseBool(node, "SaleExcisableGoods");
                paramFiscal.ParamFiscal.SignOfGambling = ParseData.ParseBool(node, "SignOfGambling");
                paramFiscal.ParamFiscal.SignOfLottery = ParseData.ParseBool(node, "SignOfLottery");
                paramFiscal.ParamFiscal.SignOfAgent = ParseData.Parse(node, "SignOfAgent");
                paramFiscal.ParamFiscal.BSOSing = ParseData.ParseBool(node, "BSOSing");
                paramFiscal.ParamFiscal.CalcOnlineSign = ParseData.ParseBool(node, "CalcOnlineSign");
                paramFiscal.ParamFiscal.PrinterAutomatic = ParseData.ParseBool(node, "PrinterAutomatic");
                paramFiscal.ParamFiscal.AutomaticMode = ParseData.ParseBool(node, "AutomaticMode");
                paramFiscal.ParamFiscal.AutomaticNumber = ParseData.Parse(node, "AutomaticNumber");
                paramFiscal.ParamFiscal.ReasonCode = ParseData.ParseInt(node, "ReasonCode");
                paramFiscal.ParamFiscal.InfoChangesReasonsCodes = ParseData.Parse(node, "InfoChangesReasonsCodes");
                paramFiscal.ParamFiscal.OFDOrganizationName = ParseData.Parse(node, "OFDOrganizationName");
                paramFiscal.ParamFiscal.OFDVATIN = ParseData.Parse(node, "OFDVATIN");
                paramFiscal.ParamFiscal.FNSWebSite = ParseData.Parse(node, "FNSWebSite");
                paramFiscal.ParamFiscal.SenderEmail = ParseData.Parse(node, "SenderEmail");
                paramFiscal.ИмяКассы = deviceId;

                var resPrint = PrintOperationFN(paramFiscal);
                if (resPrint.КодОшибки == 0)
                {
                    return true;
                }
                else
                {
                    SetError(paramFiscal);
                    return false;
                }
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool ОперацияФН(string deviceId, long operationType, string cashierName, string parametersFiscal)
        {
            if (Log)
                WriteLog(new List<DataParam>()
                {
                    SetNameMetod(MethodBase.GetCurrentMethod().Name),
                    Set(nameof(deviceId), deviceId),
                    Set(nameof(operationType), operationType),
                    Set(nameof(parametersFiscal), parametersFiscal)
                });
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(parametersFiscal);

                ПараметрыФискализации paramFiscal = new ПараметрыФискализации();
                paramFiscal.ParamFiscal.OperationType = (int)operationType;
                XmlNode node = doc.DocumentElement;
                paramFiscal.ParamFiscal.Cashier = ParseData.Parse(node, "CashierName");
                paramFiscal.ParamFiscal.CashierVATIN = ParseData.Parse(node, "CashierVATIN");
                paramFiscal.ParamFiscal.KKTNumber = ParseData.Parse(node, "KKTNumber");
                paramFiscal.ParamFiscal.OrganizationName = ParseData.Parse(node, "OrganizationName");
                paramFiscal.ParamFiscal.VATIN = ParseData.Parse(node, "VATIN");
                paramFiscal.ParamFiscal.AddressSettle = ParseData.Parse(node, "AddressSettle");
                paramFiscal.ParamFiscal.PlaceSettle = ParseData.Parse(node, "PlaceSettle");
                paramFiscal.ParamFiscal.TaxVariant = ParseData.Parse(node, "TaxVariant");
                paramFiscal.ParamFiscal.OfflineMode = ParseData.ParseBool(node, "OfflineMode");
                paramFiscal.ParamFiscal.DataEncryption = ParseData.ParseBool(node, "DataEncryption");
                paramFiscal.ParamFiscal.ServiceSign = ParseData.ParseBool(node, "ServiceSign");
                paramFiscal.ParamFiscal.SaleExcisableGoods = ParseData.ParseBool(node, "SaleExcisableGoods");
                paramFiscal.ParamFiscal.SignOfGambling = ParseData.ParseBool(node, "SignOfGambling");
                paramFiscal.ParamFiscal.SignOfLottery = ParseData.ParseBool(node, "SignOfLottery");
                paramFiscal.ParamFiscal.SignOfAgent = ParseData.Parse(node, "SignOfAgent");
                paramFiscal.ParamFiscal.BSOSing = ParseData.ParseBool(node, "BSOSing");
                paramFiscal.ParamFiscal.CalcOnlineSign = ParseData.ParseBool(node, "CalcOnlineSign");
                paramFiscal.ParamFiscal.PrinterAutomatic = ParseData.ParseBool(node, "PrinterAutomatic");
                paramFiscal.ParamFiscal.AutomaticMode = ParseData.ParseBool(node, "AutomaticMode");
                paramFiscal.ParamFiscal.AutomaticNumber = ParseData.Parse(node, "AutomaticNumber");
                paramFiscal.ParamFiscal.ReasonCode = ParseData.ParseInt(node, "ReasonCode");
                paramFiscal.ParamFiscal.InfoChangesReasonsCodes = ParseData.Parse(node, "InfoChangesReasonsCodes");
                paramFiscal.ParamFiscal.OFDOrganizationName = ParseData.Parse(node, "OFDOrganizationName");
                paramFiscal.ParamFiscal.OFDVATIN = ParseData.Parse(node, "OFDVATIN");
                paramFiscal.ParamFiscal.FNSWebSite = ParseData.Parse(node, "FNSWebSite");
                paramFiscal.ParamFiscal.SenderEmail = ParseData.Parse(node, "SenderEmail");
                paramFiscal.ИмяКассы = deviceId;
                var resPrint = PrintOperationFN(paramFiscal);
                if (resPrint.КодОшибки == 0)
                {
                    return true;
                }
                else
                {
                    SetError(paramFiscal);
                    return false;
                }
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool Отключить(string deviceId)
        {
            return Disconnect();
        }

        public int Отключить77(string deviceId)
        {
            return Convert.ToInt32(Disconnect());
        }

        public bool ОткрытьДенежныйЯщик(string deviceId)
        {
            try
            {
                return ОткрытьЯщик(ПолучитьИдентификаторПоИмени(deviceId));
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool ОткрытьЧек(string deviceId, bool isFiscal, bool isReturn, bool canceleOpenedCheck,
            out int checkNumber, out int sessionNumber)
        {
            try
            {
                string device = deviceId;
                var deviceStatus =
                    _client.GetDeviceStatus(Guid.Parse(ПолучитьИдентификаторПоИмени(deviceId))) as AtolFRStatus;
                if (deviceStatus != null && deviceStatus.DeviceStatus != 0)
                {
                    checkNumber = 0;
                    sessionNumber = 0;
                    SetError(deviceStatus.DeviceStatus, deviceStatus.DeviceStatusDescription);
                    return false;
                }

                checkNumber = deviceStatus.CheckNumber + 1;
                sessionNumber = deviceStatus.SessionNumber;
                if ((_mainCheckIsOpen) && (canceleOpenedCheck))
                {
                    _mainCheckIsOpen = false;
                }

                return OpenCheck(device, isFiscal, Convert.ToBoolean(isReturn), canceleOpenedCheck);
            }
            catch (Exception ex)
            {
                checkNumber = 0;
                sessionNumber = 0;
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public int ОткрытьЧек77(string deviceId, int isFiscal, int isReturn, int canceleOpenedCheck,
            out int checkNumber, out int sessionNumber)
        {
            return Convert.ToInt32(ОткрытьЧек(deviceId, Convert.ToBoolean(isFiscal), Convert.ToBoolean(isReturn),
                Convert.ToBoolean(canceleOpenedCheck), out checkNumber, out sessionNumber));
        }

        public bool ОтменитьЧек(string deviceId)
        {
            _mainCheckIsOpen = false;
            return true;
        }

        public bool Подключить(out string идУстройства)
        {
            идУстройства = ИдентификаторУстройства;
            return Connect();
        }

        public int Подключить77(out string deviceId)
        {
            deviceId = ИдентификаторУстройства;
            return Convert.ToInt32(Connect());
        }

        public bool ПолучитьДополнительныеДействия(out string tableActions)
        {
            tableActions = "";
            return true;
        }

        public bool ПолучитьИнфоПоследнийДокумент(out int number)
        {
            int session;
            return ПолучитьИнфоПоследнийДокумент(out number, out session);
        }

        public bool ПолучитьИнфоПоследнийДокумент(out int number, out int session)
        {
            try
            {
                number = _lastDocument.НомерЧека;
                session = _lastDocument.НомерСмены;
            }
            catch (Exception ex)
            {
                number = 0;
                session = 0;
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }

            return true;
        }

        public string ПолучитьНомерВерсииСервера()
        {
            try
            {
                return _client.GetServerVersion();
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return Empty;
            }
        }

        public bool ПолучитьПараметрыККТ(string deviceId, out string tableParametersKkt)
        {
            try
            {
                var parameters = _client.GetDeviceKKTParameters(Guid.Parse(ПолучитьИдентификаторПоИмени(deviceId)));
                tableParametersKkt = MakeXMLTableParametersKKT(parameters);
                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                tableParametersKkt = "";
                return false;
            }
        }

        public bool ПолучитьТекущееСостояние(string deviceId, out int checkNumber, out int sessionNumber,
            out int sessionState, out string statusParameters)
        {
            try
            {
                Guid g = Guid.Parse(ПолучитьИдентификаторПоИмени(deviceId));
                var deviceStatus = _client.GetDeviceStatus(g) as AtolFRStatus;
                var kkmStatusParameters = _client.GetStatusParameters(g);

                sessionNumber = deviceStatus.SessionNumber;
                sessionState = deviceStatus.SessionExceedLimit ? 3 : deviceStatus.SessionOpened ? 2 : 1;
                checkNumber = deviceStatus.CheckNumber;
                statusParameters = MakeXMLStatusParameters(kkmStatusParameters.Item1, kkmStatusParameters.Item2,
                    kkmStatusParameters.Item3);
                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                sessionNumber = 0;
                sessionState = 0;
                checkNumber = 0;
                statusParameters = "";
                return false;
            }
        }

        public bool ПолучитьШиринуСтроки(string deviceId, out int lineLength)
        {
            try
            {
                int length = _client.GetCharLength(Guid.Parse(ПолучитьИдентификаторПоИмени(deviceId)));
                lineLength = length;
                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                lineLength = 40;
                return false;
            }
        }

        public bool СформироватьЧекКоррекции(string deviceId, string checkPackage, out int checkNumber,
            out int sessionNumber, out string fiscalSign, out string addressSiteInspection)
        {
            if (Log)
                WriteLog(new List<DataParam>()
                {
                    SetNameMetod(MethodBase.GetCurrentMethod().Name),
                    Set(nameof(deviceId), deviceId),
                    Set(nameof(checkPackage), checkPackage)
                });
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(checkPackage);
                // Параметры
                var nodeParams = doc.DocumentElement.SelectSingleNode("Parameters");
                int paymentType = ParseData.ParseInt(nodeParams, "PaymentType");
                TaxVariants taxVariant = ParseData.ParseIntTax(nodeParams, "TaxVariant");
                string cashierName = ParseData.Parse(nodeParams, "CashierName");
                string cashierVatin = ParseData.Parse(nodeParams, "CashierVATIN");

                int correctionType = ParseData.ParseInt(nodeParams, "CorrectionType");
                string сorrectionBaseName = ParseData.Parse(nodeParams, "СorrectionBaseName");
                string сorrectionBaseDate = ParseData.Parse(nodeParams, "СorrectionBaseDate");
                string сorrectionBaseNumber = ParseData.Parse(nodeParams, "СorrectionBaseNumber");

                double sumTaxNone = ParseData.ParseDouble(nodeParams, "SumTAXNone");
                double sumTax0 = ParseData.ParseDouble(nodeParams, "SumTAX0");
                double sumTax10 = ParseData.ParseDouble(nodeParams, "SumTAX10");
                double sumTax18 = ParseData.ParseDouble(nodeParams, "SumTAX18");
                double sumTax110 = ParseData.ParseDouble(nodeParams, "SumTAX110");
                double sumTax118 = ParseData.ParseDouble(nodeParams, "SumTAX118");

                // Оплата
                var nodePayments = doc.DocumentElement.SelectSingleNode("Payments");
                double cashPayment = ParseData.ParseDouble(nodePayments, "Cash");
                double electronicPayment = ParseData.ParseDouble(nodePayments, "ElectronicPayment");
                double credit = ParseData.ParseDouble(nodePayments, "Credit");
                double advancePayment = ParseData.ParseDouble(nodePayments, "AdvancePayment");
                double cashProvision = ParseData.ParseDouble(nodePayments, "CashProvision");

                // Заполнение параметров

                // 1С передает
                // 1 - Приход
                // 2 - Возврат прихода
                // 3 - Расход
                // 4 - Возврат расхода

                // Тип чека в Сервере ККМ
                // ЧекКоррекцииПрихода = 7,
                // ЧекКоррекцииВозвратаПрихода = 8,
                // ЧекКоррекцииРасхода = 9,
                // ЧекКоррекцииВозвратаРасхода = 10,

                // Приведение к нужному типу чека, который ожидает Сервер ККМ.
                var check = new CheckCorrection
                {
                    DockId = Guid.NewGuid(),
                    TerminalId = _terminalId,
                    CheckType = (CheckTypes)(paymentType + 6)
                };

                switch (check.CheckType)
                {
                    case CheckTypes.ЧекКоррекцииПрихода:
                    case CheckTypes.ЧекКоррекцииВозвратаПрихода:
                    case CheckTypes.ЧекКоррекцииРасхода:
                    case CheckTypes.ЧекКоррекцииВозвратаРасхода:
                        // Данные верные
                        break;

                    default:
                        SetError(Error.Передан_неверный_тип_расчета);

                        SetDefaultValue(out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);
                        return false;
                }

                var format = "yyyy-MM-ddThh:mm:ss";
                try
                {
                    check.CorrectionBaseDate =
                        DateTime.ParseExact(сorrectionBaseDate, format, CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    check.CorrectionBaseDate = DateTime.Now;
                }
                catch (ArgumentException)
                {
                    check.CorrectionBaseDate = DateTime.Now;
                }

                check.Cashier = cashierName;
                check.CashierVATIN = cashierVatin;
                check.СorrectionType = (CorrectionTypes)correctionType;
                check.CorrectionBaseNumber = сorrectionBaseNumber;
                check.CorrectionBaseName = сorrectionBaseName;
                check.TaxType = taxVariant;
                //Sum
                check.TaxSumNone = (decimal)sumTaxNone;
                check.TaxSum0 = (decimal)sumTax0;
                check.TaxSum10 = (decimal)sumTax10;
                check.TaxSum18 = (decimal)sumTax18;
                check.TaxSum118 = (decimal)sumTax118;
                check.TaxSum110 = (decimal)sumTax110;

                check.CashPayment = new Payment()
                {
                    Summ = (decimal)cashPayment,
                    TypeClose = PayTypes.Наличные
                };
                check.ElectronicPayment = new Payment()
                {
                    Summ = (decimal)electronicPayment,
                    TypeClose = PayTypes.Электронными
                };

                check.CreditPayment = new Payment()
                {
                    Summ = (decimal)credit,
                    TypeClose = PayTypes.Кредит
                };
                check.AdvancePayment = new Payment()
                {
                    Summ = (decimal)advancePayment,
                    TypeClose = PayTypes.Предоплата
                };
                check.CashProvisionPayment = new Payment()
                {
                    Summ = (decimal)cashProvision,
                    TypeClose = PayTypes.Представление
                };

                using (var dlgCheckCorrection = new DialogCheckCorrection(check))
                {
                    if (dlgCheckCorrection.ShowDialog() == DialogResult.OK)
                    {
                        check = dlgCheckCorrection.Check;
                    }
                    else
                    {
                        SetError(Error.Отменено_пользователем);
                        SetDefaultValue(out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);
                        return false;
                    }
                }

                try
                {
                    check.DeviceId = Guid.Parse(ПолучитьИдентификаторПоИмени(deviceId));
                    check = SavePrintCorrecion(check);
                }
                catch (Exception ex)
                {
                    SetError(Error.Внутренняя_ошибка, ex.Message);
                    SetDefaultValue(out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);
                    return false;
                }

                if (check.ResultCode != 0)
                {
                    SetError(check.ResultCode, check.ResultDescription);
                    SetDefaultValue(out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);
                    return false;
                }
                else
                {
                    sessionNumber = check.SessionNumber;
                    checkNumber = check.DocNumber;
                    fiscalSign = check.FiscalSign;
                    // Заглушка, для соответствия стандарту 1С
                    addressSiteInspection = "nalog.ru";
                    return true;
                }
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                SetDefaultValue(out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);
                return false;
            }
        }

        private static void SetDefaultValue(out int checkNumber, out int sessionNumber, out string fiscalSign,
            out string addressSiteInspection)
        {
            checkNumber = 0;
            sessionNumber = 0;
            addressSiteInspection = "";
            fiscalSign = "";
        }

        public bool СформироватьЧекКоррекции(string deviceId, string cashierName, string checkPackage,
            out int checkNumber, out int sessionNumber, out string fiscalSign, out string addressSiteInspection)
        {
            if (Log)
                WriteLog(new List<DataParam>()
                {
                    SetNameMetod(MethodBase.GetCurrentMethod().Name),
                    Set(nameof(deviceId), deviceId),
                    Set(nameof(cashierName), cashierName),
                    Set(nameof(checkPackage), checkPackage)
                });

            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(checkPackage);
                // Параметры
                var nodeParams = doc.DocumentElement.SelectSingleNode("Parameters");
                int paymentType = ParseData.ParseInt(nodeParams, "PaymentType");
                TaxVariants taxVariant = ParseData.ParseIntTax(nodeParams, "TaxVariant");
                //string CashierName = ParseData.Parse(nodeParams, "CashierName");

                int correctionType = ParseData.ParseInt(nodeParams, "CorrectionType");
                string сorrectionBaseName = ParseData.Parse(nodeParams, "СorrectionBaseName");
                string сorrectionBaseDate = ParseData.Parse(nodeParams, "СorrectionBaseDate");
                string сorrectionBaseNumber = ParseData.Parse(nodeParams, "СorrectionBaseNumber");

                //2001 не передает эти данные
                //double Sum = ParseData.ParseDouble(nodeParams, "Sum");
                //double SumTaxNone = ParseData.ParseDouble(nodeParams, "SumTAXNone");
                //double SumTax0 = ParseData.ParseDouble(nodeParams, "SumTAX0");
                //double SumTax10 = ParseData.ParseDouble(nodeParams, "SumTAX10");
                //double SumTax18 = ParseData.ParseDouble(nodeParams, "SumTAX18");
                //double SumTax110 = ParseData.ParseDouble(nodeParams, "SumTAX110");
                //double SumTax118 = ParseData.ParseDouble(nodeParams, "SumTAX118");

                // Оплата
                var nodePayments = doc.DocumentElement.SelectSingleNode("Payments");
                double cashPayment = ParseData.ParseDouble(nodePayments, "Cash");
                double electronicPayment = ParseData.ParseDouble(nodePayments, "CashLessType1");
                double credit = ParseData.ParseDouble(nodePayments, "CashLessType2");
                double advancePayment = ParseData.ParseDouble(nodePayments, "CashLessType3");

                // Заполнение параметров
                // 1С передает
                // 1 - Приход
                // 2 - Возврат прихода
                // 3 - Расход
                // 4 - Возврат расхода

                // Тип чека в Сервере ККМ
                // ЧекКоррекцииПрихода = 7,
                // ЧекКоррекцииВозвратаПрихода = 8,
                // ЧекКоррекцииРасхода = 9,
                // ЧекКоррекцииВозвратаРасхода = 10,

                // Приведение к нужному типу чека, который ожидает сервер ККМ.
                var check = new CheckCorrection();
                check.DockId = Guid.NewGuid();
                check.TerminalId = _terminalId;

                check.CheckType = (CheckTypes)(paymentType + 6);
                switch (check.CheckType)
                {
                    case CheckTypes.ЧекКоррекцииПрихода:
                    case CheckTypes.ЧекКоррекцииВозвратаПрихода:
                    case CheckTypes.ЧекКоррекцииРасхода:
                    case CheckTypes.ЧекКоррекцииВозвратаРасхода:
                        // Данные верные
                        break;

                    default:
                        SetError(Error.Передан_неверный_тип_расчета);
                        SetDefaultValue(out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);
                        return false;
                }

                var format = "yyyy-MM-ddThh:mm:ss";
                try
                {
                    check.CorrectionBaseDate =
                        DateTime.ParseExact(сorrectionBaseDate, format, CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    check.CorrectionBaseDate = DateTime.Now;
                }
                catch (ArgumentException)
                {
                    check.CorrectionBaseDate = DateTime.Now;
                }

                check.Cashier = cashierName;
                check.СorrectionType = (CorrectionTypes)correctionType;
                check.CorrectionBaseNumber = сorrectionBaseNumber;
                check.CorrectionBaseName = сorrectionBaseName;
                check.TaxType = taxVariant;

                check.CashPayment = new Payment()
                {
                    Summ = (decimal)cashPayment,
                    TypeClose = PayTypes.Наличные
                };
                check.ElectronicPayment = new Payment()
                {
                    Summ = (decimal)electronicPayment,
                    TypeClose = PayTypes.Электронными
                };

                check.CreditPayment = new Payment()
                {
                    Summ = (decimal)credit,
                    TypeClose = PayTypes.Кредит
                };
                check.AdvancePayment = new Payment()
                {
                    Summ = (decimal)advancePayment,
                    TypeClose = PayTypes.Предоплата
                };

                // Недостающие данные вводятся в дополнительной форме
                using (var dlgCheckCorrection = new DialogCheckCorrection(check))
                {
                    if (dlgCheckCorrection.ShowDialog() == DialogResult.OK)
                    {
                        check = dlgCheckCorrection.Check;
                    }
                    else
                    {
                        SetError(Error.Отменено_пользователем);
                        SetDefaultValue(out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);
                        return false;
                    }
                }

                try
                {
                    check.DeviceId = Guid.Parse(ПолучитьИдентификаторПоИмени(deviceId));
                    check = SavePrintCorrecion(check);
                }
                catch (Exception ex)
                {
                    SetError(Error.Внутренняя_ошибка, ex.Message);
                    SetDefaultValue(out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);
                    return false;
                }

                if (check.ResultCode != 0)
                {
                    SetError(check.ResultCode, check.ResultDescription);
                    SetDefaultValue(out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);
                    return false;
                }
                else
                {
                    sessionNumber = check.SessionNumber;
                    checkNumber = check.DocNumber;
                    fiscalSign = check.FiscalSign;
                    // Заглушка, для соответствия стандарту 1С
                    addressSiteInspection = "nalog.ru";
                    return true;
                }
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                SetDefaultValue(out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);
                return false;
            }
        }

        public bool ТестУстройства(out string description, out string demoIsActivated)
        {
            return ConnectionDeviceTest(out description, out demoIsActivated);
        }

        public bool УстановитьИмяКассира(string cashier)
        {
            try
            {
                _mainCheck.Кассир = cashier;
                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool УстановитьИннКассира(string иннКассира)
        {
            try
            {
                _mainCheck.ИннКассира = иннКассира;
                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool УстановитьПарольКассира(string password)
        {
            try
            {
                _mainCheck.ПарольКассира = password;
                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public int УстановитьИмяКассира77(string cashier)
        {
            return УстановитьИмяКассира(cashier) ? 1 : 0;
        }

        public bool УстановитьПараметр(string name, object value)
        {
            if (value == null)
            {
                SetError(Error.Значение_параметра_не_может_быть_пустым);
                return false;
            }

            if (IsNullOrEmpty(name))
            {
                SetError(Error.Имя_параметра_не_может_быть_пустым);
                return false;
            }
            // не исправлять Adress на верный вариант Address, т.к. это может сломать настройку оборудования 1С при обновлении
            if (Compare(name, "Adress", StringComparison.OrdinalIgnoreCase) == 0)
            {
                try
                {
                    Адрес = Convert.ToString(value);
                }
                catch (Exception ex)
                {
                    SetError(Error.Ошибка_преобразования_параметра, ex.Message);
                    return false;
                }
            }
            else if (Compare(name, "Port", StringComparison.OrdinalIgnoreCase) == 0)
            {
                try
                {
                    Порт = Convert.ToInt32(value);
                }
                catch (Exception ex)
                {
                    SetError(Error.Ошибка_преобразования_параметра, ex.Message);
                    return false;
                }
            }
            else if (Compare(name, "DeviceId", StringComparison.OrdinalIgnoreCase) == 0)
            {
                try
                {
                    ИдентификаторУстройства = Convert.ToString(value);
                }
                catch (Exception ex)
                {
                    SetError(Error.Ошибка_преобразования_параметра, ex.Message);
                    return false;
                }
            }
            else
            {
                SetError(Error.Неизвестный_параметр, "<" + name + "> со значением <" + value + ">");
                return false;
            }

            return true;
        }

        public void УстановитьРазмерШтрихКода(int size)
        {
            if (size < 0) return;

            _barcodeSize = size;
        }

        private static void WriteLog(List<DataParam> args)
        {
            //Пример вызова
            // SendData(new List<D>() {
            //   new D() {Name = nameof(Mouse), Data = Mouse },
            //   new D() { Name = nameof(count), Data = count },
            //   new D() { Name = nameof(Сумма), Data = Сумма }});

            var data = $"\n{{ \n\t\"customDate\":\"{DateTime.Now}\",";
            var count = args.Count;
            for (var i = 0; i < count; i++)
            {
                data += $"\n\t\"{args[i].Name}\":\"{args[i].Data}\"";
                if (i != count - 1)
                    data += ",";
            }

            data += "\n},";

            if (!Directory.Exists(PathLogs))
            {
                Directory.CreateDirectory(PathLogs);
            }

            var dayNameDirectory = DateTime.Now.ToShortDateString() + ".txt";
            var fileLogPath = Path.Combine(PathLogs, dayNameDirectory);
            using (var writer = File.AppendText(fileLogPath))
            {
                writer.WriteLine(data);
            }
        }

        private bool Connect()
        {
            СтатусПодключения = false;
            try
            {
                _client = new PrintServiceClient(GetBinding(),
                    new EndpointAddress($"http://{Адрес}:{Порт}/PrintService/"));
                //var timeout = TimeOut.GetTimeOut();
                //client.Endpoint.Binding.SendTimeout = timeout;
                //client.Endpoint.Binding.ReceiveTimeout = timeout;
                //client.Endpoint.Binding.OpenTimeout = timeout;
                //client.Endpoint.Binding.CloseTimeout = timeout;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                СтатусПодключения = false;
                return СтатусПодключения;
            }

            MakeTerminalId();
            try
            {
                СтатусПодключения = _client.Connect();
                if (СтатусПодключения)
                {
                    var ti = _client.GetTimeOut();
                    if (ti != _timeout)
                    {
                        TimeOut.SetTimeOut(ti);
                    }
                }
            }
            catch (EndpointNotFoundException)
            {
                SetError(Error.Нет_подключения_к_Cерверу_ККМ,
                    "Проверьте подключение к Серверу ККМ, для проверки связи можете использовать инструмент 'TestServerKKM.exe'");
                СтатусПодключения = false;
            }
            catch (WebException)
            {
                SetError(Error.Нет_подключения_к_Cерверу_ККМ,
                    "Проверьте подключение к Серверу ККМ, для проверки связи можете использовать инструмент 'TestServerKKM.exe'");
                СтатусПодключения = false;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                СтатусПодключения = false;
            }

            return СтатусПодключения;
        }

        private bool ConnectionDeviceTest(out string description, out string demoIsActivated)
        {
            bool result = Connect();
            description = "";
            demoIsActivated = "";
            if (result)
            {
                Guid g;
                try
                {
                    g = Guid.Parse(ПолучитьИдентификаторПоИмени(ИдентификаторУстройства));
                    var deviceSetting = _client.GetDeviceSettings(g);
                    //_client.UpdateKKM(deviceSetting);
                }
                catch (Exception ex)
                {
                    SetError(Error.Внутренняя_ошибка, ex.Message);
                    string listDevicesNames;
                    try
                    {
                        listDevicesNames = "\nСписок устройств на сервере:";
                        var deviceSettings = _client.GetDevices();
                        foreach (var item in deviceSettings)
                        {
                            listDevicesNames += "\n\t" + item.DeviceName;
                        }

                        listDevicesNames += "\n";
                    }
                    catch (Exception)
                    {
                        listDevicesNames = Empty;
                    }

                    description = "Подключение к Серверу ККМ прошло успешно \n" + _errorString + listDevicesNames;
                    return false;
                }

                try
                {
                    var status = (AtolFRStatus)_client.GetDeviceStatus(g);
                    description =
                        $"Имя устройства: {ИдентификаторУстройства} Статус: {(status.DriverVersion == Empty ? "Нет подключения" : "Подключен")}";
                    var statusMsg = status.DeviceStatus == 0 ? "Подключено" : "Нет подключения";
                    var sessionOpened = status.SessionOpened ? "Открыта" : "Закрыта";
                    var over24Hour = status.SessionExceedLimit ? "Да" : "Нет";

                    // Основное описание
                    description =
                        $"Имя устройства: {ИдентификаторУстройства}, подключение к Серверу ККМ прошло успешно\n";

                    // Дополнительное описание в случае успешного подключения
                    if (status.DeviceStatus == 0)
                    {
                        description +=
                            $"Статус устройства: {statusMsg}, Смена: {sessionOpened}, 24 часа истекли: {over24Hour}";
                    }
                    // Дополнительное описание при статусе !=0
                    else
                    {
                        description +=
                            $"Статус устройства: {statusMsg}, Код: {status.DeviceStatus}, Описание: {status.DeviceStatusDescription}";
                        SetError(Error.Ошибка_во_время_тестирования_устройства, description);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    SetError(Error.Ошибка_во_время_тестирования_устройства, ex.Message);

                    return false;
                }

                try
                {
                    if (!_client.IsLicenseAvailable())
                    {
                        demoIsActivated =
                            "Сервер ККМ работает в режиме демонстрации. Вам доступна печать ограниченного числа документов," +
                            " а также при печати чеков будет отображаться строка, сигнализирующая о работе программы в режиме демонстрации.";
                    }
                    else
                    {
                        demoIsActivated = "Лицензия установлена";
                    }
                }
                catch (Exception ex)
                {
                    SetError(Error.Ошибка_при_проверке_лицензии, ex.Message);
                    demoIsActivated = "Произошла " + Error.Ошибка_при_проверке_лицензии.GetDescription();
                }
            }
            else
            {
                demoIsActivated = "Невозможно проверить лицензию";
                //SetError(1, "Ошибка подключения к Серверу ККМ. "+ DemoIsActivated);
                description = _errorString +
                              $"\nПроверьте правильность ввода IP-адреса и открыт ли порт {Порт} на сервере и клиенте. \nПроверьте состояние службы 'RBSOFT DeviceNet 3.0 WCF'. Служба должна выполняться.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Создание атрибута элемента XML
        /// </summary>
        /// <param name="doc">XML документ для создания атрибута</param>
        /// <param name="name">Имя атрибута</param>
        /// <param name="value">Значение</param>
        /// <returns></returns>
        private static XmlAttribute CreateXmlAttribute(XmlDocument doc, string name, string value)
        {
            XmlAttribute oAttribute = doc.CreateAttribute(name);
            oAttribute.Value = value;
            return oAttribute;
        }

        private bool Disconnect()
        {
            try
            {
                // TODO решить
                // У одного клиента есть проблема:
                // После вызова метода Отключить, он продолжает использовать компоненту
                // Для него выпустили спец релиз в котором  вызов _client.Close(); закомментирован
                // но это может вызвать проблему, когда сервер будет перегружен клиентами.
                // Поэтому в основной релиз не попадает это решение
                // Один из вариантов решения:
                // заглушить эту строку
                // вызывать закрытие канала при освобождении объекта
                // Второй Вариант
                // Оставить как есть. Т.к. подобная проблема не проявлялась не одного клиента, кроме него.
                // Почему нужно освобождать подключения https://stackoverflow.com/questions/7184509/why-is-it-important-to-dispose-close-a-wcf-client-proxy
                _client.Close();
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }

            return true;
        }

        private static string GetTax(int taxCode)
        {
            if (taxCode < 1 || taxCode > 63)
                return null; //Добавить исключение
            int length = 6;
            string binary = Convert.ToString(taxCode, 2);
            binary = binary.PadLeft(length, '0');

            binary = new string(binary.Reverse().ToArray());
            string res = Empty;
            for (int i = 0; i < length; i++)
            {
                if (binary[i] == '1')
                    res += i + ",";
            }

            if (res.Length != 0)
            {
                res = res.Remove(res.Length - 1);
            }

            return res;
            //Битовое поле
            //0 0 0 0 0 1 -0- Общая - 1
            //0 0 0 0 1 0 -1- Упрощенная Доход -2
            //0 0 0 1 0 0 -2- Упрощенная Доход минус расход - 4
            //0 0 1 0 0 0 -3- Единый налог на вмененный доход - 8
            //0 1 0 0 0 0 -4- Единый сельскохозяйственный налог - 16
            //1 0 0 0 0 0 -5- Патентная система налогообложения  - 32
        }

        /// <summary>
        /// Формирование имени терминала (компьютера), где запущена компонента
        /// При запуске в терминальном сеансе проставляется пометка RDP_[имя_терминала]_[имя_компьютера_с_которого_производится_подключение]
        /// При запуске с локальной машины записывается имя компьютера
        /// </summary>
        private void MakeTerminalId()
        {
            if (!SystemInformation.TerminalServerSession)
            {
                _terminalId = Environment.MachineName;
            }
            else
            {
                _terminalId = "RDP_" + Environment.MachineName + "_" + NativeMethods.GetTerminalServicesClientName();
            }
        }

        private string MakeXMLStatusParameters(long counter, long number, DateTime date)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlNode root = doc.CreateElement("StatusParameters");
            XmlElement parameters = doc.CreateElement("Parameters");

            parameters.Attributes.Append(CreateXmlAttribute(doc, "BacklogDocumentsCounter", counter.ToString()));
            parameters.Attributes.Append(CreateXmlAttribute(doc, "BacklogDocumentFirstNumber", number.ToString()));
            parameters.Attributes.Append(CreateXmlAttribute(doc, "BacklogDocumentFirstDateTime",
                date.ToString("yyyy-MM-ddTHH:mm:ss")));

            doc.AppendChild(xmlDec);
            doc.AppendChild(root);
            root.AppendChild(parameters);

            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                doc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Текст в формате XML, передаваемый с помощью параметра типа STRING
        /// Формирование регистрационных данных фискального накопителя
        /// </summary>
        /// <param name="parameters">Параметры ККТ полученные от сервера</param>
        /// <returns>XML документ в виде строке</returns>
        private string MakeXMLTableParametersKKT(FRParametersKKT parameters)
        {
           
            
            bool.TryParse(parameters.ServiceSign, out var serviceSign);
            bool.TryParse(parameters.BSOSing, out var bsoSing);
            bool.TryParse(parameters.CalcOnlineSign, out var calcOnlineSign);
            bool.TryParse(parameters.AutomaticMode, out var automaticMode);
            bool.TryParse(parameters.OfflineMode, out var offlineMode);

            string ffdVersionFn = "1.0";
            if (parameters.FFDVersionFN == "1.0" || parameters.FFDVersionFN == "1.0.5" ||
                parameters.FFDVersionFN == "1.1")
                ffdVersionFn = parameters.FFDVersionFN;

            string ffdVersionKkt = "1.0";
            if (parameters.FFDVersionKKT == "1.0" || parameters.FFDVersionKKT == "1.0.5" ||
                parameters.FFDVersionKKT == "1.1")
                ffdVersionKkt = parameters.FFDVersionKKT;

            XElement xml = new XElement("Parameters",

                new XAttribute("KKTNumber", parameters.KKTNumber ?? ""),
                new XAttribute("KKTSerialNumber", parameters.KKTSerialNumber ?? ""),
                new XAttribute("Fiscal", parameters.Fiscal),
                new XAttribute("FFDVersionFN", ffdVersionFn),
                new XAttribute("FFDVersionKKT", ffdVersionKkt),
                new XAttribute("FNSerialNumber", parameters.FNSerialNumber ?? ""),
                new XAttribute("DocumentNumber", parameters.DocumentNumber ?? ""),
                new XAttribute("DateTime", parameters.DateTime.ToString("yyyy-MM-ddTHH:mm:ss")),
                new XAttribute("OrganizationName", parameters.OrganizationName ?? ""),
                new XAttribute("OFDOrganizationName", parameters.OFDOrganizationName ?? ""),
                new XAttribute("VATIN", parameters.VATIN ?? ""),
                new XAttribute("AddressSettle", parameters.AddressSettle ?? ""),
                new XAttribute("TaxVariant",
                    GetTax(parameters.TaxVariant) == null ? "" : GetTax(parameters.TaxVariant)),
                new XAttribute("OfflineMode", offlineMode),
                new XAttribute("ServiceSign", serviceSign),
                new XAttribute("BSOSing", bsoSing),
                new XAttribute("CalcOnlineSign", calcOnlineSign),
                new XAttribute("AutomaticMode", automaticMode),
                new XAttribute("AutomaticNumber", parameters.AutomaticNumber ?? ""),
                new XAttribute("OFDVATIN", parameters.OFDVATIN ?? ""),
                new XAttribute("PlaceSettle", parameters.PlaceSettle ?? ""),//	Место проведения расчетов
                new XAttribute("DataEncryption", parameters.DataEncryption),//	Признак шифрование данных
                new XAttribute("SaleExcisableGoods", parameters.SaleExcisableGoods),//	продажа подакцизного товара
                new XAttribute("SignOfGambling", parameters.SignOfGambling),//	признак проведения азартных игр
                new XAttribute("SignOfLottery", parameters.SignOfLottery),//	признак проведения лотереи
                new XAttribute("SignOfAgent", parameters.SignOfAgent ?? ""),	//Коды признаков агента через разделитель ",".
                new XAttribute("PrinterAutomatic", parameters.PrinterAutomatic),//	Признак установки принтера в автомате
                new XAttribute("FNSWebSite", parameters.FNSWebSite ?? ""),//	Адрес сайта уполномоченного органа (ФНС) в сети «Интернет»
                new XAttribute("SenderEmail", parameters.SenderEmail ?? ""));//	Адрес электронной почты отправителя чека

            return xml.ToString();
        }

        // ReSharper disable once UnusedParameter.Local
        private bool OpenCheck(string deviceId, bool isFiscal, bool isReturn, bool canceleOpenedCheck)
        {
            _mainCheck = СоздатьЧек();
            _mainCheck.Фискальный = isFiscal;
            try
            {
                _mainCheck.ИмяКассы = deviceId;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);

                return false;
            }

            _mainCheck.ТипЧека = isReturn ? CheckTypes.Возврат : CheckTypes.Продажа;

            _mainCheckIsOpen = true;
            return true;
        }

        private DataParam Set(string nameParam, object data)
        {
            return new DataParam() { Name = nameParam, Data = data };
        }

        private DataParam SetNameMetod(string name)
        {
            return new DataParam() { Name = "method", Data = name };
        }

        private void SetError(long code, string message)
        {
            var error = Error.Ответ_сервера.GetDescription();
            _errorCode = code;
            if (code == 0 && IsNullOrEmpty(message))
                message = Error.Ошибок_нет.GetDescription();
            _errorString = $"{error} код: {code},  {message}";
        }

        private void SetError(Error error, string addMessage = null)
        {
            _errorCode = (int)error;
            _errorString = $"код: {_errorCode}, {error.GetDescription()}";
            if (!string.IsNullOrEmpty(addMessage))
            {
                _errorString += $", описание {addMessage}";
            }
        }

        public bool ДокументПечаталсяРанее(string dockId)
        {
            try
            {
                return _client.IsPrinted(Guid.Parse(dockId));
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool Инициализация()
        {
            return Connect();
        }

        public bool ОткрытьСмену(string deviceId)
        {
            try
            {
                var g = Guid.Parse(ПолучитьИдентификаторПоИмени(deviceId));
                var openShift = new OpenShift
                {
                    DeviceId = g,
                    DockId = Guid.NewGuid(),
                    TerminalId = _terminalId
                };

                if (!IsNullOrEmpty(Кассир))
                    openShift.Cashier = Кассир;
                var res = SavePrintOpenShift(openShift);

                if (res.ResultCode == 0) return true;

                SetError(res.ResultCode, res.ResultDescription);
                return false;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool ОткрытьЯщик(string deviceId)
        {
            try
            {
                var drawer = new OpenDrawer
                {
                    After = new PrintLine[] { },
                    Before = new PrintLine[] { },
                    DeviceId = Guid.Parse(deviceId),
                    DockId = Guid.NewGuid(),
                    DocType = DocTypes.ОткрытьЯщик,
                    TerminalId = _terminalId
                };

                var res = _client.PrintOpenDrawer(drawer);
                if (res.ResultCode == 0) return true;

                SetError(res.ResultCode, res.ResultDescription);
                return false;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool Отрезка(string deviceId)
        {
            try
            {
                // Алгоритм ожидает на входе как GUID так и ИД устройства
                // Сделано для поддержки разных вызовов из обработок 1С
                //
                Guid devId;
                var res = Guid.TryParse(deviceId, out devId);
                if (res)
                {
                    return _client.Cut(devId);
                }
                else
                {
                    devId = Guid.Parse(ПолучитьИдентификаторПоИмени(deviceId));
                    return _client.Cut(devId);
                }
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public int ПечатьКлише(string deviceId)
        {
            try
            {
                // Алгоритм ожидает на входе как GUID так и ИД устройства
                // Сделано для поддержки разных вызовов из обработок 1С
                //
                Guid devId;
                var res = Guid.TryParse(deviceId, out devId);
                if (res)
                {
                    return _client.PrintCliche(devId);
                }
                else
                {
                    devId = Guid.Parse(ПолучитьИдентификаторПоИмени(deviceId));
                    return _client.PrintCliche(devId);
                }
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                throw;
            }
        }

        public string ПолучитьИдентификаторПоИмени(string имяУстройства)
        {
            if (IsNullOrEmpty(имяУстройства))
            {
                throw new Exception("Имя устройства не может быть пустым");
            }

            if (!СтатусПодключения)
            {
                if (!Connect())
                    throw new Exception("Нет соединения");
            }

            try
            {
                return _client.GetDeviceIdByDeviceName(имяУстройства).ToString();
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                throw;
            }
        }

        public string[] ПолучитьИменаУстройств()
        {
            var result = _client.GetDevices();
            var ret = from item in result
                      select item.DeviceName;
            return ret.ToArray();
        }

        public string ПолучитьНомерВерсии()
        {
            return VersionLib;
        }

        public bool ПолучитьОписание(out string name, out string description, out string equipmentType,
            out int interfaceRevision,
            out bool integrationLibrary, out bool mainDriverInstalled, out string downloadUrl)
        {
            SetError(Error.Описания_имеют_только_компоненты_подключаемые_к_1С);
            name = null;
            description = null;
            equipmentType = null;
            interfaceRevision = 0;
            integrationLibrary = false;
            mainDriverInstalled = false;
            downloadUrl = null;
            return false;

            // Пример реализации интерфейса для внешних компонент для 1С
            //
            // Name = "AddInRBSoftPrintService";
            // Description = "Внешняя компонента для 1С Сервис печати РБ-Софт. Для ККТ";
            // EquipmentType = "ККТ";
            // InterfaceRevision = 2001;
            // IntegrationLibrary = false;
            // MainDriverInstalled = true;
            // DownloadURL = "http://www.rbsoft.ru/product/devicenet/download";
            // return true;
        }

        public long ПолучитьОшибку(out string errorDescription)
        {
            errorDescription = _errorString;
            return _errorCode;
        }

        public bool ПолучитьПараметры(out string tableParameters)
        {
            tableParameters = GetXmlParameters();
            return true;
        }

        [return: MarshalAs(UnmanagedType.SafeArray)]
        public string[] ПолучитьСписокИдентификаторовУстройств()
        {
            Guid[] guids = _client.GetDeviceIds();
            return guids.Select(x => x.ToString()).ToArray();
        }

        public double ПолучитьСуммуНаличных(string deviceId)
        {
            try
            {
                Guid devId = Guid.Parse(deviceId);
                return _client.GetSumm(devId);
            }
            catch (FormatException)
            {
                throw new Exception("Не удалось преобразовать string -> Guid");
            }
        }

        public Чек ПолучитьЧек(string id)
        {
            try
            {
                Guid dockId = Guid.Parse(id);
                var result = _client.GetCheck(dockId);
                Чек чек = new Чек { Check = result };
                return чек;
            }
            catch (FormatException)
            {
                throw new Exception("Не удалось преобразовать string -> Guid");
            }
            catch (NullReferenceException)
            {
                throw new Exception("Значение параметра  не определено");
            }
        }

        public ВнесениеВыемка РаспечататьВнесениеВыемка(ВнесениеВыемка выемка)
        {
            if (!СтатусПодключения)
            {
                выемка.Cash.ResultCode = -404;
                выемка.Cash.ResultDescription = "Ошибка подключения к серверу";
            }
            else
            {
                try
                {
                    SetId(выемка, _client);
                    выемка.Cash = _client.PrintCash(выемка.Cash);
                }
                catch (TimeoutException)
                {
                    var status = ShowHistoryPrintForm($"{выемка.Cash.DocType}", выемка.Cash.DockId);
                    выемка.УстановитьОшибку(status.Item1, status.Item2);
                }
                catch (Exception ex)
                {
                    выемка.УстановитьОшибку(400, ex.Message);
                }
            }

            return выемка;
        }

        public Заказ РаспечататьЗаказ(Заказ заказ)
        {
            if (!СтатусПодключения)
            {
                заказ.Order.ResultCode = -404;
                заказ.Order.ResultDescription = "Ошибка подключения к серверу";
            }
            else
            {
                try
                {
                    SetId(заказ, _client);
                    заказ.Order = _client.PrintOrder(заказ.Order);
                }
                catch (TimeoutException)
                {
                    var status = ShowHistoryPrintForm($"{заказ.Order.DocType}", заказ.Order.DockId);
                    заказ.УстановитьОшибку(status.Item1, status.Item2);
                }
                catch (Exception ex)
                {
                    заказ.Order.ResultCode = -400;
                    заказ.Order.ResultDescription = ex.Message;
                }
            }

            return заказ;
        }

        [Obsolete("Возвращает недостаточно данных")]
        public bool РаспечататьИЗакрытьЧек()
        {
            try
            {
                _mainCheck = РаспечататьЧек(_mainCheck);
                _mainCheckIsOpen = false;

                return _mainCheck.КодОшибки != 0;
            }
            catch (NullReferenceException)
            {
                SetError(Error.Чек_не_открыт);
                return false;
            }
        }

        public ПараметрыФискализации PrintOperationFN(ПараметрыФискализации param)
        {
            if (!СтатусПодключения)
            {
                param.ParamFiscal.ResultCode = -404;
                param.ParamFiscal.ResultDescription = "Ошибка подключения к серверу";
                return param;
            }

            try
            {
                SetId(param, _client);
                var g = param.ParamFiscal.DeviceId;
                param.ParamFiscal = _client.PrintOperationFN(param.ParamFiscal);
                //   _lastDocument = rep;

                ShowAlert(g);
                return param;
            }
            catch
            {
                return param;
            }
            //    catch (TimeoutException)
            //    {
            //        var resWait = ShowHistoryPrintForm($"{rep.Report.DocType} {rep.Report.ReportType}", rep.Report.DockId);

            //        rep.УстановитьОшибку(resWait.Item1, resWait.Item2);

            //        if (resWait.Item1 != 0)
            //            return rep;

            //        var reportRes = rep.Report;

            //        while (!CanGetResultReport(out reportRes, rep.Report.DockId))
            //        { }

            //        rep.Report = reportRes;
            //        return rep;
            //    }
            //    catch (Exception ex)
            //    {
            //        rep.Report.ResultCode = -400;
            //        rep.Report.ResultDescription = ex.Message;
            //        return rep;
            //    }
        }

        public Отчет РаспечататьОтчет(Отчет rep)
        {
            if (!СтатусПодключения)
            {
                rep.Report.ResultCode = -404;
                rep.Report.ResultDescription = "Ошибка подключения к серверу";
                return rep;
            }

            try
            {
                SetId(rep, _client);
                var g = rep.Report.DeviceId;
                rep.Report = _client.PrintReport(rep.Report);
                _lastDocument = rep;

                ShowAlert(g);
                return rep;
            }
            catch (TimeoutException)
            {
                var resWait = ShowHistoryPrintForm($"{rep.Report.DocType} {rep.Report.ReportType}", rep.Report.DockId);

                rep.УстановитьОшибку(resWait.Item1, resWait.Item2);

                if (resWait.Item1 != 0)
                    return rep;

                var reportRes = rep.Report;

                while (!CanGetResultReport(out reportRes, rep.Report.DockId))
                { }

                rep.Report = reportRes;
                return rep;
            }
            catch (Exception ex)
            {
                rep.Report.ResultCode = -400;
                rep.Report.ResultDescription = ex.Message;
                return rep;
            }
        }

        /// <summary>
        /// Отобразить предупреждение, что данные в ОФД не предаются
        /// </summary>
        /// <param name="g">Идентификатор устройства на Сервере ККМ</param>
        private void ShowAlert(Guid g)
        {
            try
            {
                var st = _client.GetDeviceSettings(g);
                if (!st.IsControlSendingData) return;

                var sp = _client.GetStatusParameters(g);
                if (sp.Item1 != 0 && (Math.Abs((sp.Item3 - DateTime.Now).Days) >= st.DayDifferenceAlert))
                {
                    using (var f = new Forms.Message())
                    {
                        f.SetData(
                            "Превышено время работы без передачи данных в ОФД!",
                            "Дней работы без передачи данных: " +
                            Math.Abs((sp.Item3 - DateTime.Now.AddDays((-1) * st.DayDifferenceAlert)).Days),
                            "Количество не отправленных чеков: " + sp.Item1);
                        f.ShowDialog();
                    }
                }
            }
            catch
            {
                //ignore
            }
        }

        public ТекстовыйФайл РаспечататьТекст(ТекстовыйФайл текст)
        {
            if (!СтатусПодключения)
            {
                текст.TextFile.ResultCode = -404;
                текст.TextFile.ResultDescription = "Ошибка подключения к серверу";
            }
            else
            {
                TextFile text = текст.TextFile;
                try
                {
                    SetId(текст, _client);
                    текст.TextFile = _client.PrintTextFile(text);
                }
                catch (TimeoutException)
                {
                    var status = ShowHistoryPrintForm($"{текст.TextFile.DocType}", текст.TextFile.DockId);
                    текст.УстановитьОшибку(status.Item1, status.Item2);
                }
                catch (Exception ex)
                {
                    текст.TextFile.ResultCode = -400;
                    текст.TextFile.ResultDescription = "Ошибка при вызове метода 'РаспечататьТекст': " + ex.Message;
                }
            }

            return текст;
        }

        public Чек РаспечататьЧек(Чек чек)
        {
            if (!СтатусПодключения)
            {
                чек.Check.ResultCode = -404;
                чек.Check.ResultDescription = "Ошибка подключения к серверу";
                return чек;
            }
            Guid id = чек.Check.DockId;
            try
            {
                SetId(чек, _client);
                Check check = чек.Check;
                if (!_client.IsLicenseAvailable())
                {
                    int docCount = _client.GetDemoDocCount();
                    if (docCount == 0)
                    {
                        чек.ДобавитьВНачало("ЛИМИТ ИСЧЕРПАН");
                    }
                    чек.ДобавитьВНачало("ВЫЗОВОВ ПЕЧАТИ ОСТАЛОСЬ " + docCount);
                    чек.ДобавитьВНачало("ВНИМАНИЕ! ПЕЧАТЬ В РЕЖИМЕ ДЕМОНСТРАЦИИ");
                }
                Guid g = check.DeviceId;
                var deviceSetting = _client.GetDeviceSettings(g);
                if (deviceSetting.IsElectronically)
                {
                    using (var f = new DialogContact())
                    {
                        f.ClientData = чек.Контакт;
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            чек.Контакт = f.ClientData;
                        }
                    }
                }

                чек.Check = _client.PrintCheck(check);
                if (чек.КодОшибки != 0) SetError(чек.КодОшибки, чек.ОписаниеОшибки);
                _lastDocument = чек;
            }
            catch (TimeoutException)
            {
                var resWait = ShowHistoryPrintForm($"{чек.Check.DocType} {чек.Check.CheckType}", чек.Check.DockId);

                чек.УстановитьОшибку(resWait.Item1, resWait.Item2);

                if (resWait.Item1 != 0)
                    return чек;

                var сheckBuf = чек.Check;

                while (!GetGetResultCheck(out сheckBuf, чек.Check.DockId))
                { }

                чек.Check = сheckBuf;
                return чек;
            }
            catch (Exception ex)
            {
                чек.Check.ResultCode = -8;
                чек.Check.ResultDescription = ex.Message;
            }
            return чек;
        }

        #region TimeOut handler

        /// <summary>
        /// Отображает диалоговое окно ожидания результатов печати
        /// Запрос результатов печати при для документов не имеющих конкретных интерфейсов запроса
        /// </summary>
        /// <param name="typDoc">Название типа документа, отображается в заголовке формы</param>
        /// <param name="docId">Идентификатор документа</param>
        /// <returns>
        /// 1. Код ошибки
        /// 2. Описание ошибки
        /// 3. Номер чека
        /// 4. Номер смены
        /// </returns>
        private Tuple<int, string, int, int> ShowHistoryPrintForm(string typDoc, Guid docId)
        {
            //TODO рефакторинг tuple
            using (var f = new WaitPrintResultcs(typDoc, docId, _client))
            {
                f.ShowDialog();
                return new Tuple<int, string, int, int>(f.Model.Code, f.Model.Description, f.Model.CheckNumber, f.Model.SessionNumber);
            }
        }

        /// <summary>
        /// Безопасное открытие смены в случае тайм-аута
        /// </summary>
        private OpenShift SavePrintOpenShift(OpenShift openShift)
        {
            try
            {
                return _client.PrintOpenShift(openShift);
            }
            catch (TimeoutException)
            {
                var status = ShowHistoryPrintForm($"{openShift.DocType}", openShift.DockId);
                openShift.ResultCode = status.Item1;
                openShift.ResultDescription = status.Item2;
                openShift.DocNumber = status.Item3;
                openShift.SessionNumber = status.Item4;
                return openShift;
            }
        }

        private CheckCorrection SavePrintCorrecion(CheckCorrection checkCorrection)
        {
            try
            {
                return _client.PrintCheckCorrection(checkCorrection);
            }
            catch (TimeoutException)
            {
                var status = ShowHistoryPrintForm($"{checkCorrection.DocType}", checkCorrection.DockId);
                checkCorrection.ResultCode = status.Item1;
                checkCorrection.ResultDescription = status.Item2;
                checkCorrection.DocNumber = status.Item3;
                checkCorrection.SessionNumber = status.Item4;
                return checkCorrection;
            }
        }

        /// <summary>
        /// Запрос результатов печати чека
        /// </summary>
        /// <returns>Результаты получены</returns>
        private bool GetGetResultCheck(out Check doc, Guid docId)
        {
            try
            {
                var client = GetClientWithNewTimeOut();
                doc = client.GetCheck(docId);
                return true;
            }
            catch (TimeoutException)
            {
                doc = null;
                return false;
            }
        }

        /// <summary>
        /// Запрос результатов печати отчета
        /// </summary>
        /// <returns>Результаты получены</returns>
        private bool CanGetResultReport(out Report reportRes, Guid docId)
        {
            try
            {
                var client = GetClientWithNewTimeOut();
                reportRes = client.GetReport(docId);
                return true;
            }
            catch (TimeoutException)
            {
                reportRes = null;
                return false;
            }
        }

        /// <summary>
        /// Получить клиента с независимыми настройками тайм-аута
        /// </summary>
        /// <returns></returns>
        private PrintServiceClient GetClientWithNewTimeOut(double second = 15)
        {
            var binding = GetBinding();
            var time = TimeSpan.FromSeconds(second);
            binding.CloseTimeout = time;
            binding.OpenTimeout = time;
            binding.ReceiveTimeout = time;
            binding.SendTimeout = time;
            var client = new PrintServiceClient(binding, new EndpointAddress(Format("http://{0}:{1}/PrintService/", Адрес, Порт)));
            return client;
        }

        #endregion TimeOut handler

        public ВнесениеВыемка СоздатьВнесениеВыемка()
        {
            var внесениеВыемка = new ВнесениеВыемка();
            DefaultInit(внесениеВыемка);
            return внесениеВыемка;
        }

        private void DefaultInit(DockBase doc)
        {
            doc.Терминал = _terminalId;
            doc.ИмяКассы = ИдентификаторУстройства;
        }

        public Заказ СоздатьЗаказ()
        {
            var заказ = new Заказ();
            DefaultInit(заказ);
            return заказ;
        }

        public Отчет СоздатьОтчет()
        {
            var report = new Отчет();
            DefaultInit(report);
            return report;
        }

        public ТекстовыйФайл СоздатьТекст()
        {
            var файл = new ТекстовыйФайл();
            DefaultInit(файл);
            return файл;
        }

        public Чек СоздатьЧек()
        {
            Чек чек = new Чек();
            DefaultInit(чек);
            return чек;
        }

        private WSHttpBinding GetBinding()
        {
            WSHttpBinding binding = new WSHttpBinding();
            // Чтобы принять чек с 400 позициями.
            binding.MaxReceivedMessageSize = 500000;
            binding.Security.Mode = SecurityMode.None;
            _timeout = TimeOut.GetTimeOut();
            binding.ReceiveTimeout = _timeout;
            binding.OpenTimeout = _timeout;
            binding.SendTimeout = _timeout;
            binding.CloseTimeout = _timeout;

            return binding;
        }

        /// <summary>
        /// Формирование XML в виде строки с параметрами компоненты
        /// </summary>
        /// <returns>XML документ в виде строки</returns>
        private string GetXmlParameters()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.CreateElement("Settings");
            XmlNode main = doc.CreateElement("Page");
            XmlAttribute mainCaption = doc.CreateAttribute("Caption");
            mainCaption.Value = "Параметры";
            main.Attributes.Append(mainCaption);
            XmlElement connectionGroup = doc.CreateElement("Group");
            XmlAttribute connectionGroupCaption = doc.CreateAttribute("Caption");
            connectionGroupCaption.Value = "Основные";
            connectionGroup.Attributes.Append(connectionGroupCaption);

            connectionGroup.AppendChild(MakeXmlParameter(doc, "Adress", "Адрес", "String", "127.0.0.1"));
            connectionGroup.AppendChild(MakeXmlParameter(doc, "Port", "Порт", "Number", "4398"));
            connectionGroup.AppendChild(MakeXmlParameter(doc, "DeviceId", "Псевдоним устройства", "String", ""));

            main.AppendChild(connectionGroup);
            root.AppendChild(main);
            doc.AppendChild(root);
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                doc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Запись параметра в XML документ по стандарту 1С совместимо.
        /// </summary>
        /// <param name="doc">XML документ для записи данных</param>
        /// <param name="name">Имя параметра</param>
        /// <param name="caption">Выводимый текст параметра</param>
        /// <param name="typeValue">Тип параметра</param>
        /// <param name="deafultValue">Значение по умолчанию</param>
        /// <returns>Возвращает XML элемент параметра</returns>
        private XmlElement MakeXmlParameter(XmlDocument doc, string name, string caption, string typeValue,
            string deafultValue)
        {
            XmlAttribute nameAttribute = doc.CreateAttribute("Name");
            XmlAttribute captionAttribute = doc.CreateAttribute("Caption");
            XmlAttribute typeValueAttribute = doc.CreateAttribute("TypeValue");
            XmlAttribute defaultValueAttribute = doc.CreateAttribute("DefaultValue");

            XmlElement element = doc.CreateElement("Parameter");
            nameAttribute.Value = name;
            element.Attributes.Append(nameAttribute);
            captionAttribute.Value = caption;
            element.Attributes.Append(captionAttribute);
            typeValueAttribute.Value = typeValue;
            element.Attributes.Append(typeValueAttribute);
            if (deafultValue != Empty)
            {
                defaultValueAttribute.Value = deafultValue;
                element.Attributes.Append(defaultValueAttribute);
            }

            return element;
        }

        #region FFD105

        public bool НапечататьЧекВнесенияВыемки(string deviceId, string параметрыОперации, double amount)
        {
            if (Log)
                WriteLog(new List<DataParam>()
                {
                    SetNameMetod(MethodBase.GetCurrentMethod().Name),
                    Set(nameof(deviceId), deviceId),
                    Set(nameof(параметрыОперации), параметрыОперации),
                    Set(nameof(amount), amount)
                });

            var inOutCash = СоздатьВнесениеВыемка();

            // ПараметрыОперации
            string cashierName;
            string cashierVatin;
            ParseImputParametes(параметрыОперации, out cashierName, out cashierVatin);

            inOutCash.Cash.Cashier = cashierName;
            inOutCash.Cash.CashierVATIN = cashierVatin;

            double localAmount = amount;
            if (localAmount < 0)
            {
                inOutCash.ВидОперации = CashType.Выемка;
                inOutCash.Сумма = localAmount * -1;
            }
            else
            {
                inOutCash.ВидОперации = CashType.Внесение;
                inOutCash.Сумма = localAmount;
            }

            inOutCash.ИмяКассы = deviceId;

            return PrintCashInOut(inOutCash);
        }

        #endregion FFD105

        #region FFD10

        public bool НапечататьЧекВнесенияВыемки(string deviceId, double amount)
        {
            var inOutCash = СоздатьВнесениеВыемка();
            double localAmount = amount;
            if (localAmount < 0)
            {
                inOutCash.ВидОперации = CashType.Выемка;
                inOutCash.Сумма = localAmount * -1;
            }
            else
            {
                inOutCash.ВидОперации = CashType.Внесение;
                inOutCash.Сумма = localAmount;
            }

            inOutCash.ИмяКассы = deviceId;

            return PrintCashInOut(inOutCash);
        }

        private bool PrintCashInOut(ВнесениеВыемка inOutCash)
        {
            if (!СтатусПодключения)
            {
                SetError(Error.Нет_подключения_к_Cерверу_ККМ);
                return false;
            }

            if (!_client.IsLicenseAvailable())
            {
                int docCount = _client.GetDemoDocCount();
                if (docCount < 0)
                {
                    SetError(Error.Исчерпан_лимит_демо_печати);
                    return false;
                }

                inOutCash.ДобавитьСтрокуДемоЛицензии(docCount);
            }

            Cash cash = inOutCash.Cash;
            try
            {
                SetId(inOutCash, _client);
                cash = _client.PrintCash(cash);
                _lastDocument = inOutCash;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }

            if (cash.ResultCode != 0)
            {
                SetError(cash.ResultCode, cash.ResultDescription);
                return false;
            }

            return true;
        }

        #endregion FFD10

        #region FFD105

        public bool НапечататьОтчетБезГашения(string deviceId, string параметрыОперации)
        {
            if (Log)
                WriteLog(new List<DataParam>()
                {
                    SetNameMetod(MethodBase.GetCurrentMethod().Name),
                    Set(nameof(deviceId), deviceId),
                    Set(nameof(параметрыОперации), параметрыОперации)
                });

            // ПараметрыОперации
            string cashierName;
            string cashierVatin;
            ParseImputParametes(параметрыОперации, out cashierName, out cashierVatin);

            var xReport = СоздатьОтчет();
            xReport.Report.ReportType = ReportType.ХОтчет;
            xReport.Кассир = cashierName;
            xReport.ИннКассира = cashierVatin;
            xReport.ИмяКассы = deviceId;

            return PrintReport(xReport);
        }

        #endregion FFD105

        #region FFD10

        public bool НапечататьОтчетБезГашения(string deviceId)
        {
            var xReport = СоздатьОтчет();
            xReport.Report.ReportType = ReportType.ХОтчет;
            xReport.ИмяКассы = deviceId;
            return PrintReport(xReport);
        }

        #endregion FFD10

        #region FFD105

        public bool ЗакрытьСмену(string deviceId, string параметрыОперации, out string параметрыСостояния,
            out int sessionNumber, out int documentNumber)
        {
            if (Log)
                WriteLog(new List<DataParam>()
                {
                    SetNameMetod(MethodBase.GetCurrentMethod().Name),
                    Set(nameof(deviceId), deviceId),
                    Set(nameof(параметрыОперации), параметрыОперации),
                    Set(nameof(параметрыСостояния), "out param")
                });

            string cashierName;
            string cashierVatin;
            ParseImputParametes(параметрыОперации, out cashierName, out cashierVatin);

            var rep = СоздатьОтчет();
            rep.Кассир = cashierName;
            rep.ИннКассира = cashierVatin;
            rep.ТипОтчета = (int)ReportType.ZОтчет;
            var result = РаспечататьОтчет(rep);
            if (result.КодОшибки != 0)
            {
                sessionNumber = 0;
                documentNumber = 0;
                параметрыСостояния = null;
                SetError(result.КодОшибки, result.ОписаниеОшибки);
                return false;
            }
            else
            {
                sessionNumber = result.НомерСмены;
                documentNumber = result.НомерЧека;
                параметрыСостояния = MakeXmlOutput(result.Report.Output);
                return true;
            }
        }

        private static string MakeXmlOutput(OutputParameters p, bool isOpenShift = false)
        {
            if (p == null) return Empty;
            XElement xml;
            if (isOpenShift)
            {
                xml = new XElement("Parameters");
                if (p.UrgentReplacementFn != null)
                    xml.Add(new XAttribute("UrgentReplacementFN", p.UrgentReplacementFn));
                if (p.MemoryOverflowFn != null)
                    xml.Add(new XAttribute("MemoryOverflowFN", p.MemoryOverflowFn));
                if (p.ResourcesExhaustionFn != null)
                    xml.Add(new XAttribute("ResourcesExhaustionFN", p.ResourcesExhaustionFn));
                if (p.OfdTimeout != null)
                    xml.Add(new XAttribute("OFDTimeout", p.OfdTimeout));
            }
            else
            {
                xml = new XElement("Parameters");
                if (p.NumberOfChecks != null)
                    xml.Add(new XAttribute("NumberOfChecks", p.NumberOfChecks));
                if (p.NumberOfDocuments != null)
                    xml.Add(new XAttribute("NumberOfDocuments", p.NumberOfDocuments));
                if (p.BacklogDocumentsCounter != null)
                    xml.Add(new XAttribute("BacklogDocumentsCounter", p.BacklogDocumentsCounter));
                if (p.BacklogDocumentFirstNumber != null)
                    xml.Add(new XAttribute("BacklogDocumentFirstNumber", p.BacklogDocumentFirstNumber));
                if (p.BacklogDocumentFirstDateTime != null)
                    xml.Add(new XAttribute("BacklogDocumentFirstDateTime",
                        p.BacklogDocumentFirstDateTime.Value.ToString("yyyy-MM-ddTHH:mm:ss")));
                if (p.UrgentReplacementFn != null)
                    xml.Add(new XAttribute("UrgentReplacementFN", p.UrgentReplacementFn));
                if (p.MemoryOverflowFn != null)
                    xml.Add(new XAttribute("MemoryOverflowFN", p.MemoryOverflowFn));
                if (p.ResourcesExhaustionFn != null)
                    xml.Add(new XAttribute("ResourcesExhaustionFN", p.ResourcesExhaustionFn));
                if (p.ResourcesFn != null)
                    xml.Add(new XAttribute("ResourcesFN", p.ResourcesFn));
                if (p.OfdTimeout != null)
                    xml.Add(new XAttribute("OFDTimeout", p.OfdTimeout));
            }

            var doc = new XDocument(new XDeclaration("1.0", "UTF-8", null),
                new XElement("OutputParameters", xml));

            return Concat(doc.Declaration, doc);
        }

        public bool ОткрытьСмену(string deviceId, string параметрыОперации, out string параметрыСостояния,
            out int sessionNumber, out int documentNumber)
        {
            if (Log)
                WriteLog(new List<DataParam>()
                {
                    SetNameMetod(MethodBase.GetCurrentMethod().Name),
                    Set(nameof(deviceId), deviceId),
                    Set(nameof(параметрыОперации), параметрыОперации),
                    Set(nameof(параметрыСостояния), "out param")
                });

            ParseImputParametes(параметрыОперации, out var cashierName, out var cashierVatin);
            // ПараметрыСостояния не представлены

            return OpenShift105(deviceId, cashierName, cashierVatin, out параметрыСостояния, out sessionNumber, out documentNumber);
        }

        private bool OpenShift105(string deviceId, string cashierName, string cashierVatin, out string paramState, out int sessionNumber,
            out int documentNumber)
        {
            try
            {
                Guid g = Guid.Parse(ПолучитьИдентификаторПоИмени(deviceId));

                OpenShift openShift = new OpenShift
                {
                    Cashier = cashierName,
                    CashierVATIN = cashierVatin,
                    DockId = Guid.NewGuid(),
                    DeviceId = g,
                    TerminalId = _terminalId
                };

                var res = SavePrintOpenShift(openShift);

                if (res.ResultCode != 0)
                {
                    SetError(res.ResultCode, res.ResultDescription);
                    sessionNumber = 0;
                    documentNumber = 0;
                    paramState = Empty;

                    return false;
                }
                else
                {
                    sessionNumber = res.SessionNumber;
                    documentNumber = res.DocNumber;
                    paramState = MakeXmlOutput(res.Output, true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                sessionNumber = 0;
                documentNumber = 0;
                paramState = Empty;
                return false;
            }
        }

        /// <summary>
        /// Парсинг входные параметров операции
        /// </summary>
        /// <param name="параметрыОперации">Текст в формате XML</param>
        /// <param name="cashierName">ФИО и должность уполномоченного лица для проведения операции</param>
        /// <param name="cashierVatin">ИНН уполномоченного лица для проведения операции</param>
        private static void ParseImputParametes(string параметрыОперации, out string cashierName, out string cashierVatin)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(параметрыОперации);
                if (doc.DocumentElement != null)
                {
                    XmlNode node = doc.DocumentElement.SelectSingleNode("Parameters");

                    // ПараметрыОперации
                    cashierName = ParseData.Parse(node, "CashierName");
                    cashierVatin = ParseData.Parse(node, "CashierVatin");
                }
                else
                {
                    cashierName = null;
                    cashierVatin = null;
                }
            }
            catch
            {
                cashierName = null;
                cashierVatin = null;
            }
        }

        public bool ОтчетОТекущемСостоянииРасчетов(string deviceId, string параметрыОперации,
            out string параметрыСостояния)
        {
            if (Log)
                WriteLog(new List<DataParam>()
                {
                    SetNameMetod(MethodBase.GetCurrentMethod().Name),
                    Set(nameof(deviceId), deviceId),
                    Set(nameof(параметрыОперации), параметрыОперации)
                });

            ParseImputParametes(параметрыОперации, out var cashierName, out var cashierVatin);

            var report = СоздатьОтчет();
            report.ИннКассира = cashierVatin;
            report.Кассир = cashierName;
            report.Report.ReportType = ReportType.ОтчетОТекущемСостоянииРасчетов;
            report.ИмяКассы = deviceId;

            var res = РаспечататьОтчет(report);
            if (res.КодОшибки == 0)
            {
                параметрыСостояния = MakeXmlOutput(res.Report.Output);
                return true;
            }

            параметрыСостояния = Empty;
            SetError(res.КодОшибки, res.ОписаниеОшибки);
            return false;
        }

        public bool СформироватьЧек(string deviceId, bool electronically, string checkPackage, out int checkNumber,
            out int sessionNumber, out string fiscalSign, out string addressSiteInspection)
        {
            if (Log)
                WriteLog(new List<DataParam>()
                {
                    SetNameMetod(MethodBase.GetCurrentMethod().Name),
                    Set(nameof(deviceId), deviceId),
                    Set(nameof(electronically), electronically),
                    Set(nameof(checkPackage), checkPackage)
                });

            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(checkPackage);

                var node = doc.DocumentElement.SelectSingleNode("Parameters");
                int paymentType = ParseData.ParseInt(node, "PaymentType");

                _mainCheck = СоздатьЧек();
                _mainCheck.Фискальный = true;
                _mainCheckIsOpen = true;
                _mainCheck.ТипЧека = GetType(paymentType);
                _mainCheck.УстройствоПечати = ПолучитьИдентификаторПоИмени(deviceId);

                _mainCheck.Check.Cashier = ParseData.Parse(node, "CashierName");
                _mainCheck.Check.CashierVATIN = ParseData.Parse(node, "CashierVATIN");
                _mainCheck.Check.SenderEmail = ParseData.Parse(node, "SenderMail");
                _mainCheck.Check.AddressSettle = ParseData.Parse(node, "AddressSettle");
                if (node.Attributes?["AgentSign"]?.InnerText != null && node.Attributes?["AgentSign"]?.InnerText != Empty)
                {
                    _mainCheck.Check.AgentSign = ParseData.ParseInt(node, "AgentSign");
                }
                else
                {
                    _mainCheck.Check.AgentSign = null;
                }
                node = doc.DocumentElement.SelectSingleNode("Parameters/AgentData");
                if (node != null)
                {
                    _mainCheck.Check.AgentData = new CheckAgentData();
                    _mainCheck.Check.AgentData.PayingAgentOperation = ParseData.Parse(node, "PayingAgentOperation");
                    _mainCheck.Check.AgentData.PayingAgentPhone = ParseData.Parse(node, "PayingAgentPhone");
                    _mainCheck.Check.AgentData.ReceivePaymentsOperatorPhone = ParseData.Parse(node, "ReceivePaymentsOperatorPhone");
                    _mainCheck.Check.AgentData.MoneyTransferOperatorPhone = ParseData.Parse(node, "MoneyTransferOperatorPhone");
                    _mainCheck.Check.AgentData.MoneyTransferOperatorName = ParseData.Parse(node, "MoneyTransferOperatorName");
                    _mainCheck.Check.AgentData.MoneyTransferOperatorAddress = ParseData.Parse(node, "MoneyTransferOperatorAddress");
                    _mainCheck.Check.AgentData.MoneyTransferOperatorVATIN = ParseData.Parse(node, "MoneyTransferOperatorVATIN");
                }
                node = doc.DocumentElement.SelectSingleNode("Parameters/PurveyorData");
                if (node != null)
                {
                    _mainCheck.Check.PurveyorData = new CheckPurveyorData();
                    _mainCheck.Check.PurveyorData.PurveyorPhone = ParseData.Parse(node, "PurveyorPhone");
                    _mainCheck.Check.PurveyorData.PurveyorName = ParseData.Parse(node, "PurveyorName");
                    _mainCheck.Check.PurveyorData.PurveyorVATIN = ParseData.Parse(node, "PurveyorVATIN");
                }
                node = doc.DocumentElement.SelectSingleNode("Parameters");
                string email = ParseData.Parse(node, "CustomerEmail");
                string telephone = ParseData.Parse(node, "CustomerPhone");
                if (!IsNullOrEmpty(email))
                {
                    _mainCheck.Контакт = email;
                }
                else if (!IsNullOrEmpty(telephone))
                {
                    _mainCheck.Контакт = telephone;
                }

                _mainCheck.Check.TaxType = ParseData.ParseIntTax(node, "TaxVariant");
                _mainCheck.Check.CheckMode = electronically ? 0 : 1;
                var docx = doc.DocumentElement.SelectNodes("Positions");

                var items = new List<CheckItem>();
                foreach (XmlNode nd in docx[0].ChildNodes)
                {
                    if (nd.Name == "FiscalString")
                    {
                        string name = ParseData.Parse(nd, "Name");
                        double quantity = ParseData.ParseDouble(nd, "Quantity");
                        double price = ParseData.ParseDouble(nd, "PriceWithDiscount");
                        double amount = ParseData.ParseDouble(nd, "SumWithDiscount");
                        double infoDicount = ParseData.ParseDouble(nd, "DiscountSum");

                        int department = ParseData.ParseInt(nd, "Department") == 0
                            ? 1
                            : ParseData.ParseInt(nd, "Department");
                        int tax = ParseData.ParseTax(nd, "Tax");
                        // ПризнакСпособаРасчета
                        int signMethodCalculation = ParseData.ParseInt(nd, "SignMethodCalculation");
                        // Признак предмета
                        int signCalculationObject = ParseData.ParseInt(nd, "SignCalculationObject");
                        double taxSum;
                        // Атрибуты с ФФД 1.0.5
                        {
                            taxSum = ParseData.ParseDouble(nd, "TaxSum");
                        }

                        // Принудительное округление
                        quantity = Math.Round(quantity, 3, MidpointRounding.AwayFromZero);
                        price = Math.Round(price, 2, MidpointRounding.AwayFromZero);
                        amount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);

                        CheckItem item = new CheckItem();
                        item.isFiscal = true;
                        item.After = new PrintLine();
                        item.Before = new PrintLine();
                        item.Quantity = (decimal)quantity;
                        // Скидка уже учтена в цене
                        item.DiscountValue = 0;
                        item.Summ = (decimal)amount;
                        // Сервер ККМ.
                        // Больше нуля скидка, Меньше нуля надбавка
                        // Только информация, печатается для справки
                        item.DiscountInfoValue = (decimal)infoDicount;
                        item.Name = name;
                        item.Price = (decimal)price;
                        item.Department = department;
                        item.TaxValue = tax;
                        item.TaxSum = (decimal)taxSum;

                        // ReSharper disable once RedundantAssignment
                        var paymentMod = PaymentModes.НеПрименяется;
                        Enum.TryParse(signMethodCalculation.ToString(), out paymentMod);
                        item.PaymentMode = paymentMod;

                        // ReSharper disable once RedundantAssignment
                        var itemType = ItemTypes.НеПрименяется;
                        Enum.TryParse(signCalculationObject.ToString(), out itemType);
                        item.ItemType = itemType;
                        if (nd.Attributes?["SignSubjectCalculationAgent"]?.InnerText != null && nd.Attributes?["SignSubjectCalculationAgent"]?.InnerText != Empty)
                        {
                            int? agent = ParseData.ParseInt(nd, "SignSubjectCalculationAgent");
                            item.SignSubjectCalculationAgent = agent;
                        }
                        //данные Платежного Агента
                        var ndAgentData = nd.SelectSingleNode("AgentData");
                        if (ndAgentData != null)
                        {
                            var payingAgentOperation = ParseData.Parse(ndAgentData, "PayingAgentOperation");
                            var payingAgentPhone = ParseData.Parse(ndAgentData, "PayingAgentPhone");
                            var receivePaymentsOperatorPhone = ParseData.Parse(ndAgentData, "ReceivePaymentsOperatorPhone");
                            var moneyTransferOperatorPhone = ParseData.Parse(ndAgentData, "MoneyTransferOperatorPhone");
                            var moneyTransferOperatorName = ParseData.Parse(ndAgentData, "MoneyTransferOperatorName");
                            var moneyTransferOperatorAddress = ParseData.Parse(ndAgentData, "MoneyTransferOperatorAddress");
                            var moneyTransferOperatorVatin = ParseData.Parse(ndAgentData, "MoneyTransferOperatorVATIN");

                            item.AgentDataItem = new CheckAgentData
                            {
                                PayingAgentOperation = payingAgentOperation,
                                PayingAgentPhone = payingAgentPhone,
                                ReceivePaymentsOperatorPhone = receivePaymentsOperatorPhone,
                                MoneyTransferOperatorPhone = moneyTransferOperatorPhone,
                                MoneyTransferOperatorName = moneyTransferOperatorName,
                                MoneyTransferOperatorVATIN = moneyTransferOperatorVatin,
                                MoneyTransferOperatorAddress = moneyTransferOperatorAddress
                            };
                        }
                        var ndPurveyorData = nd.SelectSingleNode("PurveyorData");
                        if (ndPurveyorData != null)
                        {
                            var purveyorPhone = ParseData.Parse(ndPurveyorData, "PurveyorPhone");
                            var purveyorName = ParseData.Parse(ndPurveyorData, "PurveyorName");
                            var purveyorVatin = ParseData.Parse(ndPurveyorData, "PurveyorVATIN");

                            item.PurveyorDataItem = new CheckPurveyorData
                            {
                                PurveyorPhone = purveyorPhone,
                                PurveyorName = purveyorName,
                                PurveyorVATIN = purveyorVatin
                            };
                        }

                        var goodCodeData = nd.SelectSingleNode("GoodCodeData");

                        if (goodCodeData != null)
                        {
                            var stampType = ParseData.Parse(goodCodeData, "StampType", true);
                            var stamp = ParseData.Parse(goodCodeData, "Stamp", true);
                            var gTin = ParseData.Parse(goodCodeData, "GTIN", true);
                            var serialNumber = ParseData.Parse(goodCodeData, "SerialNumber", true);

                            item.GoodCodeData = new CommodityNomenclatureCode
                            {
                                StampType = stampType,
                                Stamp = stamp,
                                Gtin = gTin,
                                SerialNumber = serialNumber
                            };
                        }

                        var measurementUnit = ParseData.Parse(nd, "MeasurementUnit", true);
                        item.MeasurementUnit = measurementUnit;

                        items.Add(item);
                    }
                    if (nd.Name == "TextString")
                    {
                        var item = new CheckItem
                        {
                            isFiscal = false,
                            After = new PrintLine(),
                            Before = new PrintLine(),
                            Name = ParseData.Parse(nd, "Text")
                        };
                        items.Add(item);
                    }
                    else if (nd.Name == "Barcode")
                    {
                        var barcodeType = ParseData.Parse(nd, "BarcodeType");
                        var barcode = ParseData.Parse(nd, "Barcode");

                        var item = new CheckItem();
                        item.isFiscal = false;
                        item.Barcode = new Barcode()
                        {
                            BarcodeType = barcodeType,
                            BarcodeText = barcode
                        };
                        items.Add(item);
                    }
                }

                _mainCheck.Check.CheckItems = items.ToArray();

                node = doc.DocumentElement.SelectSingleNode("Payments");

                // Атрибуты с ФФД 1.0.5
                // СуммаНаличными
                double cash = ParseData.ParseDouble(node, "Cash");
                // СуммаЭлектронными
                double electronicPayment = ParseData.ParseDouble(node, "ElectronicPayment");
                // СуммаПостоплатой
                double credit = ParseData.ParseDouble(node, "Credit");
                // СуммаПредоплатой
                double advancePayment = ParseData.ParseDouble(node, "AdvancePayment");
                // СуммаПредоставлением
                double cashProvision = ParseData.ParseDouble(node, "CashProvision");

                _mainCheck.Check.CashPayment = new Payment() { Summ = (decimal)cash };
                _mainCheck.Check.ElectronicPayment = new Payment() { Summ = (decimal)electronicPayment };
                _mainCheck.Check.CreditPayment = new Payment() { Summ = (decimal)credit };
                _mainCheck.Check.AdvancePayment = new Payment() { Summ = (decimal)advancePayment };
                _mainCheck.Check.CashProvisionPayment = new Payment() { Summ = (decimal)cashProvision };

                _mainCheck = РаспечататьЧек(_mainCheck);
                _mainCheckIsOpen = false;

                if (_mainCheck.КодОшибки == 0)
                {
                    sessionNumber = _mainCheck.Check.SessionNumber;
                    checkNumber = _mainCheck.Check.DocNumber;
                    fiscalSign = _mainCheck.Check.FiscalSign;
                    // Заглушка, для соответствию стандарту
                    addressSiteInspection = "www.nalog.ru";

                    return true;
                }
                else
                {
                    SetError(_mainCheck);
                    sessionNumber = 0;
                    addressSiteInspection = "";
                    checkNumber = 0;
                    fiscalSign = "";
                    return false;
                }
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                sessionNumber = 0;
                addressSiteInspection = "";
                checkNumber = 0;
                fiscalSign = "";
                return false;
            }

            //var CashierName = "тест";
            //return СформироватьЧекOld(DeviceID, CashierName, Electronically, CheckPackage, out CheckNumber, out SessionNumber, out FiscalSign, out AddressSiteInspection);
        }

        private void SetError(DockBase doc)
        {
            if (doc.КодОшибки != 0)
                SetError(doc.КодОшибки, doc.ОписаниеОшибки);
        }

        private static CheckTypes GetType(int paymentType)
        {
            CheckTypes type;
            // 1 - Приход
            // 2 - Возврат прихода
            // 3 - Расход
            // 4 - Возврат расхода
            switch (paymentType)
            {
                case 1:
                    type = CheckTypes.Продажа;
                    break;

                case 2:
                    type = CheckTypes.Возврат;
                    break;

                case 3:
                    type = CheckTypes.Покупка;
                    break;

                case 4:
                    type = CheckTypes.ВозвратПокупки;
                    break;

                default:
                    throw new ArgumentException(
                        $"Неверный тип чека, может быть только в диапазоне от 1..4. Значение =  {paymentType} ", nameof(paymentType));
            }

            return type;
        }

        #endregion FFD105

        #region FFD10

        /// <inheritdoc/>
        public bool ЗакрытьСмену(string deviceId, string cashierName, out int sessionNumber, out int documentNumber)
        {
            var rep = СоздатьОтчет();
            rep.Кассир = cashierName;
            rep.ТипОтчета = (int)ReportType.ZОтчет;
            rep.ИмяКассы = deviceId;
            var result = РаспечататьОтчет(rep);
            if (result.КодОшибки != 0)
            {
                sessionNumber = 0;
                documentNumber = 0;
                SetError(result.КодОшибки, result.ОписаниеОшибки);
                return false;
            }
            else
            {
                sessionNumber = result.НомерСмены;
                documentNumber = result.НомерЧека;
                return true;
            }
        }

        public bool ОткрытьСмену(string deviceId, string cashierName, out int sessionNumber, out int documentNumber)
        {
            try
            {
                Guid g = Guid.Parse(ПолучитьИдентификаторПоИмени(deviceId));

                OpenShift openShift = new OpenShift();
                if (!IsNullOrEmpty(cashierName))
                    openShift.Cashier = cashierName;

                openShift.DockId = Guid.NewGuid();
                openShift.DeviceId = g;
                openShift = SavePrintOpenShift(openShift);

                if (openShift.ResultCode != 0)
                {
                    SetError(openShift.ResultCode, openShift.ResultDescription);
                    sessionNumber = 0;
                    documentNumber = 0;
                    return false;
                }
                else
                {
                    sessionNumber = openShift.SessionNumber;
                    documentNumber = openShift.DocNumber;
                    return true;
                }
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                sessionNumber = 0;
                documentNumber = 0;
                return false;
            }
        }

        public bool ОтчетОТекущемСостоянииРасчетов(string deviceId)
        {
            var report = СоздатьОтчет();
            report.Report.ReportType = ReportType.ОтчетОТекущемСостоянииРасчетов;
            report.ИмяКассы = deviceId;
            return PrintReport(report);
        }

        public bool СформироватьЧек(string deviceId, string cashierName, bool electronically, string checkPackage,
            out int checkNumber, out int sessionNumber, out string fiscalSign, out string addressSiteInspection)
        {
            if (Log)
                WriteLog(new List<DataParam>()
                {
                    SetNameMetod(MethodBase.GetCurrentMethod().Name),
                    Set(nameof(deviceId), deviceId),
                    Set(nameof(cashierName), cashierName),
                    Set(nameof(electronically), electronically),
                    Set(nameof(checkPackage), checkPackage)
                });

            try
            {
                bool res;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(checkPackage);

                var node = doc.DocumentElement.SelectSingleNode("Parameters");
                int paymentType = ParseData.ParseInt(node, "PaymentType");

                _mainCheck = СоздатьЧек();
                _mainCheck.Фискальный = true;
                _mainCheckIsOpen = true;
                _mainCheck.ТипЧека = GetType(paymentType);
                _mainCheck.ИмяКассы = deviceId;
                _mainCheck.Check.Cashier = cashierName;
                _mainCheck.Check.AddressSettle = ParseData.Parse(node, "AddressSettle");
                if (node.Attributes?["AgentSign"]?.InnerText != null && node.Attributes?["AgentSign"]?.InnerText != Empty)
                {
                    _mainCheck.Check.AgentSign = ParseData.ParseInt(node, "AgentSign");
                }
                else
                {
                    _mainCheck.Check.AgentSign = null;
                }
                node = doc.DocumentElement.SelectSingleNode("Parameters/AgentData");
                if (node != null)
                {
                    _mainCheck.Check.AgentData = new CheckAgentData();
                    _mainCheck.Check.AgentData.PayingAgentOperation = ParseData.Parse(node, "PayingAgentOperation");
                    _mainCheck.Check.AgentData.PayingAgentPhone = ParseData.Parse(node, "PayingAgentPhone");
                    _mainCheck.Check.AgentData.ReceivePaymentsOperatorPhone = ParseData.Parse(node, "ReceivePaymentsOperatorPhone");
                    _mainCheck.Check.AgentData.MoneyTransferOperatorPhone = ParseData.Parse(node, "MoneyTransferOperatorPhone");
                    _mainCheck.Check.AgentData.MoneyTransferOperatorName = ParseData.Parse(node, "MoneyTransferOperatorName");
                    _mainCheck.Check.AgentData.MoneyTransferOperatorAddress = ParseData.Parse(node, "MoneyTransferOperatorAddress");
                    _mainCheck.Check.AgentData.MoneyTransferOperatorVATIN = ParseData.Parse(node, "MoneyTransferOperatorVATIN");
                }
                node = doc.DocumentElement.SelectSingleNode("Parameters/PurveyorData");
                if (node != null)
                {
                    _mainCheck.Check.PurveyorData = new CheckPurveyorData();
                    _mainCheck.Check.PurveyorData.PurveyorPhone = ParseData.Parse(node, "PurveyorPhone");
                    _mainCheck.Check.PurveyorData.PurveyorName = ParseData.Parse(node, "PurveyorName");
                    _mainCheck.Check.PurveyorData.PurveyorVATIN = ParseData.Parse(node, "PurveyorVATIN");
                }
                node = doc.DocumentElement.SelectSingleNode("Parameters");

                _mainCheck.Check.Cashier = cashierName;
                string email = ParseData.Parse(node, "CustomerEmail");
                string telephone = ParseData.Parse(node, "CustomerPhone");
                if (!IsNullOrEmpty(email))
                {
                    _mainCheck.Контакт = email;
                }
                else if (!IsNullOrEmpty(telephone))
                {
                    _mainCheck.Контакт = telephone;
                }

                _mainCheck.Check.TaxType = ParseData.ParseIntTax(node, "TaxVariant");
                _mainCheck.Check.CheckMode = electronically ? 0 : 1;
                var docx = doc.DocumentElement.SelectNodes("Positions");

                var items = new List<CheckItem>();
                foreach (XmlNode nd in docx[0].ChildNodes)
                {
                    if (nd.Name == "FiscalString")
                    {
                        string name = ParseData.Parse(nd, "Name");
                        double quantity = ParseData.ParseDouble(nd, "Quantity");
                        double price = ParseData.ParseDouble(nd, "Price");
                        double amount = ParseData.ParseDouble(nd, "Amount");
                        int department = ParseData.ParseInt(nd, "Department") == 0
                            ? 1
                            : ParseData.ParseInt(nd, "Department");
                        int tax = ParseData.ParseTax(nd, "Tax");

                        // Принудительное округление
                        quantity = Math.Round(quantity, 3, MidpointRounding.AwayFromZero);
                        price = Math.Round(price, 2, MidpointRounding.AwayFromZero);
                        amount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);

                        // Исправление ошибочных данных от 1С
                        if (Math.Abs(amount) < 0.001)
                            amount = Math.Round(quantity * price, 2, MidpointRounding.AwayFromZero);

                        double discountInfo = (amount - Math.Round(quantity * price, 2, MidpointRounding.AwayFromZero));
                        //   res = НапечататьФискСтроку(DeviceID, name, quantity, price, amount, department, tax);

                        //Данные Агента
                        int? agent = null;
                        if (nd.Attributes?["SignSubjectCalculationAgent"]?.InnerText != null && nd.Attributes?["SignSubjectCalculationAgent"]?.InnerText != Empty)
                        {
                            agent = ParseData.ParseInt(nd, "SignSubjectCalculationAgent");
                        }
                        var ndAgentData = nd.SelectSingleNode("AgentData");
                        string payingAgentOperation = ParseData.Parse(ndAgentData, "PayingAgentOperation");
                        string payingAgentPhone = ParseData.Parse(ndAgentData, "PayingAgentPhone");
                        string receivePaymentsOperatorPhone = ParseData.Parse(ndAgentData, "ReceivePaymentsOperatorPhone");
                        string moneyTransferOperatorPhone = ParseData.Parse(ndAgentData, "MoneyTransferOperatorPhone");
                        string moneyTransferOperatorName = ParseData.Parse(ndAgentData, "MoneyTransferOperatorName");
                        string moneyTransferOperatorAddress = ParseData.Parse(ndAgentData, "MoneyTransferOperatorAddress");
                        string moneyTransferOperatorVATIN = ParseData.Parse(ndAgentData, "MoneyTransferOperatorVATIN");
                        var ndPurveorData = nd.SelectSingleNode("PurveyorData");
                        string purveyorPhone = ParseData.Parse(ndPurveorData, "PurveyorPhone");
                        string purveyorName = ParseData.Parse(ndPurveorData, "PurveyorName");
                        string purveyorVATIN = ParseData.Parse(ndPurveorData, "PurveyorVATIN");

                        CheckItem item = new CheckItem
                        {
                            isFiscal = true,
                            After = new PrintLine(),
                            Before = new PrintLine(),
                            Quantity = (decimal)quantity,
                            Summ = (decimal)amount,
                            DiscountValue = 0,
                            DiscountInfoValue = (decimal)discountInfo,
                            Name = name,
                            Price = (decimal)price,
                            Department = department,
                            TaxValue = tax,
                            PaymentMode = PaymentModes.НеПрименяется,
                            ItemType = ItemTypes.НеПрименяется,
                            SignSubjectCalculationAgent = agent,
                            AgentDataItem = new CheckAgentData
                            {
                                PayingAgentOperation = payingAgentOperation,
                                PayingAgentPhone = payingAgentPhone,
                                ReceivePaymentsOperatorPhone = receivePaymentsOperatorPhone,
                                MoneyTransferOperatorPhone = moneyTransferOperatorPhone,
                                MoneyTransferOperatorAddress = moneyTransferOperatorAddress,
                                MoneyTransferOperatorName = moneyTransferOperatorName,
                                MoneyTransferOperatorVATIN = moneyTransferOperatorVATIN
                            },
                            PurveyorDataItem = new CheckPurveyorData
                            {
                                PurveyorName = purveyorName,
                                PurveyorPhone = purveyorPhone,
                                PurveyorVATIN = purveyorVATIN
                            }
                        };

                        // Скидка рассчитывается на сервере

                        items.Add(item);
                    }
                    else if (nd.Name == "TextString")
                    {
                        var item = new CheckItem();
                        item.isFiscal = false;
                        item.After = new PrintLine();
                        item.Before = new PrintLine();
                        item.Name = ParseData.Parse(nd, "Text");
                        items.Add(item);
                    }
                    else if (nd.Name == "Barcode")
                    {
                        var barcodeType = ParseData.Parse(nd, "BarcodeType");
                        var barcode = ParseData.Parse(nd, "Barcode");

                        var item = new CheckItem();
                        item.isFiscal = false;
                        item.Barcode = new Barcode()
                        {
                            BarcodeType = barcodeType,
                            BarcodeText = barcode
                        };
                        items.Add(item);
                    }
                }

                _mainCheck.Check.CheckItems = items.ToArray();

                node = doc.DocumentElement.SelectSingleNode("Payments");
                res = ЗакрытьЧек
                (
                    deviceId,
                    ParseData.ParseDouble(node, "Cash"),
                    ParseData.ParseDouble(node, "CashLessType1"),
                    ParseData.ParseDouble(node, "CashLessType2"),
                    ParseData.ParseDouble(node, "CashLessType3")
                );
                if (res)
                {
                    sessionNumber = _mainCheck.Check.SessionNumber;
                    checkNumber = _mainCheck.Check.DocNumber;
                    fiscalSign = _mainCheck.Check.FiscalSign;
                    // Заглушка, для соответствию стандарту
                    addressSiteInspection = "www.nalog.ru";

                    return true;
                }
                else
                {
                    sessionNumber = 0;
                    addressSiteInspection = "";
                    checkNumber = 0;
                    fiscalSign = "";
                    return false;
                }
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                sessionNumber = 0;
                addressSiteInspection = "";
                checkNumber = 0;
                fiscalSign = "";
                return false;
            }
        }

        public bool УстановитьКонтактКлиента(string contact)
        {
            try
            {
                _mainCheck.Контакт = contact;
                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Ошибка_установки_параметра, ex.Message);
                return false;
            }
        }

        public int УстановитьКонтактКлиента77(string contact)
        {
            return Convert.ToInt32(УстановитьКонтактКлиента(contact));
        }

        public bool ОтчетПоКассирам(string deviceId)
        {
            var report = СоздатьОтчет();
            report.Report.ReportType = ReportType.ОтчетПоКассирам;
            report.ИмяКассы = deviceId;
            return PrintReport(report);
        }

        public bool ОтчетПоСекциям(string deviceId)
        {
            var report = СоздатьОтчет();
            report.Report.ReportType = ReportType.ОтчетПоСекциям;
            report.ИмяКассы = deviceId;
            return PrintReport(report);
        }

        public bool НапечататьФискСтроку105(string идУстойства, string наименование, double количество, double цена,
            double сумма, long отдел, double ндс, int признакСпособаРасчета, int признакПредметаРасчета)
        {
            if (!_mainCheckIsOpen)
            {
                SetError(Error.Чек_не_открыт);
                return false;
            }

            _mainCheck.ДобавитьПозициюСУчетомСкидки(наименование, цена, количество, сумма, (int)отдел, ндс,
                признакСпособаРасчета, признакПредметаРасчета);
            return true;
        }

        public bool ЗакрытьЧек(string идУстойства, double наличнаяОплата, double оплатаКартой, double оплатаКредитом,
            double оплатаСертификатом)
        {
            return ЗакрытьЧек(идУстойства, наличнаяОплата, оплатаКартой, оплатаКредитом, оплатаСертификатом, 0);
        }

        public ЧекКоррекции СоздатьЧекКоррекции()
        {
            var чек = new ЧекКоррекции();

            DefaultInit(чек);
            return чек;
        }

        public ЧекКоррекции РаспечататьЧекКоррекции(ЧекКоррекции чек)
        {
            if (!СтатусПодключения)
            {
                чек.Check.ResultCode = -404;
                чек.Check.ResultDescription = "Ошибка подключения к серверу";
            }
            else
            {
                try
                {
                    SetId(чек, _client);
                    CheckCorrection check = чек.Check;
                    чек.Check = SavePrintCorrecion(check);
                }
                catch (Exception ex)
                {
                    чек.Check.ResultCode = -400;
                    чек.Check.ResultDescription = ex.Message;
                }
            }

            return чек;
        }

        private static void SetId(DockBase doc, PrintServiceClient client)
        {
            if (doc.УстройствоПечати == Guid.Empty.ToString())
                doc.УстройствоПечати = client.GetDeviceIdByDeviceName(doc.ИмяКассы).ToString();
            doc.ВерсияКлиента = VersionLib;
        }

        public bool ПолучитьСостояниеКассы(string идУстойства, out int номерЧека, out int номерСмены,
            out int статусСмены)
        {
            return ПолучитьТекущееСостояние(идУстойства, out номерЧека, out номерСмены, out статусСмены,
                out _);
        }

        public bool ОткрытьСмену105(string deviceId, string cashierName, string cashierVatin, out int documentNumber,
            out int sessionNumber)
        {
            return OpenShift105(deviceId, cashierName, cashierVatin, out _, out documentNumber, out sessionNumber);
        }

        #endregion FFD10

        /// <summary>
        /// Данные для записи логов
        /// </summary>
        private class DataParam
        {
            public object Data { get; set; }
            public string Name { get; set; }
        }

        #region ЧекКоррекции

        public void ОткрытьЧекКоррекции()
        {
            _corectionCheck = СоздатьЧекКоррекции();
            _corectionCheck.Check.TerminalId = _terminalId;
        }

        public bool УстановитьДатуДокументаОснованияДляКоррекции(DateTime date)
        {
            try
            {
                _corectionCheck.ДатаДокументаОснованияДляКоррекции = date;
                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool УстановитьНомерДокументаОснованияДляКоррекции(string number)
        {
            try
            {
                _corectionCheck.НомерДокументаОснованияДляКоррекции = number;
                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool УстановитьОписаниеКоррекции(string description)
        {
            try
            {
                _corectionCheck.ОписаниеКоррекции = description;

                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool УстановитьДляКоррекцииСуммыНалогов(double taxNone, double tax0, double tax10, double tax18,
            double tax110, double tax118)
        {
            try
            {
                _corectionCheck.СуммаНалогаБезНдс = taxNone;
                _corectionCheck.СуммаНалога0 = tax0;
                _corectionCheck.СуммаНалога10 = tax10;
                _corectionCheck.СуммаНалога18 = tax18;
                _corectionCheck.СуммаНалога110 = tax110;
                _corectionCheck.СуммаНалога118 = tax118;
                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool УстановитьТипКоррекции(int corretionType)
        {
            try
            {
                _corectionCheck.ТипКоррекции = corretionType;
                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool УстановитьДляКоррекцииТипНалогообложения(int snoType)
        {
            try
            {
                _corectionCheck.ТипНалогообложения = snoType;

                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool УстановитьДляКоррекцииТипЧека(int checkType)
        {
            try
            {
                _corectionCheck.ТипЧека = checkType;
                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool ДобавитьОплатыЧекаКоррекции(double наличнаяОплата, double оплатаЭлектронными, double оплатаКредитом,
            double предоплатаой, double представлением)
        {
            try
            {
                _corectionCheck.ДобавитьОплатуНаличными(наличнаяОплата);
                _corectionCheck.ДобавитьОплатуКартой(оплатаЭлектронными);
                _corectionCheck.ДобавитьОплатуКредит(оплатаКредитом);
                _corectionCheck.ДобавитьОплатуПредоплатой(предоплатаой);
                _corectionCheck.ДобавитьОплатуПредставлением(представлением);
                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool ЗакрытьЧекКоррекции(string deviceId, string кассир, string vatin, out int checkNumber,
            out int sessionNumber, out string fiscalSign)
        {
            try
            {
                _corectionCheck.ИмяКассы = deviceId;
                _corectionCheck.Кассир = кассир;
                _corectionCheck.ИннКассира = vatin;
                var res = РаспечататьЧекКоррекции(_corectionCheck);

                if (res.КодОшибки != 0)
                {
                    checkNumber = 0;
                    sessionNumber = 0;
                    fiscalSign = null;

                    SetError(res.КодОшибки, res.ОписаниеОшибки);
                    return false;
                }
                else
                {
                    checkNumber = res.НомерЧека;
                    sessionNumber = res.НомерСмены;
                    fiscalSign = res.ФискальныйПризнак;
                    return true;
                }
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                checkNumber = 0;
                sessionNumber = 0;
                fiscalSign = null;

                return false;
            }
            finally
            {
                _corectionCheck = null;
            }
        }

        #endregion ЧекКоррекции

        #region Kostanova

        public int ПолучитьКодОшибки()
        {
            return (int)_errorCode;
        }

        public string ПолучитьТекстОшибки()
        {
            return _errorString;
        }

        public int РаспечататьЧекАсинхронно(Чек чек)
        {
            var doc = РаспечататьЧек(чек);

            var status = doc.КодОшибки == 0;
            if (!status) SetError(doc.КодОшибки, doc.ОписаниеОшибки);

            return Get(status);
        }

        private static int Get(bool value)
        {
            return value ? 0 : 1;
        }

        public int ПроверитьЧек(string docId)
        {
            try
            {
                var doc = ПолучитьЧек(docId);
                var status = doc.КодОшибки == 0;
                if (!status) SetError(doc.КодОшибки, doc.ОписаниеОшибки);

                return Get(status);
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);

                return Get(false);
            }
        }

        public int УстройствоИсправно(string deviceId)
        {
            try
            {
                var deviceStatus =
                    _client.GetDeviceStatus(Guid.Parse(ПолучитьИдентификаторПоИмени(deviceId))) as AtolFRStatus;
                if (deviceStatus != null && deviceStatus.DeviceStatus != 0)
                {
                    SetError(deviceStatus.DeviceStatus, deviceStatus.DeviceStatusDescription);
                    return Convert.ToInt32(false);
                }
                var isOkey = deviceStatus.DeviceStatus == 0;

                return Convert.ToInt32(isOkey);
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return Convert.ToInt32(false);
            }
        }

        #endregion Kostanova

        public bool ДобавитьОплаты105(double наличнаяОплата, double оплатаЭлектронными, double оплатаКредитом, double предоплатаой, double представлением)
        {
            // дубль кода методов чеков коррекции
            // TODO рефакторинг
            try
            {
                _mainCheck.ДобавитьОплатуНаличными(наличнаяОплата);
                _mainCheck.ДобавитьОплатуКартой(оплатаЭлектронными);
                _mainCheck.ДобавитьОплатуКредит(оплатаКредитом);
                _mainCheck.ДобавитьОплатуПредоплатой(предоплатаой);
                _mainCheck.ДобавитьОплатуПредставлением(представлением);
                return true;
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                return false;
            }
        }

        public bool ЗакрытьЧек105(string deviceId, string кассир, string vatin, out int checkNumber, out int sessionNumber, out string fiscalSign)
        {
            // дубль кода методов чеков коррекции
            // TODO рефакторинг
            try
            {
                _mainCheck.ИмяКассы = deviceId;
                _mainCheck.Кассир = кассир;
                _mainCheck.ИннКассира = vatin;
                var res = РаспечататьЧек(_mainCheck);

                if (res.КодОшибки != 0)
                {
                    checkNumber = 0;
                    sessionNumber = 0;
                    fiscalSign = null;

                    SetError(res.КодОшибки, res.ОписаниеОшибки);
                    return false;
                }
                else
                {
                    checkNumber = res.НомерЧека;
                    sessionNumber = res.НомерСмены;
                    fiscalSign = res.ФискальныйПризнак;
                    return true;
                }
            }
            catch (Exception ex)
            {
                SetError(Error.Внутренняя_ошибка, ex.Message);
                checkNumber = 0;
                sessionNumber = 0;
                fiscalSign = null;

                return false;
            }
            finally
            {
                _corectionCheck = null;
            }
        }
    }
}