using System;
using AppKit;
using Foundation;
using ProjectCeilidh.Ceilidh.Standard;
using ProjectCeilidh.Ceilidh.XamarinShell.Ceilidh;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Mac
{
    [Register("AppDelegate")]
    public sealed class AppDelegate : NSApplicationDelegate
    {
        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender) => true;

        public override void DidFinishLaunching(NSNotification notification)
        {
            Forms.Init();
            CeilidhLoader.LoadCeilidh(new CeilidhStartOptions(), x => {
                x.AddManaged<MacUnitLoader>();
            });
        }

        public static void Main(string[] args)
        {
            NSApplication.Init();
            NSApplication.SharedApplication.Delegate = new AppDelegate();
            NSApplication.Main(args);
        }
    }
}
