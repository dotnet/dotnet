namespace Microsoft.Extensions.DependencyInjection
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The <see cref="IOptions{TelemetryConfiguration}"/> implementation that create new <see cref="TelemetryConfiguration"/> every time when called".
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "This class is instantiated by Dependency Injection.")]
    internal class TelemetryConfigurationOptions : IOptions<TelemetryConfiguration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryConfigurationOptions"/> class.
        /// </summary>
        /// <param name="configureOptions">Collection of options to be configured.</param>
        public TelemetryConfigurationOptions(IEnumerable<IConfigureOptions<TelemetryConfiguration>> configureOptions)
        {
            this.Value = TelemetryConfiguration.CreateDefault();

            var configureOptionsArray = configureOptions.ToArray();
            foreach (var c in configureOptionsArray)
            {
                c.Configure(this.Value);
            }
        }

        /// <inheritdoc />
        public TelemetryConfiguration Value { get; }
    }
}