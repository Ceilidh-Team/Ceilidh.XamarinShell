namespace ProjectCeilidh.Ceilidh.XamarinShell
{
    public abstract class WindowHandle
    {
        public abstract string Title { get; set; }
        public abstract bool IsVisible { get; set; }
        public abstract (int Width, int Height) Size { get; set; }
        public abstract (int X, int Y) Position { get; set; }

        public abstract void Close();
    }
}
