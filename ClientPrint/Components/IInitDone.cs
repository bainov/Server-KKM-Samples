using System;
using System.Runtime.InteropServices;

#pragma warning disable 1591

namespace ClientPrint.Components
{
    [Guid("AB634001-F13D-11d0-A459-004095E1DAEA")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInitDone
    {
        /// <summary>
        /// Инициализация компонента
        /// </summary>
        /// <param name="connection">reference to IDispatch</param>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT Init([In, MarshalAs(UnmanagedType.IDispatch)]object connection);

        /// <summary>
        /// Вызывается перед уничтожением компонента
        /// </summary>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT Done();

        /// <summary>
        /// Возвращается инициализированная информация
        /// </summary>
        /// <param name="info">Component information</param>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        HRESULT GetInfo([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)]ref object[] info);
    }
}