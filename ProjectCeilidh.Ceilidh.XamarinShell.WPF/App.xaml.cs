﻿using ProjectCeilidh.Ceilidh.Standard;
using System.Windows;
using ProjectCeilidh.Ceilidh.Standard.Cobble;
using ProjectCeilidh.Ceilidh.XamarinShell.WPF.Notification;
using ProjectCeilidh.Cobble;
using Xamarin.Forms;

namespace ProjectCeilidh.Ceilidh.XamarinShell.WPF
{
    public partial class App
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            Forms.Init();

            DesktopNotificationManagerCompat.RegisterActivator<WpfNotificationActivator>();

            await CeilidhLoader.LoadCeilidhAsync(new CeilidhStartOptions(), x =>
            {
                x.AddManaged<WpfUnitLoader>();
            });
        }

        private class WpfUnitLoader  : IUnitLoader
        {
            public void RegisterUnits(CobbleContext context)
            {
                context.AddManaged<WpfWindowProvider>();
                context.AddManaged<WpfNotificationProvider>();
            }
        }
    }
}
