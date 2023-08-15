// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// Provides an overview of a single product, including information related to its support level and the latest SDK and runtime releases.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Product
    {
        private string DebuggerDisplay => $"{ProductName} {ProductVersion} ({SupportPhase})";

        /// <summary>
        /// The version of the product, e.g "5.0" or "1.1".
        /// </summary>
        public string ProductVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// The end-of-life (EOL) date for this <see cref="Product"/> when it is considered to be out of support. The value 
        /// may be <see langword="null" /> if the EOL date is undetermined, e.g. when a product is still a prerelease.
        /// </summary>
        public DateTime? EndOfLifeDate
        {
            get;
            private set;
        }

        /// <summary>
        /// <see langword="True"/> if the latest release of the product contained a security update;
        /// <see langword="false"/> otherwise.
        /// </summary>
        public bool LatestReleaseIncludesSecurityUpdate
        {
            get;
            private set;
        }

        /// <summary>
        /// The date of the latest release for this product.
        /// </summary>
        public DateTime LatestReleaseDate
        {
            get;
            private set;
        }

        /// <summary>
        /// The version of the latest release.
        /// </summary>
        public ReleaseVersion LatestReleaseVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// The version of the runtime included in the latest release.
        /// </summary>
        public ReleaseVersion LatestRuntimeVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// The version of the SDK included in the latest release.
        /// </summary>
        /// <remarks>
        /// This is usually the SDK with the highest feature band. A <see cref="ProductRelease"/>
        /// may include multiple SDKs across different feature bands, all of which carry the same runtime version.
        /// </remarks>
        public ReleaseVersion LatestSdkVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// The name of the product.
        /// </summary>
        public string ProductName
        {
            get;
            private set;
        }

        /// <summary>
        /// The URL pointing to the releases.json file that contains information about all the releases 
        /// associated with this <see cref="Product"/>.
        /// </summary>
        public Uri ReleasesJson
        {
            get;
            private set;
        }

        /// <summary>
        /// A value indicating the support duration of the product.
        /// </summary>
        public ReleaseType ReleaseType
        {
            get;
            private set;
        } = ReleaseType.Unknown;

        /// <summary>
        /// The current support phase of this <see cref="Product"/>. For an LTS release, the <see cref="EndOfLifeDate"/> property should 
        /// be checked to confirm whether a release is still supported.
        /// </summary>
        /// <remarks>
        /// The EOL dates are often published in advance, but there can be delays to updating the support phase in the published
        /// data.
        /// </remarks>
        public SupportPhase SupportPhase
        {
            get;
            private set;
        } = SupportPhase.Unknown;

        /// <summary>
        /// <see langword="true"/> if the support phase is not <see cref="SupportPhase.EOL"/>
        /// and the current date is less than the EOL date of the product, 
        /// <see langword="false"/> otherwise.
        /// </summary>
        /// <returns><see langword="true"/> if the product is currently supported; <see langword="false"/> otherwise.</returns>
        public bool IsSupported => !IsOutOfSupport();

        /// <summary>
        /// Gets a collection of all releases associated with this <see cref="Product"/>.
        /// </summary>
        /// <returns>A collection of all releases for this product.</returns>
        public Task<ReadOnlyCollection<ProductRelease>> GetReleasesAsync() =>
            GetReleasesAsync(ReleasesJson);

        internal Product(JsonElement productElement)
        {
            ReleaseType = productElement.TryGetProperty("release-type", out JsonElement value) ?
                Enum.TryParse(value.GetString(), ignoreCase: true, out ReleaseType releaseType) ? releaseType : ReleaseType.Unknown :
                ReleaseType.Unknown;

            SupportPhase = productElement.TryGetProperty("support-phase", out value) ?
                Enum.TryParse(value.GetString()?.Replace("-", ""), ignoreCase: true, out SupportPhase supportPhase) ? supportPhase : SupportPhase.Unknown :
                SupportPhase.Unknown;

            if (productElement.TryGetProperty("eol-date", out value))
            {
                EndOfLifeDate = value.ValueKind == JsonValueKind.Null ? null : value.GetDateTime();
            }

            LatestReleaseDate = productElement.GetProperty("latest-release-date").GetDateTime();
            LatestReleaseVersion = productElement.GetReleaseVersionOrDefault("latest-release");
            LatestReleaseIncludesSecurityUpdate = productElement.GetProperty("security").GetBoolean();
            LatestRuntimeVersion = productElement.GetReleaseVersionOrDefault("latest-runtime");
            LatestSdkVersion = productElement.GetReleaseVersionOrDefault("latest-sdk");
            ProductName = productElement.GetStringOrDefault("product");
            ProductVersion = productElement.GetStringOrDefault("channel-version");
            ReleasesJson = productElement.GetUriOrDefault("releases.json");
        }

        /// <summary>
        /// Gets a collection of all releases associated with this <see cref="Product"/> using a file
        /// containing the releases data.
        /// </summary>
        /// <param name="path">The path of the file containing the releases data.</param>
        /// <param name="downloadLatest">When <see langword="true"/>, the latest copy of the releases data is used
        /// if the online version is newer than the local file copy.</param>
        /// <returns>A collection of releases associated with this <see cref="Product"/>.</returns>
        public async Task<ReadOnlyCollection<ProductRelease>> GetReleasesAsync(string path, bool downloadLatest)
        {
            await Utils.GetLatestFileAsync(path, downloadLatest, ReleasesJson).ConfigureAwait(false);

            using TextReader reader = File.OpenText(path);

            return await GetReleasesAsync(reader, this).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new <see cref="ProductRelease"/> collection using the releases.json file pointed to
        /// by the provided URL.
        /// </summary>
        /// <param name="address">The URL pointing to the releases.json file to use.</param>
        /// <returns>A collection of releases associated with this <see cref="Product"/>.</returns>
        public async Task<ReadOnlyCollection<ProductRelease>> GetReleasesAsync(Uri address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            using var stream = new MemoryStream(await Utils.s_httpClient.GetByteArrayAsync(address).ConfigureAwait(false));
            using var reader = new StreamReader(stream);

            return await GetReleasesAsync(reader, this).ConfigureAwait(false);
        }

        /// <summary>
        /// <see langword="true"/> if the support phase is <see cref="SupportPhase.EOL"/>
        /// or the current date is greater than or equal to the EOL date of the product, 
        /// <see langword="false"/> otherwise.
        /// </summary>
        /// <returns><see langword="true"/> if the product is out of support; <see langword="false"/> otherwise.</returns>
        public bool IsOutOfSupport()
        {
            return SupportPhase == SupportPhase.EOL || EndOfLifeDate?.Date <= DateTime.Now.Date;
        }

        /// <summary>
        /// Gets a collection of all releases defined in the specified file.
        /// </summary>
        /// <param name="path">The path of the file containing the releases data.</param>
        /// <returns>A collection of releases. The releases are not linked to a specific <see cref="Product"/>.</returns>
        public static async Task<ReadOnlyCollection<ProductRelease>> GetReleasesAsync(string path)
        {
            using TextReader reader = File.OpenText(path);

            return await GetReleasesAsync(reader, null).ConfigureAwait(false);
        }

        private static async Task<ReadOnlyCollection<ProductRelease>> GetReleasesAsync(TextReader reader, Product product)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            using var releasesDocument = JsonDocument.Parse(await reader.ReadToEndAsync().ConfigureAwait(false));
            JsonElement root = releasesDocument.RootElement;
            var releases = new List<ProductRelease>();
            var enumerator = root.GetProperty("releases").EnumerateArray();

            while (enumerator.MoveNext())
            {
                releases.Add(new ProductRelease(enumerator.Current, product));
            }

            return new ReadOnlyCollection<ProductRelease>(releases);
        }
    }
}
