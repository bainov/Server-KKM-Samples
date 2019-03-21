using ClientPrint.Документы;

namespace ClientPrint
{
    public interface IKostanova
    {
        int ПолучитьКодОшибки();

        string ПолучитьТекстОшибки();

        int РаспечататьЧекАсинхронно(Чек чек);

        int УстройствоИсправно(string deviceId);

        int ПроверитьЧек(string docId);
    }
}