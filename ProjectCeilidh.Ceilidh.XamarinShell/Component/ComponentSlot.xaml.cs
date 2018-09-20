using System.Windows.Input;
using Xamarin.Forms;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Component
{
    public partial class ComponentSlot
    {
        public static readonly BindableProperty SlotNameProperty = BindableProperty.Create(nameof(SlotName), typeof(string), typeof(ComponentSlot));
        public static readonly BindableProperty ComponentProperty = BindableProperty.Create(nameof(Component), typeof(Element), typeof(ComponentSlot));

        public string SlotName
        {
            get => (string)GetValue(SlotNameProperty);
            set => SetValue(SlotNameProperty, value);
        }

        public Element Component
        {
            get => (Element)GetValue(ComponentProperty);
            set => SetValue(ComponentProperty, value);
        }

        public ComponentSlot()
        {
            InitializeComponent();
        }

        #region Commands

        public ICommand SetComponentCommand => new DelegateCommand(SetComponent);

        private void SetComponent()
        {
            Component = new Label { Text = "I'm a component!" };
        }

        #endregion
    }
}
