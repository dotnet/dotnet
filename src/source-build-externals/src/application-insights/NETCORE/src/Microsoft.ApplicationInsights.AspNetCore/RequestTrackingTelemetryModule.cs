namespace Microsoft.ApplicationInsights.AspNetCore
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Threading;
    using Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners;
    using Microsoft.ApplicationInsights.AspNetCore.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.ApplicationInsights.AspNetCore.Implementation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// Telemetry module tracking requests using Diagnostic Listeners.
    /// </summary>
    public class RequestTrackingTelemetryModule : ITelemetryModule, IObserver<DiagnosticListener>, IDisposable
    {
        internal bool IsInitialized = false;

        // We are only interested in BeforeAction event from Microsoft.AspNetCore.Mvc source.
        // We are interested in Microsoft.AspNetCore.Hosting and Microsoft.AspNetCore.Diagnostics as well.
        // Below filter achieves acceptable performance, character 22 should not be M unless event is BeforeAction.
        private static readonly Predicate<string> HostingPredicate = (string eventName) => (eventName != null) ? !(eventName[21] == 'M') || eventName == "Microsoft.AspNetCore.Mvc.BeforeAction" : false;
        private readonly object lockObject = new object();
        private readonly IApplicationIdProvider applicationIdProvider;

        private TelemetryClient telemetryClient;
        private ConcurrentBag<IDisposable> subscriptions;
        private HostingDiagnosticListener diagnosticListener;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestTrackingTelemetryModule"/> class.
        /// </summary>
        public RequestTrackingTelemetryModule()
            : this(null)
        {
            this.CollectionOptions = new RequestCollectionOptions();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestTrackingTelemetryModule"/> class.
        /// </summary>
        /// <param name="applicationIdProvider">Provider to resolve Application Id.</param>
        public RequestTrackingTelemetryModule(IApplicationIdProvider applicationIdProvider)
        {
            this.applicationIdProvider = applicationIdProvider;
            this.subscriptions = new ConcurrentBag<IDisposable>();
        }

        /// <summary>
        /// Gets or sets request collection options.
        /// </summary>
        public RequestCollectionOptions CollectionOptions { get; set; }

        /// <summary>
        /// Initializes the telemetry module.
        /// </summary>
        /// <param name="configuration">Telemetry configuration to use for initialization.</param>
        public void Initialize(TelemetryConfiguration configuration)
        {
            try
            {
                if (!this.IsInitialized)
                {
                    lock (this.lockObject)
                    {
                        if (!this.IsInitialized)
                        {
                            this.telemetryClient = new TelemetryClient(configuration);

                            // Assume default = 3, as its possible that IWebHostBuilder be removed in future and we hit exception.
                            AspNetCoreMajorVersion aspNetCoreMajorVersion = AspNetCoreMajorVersion.Three;

                            try
                            {
                                var version = typeof(IWebHostBuilder).GetTypeInfo().Assembly.GetName().Version.Major;

                                if (version < 2)
                                {
                                    aspNetCoreMajorVersion = AspNetCoreMajorVersion.One;
                                }
                                else if (version == 2)
                                {
                                    aspNetCoreMajorVersion = AspNetCoreMajorVersion.Two;
                                }
                                else
                                {
                                    aspNetCoreMajorVersion = AspNetCoreMajorVersion.Three;
                                }
                            }
                            catch (Exception e)
                            {
                                AspNetCoreEventSource.Instance.LogError($"Exception occured while attempting to find Asp.Net Core Major version. Assuming {aspNetCoreMajorVersion.ToString()} and continuing. Exception: {e.Message}");
                            }

#pragma warning disable CS0618 // EnableW3CDistributedTracing is obsolete. Ignore because the property inside this constructor is still in use.
                            this.diagnosticListener = new HostingDiagnosticListener(
                                configuration,
                                this.telemetryClient,
                                this.applicationIdProvider,
                                this.CollectionOptions.InjectResponseHeaders,
                                this.CollectionOptions.TrackExceptions,
                                this.CollectionOptions.EnableW3CDistributedTracing,
                                aspNetCoreMajorVersion);
#pragma warning restore CS0618

                            this.subscriptions?.Add(DiagnosticListener.AllListeners.Subscribe(this));

                            this.IsInitialized = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                AspNetCoreEventSource.Instance.RequestTrackingModuleInitializationFailed(e.ToInvariantString());
            }
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Already shipped.")]
        void IObserver<DiagnosticListener>.OnNext(DiagnosticListener value)
        {
            var subs = Volatile.Read(ref this.subscriptions);
            if (subs is null)
            {
                return;
            }

            if (this.diagnosticListener.ListenerName == value.Name)
            {
                subs.Add(value.Subscribe(this.diagnosticListener, HostingPredicate));
                this.diagnosticListener.OnSubscribe();
            }
        }

        /// <inheritdoc />
        void IObserver<DiagnosticListener>.OnError(Exception error)
        {
        }

        /// <inheritdoc />
        void IObserver<DiagnosticListener>.OnCompleted()
        {
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the class.
        /// </summary>
        /// <param name="disposing">Indicates if this class is currently being disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            var subs = Interlocked.Exchange(ref this.subscriptions, null);
            if (subs is null)
            {
                return;
            }

            foreach (var subscription in subs)
            {
                subscription.Dispose();
            }

            if (this.diagnosticListener != null)
            {
                this.diagnosticListener.Dispose();
            }
        }
    }
}