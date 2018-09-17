using System;
using ProjectCeilidh.Ceilidh.Standard.Cobble;
using ProjectCeilidh.Cobble;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Mac
{
    public class MacUnitLoader : IUnitLoader
    {
        public void RegisterUnits(CobbleContext context)
        {
            context.AddManaged<MacWindowProvider>();
            context.AddManaged<MacNotificationProvider>();
        }
    }
}
