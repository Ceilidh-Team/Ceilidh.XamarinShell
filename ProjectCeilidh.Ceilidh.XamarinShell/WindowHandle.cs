using System.ComponentModel;

namespace ProjectCeilidh.Ceilidh.XamarinShell
{
    public delegate void ClosingEventHandler(object sender, CancelEventArgs e);

    public abstract class WindowHandle
    {
        public abstract string Title { get; set; }
        public abstract bool IsVisible { get; set; }
        public abstract (double Width, double Height) Size { get; set; }
        public abstract (double X, double Y) Position { get; set; }

        public abstract void Close();

        public abstract event ClosingEventHandler Closing;
    }
}
