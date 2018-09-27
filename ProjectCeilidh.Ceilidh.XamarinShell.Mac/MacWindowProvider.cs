using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using AppKit;
using Foundation;
using ProjectCeilidh.Ceilidh.XamarinShell.Cobble;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Mac
{
    public class MacWindowProvider : IWindowProvider
    {
        public WindowHandle CreateWindow(Page page, WindowCreationFlags flags)
        {
            var window = new NSWindow(new CoreGraphics.CGRect(100, 100, 640, 480), NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled | NSWindowStyle.Miniaturizable, NSBackingStore.Buffered, false)
            {
                ContentViewController = page.CreateViewController(),
                IsVisible = !flags.HasFlag(WindowCreationFlags.Hidden)
            };

            using (var ctrl = new NSWindowController(window))
                ctrl.ShowWindow(null);

            return new MacWindowHandle(window);
        }

        private class MacWindowHandle : WindowHandle
        {
            private static readonly ConcurrentDictionary<NSWindow, MacWindowHandle> _windows = new ConcurrentDictionary<NSWindow, MacWindowHandle>();

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

            public override (double Width, double Height) Size
            {
                get => (_window.ContentLayoutRect.Width, _window.ContentLayoutRect.Height);
                set => _window.SetContentSize(new CoreGraphics.CGSize(value.Width, value.Height));
            }

            public override (double X, double Y) Position
            {
                get => (_window.ContentLayoutRect.Left, _window.ContentLayoutRect.Top);
                set => throw new NotSupportedException();
            }
            public override WindowHandle Owner
            {
                get => GetWindowHandle(_window.ParentWindow);
                set => _window.ParentWindow = (value as MacWindowHandle)._window;
            }

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
                _windows.TryRemove(_window, out _);
                _window.Close();
                _window.Dispose();
            }

            private static MacWindowHandle GetWindowHandle(NSWindow window)
            {
                return _windows.GetOrAdd(window, x => new MacWindowHandle(window));
            }

            public override event CancelEventHandler Closing;
        }
    }
}
