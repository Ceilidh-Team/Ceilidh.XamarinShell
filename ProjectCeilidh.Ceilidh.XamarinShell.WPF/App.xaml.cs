using System;
using System.Runtime.InteropServices;
using System.Threading;
using ProjectCeilidh.Ceilidh.Standard;
using System.Windows;
using ProjectCeilidh.Ceilidh.Standard.Cobble;
using ProjectCeilidh.Ceilidh.XamarinShell.Ceilidh;
using ProjectCeilidh.Ceilidh.XamarinShell.WPF.Notification;
using ProjectCeilidh.Cobble;
using Xamarin.Forms;

namespace ProjectCeilidh.Ceilidh.XamarinShell.WPF
{
    public partial class App
    {
        [ComVisible(true)]
        [Guid("605EE749-62E8-4F29-AE79-874C4E0598EA")]
        public class Test
        {
            public int b(int y) => y + 1;

            public object c() => this;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            Forms.Init();

            DesktopNotificationManagerCompat.RegisterActivator<WpfNotificationActivator>();

            var thread = new Thread(() =>
            {
                var a = new MSScriptControl.ScriptControl
                {
                    Language = "jScript"
                };
                a.AddObject("test", new Test(), true);
                var res = a.Eval("test.b(1)");
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            await CeilidhLoader.LoadCeilidhAsync(new CeilidhStartOptions(), x =>
            {
                x.AddManaged<WpfUnitLoader>();
            });
        }

        private class WpfUnitLoader  : IUnitLoader
        {
            public void RegisterUnits(CobbleContext context)
            {
                context.AddManaged<WpfWindowProvider>();
                context.AddManaged<WpfNotificationProvider>();
            }
        }
    }
}
