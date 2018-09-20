using System;
using System.Runtime.InteropServices;

namespace ProjectCeilidh.Ceilidh.XamarinShell.GTK.LibNotify
{
    public delegate void NotifyActionCallback<T>(NotifyNotification notifyNotification, string action, T userData);

    public class NotifyNotification
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GFreeFunc(IntPtr data);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate void NotifyActionCallback(IntPtr notificationPtr, string action, IntPtr opaque);

        private static readonly GFreeFunc GcHandleFreeFuncDelegate = ptr =>
        {
            if (ptr == IntPtr.Zero) return;

            GCHandle.FromIntPtr(ptr).Free();
        };

        private static readonly NotifyActionCallback NotifyActionCallbackDelegate = (notificationPtr, action, opaque) =>
        {
            var handle = (UserData) GCHandle.FromIntPtr(opaque).Target;
            handle.Callback.DynamicInvoke(new NotifyNotification(notificationPtr), action, handle.Data);
        };

        public static bool IsInitted => NotifyIsInitted();

        public static extern string AppName
        {
            [DllImport("libnotify", EntryPoint = "notify_get_app_name", CharSet = CharSet.Ansi)]
            get;
            [DllImport("libnotify", EntryPoint = "notify_set_app_name", CharSet = CharSet.Ansi)]
            set;
        }

        private readonly IntPtr _notificationPtr;

        public NotifyNotification(string summary, string body, string icon) : this(NotifyNotificationNew(summary, body, icon))
        {
        }

        private NotifyNotification(IntPtr notificationPtr)
        {
            if (!IsInitted && !NotifyInit("hiya"))
                throw new Exception("Notify did not init"); // ayy lmao

            _notificationPtr = notificationPtr;
        }

        public bool Show(out IntPtr error) => NotifyNotificationShow(_notificationPtr, out error);

        public void AddAction<T>(string action, string label, NotifyActionCallback<T> callback, T userData)
        {
            NotifyNotificationAddAction(_notificationPtr, action, label, NotifyActionCallbackDelegate, GCHandle.ToIntPtr(GCHandle.Alloc(new UserData(callback, userData))), GcHandleFreeFuncDelegate);
        }

        private class UserData
        {
            public readonly Delegate Callback;
            public readonly object Data;

            public UserData(Delegate callback, object userData)
            {
                Callback = callback;
                Data = userData;
            }
        }

        #region Native

        [DllImport("libnotify", EntryPoint = "notify_init", CharSet = CharSet.Ansi)]
        private static extern bool NotifyInit(string appName);

        [DllImport("libnotify", EntryPoint = "notify_is_initted")]
        private static extern bool NotifyIsInitted();

        [DllImport("libnotify", EntryPoint = "notify_notification_new", CharSet = CharSet.Ansi)]
        private static extern IntPtr NotifyNotificationNew([MarshalAs(UnmanagedType.LPStr)] string summary, [MarshalAs(UnmanagedType.LPStr)] string body, [MarshalAs(UnmanagedType.LPStr)] string icon);

        [DllImport("libnotify", EntryPoint = "notify_notification_show")]
        private static extern bool NotifyNotificationShow(IntPtr notificationPtr, out IntPtr error);

        [DllImport("libnotify", EntryPoint = "notify_notification_add_action", CharSet = CharSet.Ansi)]
        private static extern void NotifyNotificationAddAction(IntPtr notificationPtr, string action, string label, NotifyActionCallback callback, IntPtr opaque, GFreeFunc freeFunc);

        #endregion
    }
}
