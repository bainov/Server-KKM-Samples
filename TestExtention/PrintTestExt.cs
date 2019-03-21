using ClientPrint.PrintServiceRef;
using System;
using System.IO;
using System.ServiceModel;
using System.Xml.Serialization;

namespace TestExtention
{
    public static class PrintTestExt
    {
        public static Check GetCheckFromXml(string textXml)
        {
            var serializer = new XmlSerializer(typeof(Check));
            using (TextReader reader = new StringReader(textXml))
            {
                var check = (Check)serializer.Deserialize(reader);
                // initilize
                check.DockId = Guid.NewGuid();
                check.ResultCode = 0;
                check.ResultDescription = string.Empty;

                return check;
            }
        }

        public static PrintServiceClient GetClient(string address, int port)
        {
            var client = new PrintServiceClient(GetBinding(), new EndpointAddress(string.Format("http://{0}:{1}/PrintService/", address, port)));
            return client;
        }

        public static CheckBase SetDeviceId(string deviceName, CheckBase doc, PrintServiceClient client)
        {
            doc.DeviceId = client.GetDeviceIdByDeviceName(deviceName);
            return doc;
        }

        public static ParamsConnect Get()
        {
            return new ParamsConnect
            {
                Addres = "localhost",
                Port = 4398,
                DeviceName = Devices.Shtrih.ToString(),
                Cashier = "Джуниор Девелоперов"
            };
        }

        public enum Devices
        {
            Atol,
            Shtrih,
            Viki,
            Fpint5200
        }

        public static string GetPaphSolution()
        {
            throw new NotImplementedException();
            //var path = "";
        }

        private static WSHttpBinding GetBinding()
        {
            // приватный метод
            // return WCFServer.Setbinding();

            // TODO Сделать так метода чтобы получать привязки во время тестирования из метода Setbinding() не нарушая инкапсуляцию
            var binding = new WSHttpBinding();
            // Чтобы принять чек с 400 позициями.
            binding.MaxReceivedMessageSize = 500000;
            binding.Security.Mode = SecurityMode.None;
            return binding;
        }
    }
}