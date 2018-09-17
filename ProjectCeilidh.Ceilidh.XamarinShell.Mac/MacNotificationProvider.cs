using System;
using Foundation;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Mac
{
    public class MacNotificationProvider : INotificationProvider
    {
        public MacNotificationProvider()
        {
        }

        public NotificationHandle CreateNotification()
        {
            return new MacNotificationHandle();
        }

        private class MacNotificationHandle : NotificationHandle
        {
            public override string Title { get => _notification.Title; set => _notification.Title = value; }
            public override string Text { get => _notification.InformativeText; set => _notification.InformativeText = value; }

            private readonly NSUserNotification _notification;

            public MacNotificationHandle()
            {
                _notification = new NSUserNotification
                {
                    SoundName = NSUserNotification.NSUserNotificationDefaultSoundName,
                    Identifier = Guid.NewGuid().ToString()
                };

                NSUserNotificationCenter.DefaultUserNotificationCenter.DidActivateNotification += DefaultUserNotificationCenter_DidActivateNotification;
                NSUserNotificationCenter.DefaultUserNotificationCenter.ShouldPresentNotification = (a, b) => true;
            }

            private void DefaultUserNotificationCenter_DidActivateNotification(object sender, UNCDidActivateNotificationEventArgs e)
            {
                if (_notification.Identifier != e.Notification.Identifier) return;

                switch (e.Notification.ActivationType)
                {
                    case NSUserNotificationActivationType.ContentsClicked:
                        NotificationAction?.Invoke(null);
                        break;
                    case NSUserNotificationActivationType.AdditionalActionClicked:
                        NotificationAction?.Invoke(e.Notification.Title);
                        break;
                }
            }


            public override void CreateAction(string label)
            {
                _notification.HasActionButton = true;
                _notification.AdditionalActions = new [] {
                    NSUserNotificationAction.GetAction("more", label)
                };
            }

            public override void Show()
            {
                NSUserNotificationCenter.DefaultUserNotificationCenter.DeliverNotification(_notification);
            }

            public override event NotificationActionEventHandler NotificationAction;
        }
    }
}
