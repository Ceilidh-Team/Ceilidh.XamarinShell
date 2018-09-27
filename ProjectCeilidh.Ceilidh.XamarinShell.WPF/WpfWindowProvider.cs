using System.ComponentModel;
using System.Windows;
using ProjectCeilidh.Ceilidh.XamarinShell.Cobble;
using Xamarin.Forms;
using Application = System.Windows.Application;

namespace ProjectCeilidh.Ceilidh.XamarinShell.WPF
{
    public class WpfWindowProvider : IWindowProvider
    {
        public WindowHandle CreateWindow(Page page, WindowCreationFlags flags)
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                var window = new PageWindow(page)
                {
                    ShowInTaskbar = !flags.HasFlag(WindowCreationFlags.Popup),
                };

                if (!flags.HasFlag(WindowCreationFlags.Popup))
                    Application.Current.MainWindow = window;

                window.Show();
                return new WpfWindowHandle(window);
            });
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

            public override (double Width, double Height) Size
            {
                get => (_window.Width, _window.Height);
                set => (_window.Width, _window.Height) = value;
            }

            public override (double X, double Y) Position
            {
                get => (_window.Left, _window.Top);
                set => (_window.Left, _window.Top) = value;
            }

            public override WindowHandle Owner
            {
                get => _window.Owner == null ? null : (WindowHandle) (_window.Owner.Tag ?? (_window.Owner.Tag = new WpfWindowHandle(_window.Owner)));
                set => _window.Owner = (value as WpfWindowHandle)?._window;
            }

            private readonly Window _window;

            public WpfWindowHandle(Window window)
            {
                _window = window;
                _window.Tag = this;
                _window.Closing += WindowOnClosing;
            }

            private void WindowOnClosing(object sender, CancelEventArgs e) => Closing?.Invoke(this, e);

            public override void Close()
            {
                _window.Closing -= WindowOnClosing;
                _window.Close();
            }

            public override event CancelEventHandler Closing;
        }
    }
}
