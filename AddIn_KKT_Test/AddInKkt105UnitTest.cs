using AddInRBSoft;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TestExtention;

namespace AddIn_KKT_Test
{
    [TestClass]
    public class AddInKkt105UnitTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        private RBSoftPrintServiceKKTv105 Kkt { get; set; }

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

            Kkt = new RBSoftPrintServiceKKTv105();
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

        [TestMethod]
        public void PrintCheck()
        {
            string checkPackage = File.ReadAllText(@"1C XML\nataliya profi.xml");
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsNotNull(fiscalSign, "Фискальный признак не может быть пустым");
            Assert.IsNotNull(addressSiteInspection, "Адрес проверки чека не может быть пустым");
        }

        [TestMethod]
        public void PrintNDS18118()
        {
            string checkPackage =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<CheckPackage>" +
                "	<Parameters PaymentType=\"1\" TaxVariant=\"1\" CashierName=\"\" CustomerEmail=\"\" CustomerPhone=\"\"/>" +
                "	<Positions>" +
                "		<FiscalString Name=\"Выдать:  Основание: \" Quantity=\"1\" PriceWithDiscount=\"1\" SumWithDiscount=\"1\" DiscountSum=\"\" Department=\"0\" Tax=\"18/118\" SignMethodCalculation=\"3\" SignCalculationObject=\"10\" TaxSum=\"0\"/>" +
                "	</Positions>" +
                "	<Payments Cash=\"1\" ElectronicPayment=\"0\" Credit=\"0\" AdvancePayment=\"0\" CashProvision=\"0\"/>" +
                "</CheckPackage>";
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsNotNull(fiscalSign, "Фискальный признак не может быть пустым");
            Assert.IsNotNull(addressSiteInspection, "Адрес проверки чека не может быть пустым");
        }

        [TestMethod]
        public void PrintNDS20()
        {
            string checkPackage =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<CheckPackage>" +
                "	<Parameters PaymentType=\"1\" TaxVariant=\"1\" CashierName=\"\" CustomerEmail=\"\" CustomerPhone=\"\"/>" +
                "	<Positions>" +
                "		<FiscalString Name=\"Выдать:  Основание: \" Quantity=\"1\" PriceWithDiscount=\"1\" SumWithDiscount=\"1\" DiscountSum=\"\" Department=\"0\" Tax=\"20\" SignMethodCalculation=\"3\" SignCalculationObject=\"10\" TaxSum=\"0\"/>" +
                "	</Positions>" +
                "	<Payments Cash=\"1\" ElectronicPayment=\"0\" Credit=\"0\" AdvancePayment=\"0\" CashProvision=\"0\"/>" +
                "</CheckPackage>";
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsNotNull(fiscalSign, "Фискальный признак не может быть пустым");
            Assert.IsNotNull(addressSiteInspection, "Адрес проверки чека не может быть пустым");
        }

        [TestMethod]
        public void PrintNDS120()
        {
            string checkPackage =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<CheckPackage>" +
                "	<Parameters PaymentType=\"1\" TaxVariant=\"1\" CashierName=\"\" CustomerEmail=\"\" CustomerPhone=\"\"/>" +
                "	<Positions>" +
                "		<FiscalString Name=\"Выдать:  Основание: \" Quantity=\"1\" PriceWithDiscount=\"1\" SumWithDiscount=\"1\" DiscountSum=\"\" Department=\"0\" Tax=\"20/120\" SignMethodCalculation=\"3\" SignCalculationObject=\"10\" TaxSum=\"0\"/>" +
                "	</Positions>" +
                "	<Payments Cash=\"1\" ElectronicPayment=\"0\" Credit=\"0\" AdvancePayment=\"0\" CashProvision=\"0\"/>" +
                "</CheckPackage>";
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsNotNull(fiscalSign, "Фискальный признак не может быть пустым");
            Assert.IsNotNull(addressSiteInspection, "Адрес проверки чека не может быть пустым");
        }

        [TestMethod]
        public void PrintCheckAlex()
        {
            string checkPackage =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<CheckPackage>" +
                "	<Parameters PaymentType=\"3\" TaxVariant=\"1\" CashierName=\"\" CustomerEmail=\"\" CustomerPhone=\"\"/>" +
                "	<Positions>" +
                "		<FiscalString Name=\"Выдать:  Основание: \" Quantity=\"1\" PriceWithDiscount=\"1\" SumWithDiscount=\"1\" DiscountSum=\"\" Department=\"0\" Tax=\"none\" SignMethodCalculation=\"3\" SignCalculationObject=\"10\" TaxSum=\"0\"/>" +
                "	</Positions>" +
                "	<Payments Cash=\"1\" ElectronicPayment=\"0\" Credit=\"0\" AdvancePayment=\"0\" CashProvision=\"0\"/>" +
                "</CheckPackage>";
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsNotNull(fiscalSign, "Фискальный признак не может быть пустым");
            Assert.IsNotNull(addressSiteInspection, "Адрес проверки чека не может быть пустым");
        }

        [TestMethod]
        public void PrintDiscount()
        {
            string checkPackage =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<CheckPackage>" +
                "	<Parameters PaymentType=\"1\" TaxVariant=\"1\" CashierName=\"Мухамед\" CustomerEmail=\"\" CustomerPhone=\"\"/>" +
                "	<Positions>" +
                "		<FiscalString Name=\"Мяч\" Quantity=\"1\" PriceWithDiscount=\"45\" SumWithDiscount=\"45\" DiscountSum=\"15\" Department=\"0\" Tax=\"none\" SignMethodCalculation=\"3\" SignCalculationObject=\"10\" TaxSum=\"0\"/>" +
                "	</Positions>" +
                "	<Payments Cash=\"45\" ElectronicPayment=\"0\" Credit=\"0\" AdvancePayment=\"0\" CashProvision=\"0\"/>" +
                "</CheckPackage>";
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsNotNull(fiscalSign, "Фискальный признак не может быть пустым");
            Assert.IsNotNull(addressSiteInspection, "Адрес проверки чека не может быть пустым");
        }

        /// <summary>
        /// Печать чека без признаков предмета расчета и признака расчета
        /// </summary>
        [TestMethod]
        public void PrintCheckWithouItemsAndSigns()
        {
            string checkPackage =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<CheckPackage>" +
                "	<Parameters PaymentType=\"3\" TaxVariant=\"1\" CashierName=\"\" CustomerEmail=\"\" CustomerPhone=\"\"/>" +
                "	<Positions>" +
                "		<FiscalString Name=\"Выдать:  Основание: \" Quantity=\"1\" PriceWithDiscount=\"1\" SumWithDiscount=\"1\" DiscountSum=\"\" Department=\"0\" Tax=\"none\"  TaxSum=\"0\"/>" +
                //"		<FiscalString Name=\"Выдать:  Основание: \" Quantity=\"1\" PriceWithDiscount=\"1\" SumWithDiscount=\"1\" DiscountSum=\"\" Department=\"0\" Tax=\"none\" SignMethodCalculation=\"3\" SignCalculationObject=\"10\" TaxSum=\"0\"/>" +
                "	</Positions>" +
                "	<Payments Cash=\"1\" ElectronicPayment=\"0\" Credit=\"0\" AdvancePayment=\"0\" CashProvision=\"0\"/>" +
                "</CheckPackage>";
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsNotNull(fiscalSign, "Фискальный признак не может быть пустым");
            Assert.IsNotNull(addressSiteInspection, "Адрес проверки чека не может быть пустым");
        }

        /// <summary>
        /// Расхождение и ленты ФП
        /// </summary>
        [TestMethod]
        public void PrintCheckTechnoopt()
        {
            string checkPackage = "<?xml version=\"1.0\" encoding=\"UTF-8\"?> <CheckPackage> <Parameters PaymentType=\"1\" TaxVariant=\"3\" CashierName=\"Администратор\" CustomerEmail=\"\" CustomerPhone=\"\"/> <Positions> <TextString Text=\"Документ №  ТХТ00005124         \"/> <FiscalString Name=\"Пакет Техноопт шт\" Quantity=\"1\" PriceWithDiscount=\"1\" SumWithDiscount=\"1\" DiscountSum=\"\" Department=\"1\" Tax=\"none\" SignMethodCalculation=\"4\" SignCalculationObject=\"1\" TaxSum=\"\"/> </Positions> <Payments Cash=\"0\" ElectronicPayment=\"1\" Credit=\"0\" AdvancePayment=\"0\" CashProvision=\"0\"/> </CheckPackage>";
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsNotNull(fiscalSign, "Фискальный признак не может быть пустым");
            Assert.IsNotNull(addressSiteInspection, "Адрес проверки чека не может быть пустым");
        }

        [TestMethod]
        public void OpenShift()
        {
            string paramOperation =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<CheckPackage>" +
                "	<Parameters  CashierName=\"Мужик\"  CashierVatin=\"123654789507\"/>" +
                "</CheckPackage>";
            int sessionNumber;
            int checkNumber;

            Res = Kkt.ОткрытьСмену(Device, paramOperation, out string paramState, out checkNumber, out sessionNumber);
            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsFalse(string.IsNullOrEmpty(paramState), "параметры сосотяния недоступны");
        }

        /// <summary>
        /// 3635
        /// </summary>
        [TestMethod]
        public void PrintCheck3635()
        {
            string checkPackage =
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
            "<CheckPackage>" +
            "	<Parameters PaymentType=\"1\" TaxVariant=\"1\" CashierName=\"\" CustomerEmail=\"\" CustomerPhone=\"\"/>" +
            "	<Positions>" +
            "		<FiscalString Name=\"Мясо\" Quantity=\"28\" PriceWithDiscount=\"238.46\" SumWithDiscount=\"6676.87\" DiscountSum=\"351.41\" Department=\"0\" Tax=\"none\" SignMethodCalculation=\"3\" SignCalculationObject=\"10\" TaxSum=\"0\"/>" +
            "	</Positions>" +
            "	<Payments Cash=\"0\" ElectronicPayment=\"6676.87\" Credit=\"0\" AdvancePayment=\"0\" CashProvision=\"0\"/>" +
            "</CheckPackage>";
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsNotNull(fiscalSign, "Фискальный признак не может быть пустым");
            Assert.IsNotNull(addressSiteInspection, "Адрес проверки чека не может быть пустым");
        }

        /// <summary>
        /// Печать чека с нулевой ценой
        /// </summary>
        [TestMethod]
        public void PrintCheckWithPrice0()
        {
            string checkPackage =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<CheckPackage>" +
                "	<Parameters PaymentType=\"1\" TaxVariant=\"1\" CashierName=\"\" CustomerEmail=\"\" CustomerPhone=\"\"/>" +
                "	<Positions>" +
                "		<FiscalString Name=\"Мясо подарочное\" Quantity=\"1\" PriceWithDiscount=\"0\" SumWithDiscount=\"0\" DiscountSum=\"0\" Department=\"0\" Tax=\"none\" SignMethodCalculation=\"4\" SignCalculationObject=\"1\" TaxSum=\"0\"/>" +
                "		<FiscalString Name=\"Мясо \" Quantity=\"10\" PriceWithDiscount=\"200\" SumWithDiscount=\"2000\" DiscountSum=\"0\" Department=\"0\" Tax=\"none\" SignMethodCalculation=\"4\" SignCalculationObject=\"1\" TaxSum=\"0\"/>" +
                "	</Positions>" +
                "	<Payments Cash=\"0\" ElectronicPayment=\"2000\" Credit=\"0\" AdvancePayment=\"0\" CashProvision=\"0\"/>" +
                "</CheckPackage>";
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsNotNull(fiscalSign, "Фискальный признак не может быть пустым");
            Assert.IsNotNull(addressSiteInspection, "Адрес проверки чека не может быть пустым");
        }

        /// <summary>
        /// Все чеки печаются в один отдел/секцию/департамент через драйвер Атол 10
        /// </summary>
        [TestMethod]
        public void PrintCheckDepartmnt()
        {
            string checkPackage0 =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<CheckPackage>" +
                "	<Parameters PaymentType=\"1\" TaxVariant=\"1\" CashierName=\"\" CustomerEmail=\"\" CustomerPhone=\"\"/>" +
                "	<Positions>" +
                "		<FiscalString Name=\"Выдать:  Основание: \" Quantity=\"1\" PriceWithDiscount=\"1\" SumWithDiscount=\"1\" DiscountSum=\"\" Department=\"1\" Tax=\"none\" SignMethodCalculation=\"3\" SignCalculationObject=\"10\" TaxSum=\"0\"/>" +
                "	</Positions>" +
                "	<Payments Cash=\"1\" ElectronicPayment=\"0\" Credit=\"0\" AdvancePayment=\"0\" CashProvision=\"0\"/>" +
                "</CheckPackage>";

            string checkPackage1 =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<CheckPackage>" +
                "	<Parameters PaymentType=\"1\" TaxVariant=\"1\" CashierName=\"\" CustomerEmail=\"\" CustomerPhone=\"\"/>" +
                "	<Positions>" +
                "		<FiscalString Name=\"Выдать:  Основание: \" Quantity=\"1\" PriceWithDiscount=\"1\" SumWithDiscount=\"1\" DiscountSum=\"\" Department=\"2\" Tax=\"none\" SignMethodCalculation=\"3\" SignCalculationObject=\"10\" TaxSum=\"0\"/>" +
                "	</Positions>" +
                "	<Payments Cash=\"1\" ElectronicPayment=\"0\" Credit=\"0\" AdvancePayment=\"0\" CashProvision=\"0\"/>" +
                "</CheckPackage>";

            string checkPackage2 =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<CheckPackage>" +
                "	<Parameters PaymentType=\"1\" TaxVariant=\"1\" CashierName=\"\" CustomerEmail=\"\" CustomerPhone=\"\"/>" +
                "	<Positions>" +
                "		<FiscalString Name=\"Выдать:  Основание: \" Quantity=\"1\" PriceWithDiscount=\"1\" SumWithDiscount=\"1\" DiscountSum=\"\" Department=\"3\" Tax=\"none\" SignMethodCalculation=\"3\" SignCalculationObject=\"10\" TaxSum=\"0\"/>" +
                "	</Positions>" +
                "	<Payments Cash=\"1\" ElectronicPayment=\"0\" Credit=\"0\" AdvancePayment=\"0\" CashProvision=\"0\"/>" +
                "</CheckPackage>";

            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, elect, checkPackage0, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);
            Assert.IsTrue(Res, MessageError);

            Res = Kkt.СформироватьЧек(Device, elect, checkPackage1, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);
            Assert.IsTrue(Res, MessageError);

            Res = Kkt.СформироватьЧек(Device, elect, checkPackage2, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);
            Assert.IsTrue(Res, MessageError);

            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsNotNull(fiscalSign, "Фискальный признак не может быть пустым");
            Assert.IsNotNull(addressSiteInspection, "Адрес проверки чека не может быть пустым");

            // Отчет по секциям показывает, что в все чеки в одной секции

            // Исправление в 10 драйвере,  при формировании позиции чека

            // Секции  0 и 1 группируются  в 1 секцию
        }

        /// <summary>
        /// Ошибка - неверная величина скидка/надбавка атол 10
        /// </summary>
        [TestMethod]
        public void PrintDiscountAtol10()
        {
            string checkPackage =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<CheckPackage>" +
                "	<Parameters PaymentType=\"1\" TaxVariant=\"1\" CashierName=\"\" CustomerEmail=\"\" CustomerPhone=\"\"/>" +
                "	<Positions>" +
                "		<FiscalString Name=\"Наклейка (стикер) виниловая\" Quantity=\"5\" PriceWithDiscount=\"1\" SumWithDiscount=\"120\" DiscountSum=\"200\" Department=\"0\" Tax=\"none\" SignMethodCalculation=\"3\" SignCalculationObject=\"10\" TaxSum=\"0\"/>" +
                "	</Positions>" +
                "	<Payments Cash=\"120\" ElectronicPayment=\"0\" Credit=\"0\" AdvancePayment=\"0\" CashProvision=\"0\"/>" +
                "</CheckPackage>";
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsNotNull(fiscalSign, "Фискальный признак не может быть пустым");
            Assert.IsNotNull(addressSiteInspection, "Адрес проверки чека не может быть пустым");
            //Атол 10 надбавка не может превышать цену позиции
        }

        /// <summary>
        /// Печать чека с данными агента/поставщика
        /// </summary>
        [TestMethod]
        public void PrintCheckWithAgentData()
        {
            string checkPackage = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<CheckPackage>" +
                "	<Parameters PaymentType=\"1\" TaxVariant=\"1\" CashierName=\"Администратор\" CustomerEmail=\"\" CustomerPhone=\"\" AgentSign=\"0\">" +
                "		<AgentData PayingAgentOperation=\"Перевод\" PayingAgentPhone=\"89839832112\" MoneyTransferOperatorPhone=\"323232\" MoneyTransferOperatorName=\"ООО МТС платеж\" MoneyTransferOperatorAddress=\"ул Туполева 142\" MoneyTransferOperatorVATIN=\"1524182342\"/>" +
                "		<PurveyorData PurveyorPhone=\"303030\" PurveyorName=\"ООО МТС\" PurveyorVATIN=\"4112689026\"/>" +
                "	</Parameters>" +
                "	<Positions>" +
                "		<FiscalString Name=\"Оплата от: Общество с ограниченной ответственностью &quot;Кафе &quot;Сказка&quot; Основание: \" Quantity=\"1\" PriceWithDiscount=\"28000\" SumWithDiscount=\"28000\" DiscountSum=\"\" Department=\"0\" Tax=\"20\" SignMethodCalculation=\"3\" SignCalculationObject=\"10\" TaxSum=\"4666.666666666666666666666666667\" SignSubjectCalculationAgent=\"0\">" +
                "			<AgentData PayingAgentOperation=\"Перевод\" PayingAgentPhone=\"89839832112\" MoneyTransferOperatorPhone=\"323232\" MoneyTransferOperatorName=\"ООО МТС платеж\" MoneyTransferOperatorAddress=\"ул туполева 142\" MoneyTransferOperatorVATIN=\"1524182342\"/>" +
                "			<PurveyorData PurveyorPhone=\"303030\" PurveyorName=\"ООО МТС\" PurveyorVATIN=\"4112689026\"/>" +
                "		</FiscalString>" +
                "		<FiscalString Name=\"Оплата от: Общество с ограниченной ответственностью &quot;Кафе &quot;Рассказка&quot; Основание: \" Quantity=\"0.499\" PriceWithDiscount=\"28000\" SumWithDiscount=\"56001.99\" DiscountSum=\"\" Department=\"0\" Tax=\"20\" SignMethodCalculation=\"3\" SignCalculationObject=\"10\" SignSubjectCalculationAgent=\"0\" TaxSum=\"4666.666666666666666666666666667\">" +
                "			<AgentData PayingAgentOperation=\"Перевод\" PayingAgentPhone=\"89839832112\" MoneyTransferOperatorPhone=\"323232\" MoneyTransferOperatorName=\"ООО МТС платеж\" MoneyTransferOperatorAddress=\"ул туполева 142\" MoneyTransferOperatorVATIN=\"1524182342\"/>" +
                "			<PurveyorData PurveyorPhone=\"303030\" PurveyorName=\"ООО МТС\" PurveyorVATIN=\"4112689026\"/>" +
                "		</FiscalString>" +
                "	</Positions>" +
                "	<Payments Cash=\"112000\" ElectronicPayment=\"0\" Credit=\"0\" AdvancePayment=\"0\" CashProvision=\"0\"/>" +
                "</CheckPackage>";
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsNotNull(fiscalSign, "Фискальный признак не может быть пустым");
            Assert.IsNotNull(addressSiteInspection, "Адрес проверки чека не может быть пустым");
        }

        /// <summary>
        /// Регистрация/перерегистрация ккм, закрытие фн
        /// </summary>
        [TestMethod]
        public void RegistrationKKT()
        {
            string parametersFiscal = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                 "<Parameters CashierName=\"Администратор\" CashierVATIN =\"\" KKTNumber=\"0000000000007620\"" +
                 " OFDVATIN=\"7728699517\" OFDOrganizationName=\"ООО Ярус\" OrganizationName=\"РБ СОФТ\"" +
                 " VATIN=\"0326031413\" AddressSettle=\"670000, Республика Бурятия, г. Улан-Удэ, ул. Балтахинова, дом 17 Е, офис 215\"" +
                 " TaxVariant=\"0,1,2,3,4,5\" OfflineMode=\"false\" DataEncryption=\"true\" ServiceSign=\"true\"" +
                 " BSOSing=\"false\" CalcOnlineSign=\"true\" AutomaticMode=\"true\" AutomaticNumber=\"10\" PlaceSettle=\"офис 216\"" +
                 " SaleExcisableGoods=\"true\" SignOfGambling=\"false\" SignOfLottery=\"false\" SignOfAgent=\"0,1,2,3,4,5,6\"" +
                 " PrinterAutomatic=\"false\" ReasonCode=\"4\" SenderEmail=\"rbsoft@rbsoft.ru\" FNSWebSite=\"nalog.ru\"/>";

            Res = Kkt.ОперацияФН(Device, 2, parametersFiscal);
            Assert.IsTrue(Res, MessageError);
        }

        [TestMethod]
        public void PrintCodeNomenclature()
        {
            string checkPackage =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<CheckPackage>" +
                "	<Parameters PaymentType=\"1\" TaxVariant=\"1\" CashierName=\"\" CustomerEmail=\"\" CustomerPhone=\"\"/>" +
                "	<Positions>" +
                "		<FiscalString Name=\"Сигареты Winstone\" Quantity=\"1\" PriceWithDiscount=\"100\" SumWithDiscount=\"100\" DiscountSum=\"0\" Department=\"0\" Tax=\"none\" SignMethodCalculation=\"3\" SignCalculationObject=\"4\" TaxSum=\"0\">" +
                "           <GoodCodeData StampType=\"05\" SerialNumber=\"ABC1234\" GTIN=\"98765432101234\"  />" +
                    " </FiscalString>" +
                //"	<Positions>" +
                //"		<FiscalString Name=\"Антибиотик\" Quantity=\"1\" PriceWithDiscount=\"1000\" SumWithDiscount=\"1000\" DiscountSum=\"0\" Department=\"0\" Tax=\"none\" SignMethodCalculation=\"3\" SignCalculationObject=\"4\" TaxSum=\"0\">" +
                //"       <GoodCodeData StampType=\"05\" SerialNumber\"ABC1234\" GTIN=\"98765432101234\"  />" +
                //" </FiscalString>" +
                "		<FiscalString Name=\"Шуба\" Quantity=\"1\" PriceWithDiscount=\"100\" SumWithDiscount=\"100\" DiscountSum=\"19000\" Department=\"0\" Tax=\"none\" SignMethodCalculation=\"3\" SignCalculationObject=\"4\" TaxSum=\"0\">" +
                "           <GoodCodeData StampType=\"02\" SerialNumber=\"RU-430302-ABC1234567\" GTIN=\"98765432101234\"  />" +
                "        </FiscalString>" +
                "	</Positions>" +
                "<Payments Cash=\"200\" ElectronicPayment=\"0\" Credit=\"0\" AdvancePayment=\"0\" CashProvision=\"0\"/>" +
                "</CheckPackage>";
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsNotNull(fiscalSign, "Фискальный признак не может быть пустым");
            Assert.IsNotNull(addressSiteInspection, "Адрес проверки чека не может быть пустым");
        }

        /// <summary>
        /// Ошибка - неверная величина скидка/надбавка атол 10
        /// </summary>
        [TestMethod]
        public void PrintBarCode()
        {
            string checkPackage =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<CheckPackage>" +
                "	<Parameters PaymentType=\"1\" TaxVariant=\"1\" CashierName=\"\" CustomerEmail=\"\" CustomerPhone=\"\"/>" +
                "	<Positions>" +
                "		<FiscalString Name=\"Наклейка (стикер) виниловая\" Quantity=\"5\" PriceWithDiscount=\"1\" SumWithDiscount=\"120\" DiscountSum=\"200\" Department=\"0\" Tax=\"none\" SignMethodCalculation=\"3\" SignCalculationObject=\"10\" TaxSum=\"0\"/>" +
                "       <Barcode BarcodeType=\"EAN8\" Barcode=\"9031101\"/>" +
                "       <TextString Text=\"EAN8: 9031101\"/>" +
                "       <Barcode BarcodeType=\"EAN13\" Barcode=\"2000021262157\"/>" +
                "       <TextString Text=\"EAN13: 2000021262157\"/>" +
                "       <Barcode BarcodeType=\"CODE39\" Barcode=\"123456\"/>" +
                "       <TextString Text=\"CODE39: 123456\"/>" +
                "       <Barcode BarcodeType=\"QR\" Barcode=\"www.rbsoft.ru\"/>	" +
                "       <TextString Text=\"QR: www.rbsoft.ru\"/>" +
                "	</Positions>" +
                "	<Payments Cash=\"120\" ElectronicPayment=\"0\" Credit=\"0\" AdvancePayment=\"0\" CashProvision=\"0\"/>" +
                "</CheckPackage>";
            var elect = false;
            int checkNumber;
            int sessionNumber;
            string fiscalSign;
            string addressSiteInspection;
            Res = Kkt.СформироватьЧек(Device, elect, checkPackage, out checkNumber, out sessionNumber, out fiscalSign, out addressSiteInspection);

            Assert.IsTrue(Res, MessageError);
            Assert.AreNotEqual(0, checkNumber, "Номер чека не может быть равным нулю");
            Assert.AreNotEqual(0, sessionNumber, "Номер смены не может быть равным нулю");
            Assert.IsNotNull(fiscalSign, "Фискальный признак не может быть пустым");
            Assert.IsNotNull(addressSiteInspection, "Адрес проверки чека не может быть пустым");
            //Атол 10 надбавка не может превышать цену позиции
        }

        [TestMethod]
        public void PrintTextDocumentWithBarCode()
        {
            string docPackage =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<Document>" +
                "	<Positions>" +
                "       <Barcode BarcodeType=\"EAN8\" Barcode=\"9031101\"/>" +
                "       <TextString Text=\"EAN8: 9031101\"/>" +
                "       <Barcode BarcodeType=\"EAN13\" Barcode=\"2000021262157\"/>" +
                "       <TextString Text=\"EAN13: 2000021262157\"/>" +
                "       <Barcode BarcodeType=\"CODE39\" Barcode=\"123456\"/>" +
                "       <TextString Text=\"CODE39: 123456\"/>" +
                "       <Barcode BarcodeType=\"QR\" Barcode=\"www.rbsoft.ru\"/>	" +
                "       <TextString Text=\"QR: www.rbsoft.ru\"/>" +
                "	</Positions>" +
                "</Document>";

            Res = Kkt.НапечататьТекстовыйДокумент(Device, docPackage);

            Assert.IsTrue(Res, MessageError);
        }
    }
}