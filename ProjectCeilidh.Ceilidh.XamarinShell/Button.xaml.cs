using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ProjectCeilidh.Ceilidh.XamarinShell
{
    public partial class Button
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(Command), typeof(Button));

        public Command Command
        {
            get => (Command) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public Button()
        {
            InitializeComponent();
        }
    }
}
