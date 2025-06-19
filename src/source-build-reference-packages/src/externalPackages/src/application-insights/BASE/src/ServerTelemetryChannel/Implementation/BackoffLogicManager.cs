namespace Microsoft.ApplicationInsights.Channel.Implementation
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;

    internal class BackoffLogicManager
    {
        internal const int SlotDelayInSeconds = 10;

        private const int MaxDelayInSeconds = 3600;
        private const int DefaultBackoffEnabledReportingIntervalInMin = 30;

        private static readonly Random Random = new Random();
        private static readonly DataContractJsonSerializer Serializer = new DataContractJsonSerializer(typeof(BackendResponse));

        private readonly object lockConsecutiveErrors = new object();
        private readonly TimeSpan minIntervalToUpdateConsecutiveErrors;
        
        private bool exponentialBackoffReported = false;
        private int consecutiveErrors;
        private DateTimeOffset nextMinTimeToUpdateConsecutiveErrors = DateTimeOffset.MinValue;

        public BackoffLogicManager()
        {
            this.DefaultBackoffEnabledReportingInterval = TimeSpan.FromMinutes(DefaultBackoffEnabledReportingIntervalInMin);
            this.minIntervalToUpdateConsecutiveErrors = TimeSpan.FromSeconds(SlotDelayInSeconds);
            this.CurrentDelay = TimeSpan.FromSeconds(SlotDelayInSeconds);
        }

        public BackoffLogicManager(TimeSpan defaultBackoffEnabledReportingInterval)
        {
            this.DefaultBackoffEnabledReportingInterval = defaultBackoffEnabledReportingInterval;
            this.minIntervalToUpdateConsecutiveErrors = TimeSpan.FromSeconds(SlotDelayInSeconds);
            this.CurrentDelay = TimeSpan.FromSeconds(SlotDelayInSeconds);
        }

        internal BackoffLogicManager(TimeSpan defaultBackoffEnabledReportingInterval, TimeSpan minIntervalToUpdateConsecutiveErrors) 
            : this(defaultBackoffEnabledReportingInterval)
        {
            // This constructor is used from unit tests
            this.minIntervalToUpdateConsecutiveErrors = minIntervalToUpdateConsecutiveErrors;
        }

        /// <summary>
        /// Gets the number of consecutive errors SDK transmitter got so far while sending telemetry to backend.
        /// </summary>
        public int ConsecutiveErrors
        {
            get { return this.consecutiveErrors; }
        }

        /// <summary>
        /// Gets the last status code SDK received from the backend.
        /// </summary>
        public int LastStatusCode { get; private set; }

        public TimeSpan DefaultBackoffEnabledReportingInterval { get; set; }

        internal TimeSpan CurrentDelay { get; private set; }

        public static BackendResponse GetBackendResponse(string responseContent)
        {
            BackendResponse backendResponse = null;

            try
            {
                if (!string.IsNullOrEmpty(responseContent))
                {
                    using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(responseContent)))
                    {
                        backendResponse = Serializer.ReadObject(ms) as BackendResponse;
                    }  
                }
            }
            catch (ArgumentException exp)
            {
                TelemetryChannelEventSource.Log.BreezeResponseWasNotParsedWarning(exp.Message, responseContent);
                backendResponse = null;
            }
            catch (InvalidOperationException exp)
            {
                TelemetryChannelEventSource.Log.BreezeResponseWasNotParsedWarning(exp.Message, responseContent);
                backendResponse = null;
            }
            catch (SerializationException exp)
            {
                TelemetryChannelEventSource.Log.BreezeResponseWasNotParsedWarning(exp.Message, responseContent);
                backendResponse = null;
            }

            return backendResponse;
        }

        /// <summary>
        /// Sets ConsecutiveErrors to 0.
        /// </summary>
        public void ResetConsecutiveErrors()
        {
            lock (this.lockConsecutiveErrors)
            {
                this.consecutiveErrors = 0;
            }
        }

        public void ReportBackoffEnabled(int statusCode)
        {
            this.LastStatusCode = statusCode;
            
            if (!this.exponentialBackoffReported && this.CurrentDelay > this.DefaultBackoffEnabledReportingInterval)
            {
                TelemetryChannelEventSource.Log.BackoffEnabled(this.CurrentDelay.TotalMinutes, statusCode);
                this.exponentialBackoffReported = true;
            }

            lock (this.lockConsecutiveErrors)
            {
                // Do not increase number of errors more often than minimum interval (SlotDelayInSeconds) 
                // since we have 3 senders and all of them most likely would fail if we have intermittent error  
                if (DateTimeOffset.UtcNow > this.nextMinTimeToUpdateConsecutiveErrors)
                {
                    this.consecutiveErrors++;
                    this.nextMinTimeToUpdateConsecutiveErrors = DateTimeOffset.UtcNow + this.minIntervalToUpdateConsecutiveErrors;
                }
            }
        }

        public void ReportBackoffDisabled()
        {
            this.LastStatusCode = 200;

            if (this.exponentialBackoffReported)
            {
                TelemetryChannelEventSource.Log.BackoffDisabled();
                this.exponentialBackoffReported = false;
            }
        }

        public TimeSpan GetBackOffTimeInterval(string headerValue)
        {
            TimeSpan backOffTime = this.GetBackOffTime(headerValue);
            this.CurrentDelay = backOffTime;

            return backOffTime;
        }

        // Calculates the time to wait before retrying in case of an error based on
        // http://en.wikipedia.org/wiki/Exponential_backoff
        protected virtual TimeSpan GetBackOffTime(string headerValue)
        {
            if (!TryParseRetryAfter(headerValue, out TimeSpan retryAfterTimeSpan))
            {
                double delayInSeconds;

                if (this.ConsecutiveErrors <= 1)
                {
                    delayInSeconds = SlotDelayInSeconds;
                }
                else
                {
                    double backOffSlot = (Math.Pow(2, this.ConsecutiveErrors) - 1) / 2;
                    var backOffDelay = Random.Next(1, (int)Math.Min(backOffSlot * SlotDelayInSeconds, int.MaxValue));
                    delayInSeconds = Math.Max(Math.Min(backOffDelay, MaxDelayInSeconds), SlotDelayInSeconds);
                }

                TelemetryChannelEventSource.Log.BackoffTimeSetInSeconds(delayInSeconds);
                retryAfterTimeSpan = TimeSpan.FromSeconds(delayInSeconds);
            }

            TelemetryChannelEventSource.Log.BackoffInterval(retryAfterTimeSpan.TotalSeconds);
            return retryAfterTimeSpan;
        }

        private static bool TryParseRetryAfter(string retryAfter, out TimeSpan retryAfterTimeSpan)
        {
            retryAfterTimeSpan = TimeSpan.FromSeconds(0);

            if (string.IsNullOrEmpty(retryAfter))
            {
                return false;
            }

            TelemetryChannelEventSource.Log.RetryAfterHeaderIsPresent(retryAfter);

            var now = DateTimeOffset.UtcNow;
            if (DateTimeOffset.TryParse(retryAfter, out DateTimeOffset retryAfterDate))
            {
                if (retryAfterDate > now)
                {
                    retryAfterTimeSpan = retryAfterDate - now;
                    return true;
                }

                return false;
            }

            TelemetryChannelEventSource.Log.TransmissionPolicyRetryAfterParseFailedWarning(retryAfter);

            return false;
        }
    }
}
