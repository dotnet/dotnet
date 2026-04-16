namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Common.Internal;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Experimental;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.ServiceContract;
    using Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService;

    /// <summary>
    /// Extracts QuickPulse data from the telemetry stream.
    /// </summary>
    public class QuickPulseTelemetryProcessor : ITelemetryProcessor, ITelemetryModule, IQuickPulseTelemetryProcessor
    {
        /// <summary>
        /// 1.0 - initial release.
        /// 1.1 - added DocumentStreamId, EventTelemetryDocument, TraceTelemetryDocument.
        /// </summary>
        private const string TelemetryDocumentContractVersion = "1.1";

        private const float MaxGlobalTelemetryQuota = 30f * 10f;

        private const float InitialGlobalTelemetryQuota = 3f * 10f;

        private const int MaxFieldLength = 32768;

        private const int MaxPropertyCount = 10;

        private const string SpecialDependencyPropertyName = "ErrorMessage";

        private const string ExceptionMessageSeparator = " <--- ";

        /// <summary>
        /// An overall, cross-stream quota tracker.
        /// </summary>
        private QuickPulseQuotaTracker globalQuotaTracker;

        private IQuickPulseDataAccumulatorManager dataAccumulatorManager = null;

        private Uri currentServiceEndpoint = QuickPulseDefaults.QuickPulseServiceEndpoint;

        private TelemetryConfiguration config = null;

        private bool isCollecting = false;

        private bool disableFullTelemetryItems = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickPulseTelemetryProcessor"/> class.
        /// </summary>
        /// <param name="next">The next TelemetryProcessor in the chain.</param>
        /// <exception cref="ArgumentNullException">Thrown if next is null.</exception>
        public QuickPulseTelemetryProcessor(ITelemetryProcessor next)
            : this(next, new Clock())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickPulseTelemetryProcessor"/> class. Internal constructor for unit tests only.
        /// </summary>
        /// <param name="next">The next TelemetryProcessor in the chain.</param>
        /// <param name="timeProvider">Time provider.</param>
        /// <param name="maxGlobalTelemetryQuota">Max overall telemetry quota.</param>
        /// <param name="initialGlobalTelemetryQuota">Initial overall telemetry quota.</param>
        /// <param name="quotaAccrualRatePerSec">Quota Accrual rate per second.</param>
        /// <exception cref="ArgumentNullException">Thrown if next is null.</exception>
        internal QuickPulseTelemetryProcessor(
            ITelemetryProcessor next,
            Clock timeProvider,
            float? maxGlobalTelemetryQuota = null,
            float? initialGlobalTelemetryQuota = null,
            float? quotaAccrualRatePerSec = null)
        {
            this.Next = next ?? throw new ArgumentNullException(nameof(next));

            this.RegisterSelfWithQuickPulseTelemetryModule();

            this.globalQuotaTracker = new QuickPulseQuotaTracker(
                timeProvider,
                maxGlobalTelemetryQuota ?? MaxGlobalTelemetryQuota,
                initialGlobalTelemetryQuota ?? InitialGlobalTelemetryQuota,
                quotaAccrualRatePerSec);
        }

        /// <summary>
        /// Gets or sets an endpoint that is compared against telemetry to remove our requests from customer telemetry.
        /// </summary>
        /// <remarks>
        /// This is set from the QuickPulseTelemetryModule. The value might be changing as we communicate with the service, so this might be updated in flight.
        /// </remarks>
        Uri IQuickPulseTelemetryProcessor.ServiceEndpoint
        {
            get => Volatile.Read(ref this.currentServiceEndpoint);
            set => Volatile.Write(ref this.currentServiceEndpoint, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether request properties
        /// which were disabled via "RequestTrackingTelemetryModule.DisableTrackingProperties" should be evaluated.
        /// </summary>
        /// <remarks>This feature is still being evaluated and not recommended for end users.</remarks>
        internal bool EvaluateDisabledTrackingProperties { get; set; }

        private ITelemetryProcessor Next { get; }

        /// <summary>
        /// Initialize method is called after all configuration properties have been loaded from the configuration.
        /// </summary>
        public void Initialize(TelemetryConfiguration configuration)
        {
            /*
            The configuration that is being passed into this method is the configuration that is the reason
            why this instance of telemetry processor was created. Regardless of which instrumentation key is passed in,
            this telemetry processor will only collect for whichever instrumentation key is specified by the module in StartCollection call.
            */

            this.EvaluateDisabledTrackingProperties = configuration.EvaluateExperimentalFeature(ExperimentalConstants.DeferRequestTrackingProperties);

            this.RegisterSelfWithQuickPulseTelemetryModule();
        }

        void IQuickPulseTelemetryProcessor.UpdateGlobalQuotas(Clock timeProvider, QuotaConfigurationInfo quotaInfo)
        {
            if (quotaInfo != null)
            {
                this.globalQuotaTracker = new QuickPulseQuotaTracker(
                    timeProvider,
                    quotaInfo.MaxQuota,
                    quotaInfo.InitialQuota ?? InitialGlobalTelemetryQuota,
                    quotaInfo.QuotaAccrualRatePerSec);
            }
            else
            {
                this.globalQuotaTracker = new QuickPulseQuotaTracker(
                    timeProvider,
                    MaxGlobalTelemetryQuota,
                    InitialGlobalTelemetryQuota);
            }
        }

        void IQuickPulseTelemetryProcessor.StartCollection(
            IQuickPulseDataAccumulatorManager accumulatorManager,
            Uri serviceEndpoint,
            TelemetryConfiguration configuration,
            bool disableFullTelemetryItems)
        {
            if (this.isCollecting)
            {
                throw new InvalidOperationException("Can't start collection while it is already running.");
            }

            this.dataAccumulatorManager = accumulatorManager;
            Volatile.Write(ref this.currentServiceEndpoint, serviceEndpoint);
            this.config = configuration;
            this.isCollecting = true;
            this.disableFullTelemetryItems = disableFullTelemetryItems;
        }

        void IQuickPulseTelemetryProcessor.StopCollection()
        {
            this.dataAccumulatorManager = null;
            this.isCollecting = false;
        }

        /// <summary>
        /// Intercepts telemetry items and updates QuickPulse data when needed.
        /// </summary>
        /// <param name="telemetry">Telemetry item being tracked by AI.</param>
        /// <remarks>This method is performance critical since every AI telemetry item goes through it.</remarks>
        public void Process(ITelemetry telemetry)
        {
            bool letTelemetryThrough = true;

            try
            {
                // filter out QPS requests from dependencies even when we're not collecting (for Pings)
                var dependency = telemetry as DependencyTelemetry;
                Uri localCurrentServiceEndpoint = Volatile.Read(ref this.currentServiceEndpoint);
                if (localCurrentServiceEndpoint != null && !string.IsNullOrWhiteSpace(dependency?.Target))
                {
                    if (dependency.Target.IndexOf(localCurrentServiceEndpoint.Host, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // this is an HTTP request to QuickPulse service, we don't want to let it through
                        letTelemetryThrough = false;

                        return;
                    }
                }

                if (!this.isCollecting || this.dataAccumulatorManager == null)
                {
                    return;
                }

                this.ProcessTelemetry(telemetry);
            }
            catch (Exception e)
            {
                // whatever happened up there - we don't want to interrupt the chain of processors
                QuickPulseEventSource.Log.UnknownErrorEvent(e.ToInvariantString());
            }
            finally
            {
                if (letTelemetryThrough)
                {
                    this.Next.Process(telemetry);
                }
            }
        }

        private static ITelemetryDocument ConvertDependencyToTelemetryDocument(DependencyTelemetry dependencyTelemetry)
        {
            ITelemetryDocument telemetryDocument = new DependencyTelemetryDocument()
            {
                Id = Guid.NewGuid(),
                Version = TelemetryDocumentContractVersion,
                Timestamp = dependencyTelemetry.Timestamp,
                Name = TruncateValue(dependencyTelemetry.Name),
                Target = TruncateValue(dependencyTelemetry.Target),
                Success = dependencyTelemetry.Success,
                Duration = dependencyTelemetry.Duration,
                OperationId = TruncateValue(dependencyTelemetry.Context?.Operation?.Id),
                ResultCode = dependencyTelemetry.ResultCode,
                CommandName = TruncateValue(dependencyTelemetry.Data),
                DependencyTypeName = dependencyTelemetry.Type,
                Properties = GetProperties(dependencyTelemetry, SpecialDependencyPropertyName),
            };

            SetCommonTelemetryDocumentData(telemetryDocument, dependencyTelemetry);

            return telemetryDocument;
        }

        private static ITelemetryDocument ConvertExceptionToTelemetryDocument(ExceptionTelemetry exceptionTelemetry)
        {
            ITelemetryDocument telemetryDocument = new ExceptionTelemetryDocument()
            {
                Id = Guid.NewGuid(),
                Version = TelemetryDocumentContractVersion,
                SeverityLevel = exceptionTelemetry.SeverityLevel != null ? exceptionTelemetry.SeverityLevel.Value.ToString() : null,
                Exception = exceptionTelemetry.Exception != null ? TruncateValue(exceptionTelemetry.Exception.ToString()) : null,
                ExceptionType = exceptionTelemetry.Exception != null ? TruncateValue(exceptionTelemetry.Exception.GetType().FullName) : null,
                ExceptionMessage = TruncateValue(ExpandExceptionMessage(exceptionTelemetry)),
                OperationId = TruncateValue(exceptionTelemetry.Context?.Operation?.Id),
                Properties = GetProperties(exceptionTelemetry),
            };

            SetCommonTelemetryDocumentData(telemetryDocument, exceptionTelemetry);

            return telemetryDocument;
        }

        private static ITelemetryDocument ConvertEventToTelemetryDocument(EventTelemetry eventTelemetry)
        {
            ITelemetryDocument telemetryDocument = new EventTelemetryDocument()
            {
                Id = Guid.NewGuid(),
                Version = TelemetryDocumentContractVersion,
                Timestamp = eventTelemetry.Timestamp,
                OperationId = TruncateValue(eventTelemetry.Context?.Operation?.Id),
                Name = TruncateValue(eventTelemetry.Name),
                Properties = GetProperties(eventTelemetry),
            };

            SetCommonTelemetryDocumentData(telemetryDocument, eventTelemetry);

            return telemetryDocument;
        }

        private static ITelemetryDocument ConvertTraceToTelemetryDocument(TraceTelemetry traceTelemetry)
        {
            ITelemetryDocument telemetryDocument = new TraceTelemetryDocument()
            {
                Id = Guid.NewGuid(),
                Version = TelemetryDocumentContractVersion,
                Timestamp = traceTelemetry.Timestamp,
                Message = TruncateValue(traceTelemetry.Message),
                SeverityLevel = traceTelemetry.SeverityLevel.ToString(),
                Properties = GetProperties(traceTelemetry),
            };

            SetCommonTelemetryDocumentData(telemetryDocument, traceTelemetry);

            return telemetryDocument;
        }

        private static void SetCommonTelemetryDocumentData(ITelemetryDocument telemetryDocument, ITelemetry telemetry)
        {
            if (telemetry.Context == null)
            {
                return;
            }

            telemetryDocument.OperationName = TruncateValue(telemetry.Context.Operation?.Name);
            telemetryDocument.InternalNodeName = TruncateValue(telemetry.Context.GetInternalContext()?.NodeName);
            telemetryDocument.CloudRoleName = TruncateValue(telemetry.Context.Cloud?.RoleName);
            telemetryDocument.CloudRoleInstance = TruncateValue(telemetry.Context.Cloud?.RoleInstance);
        }

        private static string ExpandExceptionMessage(ExceptionTelemetry exceptionTelemetry)
        {
            Exception exception = exceptionTelemetry.Exception;

            if (exception == null)
            {
                return string.Empty;
            }

            if (exception.InnerException == null)
            {
                // perf optimization for a special case
                return exception.Message;
            }

            // use a fake AggregateException to take advantage of Flatten()
            var nonAggregateExceptions = new AggregateException(exception).Flatten().InnerExceptions;

            var messageHashes = new HashSet<string>();
            var nonDuplicateMessages = new LinkedList<string>();

            foreach (var ex in nonAggregateExceptions)
            {
                foreach (var msg in FlattenMessages(ex))
                {
                    if (!messageHashes.Contains(msg))
                    {
                        nonDuplicateMessages.AddLast(msg);

                        messageHashes.Add(msg);
                    }
                }
            }

            return string.Join(ExceptionMessageSeparator, nonDuplicateMessages);
        }

        private static IEnumerable<string> FlattenMessages(Exception exception)
        {
            var currentEx = exception;
            while (currentEx != null)
            {
                yield return currentEx.Message;

                currentEx = currentEx.InnerException;
            }
        }

        private static bool IsRequestSuccessful(RequestTelemetry request)
        {
            string responseCode = request.ResponseCode;
            bool? success = request.Success;

            if (string.IsNullOrWhiteSpace(responseCode))
            {
                return true;
            }

            if (success != null)
            {
                return success.Value;
            }

            int responseCodeInt;
            if (int.TryParse(responseCode, NumberStyles.Any, CultureInfo.InvariantCulture, out responseCodeInt))
            {
                return (responseCodeInt < 400) || (responseCodeInt == 401);
            }

            return true;
        }

        private static string TruncateValue(string value)
        {
            if (value != null && value.Length > MaxFieldLength)
            {
                value = value.Substring(0, MaxFieldLength);
            }

            return value;
        }

        private static KeyValuePair<string, string>[] GetProperties(ISupportProperties telemetry, string specialPropertyName = null)
        {
            Dictionary<string, string> properties = null;

            if (telemetry.Properties != null && telemetry.Properties.Count > 0)
            {
                properties = new Dictionary<string, string>(MaxPropertyCount + 1);

                foreach (var prop in
                    telemetry.Properties.Where(p => !string.Equals(p.Key, specialPropertyName, StringComparison.Ordinal)).OrderBy(p => p.Key, StringComparer.Ordinal).Take(MaxPropertyCount))
                {
                    string truncatedKey = TruncateValue(prop.Key);

                    if (!properties.ContainsKey(truncatedKey))
                    {
                        properties.Add(truncatedKey, TruncateValue(prop.Value));
                    }
                }

                if (specialPropertyName != null)
                {
                    string specialPropertyValue;
                    if (telemetry.Properties.TryGetValue(specialPropertyName, out specialPropertyValue))
                    {
                        properties.Add(TruncateValue(specialPropertyName), TruncateValue(specialPropertyValue));
                    }
                }
            }

            return properties != null ? properties.ToArray() : null;
        }

        private static void ProcessMetrics<TTelemetry>(
            CollectionConfigurationAccumulator configurationAccumulatorLocal,
            IEnumerable<CalculatedMetric<TTelemetry>> metrics,
            TTelemetry telemetry,
            out CollectionConfigurationError[] filteringErrors,
            ref string projectionError)
        {
            filteringErrors = ArrayExtensions.Empty<CollectionConfigurationError>();

            foreach (CalculatedMetric<TTelemetry> metric in metrics)
            {
                if (metric.CheckFilters(telemetry, out filteringErrors))
                {
                    // the telemetry document has passed the filters, count it in and project
                    try
                    {
                        double projection = metric.Project(telemetry);

                        configurationAccumulatorLocal.MetricAccumulators[metric.Id].AddValue(projection);
                    }
                    catch (Exception e)
                    {
                        // most likely the projection did not result in a value parsable by double.Parse()
                        projectionError = e.ToString();
                    }
                }
            }
        }

        private void ProcessTelemetry(ITelemetry telemetry)
        {
            // only process items that are going to the instrumentation key that our module is initialized with
            if (string.IsNullOrWhiteSpace(this.config?.InstrumentationKey)
                || !string.Equals(telemetry?.Context?.InstrumentationKey, this.config.InstrumentationKey, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var telemetryAsRequest = telemetry as RequestTelemetry;
            var telemetryAsDependency = telemetry as DependencyTelemetry;
            var telemetryAsException = telemetry as ExceptionTelemetry;
            var telemetryAsEvent = telemetry as EventTelemetry;
            var telemetryAsTrace = telemetry as TraceTelemetry;

            // update aggregates
            bool? originalRequestTelemetrySuccessValue = null;
            if (telemetryAsRequest != null)
            {
                // special treatment for RequestTelemetry.Success
                originalRequestTelemetrySuccessValue = telemetryAsRequest.Success;
                telemetryAsRequest.Success = IsRequestSuccessful(telemetryAsRequest);

                this.UpdateRequestAggregates(telemetryAsRequest);
            }
            else if (telemetryAsDependency != null)
            {
                this.UpdateDependencyAggregates(telemetryAsDependency);
            }
            else if (telemetryAsException != null)
            {
                this.UpdateExceptionAggregates();
            }

            // get a local reference, the accumulator might get swapped out at any time
            // in case we continue to process this configuration once the accumulator is out, increase the reference count so that this accumulator is not sent out before we're done
            CollectionConfigurationAccumulator configurationAccumulatorLocal =
                this.dataAccumulatorManager.CurrentDataAccumulator.CollectionConfigurationAccumulator;

            // if the accumulator is swapped out and a sample is created and sent out - all while between these two lines, this telemetry item gets lost
            // however, that is not likely to happen
            configurationAccumulatorLocal.AddRef();

            try
            {
                // collect full telemetry items
                if (!this.disableFullTelemetryItems)
                {
                    ITelemetryDocument telemetryDocument = null;
                    IEnumerable<DocumentStream> documentStreams = configurationAccumulatorLocal.CollectionConfiguration.DocumentStreams;

                    // !!! report runtime errors for filter groups?
                    CollectionConfigurationError[] groupErrors;

                    if (telemetryAsRequest != null)
                    {
                        telemetryDocument = this.CreateTelemetryDocument(
                            telemetryAsRequest,
                            documentStreams,
                            documentStream => documentStream.RequestQuotaTracker,
                            documentStream => documentStream.CheckFilters(telemetryAsRequest, out groupErrors),
                            this.ConvertRequestToTelemetryDocument);
                    }
                    else if (telemetryAsDependency != null)
                    {
                        telemetryDocument = this.CreateTelemetryDocument(
                            telemetryAsDependency,
                            documentStreams,
                            documentStream => documentStream.DependencyQuotaTracker,
                            documentStream => documentStream.CheckFilters(telemetryAsDependency, out groupErrors),
                            ConvertDependencyToTelemetryDocument);
                    }
                    else if (telemetryAsException != null)
                    {
                        telemetryDocument = this.CreateTelemetryDocument(
                            telemetryAsException,
                            documentStreams,
                            documentStream => documentStream.ExceptionQuotaTracker,
                            documentStream => documentStream.CheckFilters(telemetryAsException, out groupErrors),
                            ConvertExceptionToTelemetryDocument);
                    }
                    else if (telemetryAsEvent != null)
                    {
                        telemetryDocument = this.CreateTelemetryDocument(
                            telemetryAsEvent,
                            documentStreams,
                            documentStream => documentStream.EventQuotaTracker,
                            documentStream => documentStream.CheckFilters(telemetryAsEvent, out groupErrors),
                            ConvertEventToTelemetryDocument);
                    }
                    else if (telemetryAsTrace != null)
                    {
                        telemetryDocument = this.CreateTelemetryDocument(
                            telemetryAsTrace,
                            documentStreams,
                            documentStream => documentStream.TraceQuotaTracker,
                            documentStream => documentStream.CheckFilters(telemetryAsTrace, out groupErrors),
                            ConvertTraceToTelemetryDocument);
                    }

                    if (telemetryDocument != null)
                    {
                        this.dataAccumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Push(telemetryDocument);
                    }

                    this.dataAccumulatorManager.CurrentDataAccumulator.GlobalDocumentQuotaReached = this.globalQuotaTracker.QuotaExhausted;
                }

                // collect calculated metrics
                CollectionConfigurationError[] filteringErrors;
                string projectionError = null;

                if (telemetryAsRequest != null)
                {
                    QuickPulseTelemetryProcessor.ProcessMetrics(
                        configurationAccumulatorLocal,
                        configurationAccumulatorLocal.CollectionConfiguration.RequestMetrics,
                        telemetryAsRequest,
                        out filteringErrors,
                        ref projectionError);
                }
                else if (telemetryAsDependency != null)
                {
                    QuickPulseTelemetryProcessor.ProcessMetrics(
                        configurationAccumulatorLocal,
                        configurationAccumulatorLocal.CollectionConfiguration.DependencyMetrics,
                        telemetryAsDependency,
                        out filteringErrors,
                        ref projectionError);
                }
                else if (telemetryAsException != null)
                {
                    QuickPulseTelemetryProcessor.ProcessMetrics(
                        configurationAccumulatorLocal,
                        configurationAccumulatorLocal.CollectionConfiguration.ExceptionMetrics,
                        telemetryAsException,
                        out filteringErrors,
                        ref projectionError);
                }
                else if (telemetryAsEvent != null)
                {
                    QuickPulseTelemetryProcessor.ProcessMetrics(
                        configurationAccumulatorLocal,
                        configurationAccumulatorLocal.CollectionConfiguration.EventMetrics,
                        telemetryAsEvent,
                        out filteringErrors,
                        ref projectionError);
                }
                else if (telemetryAsTrace != null)
                {
                    QuickPulseTelemetryProcessor.ProcessMetrics(
                        configurationAccumulatorLocal,
                        configurationAccumulatorLocal.CollectionConfiguration.TraceMetrics,
                        telemetryAsTrace,
                        out filteringErrors,
                        ref projectionError);
                }

                // !!! report errors from string[] errors; and string projectionError;
            }
            finally
            {
                // special treatment for RequestTelemetry.Success - restore the value
                if (telemetryAsRequest != null)
                {
                    telemetryAsRequest.Success = originalRequestTelemetrySuccessValue;
                }

                configurationAccumulatorLocal.Release();
            }
        }

        private ITelemetryDocument ConvertRequestToTelemetryDocument(RequestTelemetry requestTelemetry)
        {
            var url = requestTelemetry.Url;
#if NET452
            if (this.EvaluateDisabledTrackingProperties && url == null)
            {
                try
                {
                    // some of the requestTelemetry properties might be deferred by using RequestTrackingTelemetryModule.DisableTrackingProperties.
                    // evaluate them now
                    // note: RequestTrackingUtilities.UpdateRequestTelemetryFromRequest is not used here, since not all fields need to be populated
                    var request = System.Web.HttpContext.Current?.Request;
                    url = request?.Unvalidated.Url;
                }
                catch (Exception e)
                {
                    QuickPulseEventSource.Log.UnknownErrorEvent(e.ToInvariantString());
                }
            }
#endif

            ITelemetryDocument telemetryDocument = new RequestTelemetryDocument()
            {
                Id = Guid.NewGuid(),
                Version = TelemetryDocumentContractVersion,
                Timestamp = requestTelemetry.Timestamp,
                OperationId = TruncateValue(requestTelemetry.Context?.Operation?.Id),
                Name = TruncateValue(requestTelemetry.Name),
                Success = requestTelemetry.Success,
                Duration = requestTelemetry.Duration,
                ResponseCode = requestTelemetry.ResponseCode,
                Url = url,
                Properties = GetProperties(requestTelemetry),
            };

            SetCommonTelemetryDocumentData(telemetryDocument, requestTelemetry);

            return telemetryDocument;
        }

        private ITelemetryDocument CreateTelemetryDocument<TTelemetry>(
            TTelemetry telemetry,
            IEnumerable<DocumentStream> documentStreams,
            Func<DocumentStream, QuickPulseQuotaTracker> getQuotaTracker,
            Func<DocumentStream, bool> checkDocumentStreamFilters,
            Func<TTelemetry, ITelemetryDocument> convertTelemetryToTelemetryDocument)
        {
            // check which document streams are interested in this telemetry
            ITelemetryDocument telemetryDocument = null;
            var matchingDocumentStreamIds = new List<string>();

            foreach (DocumentStream matchingDocumentStream in documentStreams.Where(checkDocumentStreamFilters))
            {
                // for each interested document stream only let the document through if there's quota available for that stream
                if (getQuotaTracker(matchingDocumentStream).ApplyQuota())
                {
                    // only create the telemetry document once
                    telemetryDocument = telemetryDocument ?? convertTelemetryToTelemetryDocument(telemetry);

                    matchingDocumentStreamIds.Add(matchingDocumentStream.Id);
                }
            }

            if (telemetryDocument != null)
            {
                telemetryDocument.DocumentStreamIds = matchingDocumentStreamIds.ToArray();

                // this document will count as 1 towards the global quota regardless of number of streams that are interested in it
                telemetryDocument = this.globalQuotaTracker.ApplyQuota() ? telemetryDocument : null;
            }

            return telemetryDocument;
        }

        private void UpdateExceptionAggregates()
        {
            Interlocked.Increment(ref this.dataAccumulatorManager.CurrentDataAccumulator.AIExceptionCount);
        }

        private void UpdateDependencyAggregates(DependencyTelemetry dependencyTelemetry)
        {
            long dependencyCallCountAndDurationInTicks = QuickPulseDataAccumulator.EncodeCountAndDuration(1, dependencyTelemetry.Duration.Ticks);

            Interlocked.Add(
                ref this.dataAccumulatorManager.CurrentDataAccumulator.AIDependencyCallCountAndDurationInTicks,
                dependencyCallCountAndDurationInTicks);

            if (dependencyTelemetry.Success == true)
            {
                Interlocked.Increment(ref this.dataAccumulatorManager.CurrentDataAccumulator.AIDependencyCallSuccessCount);
            }
            else if (dependencyTelemetry.Success == false)
            {
                Interlocked.Increment(ref this.dataAccumulatorManager.CurrentDataAccumulator.AIDependencyCallFailureCount);
            }
        }

        private void UpdateRequestAggregates(RequestTelemetry requestTelemetry)
        {
            long requestCountAndDurationInTicks = QuickPulseDataAccumulator.EncodeCountAndDuration(1, requestTelemetry.Duration.Ticks);

            Interlocked.Add(ref this.dataAccumulatorManager.CurrentDataAccumulator.AIRequestCountAndDurationInTicks, requestCountAndDurationInTicks);

            if (requestTelemetry.Success == true)
            {
                Interlocked.Increment(ref this.dataAccumulatorManager.CurrentDataAccumulator.AIRequestSuccessCount);
            }
            else
            {
                Interlocked.Increment(ref this.dataAccumulatorManager.CurrentDataAccumulator.AIRequestFailureCount);
            }
        }

        private void RegisterSelfWithQuickPulseTelemetryModule()
        {
            var module = TelemetryModules.Instance.Modules.OfType<QuickPulseTelemetryModule>().SingleOrDefault();

            if (module != null)
            {
                module.RegisterTelemetryProcessor(this);
                Volatile.Write(ref this.currentServiceEndpoint, module.ServiceClient?.CurrentServiceUri ?? QuickPulseDefaults.QuickPulseServiceEndpoint);
            }
        }
    }
}