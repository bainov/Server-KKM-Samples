using ClientPrint.PrintServiceRef;
using System;
using System.Xml;

#pragma warning disable 1591

namespace ClientPrint
{
    /// <summary>
    /// Предоставляет статические методы для преобразования XML узлов к нужному типу данных
    /// </summary>
    public static class ParseData
    {
        private static string _buff;

        public static string Parse(XmlNode x, string attr, bool nullable = false)
        {
            if (x == null)
            {
                return nullable ? null : string.Empty;
            }

            _buff = x.Attributes?[attr]?.InnerText;
            if (string.IsNullOrEmpty(_buff))
            {
                return nullable ? null : string.Empty;
            }
            else
                return _buff;
        }

        public static int ParseTax(XmlNode x, string attr)
        {
            if (x == null) return 0;

            _buff = x.Attributes?[attr]?.InnerText;

            if (_buff == null) return 0;

            // Требования к разработке драйверов для ККТ с функцией передачи в ОФД
            // Поле Tax теперь типа string.
            //  "none" без НДС.
            // при обработке значения -1 предает драйверу значение "Без НДС"

            if (_buff.Equals("none", System.StringComparison.OrdinalIgnoreCase)) return -1;

            switch (_buff)
            {
                case "0": return 0;
                case "10": return 10;
                case "18": return 18;
                case "20": return 20;

                case "10/110": return 110;
                case "18/118": return 118;
                case "20/120": return 120;

                default:
                    throw new ArgumentException($"Не удалось распознать  значение налога, налог '{_buff}'. Допустимые значение описаны на https://its.1c.ru/db/content/metod8dev/src/developers/additional/guides/i8104829.htm?_=1542292881#checkcorrectionpackage");
            }
        }

        public static double ParseDouble(XmlNode x, string attr)
        {
            if (x == null)
            {
                return 0;
            }

            _buff = x.Attributes?[attr]?.InnerText;

            // Требования к разработке драйверов для ККТ с функцией передачи в ОФД
            // Поле Tax теперь типа string.
            //  "none" без НДС.
            // при обработке значения -1 предает драйверу значение "Без НДС"
            if (_buff == "none")
                return -1;

            double b = 0;
            if (!string.IsNullOrEmpty(_buff))
            {
                _buff = _buff.Replace('.', ',');
                double.TryParse(_buff, out b);
            }

            return b;
        }

        public static int ParseInt(XmlNode x, string attr)
        {
            if (x == null)
            {
                return 0;
            }

            _buff = x.Attributes?[attr]?.InnerText;
            int b = 0;
            if (!string.IsNullOrEmpty(_buff))
                int.TryParse(_buff, out b);
            return b;
        }

        public static bool ParseBool(XmlNode x, string attr)
        {
            if (x == null)
            {
                return false;
            }

            _buff = x.Attributes?[attr]?.InnerText;

            if (!string.IsNullOrEmpty(_buff))
                switch (_buff)
                {
                    case "false": return false;
                    case "true": return true;
                }
            return false;
        }

        public static TaxVariants ParseIntTax(XmlNode x, string attr)
        {
            if (x == null)
            {
                return TaxVariants.None;
            }

            _buff = x.Attributes?[attr]?.InnerText;
            int taxCode = 999;
            if (!string.IsNullOrEmpty(_buff))
                int.TryParse(_buff, out taxCode);

            switch (taxCode)
            {
                case 0: return TaxVariants.ОСН;
                case 1: return TaxVariants.УСН;
                case 2: return TaxVariants.УСНД_Р;
                case 3: return TaxVariants.ЕНВД;
                case 4: return TaxVariants.ЕСН;
                case 5: return TaxVariants.ПСН;
                default:
                    return TaxVariants.None;
            }
        }
    }
}