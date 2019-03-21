using ClientPrint.PrintServiceRef;
using System;

#pragma warning disable 1591

namespace ClientPrint.Документы
{
    [Obsolete]
    public class Штрихкод
    {
        public BarcodeDoc Barcode;

        public Штрихкод()
        {
            Barcode = new BarcodeDoc
            {
                DockId = Guid.NewGuid(),
                DeviceId = Guid.Empty,
                DocType = DocTypes.Штрихкод,
                PrintPurpose = 1,
                Aligment = 1,
                LeftMargin = 0,
                Height = 51,
                PrintBarcodeText = 0,
                BarcodeControlCode = false,
                AutoSize = true,
                Scale = 100,
                ScaleVB = 100
            };
        }
    }
}