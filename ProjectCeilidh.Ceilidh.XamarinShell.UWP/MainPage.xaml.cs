using System.IO;

namespace ProjectCeilidh.Ceilidh.XamarinShell.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new XamarinShell.App());
        }
    }
}
