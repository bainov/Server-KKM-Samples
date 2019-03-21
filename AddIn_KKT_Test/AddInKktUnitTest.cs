using Addin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;
using TestExtention;

namespace AddIn_KKT_Test
{
    [TestClass]
    public class AddInKktUnitTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        private RBsoftPrintServiceKKTV3 Kkt { get; set; }

        private string Device { get; set; }

        private bool Res
        {
            get { return res; }
            set
            {
                res = value;
                string errorDescription;
                long resCode = Kkt.ПолучитьОшибку(out errorDescription);
                MessageError = $"{resCode} {errorDescription}";
                Console.WriteLine(MessageError);
            }
        }

        private string Cashier { get; set; }

        private string MessageError { get; set; }

        private bool res;

        [TestInitialize]
        public void Initialize()

        {
            var paramsConnect = PrintTestExt.Get();
            Device = paramsConnect.DeviceName;
            Cashier = paramsConnect.Cashier;

            Kkt = new RBsoftPrintServiceKKTV3();
            string tableParam;
            Res = Kkt.ПолучитьПараметры(out tableParam);
            Res = Kkt.УстановитьПараметр("Adress", paramsConnect.Addres);
            Assert.IsTrue(Res, MessageError);
            Res = Kkt.УстановитьПараметр("Port", paramsConnect.Port);
            Assert.IsTrue(Res, MessageError);
            Res = Kkt.УстановитьПараметр("DeviceID", paramsConnect.DeviceName);
            Assert.IsTrue(Res, MessageError);
            string deviceName;
            Res = Kkt.Подключить(out deviceName);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Kkt.Отключить(Device);
        }

        /// <summary>
        /// Сумма позиции равна нулю во входных от 1С
        /// Это плохие данные, но при этом печать должна пройти
        /// </summary>
        /// <remark>
        /// Установлено исправление данных на в драйвере ККТ
        /// </remark>
        [TestMethod]
        public void PrintCheckSummIsZero()
        {
            string checkPackage =
            "<CheckPackage>                                                                                                                                               " +
            "    <Parameters PaymentType=\"1\" TaxVariant=\"3\" SenderEmail=\"\" CustomerEmail=\"\" CustomerPhone=\"\"/>                                                            " +
            "    <Positions>                                                                                                                                              " +
            "        <FiscalString Name=\"SIM МТС РТК Мой Smart 30/220 мульт Томск + пополнение 220 р шт\" Quantity=\"1\" Price=\"30\" Amount=\"0\" Department=\"1\" Tax=\"none\"/>   " +
            "        <TextString Text=\"Серия/SN: 9138843703\"/>                                                                                                            " +
            "    </Positions>                                                                                                                                             " +
            "    <Payments Cash=\"30\" CashLessType1=\"0\" CashLessType2=\"0\" CashLessType3=\"0\"/>                                                                               " +
            "</CheckPackage>";

            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, Cashier, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(checkNumber, 0);
            Assert.AreNotEqual(sessionNumber, 0);
            Assert.IsNotNull(fiscalSign);
            Assert.IsNotNull(addressSiteInspection);
        }

        [TestMethod]
        public void PrintWithDiscount()
        {
            string checkPackage =
            "<CheckPackage>                                                                                                                                               " +
            "    <Parameters PaymentType=\"1\" TaxVariant=\"3\" SenderEmail=\"\" CustomerEmail=\"\" CustomerPhone=\"\"/>                                                            " +
            "    <Positions>                                                                                                                                              " +
            "        <FiscalString Name=\"Мяч\" Quantity=\"1\" Price=\"30\" Amount=\"45\" Department=\"1\" Tax=\"none\"/>   " +
            "    </Positions>                                                                                                                                             " +
            "    <Payments Cash=\"45\" CashLessType1=\"0\" CashLessType2=\"0\" CashLessType3=\"0\"/>                                                                               " +
            "</CheckPackage>";

            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, Cashier, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(checkNumber, 0);
            Assert.AreNotEqual(sessionNumber, 0);
            Assert.IsNotNull(fiscalSign);
            Assert.IsNotNull(addressSiteInspection);
        }

        [TestMethod]
        public void PrintCheck_ЧекККМ()
        {
            var checkPackage = "<CheckPackage>  <Parameters PaymentType =\"1\" TaxVariant=\"0\" CashierName=\"Федоров Николай Петрович\" SenderEmail=\"true\" CustomerEmail=\"\" CustomerPhone=\"\"/> <Positions>  <FiscalString Name =\"Блокнот 16 л. шт\" Quantity=\"10\" Price=\"0.25\" Amount=\"2.5\" Department=\"0\" Tax=\"none\"/> <FiscalString Name =\"Блокнот 16 л. шт\" Quantity=\"9\" Price=\"0.25\" Amount=\"2.25\" Department=\"0\" Tax=\"none\"/> <FiscalString Name =\"Пенал шт\" Quantity=\"8\" Price=\"0.92\" Amount=\"7.36\" Department=\"0\" Tax=\"none\"/> <FiscalString Name =\"Тетрадь 28 листов шт\" Quantity=\"7\" Price=\"0.87\" Amount=\"6.09\" Department=\"0\" Tax=\"none\"/> <FiscalString Name =\"Ручка BIC шт\" Quantity=\"6\" Price=\"0.98\" Amount=\"5.88\" Department=\"0\" Tax=\"none\"/><FiscalString Name =\"Ручка BIC шт\" Quantity=\"5\" Price=\"0.98\" Amount=\"4.9\" Department=\"0\" Tax=\"none\"/></Positions>  <Payments Cash =\"60\" CashLessType1=\"6\" CashLessType2=\"0\" CashLessType3=\"0\"/> </CheckPackage>";
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, Cashier, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(checkNumber, 0);
            Assert.AreNotEqual(sessionNumber, 0);
            Assert.IsNotNull(fiscalSign);
            Assert.IsNotNull(addressSiteInspection);
        }

        [TestMethod]
        public void PrintCheck_РеализациТоваровИУслуг()
        {
            var checkPackage = "<CheckPackage> <Parameters PaymentType=\"1\" TaxVariant=\"0\" CashierName=\"Федоров Николай Петрович\" SenderEmail=\"true\" CustomerEmail=\"\" CustomerPhone=\"\"/> <Positions> <FiscalString Name=\"[БезОп] Карандаш Eric шт\" Quantity=\"10\" Price=\"0.52\" Amount=\"5.2\" Department=\"0\" Tax=\"none\"/> <FiscalString Name=\"[БезОп] Ластик большой шт\" Quantity=\"9\" Price=\"0.43\" Amount=\"3.87\" Department=\"0\" Tax=\"0\"/> <FiscalString Name=\"[БезОп] Линейка 15 см шт\" Quantity=\"8\" Price=\"0.26\" Amount=\"2.08\" Department=\"0\" Tax=\"10\"/> <FiscalString Name=\"[БезОп] Пенал шт\" Quantity=\"7\" Price=\"0.92\" Amount=\"6.44\" Department=\"0\" Tax=\"18\"/> <FiscalString Name=\"[БезОп] Ручка BIC шт\" Quantity=\"6\" Price=\"156\" Amount=\"936\" Department=\"0\" Tax=\"10\"/> <FiscalString Name=\"[БезОп] Тетрадь 28 листов шт\" Quantity=\"5\" Price=\"0.87\" Amount=\"4.35\" Department=\"0\" Tax=\"18\"/> </Positions> <Payments Cash=\"0\" CashLessType1=\"0\" CashLessType2=\"957.94\" CashLessType3=\"0\"/> </CheckPackage>";
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, Cashier, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(checkNumber, 0);
            Assert.AreNotEqual(sessionNumber, 0);
            Assert.IsNotNull(fiscalSign);
            Assert.IsNotNull(addressSiteInspection);
        }

        [TestMethod]
        public void PrintCheck_ПКО()
        {
            var checkPackage = "<CheckPackage> <Parameters PaymentType=\"1\" TaxVariant=\"0\" CashierName=\"Федоров Николай Петрович\" SenderEmail=\"true\" CustomerEmail=\"\" CustomerPhone=\"\"/> <Positions> <FiscalString Name=\"[Кред] Принято от: ООО &quot;Рога и Копыта&quot; Основание: Шашалык и картошока\" Quantity=\"1\" Price=\"1\" Amount=\"1\" Department=\"0\" Tax=\"10\"/> </Positions> <Payments Cash=\"1\" CashLessType1=\"0\" CashLessType2=\"0\" CashLessType3=\"0\"/></CheckPackage>";
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, Cashier, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(checkNumber, 0);
            Assert.AreNotEqual(sessionNumber, 0);
            Assert.IsNotNull(fiscalSign);
            Assert.IsNotNull(addressSiteInspection);
        }

        [TestMethod]
        public void PrintCheck_РКО()
        {
            var checkPackage = "<CheckPackage> <Parameters PaymentType=\"2\" TaxVariant=\"0\" CashierName=\"Федоров Николай Петрович\" SenderEmail=\"true\" CustomerEmail=\"\" CustomerPhone=\"\"/> <Positions> <FiscalString Name=\"[ПрОпл] Выдать: ООО &quot;Рога и Копыта&quot; Основание: \" Quantity=\"1\" Price=\"0.5\" Amount=\"0.5\" Department=\"0\" Tax=\"0\"/> </Positions> <Payments Cash=\"0.5\" CashLessType1=\"0\" CashLessType2=\"0\" CashLessType3=\"0\"/> </CheckPackage>";
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, Cashier, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(checkNumber, 0);
            Assert.AreNotEqual(sessionNumber, 0);
            Assert.IsNotNull(fiscalSign);
            Assert.IsNotNull(addressSiteInspection);
        }

        [TestMethod]
        public void PrintText_With_OR()
        {
            string documentPackage = "<Document> <Positions> <TextString Text=\"Тестовая печать QR\"/> <Barcode BarcodeType=\"QR\" Barcode=\"http://www.rbsoft.ru/product/devicenet/buy/\"/> </Positions> </Document>";
            Res = Kkt.НапечататьТекстовыйДокумент(Device, documentPackage);
            Assert.IsTrue(Res, MessageError);
        }

        /// <summary>
        /// Данный стандарт определяет требования к ККТ на основании форматов фискальных документов версии «1.0»
        /// </summary>

        #region Standart Version v2.1 KKT

        [TestMethod]
        public void GetDataKKT()
        {
            //< Parameters
            //KKTNumber="2345234523452345"
            //KKTSerialNumber="412412412412412"
            //Fiscal="true"
            //FNSerialNumber="23523445"
            //OrganizationName="ООО ВЕКТОР"
            //VATIN="325435435223"
            //AddressSettle="Москва, Дмитровское шоссе д.6"
            //TaxVariant="0,3"
            //OfflineMode="false"
            //AutomaticMode="false"
            //AutomaticNumber=""
            //OFDVATIN="32456234523452"/>
            int length = 16;
            string tableParametesKKT;
            Res = Kkt.ПолучитьПараметрыККТ(Device, out tableParametesKKT);
            Console.WriteLine(tableParametesKKT);
            var doc = new XmlDocument();
            doc.LoadXml(tableParametesKKT);
            XmlElement root = doc.DocumentElement;
            // Обязательное наличие в структуре
            var KKTSerialNumber = root.GetAttribute("KKTSerialNumber");
            var FNSerialNumber = root.GetAttribute("FNSerialNumber");
            bool Fiscal = bool.Parse(root.GetAttribute("Fiscal"));

            // Необязательное наличие в структуре.
            var KKTNumber = root.GetAttribute("KKTNumber");
            var FFDVersionFN = root.GetAttribute("FFDVersionFN");
            var FFDVersionKKT = root.GetAttribute("FFDVersionKKT");
            var DocumentNumber = root.GetAttribute("DocumentNumber");
            var DateTimeParam = DateTime.Parse(root.GetAttribute("DateTime"));
            var OrganizationName = root.GetAttribute("OrganizationName");
            var VATIN = root.GetAttribute("VATIN");
            var AddressSettle = root.GetAttribute("AddressSettle");
            var TaxVariant = root.GetAttribute("TaxVariant");
            var OfflineMode = bool.Parse(root.GetAttribute("OfflineMode"));
            var ServiceSign = bool.Parse(root.GetAttribute("ServiceSign"));
            var BSOSing = bool.Parse(root.GetAttribute("BSOSing"));
            var CalcOnlineSign = bool.Parse(root.GetAttribute("CalcOnlineSign"));
            var AutomaticMode = root.GetAttribute("AutomaticMode");
            var AutomaticNumber = root.GetAttribute("AutomaticNumber");
            var OFDOrganizationName = root.GetAttribute("OFDOrganizationName");
            var OFDVATIN = root.GetAttribute("OFDVATIN");

            int docNumber = 0;
            int.TryParse(DocumentNumber, out docNumber);
            Assert.AreNotEqual(0, docNumber, "Номер документ не должен быть нулевым");
            Assert.IsTrue(Res, MessageError);
            Assert.AreEqual(length, KKTNumber.Length, $"Неверная длина {nameof(KKTNumber)} '{KKTNumber}'");
            Assert.AreEqual(length, FNSerialNumber.Length, $"Неверная длина {nameof(FNSerialNumber)} '{FNSerialNumber}'");
            Assert.IsNotNull(OFDOrganizationName, "Название ОФД не может быть пустым");
            Assert.IsNotNull(OFDVATIN, "ИНН ОФД не может быть пустым");
        }

        [TestMethod]
        public void OperationFN()
        {
            string parametersFiscal =
                " <Parameters " +
                " KKTNumber=\"2345234523452345\" " +
                " OrganizationName=\"ООО ВЕКТОР\" " +
                " VATIN=\"325435435223\" " +
                " AddressSettle=\"Москва, Дмитровское шоссе д.6\" " +
                " TaxVariant=\"0\" " +
                " OfflineMode=\"false\" " +
                " AutomaticMode=\"false\" " +
                " AutomaticNumber=\"\" " +
                " ReasonCode=\"1\" " +
                " OFDVATIN=\"32456234523452\"/> ";

            //Тип операции:
            //1 - Регистрация
            //2 - Изменение параметров регистрации
            //3 - Закрытие ФН
            long operationType = 2;
            Res = Kkt.ОперацияФН(Device, operationType, Cashier, parametersFiscal);

            Assert.IsTrue(Res, MessageError);
        }

        [TestMethod]
        public void OpenShift()
        {
            int checkNumber;
            int sessionNumber;
            Res = Kkt.ОткрытьСмену(Device, Cashier, out checkNumber, out sessionNumber);
            //if (!Res)
            //{
            //    Res = Kkt.ЗакрытьСмену(Device, Cashier, out checkNumber, out sessionNumber);
            //    if (Res)
            //        Res = Kkt.ОткрытьСмену(Device, Cashier, out checkNumber, out sessionNumber);
            //}
            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
        }

        [TestMethod]
        public void CloseShift()
        {
            int checkNumber;
            int sessionNumber;
            Res = Kkt.ЗакрытьСмену(Device, Cashier, out checkNumber, out sessionNumber);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
        }

        [TestMethod]
        public void ProcessCheck()
        {
            string checkPackage =
            "<CheckPackage>                                                                                                                                   " +
                "<Parameters PaymentType=\"1\" SenderEmail=\"info@1c.ru\" CustomerEmail=\"alex2000@mail.ru\" CustomerPhone=\"\" AgentCompensation=\"\" AgentPhone=\"\"/>      " +
                "<Positions>                                                                                                                                      " +
                    "<FiscalString Name=\"Макароны\" Quantity=\"1\" Price=\"16.75\" Amount=\"16.75\" Tax=\"10\"/>                                                               " +
                    "<FiscalString Name=\"Томатный сок\" Quantity=\"1\" Price=\"200\" Amount=\"200\" Tax=\"18\"/>                                                               " +
                    "<FiscalString Name=\"Алкоголь Шампрео 0.7\" Quantity=\"1\" Price=\"455\" Amount=\"455\" Tax=\"18\"/>                                                       " +
                    "<TextString Text=\"Дисконтная карта: 00002345\"/>                                                                                                  " +
                    "<Barcode BarcodeType=\"EAN13\" Barcode=\"2000021262157\"/>                                                                                           " +
                "</Positions>                                                                                                                                     " +
              "<Payments Cash=\"471.75\" CashLessType1=\"0\" CashLessType2=\"0\" CashLessType3=\"200\"/>                                                                " +
            "</CheckPackage>";

            //TODO добавить печать электронного чека
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, Cashier, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsNotNull(fiscalSign, "Фискальный признак не может быть пустым");
            Assert.IsNotNull(addressSiteInspection, "Адрес проверки чека не может быть пустым");
        }

        [TestMethod]
        public void ProcessCorrectionCheck()
        {
            string checkCorrectionPackage =
            "<CheckCorrectionPackage> " +
                "<Parameters PaymentType=\"1\"/> " +
                "<Payments Cash=\"200\"/> " +
            "</CheckCorrectionPackage>";

            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;

            Res = Kkt.СформироватьЧекКоррекции(Device, Cashier, checkCorrectionPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsNotNull(fiscalSign, "Фискальный признак не может быть пустым");
            Assert.IsNotNull(addressSiteInspection, "Адрес проверки чека не может быть пустым");
        }

        [TestMethod]
        public void PrintTextDocument()
        {
            string documentPackage =
            "<Document> " +
                "<Positions> " +
                    "<TextString Text=\"Участие в дисконтной системе\"/> " +
                    "<TextString Text=\"Дисконтная карта: 00002345\"/> " +
                    "<Barcode BarcodeType=\"EAN13\" Barcode=\"2000021262157\"/> " +
                "</Positions> " +
            "</Document>";

            Res = Kkt.НапечататьТекстовыйДокумент(Device, documentPackage);

            Assert.IsTrue(Res, MessageError);
        }

        [TestMethod]
        public void CashInOutcome()
        {
            // Сумма > = 0 - внесение, Сумма < 0 - выемка
            double cashIn = 5;
            double cashOun = -5;

            Res = Kkt.НапечататьЧекВнесенияВыемки(Device, cashIn);
            Assert.IsTrue(Res, MessageError);

            Res = Kkt.НапечататьЧекВнесенияВыемки(Device, cashOun);
            Assert.IsTrue(Res, MessageError);
        }

        [TestMethod]
        public void PrintXReport()
        {
            Res = Kkt.НапечататьОтчетБезГашения(Device);

            Assert.IsTrue(res, MessageError);
        }

        [TestMethod]
        public void GetCurrentStatus()
        {
            string statusParameters;
            int sessionState;
            int sessionNumber;
            int checkNumber;
            Res = Kkt.ПолучитьТекущееСостояние(Device, out checkNumber, out sessionNumber, out sessionState, out statusParameters);

            Assert.IsTrue(Res, MessageError);

            // Состояние смены
            // 1 - Закрыта
            // 2 - Открыта
            // 3 - Истекла
            bool sessionStateValid;
            switch (sessionState)
            {
                case 1: sessionStateValid = true; break;
                case 2: sessionStateValid = true; break;
                case 3: sessionStateValid = true; break;
                default:
                    sessionStateValid = false; break;
            }
            Assert.IsTrue(sessionStateValid, "Состояние смены может принимать значение от 1 до 3");
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
        }

        /// <summary>
        /// ОтчетОТекущемСостоянииРасчетов
        /// </summary>
        [TestMethod]
        public void ReportCurrentStatusOfSettlements()
        {
            Res = Kkt.ОтчетОТекущемСостоянииРасчетов(Device);

            Assert.IsTrue(res, MessageError);
        }

        [TestMethod]
        public void OpenCashDrawer()
        {
            Res = Kkt.ОткрытьДенежныйЯщик(Device);

            Assert.IsTrue(Res, MessageError);
        }

        [TestMethod]
        public void GetLineLenngth()
        {
            int lineLength;
            Res = Kkt.ПолучитьШиринуСтроки(Device, out lineLength);

            Assert.AreNotEqual(0, lineLength, "Получена неверная ширина строки");
            Assert.IsTrue(Res, MessageError);
        }

        #endregion Standart Version v2.1 KKT
    }
}