using System;
using System.Runtime.InteropServices;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Cobble
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class CobbleExportAttribute : Attribute
    {
        public readonly OSPlatform? Platform;

        public CobbleExportAttribute(string platform = null)
        {
            Platform = platform == null ? default(OSPlatform?) : OSPlatform.Create(platform.ToUpperInvariant());
        }
    }
}
