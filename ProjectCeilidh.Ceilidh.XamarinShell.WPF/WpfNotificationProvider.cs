using System;

namespace ProjectCeilidh.Ceilidh.XamarinShell.WPF
{
    public class WpfNotificationProvider : INotificationProvider
    {
        public void DisplayNotification(string identifier, string title, string text)
        {
            throw new NotImplementedException();
        }

        public void RequestUserAttention()
        {
            throw new NotImplementedException();
        }

        public event NotificationActionEventHandler Action;
    }
}
