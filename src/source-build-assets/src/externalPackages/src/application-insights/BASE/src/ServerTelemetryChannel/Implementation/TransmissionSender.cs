namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Common.Extensions;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication;

    internal class TransmissionSender
    {
        private static readonly HttpWebResponseWrapper DefaultHttpWebResponseWrapper = default(HttpWebResponseWrapper);
        // Stores all inflight requests using this dictionary, before SendAsync.
        // Removes entry from dictionary after response.
        private ConcurrentDictionary<long, Task<HttpWebResponseWrapper>> inFlightTransmissions = new ConcurrentDictionary<long, Task<HttpWebResponseWrapper>>();
        private int transmissionCount = 0;
        private int capacity = 10;

        private bool applyThrottle = false;
        private int throttleWindowInMilliseconds = 1000;
        private long currentThrottleWindowId = 0;
        private int currentItemsCount = 0;
        private int throttleLimit = 10;

        public event EventHandler<TransmissionProcessedEventArgs> TransmissionSent;

        /// <summary>
        /// Gets or sets the maximum number of <see cref="Transmission"/> objects that can be sent simultaneously.
        /// </summary>
        /// <remarks>
        /// Use this property to limit the number of concurrent HTTP connections. Once the maximum number of 
        /// transmissions in progress is reached, <see cref="Enqueue"/> will stop accepting new transmissions
        /// until previous transmissions are sent.
        /// </remarks>
        public virtual int Capacity
        {
            get
            {
                return this.capacity;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.capacity = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a limiter on the maximum number of <see cref="ITelemetry"/> objects 
        /// that can be sent in a given throttle window is enabled. Items attempted to be sent exceeding of the local 
        /// throttle amount will be treated the same as a backend throttle.
        /// </summary>
        public virtual bool ApplyThrottle
        {
            get
            {
                return this.applyThrottle;
            }

            set
            {
                this.applyThrottle = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of items that will be allowed to send in a given throttle window.
        /// </summary>
        public virtual int ThrottleLimit
        {
            get
            {
                return this.throttleLimit;
            }

            set
            {
                this.throttleLimit = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the self-limiting throttle window in milliseconds.
        /// </summary>
        public virtual int ThrottleWindow
        {
            get
            {
                return this.throttleWindowInMilliseconds;
            }

            set
            {
                this.throttleWindowInMilliseconds = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="CredentialEnvelope"/> which is used for AAD.
        /// </summary>
        /// <remarks>
        /// <see cref="ISupportCredentialEnvelope.CredentialEnvelope"/> on <see cref="ServerTelemetryChannel"/> sets <see cref="Transmitter.CredentialEnvelope"/> and then sets <see cref="TransmissionSender.CredentialEnvelope"/> 
        /// which is used to set <see cref="Transmission.CredentialEnvelope"/> just before calling <see cref="Transmission.SendAsync"/>.
        /// </remarks>
        internal CredentialEnvelope CredentialEnvelope { get; set; }

        public virtual bool Enqueue(Func<Transmission> transmissionGetter)
        {
            bool enqueueSucceded = false;

            int currentCount = Interlocked.Increment(ref this.transmissionCount);
            if (currentCount <= this.capacity)
            {
                var transmission = transmissionGetter();

                if (transmission != null)
                {
                    ExceptionHandler.Start(this.StartSending, transmission);
                    enqueueSucceded = true;
                }
                else
                {
                    // We tried to dequeue from buffer and buffer is empty
                    // return false to stop moving from buffer to sender when policy is applied otherwise we get infinite loop
                }
            }
            else
            {
                TelemetryChannelEventSource.Log.SenderEnqueueNoCapacityWarning(currentCount - 1, this.capacity);
            }

            if (!enqueueSucceded)
            {
                Interlocked.Decrement(ref this.transmissionCount);
            }

            return enqueueSucceded;
        }

        /// <summary>
        /// Wait for inflight transmissions to complete. Honors the CancellationToken set by an application on IAsyncFlushable.FlushAsync.
        /// </summary>
        internal Task<TaskStatus> WaitForPreviousTransmissionsToComplete(CancellationToken cancellationToken)
        {
            var transmissionFlushAsyncId = this.inFlightTransmissions.LastOrDefault().Key;
            return this.WaitForPreviousTransmissionsToComplete(transmissionFlushAsyncId, cancellationToken);
        }

        /// <summary>
        /// Wait for inflight transmissions to complete. Honors the CancellationToken set by an application on IAsyncFlushable.FlushAsync.
        /// </summary>
        internal async Task<TaskStatus> WaitForPreviousTransmissionsToComplete(long transmissionFlushAsyncId, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return TaskStatus.Canceled;
            }

            var activeTransmissions = this.inFlightTransmissions.Where(p => p.Key <= transmissionFlushAsyncId).Select(p => p.Value);

            if (activeTransmissions?.Count() > 0)
            {
                // Wait for all transmissions over the wire to complete.
                var inTransitTasks = Task<HttpWebResponseWrapper>.WhenAll(activeTransmissions);
                // Respect passed Cancellation token from FlushAsync call
                var inTransitTasksWithCancellationToken = Task<HttpWebResponseWrapper>
                                                          .WhenAny(inTransitTasks, new Task<HttpWebResponseWrapper>(() => { return DefaultHttpWebResponseWrapper; }, cancellationToken));
                await inTransitTasksWithCancellationToken.ConfigureAwait(false);
                return cancellationToken.IsCancellationRequested ? TaskStatus.Canceled : inTransitTasksWithCancellationToken.Status;
            }

            return TaskStatus.RanToCompletion;
        }

        protected void OnTransmissionSent(TransmissionProcessedEventArgs args)
        {
            this.TransmissionSent?.Invoke(this, args);
        }

        private async Task StartSending(Transmission transmission)
        {
            SdkInternalOperationsMonitor.Enter();
            Task<HttpWebResponseWrapper> transmissionTask = null;

            try
            {
                Exception exception = null;
                HttpWebResponseWrapper responseContent = null;

                // Locally self-throttle this payload before we send it
                Transmission acceptedTransmission = this.Throttle(transmission);

                // Now that we've self-imposed a throttle, we can try to send the remaining data
                try
                {
                    TelemetryChannelEventSource.Log.TransmissionSendStarted(acceptedTransmission.Id);
                    acceptedTransmission.CredentialEnvelope = this.CredentialEnvelope;

                    transmissionTask = acceptedTransmission.SendAsync();
                    this.inFlightTransmissions.TryAdd(transmission.FlushAsyncId, transmissionTask);
                    responseContent = await transmissionTask.ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    exception = e;
                }
                finally
                {
                    int currentCapacity = Interlocked.Decrement(ref this.transmissionCount);
                    if (exception != null)
                    {
                        TelemetryChannelEventSource.Log.TransmissionSendingFailedWarning(acceptedTransmission.Id, exception.ToLogString());
                    }
                    else
                    {
                        if (responseContent != null)
                        {
                            if (responseContent.StatusCode < 400)
                            {
                                TelemetryChannelEventSource.Log.TransmissionSentSuccessfully(acceptedTransmission.Id,
                                    currentCapacity);
                            }
                            else
                            {
                                TelemetryChannelEventSource.Log.TransmissionSendingFailedWarning(
                                    acceptedTransmission.Id,
                                    responseContent.StatusCode.ToString(CultureInfo.InvariantCulture));
                            }

                            if (TelemetryChannelEventSource.IsVerboseEnabled)
                            {
                                TelemetryChannelEventSource.Log.RawResponseFromAIBackend(acceptedTransmission.Id,
                                    responseContent.Content);
                            }
                        }
                    }

                    if (responseContent == null && exception is HttpRequestException)
                    {
                        // HttpClient.SendAsync throws HttpRequestException on the following scenarios:
                        // "The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout."
                        // https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.sendasync?view=netstandard-1.6
                        responseContent = new HttpWebResponseWrapper()
                        {
                            // Expectation is that RetryPolicy will attempt retry for this status.
                            StatusCode = ResponseStatusCodes.UnknownNetworkError,
                        };
                    }

                    this.inFlightTransmissions.TryRemove(transmission.FlushAsyncId, out _);
                    this.OnTransmissionSent(new TransmissionProcessedEventArgs(acceptedTransmission, exception, responseContent));
                }
            }
            finally
            {
                SdkInternalOperationsMonitor.Exit();
            }
        }

        /// <summary>
        /// Checks if the transmission throttling policy allows for sending another request.
        /// If so, this method will add a request to the current throttle count (unless peeking).
        /// </summary>
        /// <returns>The number of events that are able to be sent.</returns>
        private int IsTransmissionSendable(int numEvents, bool peek = false)
        {
            if (!this.applyThrottle)
            {
                return numEvents;
            }

            long throttleWindowId = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds / this.ThrottleWindow);
            bool isInThrottleWindow = throttleWindowId == this.currentThrottleWindowId;
            if (isInThrottleWindow && this.currentItemsCount < this.ThrottleLimit)
            {
                int numAccepted = Math.Min(numEvents, this.ThrottleLimit - this.currentItemsCount);
                if (!peek)
                {
                    this.currentItemsCount += numAccepted;
                }

                return numAccepted;
            }
            else if (!isInThrottleWindow)
            {
                int numAccepted = Math.Min(numEvents, this.ThrottleLimit);
                this.currentThrottleWindowId = throttleWindowId;
                this.currentItemsCount = numAccepted;
                return numAccepted;
            }

            return 0;
        }

        private Transmission Throttle(Transmission transmission)
        {
            if (!this.ApplyThrottle)
            {
                return transmission;
            }

            int attemptedItemsCount = -1;
            int acceptedItemsCount = -1;
            Tuple<Transmission, Transmission> transmissions = transmission.Split((transmissionLength) =>
            {
                attemptedItemsCount = transmissionLength;
                acceptedItemsCount = this.IsTransmissionSendable(transmissionLength);
                return acceptedItemsCount;
            });

            Transmission acceptedTransmission = transmissions.Item1;
            Transmission rejectedTransmission = transmissions.Item2;

            // Send rejected payload back for retry
            if (rejectedTransmission != null)
            {
                TelemetryChannelEventSource.Log.TransmissionThrottledWarning(this.ThrottleLimit, attemptedItemsCount, acceptedItemsCount);
                if (transmission.IsFlushAsyncInProgress)
                {
                    acceptedTransmission.IsFlushAsyncInProgress = true;
                    rejectedTransmission.IsFlushAsyncInProgress = true;
                }

                this.SendTransmissionThrottleRejection(rejectedTransmission);
            }

            return acceptedTransmission;
        }

        private void SendTransmissionThrottleRejection(Transmission rejectedTransmission)
        {
            WebException exception = new WebException(
                "Transmission was split by local throttling policy",
                null,
                System.Net.WebExceptionStatus.Success,
                null);
            this.OnTransmissionSent(new TransmissionProcessedEventArgs(
                rejectedTransmission,
                exception,
                new HttpWebResponseWrapper()
                {
                    StatusCode = ResponseStatusCodes.ResponseCodeTooManyRequests,
                    StatusDescription = "Internally Throttled",
                    RetryAfterHeader = null,
                }));
        }
    }
}
