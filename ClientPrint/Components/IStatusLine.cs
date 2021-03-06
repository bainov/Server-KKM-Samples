﻿using System;
using System.Runtime.InteropServices;

#pragma warning disable 1591

namespace ClientPrint.Components
{
    [Guid("AB634005-F13D-11D0-A459-004095E1DAEA")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IStatusLine
    {
        /// <summary>
        /// Задает текст статусной строки
        /// </summary>
        /// <param name="bstrStatusLine">Текст статусной строки</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT SetStatusLine(BSTR bstrStatusLine);
        /// </prototype>
        /// </remarks>
        void SetStatusLine([MarshalAs(UnmanagedType.BStr)]String bstrStatusLine);

        /// <summary>
        /// Сброс статусной строки
        /// </summary>
        /// <remarks>
        /// <propotype>
        /// HRESULT ResetStatusLine();
        /// </propotype>
        /// </remarks>
        void ResetStatusLine();
    }
}