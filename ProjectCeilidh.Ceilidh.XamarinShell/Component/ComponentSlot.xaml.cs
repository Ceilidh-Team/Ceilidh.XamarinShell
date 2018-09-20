﻿using Xamarin.Forms;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Component
{
    public partial class ComponentSlot
    {
        public static readonly BindableProperty SlotNameProperty = BindableProperty.Create(nameof(SlotName), typeof(string), typeof(ComponentSlot));

        public string SlotName
        {
            get => (string)GetValue(SlotNameProperty);
            set => SetValue(SlotNameProperty, value);
        }

        public ComponentSlot()
        {
            InitializeComponent();
        }
    }
}