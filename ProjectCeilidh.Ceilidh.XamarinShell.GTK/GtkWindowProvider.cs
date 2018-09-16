using System;
using System.ComponentModel;
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

            public override (double Width, double Height) Size
            {
                get
                {
                    _window.GetSize(out var width, out var height);
                    return (width, height);
                }
                set => _window.SetDefaultSize((int)value.Width, (int)value.Height);
            }

            public override (double X, double Y) Position
            {
                get
                {
                    _window.GetPosition(out var width, out var height);
                    return (width, height);
                }
                set => _window.Move((int)value.X, (int)value.Y);
            }

            private readonly Window _window;

            public GtkWindowHandle(Window window)
            {
                _window = window;

                _window.DeleteEvent += WindowOnDeleteEvent;
            }

            private void WindowOnDeleteEvent(object o, DeleteEventArgs args)
            {
                var e = new CancelEventArgs();
                Closing?.Invoke(this, e);
                args.RetVal = e.Cancel;
            }

            public override void Close()
            {
                _window.DeleteEvent -= WindowOnDeleteEvent;
                _window.Hide();
                _window.Destroy();
                _window.Dispose();
            }

            public override event ClosingEventHandler Closing;
        }
    }
}
