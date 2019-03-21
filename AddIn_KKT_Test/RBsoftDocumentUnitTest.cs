using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AddinRBsoft;
using TestExtention;

namespace AddIn_KKT_Test
{
    /// <summary>
    /// Сводное описание для RBsoftDocumentUnitTest
    /// </summary>
    [TestClass]
    public class RBsoftDocumentUnitTest
    {
        private RBsoftPrintServiceV3 Fr { get; set; }

        private ParamsConnect paramsConnect;

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
            paramsConnect = PrintTestExt.Get();
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

        [TestMethod]
        public void PrintCheck()
        {
            var check = Fr.СоздатьЧек();
            check.Фискальный = true;
            check.ТипНалогообложения = 1;
            check.Кассир = "Иван Иванов";
            check.ИннКассира = "0326031413";
            //TODO придумать предупреждение NRE

            check.Контакт = "mail@mail.ru";
            check.Департамент = 1;
            check.Электронный = false;

            check.Терминал = "test" + check.Терминал;
            check.ТипЧека = ClientPrint.PrintServiceRef.CheckTypes.Продажа;
            check.ДобавитьПозицию105("Доставка пакетов", 1, 1, -30, 1, -1, 4, 4);
            check.ДобавитьПозицию105("Пакет красный", 1, 1, 0.5, 1, 0, 4, 1);
            check.ДобавитьПозицию105("Пакет желтый", 1, 1, 0.5, 1, 10, 4, 1);
            check.ДобавитьПозицию105("Пакет синий", 1, 1, 0.5, 1, 18, 4, 4);
            check.ДобавитьПозицию105("Пакет оранжевый", 1, 1, 0.5, 1, 110, 4, 1);
            check.ДобавитьПозицию105("Пакет с принтом", 1, 1, 0.5, 1, 118, 4, 1);

            check.ДобавитьОплатуНаличными(510);

            var val = Fr.РаспечататьЧекАсинхронно(check);
            Res = val == 0 ? true : false;
            Assert.AreEqual(true, Res, MessageError);
        }

    }
}
