using System.ComponentModel;
using Xamarin.Forms;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Cobble
{

    public abstract class WindowHandle
    {
        public abstract string Title { get; set; }
        public abstract bool IsVisible { get; set; }
        public abstract (double Width, double Height) Size { get; set; }
        public abstract (double X, double Y) Position { get; set; }
        public abstract WindowHandle Owner { get; set; }

        public abstract void Close();

        public abstract event CancelEventHandler Closing;
    }
}
