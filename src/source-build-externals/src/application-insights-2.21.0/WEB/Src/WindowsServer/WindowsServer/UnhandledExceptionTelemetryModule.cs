#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;

    /// <summary>
    /// The module subscribed to AppDomain.CurrentDomain.UnhandledException to send exceptions to ApplicationInsights.
    /// </summary>
    public sealed class UnhandledExceptionTelemetryModule : ITelemetryModule, IDisposable
    {
        private readonly ITelemetryChannel channel;
        private readonly Action<UnhandledExceptionEventHandler> unregisterAction;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledExceptionTelemetryModule"/> class.
        /// </summary>
        public UnhandledExceptionTelemetryModule() : this(
            action => AppDomain.CurrentDomain.UnhandledException += action,
            action => AppDomain.CurrentDomain.UnhandledException -= action,
            new InMemoryChannel())
        {
        }

        internal UnhandledExceptionTelemetryModule(
            Action<UnhandledExceptionEventHandler> registerAction,
            Action<UnhandledExceptionEventHandler> unregisterAction,
            ITelemetryChannel channel)
        {
            this.unregisterAction = unregisterAction;
            this.channel = channel;

            registerAction(this.CurrentDomainOnUnhandledException);
        }

        /// <summary>
        /// Initializes the telemetry module.
        /// </summary>
        public void Initialize(TelemetryConfiguration configuration)
        {
        }

        /// <summary>
        /// Disposing UnhandledExceptionTelemetryModule instance.
        /// </summary>
        public void Dispose()
        {
            this.unregisterAction(this.CurrentDomainOnUnhandledException);

            if (this.channel != null)
            {
                this.channel.Dispose();
            }
        }

        private static void CopyConfiguration(TelemetryConfiguration source, TelemetryConfiguration target)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            target.InstrumentationKey = source.InstrumentationKey;
#pragma warning restore CS0618 // Type or member is obsolete

            foreach (var telemetryInitializer in source.TelemetryInitializers)
            {
                target.TelemetryInitializers.Add(telemetryInitializer);
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "TelemetryConfiguration is needed for the life of the application.")]
        private TelemetryClient GetTelemetryClient(TelemetryConfiguration sourceConfiguration)
        {
            this.channel.EndpointAddress = sourceConfiguration.TelemetryChannel.EndpointAddress;

            var newConfiguration = new TelemetryConfiguration
            {
                TelemetryChannel = this.channel,
            };

            CopyConfiguration(sourceConfiguration, newConfiguration);

            var telemetryClient = new TelemetryClient(newConfiguration);
            telemetryClient.Context.GetInternalContext().SdkVersion = SdkVersionUtils.GetSdkVersion("unhnd:");

            return telemetryClient;
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            WindowsServerEventSource.Log.CurrentDomainOnUnhandledException();

            var telemetryClient = this.GetTelemetryClient(TelemetryConfiguration.Active);

            var exp = new ExceptionTelemetry(unhandledExceptionEventArgs.ExceptionObject as Exception)
            {
                SeverityLevel = SeverityLevel.Critical,
            };

            telemetryClient.TrackException(exp);
            telemetryClient.Flush(); 
        }
    }
}
#endif