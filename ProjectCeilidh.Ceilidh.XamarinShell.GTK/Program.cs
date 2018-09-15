using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

namespace ProjectCeilidh.Ceilidh.XamarinShell.GTK
{
    class MainClass
    {
		[STAThread]
        public static void Main(string[] args)
        {
			Gtk.Application.Init();
			Forms.Init();
   

            var window = new FormsWindow();
			window.LoadApplication(new XamarinShell.App());
			window.SetApplicationTitle("Ceilidh");
			window.Show();

			Gtk.Application.Run();
        }
    }
}
