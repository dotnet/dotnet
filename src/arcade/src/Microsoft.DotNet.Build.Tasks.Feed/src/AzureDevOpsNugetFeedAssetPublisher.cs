// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using Azure.Core;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.ArcadeAzureIntegration;
using Microsoft.DotNet.Build.Tasks.Feed.Model;
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
            // Trim so a whitespace-only token is treated as "no token" and falls back to Entra auth
            // instead of throwing in CreateAzdoAuthHeader.
            _accessToken = accessToken?.Trim();
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
            };

            if (!string.IsNullOrEmpty(_accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = GeneralUtils.CreateAzdoAuthHeader(_accessToken);
            }
            else
            {
                // No token provided; acquire an Entra token via DefaultIdentityTokenCredential.
                try
                {
                    var credential = new DefaultIdentityTokenCredential(
                        new DefaultIdentityTokenCredentialOptions
                        {
                            ManagedIdentityClientId = task.ManagedIdentityClientId
                        });
                    var tokenRequestContext = new TokenRequestContext(new[] { "499b84ac-1321-427f-aa17-267ca6975798/.default" });
                    var token = credential.GetToken(tokenRequestContext, CancellationToken.None);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
                }
                catch (Exception e)
                {
                    // The constructor is throwing, so Dispose() will never be called on this instance;
                    // dispose the HttpClient here to avoid leaking the underlying handler/sockets on
                    // repeated failures (e.g. a misconfigured service connection).
                    _httpClient.Dispose();
                    throw new InvalidOperationException(
                        "Failed to acquire an Entra token for Azure DevOps feed publishing. Provide 'AzureDevOpsFeedsKey', " +
                        "or run the publish step under an AzureCLI@2 task with addSpnToEnvironment: true (or a configured " +
                        "managed/workload identity) so DefaultIdentityTokenCredential can obtain a token.", e);
                }
            }
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
