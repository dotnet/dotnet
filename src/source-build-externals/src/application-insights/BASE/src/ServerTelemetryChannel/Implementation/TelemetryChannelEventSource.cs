namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;

#if REDFIELD
    [EventSource(Name = "Redfield-Microsoft-ApplicationInsights-WindowsServer-TelemetryChannel")]
#else
    [EventSource(Name = "Microsoft-ApplicationInsights-WindowsServer-TelemetryChannel")]
#endif
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "appDomainName is required")]
    internal sealed class TelemetryChannelEventSource : EventSource
    {
        public static readonly TelemetryChannelEventSource Log = new TelemetryChannelEventSource();
        public readonly string ApplicationName;

        private TelemetryChannelEventSource()
        {
            this.ApplicationName = GetApplicationName();
        }

        public static bool IsVerboseEnabled
        {
            [NonEvent]
            get
            {
                return Log.IsEnabled(EventLevel.Verbose, (EventKeywords)(-1));
            }
        }

        // Verbosity is Error - so it is always sent to portal; Keyword is Diagnostics so throttling is not applied.
        [Event(1,
            Message = "Diagnostic message: backoff logic disabled, transmission will be resolved.",
            Level = EventLevel.Error,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable)]
        public void BackoffDisabled(string appDomainName = "Incorrect")
        {
            this.WriteEvent(1, this.ApplicationName);
        }

        // Verbosity is Error - so it is always sent to portal; Keyword is Diagnostics so throttling is not applied.
        [Event(2,
            Message = "Diagnostic message: backoff logic was enabled. Backoff internal exceeded {0} min. Last status code received from the backend was {1}.",
            Level = EventLevel.Error,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable)]
        public void BackoffEnabled(double intervalInMin, int statusCode, string appDomainName = "Incorrect")
        {
            this.WriteEvent(2, intervalInMin, statusCode, this.ApplicationName);
        }

        [Event(3, Message = "Sampling skipped: {0}.", Level = EventLevel.Verbose)]
        public void SamplingSkippedByType(string telemetryType, string appDomainName = "Incorrect")
        {
            this.WriteEvent(3, telemetryType ?? string.Empty, this.ApplicationName);
        }

        [Event(4, Message = "Backoff interval in seconds {0}.", Level = EventLevel.Verbose)]
        public void BackoffInterval(double intervalInSec, string appDomainName = "Incorrect")
        {
            this.WriteEvent(4, intervalInSec, this.ApplicationName);
        }

        [Event(5, Message = "Backend response {1} was not parsed. Some items may be dropped: {0}.", Level = EventLevel.Warning)]
        public void BreezeResponseWasNotParsedWarning(string exception, string response, string appDomainName = "Incorrect")
        {
            this.WriteEvent(5, exception ?? string.Empty, response ?? string.Empty, this.ApplicationName);
        }

        [Event(6, Message = "Unexpected backend response. Items # in batch {0} <= Error index in response: {1}.", Level = EventLevel.Warning)]
        public void UnexpectedBreezeResponseWarning(int size, int index, string appDomainName = "Incorrect")
        {
            this.WriteEvent(6, size, index, this.ApplicationName);
        }

        [Event(7, Message = "Item was rejected by endpoint. Message: {0}", Level = EventLevel.Warning)]
        public void ItemRejectedByEndpointWarning(string message, string appDomainName = "Incorrect")
        {
            this.WriteEvent(7, message ?? string.Empty, this.ApplicationName);
        }

        [Event(8, Keywords = Keywords.UserActionable, Message = "User-defined sampling callback failed. Exception: {0}.", Level = EventLevel.Error)]
        public void SamplingCallbackError(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(8, exception, this.ApplicationName);
        }

        [Event(9, Message = "Sampling changed to {0}.", Level = EventLevel.Verbose)]
        public void SamplingChanged(double interval, string appDomainName = "Incorrect")
        {
            this.WriteEvent(9, interval, this.ApplicationName);
        }

        [Event(10, Message = "Sampled out: {0}.", Level = EventLevel.Verbose)]
        public void ItemSampledOut(string telemetryType, string appDomainName = "Incorrect")
        {
            this.WriteEvent(10, telemetryType ?? string.Empty, this.ApplicationName);
        }

        [Event(11, Message = "Storage folder: {0} successfully validated.", Level = EventLevel.Informational)]
        public void StorageFolder(string folder, string appDomainName = "Incorrect")
        {
            this.WriteEvent(11, folder ?? string.Empty, this.ApplicationName);
        }

        [Event(12, Message = "BufferEnqueued. TransmissionId: {0}. TransmissionCount: {1}.", Level = EventLevel.Verbose)]
        public void BufferEnqueued(string transmissionId, int transmissionCount, string appDomainName = "Incorrect")
        {
            this.WriteEvent(12, transmissionId ?? string.Empty, transmissionCount, this.ApplicationName);
        }

        [Event(13, Message = "BufferEnqueueNoCapacity. Size: {0}. Capacity: {1}.", Level = EventLevel.Warning)]
        public void BufferEnqueueNoCapacityWarning(long size, int capacity, string appDomainName = "Incorrect")
        {
            this.WriteEvent(13, size, capacity, this.ApplicationName);
        }

        [Event(14, Message = "UnauthorizedAccessExceptionOnTransmissionSave. TransmissionId: {0}. Message: {1}.", Level = EventLevel.Warning)]
        public void UnauthorizedAccessExceptionOnTransmissionSaveWarning(string transmissionId, string message, string appDomainName = "Incorrect")
        {
            this.WriteEvent(14, transmissionId ?? string.Empty, message ?? string.Empty, this.ApplicationName);
        }

        [Event(15, Message = "StorageSize. StorageSize: {0}.", Level = EventLevel.Verbose)]
        public void StorageSize(long size, string appDomainName = "Incorrect")
        {
            this.WriteEvent(15, size, this.ApplicationName);
        }

        [Event(16, Message = "SenderEnqueueNoCapacity. TransmissionCount: {0}. Capacity: {1}.", Level = EventLevel.Warning)]
        public void SenderEnqueueNoCapacityWarning(int transmissionCount, int capacity, string appDomainName = "Incorrect")
        {
            this.WriteEvent(16, transmissionCount, capacity, this.ApplicationName);
        }

        [Event(17, Message = "TransmissionSendStarted. TransmissionId: {0}.", Level = EventLevel.Verbose)]
        public void TransmissionSendStarted(string id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(17, id ?? string.Empty, this.ApplicationName);
        }

        [Event(18, Message = "SerializationStarted. Items count: {0}", Level = EventLevel.Verbose)]
        public void SerializationStarted(int count, string appDomainName = "Incorrect")
        {
            this.WriteEvent(18, count, this.ApplicationName);
        }

        [Event(19, Message = "Transmitter flushed telemetry events.", Level = EventLevel.Verbose)]
        public void TelemetryChannelFlush(string appDomainName = "Incorrect")
        {
            this.WriteEvent(19, this.ApplicationName);
        }

        [Event(20, Message = "{0} passed to channel with iKey {1}...", Level = EventLevel.Verbose)]
        public void TelemetryChannelSend(string type, string key, string appDomainName = "Incorrect")
        {
            this.WriteEvent(20, type, key, this.ApplicationName);
        }

        [Event(21, Message = "TransmitterEnqueue. TransmissionId: {0}.", Level = EventLevel.Verbose)]
        public void TransmitterEnqueue(string transmissionId, string appDomainName = "Incorrect")
        {
            this.WriteEvent(21, transmissionId ?? string.Empty, this.ApplicationName);
        }

        [Event(22, Message = "TransmissionSent. TransmissionId: {0}. Capacity: {1}.", Level = EventLevel.Verbose)]
        public void TransmissionSentSuccessfully(string transmissionId, int capacity, string appDomainName = "Incorrect")
        {
            this.WriteEvent(22, transmissionId ?? string.Empty, capacity, this.ApplicationName);
        }

        [Event(23, Message = "TransmissionSendingFailed. TransmissionId: {0}. Message: {1}. StatusCode: {2}. Description: {3}.", Level = EventLevel.Warning)]
        public void TransmissionSendingFailedWebExceptionWarning(string transmissionId, string exceptionMessage, int statusCode, string description, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                23,
                transmissionId ?? string.Empty,
                exceptionMessage ?? string.Empty,
                statusCode,
                description ?? string.Empty,
                this.ApplicationName);
        }

        [Event(24, Message = "Transmission policy failed with parsing Retry-After http header: '{0}'", Level = EventLevel.Warning)]
        public void TransmissionPolicyRetryAfterParseFailedWarning(string retryAfterHeader, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                24,
                retryAfterHeader ?? string.Empty,
                this.ApplicationName);
        }

        [Event(25, Message = "StorageEnqueueNoCapacity. Size: {0}. Capacity: {1}.", Level = EventLevel.Warning)]
        public void StorageEnqueueNoCapacityWarning(long size, long capacity, string appDomainName = "Incorrect")
        {
            this.WriteEvent(25, size, capacity, this.ApplicationName);
        }

        [Event(26, Message = "TransmissionSavedToStorage. TransmissionId: {0}.", Level = EventLevel.Verbose)]
        public void TransmissionSavedToStorage(string transmissionId, string appDomainName = "Incorrect")
        {
            this.WriteEvent(26, transmissionId ?? string.Empty, this.ApplicationName);
        }

        [Event(27, Message = "{0} changed sender capacity to {1}", Level = EventLevel.Verbose)]
        public void SenderCapacityChanged(string policyName, int newCapacity, string appDomainName = "Incorrect")
        {
            this.WriteEvent(27, policyName ?? string.Empty, newCapacity, this.ApplicationName);
        }

        [Event(28, Message = "{0} changed buffer capacity to {1}", Level = EventLevel.Verbose)]
        public void BufferCapacityChanged(string policyName, int newCapacity, string appDomainName = "Incorrect")
        {
            this.WriteEvent(28, policyName ?? string.Empty, newCapacity, this.ApplicationName);
        }

        [Event(29, Message = "SenderCapacityReset: {0}", Level = EventLevel.Verbose)]
        public void SenderCapacityReset(string policyName, string appDomainName = "Incorrect")
        {
            this.WriteEvent(29, policyName ?? string.Empty, this.ApplicationName);
        }

        [Event(30, Message = "BufferCapacityReset: {0}", Level = EventLevel.Verbose)]
        public void BufferCapacityReset(string policyName, string appDomainName = "Incorrect")
        {
            this.WriteEvent(30, policyName ?? string.Empty, this.ApplicationName);
        }

        [Event(31, Message = "BackoffTimeSetInSeconds: {0}", Level = EventLevel.Verbose)]
        public void BackoffTimeSetInSeconds(double seconds, string appDomainName = "Incorrect")
        {
            this.WriteEvent(31, seconds, this.ApplicationName);
        }

        [Event(32, Message = "NetworkIsNotAvailable", Level = EventLevel.Warning)]
        public void NetworkIsNotAvailableWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(32, this.ApplicationName);
        }

        [Event(33, Message = "StorageCapacityReset: {0}", Level = EventLevel.Verbose)]
        public void StorageCapacityReset(string policyName, string appDomainName = "Incorrect")
        {
            this.WriteEvent(33, policyName ?? string.Empty, this.ApplicationName);
        }

        [Event(34, Message = "{0} changed storage capacity to {1}", Level = EventLevel.Verbose)]
        public void StorageCapacityChanged(string policyName, int newCapacity, string appDomainName = "Incorrect")
        {
            this.WriteEvent(34, policyName ?? string.Empty, newCapacity, this.ApplicationName);
        }

        [Event(35, Message = "ThrottlingRetryAfterParsedInSec: {0}", Level = EventLevel.Verbose)]
        public void ThrottlingRetryAfterParsedInSec(double retryAfter, string appDomainName = "Incorrect")
        {
            this.WriteEvent(35, retryAfter, this.ApplicationName);
        }

        [Event(36, Message = "TransmitterEmptyStorage", Level = EventLevel.Verbose)]
        public void TransmitterEmptyStorage(string appDomainName = "Incorrect")
        {
            this.WriteEvent(36, this.ApplicationName);
        }

        [Event(37, Message = "TransmitterEmptyBuffer", Level = EventLevel.Verbose)]
        public void TransmitterEmptyBuffer(string appDomainName = "Incorrect")
        {
            this.WriteEvent(37, this.ApplicationName);
        }

        [Event(38, Message = "SubscribeToNetworkFailure: {0}", Level = EventLevel.Warning)]
        public void SubscribeToNetworkFailureWarning(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(38, exception ?? string.Empty, this.ApplicationName);
        }

        [Event(39, Message = "SubscribeToNetworkFailure: {0}", Level = EventLevel.Warning)]
        public void ExceptionHandlerStartExceptionWarning(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(39, exception ?? string.Empty, this.ApplicationName);
        }

        [Event(40, Message = "TransmitterSenderSkipped. TransmissionId: {0}.", Level = EventLevel.Verbose)]
        public void TransmitterSenderSkipped(string transmissionId, string appDomainName = "Incorrect")
        {
            this.WriteEvent(40, transmissionId ?? string.Empty, this.ApplicationName);
        }

        [Event(41, Message = "TransmitterBufferSkipped. TransmissionId: {0}.", Level = EventLevel.Verbose)]
        public void TransmitterBufferSkipped(string transmissionId, string appDomainName = "Incorrect")
        {
            this.WriteEvent(41, transmissionId, this.ApplicationName);
        }

        [Event(42, Message = "TransmitterStorageSkipped. TransmissionId: {0}.", Level = EventLevel.Verbose)]
        public void TransmitterStorageSkipped(string transmissionId, string appDomainName = "Incorrect")
        {
            this.WriteEvent(42, transmissionId ?? string.Empty, this.ApplicationName);
        }

        [Event(43, Message = "IncorrectFileFormat. Error: {0}.", Level = EventLevel.Warning)]
        public void IncorrectFileFormatWarning(string errorMessage, string appDomainName = "Incorrect")
        {
            this.WriteEvent(43, errorMessage ?? string.Empty, this.ApplicationName);
        }

        [Event(44, Message = "Unexpected exception when handling IApplicationLifecycle.Stopping event:{0}", Level = EventLevel.Error)]
        public void UnexpectedExceptionInStopError(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(44, exception ?? string.Empty, this.ApplicationName);
        }

        [Event(45, Message = "Transmission polices failed to execute. Exception:{0}", Level = EventLevel.Error)]
        public void ApplyPoliciesError(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(45, exception ?? string.Empty, this.ApplicationName);
        }

        [Event(46, Message = "Retry-After http header: '{0}'. Transmission will be stopped.", Level = EventLevel.Warning)]
        public void RetryAfterHeaderIsPresent(string retryAfterHeader, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                46,
                retryAfterHeader ?? string.Empty,
                this.ApplicationName);
        }

        [Event(48, Message = "TransmissionFailedToStoreWarning. TransmissionId: {0}. Exception: {1}.", Level = EventLevel.Warning)]
        public void TransmissionFailedToStoreWarning(string transmissionId, string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(48, transmissionId ?? string.Empty, exception ?? string.Empty, this.ApplicationName);
        }

        [Event(49, Message = "MovedFromBufferToSender.", Level = EventLevel.Verbose)]
        public void MovedFromBufferToSender(string appDomainName = "Incorrect")
        {
            this.WriteEvent(49, this.ApplicationName);
        }

        [Event(50, Message = "MovedFromStorageToSender.", Level = EventLevel.Verbose)]
        public void MovedFromStorageToSender(string appDomainName = "Incorrect")
        {
            this.WriteEvent(50, this.ApplicationName);
        }

        [Event(51, Message = "MovedFromStorageToBuffer.", Level = EventLevel.Verbose)]
        public void MovedFromStorageToBuffer(string appDomainName = "Incorrect")
        {
            this.WriteEvent(51, this.ApplicationName);
        }

        [Event(52, Message = "MovedFromBufferToStorage.", Level = EventLevel.Verbose)]
        public void MovedFromBufferToStorage(string appDomainName = "Incorrect")
        {
            this.WriteEvent(52, this.ApplicationName);
        }

        [Event(53, Message = "UnauthorizedAccessExceptionOnCalculateSizeWarning. Message: {0}.", Level = EventLevel.Warning)]
        public void UnauthorizedAccessExceptionOnCalculateSizeWarning(string message, string appDomainName = "Incorrect")
        {
            this.WriteEvent(53, message ?? string.Empty, this.ApplicationName);
        }

        [Event(54, Message = "TransmissionSendingFailed. TransmissionId: {0}. Message: {1}.", Level = EventLevel.Warning)]
        public void TransmissionSendingFailedWarning(string transmissionId, string exceptionMessage, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                54,
                transmissionId ?? string.Empty,
                exceptionMessage ?? string.Empty,
                this.ApplicationName);
        }

        [Event(
            55,
            Keywords = Keywords.UserActionable,
            Message = "Local storage access has resulted in an error (User: {1}) (CustomFolder: {2}). If you want Application Insights SDK to store telemetry locally on disk in case of transient network issues please give the process access to %LOCALAPPDATA% or %TEMP% folder. If application is running in non-windows platform, create StorageFolder yourself, and set ServerTelemetryChannel.StorageFolder to the custom folder name. After you gave access to the folder you need to restart the process. Currently monitoring will continue but if telemetry cannot be sent it will be dropped. Error message: {0}.",
            Level = EventLevel.Error)]
        public void TransmissionStorageAccessDeniedError(string error, string user, string customFolder, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                55,
                error ?? string.Empty,
                user ?? string.Empty,
                customFolder ?? string.Empty,
                this.ApplicationName);
        }

        [Event(
            56,
            Message = "Local storage access has resulted in an error. Error Info: {0}. User: {1}.",
            Level = EventLevel.Warning)]
        public void TransmissionStorageIssuesWarning(string error, string user, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                56,
                error ?? string.Empty,
                user ?? string.Empty,
                this.ApplicationName);
        }

        [Event(
            57,
            Keywords = Keywords.UserActionable,
            Message = "Server telemetry channel was not initialized. So persistent storage is turned off. You need to call ServerTelemetryChannel.Initialize(). Currently monitoring will continue but if telemetry cannot be sent it will be dropped.",
            Level = EventLevel.Error)]
        public void StorageNotInitializedError(string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                57,
                this.ApplicationName);
        }

        [Event(58, Message = "Telemetry item is going to storage. Last backend status code: {0}. Current delay in sec: {1}.", Level = EventLevel.Verbose)]
        public void LastBackendResponseWhenPutToStorage(int statusCode, double currentDelayInSeconds, string appDomainName = "Incorrect")
        {
            this.WriteEvent(58, statusCode, currentDelayInSeconds, this.ApplicationName);
        }

        [Event(59, Message = "Error dequeuing file: {0}. Exception: {1}.", Level = EventLevel.Warning)]
        public void TransmissionStorageDequeueIOError(string fileName, string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(59, fileName, exception, this.ApplicationName);
        }

        [Event(60, Message = "Unauthorized access dequeuing file, folder not accessible: {0}. Exception: {1}.", Level = EventLevel.Error)]
        public void TransmissionStorageDequeueUnauthorizedAccessException(string fileName, string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(60, fileName, exception, this.ApplicationName);
        }

        [Event(61, Keywords = Keywords.UserActionable, Message = "Inaccessible transmission storage file: {0}.", Level = EventLevel.Error)]
        public void TransmissionStorageInaccessibleFile(string fileName, string appDomainName = "Incorrect")
        {
            this.WriteEvent(61, fileName, this.ApplicationName);
        }

        [Event(
            62,
            Keywords = Keywords.Diagnostics,
            Message = "Transmission storage file '{0}' has expired and been deleted.  It was created on {1}.",
            Level = EventLevel.Warning)]
        public void TransmissionStorageFileExpired(string fileName, string created, string appDomainName = "Incorrect")
        {
            this.WriteEvent(62, fileName, created, this.ApplicationName);
        }

        [Event(63, Keywords = Keywords.UserActionable, Message = "Unexpected retry of known bad transmission storage file: {0}.", Level = EventLevel.Error)]
        public void TransmissionStorageUnexpectedRetryOfBadFile(string fileName, string appDomainName = "Incorrect")
        {
            this.WriteEvent(63, fileName, this.ApplicationName);
        }

        [Event(64, Message = "Transmission locally throttled. Throttle Limit: {0}. Attempted: {1}. Accepted: {2}. ", Level = EventLevel.Warning)]
        public void TransmissionThrottledWarning(int limit, int attempted, int accepted, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                64,
                limit.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
                attempted.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
                accepted.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
                this.ApplicationName);
        }

        [Event(65, Message = "The backlog of unsent items has reached maximum size of {0}. Items will be dropped until the backlog is cleared.",
        Level = EventLevel.Error)]
        public void ItemDroppedAsMaximumUnsentBacklogSizeReached(int maxBacklogSize, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                65,
                maxBacklogSize,
                this.ApplicationName);
        }

        [Event(66, Message = "[msg=Log Error];[msg={0}]", Level = EventLevel.Error)]
        public void LogError(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                66,
                msg ?? string.Empty,
                this.ApplicationName);
        }

        [Event(67, Message = "Item was rejected because it has no instrumentation key set. Item: {0}", Level = EventLevel.Verbose)]
        public void ItemRejectedNoInstrumentationKey(string item, string appDomainName = "Incorrect")
        {
            this.WriteEvent(67, item ?? string.Empty, this.ApplicationName);
        }

        [Event(68, Message = "Failed to set access permissions on storage directory {0}. Error : {1}.", Level = EventLevel.Warning)]
        public void FailedToSetSecurityPermissionStorageDirectory(string directory, string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(68, directory, error, this.ApplicationName);
        }

        [Event(69, Message = "TransmissionDataLossError. Telemetry items are being lost here due to unknown error. TransmissionId: {0}. Error Message: {1}.", Level = EventLevel.Error)]
        public void TransmissionDataLossError(string transmissionId, string message, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                69,
                transmissionId ?? string.Empty,
                message ?? string.Empty,
                this.ApplicationName);
        }

        [Event(70, Message = "Raw response content from AI Backend for Transmission Id {0} : {1}.", Level = EventLevel.Verbose)]
        public void RawResponseFromAIBackend(string transmissionId, string message, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                70,
                transmissionId ?? string.Empty,
                message ?? string.Empty,
                this.ApplicationName);
        }

        [Event(71, Message = "TransmissionDataLossError. Telemetry items are being lost here as the response code is not in the whitelisted set of retriable codes." +
                             "TransmissionId: {0}. Status Code: {1}.", Level = EventLevel.Warning)]
        public void TransmissionDataNotRetriedForNonWhitelistedResponse(string transmissionId, string status, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                71,
                transmissionId ?? string.Empty,
                status ?? string.Empty,
                this.ApplicationName);
        }

        [Event(72, Message = "Sampled out at head, sampled in at tail, gain up calculated: {0}.", Level = EventLevel.Verbose)]
        public void ItemProactivelySampledOut(string telemetryType, string appDomainName = "Incorrect")
        {
            this.WriteEvent(72, telemetryType ?? string.Empty, this.ApplicationName);
        }

        [Event(73, Message = "Configuration Error: Cannot specify both Included and Excluded types in the sampling processor. Included will be ignored.", Level = EventLevel.Warning)]
        public void SamplingConfigErrorBothTypes(string appDomainName = "Incorrect")
        {
            this.WriteEvent(73, this.ApplicationName);
        }

        [Event(74, Message = "TelemetryChannel found a telemetry item without an InstrumentationKey. This is a required field and must be set in either your config file or at application startup.", Level = EventLevel.Error, Keywords = Keywords.UserActionable)]
        public void TelemetryChannelNoInstrumentationKey(string appDomainName = "Incorrect")
        {
            this.WriteEvent(74, this.ApplicationName);
        }

        [Event(
            75,
            Keywords = Keywords.UserActionable,
            Message = "Unable to use configured StorageFolder: {2}. Please make sure the folder exist and the application has read/write permissions to the same. Currently monitoring will continue but if telemetry cannot be sent it will be dropped. User: {1} Error message: {0}.",
            Level = EventLevel.Error)]
        public void TransmissionCustomStorageError(string error, string user, string customFolder, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                75,
                error ?? string.Empty,
                user ?? string.Empty,
                customFolder ?? string.Empty,
                this.ApplicationName);
        }

        [Event(76, Message = "Flush telemetry events using IAsyncFlushable.FlushAsync.", Level = EventLevel.Verbose)]
        public void TelemetryChannelFlushAsync(string appDomainName = "Incorrect")
        {
            this.WriteEvent(76, this.ApplicationName);
        }

        [Event(77, Message = "TransmissionFlushAsyncFailed. Exception:{0}", Level = EventLevel.Warning)]
        public void TransmissionFlushAsyncWarning(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(77, exception ?? string.Empty, this.ApplicationName);
        }

        [Event(78, Message = "AuthenticatedTransmissionError. Received a failed ingestion response. TransmissionId: {0}. Status Code: {1}. Status Description: {2}", Level = EventLevel.Warning)]
        public void AuthenticationPolicyCaughtFailedIngestion(string transmissionId, string statusCode, string statusDescription, string appDomainName = "Incorrect")
        {
            this.WriteEvent(78, transmissionId ?? string.Empty, statusCode ?? string.Empty, statusDescription ?? string.Empty, this.ApplicationName);
        }

        [Event(79, Message = "Unexpected backend response. Invalid Error index in response: {0}.", Level = EventLevel.Warning)]
        public void UnexpectedBreezeResponseErrorIndexWarning(int index, string appDomainName = "Incorrect")
        {
            this.WriteEvent(79, index, this.ApplicationName);
        }

        private static string GetApplicationName()
        {
            //// We want to add application name to all events BUT
            //// It is prohibited by EventSource rules to have more parameters in WriteEvent that in event source method
            //// Parameter will be available in payload but in the next versions EventSource may
            //// start validating that number of parameters match
            //// It is not allowed to call additional methods, only WriteEvent

            string name;
            try
            {
                name = AppDomain.CurrentDomain.FriendlyName;
            }
            catch (Exception exp)
            {
                name = "Undefined " + exp.Message ?? exp.ToString();
            }

            return name;
        }

        public sealed class Keywords
        {
            public const EventKeywords UserActionable = (EventKeywords)0x1;

            public const EventKeywords Diagnostics = (EventKeywords)0x2;
        }
    }
}
