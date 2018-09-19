// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Windows.UI.Notifications;

namespace ProjectCeilidh.Ceilidh.XamarinShell.WPF.Notification
{
    public static class DesktopNotificationManagerCompat
    {
        private static bool _registeredAumidAndComServer;
        private static bool _registeredActivator;

        /// <summary>
        /// Registers the activator type as a COM server client so that Windows can launch your activator.
        /// </summary>
        /// <typeparam name="T">Your implementation of NotificationActivator. Must have GUID and ComVisible attributes on class.</typeparam>
        public static void RegisterActivator<T>()
            where T : NotificationActivator
        {
            // Register type
            var regService = new RegistrationServices();

            regService.RegisterTypeForComClients(
                typeof(T),
                RegistrationClassContext.LocalServer,
                RegistrationConnectionType.MultipleUse);

            _registeredActivator = true;
        }

        /// <summary>
        /// Creates a toast notifier. You must have called <see cref="RegisterActivator{T}"/> first or this will throw an exception.
        /// </summary>
        /// <returns></returns>
        public static ToastNotifier CreateToastNotifier()
        {
            EnsureRegistered();

            return ToastNotificationManager.CreateToastNotifier();
        }

        private static void EnsureRegistered()
        {
            // If not registered AUMID yet
            if (!_registeredAumidAndComServer)
            {
                // Check if Desktop Bridge
                if (DesktopBridgeHelpers.IsRunningAsUwp())
                {
                    // Implicitly registered, all good!
                    _registeredAumidAndComServer = true;
                }

                else
                {
                    // Otherwise, incorrect usage
                    throw new Exception("You must call RegisterAumidAndComServer first.");
                }
            }

            // If not registered activator yet
            if (!_registeredActivator)
            {
                // Incorrect usage
                throw new Exception("You must call RegisterActivator first.");
            }
        }

        /// <summary>
        /// Code from https://github.com/qmatteoq/DesktopBridgeHelpers/edit/master/DesktopBridge.Helpers/Helpers.cs
        /// </summary>
        private static class DesktopBridgeHelpers
        {
            private const long AppmodelErrorNoPackage = 15700L;

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder packageFullName);

            private static bool? _isRunningAsUwp;
            public static bool IsRunningAsUwp()
            {
                if (_isRunningAsUwp == null)
                {
                    if (IsWindows7OrLower)
                    {
                        _isRunningAsUwp = false;
                    }
                    else
                    {
                        var length = 0;
                        GetCurrentPackageFullName(ref length, new StringBuilder(0));

                        var sb = new StringBuilder(length);
                        var result = GetCurrentPackageFullName(ref length, sb);

                        _isRunningAsUwp = result != AppmodelErrorNoPackage;
                    }
                }

                return _isRunningAsUwp.Value;
            }

            private static bool IsWindows7OrLower
            {
                get
                {
                    var versionMajor = Environment.OSVersion.Version.Major;
                    var versionMinor = Environment.OSVersion.Version.Minor;
                    var version = versionMajor + (double)versionMinor / 10;
                    return version <= 6.1;
                }
            }
        }
    }

    /// <summary>
    /// Apps must implement this activator to handle notification activation.
    /// </summary>
    public abstract class NotificationActivator : NotificationActivator.INotificationActivationCallback
    {
        public void Activate(string appUserModelId, string invokedArgs, NotificationUserInputData[] data, uint dataCount)
        {
            OnActivated(invokedArgs, new NotificationUserInput(data), appUserModelId);
        }

        /// <summary>
        /// This method will be called when the user clicks on a foreground or background activation on a toast. Parent app must implement this method.
        /// </summary>
        /// <param name="arguments">The arguments from the original notification. This is either the launch argument if the user clicked the body of your toast, or the arguments from a button on your toast.</param>
        /// <param name="userInput">Text and selection values that the user entered in your toast.</param>
        /// <param name="appUserModelId">Your AUMID.</param>
        protected abstract void OnActivated(string arguments, NotificationUserInput userInput, string appUserModelId);

        // These are the new APIs for Windows 10
        #region NewAPIs
        [StructLayout(LayoutKind.Sequential), Serializable]
        public struct NotificationUserInputData
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public readonly string Key;

            [MarshalAs(UnmanagedType.LPWStr)]
            public readonly string Value;
        }

        [ComImport,
        Guid("53E31837-6600-4A81-9395-75CFFE746F94"), ComVisible(true),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface INotificationActivationCallback
        {
            void Activate(
                [In, MarshalAs(UnmanagedType.LPWStr)]
            string appUserModelId,
                [In, MarshalAs(UnmanagedType.LPWStr)]
            string invokedArgs,
                [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
            NotificationUserInputData[] data,
                [In, MarshalAs(UnmanagedType.U4)]
            uint dataCount);
        }
        #endregion
    }

    /// <summary>
    /// Text and selection values that the user entered on your notification. The Key is the ID of the input, and the Value is what the user entered.
    /// </summary>
    public class NotificationUserInput : IReadOnlyDictionary<string, string>
    {
        private NotificationActivator.NotificationUserInputData[] _data;

        internal NotificationUserInput(NotificationActivator.NotificationUserInputData[] data)
        {
            _data = data;
        }

        public string this[string key] => _data.First(i => i.Key == key).Value;

        public IEnumerable<string> Keys => _data.Select(i => i.Key);

        public IEnumerable<string> Values => _data.Select(i => i.Value);

        public int Count => _data.Length;

        public bool ContainsKey(string key)
        {
            return _data.Any(i => i.Key == key);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _data.Select(i => new KeyValuePair<string, string>(i.Key, i.Value)).GetEnumerator();
        }

        public bool TryGetValue(string key, out string value)
        {
            foreach (var item in _data)
            {
                if (item.Key == key)
                {
                    value = item.Value;
                    return true;
                }
            }

            value = null;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}