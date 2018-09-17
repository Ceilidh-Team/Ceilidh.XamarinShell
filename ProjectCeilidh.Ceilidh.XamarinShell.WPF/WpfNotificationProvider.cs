using System;
using System.Linq;
using Windows.ApplicationModel;

namespace ProjectCeilidh.Ceilidh.XamarinShell.WPF
{
    public class WpfNotificationProvider : INotificationProvider
    {
        public NotificationHandle CreateNotification()
        {
            return new WpfNotificationHandle();
        }

        private class WpfNotificationHandle : NotificationHandle
        {
            public override string Title { get; set; }
            public override string Text { get; set; }

            public WpfNotificationHandle()
            {

            }

            public override void CreateAction(string label)
            {
            }

            public override void Show()
            {
                var id = Package.Current.GetAppListEntriesAsync().AsTask().Result.First().AppUserModelId;
                var a = "ProjectCeilidh.Ceilidh.XamarinShell.WPF!App";
            }

            public override event NotificationActionEventHandler NotificationAction;
        }
    }
}
