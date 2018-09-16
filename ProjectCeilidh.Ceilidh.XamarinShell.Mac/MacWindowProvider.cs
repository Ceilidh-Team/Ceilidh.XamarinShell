using System;
using System.ComponentModel;
using AppKit;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Mac
{
    public class MacWindowProvider : IWindowProvider
    {
        public WindowHandle CreateWindow(Page page, bool isVisible)
        {
            var window = new NSWindow
            {
                ContentViewController = page.CreateViewController(),
                IsVisible = isVisible
            };

            return new MacWindowHandle(window);
        }

        private class MacWindowHandle : WindowHandle
        {
            public override string Title
            {
                get => _window.Title;
                set => _window.Title = value;
            }

            public override bool IsVisible
            {
                get => _window.IsVisible;
                set => _window.IsVisible = value;
            }

            public override (double Width, double Height) Size { get; set; }
            public override (double X, double Y) Position { get; set; }

            private readonly NSWindow _window;

            public MacWindowHandle(NSWindow window)
            {
                _window = window;
                _window.WindowShouldClose = WindowShouldClose;
            }

            private bool WindowShouldClose(NSObject sender)
            {
                var e = new CancelEventArgs();
                Closing?.Invoke(this, e);
                return !e.Cancel;
            }

            public override void Close()
            {
                throw new NotImplementedException();
            }

            public override event ClosingEventHandler Closing;
        }
    }
}
