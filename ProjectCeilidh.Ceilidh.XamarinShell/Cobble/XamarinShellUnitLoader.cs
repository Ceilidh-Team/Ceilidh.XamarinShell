using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using ProjectCeilidh.Ceilidh.Standard.Cobble;
using ProjectCeilidh.Cobble;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Cobble
{
    public class XamarinShellUnitLoader : IUnitLoader
    {
        public void RegisterUnits(CobbleContext context)
        {
            foreach (var unit in typeof(XamarinShellUnitLoader).Assembly.GetExportedTypes()
                .Where(x => {
                    var ann = x.GetCustomAttribute<CobbleExportAttribute>();
                    if (ann == null) return false;
                    return ann.Platform == null || RuntimeInformation.IsOSPlatform(ann.Platform.Value);
                }))
                context.AddManaged(unit);
        }
    }
}
