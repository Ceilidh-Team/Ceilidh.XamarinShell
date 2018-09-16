using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProjectCeilidh.Ceilidh.Standard;
using ProjectCeilidh.Ceilidh.Standard.Cobble;
using ProjectCeilidh.Cobble;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ProjectCeilidh.Ceilidh.XamarinShell
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var loadContext = new CobbleContext();
            foreach (var loader in typeof(IUnitLoader).Assembly.GetExportedTypes()
                .Where(x => x != typeof(IUnitLoader) && typeof(IUnitLoader).IsAssignableFrom(x)))
                loadContext.AddManaged(loader);
            loadContext.AddUnmanaged(new CeilidhStartOptions());
            AsyncHelpers.RunSync(() => loadContext.ExecuteAsync());

            var mainContext = new CobbleContext();

            if (loadContext.TryGetImplementations(out IEnumerable<IUnitLoader> loaders))
                foreach (var loader in loaders)
                    loader.RegisterUnits(mainContext);

            AsyncHelpers.RunSync(() => mainContext.ExecuteAsync());

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }

    public static class AsyncHelpers
    {
        /// <summary>
        /// Execute's an async Task<T> method which has a void return value synchronously
        /// </summary>
        /// <param name="task">Task<T> method to execute</param>
        public static void RunSync(Func<Task> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            synch.Post(async _ =>
            {
                try
                {
                    await task();
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();

            SynchronizationContext.SetSynchronizationContext(oldContext);
        }

        private class ExclusiveSynchronizationContext : SynchronizationContext
        {
            private bool _done;
            public Exception InnerException { get; set; }
            private readonly AutoResetEvent _workItemsWaiting = new AutoResetEvent(false);

            private readonly Queue<Tuple<SendOrPostCallback, object>> _items =
                new Queue<Tuple<SendOrPostCallback, object>>();

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("We cannot send to our same thread");
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                lock (_items)
                {
                    _items.Enqueue(Tuple.Create(d, state));
                }

                _workItemsWaiting.Set();
            }

            public void EndMessageLoop()
            {
                Post(_ => _done = true, null);
            }

            public void BeginMessageLoop()
            {
                while (!_done)
                {
                    Tuple<SendOrPostCallback, object> task = null;
                    lock (_items)
                    {
                        if (_items.Count > 0)
                        {
                            task = _items.Dequeue();
                        }
                    }

                    if (task != null)
                    {
                        task.Item1(task.Item2);
                        if (InnerException != null) // the method threw an exeption
                        {
                            throw new AggregateException("AsyncHelpers.Run method threw an exception.", InnerException);
                        }
                    }
                    else
                    {
                        _workItemsWaiting.WaitOne();
                    }
                }
            }

            public override SynchronizationContext CreateCopy()
            {
                return this;
            }
        }
    }
}
