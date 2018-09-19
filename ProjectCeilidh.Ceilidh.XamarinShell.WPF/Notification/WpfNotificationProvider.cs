using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Xml;
using System.Xml.Serialization;
using Windows.ApplicationModel;
using Windows.UI.Notifications;
using XmlDocument = Windows.Data.Xml.Dom.XmlDocument;

namespace ProjectCeilidh.Ceilidh.XamarinShell.WPF.Notification
{
    public class WpfNotificationProvider : INotificationProvider
    {
        private readonly ToastNotifier _toastNotifier;

        public WpfNotificationProvider()
        {
            _toastNotifier = DesktopNotificationManagerCompat.CreateToastNotifier();
        }

        public bool DisplayNotification(string identifier, string title, string text)
        {
            if (_toastNotifier == null) return false;

            var content = new ToastContent
            {
                Launch = identifier,
                Visual = new ToastVisual
                {
                    BindingGeneric = new ToastBindingGeneric
                    {
                        Text = new[]
                        {
                            title,
                            text
                        }
                    }
                }
            };

            var doc = new XmlDocument();
            using (var str = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(str))
            {
                new XmlSerializer(typeof(ToastContent)).Serialize(xmlWriter, content);

                doc.LoadXml(str.ToString());
            }

            var notification = new ToastNotification(doc);
            _toastNotifier.Show(notification);
            return true;
        }

        public void RequestUserAttention()
        {
            if (Application.Current.MainWindow == null) return;

            var interop = new WindowInteropHelper(Application.Current.MainWindow);
            var info = new FlashWInfo
            {
                hwnd = interop.Handle,
                dwFlags = FlashFlags.Tray | FlashFlags.TimerNoForeground,
                uCount = uint.MaxValue,
                dwTimeout = 0,
                cbSize = (uint) Marshal.SizeOf<FlashWInfo>()
            };
            FlashWindowEx(ref info);
        }

        private NotificationActionEventHandler _action;
        public event NotificationActionEventHandler Action
        {
            add
            {
                // TODO
            }
            remove
            {

            }
        }

        #region Native

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref FlashWInfo pwfi);

        [StructLayout(LayoutKind.Sequential)]
        private struct FlashWInfo
        {
            public uint cbSize; //The size of the structure in bytes.            
            public IntPtr hwnd; //A Handle to the Window to be Flashed. The window can be either opened or minimized.


            public FlashFlags dwFlags; //The Flash Status.            
            public uint uCount; // number of times to flash the window            
            public uint dwTimeout; //The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.        
        }

        [Flags]
        private enum FlashFlags : uint
        {
            Stop = 0,
            Caption = 1,
            Tray = 2,
            All = Caption | Tray,
            Timer = 4,
            TimerNoForeground = 12
        }

        #endregion
    }

    [XmlRoot("toast")]
    public class ToastContent
    {
        public const string
            NOTIFICATION_DEFAULT = "ms-winsoundevent:Notification.Default",
            NOTIFICATION_IM = "ms-winsoundevent:Notification.IM",
            NOTIFICATION_MAIL = "ms-winsoundevent:Notification.Mail",
            NOTIFICATION_REMINDER = "ms-winsoundevent:Notification.Reminder",
            NOTIFICATION_SMS = "ms-winsoundevent:Notification.SMS";

        [XmlAttribute("launch")]
        public string Launch { get; set; }

        [XmlAttribute("duration")]
        public ToastDuration Duration { get; set; }

        [XmlAttribute("activationType")]
        public ToastActivationType ActivationType { get; set; }

        [XmlAttribute("scenario")]
        public ToastScenario Scenario { get; set; }

        [XmlElement("visual")]
        public ToastVisual Visual { get; set; }

        [Serializable]
        public enum ToastDuration
        {
            [XmlEnum("short")]
            Short,
            [XmlEnum("long")]
            Long
        }

        [Serializable]
        public enum ToastActivationType
        {
            [XmlEnum("foreground")]
            Foreground,
            [XmlEnum("background")]
            Background,
            [XmlEnum("protocol")]
            Protocol,
            [XmlEnum("system")]
            System
        }

        [Serializable]
        public enum ToastScenario
        {
            [XmlEnum("default")]
            Default,
            [XmlEnum("alarm")]
            Alarm,
            [XmlEnum("reminder")]
            Reminder,
            [XmlEnum("incomingCall")]
            IncomingCall
        }
    }

    public class ToastVisual
    {
        [XmlElement("binding")]
        public ToastBindingGeneric BindingGeneric { get; set; }
    }

    public class ToastBindingGeneric
    {
        [XmlAttribute("template")]
        public ToastBindingTemplate Template { get; set; }

        [XmlElement("text")]
        public string[] Text { get; set; }

        [Serializable]
        public enum ToastBindingTemplate
        {
            [XmlEnum("ToastGeneric")]
            Generic = 0
        }
    }
}
