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
                .Where(x => x.GetCustomAttribute<CobbleExportAttribute>() != null &&
                            (x.GetCustomAttribute<CobbleExportAttribute>().Platform == null ||
                             RuntimeInformation.IsOSPlatform(
                                 x.GetCustomAttribute<CobbleExportAttribute>().Platform ?? default))))
                context.AddManaged(unit);
        }
    }
}
