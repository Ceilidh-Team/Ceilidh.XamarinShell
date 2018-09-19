namespace ProjectCeilidh.Ceilidh.XamarinShell
{
    public delegate void NotificationActionEventHandler(object sender, NotificationActionEventArgs e);

    public struct NotificationActionEventArgs
    {
        public readonly string Identifier;

        public NotificationActionEventArgs(string identifier)
        {
            Identifier = identifier;
        }
    }

    public interface INotificationProvider
    {
        void RequestUserAttention();

        bool DisplayNotification(string identifier, string title, string text);

        event NotificationActionEventHandler Action;
    }
}
