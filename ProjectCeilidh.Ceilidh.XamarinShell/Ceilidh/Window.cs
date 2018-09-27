using ProjectCeilidh.Ceilidh.XamarinShell.Cobble;
using Xamarin.Forms;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Ceilidh
{
    public abstract class Window : ContentPage
    {
        public static readonly BindableProperty WindowTitleProperty = BindableProperty.Create(nameof(WindowTitle), typeof(string), typeof(Window), propertyChanged: WindowTitlePropertyChanged);

        public string WindowTitle
        {
            get => (string)GetValue(WindowTitleProperty);
            set => SetValue(WindowTitleProperty, value);
        }

        public Size MinSize { get; set; }

        private readonly WindowHandle _handle;

        public Window(WindowHandle handle)
        {
            _handle = handle;
        }

        private static void WindowTitlePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (!(bindable is Window window)) return;

            window._handle.Title = (string)newvalue;
        }
    }
}
