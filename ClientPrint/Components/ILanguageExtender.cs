using System;
using System.Runtime.InteropServices;

#pragma warning disable 1591

namespace ClientPrint.Components
{
    [Guid("AB634003-F13D-11d0-A459-004095E1DAEA")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ILanguageExtender
    {
        /// <summary>
        /// Регистрация компонента в 1C
        /// </summary>
        /// <param name="extensionName"></param>
        /// <remarks>
        /// <prototype>
        /// [helpstring("method RegisterExtensionAs")]
        /// HRESULT RegisterExtensionAs([in,out]BSTR *bstrExtensionName);
        /// </prototype>
        /// </remarks>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT RegisterExtensionAs([Out, MarshalAs(UnmanagedType.BStr)]out string extensionName);

        /// <summary>
        /// Возвращается количество свойств
        /// </summary>
        /// <param name="props">Количество свойств </param>
        /// <remarks>
        /// <prototype>
        /// HRESULT GetNProps([in,out]long *plProps);
        /// </prototype>
        /// </remarks>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT GetNProps(ref int props);

        /// <summary>
        /// Возвращает целочисленный идентификатор свойства, соответствующий
        /// переданному имени
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        /// <param name="propNum">Идентификатор свойства</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT FindProp([in]BSTR bstrPropName,[in,out]long *plPropNum);
        /// </prototype>
        /// </remarks>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT FindProp([Out, MarshalAs(UnmanagedType.BStr)]string propName, [In, MarshalAs(UnmanagedType.I8)] ref long propNum);

        /// <summary>
        /// Возвращает имя свойства, соответствующее
        /// переданному целочисленному идентификатору
        /// </summary>
        /// <param name="propNum">Идентификатор свойства</param>
        /// <param name="propAlias"></param>
        /// <param name="propName">Имя свойства</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT GetPropName([in]long lPropNum,[in]long lPropAlias,[in,out]BSTR *pbstrPropName);
        /// </prototype>
        /// </remarks>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT GetPropName(int propNum, int propAlias, [Out, MarshalAs(UnmanagedType.BStr)]out string propName);

        /// <summary>
        /// Возвращает значение свойства.
        /// </summary>
        /// <param name="propNum">Идентификатор свойства </param>
        /// <param name="propVal">Значение свойства</param>
        /// <remarks>
        /// <prototype>
        /// </prototype>
        /// </remarks>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT GetPropVal(int propNum, [Out, MarshalAs(UnmanagedType.Struct)]out object propVal);

        /// <summary>
        /// Устанавливает значение свойства.
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        /// <param name="propVal">Значение свойства</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT SetPropVal([in]long lPropNum,[in]VARIANT *varPropVal);
        /// </prototype>
        /// </remarks>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT SetPropVal(int propName, [In, MarshalAs(UnmanagedType.Struct)]ref object propVal);

        /// <summary>
        /// Определяет, можно ли читать значение свойства
        /// </summary>
        /// <param name="propNum"> Идентификатор свойства </param>
        /// <param name="propRead">Флаг читаемости</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT IsPropReadable([in]long lPropNum,[in,out]BOOL *pboolPropRead);
        /// </prototype>
        /// </remarks>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT IsPropReadable(int propNum, [Out, MarshalAs(UnmanagedType.Bool)] out bool propRead);

        /// <summary>
        /// Определяет, можно ли изменять значение свойства
        /// </summary>
        /// <param name="propNum">Идентификатор свойства</param>
        /// <param name="propWrite">Флаг записи</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT IsPropWritable([in]long lPropNum,[in,out]BOOL *pboolPropWrite);
        /// </prototype>
        /// </remarks>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT IsPropWritable(int propNum, [Out, MarshalAs(UnmanagedType.Bool)] out bool propWrite);

        /// <summary>
        /// Возвращает количество методов
        /// </summary>
        /// <param name="pMethods">Количество методов</param>
        /// <remarks>
        /// <prototype>
        /// [helpstring("method GetNMethods")]
        /// HRESULT GetNMethods([in,out]long *plMethods);
        /// </prototype>
        /// </remarks>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT GetNMethods(ref int pMethods);

        /// <summary>
        /// Возвращает идентификатор метода по его имени
        /// </summary>
        /// <param name="methodName">Имя метода</param>
        /// <param name="methodNum">Идентификатор метода</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT FindMethod(BSTR bstrMethodName,[in,out]long *plMethodNum);
        /// </prototype>
        /// </remarks>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT FindMethod([In, MarshalAs(UnmanagedType.BStr)]string methodName, out int methodNum);

        /// <summary>
        /// Возвращает имя метода по его идентификатору
        /// </summary>
        /// <param name="methodNum">Идентификатор метода</param>
        /// <param name="methodAlias"></param>
        /// <param name="methodName">Имя метода</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT GetMethodName([in]long lMethodNum,[in]long lMethodAlias,[in,out]BSTR *pbstrMethodName);
        /// </prototype>
        /// </remarks>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT GetMethodName(int methodNum, int methodAlias, [MarshalAs(UnmanagedType.BStr)]ref string methodName);

        /// <summary>
        /// Возвращает количество параметров метода по его идентификатору
        /// </summary>
        /// <param name="methodNum">Идентификатор метода</param>
        /// <param name="pParams">Количество параметров</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT GetNParams([in]long lMethodNum, [in,out]long *plParams);
        /// </prototype>
        /// </remarks>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT GetNParams(int methodNum, ref Int32 pParams);

        /// <summary>
        /// Возвращает значение параметра метода по умолчанию
        /// </summary>
        /// <param name="methodNum"></param>
        /// <param name="paramNum"></param>
        /// <param name="paramDefValue"></param>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT GetParamDefValue(int methodNum, Int32 paramNum, [MarshalAs(UnmanagedType.Struct)]ref object paramDefValue);

        /// <summary>
        /// Указывает, что у метода есть возвращаемое значение
        /// </summary>
        /// <param name="methodNum">Идентификатор метода</param>
        /// <param name="retValue">Наличие возвращаемого значения</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT HasRetVal([in]long lMethodNum,[in,out]BOOL *pboolRetValue);
        /// </prototype>
        /// </remarks>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT HasRetVal(Int32 methodNum, [MarshalAs(UnmanagedType.Bool)] ref bool retValue);

        /// <summary>
        /// Вызов метода как процедуры с использованием идентификатора
        /// </summary>
        /// <param name="methodNum">Идентификатор метода</param>
        /// <param name="pParams">Параметры</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT CallAsProc([in]long lMethodNum,[in] SAFEARRAY (VARIANT) *paParams);
        /// </prototype>
        /// </remarks>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT CallAsProc(Int32 methodNum, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)]ref object[] pParams);

        /// <summary>
        /// Вызов метода как функции с использованием идентификатора
        /// </summary>
        /// <param name="methodNum">Идентификатор метода</param>
        /// <param name="retValue">Возвращаемое значение</param>
        /// <param name="pParams">Параметры</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT CallAsFunc([in]long lMethodNum,[in,out] VARIANT *pvarRetValue,
        ///         [in] SAFEARRAY (VARIANT) *paParams);
        /// </prototype>
        /// </remarks>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT CallAsFunc(Int32 methodNum,
                        [MarshalAs(UnmanagedType.Struct)]ref object retValue,
                        [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)]ref object[] pParams);
    }
}