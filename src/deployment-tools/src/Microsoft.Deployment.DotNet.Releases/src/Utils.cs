// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;

#pragma warning disable CA1810

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// Utility and helper methods.
    /// </summary>
    internal class Utils
    {
        internal static readonly HttpClient s_httpClient;

        /// <summary>
        /// Determines if a local file is the latest version compared to an online copy.
        /// </summary>
        /// <param name="fileName">The path of the local file.</param>
        /// <param name="address">The address pointing of the file.</param>
        /// <returns><see langword="true"/> if the local file is the latest; <see langword="false"/> otherwise.</returns>
        internal static async Task<bool> IsLatestFileAsync(string fileName, Uri address)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Head, address);
            HttpResponseMessage httpResponse = await s_httpClient.SendAsync(httpRequest).ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();

            DateTime? onlineLastModified = httpResponse.Content.Headers.LastModified?.DateTime;
            var fileInfo = new FileInfo(fileName);

            return fileInfo.LastWriteTime >= onlineLastModified;
        }

        /// <summary>
        /// Downloads or copy a file from the specified address to the specified destination.
        /// </summary>
        /// <param name="address">The address of the source file to download or copy.</param>
        /// <param name="fileName">The path of the destination file.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        internal static async Task DownloadFileAsync(Uri address, string fileName)
        {
            string directory = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (address.Scheme == Uri.UriSchemeFile)
            {
                using Stream source = File.Open(address.LocalPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using Stream destination = File.Create(fileName);
                await source.CopyToAsync(destination).ConfigureAwait(false);
            }
            else
            {
                HttpResponseMessage httpResponse = await s_httpClient.GetAsync(address).ConfigureAwait(false);
                httpResponse.EnsureSuccessStatusCode();

                using FileStream stream = File.Create(fileName);
                await httpResponse.Content.CopyToAsync(stream).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Computes the hash for the specified file using the specified hash algorithm.
        /// </summary>
        /// <param name="fileName">The path, including the filename and extension of the file to use.</param>
        /// <param name="hashAlgorithm">The hash algorithm to use.</param>
        /// <returns>A string containing the file hash.</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException" />
        /// <exception cref="FileNotFoundException" />
        internal static string GetFileHash(string fileName, HashAlgorithm hashAlgorithm)
        {
            if (fileName is null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (fileName == string.Empty)
            {
                throw new ArgumentException(ReleasesResources.ValueCannotBeEmpty, nameof(fileName));
            }

            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException(nameof(hashAlgorithm));
            }

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(string.Format(ReleasesResources.FileNotFound, fileName));
            }

            using FileStream stream = File.OpenRead(fileName);
            byte[] checksum = hashAlgorithm.ComputeHash(stream);

            return BitConverter.ToString(checksum).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// Checks whether a specified file exists, and if not, optionally downloads a copy from
        /// the specified address.
        /// </summary>
        /// <param name="path">The path of the local file to check.</param>
        /// <param name="downloadLatest">When <see langword="true"/>, the latest copy of the file is downloaded if a newer version
        /// exists online.</param>
        /// <param name="address">The address of the file to download.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        internal static async Task GetLatestFileAsync(string path, bool downloadLatest, Uri address)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (address is null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (path == string.Empty)
            {
                throw new ArgumentException(ReleasesResources.ValueCannotBeEmpty, nameof(path));
            }

            if (!File.Exists(path))
            {
                if (!downloadLatest)
                {
                    throw new FileNotFoundException(string.Format(ReleasesResources.FileNotFound, path));
                }

                await DownloadFileAsync(address, path).ConfigureAwait(false);
            }
            else if (downloadLatest && !await IsLatestFileAsync(path, address).ConfigureAwait(false))
            {
                await DownloadFileAsync(address, path).ConfigureAwait(false);
            }
        }

        static Utils()
        {
            s_httpClient = new HttpClient();
            s_httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true
            };
        }
    }
}
