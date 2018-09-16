using AppKit;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Mac
{
    [Register("AppDelegate")]
    public sealed class AppDelegate : FormsApplicationDelegate
    {
        public override NSWindow MainWindow { get; }

        public AppDelegate()
        {
            MainWindow = new NSWindow(new CoreGraphics.CGRect(200, 1000, 1024, 768), NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled | NSWindowStyle.Miniaturizable, NSBackingStore.Buffered, false)
            {
                Title = "Ceilidh",
                TitleVisibility = NSWindowTitleVisibility.Visible,

            };
        }

        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender) => true;

        public override void DidFinishLaunching(NSNotification notification)
        {
            Forms.Init();
            // LoadApplication(new App());
            base.DidFinishLaunching(notification);
        }

        public static void Main(string[] args)
        {
            NSApplication.Init();
            NSApplication.SharedApplication.Delegate = new AppDelegate();
            NSApplication.Main(args);
        }
    }
}
