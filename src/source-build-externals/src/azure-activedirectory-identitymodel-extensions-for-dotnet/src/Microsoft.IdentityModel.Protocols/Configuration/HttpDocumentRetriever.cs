// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols
{
    /// <summary>
    /// Retrieves metadata information using HttpClient.
    /// </summary>
    public class HttpDocumentRetriever : IDocumentRetriever
    {
        private HttpClient _httpClient;
        private static readonly HttpClient _defaultHttpClient = new HttpClient();

        /// <summary>
        /// The key is used to add status code into <see cref="Exception.Data"/>.
        /// </summary>
        public const string StatusCode = "status_code";

        /// <summary>
        /// The key is used to add response content into <see cref="Exception.Data"/>.
        /// </summary>
        public const string ResponseContent = "response_content";

        /// <summary>
        /// Gets or sets whether additional default headers are added to a <see cref="HttpRequestMessage"/> headers. Set to true by default.
        /// </summary>
        public static bool DefaultSendAdditionalHeaderData { get; set; } = true;

        private bool _sendAdditionalHeaderData = DefaultSendAdditionalHeaderData;

        /// <summary>
        /// Gets or sets whether additional headers are added to a <see cref="HttpRequestMessage"/> headers
        /// </summary>
        public bool SendAdditionalHeaderData
        {
            get { return _sendAdditionalHeaderData; }
            set { _sendAdditionalHeaderData = value; }
        }

        internal IDictionary<string, string> AdditionalHeaderData { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpDocumentRetriever"/> class.
        /// </summary>
        public HttpDocumentRetriever()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpDocumentRetriever"/> class with a specified httpClient.
        /// </summary>
        /// <param name="httpClient"><see cref="HttpClient"/></param>
        /// <exception cref="ArgumentNullException">'httpClient' is null.</exception>
        public HttpDocumentRetriever(HttpClient httpClient)
        {
            if (httpClient == null)
                throw LogHelper.LogArgumentNullException("httpClient");

            _httpClient = httpClient;
        }

        /// <summary>
        /// Requires Https secure channel for sending requests.. This is turned ON by default for security reasons. It is RECOMMENDED that you do not allow retrieval from http addresses by default.
        /// </summary>
        public bool RequireHttps { get; set; } = true;

        /// <summary>
        /// Returns a task which contains a string converted from remote document when completed, by using the provided address.
        /// </summary>
        /// <param name="address">Location of document</param>
        /// <param name="cancel">A cancellation token that can be used by other objects or threads to receive notice of cancellation. <see cref="CancellationToken"/></param>
        /// <returns>Document as a string</returns>
        public async Task<string> GetDocumentAsync(string address, CancellationToken cancel)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw LogHelper.LogArgumentNullException("address");

            if (!Utility.IsHttps(address) && RequireHttps)
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX20108, address), nameof(address)));

            Exception unsuccessfulHttpResponseException;
            HttpResponseMessage response;
            try
            {
                if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                    LogHelper.LogVerbose(LogMessages.IDX20805, address);

                var httpClient = _httpClient ?? _defaultHttpClient;
                var uri = new Uri(address, UriKind.RelativeOrAbsolute);
                response = await SendAsyncAndRetryOnNetworkError(httpClient, uri).ConfigureAwait(false);

                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    return responseContent;

                unsuccessfulHttpResponseException = new IOException(LogHelper.FormatInvariant(LogMessages.IDX20807, address, response, responseContent));
                unsuccessfulHttpResponseException.Data.Add(StatusCode, response.StatusCode);
                unsuccessfulHttpResponseException.Data.Add(ResponseContent, responseContent);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new IOException(LogHelper.FormatInvariant(LogMessages.IDX20804, address), ex));
            }

            throw LogHelper.LogExceptionMessage(unsuccessfulHttpResponseException);
        }

        private async Task<HttpResponseMessage> SendAsyncAndRetryOnNetworkError(HttpClient httpClient, Uri uri)
        {
            int maxAttempt = 2;
            HttpResponseMessage response = null;
            for (int i = 1; i <= maxAttempt; i++)
            {
                // need to create a new message each time since you cannot send the same message twice
                using (var message = new HttpRequestMessage(HttpMethod.Get, uri))
                {
                    if (SendAdditionalHeaderData)
                        IdentityModelTelemetryUtil.SetTelemetryData(message, AdditionalHeaderData);

                    response = await httpClient.SendAsync(message).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                        return response;

                    if (response.StatusCode.Equals(HttpStatusCode.RequestTimeout) || response.StatusCode.Equals(HttpStatusCode.ServiceUnavailable))
                    {
                        if (i < maxAttempt && LogHelper.IsEnabled(EventLogLevel.Informational)) // logging exception details and that we will attempt to retry document retrieval
                            LogHelper.LogInformation(LogHelper.FormatInvariant(LogMessages.IDX20808, response.StatusCode, await response.Content.ReadAsStringAsync().ConfigureAwait(false), message.RequestUri));
                    }
                    else // if the exception type does not indicate the need to retry we should break
                    {
                        if (LogHelper.IsEnabled(EventLogLevel.Warning))
                            LogHelper.LogWarning(LogHelper.FormatInvariant(LogMessages.IDX20809, message.RequestUri, response.StatusCode, await response.Content.ReadAsStringAsync().ConfigureAwait(false)));

                        break;
                    }
                }
            }

            return response;
        }
    }
}
