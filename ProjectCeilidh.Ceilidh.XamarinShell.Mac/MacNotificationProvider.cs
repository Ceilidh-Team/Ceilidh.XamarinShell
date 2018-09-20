using AppKit;
using Foundation;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Mac
{
    public class MacNotificationProvider : INotificationProvider
    {
        public MacNotificationProvider()
        {
            NSUserNotificationCenter.DefaultUserNotificationCenter.DidActivateNotification +=
                (sender, e) => Action?.Invoke(this, new NotificationActionEventArgs(e.Notification.Identifier));
        }

        public bool DisplayNotification(string identifier, string title, string text)
        {
            using (var notification = new NSUserNotification
            {
                Identifier = identifier,
                Title = title,
                InformativeText = text,
                SoundName = NSUserNotification.NSUserNotificationDefaultSoundName
            })
                NSUserNotificationCenter.DefaultUserNotificationCenter.DeliverNotification(notification);

            return true;
        }

        public void RequestUserAttention()
        {
            NSApplication.SharedApplication.RequestUserAttention(NSRequestUserAttentionType.CriticalRequest);
        }

        public event NotificationActionEventHandler Action;
    }
}
