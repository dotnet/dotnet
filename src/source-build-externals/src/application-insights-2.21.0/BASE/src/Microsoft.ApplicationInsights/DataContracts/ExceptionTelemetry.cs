namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Telemetry type used to track exceptions. This will capture TypeName, Message, and CallStack.
    /// <a href="https://go.microsoft.com/fwlink/?linkid=723596">Learn more</a>
    /// </summary>
    /// <remarks>
    /// Additional exception details will need to be tracked manually.
    /// </remarks>
    public sealed class ExceptionTelemetry : ITelemetry, ISupportProperties, ISupportAdvancedSampling, ISupportMetrics, IAiSerializableTelemetry
    {
        internal const string EtwEnvelopeName = "Exception";
        internal string EnvelopeName = "AppExceptions";

        internal ExceptionInfo Data = null;

        private readonly bool isCreatedFromExceptionInfo = false;

        private TelemetryContext context;
        private IExtension extension;
        private Exception exception;
        private string message;
        private double? samplingPercentage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionTelemetry"/> class with empty properties.
        /// </summary>
        public ExceptionTelemetry()
        {
            this.Data = new ExceptionInfo(new ExceptionData());
            this.context = new TelemetryContext(this.Data.Properties);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionTelemetry"/> class with empty properties.
        /// </summary>
        /// <param name="exception">Exception instance.</param>
        public ExceptionTelemetry(Exception exception)
            : this()
        {
            if (exception == null)
            {
                exception = new Exception(Utils.PopulateRequiredStringValue(null, "message", typeof(ExceptionTelemetry).FullName));
            }

            this.Exception = exception;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionTelemetry"/> class.
        /// </summary>
        /// <param name="exceptionDetailsInfoList">Exception info.</param>
        /// <param name="severityLevel">Severity level.</param>
        /// <param name="problemId">Problem id.</param>
        /// <param name="properties">Properties.</param>
        /// <param name="measurements">Measurements.</param>
        public ExceptionTelemetry(IEnumerable<ExceptionDetailsInfo> exceptionDetailsInfoList, SeverityLevel? severityLevel, string problemId,
            IDictionary<string, string> properties, IDictionary<string, double> measurements)
        {
            this.isCreatedFromExceptionInfo = true;

            ExceptionInfo exceptionInfo = new ExceptionInfo(exceptionDetailsInfoList, severityLevel, problemId, properties, measurements);

            this.Data = exceptionInfo;
            this.context = new TelemetryContext(this.Data.Properties);

            this.UpdateData(exceptionInfo);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionTelemetry"/> class by cloning an existing instance.
        /// </summary>
        /// <param name="source">Source instance of <see cref="ExceptionTelemetry"/> to clone from.</param>
        private ExceptionTelemetry(ExceptionTelemetry source)
        {
            this.isCreatedFromExceptionInfo = source.isCreatedFromExceptionInfo;

            this.Data = source.Data.DeepClone();
            this.context = source.context.DeepClone(this.Data.Properties);
            this.Sequence = source.Sequence;
            this.Timestamp = source.Timestamp;
            this.samplingPercentage = source.samplingPercentage;
            this.ProactiveSamplingDecision = source.ProactiveSamplingDecision;

            if (!this.isCreatedFromExceptionInfo)
            {
                this.exception = source.Exception;
            }

            this.extension = source.extension?.DeepClone();
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
        string IAiSerializableTelemetry.BaseType => nameof(ExceptionData);

        /// <summary>
        /// Gets or sets date and time when telemetry was recorded.
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
        /// Gets or sets the problemId.
        /// </summary>
        public string ProblemId
        {
            get
            {
                return this.Data.ProblemId;
            }

            set
            {
                this.Data.ProblemId = value;
            }
        }

        /// <summary>
        /// Gets or sets the value indicated where the exception was handled.
        /// </summary>
        [Obsolete("Use custom properties to report exception handling layer")]
        public ExceptionHandledAt HandledAt
        {
            get
            {
                if (this.Properties.ContainsKey("handledAt") && Enum.TryParse(this.Properties["handledAt"], out ExceptionHandledAt result))
                {
                    return result;
                }
                else
                {
                    return default(ExceptionHandledAt);
                }
            }

            set
            {
                this.Properties["handledAt"] = value.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the original exception tracked by this <see cref="ITelemetry"/>.
        /// </summary>
        public Exception Exception
        {
            get
            {
                return this.isCreatedFromExceptionInfo
                    ? this.ConstructExceptionFromDetailsInfo(this.Data.ExceptionDetailsInfoList ?? new List<ExceptionDetailsInfo>().AsReadOnly())
                    : this.exception;
            }

            set
            {
                if (this.isCreatedFromExceptionInfo)
                {
                    throw new InvalidOperationException(
                        "The property is unavailable to be set on an instance created with the ExceptionDetailsInfo-based constructor");
                }

                this.exception = value;
                this.UpdateData(value);
            }
        }

        /// <summary>
        /// Gets or sets ExceptionTelemetry message.
        /// </summary>
        public string Message
        {
            get
            {
                const string ExceptionMessageSeparator = " <--- ";

                return this.isCreatedFromExceptionInfo
                    ? (this.Data.ExceptionDetailsInfoList != null ? string.Join(ExceptionMessageSeparator, this.Data.ExceptionDetailsInfoList.Select(info => info.Message)) : string.Empty)
                    : this.message;
            }

            set
            {
                if (this.isCreatedFromExceptionInfo)
                {
                    throw new InvalidOperationException(
                        "The property is unavailable to be set on an instance created with the ExceptionDetailsInfo-based constructor");
                }

                this.message = value;

                if (this.Data.ExceptionDetailsInfoList != null && this.Data.ExceptionDetailsInfoList.Count > 0)
                {
                    this.Data.ExceptionDetailsInfoList[0].Message = value;
                }
                else
                {
                    this.UpdateData(this.Exception);
                }
            }
        }

        /// <summary>
        /// Gets a dictionary of application-defined exception metrics.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public IDictionary<string, double> Metrics
        {
            get { return this.Data.Measurements; }
        }

        /// <summary>
        /// Gets the list of <see cref="ExceptionDetailsInfo"/>. User can modify the contents of individual object, but
        /// not the list itself.
        /// </summary>
        public IReadOnlyList<ExceptionDetailsInfo> ExceptionDetailsInfoList => this.Data.ExceptionDetailsInfoList;

        /// <summary>
        /// Gets a dictionary of application-defined property names and values providing additional information about this exception.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public IDictionary<string, string> Properties
        {
#pragma warning disable CS0618 // Type or member is obsolete
            get
            {
                if (!string.IsNullOrEmpty(this.MetricExtractorInfo) && !this.Context.Properties.ContainsKey(MetricTerms.Extraction.ProcessedByExtractors.Moniker.Key))
                {
                    this.Context.Properties[MetricTerms.Extraction.ProcessedByExtractors.Moniker.Key] = this.MetricExtractorInfo;
                }

                return this.Context.Properties;
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        /// <summary>
        /// Gets or sets Exception severity level.
        /// </summary>
        public SeverityLevel? SeverityLevel
        {
            get => this.Data.SeverityLevel;
            set => this.Data.SeverityLevel = value;
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
        public SamplingTelemetryItemTypes ItemTypeFlag => SamplingTelemetryItemTypes.Exception;

        /// <inheritdoc/>
        public SamplingDecision ProactiveSamplingDecision { get; set; }

        internal IList<ExceptionDetails> Exceptions
        {
            get { return this.Data.Data.exceptions; }
        }

        /// <summary>
        /// Gets or sets the MetricExtractorInfo.
        /// </summary>
        internal string MetricExtractorInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Deeply clones a <see cref="ExceptionTelemetry"/> object.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public ITelemetry DeepClone()
        {
            return new ExceptionTelemetry(this);
        }

        /// <inheritdoc/>
        public void SerializeData(ISerializationWriter serializationWriter)
        {
            if (serializationWriter == null)
            {
                throw new ArgumentNullException(nameof(serializationWriter));
            }

            serializationWriter.WriteProperty(this.Data.Data);
        }

        /// <summary>
        /// Set parsedStack from an array of StackFrame objects.
        /// </summary>
        public void SetParsedStack(System.Diagnostics.StackFrame[] frames)
        {
            if (this.Exceptions != null && this.Exceptions.Count > 0)
            {
                if (frames != null && frames.Length > 0)
                {
                    int stackLength = 0;

                    this.Exceptions[0].parsedStack = new List<Extensibility.Implementation.External.StackFrame>();
                    this.Exceptions[0].hasFullStack = true;

                    for (int level = 0; level < frames.Length; level++)
                    {
                        var sf = ExceptionConverter.GetStackFrame(frames[level], level);

                        stackLength += ExceptionConverter.GetStackFrameLength(sf);

                        if (stackLength > ExceptionConverter.MaxParsedStackLength)
                        {
                            this.Exceptions[0].hasFullStack = false;
                            break;
                        }

                        this.Exceptions[0].parsedStack.Add(sf);
                    }
                }
            }
        }

        /// <summary>
        /// Sanitizes the properties based on constraints.
        /// </summary>
        void ITelemetry.Sanitize()
        {
            // Sanitize on the ExceptionDetails stack information for raw stack and parsed stack is done while creating the object in ExceptionConverter.cs
            this.message = this.message.SanitizeMessage();
            this.Properties.SanitizeProperties();
            this.Metrics.SanitizeMeasurements();
        }

        private void ConvertExceptionTree(Exception exception, ExceptionDetails parentExceptionDetails, List<ExceptionDetails> exceptions)
        {
            if (exception == null)
            {
                exception = new Exception(Utils.PopulateRequiredStringValue(null, "message", typeof(ExceptionTelemetry).FullName));
            }

            ExceptionDetails exceptionDetails = ExceptionConverter.ConvertToExceptionDetails(exception, parentExceptionDetails);

            // For upper level exception see if Message was provided and do not use exceptiom.message in that case
            if (parentExceptionDetails == null && !string.IsNullOrWhiteSpace(this.Message))
            {
                exceptionDetails.message = this.Message;
            }

            exceptions.Add(exceptionDetails);

            if (exception is AggregateException aggregate)
            {
                foreach (Exception inner in aggregate.InnerExceptions)
                {
                    this.ConvertExceptionTree(inner, exceptionDetails, exceptions);
                }
            }
            else if (exception.InnerException != null)
            {
                this.ConvertExceptionTree(exception.InnerException, exceptionDetails, exceptions);
            }
        }

        private void UpdateData(Exception exception)
        {
            if (this.isCreatedFromExceptionInfo)
            {
                throw new InvalidOperationException("Operation is not supported given the state of the object.");
            }

            try
            {
                // collect the set of exceptions detail info from the passed in exception
                List<ExceptionDetails> exceptions = new List<ExceptionDetails>();
                this.ConvertExceptionTree(exception, null, exceptions);

                // trim if we have too many, also add a custom exception to let the user know we're trimmed
                if (exceptions.Count > Constants.MaxExceptionCountToSave)
                {
                    // TODO: when we localize these messages, we should consider not using InvariantCulture
                    // create our "message" exception.
                    InnerExceptionCountExceededException countExceededException =
                        new InnerExceptionCountExceededException(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "The number of inner exceptions was {0} which is larger than {1}, the maximum number allowed during transmission. All but the first {1} have been dropped.",
                                exceptions.Count,
                                Constants.MaxExceptionCountToSave));

                    // remove all but the first N exceptions
                    exceptions.RemoveRange(Constants.MaxExceptionCountToSave,
                        exceptions.Count - Constants.MaxExceptionCountToSave);

                    // we'll add our new exception and parent it to the root exception (first one in the list)
                    exceptions.Add(ExceptionConverter.ConvertToExceptionDetails(countExceededException, exceptions[0]));
                }

                this.Data = new ExceptionInfo(exceptions.Select(ex => new ExceptionDetailsInfo(ex)), this.SeverityLevel,
                    this.ProblemId, this.Properties, this.Metrics);
                this.context = new TelemetryContext(this.Data.Properties);
            }
            catch (Exception ex)
            {
                CoreEventSource.Log.UpdateDataFailed(ex.ToInvariantString());
            }
        }

        private void UpdateData(ExceptionInfo exceptionInfo)
        {
            if (!this.isCreatedFromExceptionInfo)
            {
                throw new InvalidOperationException("Operation is not supported given the state of the object.");
            }

            this.Data = exceptionInfo ?? throw new ArgumentNullException(nameof(exceptionInfo));
            this.context = new TelemetryContext(this.Data.Properties);
        }

        private Exception ConstructExceptionFromDetailsInfo(IReadOnlyList<ExceptionDetailsInfo> exceptionInfos)
        {
            if (!this.isCreatedFromExceptionInfo)
            {
                throw new InvalidOperationException("Operation is not supported given the state of the object.");
            }

            // construct a fake Exception object based on provided information
            if (!exceptionInfos.Any())
            {
                return new Exception(string.Empty);
            }

            return new Exception(exceptionInfos[0].Message, this.ConstructInnerException(exceptionInfos, 0));
        }

        private Exception ConstructInnerException(IReadOnlyList<ExceptionDetailsInfo> exceptionInfos, int parentExceptionIndex)
        {
            // inner exception is the next one after the parent
            int index = parentExceptionIndex + 1;

            if (index < exceptionInfos.Count)
            {
                // inner exception exists
                return new Exception(exceptionInfos[index].Message, this.ConstructInnerException(exceptionInfos, index));
            }

            // inner exception doesn't exist
            return null;
        }
    }
}
