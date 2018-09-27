using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Component
{
    public class ContextMenu : BindableObject
    {
        public static readonly BindableProperty ContextMenuProperty =
                BindableProperty.CreateAttached("ContextMenu", typeof(ContextMenu), typeof(ContextMenu), null, propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(HandleBindingPropertyChangedDelegate));

        public static readonly BindableProperty MenuItemsProperty =
                BindableProperty.Create(nameof(MenuItems), typeof(MenuItemCollection), typeof(ContextMenu));

        public MenuItemCollection MenuItems
        {
            get => (MenuItemCollection)GetValue(MenuItemsProperty);
            set => SetValue(MenuItemsProperty, value);
        }

        public ContextMenu()
        {

        }

        private static void HandleBindingPropertyChangedDelegate(BindableObject bindable, object oldValue, object newValue)
        {
            if ((bindable is View a))
                a.GestureRecognizers.Add(new TapGestureRecognizer { });

            var value = (ContextMenu)newValue;
        }
    }

    public class RightClickRecognizer : IGestureRecognizer
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class MenuItemCollection : ICollection<MenuItem>
    {
        public int Count => _collection.Count;

        public bool IsReadOnly => _collection.IsReadOnly;

        private readonly ICollection<MenuItem> _collection;

        public MenuItemCollection()
        {
            _collection = new List<MenuItem>();
        }

        public void Add(MenuItem item) => _collection.Add(item);

        public void Clear() => _collection.Clear();

        public bool Contains(MenuItem item) => _collection.Contains(item);

        public void CopyTo(MenuItem[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

        public IEnumerator<MenuItem> GetEnumerator() => _collection.GetEnumerator();

        public bool Remove(MenuItem item) => _collection.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();
    }

    public class MenuItem
    {

    }
}
