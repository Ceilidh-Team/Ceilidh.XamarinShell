using ProjectCeilidh.Ceilidh.Standard;
using System;
using ProjectCeilidh.Ceilidh.Standard.Cobble;
using ProjectCeilidh.Cobble;
using Xamarin.Forms;

namespace ProjectCeilidh.Ceilidh.XamarinShell.GTK
{
    class MainClass
    {
		[STAThread]
        public static void Main(string[] args)
        {
			Gtk.Application.Init();
			Forms.Init();

            CeilidhLoader.LoadCeilidh(new CeilidhStartOptions(), x =>
            {
                x.AddManaged<GtkUnitLoader>();
            });

            Gtk.Application.Run();
        }

        public class GtkUnitLoader : IUnitLoader
        {
            public void RegisterUnits(CobbleContext context)
            {
                context.AddManaged<GtkWindowProvider>();
                context.AddManaged<GtkNotificationProvider>();
            }
        }
    }
}
