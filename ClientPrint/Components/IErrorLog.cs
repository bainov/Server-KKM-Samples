using System;
using System.Runtime.InteropServices;

#pragma warning disable 1591

namespace ClientPrint.Components
{
    [ComImport, Guid("3127CA40-446E-11CE-8135-00AA004BB851"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IErrorLog
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT AddError([In, MarshalAs(UnmanagedType.BStr)] string pszPropName,
                         [In, MarshalAs(UnmanagedType.Struct)] ref System.Runtime.InteropServices.ComTypes.EXCEPINFO pExcepInfo);
    }
}