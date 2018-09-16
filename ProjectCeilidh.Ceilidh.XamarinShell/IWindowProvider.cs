using Xamarin.Forms;

namespace ProjectCeilidh.Ceilidh.XamarinShell
{
    public interface IWindowProvider
    {
        WindowHandle CreateWindow(Page page, bool isVisible);
    }
}
