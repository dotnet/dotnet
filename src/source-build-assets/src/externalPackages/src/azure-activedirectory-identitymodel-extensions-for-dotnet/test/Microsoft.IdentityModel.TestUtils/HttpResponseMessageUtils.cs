// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Net;
using System.Net.Http;
using System.Text;

namespace Microsoft.IdentityModel.TestUtils
{
    /// <summary>
    /// Utility methods for constructing <see cref="HttpResponseMessage"/> objects for testing purposes.
    /// </summary>
    public static class HttpResponseMessageUtils
    {
        /// <summary>
        /// Creates a <see cref="HttpResponseMessage"/> with a <see cref="HttpStatusCode.OK"/> and the provided <paramref name="json"/>.
        /// </summary>
        /// <param name="json">The JSON string to return in the <see cref="HttpResponseMessage"/></param>
        /// <returns>A <see cref="HttpResponseMessage"/> with <see cref="HttpStatusCode.OK"/> and the provided <paramref name="json"/>.</returns>
        public static HttpResponseMessage CreateHttpResponseMessage(string json)
        {
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json, Encoding.UTF8, "application/json") };
        }

        /// <summary>
        /// Creates a <see cref="HttpResponseMessage"/> with <paramref name="statusCode"/> and <paramref name="json"/>.
        /// </summary>
        /// <param name="json">The JSON string to return in the <see cref="HttpResponseMessage"/>.</param>
        /// <param name="statusCode">The <see cref="HttpStatusCode"/> to return in the <see cref="HttpResponseMessage"/>.</param>
        /// <returns>A <see cref="HttpResponseMessage"/> with <paramref name="statusCode"/> and <paramref name="json"/>.</returns>
        public static HttpResponseMessage CreateHttpResponseMessage(string json, HttpStatusCode statusCode)
        {
            return new HttpResponseMessage(statusCode) { Content = new StringContent(json, Encoding.UTF8, "application/json") };
        }

        /// <summary>
        ///  Creates an HttpClient that returns <paramref name="json"/> with <see cref="HttpStatusCode.OK"/>.
        /// </summary>
        /// <param name="json">The JSON string that <see cref="HttpClient"/> will return in the <see cref="HttpResponseMessage"/>.</param>
        /// <returns>An HttpClient that returns <paramref name="json"/> with <see cref="HttpStatusCode.OK"/>.</returns>
        public static HttpClient SetupHttpClientThatReturns(string json)
        {
            using (var handler = new MockHttpMessageHandler(CreateHttpResponseMessage(json)))
                return new HttpClient(handler);
        }

        /// <summary>
        ///  Creates an HttpClient that returns <paramref name="json"/> with <paramref name="failureStatusCode"/> on the first call,
        ///  but <paramref name="json"/> with <see cref="HttpStatusCode.OK"/> on the second call.
        /// </summary>
        /// <param name="json">The JSON string that <see cref="HttpClient"/> will return in the <see cref="HttpResponseMessage"/>.</param>
        /// <param name="failureStatusCode">The <see cref="HttpStatusCode"/> that will be returned in the first call the <see cref="HttpClient"/> does.</param>
        /// <returns>An HttpClient that returns <paramref name="json"/> with <paramref name="failureStatusCode"/> on the first call,
        ///  but <paramref name="json"/> with <see cref="HttpStatusCode.OK"/> on the second call.</returns>
        /// <remarks>Created to test the 'refresh on network error' functionality added to <see cref="HttpDocumentRetriever"/>.</remarks>
        public static HttpClient SetupHttpClientThatReturns(string json, HttpStatusCode failureStatusCode)
        {
            using (var handler = new MockHttpMessageHandler(CreateHttpResponseMessage(json), CreateHttpResponseMessage(json, failureStatusCode)))
                return new HttpClient(handler);
        }
    }
}
