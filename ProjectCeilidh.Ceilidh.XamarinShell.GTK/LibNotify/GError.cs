using System;
using System.Runtime.InteropServices;

namespace ProjectCeilidh.Ceilidh.XamarinShell.GTK.LibNotify
{
    public struct GError
    {
        public uint Quark;
        public int Code;

#pragma warning disable IDE0044, 649
        private IntPtr _message;
#pragma warning restore IDE0044, 649
        public string Message => Marshal.PtrToStringAnsi(_message);
    }
}
