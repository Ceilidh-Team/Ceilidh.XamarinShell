using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ProjectCeilidh.Ceilidh.XamarinShell.WPF.Notification
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(INotificationActivationCallback))]
    [Guid("89bb7948-9b50-47ae-a301-870fc89cbc26")]
    [ComVisible(true)]
    public class WpfNotificationActivator : NotificationActivator
    {
        private static readonly Queue<string> NotificationQueue = new Queue<string>();

        protected override void OnActivated(string arguments, NotificationUserInput userInput, string appUserModelId)
        {
            lock (NotificationQueue)
            {
                if (_activated == null) NotificationQueue.Enqueue(arguments);
                else _activated.Invoke(this, new NotificationActionEventArgs(arguments));
            }
        }

        private static NotificationActionEventHandler _activated;
        public static event NotificationActionEventHandler Activated
        {
            add
            {
                lock (NotificationQueue)
                {
                    _activated += value;
                    if (NotificationQueue.Count <= 0) return;

                    foreach (var action in NotificationQueue)
                        _activated(null, new NotificationActionEventArgs(action));
                }
            }
            remove
            {
                lock(NotificationQueue)
                    _activated -= value;
            }
        }
    }
}
