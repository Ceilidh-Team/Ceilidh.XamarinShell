using System;
using Gtk;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK.Extensions;

namespace ProjectCeilidh.Ceilidh.XamarinShell.GTK
{
    public class GtkWindowProvider : IWindowProvider
    {
        public WindowHandle CreateWindow(Page page, bool isVisible)
        {
            var window = new Window(WindowType.Toplevel) { Child = page.CreateContainer(), Visible = isVisible };
            window.SetSizeRequest(0, 0);
            window.Destroyed += WindowOnDestroyed;
            return new GtkWindowHandle(window);
        }

        private void WindowOnDestroyed(object sender, EventArgs e)
        {
            Gtk.Application.Quit();
        }

        private class GtkWindowHandle : WindowHandle
        {
            public override string Title
            {
                get => _window.Title;
                set => _window.Title = value;
            }

            public override bool IsVisible
            {
                get => _window.Visible;
                set => _window.Visible = value;
            }

            public override (int Width, int Height) Size
            {
                get
                {
                    _window.GetSize(out var width, out var height);
                    return (width, height);
                }
                set => _window.SetDefaultSize(value.Width, value.Height);
            }

            public override (int X, int Y) Position
            {
                get
                {
                    _window.GetPosition(out var width, out var height);
                    return (width, height);
                }
                set => _window.Move(value.X, value.Y);
            }

            private readonly Window _window;

            public GtkWindowHandle(Window window)
            {
                _window = window;
            }

            public override void Close()
            {
                _window.Hide();
                _window.Destroy();
                _window.Dispose();
            }
        }
    }
}
