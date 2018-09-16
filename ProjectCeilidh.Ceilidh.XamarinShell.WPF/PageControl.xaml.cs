using System.ComponentModel;
using System.Windows;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF.Extensions;
using Page = Xamarin.Forms.Page;

namespace ProjectCeilidh.Ceilidh.XamarinShell.WPF
{
    public partial class PageControl : INotifyPropertyChanged
    {
        public static readonly DependencyProperty PageProperty = DependencyProperty.Register(nameof(Page), typeof(Page), typeof(PageControl), new PropertyMetadata(OnPageChanged));

        public Page Page
        {
            get => (Page) GetValue(PageProperty);
            set => SetValue(PageProperty, value);
        }

        private FrameworkElement _content;
        public FrameworkElement Content
        {
            get => _content;
            private set
            {
                if (value == _content) return;

                _content = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Content)));
            }
        }

        public PageControl()
        {
            InitializeComponent();

            if (!Forms.IsInitialized) Forms.Init();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            if (Page == null) return;

            Page.Layout(new Rectangle(0, 0, ActualWidth, ActualHeight));

            base.OnRenderSizeChanged(sizeInfo);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private static void OnPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is PageControl obj)) return;
            var page = (Page) e.NewValue;
            
            obj.Content = page.ToFrameworkElement();
        }
    }
}
