// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Text.Json;
using Xunit;

namespace Microsoft.Deployment.DotNet.Releases.Tests
{
    public class TestBase : IAsyncLifetime
    {
        /// <summary>
        /// The path of the test directory.
        /// </summary>
        protected static string TestDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// A collection of all products.
        /// </summary>
        public ProductCollection Products
        {
            get;
            private set;
        }

        /// <summary>
        /// A dictionary containing all the releases for each product, indexed by the product version, e.g. "2.0".
        /// </summary>
        public Dictionary<string, ReadOnlyCollection<ProductRelease>> ProductReleases
        {
            get;
            private set;
        }

        public TestBase()
        {

        }

#if NET452
        public Task DisposeAsync() => Task.FromResult(0);
#else
        public Task DisposeAsync() => Task.CompletedTask;
#endif

        public async Task InitializeAsync()
        {
            Products = await ProductCollection.GetFromFileAsync(@"data\\releases-index.json", false).ConfigureAwait(false);
            ProductReleases = new Dictionary<string, ReadOnlyCollection<ProductRelease>>();

            foreach (Product product in Products)
            {
                string releasesJsonPath = Path.Combine("data", product.ProductVersion, "releases.json");

                ProductReleases[product.ProductVersion] = await product.GetReleasesAsync(releasesJsonPath, downloadLatest: false).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Creates a <see cref="Product"/> using the provided JSON.
        /// </summary>
        /// <param name="json">The raw JSON to deserialize.</param>
        /// <returns></returns>
        protected Product CreateProduct(string json)
        {
            return new Product(JsonDocument.Parse(json).RootElement);
        }

        /// <summary>
        /// Gets a specific <see cref="ProductRelease"/>.
        /// </summary>
        /// <param name="productVersion">The version of the product, e.g. "2.1" or "</param>
        /// <param name="releaseVersion">The version of the release, e.g. "3.1.7".</param>
        /// <returns>The <see cref="ProductRelease"/> or <see langword="null"/> if the release does not exist.</returns>
        protected ProductRelease GetProductRelease(string productVersion, string releaseVersion)
        {
            return GetProductRelease(productVersion, new ReleaseVersion(releaseVersion));
        }

        /// <summary>
        /// Gets a specific <see cref="ProductRelease"/>.
        /// </summary>
        /// <param name="productVersion">The version of the product, e.g. "2.1" or "</param>
        /// <param name="releaseVersion">The version of the release.</param>
        /// <returns>The <see cref="ProductRelease"/> or <see langword="null"/> if the release does not exist.</returns>
        protected ProductRelease GetProductRelease(string productVersion, ReleaseVersion releaseVersion)
        {
            return ProductReleases[productVersion].Where(r => r.Version == releaseVersion).FirstOrDefault();
        }
    }
}
