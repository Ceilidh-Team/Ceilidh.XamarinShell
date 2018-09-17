namespace ProjectCeilidh.Ceilidh.XamarinShell
{
    public delegate void NotificationActionEventHandler(string label);

    public abstract class NotificationHandle
    {
        public abstract string Title { get; set; }
        public abstract string Text { get; set; }

        public abstract void Show();
        public abstract void CreateAction(string label);

        public abstract event NotificationActionEventHandler NotificationAction;
    }
}
