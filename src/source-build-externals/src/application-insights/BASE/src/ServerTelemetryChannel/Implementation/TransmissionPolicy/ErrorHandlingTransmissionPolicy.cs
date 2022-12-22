namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    internal class ErrorHandlingTransmissionPolicy : TransmissionPolicy, IDisposable
    {
        private BackoffLogicManager backoffLogicManager;
        private TaskTimerInternal pauseTimer = new TaskTimerInternal { Delay = TimeSpan.FromSeconds(BackoffLogicManager.SlotDelayInSeconds) };

        public override void Initialize(Transmitter transmitter)
        {
            if (transmitter == null)
            {
                throw new ArgumentNullException(nameof(transmitter));
            }

            this.backoffLogicManager = transmitter.BackoffLogicManager;

            base.Initialize(transmitter);
            transmitter.TransmissionSent += this.HandleTransmissionSentEvent;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static void AdditionalVerboseTracing(string httpResponse)
        {
            // For perf reason deserialize the response only when verbose tracing is enabled 
            if (TelemetryChannelEventSource.IsVerboseEnabled && httpResponse != null)
            {
                try
                {
                    BackendResponse backendResponse = BackoffLogicManager.GetBackendResponse(httpResponse);
                    if (backendResponse != null && backendResponse.Errors != null)
                    {
                        foreach (var error in backendResponse.Errors)
                        {
                            if (error != null)
                            {
                                TelemetryChannelEventSource.Log.ItemRejectedByEndpointWarning(error.Message);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // This code is for tracing purposes only; it cannot not throw
                }
            }
        }

        private void HandleTransmissionSentEvent(object sender, TransmissionProcessedEventArgs e)
        {
            HttpWebResponseWrapper httpWebResponseWrapper = e.Response;
            if (httpWebResponseWrapper != null)
            {
                AdditionalVerboseTracing(httpWebResponseWrapper.Content);
                if (httpWebResponseWrapper.StatusCode == ResponseStatusCodes.Success || httpWebResponseWrapper.StatusCode == ResponseStatusCodes.PartialSuccess)
                {
                    // There is no further action for ErrorHandlingTransmissionPolicy here as transmission is success/partial success.
                    return;
                }

                switch (httpWebResponseWrapper.StatusCode)
                {
                    case ResponseStatusCodes.BadGateway:
                    case ResponseStatusCodes.GatewayTimeout:
                    case ResponseStatusCodes.RequestTimeout:
                    case ResponseStatusCodes.ServiceUnavailable:
                    case ResponseStatusCodes.InternalServerError:
                    case ResponseStatusCodes.UnknownNetworkError:
                        // Disable sending and buffer capacity (Enqueue will enqueue to the Storage)
                        this.MaxSenderCapacity = 0;
                        this.MaxBufferCapacity = 0;
                        this.LogCapacityChanged();
                        this.Apply();

                        this.backoffLogicManager.ReportBackoffEnabled((int)httpWebResponseWrapper.StatusCode);
                        this.Transmitter.Enqueue(e.Transmission);

                        this.pauseTimer.Delay = this.backoffLogicManager.GetBackOffTimeInterval(httpWebResponseWrapper.RetryAfterHeader);
                        this.pauseTimer.Start(
                           () =>
                           {
                               this.MaxBufferCapacity = null;
                               this.MaxSenderCapacity = null;
                               this.LogCapacityChanged();
                               this.Apply();

                               this.backoffLogicManager.ReportBackoffDisabled();

                               return Task.FromResult<object>(null);
                           });
                        break;
                    default:                        
                        // We are losing data here but that is intentional as the response code is
                        // not in the whitelisted set to attempt retry.
                        TelemetryChannelEventSource.Log.TransmissionDataNotRetriedForNonWhitelistedResponse(e.Transmission.Id,
                            httpWebResponseWrapper.StatusCode.ToString(CultureInfo.InvariantCulture));
                        // For non white listed response, set the result of FlushAsync to false.
                        e.Transmission.IsFlushAsyncInProgress = false;
                        break;
                }
            }
            else
            {
                // Data loss Unknown Exception
                // We are losing data here (we did not upload failed transaction back).
                // We got unknown exception. 
                if (e.Exception != null)
                {                    
                    TelemetryChannelEventSource.Log.TransmissionDataLossError(e.Transmission.Id,
                        e.Exception.Message);
                }
                else
                {
                    TelemetryChannelEventSource.Log.TransmissionDataLossError(e.Transmission.Id,
                        "Unknown Exception Message");
                }

                // For Unknown Exception set the result of FlushAsync to false.
                e.Transmission.IsFlushAsyncInProgress = false;
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.pauseTimer != null)
                {
                    this.pauseTimer.Dispose();
                    this.pauseTimer = null;
                }
            }
        }
    }
}