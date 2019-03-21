using ClientPrint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestExtention;

namespace AddIn_KKT_Test
{
    [TestClass]
    public class CommonInterfaceUnitTest
    {
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

        private bool Res
        {
            get { return res; }
            set
            {
                res = value;
                string errorDescription;
                long resCode = anyDevice1C.ПолучитьОшибку(out errorDescription);
                MessageError = $"{resCode} {errorDescription}";
                Console.WriteLine(MessageError);
            }
        }

        public string MessageError { get; private set; }

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

        private ClientPr anyDevice1C;

        private string Device;
        private bool res;

        [TestInitialize]
        public void Initialize()
        {
            var paramsConnect = PrintTestExt.Get();

            Device = paramsConnect.DeviceName;
            var Ardress = paramsConnect.Addres;
            int Port = paramsConnect.Port;
            anyDevice1C = new ClientPr();
            string tableParam;
            Res = anyDevice1C.ПолучитьПараметры(out tableParam);
            Res = anyDevice1C.УстановитьПараметр("Adress", Ardress);
            Assert.IsTrue(Res, MessageError);
            Res = anyDevice1C.УстановитьПараметр("Port", Port);
            Assert.IsTrue(Res, MessageError);
            Res = anyDevice1C.УстановитьПараметр("DeviceID", Device);
            Assert.IsTrue(Res, MessageError);
            Res = anyDevice1C.Инициализация();
            Assert.IsTrue(Res, MessageError);
        }

        [TestCleanup]
        public void Cleanup()
        {
            anyDevice1C.Отключить(Device);
        }

        [TestMethod]
        public void GetVersion()
        {
            var version = anyDevice1C.ПолучитьНомерВерсии();
            Assert.IsNotNull(version);
        }

        [TestMethod]
        public void GetLastError()
        {
            long error;
            string errorDescription;

            error = anyDevice1C.ПолучитьОшибку(out errorDescription);

            Assert.IsNotNull(errorDescription);
        }

        [TestMethod]
        public void GetDescription()
        {
            string nameDriver;
            string description;
            string equipmentType;
            int intefaceRevision;
            bool inegrationLibrary;
            bool mainDriverInsralled;
            string downloadURL;
            Res = anyDevice1C.ПолучитьОписание(out nameDriver, out description, out equipmentType, out intefaceRevision, out inegrationLibrary, out mainDriverInsralled, out downloadURL);

            Assert.IsFalse(Res);
        }

        [TestMethod]
        public void GetParameters()
        {
            // <?xml version="1.0" encoding="UTF-8" ?>
            //<Settings>
            //     <Page Caption="Параметры">
            //     <Group Caption="Параметры подключения">
            //          <Parameter Name="Model" Caption="Модель" TypeValue="Number"/>
            //          <Parameter Name="Port" Caption="Порт" TypeValue="Number" DefaultValue="0">
            //             <ChoiceList>
            //                 <Item Value="0">Клавиатура</Item>
            //                 <Item Value="1">COM1</Item>
            //                 <Item Value="2">COM2</Item>
            //              </ChoiceList>
            //          </Parameter>
            //          <Parameter Name="Parity" Caption="Четность" TypeValue="Boolean" DefaultValue="true"/>
            //          <Parameter Name="Speed" Caption="Скорость" TypeValue="Number" DefaultValue="1"/>
            //        </Group>
            //     </Page>
            //</Settings>

            string tableParameters;

            Res = anyDevice1C.ПолучитьПараметры(out tableParameters);

            Assert.IsTrue(Res, MessageError);
            Assert.IsNotNull(tableParameters);
        }

        [TestMethod]
        public void SetParameter()
        {
            var param = "DeviceId";
            var paramValue = "Atol";

            Res = anyDevice1C.УстановитьПараметр(param, paramValue);

            Assert.IsTrue(Res, MessageError);
        }

        [TestMethod]
        public void Open()
        {
            string text = "";
            Res = anyDevice1C.Подключить(out text);

            Assert.IsTrue(Res, MessageError);
            Assert.IsNotNull(text);
        }

        [TestMethod]
        public void Close()
        {
            string text = null;
            Res = anyDevice1C.Отключить(text);
            Assert.IsTrue(Res, MessageError);
        }

        [TestMethod]
        public void DeviceTest()
        {
            string testDescriptions;
            string demo;
            Res = anyDevice1C.ТестУстройства(out testDescriptions, out demo);

            Assert.IsTrue(Res, MessageError);
            Assert.IsNotNull(testDescriptions);
            Assert.IsNotNull(demo);
        }

        [TestMethod]
        public void GetAdditionalActions()
        {
            string tableAction;
            var res = anyDevice1C.ПолучитьДополнительныеДействия(out tableAction);

            Assert.IsTrue(res);
            Assert.IsNotNull(tableAction);
        }

        [TestMethod]
        public void DoAdditionalAction()
        {
            string actionName = null;
            var res = anyDevice1C.ВыполнитьДополнительноеДействие(actionName);

            Assert.IsFalse(res);
        }
    }
}