// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Protocols.WsFederation
{
    /// <summary>
    ///  Retrieves a populated <see cref="WsFederationConfiguration"/> given an address.
    /// </summary>
    public class WsFederationConfigurationRetriever : IConfigurationRetriever<WsFederationConfiguration>
    {
        private static readonly XmlReaderSettings SafeSettings = new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Prohibit, ValidationType = ValidationType.None };

        /// <summary>
        /// Retrieves a populated <see cref="WsFederationConfiguration"/> given an address.
        /// </summary>
        /// <param name="address">address of the metadata document.</param>
        /// <param name="cancel"><see cref="CancellationToken"/>.</param>
        /// <returns>A populated <see cref="WsFederationConfiguration"/> instance.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="address"/> is null or empty.</exception>
        public static Task<WsFederationConfiguration> GetAsync(string address, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(address))
                throw LogArgumentNullException(nameof(address));

            return GetAsync(address, new HttpDocumentRetriever(), cancel);
        }

        /// <summary>
        /// Retrieves a populated <see cref="WsFederationConfiguration"/> given an address and an <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="address">address of the metadata document.</param>
        /// <param name="httpClient">the <see cref="HttpClient"/> to use to read the metadata document.</param>
        /// <param name="cancel">a <see cref="CancellationToken"/>.</param>
        /// <returns>A populated <see cref="WsFederationConfiguration"/> instance.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="address"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="httpClient"/> is null.</exception>
        public static Task<WsFederationConfiguration> GetAsync(string address, HttpClient httpClient, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(address))
                throw LogArgumentNullException(nameof(address));

            if (httpClient == null)
                throw LogArgumentNullException(nameof(httpClient));

            return GetAsync(address, new HttpDocumentRetriever(httpClient), cancel);
        }

        /// <inheritdoc/>
        public Task<WsFederationConfiguration> GetConfigurationAsync(string address, IDocumentRetriever retriever, CancellationToken cancel)
        {
            return GetAsync(address, retriever, cancel);
        }

        /// <summary>
        /// Retrieves a populated <see cref="WsFederationConfiguration"/> given an address and an <see cref="IDocumentRetriever"/>.
        /// </summary>
        /// <param name="address">address of the metadata document.</param>
        /// <param name="retriever">the <see cref="IDocumentRetriever"/> to use to read the metadata document</param>
        /// <param name="cancel"><see cref="CancellationToken"/>.</param>
        /// <returns>A populated <see cref="WsFederationConfiguration"/> instance.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="address"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="retriever"/> is null.</exception>
        public static async Task<WsFederationConfiguration> GetAsync(string address, IDocumentRetriever retriever, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(address))
                throw LogArgumentNullException(nameof(address));

            if (retriever == null)
                throw LogArgumentNullException(nameof(retriever));

            string document = await retriever.GetDocumentAsync(address, cancel).ConfigureAwait(false);

            using (var metaDataReader = XmlReader.Create(new StringReader(document), SafeSettings))
            {
                return (new WsFederationMetadataSerializer()).ReadMetadata(metaDataReader);
            }
        }
    }
}
