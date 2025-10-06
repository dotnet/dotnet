namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;

#if REDFIELD
    [EventSource(Name = "Redfield-Microsoft-ApplicationInsights-Core")]
#else
    [EventSource(Name = "Microsoft-ApplicationInsights-Core")]
#endif
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "appDomainName is required")]
    internal sealed class CoreEventSource : EventSource
    {
        public static readonly CoreEventSource Log = new CoreEventSource();

#if NETSTANDARD2_0
        public EventCounter IngestionResponseTimeCounter;
#endif

        private readonly ApplicationNameProvider nameProvider = new ApplicationNameProvider();

        internal CoreEventSource()
        {
#if NETSTANDARD2_0
            this.IngestionResponseTimeCounter = new EventCounter("IngestionEndpoint-ResponseTimeMsec", this);
#endif
        }

        public static bool IsVerboseEnabled
        {
            [NonEvent]
            get
            {
                return Log.IsEnabled(EventLevel.Verbose, (EventKeywords)(-1));
            }
        }

        /// <summary>
        /// Logs the information when there operation to track is null.
        /// </summary>
        [Event(1, Message = "Operation object is null.", Level = EventLevel.Warning)]
        public void OperationIsNullWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(1, this.nameProvider.Name);
        }

        /// <summary>
        /// Logs the information when there operation to stop does not match the current operation.
        /// </summary>
        [Event(2, Message = "Operation to stop does not match the current operation. Telemetry is not tracked.", Level = EventLevel.Error)]
        public void InvalidOperationToStopError(string appDomainName = "Incorrect")
        {
            this.WriteEvent(2, this.nameProvider.Name);
        }

        [Event(
            3,
            Keywords = Keywords.VerboseFailure,
            Message = "[msg=Log verbose];[msg={0}]",
            Level = EventLevel.Verbose)]
        public void LogVerbose(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                3,
                msg ?? string.Empty,
                this.nameProvider.Name);
        }
        
        [Event(
            4,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable,
            Message = "Diagnostics event throttling has been started for the event {0}",
            Level = EventLevel.Informational)]
        public void DiagnosticsEventThrottlingHasBeenStartedForTheEvent(
            string eventId,
            string appDomainName = "Incorrect")
        {
            this.WriteEvent(4, eventId ?? "NULL", this.nameProvider.Name);
        }

        [Event(
            5,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable,
            Message = "Diagnostics event throttling has been reset for the event {0}, event was fired {1} times during last interval",
            Level = EventLevel.Informational)]
        public void DiagnosticsEventThrottlingHasBeenResetForTheEvent(
            int eventId,
            int executionCount,
            string appDomainName = "Incorrect")
        {
            this.WriteEvent(5, eventId, executionCount, this.nameProvider.Name);
        }

        [Event(
            6,
            Keywords = Keywords.Diagnostics,
            Message = "Scheduler timer dispose failure: {0}",
            Level = EventLevel.Warning)]
        public void DiagnoisticsEventThrottlingSchedulerDisposeTimerFailure(
            string exception,
            string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                6, 
                exception ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            7,
            Keywords = Keywords.Diagnostics,
            Message = "A scheduler timer was created for the interval: {0}",
            Level = EventLevel.Verbose)]
        public void DiagnoisticsEventThrottlingSchedulerTimerWasCreated(
            string intervalInMilliseconds,
            string appDomainName = "Incorrect")
        {
            this.WriteEvent(7, intervalInMilliseconds ?? "NULL", this.nameProvider.Name);
        }

        [Event(
            8,
            Keywords = Keywords.Diagnostics,
            Message = "A scheduler timer was removed",
            Level = EventLevel.Verbose)]
        public void DiagnoisticsEventThrottlingSchedulerTimerWasRemoved(string appDomainName = "Incorrect")
        {
            this.WriteEvent(8, this.nameProvider.Name);
        }
        
        [Event(
            9,
            Message = "No Telemetry Configuration provided. Using the default TelemetryConfiguration.Active.",
            Level = EventLevel.Warning)]
        public void TelemetryClientConstructorWithNoTelemetryConfiguration(string appDomainName = "Incorrect")
        {
            this.WriteEvent(9, this.nameProvider.Name);
        }

        [Event(
            10,
            Message = "Value for property '{0}' of {1} was not found. Populating it by default.",
            Level = EventLevel.Verbose)]
        public void PopulateRequiredStringWithValue(string parameterName, string telemetryType, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                10, 
                parameterName ?? string.Empty, 
                telemetryType ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            11,
            Message = "Invalid duration for Telemetry. Setting it to '00:00:00'.",
            Level = EventLevel.Warning)]
        public void TelemetryIncorrectDuration(string appDomainName = "Incorrect")
        {
            this.WriteEvent(11, this.nameProvider.Name);
        }

        [Event(
           12,
           Message = "Telemetry tracking was disabled. Message is dropped.",
           Level = EventLevel.Verbose)]
        public void TrackingWasDisabled(string appDomainName = "Incorrect")
        {
            this.WriteEvent(12, this.nameProvider.Name);
        }

        [Event(
           13,
           Message = "Telemetry tracking was enabled. Messages are being logged.",
           Level = EventLevel.Verbose)]
        public void TrackingWasEnabled(string appDomainName = "Incorrect")
        {
            this.WriteEvent(13, this.nameProvider.Name);
        }

        [Event(
            14,
            Keywords = Keywords.ErrorFailure,
            Message = "[msg=Log Error];[msg={0}]",
            Level = EventLevel.Error)]
        public void LogError(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                14, 
                msg ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            15,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Type '{0}' was not found. Type loading was skipped. Monitoring will continue.",
            Level = EventLevel.Error)]
        public void TypeWasNotFoundConfigurationError(string type, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                15,
                type ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            16,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Type '{0}' does not implement '{1}'. Type loading was skipped. Monitoring will continue.",
            Level = EventLevel.Error)]
        public void IncorrectTypeConfigurationError(string type, string expectedType, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                16,
                type ?? string.Empty,
                expectedType ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            17,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Type '{0}' does not have property '{1}'. Property initialization was skipped. Monitoring will continue.",
            Level = EventLevel.Error)]
        public void IncorrectPropertyConfigurationError(string type, string property, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                17,
                type ?? string.Empty,
                property ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            18,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Element '{0}' element does not have a Type attribute, does not specify a value and is not a valid collection type. Type initialization was skipped. Monitoring will continue.",
            Level = EventLevel.Error)]
        public void IncorrectInstanceAtributesConfigurationError(string definition, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                18,
                definition ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            19,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. '{0}' element has unexpected contents: '{1}': '{2}'. Type initialization was skipped. Monitoring will continue.",
            Level = EventLevel.Error)]
        public void LoadInstanceFromValueConfigurationError(string element, string contents, string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                19,
                element ?? string.Empty,
                contents ?? string.Empty,
                error ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            20,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Exception: '{0}'. Monitoring will continue if you set InstrumentationKey programmatically.",
            Level = EventLevel.Error)]
        public void ConfigurationFileCouldNotBeParsedError(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                20,
                error ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            21,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Type '{0}' will not be create. Error: '{1}'. Monitoring will continue if you set InstrumentationKey programmatically.",
            Level = EventLevel.Error)]
        public void MissingMethodExceptionConfigurationError(string type, string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                21,
                type ?? string.Empty,
                error ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            22,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Type '{0}' will not be initialized. Error: '{1}'. Monitoring will continue if you set InstrumentationKey programmatically.",
            Level = EventLevel.Error)]
        public void ComponentInitializationConfigurationError(string type, string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                22,
                type ?? string.Empty,
                error ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            23,
            Message = "ApplicationInsights configuration file '{0}' was not found.",
            Level = EventLevel.Warning)]
        public void ApplicationInsightsConfigNotFoundWarning(string file, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                23,
                file ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            24,
            Message = "Failed to send: {0}.",
            Level = EventLevel.Warning)]
        public void FailedToSend(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                24,
                msg ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
           25,
           Message = "Exception happened during getting the machine name: '{0}'.",
           Level = EventLevel.Error)]
        public void FailedToGetMachineName(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                25,
                error ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            26,
            Message = "Failed to flush aggregated metrics. Exception: {0}.",
            Level = EventLevel.Error)]
        public void FailedToFlushMetricAggregators(string ex, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                26,
                ex ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            27,
            Message = "Failed to snapshot aggregated metrics. Exception: {0}.",
            Level = EventLevel.Error)]
        public void FailedToSnapshotMetricAggregators(string ex, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                27,
                ex ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            28,
            Message = "Failed to invoke metric processor '{0}'. If the issue persists, remove the processor. Exception: {1}.",
            Level = EventLevel.Error)]
        public void FailedToRunMetricProcessor(string processorName, string ex, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                28,
                processorName ?? string.Empty,
                ex ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            29,
            Message = "The backlog of unsent items has reached maximum size of {0}. Items will be dropped until the backlog is cleared.",
            Level = EventLevel.Error)]
        public void ItemDroppedAsMaximumUnsentBacklogSizeReached(int maxBacklogSize, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                29,
                maxBacklogSize,               
                this.nameProvider.Name);
        }

        [Event(
            30,
            Message = "Flush was called on the telemetry channel (InMemoryChannel) after it was disposed.",
            Level = EventLevel.Warning)]
        public void InMemoryChannelFlushedAfterBeingDisposed(string appDomainName = "Incorrect")
        {
            this.WriteEvent(30, this.nameProvider.Name);
        }

        [Event(
            31,
            Message = "Send was called on the telemetry channel (InMemoryChannel) after it was disposed, the telemetry data was dropped.",
            Level = EventLevel.Warning)]
        public void InMemoryChannelSendCalledAfterBeingDisposed(string appDomainName = "Incorrect")
        {
            this.WriteEvent(31, this.nameProvider.Name);
        }

        [Event(
            32,
            Message = "Failed to get environment variables due to security exception; code is likely running in partial trust. Exception: {0}.",
            Level = EventLevel.Warning)]
        public void FailedToLoadEnvironmentVariables(string ex, string appDomainName = "Incorrect")
        {
            this.WriteEvent(32, ex, this.nameProvider.Name);
        }

        // Verbosity is Error - so it is always sent to portal; Keyword is Diagnostics so throttling is not applied.
        [Event(33,
            Message = "A Metric Extractor detected a telemetry item with SamplingPercentage < 100. Metrics Extractors should be used before Sampling Processors or any other Telemetry Processors that might filter out Telemetry Items. Otherwise, extracted metrics may be incorrect.",
            Level = EventLevel.Error,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable)]
        public void MetricExtractorAfterSamplingError(string appDomainName = "Incorrect")
        {
            this.WriteEvent(33, this.nameProvider.Name);
        }

        // Verbosity is Verbose - targeted at support personnel; Keyword is Diagnostics so throttling is not applied.
        [Event(34,
            Message = "A Metric Extractor detected a telemetry item with SamplingPercentage < 100. Metrics Extractors Extractor should be used before Sampling Processors or any other Telemetry Processors that might filter out Telemetry Items. Otherwise, extracted metrics may be incorrect.",
            Level = EventLevel.Verbose,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable)]
        public void MetricExtractorAfterSamplingVerbose(string appDomainName = "Incorrect")
        {
            this.WriteEvent(34, this.nameProvider.Name);
        }

        [Event(35, Message = "Item was rejected because it has no instrumentation key set. Item: {0}", Level = EventLevel.Verbose)]
        public void ItemRejectedNoInstrumentationKey(string item, string appDomainName = "Incorrect")
        {
            this.WriteEvent(35, item ?? string.Empty, this.nameProvider.Name);
        }

        [Event(
            36,
            Message = "Failed to obtain a value for default heartbeat payload property '{0}': Exception {1}.",
            Level = EventLevel.Warning)]
        public void FailedToObtainDefaultHeartbeatProperty(string heartbeatProperty, string ex, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                36,
                heartbeatProperty ?? string.Empty,
                ex ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            37,
            Message = "Could not add heartbeat payload property '{0}' = {1}. Exception: {2}.",
            Level = EventLevel.Warning)]
        public void FailedToAddHeartbeatProperty(string heartbeatProperty, string heartbeatPropertyValue, string ex = null, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                37,
                heartbeatProperty ?? string.Empty,
                heartbeatPropertyValue ?? string.Empty,
                ex ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            38,
            Message = "Cannot add heartbeat payload property without any property name. Value given was '{0}', isHealthy given was {1}.",
            Level = EventLevel.Warning)]
        public void HeartbeatPropertyAddedWithoutAnyName(string heartbeatPropertyValue, bool isHealthy, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                38,
                heartbeatPropertyValue ?? string.Empty,
                isHealthy,
                this.nameProvider.Name);
        }

        [Event(
            39,
            Message = "Could not set heartbeat payload property '{0}' = {1}, isHealthy was set = {2}, isHealthy value = {3}. Exception: {4}.",
            Level = EventLevel.Warning)]
        public void FailedToSetHeartbeatProperty(string heartbeatProperty, string heartbeatPropertyValue, bool isHealthyHasValue, bool isHealthy, string ex = null, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                39,
                heartbeatProperty ?? string.Empty,
                heartbeatPropertyValue ?? string.Empty,
                isHealthyHasValue,
                isHealthy,
                ex ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            40,
            Message = "Cannot set heartbeat payload property without a propertyName, or cannot set one of the default SDK properties. Property name given:'{0}'. Property value: '{1}'. isHealthy was set = {2}, isHealthy = {3}.",
            Level = EventLevel.Warning)]
        public void CannotSetHeartbeatPropertyWithNoNameOrDefaultName(string heartbeatProperty, string heartbeatPropertyValue, bool isHealthyHasValue, bool isHealthy, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                40,
                heartbeatProperty ?? string.Empty,
                heartbeatPropertyValue ?? string.Empty,
                isHealthyHasValue,
                isHealthy,
                this.nameProvider.Name);
        }

        [Event(
            41,
            Keywords = Keywords.UserActionable,
            Message = "Failed to retrieve Application Id for the current application insights resource. Make sure the configured instrumentation key is valid. Error: {0}",
            Level = EventLevel.Warning)]
        public void ApplicationIdProviderFetchApplicationIdFailed(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(41, exception, this.nameProvider.Name);
        }

        [Event(
            42,
            Keywords = Keywords.UserActionable,
            Message = "Failed to retrieve Application Id for the current application insights resource. Endpoint returned HttpStatusCode: {0}",
            Level = EventLevel.Warning)]
        public void ApplicationIdProviderFetchApplicationIdFailedWithResponseCode(string httpStatusCode, string appDomainName = "Incorrect")
        {
            this.WriteEvent(42, httpStatusCode, this.nameProvider.Name);
        }

        [Event(
            43,
            Keywords = Keywords.UserActionable,
            Message = "Process was called on the TelemetrySink after it was disposed, the telemetry data was dropped.",
            Level = EventLevel.Error)]
        public void TelemetrySinkCalledAfterBeingDisposed(string appDomainName = "Incorrect")
        {
            this.WriteEvent(43, this.nameProvider.Name);
        }

        /// <summary>
        /// Logs the details when there operation to stop does not match the current operation.
        /// </summary>
        [Event(44, Message = "Operation to stop does not match the current operation. Details {0}.", Level = EventLevel.Warning)]
        public void InvalidOperationToStopDetails(string details, string appDomainName = "Incorrect")
        {
            this.WriteEvent(44, details, this.nameProvider.Name);
        }

        [Event(
        45,
        Message = "File system containing ApplicationInsights configuration file is inaccessible.",
        Level = EventLevel.Warning)]
        public void ApplicationInsightsConfigNotAccessibleWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                45,
                this.nameProvider.Name);
        }

        [Event(46,
            Message = "Initialization is skipped for the sampled item.",
            Level = EventLevel.Informational)]
        public void InitializationIsSkippedForSampledItem(string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                46,
                this.nameProvider.Name);
        }

        [Event(47, Message = "Connection String exceeds max length of {0} characters.", Level = EventLevel.Error, Keywords = Keywords.UserActionable)]
        public void ConnectionStringExceedsMaxLength(int maxLength, string appDomainName = "Incorrect") => this.WriteEvent(47, maxLength, this.nameProvider.Name);

        [Event(48, Message = "Connection String cannot be empty.", Level = EventLevel.Error, Keywords = Keywords.UserActionable)]
        public void ConnectionStringEmpty(string appDomainName = "Incorrect") => this.WriteEvent(48, this.nameProvider.Name);

        [Event(49, Message = "Connection String cannot contain duplicate keys.", Level = EventLevel.Error, Keywords = Keywords.UserActionable)]
        public void ConnectionStringDuplicateKey(string appDomainName = "Incorrect") => this.WriteEvent(49, this.nameProvider.Name);

        [Event(50, Message = "Connection String contains invalid delimiters and cannot be parsed.", Level = EventLevel.Error, Keywords = Keywords.UserActionable)]
        public void ConnectionStringInvalidDelimiters(string appDomainName = "Incorrect") => this.WriteEvent(50, this.nameProvider.Name);
        
        [Event(51, Message = "Connection String cannot be NULL.", Level = EventLevel.Error, Keywords = Keywords.UserActionable)]
        public void ConnectionStringNull(string appDomainName = "Incorrect") => this.WriteEvent(51, this.nameProvider.Name);

        [Event(52, Message = "Connection String could not create an endpoint. {0}.", Level = EventLevel.Error, Keywords = Keywords.UserActionable)]
        public void ConnectionStringInvalidEndpoint(string exceptionMessage, string appDomainName = "Incorrect") => this.WriteEvent(52, exceptionMessage, this.nameProvider.Name);

        [Event(53, Message = "Connection String could not be set. Exception: {0}", Level = EventLevel.Error, Keywords = Keywords.UserActionable)]
        public void ConnectionStringSetFailed(string exceptionMessage, string appDomainName = "Incorrect") => this.WriteEvent(53, exceptionMessage, this.nameProvider.Name);

        [Event(54, Message = "TelemetryConfigurationFactory overwrote the ConnectionString with a value from an Environment Variable: {0}", Level = EventLevel.Informational)]
        public void TelemetryConfigurationFactoryFoundConnectionStringEnvironmentVariable(string variableName, string appDomainName = "Incorrect") => this.WriteEvent(54, variableName, this.nameProvider.Name);

        [Event(55, Message = "TelemetryConfigurationFactory overwrote the InstrumentationKey with a value from an Environment Variable: {0}", Level = EventLevel.Informational)]
        public void TelemetryConfigurationFactoryFoundInstrumentationKeyEnvironmentVariable(string variableName, string appDomainName = "Incorrect") => this.WriteEvent(55, variableName, this.nameProvider.Name);

        [Event(56, Message = "TelemetryConfigurationFactory did not find an InstrumentationKey in your config file. This needs to be set in either your config file or at application startup.", Level = EventLevel.Warning, Keywords = Keywords.UserActionable)]
        public void TelemetryConfigurationFactoryNoInstrumentationKey(string appDomainName = "Incorrect") => this.WriteEvent(56, this.nameProvider.Name);

        [Event(57, Message = "TelemetryChannel found a telemetry item without an InstrumentationKey. This is a required field and must be set in either your config file or at application startup.", Level = EventLevel.Error, Keywords = Keywords.UserActionable)]
        public void TelemetryChannelNoInstrumentationKey(string appDomainName = "Incorrect") => this.WriteEvent(57, this.nameProvider.Name);

        [Event(58, Message = "TelemetryClient.Flush was invoked.", Level = EventLevel.Verbose)]
        public void TelemetlyClientFlush(string appDomainName = "Incorrect") => this.WriteEvent(58, this.nameProvider.Name);

        [Event(59, Message = "MetricManager.Flush was invoked.", Level = EventLevel.Verbose)]
        public void MetricManagerFlush(string appDomainName = "Incorrect") => this.WriteEvent(59, this.nameProvider.Name);

        [Event(60, Message = "MetricManager created {0} Tasks.", Level = EventLevel.Verbose)]
        public void MetricManagerCreatedTasks(int taskCount, string appDomainName = "Incorrect") => this.WriteEvent(60, taskCount, this.nameProvider.Name);

        #region FileDiagnosticsTelemetryModule

        [Event(61, Message = "Logs file name: {0}.", Level = EventLevel.Verbose)]
        public void LogsFileName(string fileName, string appDomainName = "Incorrect") => this.WriteEvent(61, fileName ?? string.Empty, this.nameProvider.Name);

        [Event(62, Keywords = Keywords.UserActionable, Message = "Access to the logs folder was denied (User: {1}). Error message: {0}.", Level = EventLevel.Error)]
        public void LogStorageAccessDeniedError(string error, string user, string appDomainName = "Incorrect") => this.WriteEvent(62, error ?? string.Empty, user ?? string.Empty, this.nameProvider.Name);

        /*
        [Event(63, Message = "Trying to load http module type from assembly: {0}, type name: {1}.", Level = EventLevel.Verbose)]
        public void HttpModuleLoadingStart(string assemblyName, string moduleName, string appDomainName = "Incorrect") => this.WriteEvent(63, assemblyName ?? string.Empty, moduleName ?? string.Empty, this.nameProvider.Name);

        [Event(64, Message = "Http module type from assembly: {0}, type name: {1} loaded successfully", Level = EventLevel.Verbose)]
        public void HttpModuleLoadingEnd(string assemblyName, string moduleName, string appDomainName = "Incorrect") => this.WriteEvent(64, assemblyName ?? string.Empty, moduleName ?? string.Empty, this.nameProvider.Name);

        [Event(65, Keywords = Keywords.UserActionable, Message = "Error loading http module type from assembly {0}, type name {1}, exception: {2}.", Level = EventLevel.Error)]
        public void HttpModuleLoadingError(string assemblyName, string moduleName, string exception, string appDomainName = "Incorrect") => this.WriteEvent(65, assemblyName ?? string.Empty, moduleName ?? string.Empty, exception ?? string.Empty, this.nameProvider.Name);
        */

        [Event(66, Message = "Call to WindowsIdentity.Current failed with the exception: {0}.", Level = EventLevel.Warning)]
        public void LogWindowsIdentityAccessSecurityException(string error, string appDomainName = "Incorrect") => this.WriteEvent(66, error ?? string.Empty, this.nameProvider.Name);

        #endregion

        [Event(67, Message = "Backend has responded with {0} status code in {1}ms.", Level = EventLevel.Informational)]
        public void IngestionResponseTime(int responseCode, float responseDurationInMs, string appDomainName = "Incorrect") => this.WriteEvent(67, responseCode, responseDurationInMs, this.nameProvider.Name);

        [Event(68, Message = "{0}", Level = EventLevel.Warning, Keywords = Keywords.UserActionable)]
        public void ConfigurationStringParseWarning(string message, string appDomainName = "Incorrect") => this.WriteEvent(68, message, this.nameProvider.Name);

        [Event(69, Message = "{0}", Level = EventLevel.Error, Keywords = Keywords.UserActionable)]
        public void ConnectionStringParseError(string message, string appDomainName = "Incorrect") => this.WriteEvent(69, message, this.nameProvider.Name);

        [NonEvent]
        [SuppressMessage("Microsoft.Performance", "CA1822: MarkMembersAsStatic", Justification = "This method does access instance data in NetStandard 2.0 scenarios.")]
        public void IngestionResponseTimeEventCounter(float responseDurationInMs)
        {
#if NETSTANDARD2_0
            this.IngestionResponseTimeCounter.WriteMetric(responseDurationInMs);
#endif
        }

        [Event(70, Message = "Updating Exception has failed. Error: {0}", Level = EventLevel.Error)]
        public void UpdateDataFailed(string error, string appDomainName = "Incorrect") => this.WriteEvent(70, error, this.nameProvider.Name);

        [Event(71, Keywords = Keywords.UserActionable, Message = "TransmissionStatusEvent has failed. Error: {0}. Monitoring will continue.", Level = EventLevel.Error)]
        public void TransmissionStatusEventError(string error, string appDomainName = "Incorrect") => this.WriteEvent(71, error, this.nameProvider.Name);

        [Event(72, Keywords = Keywords.UserActionable, Message = "Failed to create file for self diagnostics at {0}. Error message: {1}.", Level = EventLevel.Error)]
        public void SelfDiagnosticsFileCreateException(string logDirectory, string exception, string appDomainName = "Incorrect") => this.WriteEvent(72, logDirectory, exception, this.nameProvider.Name);

        [Event(73, Message = "Failed to get AAD Token. Error message: {0}.", Level = EventLevel.Error)]
        public void FailedToGetToken(string exception, string appDomainName = "Incorrect") => this.WriteEvent(73, exception, this.nameProvider.Name);

        [Event(74, Message = "Ingestion Service responded with redirect. {0}", Level = EventLevel.Informational)]
        public void IngestionRedirectInformation(string message, string appDomainName = "Incorrect") => this.WriteEvent(74, message, this.nameProvider.Name);

        [Event(75, Message = "Ingestion Service responded with redirect. {0}", Level = EventLevel.Error)]
        public void IngestionRedirectError(string message, string appDomainName = "Incorrect") => this.WriteEvent(75, message, this.nameProvider.Name);

        [Event(76, Message = "MetricValueBuffer exceeded spin count.", Level = EventLevel.Warning)]
        public void MetricValueBufferExceededSpinCount(string appDomainName = "Incorrect") => this.WriteEvent(76, this.nameProvider.Name);

        [NonEvent]
        public void TransmissionStatusEventFailed(Exception ex)
        {
            if (this.IsEnabled(EventLevel.Error, (EventKeywords)(-1)))
            {
                this.TransmissionStatusEventError(ex.ToInvariantString());
            }
        }

        /// <summary>
        /// Keywords for the PlatformEventSource.
        /// </summary>
        public sealed class Keywords
        {
            /// <summary>
            /// Key word for user actionable events.
            /// </summary>
            public const EventKeywords UserActionable = (EventKeywords)EventSourceKeywords.UserActionable;

            /// <summary>
            /// Keyword for errors that trace at Verbose level.
            /// </summary>
            public const EventKeywords Diagnostics = (EventKeywords)EventSourceKeywords.Diagnostics;

            /// <summary>
            /// Keyword for errors that trace at Verbose level.
            /// </summary>
            public const EventKeywords VerboseFailure = (EventKeywords)EventSourceKeywords.VerboseFailure;

            /// <summary>
            /// Keyword for errors that trace at Error level.
            /// </summary>
            public const EventKeywords ErrorFailure = (EventKeywords)EventSourceKeywords.ErrorFailure;
        }
    }
}