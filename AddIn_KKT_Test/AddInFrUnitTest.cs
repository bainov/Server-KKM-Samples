using AddinRBsoft;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestExtention;

namespace AddIn_KKT_Test
{
    ///   <summary>
    /// Сводное описание для UnitTestFr_2001
    /// </summary>
    [TestClass]
    public class AddInFrUnitTest
    {
        private RBsoftPrintServiceV3 Fr { get; set; }

        private string Device { get; set; }

        private bool Res
        {
            get { return res; }
            set
            {
                res = value;
                string errorDescription;
                long resCode = Fr.ПолучитьОшибку(out errorDescription);
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

            Fr = new RBsoftPrintServiceV3();
            string tableParam;
            Res = Fr.ПолучитьПараметры(out tableParam);
            Res = Fr.УстановитьПараметр("Adress", paramsConnect.Addres);
            Assert.IsTrue(Res, MessageError);
            Res = Fr.УстановитьПараметр("Port", paramsConnect.Port);
            Assert.IsTrue(Res, MessageError);
            Res = Fr.УстановитьПараметр("DeviceID", paramsConnect.DeviceName);
            Assert.IsTrue(Res, MessageError);
            string deviceName;
            Res = Fr.Подключить(out deviceName);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Fr.Отключить(Device);
        }

        public AddInFrUnitTest()
        {
            //
            // TODO: добавьте здесь логику конструктора
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Получает или устанавливает контекст теста, в котором предоставляются
        ///сведения о текущем тестовом запуске и обеспечивается его функциональность.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Дополнительные атрибуты тестирования

        //
        // При написании тестов можно использовать следующие дополнительные атрибуты:
        //
        // ClassInitialize используется для выполнения кода до запуска первого теста в классе
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // ClassCleanup используется для выполнения кода после завершения работы всех тестов в классе
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // TestInitialize используется для выполнения кода перед запуском каждого теста
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // TestCleanup используется для выполнения кода после завершения каждого теста
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion Дополнительные атрибуты тестирования

        [TestMethod]
        public void OpenShift()
        {
            Res = Fr.ОткрытьСмену(Device);

            Assert.IsTrue(Res, MessageError);
        }

        [TestMethod]
        public void OpenCheck()
        {
            bool isFiscal = true;
            bool isReturn = false;
            bool canceleOpenedCheck = true;
            int checkNumber;
            int sessionNumber;

            Res = Fr.ОткрытьЧек(Device, isFiscal, isReturn, canceleOpenedCheck, out checkNumber, out sessionNumber);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(checkNumber, 0);
            Assert.AreNotEqual(sessionNumber, 0);
        }

        [TestMethod]
        public void PrintFiscalString()
        {
            bool isFiscal = true;
            bool isReturn = false;
            bool canceleOpenedCheck = true;
            int checkNumber;
            int sessionNumber;

            Res = Fr.ОткрытьЧек(Device, isFiscal, isReturn, canceleOpenedCheck, out checkNumber, out sessionNumber);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(checkNumber, 0);
            Assert.AreNotEqual(sessionNumber, 0);

            string name = "Мясо";
            double quantity = 10;
            double price = 5;
            double amount = quantity * price;
            int department = 1;
            double tax = 10;

            Res = Fr.НапечататьФискСтроку(Device, name, quantity, price, amount, department, tax);

            Assert.IsTrue(Res, MessageError);

            string text = "Нефискальная строка";

            Res = Fr.НапечататьНефискСтроку(Device, text);

            Assert.IsTrue(Res, MessageError);

            double cash = 1000;
            double payByCard = 0;
            double payByCredit = 0;
            double payByCertificate = 0;

            Res = Fr.ЗакрытьЧек(Device, cash, payByCard, payByCredit, payByCertificate);

            Assert.IsTrue(Res, MessageError);
        }

        [TestMethod]
        public void PrintNonFiscalString()
        {
            bool isFiscal = false;
            bool isReturn = false;
            bool canceleOpenedCheck = true;
            int checkNumber;
            int sessionNumber;

            Res = Fr.ОткрытьЧек(Device, isFiscal, isReturn, canceleOpenedCheck, out checkNumber, out sessionNumber);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(checkNumber, 0);
            Assert.AreNotEqual(sessionNumber, 0);

            string text = "Нефискальная строка";

            Res = Fr.НапечататьНефискСтроку(Device, text);

            Assert.IsTrue(Res, MessageError);

            double cash = 100;
            double payByCard = 0;
            double payByCredit = 0;
            double payByCertificate = 0;

            Res = Fr.ЗакрытьЧек(Device, cash, payByCard, payByCredit, payByCertificate);

            Assert.IsTrue(Res, MessageError);
        }

        [TestMethod]
        public void PrintBarCode()
        {
            string barCodeType = "QR";
            string barCode = "НапечататьШтрихКод(Device,barCodeType,barCode)";

            Res = Fr.НапечататьШтрихКод(Device, barCodeType, barCode);

            Assert.IsTrue(Res, MessageError);
        }

        [TestMethod]
        public void CloseWithoutPosition()
        {
            double cash = 100;
            double payByCard = 0;
            double payByCredit = 0;
            double payByCertificate = 0;
            int checkNumber;
            int sessionMnumber;

            Res = Fr.ОткрытьЧек(ИДУстойства: Device,
                ФискальныйЧек: true,
                ЧекВозврата: false,
                АнулироватьОткрытыйЧек: false,
                НомерЧека: out checkNumber,
                НомерСмены: out sessionMnumber);
            Assert.IsTrue(Res, MessageError);

            Res = Fr.ЗакрытьЧек(Device, cash, payByCard, payByCredit, payByCertificate);

            Assert.IsFalse(Res);
            var error = "Пустой чек";
            Assert.IsTrue(MessageError.Contains(error));
        }

        [TestMethod]
        public void CancelCheck()
        {
            Res = Fr.ОтменитьЧек(Device);

            Assert.IsTrue(Res, MessageError);
        }

        [TestMethod]
        public void CashInOutcome()
        {
            double Amount = 5;

            Res = Fr.НапечататьЧекВнесенияВыемки(Device, Amount);

            Assert.IsTrue(Res, MessageError);
        }

        [TestMethod]
        public void PrintXReport()
        {
            Res = Fr.НапечататьОтчетБезГашения(Device);
            Assert.IsTrue(Res, MessageError);
        }

        [TestMethod]
        public void PrintZReport()
        {
            Res = Fr.НапечататьОтчетСГашением(Device);

            Assert.IsTrue(Res, MessageError);
        }

        [TestMethod]
        public void OpenCashDrawer()
        {
            Res = Fr.ОткрытьДенежныйЯщик(Device);

            Assert.IsTrue(Res, MessageError);
        }

        [TestMethod]
        public void GetLineLength()
        {
            int lineLength;

            Res = Fr.ПолучитьШиринуСтроки(Device, out lineLength);

            //if (lineLength > 48)
            //    Assert.Fail($"Слишком большая ширина строки {lineLength}");

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, lineLength);
        }

        [TestMethod]
        public void PrintCheckWithDicound()
        {
            bool isFiscal = true;
            bool isReturn = false;
            bool canceleOpenedCheck = true;
            int checkNumber;
            int sessionNumber;

            string name = "Мясо";
            double quantity = 28;
            double price = 251.01;
            double amount = 6676.87;

            int department = 1;
            double tax = 10;
            double cash = 6676.87;
            double payByCard = 0;
            double payByCredit = 0;
            double payByCertificate = 0;

            Res = Fr.ОткрытьЧек(Device, isFiscal, isReturn, canceleOpenedCheck, out checkNumber, out sessionNumber);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(checkNumber, 0);
            Assert.AreNotEqual(sessionNumber, 0);

            Res = Fr.НапечататьФискСтроку(Device, name, quantity, price, amount, department, tax);
            Assert.IsTrue(Res, MessageError);
            // скидка на чеке должна быть = 351.41
            string text = "Нефискальная строка";
            Res = Fr.НапечататьНефискСтроку(Device, text);
            Assert.IsTrue(Res, MessageError);

            Res = Fr.ЗакрытьЧек(Device, cash, payByCard, payByCredit, payByCertificate);

            Assert.IsTrue(Res, MessageError);
        }
    }
}