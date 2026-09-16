// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    /// <summary>
    /// Provides operations for uploading packages to a V3 package staging service.
    /// </summary>
    public sealed class PackageStagingResourceV3 : INuGetResource
    {
        private readonly Uri _endpoint;
        private readonly HttpSource _httpSource;

        internal PackageStagingResourceV3(Uri endpoint, HttpSource httpSource)
        {
            _endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            _httpSource = httpSource ?? throw new ArgumentNullException(nameof(httpSource));
        }

        /// <summary>
        /// Gets the exact package staging endpoint advertised by the service index.
        /// </summary>
        public Uri SourceUri => _endpoint;

        /// <summary>
        /// Uploads a package to the staging service.
        /// </summary>
        public Task PushPackageAsync(
            string packagePath,
            string? apiKey,
            string? groupId,
            TimeSpan requestTimeout,
            bool allowInsecureConnections,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            return PushAsync(
                packagePath: packagePath,
                apiKey: apiKey,
                groupId: groupId,
                requestTimeout: requestTimeout,
                route: "package",
                formFieldName: "package",
                allowInsecureConnections: allowInsecureConnections,
                logger: logger,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Uploads a symbol package to the staging service.
        /// </summary>
        public Task PushSymbolsAsync(
            string packagePath,
            string? apiKey,
            string? groupId,
            TimeSpan requestTimeout,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            return PushSymbolsAsync(
                packagePath: packagePath,
                apiKey: apiKey,
                groupId: groupId,
                requestTimeout: requestTimeout,
                allowInsecureConnections: false,
                logger: logger,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Uploads a symbol package to the staging service.
        /// </summary>
        public Task PushSymbolsAsync(
            string packagePath,
            string? apiKey,
            string? groupId,
            TimeSpan requestTimeout,
            bool allowInsecureConnections,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            return PushAsync(
                packagePath: packagePath,
                apiKey: apiKey,
                groupId: groupId,
                requestTimeout: requestTimeout,
                route: "symbols",
                formFieldName: "symbols",
                allowInsecureConnections: allowInsecureConnections,
                logger: logger,
                cancellationToken: cancellationToken);
        }

        private async Task PushAsync(
            string packagePath,
            string? apiKey,
            string? groupId,
            TimeSpan requestTimeout,
            string route,
            string formFieldName,
            bool allowInsecureConnections,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            var requestUriBuilder = new UriBuilder(_endpoint)
            {
                Path = _endpoint.AbsolutePath.TrimEnd('/') + "/" + route,
            };
            Uri requestUri = requestUriBuilder.Uri;
            if (requestUri.Scheme == Uri.UriSchemeHttp && !allowInsecureConnections)
            {
                throw new FatalProtocolException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.Error_Insecure_HTTP,
                    _httpSource.PackageSource,
                    requestUri));
            }

            var request = new HttpSourceRequest(
                () => CreateRequest(requestUri, packagePath, apiKey, groupId, formFieldName, logger))
            {
                RequestTimeout = requestTimeout,
            };

            await _httpSource.ProcessResponseAsync(
                request,
                response =>
                {
                    response.EnsureSuccessStatusCode();

                    return TaskResult.Zero;
                },
                logger,
                cancellationToken);
        }

        private static HttpRequestMessage CreateRequest(
            Uri requestUri,
            string packagePath,
            string? apiKey,
            string? groupId,
            string formFieldName,
            ILogger logger)
        {
            var request = HttpRequestMessageFactory.Create(
                HttpMethod.Put,
                requestUri,
                new HttpRequestMessageConfiguration(logger, promptOn403: string.IsNullOrEmpty(apiKey)));
            var content = new MultipartFormDataContent();
            var fileStream = new FileStream(packagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            content.Add(fileContent, formFieldName, Path.GetFileName(packagePath));

            if (groupId is not null)
            {
                content.Add(new StringContent(groupId), "groupId");
            }

            request.Content = content;
            request.Headers.TransferEncodingChunked = true;

            if (!string.IsNullOrEmpty(apiKey))
            {
                request.Headers.Add(ProtocolConstants.ApiKeyHeader, apiKey);
            }

            return request;
        }
    }
}
