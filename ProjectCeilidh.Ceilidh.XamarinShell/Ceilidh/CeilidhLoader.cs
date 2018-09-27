using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectCeilidh.Ceilidh.Standard;
using ProjectCeilidh.Ceilidh.Standard.Cobble;
using ProjectCeilidh.Ceilidh.XamarinShell.Cobble;
using ProjectCeilidh.Cobble;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Ceilidh
{
    public static class CeilidhLoader
    {
        public static CobbleContext Instance { get; private set; }

        public static CobbleContext LoadCeilidh(CeilidhStartOptions startOptions, Action<CobbleContext> loaderCallback)
        {
            if (Instance != null) throw new NotSupportedException();

            var loaderContext = new CobbleContext();
            foreach (var loader in typeof(IUnitLoader).Assembly.GetExportedTypes()
                .Where(x => x != typeof(IUnitLoader) && typeof(IUnitLoader).IsAssignableFrom(x)))
                loaderContext.AddManaged(loader);

            foreach (var loader in typeof(CeilidhLoader).Assembly.GetExportedTypes()
                .Where(x => typeof(IUnitLoader).IsAssignableFrom(x)))
                loaderContext.AddManaged(loader);

            loaderCallback?.Invoke(loaderContext);
            loaderContext.AddUnmanaged(startOptions);
            loaderContext.Execute();

            var mainContext = new CobbleContext();
            if (!loaderContext.TryGetImplementations(out IEnumerable<IUnitLoader> loaders)) return mainContext;

            foreach (var loader in loaders)
                loader.RegisterUnits(mainContext);

            mainContext.Execute();

            Instance = mainContext;

            if (mainContext.TryGetSingleton(out IWindowProvider provider))
            {
                var window = provider.CreateWindow(new MainPage(), 0);
                window.Size = (640, 480);
                window.Title = "Ceilidh";
                window.IsVisible = true;
                window.Closing += (sender, args) => Environment.Exit(0);
            }

            return mainContext;
        }

        public static async Task<CobbleContext> LoadCeilidhAsync(CeilidhStartOptions startOptions, Action<CobbleContext> loaderCallback)
        {
            if (Instance != null) throw new NotSupportedException();

            var loaderContext = new CobbleContext();
            foreach (var loader in typeof(IUnitLoader).Assembly.GetExportedTypes()
                .Where(x => x != typeof(IUnitLoader) && typeof(IUnitLoader).IsAssignableFrom(x)))
                loaderContext.AddManaged(loader);

            foreach (var loader in typeof(CeilidhLoader).Assembly.GetExportedTypes()
                .Where(x => typeof(IUnitLoader).IsAssignableFrom(x)))
                loaderContext.AddManaged(loader);

            loaderCallback?.Invoke(loaderContext);
            loaderContext.AddUnmanaged(startOptions);
            await loaderContext.ExecuteAsync();

            var mainContext = new CobbleContext();
            if (!loaderContext.TryGetImplementations(out IEnumerable<IUnitLoader> loaders)) return mainContext;

            foreach (var loader in loaders)
                loader.RegisterUnits(mainContext);

            await mainContext.ExecuteAsync();

            Instance = mainContext;

            if (mainContext.TryGetSingleton(out IWindowProvider provider))
            {
                var window = provider.CreateWindow(new MainPage(), 0);
                window.Size = (640, 480);
                window.Title = "Ceilidh";
                window.IsVisible = true;
                window.Closing += (sender, args) => Environment.Exit(0);
            }


            return mainContext;
        }
    }
}
