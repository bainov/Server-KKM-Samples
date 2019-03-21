using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

#pragma warning disable 1591

namespace ClientPrint
{
    /// <summary>
    /// Метод чтобы получить имя комьютера
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class NativeMethods
    {
        public static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;
        public const int WTS_CURRENT_SESSION = -1;

        public enum WTS_INFO_CLASS
        {
            WTSClientName = 10
        }

        [DllImport("Wtsapi32.dll", CharSet = CharSet.Unicode)]
        public static extern bool WTSQuerySessionInformation(
            IntPtr hServer,
            Int32 sessionId,
            WTS_INFO_CLASS wtsInfoClass,
            out IntPtr ppBuffer,
            out Int32 pBytesReturned);

        /// <summary>
        /// The WTSFreeMemory function frees memory allocated by a Terminal
        /// Services function.
        /// </summary>
        /// <param name="memory">Pointer to the memory to free.</param>
        [DllImport("wtsapi32.dll", ExactSpelling = true, SetLastError = false)]
        public static extern void WTSFreeMemory(IntPtr memory);

        internal static string GetTerminalServicesClientName()
        {
            IntPtr buffer;

            int bytesReturned;

            bool success = WTSQuerySessionInformation(
                WTS_CURRENT_SERVER_HANDLE,
                WTS_CURRENT_SESSION,
                WTS_INFO_CLASS.WTSClientName,
                out buffer,
                out bytesReturned);

            if (!success) return null;

            var clientName = Marshal.PtrToStringUni(
                buffer,
                bytesReturned / 2 /* Because the DllImport uses CharSet.Unicode */
            );
            WTSFreeMemory(buffer);

            return clientName;
        }
    }
}