//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{

    /// <summary>
    ///  Retrieves a populated <see cref="OpenIdConnectConfiguration"/> given an address.
    /// </summary>
    public class OpenIdConnectConfigurationRetriever : IConfigurationRetriever<OpenIdConnectConfiguration>
    {

        /// <summary>
        /// Retrieves a populated <see cref="OpenIdConnectConfiguration"/> given an address.
        /// </summary>
        /// <param name="address">address of the discovery document.</param>
        /// <param name="cancel"><see cref="CancellationToken"/>.</param>
        /// <returns>A populated <see cref="OpenIdConnectConfiguration"/> instance.</returns>
        public static Task<OpenIdConnectConfiguration> GetAsync(string address, CancellationToken cancel)
        {
            return GetAsync(address, new HttpDocumentRetriever(), cancel);
        }

        /// <summary>
        /// Retrieves a populated <see cref="OpenIdConnectConfiguration"/> given an address and an <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="address">address of the discovery document.</param>
        /// <param name="httpClient">the <see cref="HttpClient"/> to use to read the discovery document.</param>
        /// <param name="cancel"><see cref="CancellationToken"/>.</param>
        /// <returns>A populated <see cref="OpenIdConnectConfiguration"/> instance.</returns>
        public static Task<OpenIdConnectConfiguration> GetAsync(string address, HttpClient httpClient, CancellationToken cancel)
        {
            return GetAsync(address, new HttpDocumentRetriever(httpClient), cancel);
        }

        Task<OpenIdConnectConfiguration> IConfigurationRetriever<OpenIdConnectConfiguration>.GetConfigurationAsync(string address, IDocumentRetriever retriever, CancellationToken cancel)
        {
            return GetAsync(address, retriever, cancel);
        }

        /// <summary>
        /// Retrieves a populated <see cref="OpenIdConnectConfiguration"/> given an address and an <see cref="IDocumentRetriever"/>.
        /// </summary>
        /// <param name="address">address of the discovery document.</param>
        /// <param name="retriever">the <see cref="IDocumentRetriever"/> to use to read the discovery document</param>
        /// <param name="cancel"><see cref="CancellationToken"/>.</param>
        /// <returns>A populated <see cref="OpenIdConnectConfiguration"/> instance.</returns>
        public static async Task<OpenIdConnectConfiguration> GetAsync(string address, IDocumentRetriever retriever, CancellationToken cancel)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw LogHelper.LogArgumentNullException(nameof(address));

            if (retriever == null)
            {
                throw LogHelper.LogArgumentNullException(nameof(retriever));
            }

            string doc = await retriever.GetDocumentAsync(address, cancel).ConfigureAwait(false);

            LogHelper.LogVerbose(LogMessages.IDX21811, doc);
            OpenIdConnectConfiguration openIdConnectConfiguration = JsonConvert.DeserializeObject<OpenIdConnectConfiguration>(doc);
            if (!string.IsNullOrEmpty(openIdConnectConfiguration.JwksUri))
            {
                LogHelper.LogVerbose(LogMessages.IDX21812, openIdConnectConfiguration.JwksUri);
                string keys = await retriever.GetDocumentAsync(openIdConnectConfiguration.JwksUri, cancel).ConfigureAwait(false);

                LogHelper.LogVerbose(LogMessages.IDX21813, openIdConnectConfiguration.JwksUri);
                openIdConnectConfiguration.JsonWebKeySet = JsonConvert.DeserializeObject<JsonWebKeySet>(keys);
                foreach (SecurityKey key in openIdConnectConfiguration.JsonWebKeySet.GetSigningKeys())
                {
                    openIdConnectConfiguration.SigningKeys.Add(key);
                }
            }

            return openIdConnectConfiguration;
        }
    }
}
