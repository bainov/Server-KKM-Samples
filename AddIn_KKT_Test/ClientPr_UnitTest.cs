using ClientPrint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AddIn_KKT_Test
{
    [TestClass]
    public class ClientPrUnitTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        private ClientPr client { get; set; }

        private string Device { get; set; }

        private bool Res
        {
            get { return res; }
            set
            {
                res = value;
                string errorDescription;
                long resCode = client.ПолучитьОшибку(out errorDescription);
                MessageError = $"{resCode} {errorDescription}";
                Console.WriteLine(MessageError);
            }
        }

        private string Cashier { get; set; }

        private string MessageError { get; set; }

        private bool res;

        [TestInitialize]
        [DataSource(@"Provider=Microsoft.SqlServerCe.Client.4.0; Data Source=C:\test.db;", "Device")]
        public void Initialize()
        {
            var Port = 4398;
            var Ardress = "localhost";
            Device = "Atol";

            Cashier = "Джиниор Девелоперов";

            client = new ClientPr();
            string tableParam;
            Res = client.ПолучитьПараметры(out tableParam);
            Res = client.УстановитьПараметр("Adress", Ardress);
            Assert.IsTrue(Res, MessageError);
            Res = client.УстановитьПараметр("Port", Port);
            Assert.IsTrue(Res, MessageError);
            Res = client.УстановитьПараметр("DeviceID", Device);
            Assert.IsTrue(Res, MessageError);
            Res = client.Инициализация();
        }

        [TestCleanup]
        public void Cleanup()
        {
            client.Close(Device);
        }
    }
}