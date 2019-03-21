using ClientPrint.PrintServiceRef;
using System;
using System.Runtime.InteropServices;

namespace ClientPrint.Документы
{
    /// <summary>
    ///  Представляет задание печати фикализации кассы
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class ПараметрыФискализации : DockBase
    {
        /// <summary>
        /// Предоставляет доступ к базовому документу
        /// </summary>
        public ParametersFiscal ParamFiscal
        {
            get { return Doc as ParametersFiscal; }
            set { Doc = value; }
        }

#pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена

        public ПараметрыФискализации()
#pragma warning restore CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена

        {
            Doc = new ParametersFiscal
            {
                DockId = Guid.NewGuid(),
                DocType = DocTypes.Отчет,
                DeviceId = Guid.Empty
            };
        }
    }
}