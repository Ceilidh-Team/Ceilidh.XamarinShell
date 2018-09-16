using Xamarin.Forms;

namespace ProjectCeilidh.Ceilidh.XamarinShell.WPF
{
    /// <summary>
    /// Interaction logic for PageWindow.xaml
    /// </summary>
    public partial class PageWindow
    {
        public PageWindow(Page page)
        {
            InitializeComponent();

            DataContext = page;
        }
    }
}
