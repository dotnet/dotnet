// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// A collection of all released products.
    /// </summary>
    public sealed class ProductCollection : ReadOnlyCollection<Product>
    {
        /// <summary>
        /// The default URL of the releases index file.
        /// </summary>
        public static readonly Uri ReleasesIndexDefaultUrl = new Uri("https://builds.dotnet.microsoft.com/dotnet/release-metadata/releases-index.json");

        /// <summary>
        /// Creates a new <see cref="ProductCollection"/> instance.
        /// </summary>
        /// <param name="productList">The list of products to include.</param>
        private ProductCollection(IList<Product> productList) : base(productList)
        {
        }

        /// <summary>
        /// Gets an enumerable of all the support phases across all products.
        /// </summary>
        public IEnumerable<SupportPhase> GetSupportPhases()
        {
            return this.Select(p => p.SupportPhase).Distinct();
        }

        /// <summary>
        /// Creates a new collection of all released products using the default URL for the releases index file.
        /// </summary>
        /// <returns>A collection of products described in the releases index file.</returns>
        public static async Task<ProductCollection> GetAsync()
        {
            return await GetAsync(ReleasesIndexDefaultUrl).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new collection of all released products using the provided URL for the releases index file.
        /// </summary>
        /// <param name="releasesIndexUri">A string containing the URL pointing to the releases index file.</param>
        /// <returns>A collection of products described in the releases index file.</returns>
        public static async Task<ProductCollection> GetAsync(string releasesIndexUri)
        {
            if (releasesIndexUri is null)
            {
                throw new ArgumentNullException(nameof(releasesIndexUri));
            }

            if (releasesIndexUri == string.Empty)
            {
                throw new ArgumentException(ReleasesResources.ValueCannotBeEmpty, nameof(releasesIndexUri));
            }

            return await GetAsync(new Uri(releasesIndexUri)).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new collection of all released products using the provided URL for the releases index file.
        /// </summary>
        /// <param name="releasesIndexUrl">A URL pointing to the releases index file.</param>
        /// <returns>A collection of products described in releases index file.</returns>
        public static async Task<ProductCollection> GetAsync(Uri releasesIndexUrl)
        {
            if (releasesIndexUrl == null)
            {
                throw new ArgumentNullException(nameof(releasesIndexUrl));
            }

            using var stream = new MemoryStream(await Utils.s_httpClient.GetByteArrayAsync(releasesIndexUrl).ConfigureAwait(false));
            using var reader = new StreamReader(stream);

            return await GetAsync(reader).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new <see cref="ProductCollection"/> using the specified file containing the releases index. If 
        /// the file does not exist or is outdated, a newer copy can optionally be downloaded. If the file exist and a
        /// newer copy is available it will replace the existing local copy.
        /// </summary>
        /// <param name="path">The path of the releases index file.</param>
        /// <param name="downloadLatest">When <see langword="true"/>, if the local copy of the index is
        /// outdated, or does not exist, a new copy is downloaded, replacing the local copy before processing the file.
        /// Otherwise, the local copy is used.</param>
        /// <returns>A collection of all products described by the index.</returns>
        /// <exception cref="FileNotFoundException">If <paramref name="downloadLatest"/> is <see langword="false"/> and 
        /// <paramref name="path"/> does not exist.
        /// </exception>
        public static async Task<ProductCollection> GetFromFileAsync(string path, bool downloadLatest)
        {
            await Utils.GetLatestFileAsync(path, downloadLatest, ReleasesIndexDefaultUrl).ConfigureAwait(false);

            using TextReader reader = File.OpenText(path);

            return await GetAsync(reader).ConfigureAwait(false);
        }

        private static async Task<ProductCollection> GetAsync(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            using var releasesIndexDocument = JsonDocument.Parse(await reader.ReadToEndAsync().ConfigureAwait(false));
            var root = releasesIndexDocument.RootElement.GetProperty("releases-index");
            var products = new List<Product>();

            using JsonElement.ArrayEnumerator enumerator = root.EnumerateArray();

            while (enumerator.MoveNext())
            {
                products.Add(new(enumerator.Current));
            }

            return new ProductCollection(products);
        }
    }
}
