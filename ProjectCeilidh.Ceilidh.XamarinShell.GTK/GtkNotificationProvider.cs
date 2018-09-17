﻿using System;
using System.Runtime.InteropServices;
using ProjectCeilidh.Ceilidh.XamarinShell.GTK.LibNotify;

namespace ProjectCeilidh.Ceilidh.XamarinShell.GTK
{
    public class GtkNotificationProvider : INotificationProvider
    {
        private static readonly NotifyActionCallback<GtkNotificationProvider> NotifyActionCallback = (notification, action, provider) =>
        {
            if (action != "default") return;

            provider.Action?.Invoke(provider, new NotificationActionEventArgs());
        };

        public void DisplayNotification(string identifier, string title, string text)
        {
            var notify = new LibNotify.NotifyNotification(title, text, "dialog-information");
            notify.AddAction("default", null, NotifyActionCallback, this);
            if (!notify.Show(out var error))
            {
                var err = Marshal.PtrToStructure<GError>(error);
                Console.WriteLine(Marshal.PtrToStringAnsi(err.message));
            }
        }

        public void RequestUserAttention() { }

        public event NotificationActionEventHandler Action;
    }
}
