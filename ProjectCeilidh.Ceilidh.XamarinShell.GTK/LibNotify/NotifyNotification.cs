using System;
using System.Runtime.InteropServices;

namespace ProjectCeilidh.Ceilidh.XamarinShell.GTK.LibNotify
{
    public class NotifyNotification
    {
        public static bool IsInitted => NotifyIsInitted();

        public static extern string AppName
        {
            [DllImport("libnotify", EntryPoint = "notify_get_app_name", CharSet = CharSet.Ansi)]
            get;
            [DllImport("libnotify", EntryPoint = "notify_set_app_name", CharSet = CharSet.Ansi)]
            set;
        }

        private readonly IntPtr _notificationPtr;

        public NotifyNotification(string summary, string body, string icon)
        {
            if (!IsInitted && !NotifyInit("hiya"))
                throw new Exception("Notify did not init"); // ayy lmao

            _notificationPtr = NotifyNotificationNew(summary, body, icon);
        }

        public bool Show(out IntPtr error) => NotifyNotificationShow(_notificationPtr, out error);

        #region Native

        [DllImport("libnotify", EntryPoint = "notify_init", CharSet = CharSet.Ansi)]
        private static extern bool NotifyInit(string appName);

        [DllImport("libnotify", EntryPoint = "notify_is_initted")]
        private static extern bool NotifyIsInitted();

        [DllImport("libnotify", EntryPoint = "notify_notification_new", CharSet = CharSet.Ansi)]
        private static extern IntPtr NotifyNotificationNew([MarshalAs(UnmanagedType.LPStr)] string summary, [MarshalAs(UnmanagedType.LPStr)] string body, [MarshalAs(UnmanagedType.LPStr)] string icon);

        [DllImport("libnotify", EntryPoint = "notify_notification_show")]
        private static extern bool NotifyNotificationShow(IntPtr notificationPtr, out IntPtr error);

        #endregion
    }
}
