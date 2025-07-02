namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Implements an asynchronous transmission of data to an HTTP POST endpoint.
    /// </summary>
    public class Transmission
    {
        internal const string ContentEncodingHeader = "Content-Encoding";

        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(100);
        private static HttpClient client = new HttpClient(new RedirectHttpHandler()) { Timeout = System.Threading.Timeout.InfiniteTimeSpan };
        private static long flushAsyncCounter = 0;

        private int isSending;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transmission"/> class.
        /// </summary>
        public Transmission(Uri address, byte[] content, string contentType, string contentEncoding, TimeSpan timeout = default(TimeSpan))
        {
            this.EndpointAddress = address ?? throw new ArgumentNullException(nameof(address));
            this.Content = content ?? throw new ArgumentNullException(nameof(content));
            this.ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
            this.ContentEncoding = contentEncoding;
            this.Timeout = timeout == default(TimeSpan) ? DefaultTimeout : timeout;
            this.Id = Convert.ToBase64String(BitConverter.GetBytes(WeakConcurrentRandom.Instance.Next()));
            this.TelemetryItems = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transmission"/> class.
        /// </summary>        
        public Transmission(Uri address, ICollection<ITelemetry> telemetryItems, TimeSpan timeout = default(TimeSpan))
            : this(address, JsonSerializer.Serialize(telemetryItems, true), JsonSerializer.ContentType, JsonSerializer.CompressionType, timeout)
        {
            this.TelemetryItems = telemetryItems;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transmission"/> class. This overload is for Test purposes. 
        /// </summary>
        internal Transmission(Uri address, IEnumerable<ITelemetry> telemetryItems, string contentType, string contentEncoding, TimeSpan timeout = default(TimeSpan))
            : this(address, JsonSerializer.Serialize(telemetryItems), contentType, contentEncoding, timeout)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transmission"/> class. This overload is for Test purposes. 
        /// </summary>
        internal Transmission(Uri address, byte[] content, HttpClient passedClient, string contentType, string contentEncoding, TimeSpan timeout = default(TimeSpan))
            : this(address, content, contentType, contentEncoding, timeout)
        {
            client = passedClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transmission"/> class. This overload is for Test purposes. 
        /// </summary>
        protected internal Transmission()
        {
        }

        /// <summary>
        /// Gets or Sets an event notification to track ingestion endpoint response.
        /// </summary>
        public EventHandler<TransmissionStatusEventArgs> TransmissionStatusEvent { get; set; }

        /// <summary>
        /// Gets the Address of the endpoint to which transmission will be sent.
        /// </summary>
        public Uri EndpointAddress
        {
            get;
            private set;
        }

#pragma warning disable CA1819 // "Properties should not return arrays" - part of the public API and too late to change.
        /// <summary>
        /// Gets the content of the transmission.
        /// </summary>
        public byte[] Content
        {
            get;
            private set;
        }
#pragma warning restore CA1819 // "Properties should not return arrays" - part of the public API and too late to change.

        /// <summary>
        /// Gets the content's type of the transmission.
        /// </summary>
        public string ContentType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the encoding method of the transmission.
        /// </summary>
        public string ContentEncoding
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a timeout value for the transmission.
        /// </summary>
        public TimeSpan Timeout
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets an id of the transmission.
        /// </summary>
        public string Id
        {
            get; private set;
        }

        /// <summary>
        /// Gets the number of telemetry items in the transmission.
        /// </summary>
        public ICollection<ITelemetry> TelemetryItems
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the <see cref="CredentialEnvelope"/> which is used for AAD. 
        /// This is used include an AAD token on HTTP Requests sent to ingestion.
        /// </summary>
        internal CredentialEnvelope CredentialEnvelope { get; set; }

        /// <summary>
        /// Gets the flush async id for the transmission.
        /// </summary>
        internal long FlushAsyncId { get; } = Interlocked.Increment(ref flushAsyncCounter);

        /// <summary>
        /// Gets or sets a value indicating whether FlushAsync is in progress.
        /// </summary>
        internal bool IsFlushAsyncInProgress { get; set; } = false;

        /// <summary>
        /// Executes the request that the current transmission represents.
        /// </summary>
        /// <returns>The task to await.</returns>
        public virtual async Task<HttpWebResponseWrapper> SendAsync()
        {
            if (Interlocked.CompareExchange(ref this.isSending, 1, 0) != 0)
            {
                throw new InvalidOperationException("SendAsync is already in progress.");
            }

            try
            {
                using (MemoryStream contentStream = new MemoryStream(this.Content))
                {
                    HttpRequestMessage request = this.CreateRequestMessage(this.EndpointAddress, contentStream);
                    HttpWebResponseWrapper wrapper = null;
                    long responseDurationInMs = 0;

                    try
                    {
                        using (var ct = new CancellationTokenSource(this.Timeout))
                        {
                            // HttpClient.SendAsync throws HttpRequestException only on the following scenarios:
                            // "The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout."
                            // i.e for Server errors (500 status code), no exception is thrown. Hence this method should read the response and status code,
                            // and return correct HttpWebResponseWrapper to give any Retry policies a chance to retry as needed.
                            var stopwatch = new Stopwatch();
                            stopwatch.Start();

                            using (var response = await client.SendAsync(request, ct.Token).ConfigureAwait(false))
                            {
                                stopwatch.Stop();
                                responseDurationInMs = stopwatch.ElapsedMilliseconds;
                                CoreEventSource.Log.IngestionResponseTime(response != null ? (int)response.StatusCode : -1, responseDurationInMs);
                                // Log ingestion respose time as event counter metric.
                                CoreEventSource.Log.IngestionResponseTimeEventCounter(stopwatch.ElapsedMilliseconds);

                                if (response != null)
                                {
                                    wrapper = new HttpWebResponseWrapper
                                    {
                                        StatusCode = (int)response.StatusCode,
                                        StatusDescription = response.ReasonPhrase, // maybe not required?
                                    };
                                    wrapper.RetryAfterHeader = response.Headers?.RetryAfter?.ToString();

                                    if (response.StatusCode == HttpStatusCode.PartialContent)
                                    {
                                        if (response.Content != null)
                                        {
                                            // Read the entire response body only on PartialContent for perf reasons.
                                            // This cannot be avoided as response tells which items are to be resubmitted.
                                            wrapper.Content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                                        }
                                    }

                                    if (CoreEventSource.IsVerboseEnabled && response.StatusCode != HttpStatusCode.PartialContent)
                                    {
                                        // Read the entire response body only on VerboseTracing for perf reasons.
                                        try
                                        {
                                            if (response.Content != null)
                                            {
                                                wrapper.Content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            // Swallow any exception here as this code is for tracing purposes only and should never throw.
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        wrapper = new HttpWebResponseWrapper
                        {
                            StatusCode = (int)HttpStatusCode.RequestTimeout,
                        };
                    }
                    finally
                    {
                        try
                        {
                            // Initiates event notification to subscriber with Transmission and TransmissionStatusEventArgs.
                            this.TransmissionStatusEvent?.Invoke(this, new TransmissionStatusEventArgs(wrapper ?? new HttpWebResponseWrapper() { StatusCode = 999 }, responseDurationInMs));
                        }
                        catch (Exception ex)
                        {
                            CoreEventSource.Log.TransmissionStatusEventFailed(ex);
                        }
                    }

                    return wrapper;
                }
            }
            finally
            {
                Interlocked.Exchange(ref this.isSending, 0);
            }
        }

        /// <summary>
        /// Splits the Transmission object into two pieces using a method 
        /// to determine the length of the first piece based off of the length of the transmission.
        /// </summary>
        /// <returns>
        /// A tuple with the first item being a Transmission object with n ITelemetry objects
        /// and the second item being a Transmission object with the remaining ITelemetry objects.
        /// </returns>
        public virtual Tuple<Transmission, Transmission> Split(Func<int, int> calculateLength)
        {
            if (calculateLength == null)
            {
                throw new ArgumentNullException(nameof(calculateLength));
            }

            Transmission transmissionA = this;
            Transmission transmissionB = null;

            // We can be more efficient if we have a copy of the telemetry items still
            if (this.TelemetryItems != null)
            {
                // We don't need to deserialize, we have a copy of each telemetry item
                int numItems = calculateLength(this.TelemetryItems.Count);
                if (numItems != this.TelemetryItems.Count)
                {
                    List<ITelemetry> itemsA = new List<ITelemetry>();
                    List<ITelemetry> itemsB = new List<ITelemetry>();
                    var i = 0;
                    foreach (var item in this.TelemetryItems)
                    {
                        if (i < numItems)
                        {
                            itemsA.Add(item);
                        }
                        else
                        {
                            itemsB.Add(item);
                        }

                        i++;
                    }

                    transmissionA = new Transmission(
                        this.EndpointAddress,
                        itemsA);
                    transmissionB = new Transmission(
                        this.EndpointAddress,
                        itemsB);
                }
            }
            else if (this.ContentType == JsonSerializer.ContentType)
            {
                // We have to decode the payload in order to split
                bool compress = this.ContentEncoding == JsonSerializer.CompressionType;
                string[] payloadItems = JsonSerializer
                    .Deserialize(this.Content, compress)
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                int numItems = calculateLength(payloadItems.Length);

                if (numItems != payloadItems.Length)
                {
                    string itemsA = string.Empty;
                    string itemsB = string.Empty;

                    for (int i = 0; i < payloadItems.Length; i++)
                    {
                        if (i < numItems)
                        {
                            if (!string.IsNullOrEmpty(itemsA))
                            {
                                itemsA += Environment.NewLine;
                            }

                            itemsA += payloadItems[i];
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(itemsB))
                            {
                                itemsB += Environment.NewLine;
                            }

                            itemsB += payloadItems[i];
                        }
                    }

                    transmissionA = new Transmission(
                        this.EndpointAddress,
                        JsonSerializer.ConvertToByteArray(itemsA, compress),
                        this.ContentType,
                        this.ContentEncoding);
                    transmissionB = new Transmission(
                        this.EndpointAddress,
                        JsonSerializer.ConvertToByteArray(itemsB, compress),
                        this.ContentType,
                        this.ContentEncoding);
                }
            }
            else
            {
                // We can't deserialize it!
                // We can say it's of length 1 at the very least
                int numItems = calculateLength(1);

                if (numItems == 0)
                {
                    transmissionA = null;
                    transmissionB = this;
                }
            }

            return Tuple.Create(transmissionA, transmissionB);
        }

        /// <summary>
        /// Serializes telemetry items.
        /// </summary>
        /// TODO: Refactor this method, it does more than serialization activity.
        internal void Serialize(Uri address, IEnumerable<ITelemetry> telemetryItems, TimeSpan timeout = default(TimeSpan))
        {
            this.EndpointAddress = address;
            this.Content = JsonSerializer.Serialize(telemetryItems);
            this.ContentType = JsonSerializer.ContentType;
            this.ContentEncoding = JsonSerializer.CompressionType;
            this.Timeout = timeout == default(TimeSpan) ? DefaultTimeout : timeout;
            this.Id = Convert.ToBase64String(BitConverter.GetBytes(WeakConcurrentRandom.Instance.Next()));
        }

        /// <summary>
        /// Creates an http request for sending a transmission.
        /// </summary>
        /// <param name="address">The address of the web request.</param>
        /// <param name="contentStream">The stream to write to.</param>
        /// <returns>The request. An object of type HttpRequestMessage.</returns>
        protected virtual HttpRequestMessage CreateRequestMessage(Uri address, Stream contentStream)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, address);
            request.Content = new StreamContent(contentStream);
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                request.Content.Headers.ContentType = new MediaTypeHeaderValue(this.ContentType);
            }

            if (!string.IsNullOrEmpty(this.ContentEncoding))
            {
                request.Content.Headers.Add(ContentEncodingHeader, this.ContentEncoding);
            }

            if (this.CredentialEnvelope != null)
            {
                var authToken = this.CredentialEnvelope.GetToken();

                if (authToken == default(AuthToken))
                {
                    // TODO: DO NOT SEND. RETURN FAILURE AND LET CHANNEL DECIDE WHEN TO RETRY.
                    // This could be either a configuration error or the AAD service is unavailable.
                }
                else
                {
                    request.Headers.TryAddWithoutValidation(AuthConstants.AuthorizationHeaderName, AuthConstants.AuthorizationTokenPrefix + authToken.Token);
                }
            }

            return request;
        }

        /// <summary>
        /// Creates a post web request.
        /// </summary>
        /// <param name="address">The Address in the web request.</param>
        /// <returns>A web request pointing to the <c>Address</c>.</returns>
        [Obsolete("Use CreateRequestMessage instead as SendAsync is now using HttpClient to send HttpRequest.")]
        protected virtual WebRequest CreateRequest(Uri address)
        {
            var request = WebRequest.Create(address);

            request.Method = "POST";

            if (!string.IsNullOrEmpty(this.ContentType))
            {
                request.ContentType = this.ContentType;
            }

            if (!string.IsNullOrEmpty(this.ContentEncoding))
            {
                request.Headers[ContentEncodingHeader] = this.ContentEncoding;
            }

            return request;
        }
    }
}
