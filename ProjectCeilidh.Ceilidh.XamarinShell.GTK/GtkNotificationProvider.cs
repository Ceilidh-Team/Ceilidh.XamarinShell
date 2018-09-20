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

        public bool DisplayNotification(string identifier, string title, string text)
        {
            try
            {
                var notify = new NotifyNotification(title, text, "dialog-information");
                notify.AddAction("default", null, NotifyActionCallback, this);
                return notify.Show(out _);
            }
            catch
            {
                return false;
            }
        }

        public void RequestUserAttention() { }

        public event NotificationActionEventHandler Action;
    }
}
