namespace ProjectCeilidh.Ceilidh.XamarinShell
{
    public delegate void NotificationActionCallback();

    public abstract class NotificationHandle
    {
        public abstract string Title { get; set; }
        public abstract string Text { get; set; }

        public abstract void Show();
        public abstract NotificationAction CreateAction();

        public abstract event NotificationActionCallback DefaultAction;
    }

    public abstract class NotificationAction
    {
        public string Label { get; set; }

        public abstract event NotificationActionCallback Action;
    }
}
