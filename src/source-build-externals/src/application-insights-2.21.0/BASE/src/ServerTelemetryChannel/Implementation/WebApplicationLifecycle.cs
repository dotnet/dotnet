#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Hosting;

    /// <summary>
    /// Implements the <see cref="IApplicationLifecycle"/> events for web applications.
    /// </summary>
    internal class WebApplicationLifecycle : IApplicationLifecycle, IRegisteredObject, IDisposable
    {
        private readonly Type hostingEnvironment;

        public WebApplicationLifecycle() : this(typeof(HostingEnvironment))
        {
        }

        protected WebApplicationLifecycle(Type hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.hostingEnvironment.GetMethod("RegisterObject").Invoke(null, new[] { this });
        }

        /// <summary>
        /// The <see cref="Started "/> event is raised when the <see cref="WebApplicationLifecycle"/> instance is first created.
        /// This event is not raised for web applications.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Cannot rename because this implements an interface which is part of the public api.")]
        public event Action<object, object> Started
        {
            add { }
            remove { }
        }

        /// <summary>
        /// The <see cref="Stopping"/> event is raised when <see cref="HostingEnvironment"/> calls the <see cref="Stop"/> method.
        /// </summary>
        public event EventHandler<ApplicationStoppingEventArgs> Stopping;

        /// <summary>
        /// Unregisters the <see cref="WebApplicationLifecycle"/> from <see cref="HostingEnvironment"/>.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets called by <see cref="HostingEnvironment"/> when the web application is stopping.
        /// </summary>
        /// <param name="immediate">
        /// False when the method is invoked first time, allowing async shutdown operations.
        /// True when the method is invoked second time, demanding to unregister immediately.
        /// </param>
        public void Stop(bool immediate)
        {
            if (!immediate)
            {
                this.OnStopping(new ApplicationStoppingEventArgs(this.RunOnCurrentThread));
            }

            this.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            this.hostingEnvironment.GetMethod("UnregisterObject").Invoke(null, new[] { this });
        }

        private void OnStopping(ApplicationStoppingEventArgs eventArgs)
        {
            this.Stopping?.Invoke(this, eventArgs);
        }

        private Task RunOnCurrentThread(Func<Task> asyncMethod)
        {
            return Task.Factory.StartNew(_ => asyncMethod().Wait(), null, CancellationToken.None, TaskCreationOptions.None, CurrentThreadTaskScheduler.Instance);
        }
    }
}
#endif