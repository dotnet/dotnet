// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Feed.Model;
#if !NET472_OR_GREATER
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class AzureDevOpsNugetFeedAssetPublisher : IAssetPublisher, IDisposable
    {
        private readonly TaskLoggingHelper _log;
        private readonly string _targetUrl;
        private readonly string _accessToken;
        private readonly PublishArtifactsInManifestBase _task;
        private readonly string _feedAccount;
        private readonly string _feedVisibility;
        private readonly string _feedName;
        private readonly HttpClient _httpClient;

        public AzureDevOpsNugetFeedAssetPublisher(TaskLoggingHelper log, string targetUrl, string accessToken, PublishArtifactsInManifestBase task)
        {
            _log = log;
            _targetUrl = targetUrl;
            _accessToken = accessToken;
            _task = task;

            var parsedUri = Regex.Match(_targetUrl, PublishingConstants.AzDoNuGetFeedPattern);
            if (!parsedUri.Success)
            {
                throw new ArgumentException(
                    $"Azure DevOps NuGetFeed was not in the expected format '{PublishingConstants.AzDoNuGetFeedPattern}'");
            }
            _feedAccount = parsedUri.Groups["account"].Value;
            _feedVisibility = parsedUri.Groups["visibility"].Value;
            _feedName = parsedUri.Groups["feed"].Value;

            _httpClient = new HttpClient(new HttpClientHandler {CheckCertificateRevocationList = true})
            {
                Timeout = GeneralUtils.NugetFeedPublisherHttpClientTimeout,
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue(
                        "Basic",
                        Convert.ToBase64String(Encoding.ASCII.GetBytes($":{_accessToken}")))
                },
            };
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public LocationType LocationType => LocationType.NugetFeed;

        public async Task PublishAssetAsync(string file, string blobPath, PushOptions options, SemaphoreSlim clientThrottle = null)
        {
            if (!file.EndsWith(GeneralUtils.PackageSuffix, StringComparison.OrdinalIgnoreCase))
            {
                _log.LogWarning(
                    $"AzDO feed publishing not available for blobs. Blob '{file}' was not published.");
                return;
            }

            string id;
            string version;
            using (var packageReader = new PackageArchiveReader(file))
            {
                PackageIdentity packageIdentity = packageReader.GetIdentity();
                id = packageIdentity.Id;
                version = packageIdentity.Version.ToString();
            }

            try
            {
                var config = new TargetFeedConfig(default, _targetUrl, default, default, default, default, default);
                await _task.PushNugetPackageAsync(config, _httpClient, file, id, version, _feedAccount, _feedVisibility, _feedName);
            }
            catch (Exception e)
            {
                _log.LogErrorFromException(e);
            }
        }
    }
}
#else
public class AzureDevOpsNugetFeedAssetPublisher : Task
{
    public override bool Execute() => throw new NotSupportedException("AzureDevOpsNugetFeedAssetPublisher depends on ProductConstructionService.Client, which has discontinued support for desktop frameworks.");
}
#endif
