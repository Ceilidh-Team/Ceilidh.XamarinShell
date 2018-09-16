using System.Windows;
using Xamarin.Forms;

namespace ProjectCeilidh.Ceilidh.XamarinShell.WPF
{
    public class WpfWindowProvider : IWindowProvider
    {
        public WindowHandle CreateWindow(Page page, bool isVisible)
        {
            var window = new PageWindow(page);
            window.Show();
            return new WpfWindowHandle(window);
        }

        private class WpfWindowHandle : WindowHandle
        {
            public override string Title
            {
                get => _window.Title;
                set => _window.Title = value;
            }

            public override bool IsVisible
            {
                get => _window.Visibility == Visibility.Visible;
                set => _window.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }

            public override (int Width, int Height) Size
            {
                get => ((int)_window.Width, (int)_window.Height);
                set => (_window.Width, _window.Height) = value;
            }
            public override (int X, int Y) Position
            {
                get => ((int) _window.Left, (int) _window.Top);
                set => (_window.Left, _window.Top) = value;
            }

            private readonly Window _window;

            public WpfWindowHandle(Window window)
            {
                _window = window;
            }

            public override void Close()
            {
                _window.Close();
            }
        }
    }
}
