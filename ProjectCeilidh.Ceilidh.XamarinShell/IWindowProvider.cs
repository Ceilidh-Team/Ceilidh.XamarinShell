using System;
using Xamarin.Forms;

namespace ProjectCeilidh.Ceilidh.XamarinShell
{
    public interface IWindowProvider
    {
        WindowHandle CreateWindow(Page page, WindowCreationFlags flags);
    }

    [Flags]
    public enum WindowCreationFlags
    {
        Modal = 1,
        Popup = 2,
        Hidden = 4
    }
}
