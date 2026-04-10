#if NET452
namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Diagnostic listener implementation that listens for Http DiagnosticSource to see all outgoing HTTP dependency requests.
    /// </summary>
    internal class HttpDesktopDiagnosticSourceListener : IObserver<KeyValuePair<string, object>>, IDisposable
    {
        private readonly DesktopDiagnosticSourceHttpProcessing httpDesktopProcessing;
        private readonly HttpDesktopDiagnosticSourceSubscriber subscribeHelper;
        private readonly PropertyFetcher requestFetcherRequestEvent;
        private readonly PropertyFetcher requestFetcherResponseEvent;
        private readonly PropertyFetcher responseFetcher;
        private readonly PropertyFetcher requestFetcherResponseExEvent;
        private readonly PropertyFetcher responseExStatusFetcher;
        private readonly PropertyFetcher responseExHeadersFetcher;

        private bool disposed = false;

        internal HttpDesktopDiagnosticSourceListener(DesktopDiagnosticSourceHttpProcessing httpProcessing, ApplicationInsightsUrlFilter applicationInsightsUrlFilter)
        {
            this.httpDesktopProcessing = httpProcessing;
            this.subscribeHelper = new HttpDesktopDiagnosticSourceSubscriber(this, applicationInsightsUrlFilter);
            this.requestFetcherRequestEvent = new PropertyFetcher("Request");
            this.requestFetcherResponseEvent = new PropertyFetcher("Request");
            this.responseFetcher = new PropertyFetcher("Response");

            this.requestFetcherResponseExEvent = new PropertyFetcher("Request");
            this.responseExStatusFetcher = new PropertyFetcher("StatusCode");
            this.responseExHeadersFetcher = new PropertyFetcher("Headers");
        }

        /// <summary>
        /// IDisposable implementation.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// This method gets called once for each event from the Http DiagnosticSource.
        /// </summary>
        /// <param name="value">The pair containing the event name, and an object representing the payload. The payload
        /// is essentially a dynamic object that contain different properties depending on the event.</param>
        public void OnNext(KeyValuePair<string, object> value)
        {
            try
            {
                switch (value.Key)
                {
                    case "System.Net.Http.Desktop.HttpRequestOut.Start":
                    {
                        var request = (HttpWebRequest)this.requestFetcherRequestEvent.Fetch(value.Value);
                        DependencyCollectorEventSource.Log.HttpDesktopBeginCallbackCalled(ClientServerDependencyTracker.GetIdForRequestObject(request), request.RequestUri.ToString());

                        // With this event, DiagnosticSource injects headers himself (after the event)
                        // ApplicationInsights must not do this
                        this.httpDesktopProcessing.OnBegin(request, false);
                        break;
                    }

                    case "System.Net.Http.Desktop.HttpRequestOut.Stop":
                    {
                        // request is never null
                        var request = this.requestFetcherResponseEvent.Fetch(value.Value);
                        DependencyCollectorEventSource.Log.HttpDesktopEndCallbackCalled(ClientServerDependencyTracker.GetIdForRequestObject(request));
                        var response = this.responseFetcher.Fetch(value.Value);
                        this.httpDesktopProcessing.OnEndResponse(request, response);
                        break;
                    }

                    case "System.Net.Http.Desktop.HttpRequestOut.Ex.Stop":
                    {
                        // request is never null
                        var request = this.requestFetcherResponseExEvent.Fetch(value.Value);
                        DependencyCollectorEventSource.Log.HttpDesktopEndCallbackCalled(ClientServerDependencyTracker.GetIdForRequestObject(request));
                        object statusCode = this.responseExStatusFetcher.Fetch(value.Value);
                        object headers = this.responseExHeadersFetcher.Fetch(value.Value);
                        this.httpDesktopProcessing.OnEndResponse(request, statusCode, headers);
                        break;
                    }

                    case "System.Net.Http.InitializationFailed":
                    {
                        DependencyTableStore.IsDesktopHttpDiagnosticSourceActivated = false;

                        Exception ex = (Exception)value.Value.GetType().GetProperty("Exception")?.GetValue(value.Value);
                        DependencyCollectorEventSource.Log.HttpHandlerDiagnosticListenerFailedToInitialize(ex?.ToInvariantString());
                        break;
                    }

                    default:
                    {
                        DependencyCollectorEventSource.Log.NotExpectedCallback(value.GetHashCode(), value.Key, "unknown key");
                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                DependencyCollectorEventSource.Log.CallbackError(0, "OnNext", exc);
            }
        }

        /// <summary>
        /// Notifies the observer that the provider has finished sending push-based notifications.
        /// <seealso cref="IObserver{T}.OnCompleted()"/>
        /// </summary>
        public void OnCompleted()
        {
        }

        /// <summary>
        /// Notifies the observer that the provider has experienced an error condition.
        /// <seealso cref="IObserver{T}.OnError(Exception)"/>
        /// </summary>
        /// <param name="error">An object that provides additional information about the error.</param>
        public void OnError(Exception error)
        {
        }

        /// <summary>
        /// IDisposable implementation.
        /// </summary>
        /// <param name="disposing">The method has been called directly or indirectly by a user's code.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.subscribeHelper != null)
                    {
                        this.subscribeHelper.Dispose();
                    }
                }

                this.disposed = true;
            }
        }
    }
}
#endif