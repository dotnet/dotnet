// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Utility;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    public class AutoCompleteResourceV2Feed : AutoCompleteResource
    {
        private readonly HttpSource _httpSource;
        private readonly Uri _baseUri;
        private readonly IEnvironmentVariableReader _environmentVariableReader;

        public AutoCompleteResourceV2Feed(HttpSourceResource httpSourceResource, string baseAddress, Configuration.PackageSource packageSource)
            : this(httpSourceResource, baseAddress, packageSource, environmentVariableReader: null)
        {
        }

        internal AutoCompleteResourceV2Feed(HttpSourceResource httpSourceResource, string baseAddress, Configuration.PackageSource packageSource, IEnvironmentVariableReader environmentVariableReader)
        {
            if (httpSourceResource == null)
            {
                throw new ArgumentNullException(nameof(httpSourceResource));
            }

            if (packageSource == null)
            {
                throw new ArgumentNullException(nameof(packageSource));
            }

            _httpSource = httpSourceResource.HttpSource;
            _baseUri = UriUtility.CreateSourceUri($"{baseAddress}/");
            _environmentVariableReader = environmentVariableReader;
        }

        public override async Task<IEnumerable<string>> IdStartsWith(
            string packageIdPrefix,
            bool includePrerelease,
            Common.ILogger log,
            CancellationToken token)
        {
            var apiEndpointUri = new UriBuilder(new Uri(_baseUri, @"package-ids"))
            {
                Query = $"partialId={packageIdPrefix}&includePrerelease={includePrerelease}&semVerLevel=2.0.0"
            };

            return await GetResults(apiEndpointUri.Uri, log, token);
        }

        public override async Task<IEnumerable<NuGetVersion>> VersionStartsWith(
            string packageId,
            string versionPrefix,
            bool includePrerelease,
            SourceCacheContext sourceCacheContext,
            Common.ILogger log,
            CancellationToken token)
        {
            var apiEndpointUri = new UriBuilder(new Uri(_baseUri, @"package-versions/" + packageId))
            {
                Query = $"includePrerelease={includePrerelease}&semVerLevel=2.0.0"
            };

            var results = await GetResults(apiEndpointUri.Uri, log, token);
            var versions = results.ToList();
            versions = versions
                .Where(item => item.StartsWith(versionPrefix, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return versions.Select(item => NuGetVersion.Parse(item));
        }

        private async Task<IEnumerable<string>> GetResults(
            Uri apiEndpointUri,
            Common.ILogger logger,
            CancellationToken token)
        {
            return await _httpSource.ProcessStreamAsync(
                new HttpSourceRequest(apiEndpointUri, logger),
                async stream =>
                {
                    if (NuGetFeatureFlags.UseSystemTextJsonDeserializationFeatureSwitch
                        || NuGetFeatureFlags.IsSystemTextJsonDeserializationEnabledByEnvironment(_environmentVariableReader))
                    {
                        var seekableStream = await stream.AsSeekableStreamAsync(token);
                        return await System.Text.Json.JsonSerializer.DeserializeAsync(seekableStream, JsonContext.Default.StringArray, token);
                    }
                    else
                    {
                        using var reader = new StreamReader(await stream.AsSeekableStreamAsync(token));
                        using var jsonReader = new JsonTextReader(reader);
                        var serializer = Newtonsoft.Json.JsonSerializer.Create();
                        return serializer.Deserialize<string[]>(jsonReader);
                    }
                },
                logger,
                token);
        }
    }
}
