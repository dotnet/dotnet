namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Telemetry type used to track page views.
    /// </summary>
    /// <remarks>
    /// You can send information about pages viewed by your application to Application Insights by
    /// passing an instance of the <see cref="PageViewTelemetry"/> class to the <see cref="TelemetryClient.TrackPageView(PageViewTelemetry)"/>
    /// method.
    /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#page-views">Learn more</a>
    /// </remarks>
    public sealed class PageViewTelemetry : ITelemetry, ISupportProperties, ISupportAdvancedSampling, ISupportMetrics, IAiSerializableTelemetry
    {
        internal const string EtwEnvelopeName = "PageView";
        internal readonly PageViewData Data;
        internal string EnvelopeName = "AppPageViews";
        private readonly TelemetryContext context;
        private IExtension extension;

        private double? samplingPercentage;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewTelemetry"/> class.
        /// </summary>
        public PageViewTelemetry()
        {
            this.Data = new PageViewData();
            this.context = new TelemetryContext(this.Data.properties);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewTelemetry"/> class with the
        /// specified <paramref name="pageName"/>.
        /// </summary>
        /// <exception cref="ArgumentException">The <paramref name="pageName"/> is null or empty string.</exception>
        public PageViewTelemetry(string pageName) : this()
        {
            this.Name = pageName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewTelemetry"/> class by cloning an existing instance.
        /// </summary>
        /// <param name="source">Source instance of <see cref="PageViewTelemetry"/> to clone from.</param>
        private PageViewTelemetry(PageViewTelemetry source)
        {
            this.Data = source.Data.DeepClone();
            this.context = source.context.DeepClone(this.Data.properties);
            this.extension = source.extension?.DeepClone();
            this.Timestamp = source.Timestamp;
            this.samplingPercentage = source.samplingPercentage;
            this.ProactiveSamplingDecision = source.ProactiveSamplingDecision;
        }

        /// <inheritdoc />
        string IAiSerializableTelemetry.TelemetryName
        {
            get
            {
                return this.EnvelopeName;
            }

            set
            {
                this.EnvelopeName = value;
            }
        }

        /// <inheritdoc />
        string IAiSerializableTelemetry.BaseType => nameof(PageViewData);

        /// <summary>
        /// Gets or sets date and time when event was recorded.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the value that defines absolute order of the telemetry item.
        /// </summary>
        public string Sequence { get; set; }

        /// <summary>
        /// Gets the context associated with the current telemetry item.
        /// </summary>
        public TelemetryContext Context
        {
            get { return this.context; }
        }

        /// <summary>
        /// Gets or sets gets the extension used to extend this telemetry instance using new strong typed object.
        /// </summary>
        public IExtension Extension
        {
            get { return this.extension; }
            set { this.extension = value; }
        }

        /// <summary>
        /// Gets or sets page view ID.
        /// </summary>
        public string Id
        {
            get { return this.Data.id; }
            set { this.Data.id = value; }
        }

        /// <summary>
        /// Gets or sets the name of the page.
        /// </summary>
        public string Name
        {
            get { return this.Data.name; }
            set { this.Data.name = value; }
        }

        /// <summary>
        /// Gets or sets the page view Uri.
        /// </summary>
        public Uri Url
        {
            get
            {
                if (this.Data.url.IsNullOrWhiteSpace())
                {
                    return null;
                }

                return new Uri(this.Data.url, UriKind.RelativeOrAbsolute);
            }

            set
            {
                if (value == null)
                {
                    this.Data.url = null;
                }
                else
                {
                    this.Data.url = value.ToString();
                }
            }
        }

        /// <summary>
        /// Gets or sets the page view duration.
        /// </summary>
        public TimeSpan Duration
        {
            get { return Utils.ValidateDuration(this.Data.duration); }
            set { this.Data.duration = value.ToString(); }
        }

        /// <summary>
        /// Gets a dictionary of custom defined metrics.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public IDictionary<string, double> Metrics
        {
            get { return this.Data.measurements; }
        }

        /// <summary>
        /// Gets a dictionary of application-defined property names and values providing additional information about this page view.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public IDictionary<string, string> Properties
        {
            get { return this.Data.properties; }
        }

        /// <summary>
        /// Gets or sets data sampling percentage (between 0 and 100).
        /// Should be 100/n where n is an integer. <a href="https://go.microsoft.com/fwlink/?linkid=832969">Learn more</a>
        /// </summary>
        double? ISupportSampling.SamplingPercentage
        {
            get { return this.samplingPercentage; }
            set { this.samplingPercentage = value; }
        }

        /// <summary>
        /// Gets item type for sampling evaluation.
        /// </summary>
        public SamplingTelemetryItemTypes ItemTypeFlag => SamplingTelemetryItemTypes.PageView;

        /// <inheritdoc/>
        public SamplingDecision ProactiveSamplingDecision { get; set; }

        /// <summary>
        /// Deeply clones a <see cref="PageViewTelemetry"/> object.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public ITelemetry DeepClone()
        {
            return new PageViewTelemetry(this);
        }

        /// <inheritdoc/>
        public void SerializeData(ISerializationWriter serializationWriter)
        {
            if (serializationWriter == null)
            {
                throw new ArgumentNullException(nameof(serializationWriter));
            }

            serializationWriter.WriteProperty(this.Data);
        }

        /// <summary>
        /// Sanitizes the properties based on constraints.
        /// </summary>
        void ITelemetry.Sanitize()
        {
            this.Name = this.Name.SanitizeName();
            this.Name = Utils.PopulateRequiredStringValue(this.Name, "name", typeof(PageViewTelemetry).FullName);
            this.Properties.SanitizeProperties();
            this.Metrics.SanitizeMeasurements();
            this.Url = this.Url.SanitizeUri();
            this.Id.SanitizeName();
        }
    }
}
