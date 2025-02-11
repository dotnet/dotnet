// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Arcade.Common;
using Microsoft.Build.Framework;
using MSBuild = Microsoft.Build.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NuGet.Versioning;
using System.Text.RegularExpressions;

namespace Microsoft.DotNet.Helix.Sdk
{
    public class FindDotNetCliPackage : MSBuildTaskBase
    {
        // Use lots of retries since an Http Client failure here means failure to send to Helix
        private ExponentialRetry _retry = new ExponentialRetry()
        {
            MaxAttempts = 10,
            DelayBase = 3.0
        };

        /// <summary>
        ///   'LTS' or 'Current'
        /// </summary>
        [Required]
        public string Channel { get; set; }

        /// <summary>
        ///   'latest' or specific version
        /// </summary>
        [Required]
        public string Version { get; set; }

        /// <summary>
        ///   RID of dotnet cli to get
        /// </summary>
        [Required]
        public string Runtime { get; set; }

        /// <summary>
        ///   'sdk', 'runtime' or 'aspnetcore-runtime' (default is runtime)
        /// </summary>
        [Required]
        public string PackageType { get; set; }

        public ITaskItem [] AdditionalFeeds { get; set; }

        [Output]
        public string PackageUri { get; set; }

        private static HttpClient _client;
        private HttpMessageHandler _httpMessageHandler;

        public override void ConfigureServices(IServiceCollection collection)
        {
            _httpMessageHandler = new HttpClientHandler {  CheckCertificateRevocationList = true };
            collection.TryAddSingleton(_httpMessageHandler);
            collection.TryAddSingleton(Log);
        }

        public bool ExecuteTask(HttpMessageHandler httpMessageHandler)
        {
            _httpMessageHandler = httpMessageHandler;
            _client = new HttpClient(_httpMessageHandler); // lgtm [cs/httpclient-checkcertrevlist-disabled] False positive; see above in ConfigureServices(); this is always created with CheckCertificateRevocationList = true
            FindCliPackage().GetAwaiter().GetResult();
            return !Log.HasLoggedErrors;
        }

        private string SanitizeString(string text)
        {
            return Regex.Replace(text, @"\?[^ ]+", "");
        }

        private async Task FindCliPackage()
        {
            NormalizeParameters();
            var feeds = new List<ITaskItem>();
            feeds.Add(new MSBuild.TaskItem("https://dotnetcli.blob.core.windows.net/dotnet"));
            feeds.Add(new MSBuild.TaskItem("https://ci.dot.net/public"));
            if (AdditionalFeeds != null)
            {
                feeds.AddRange(AdditionalFeeds);
            }

            string finalDownloadUrl = null;
            foreach (var feed in feeds)
            {
                string downloadUrl = await GetDownloadUrlAsync(feed);

                if (downloadUrl == null)
                {
                    Log.LogMessage($"Could not retrieve dotnet cli {PackageType} version {Version} package uri from feed {feed}");
                    continue;
                }

                Log.LogMessage($"Retrieved dotnet cli {PackageType} version {Version} package uri {SanitizeString(downloadUrl)} from feed {feed}, testing...");

                try
                {
                    using HttpResponseMessage res = await HeadRequestWithRetry(downloadUrl);

                    if (res.StatusCode == HttpStatusCode.NotFound)
                    {
                        // 404 means that we successfully hit the server, and it returned 404. This cannot be a network hiccup
                        Log.LogMessage($"Unable to find dotnet cli {PackageType} version {Version} from feed {feed}");
                        continue;
                    }

                    res.EnsureSuccessStatusCode();

                    finalDownloadUrl = downloadUrl;
                }
                catch (Exception ex)
                {
                    Log.LogMessage($"Unable to access dotnet cli {PackageType} version {Version} from feed {feed}, {SanitizeString(ex.Message)}");
                }
            }

            if (finalDownloadUrl == null)
            {
                Log.LogError(FailureCategory.Build, $"Unable to find dotnet cli {PackageType} version {Version} from any of the specified feeds.");
            }


            if (!Log.HasLoggedErrors)
            {
                Log.LogMessage($"Url {SanitizeString(finalDownloadUrl)} is valid.");
                PackageUri = finalDownloadUrl;
            }
        }

        private async Task<string> GetDownloadUrlAsync(ITaskItem feed)
        {
            var oldVersion = Version; // ResolveVersionAsync will adjust the Version property, but we need it set back for other feeds to see the same initial Version
            try
            {
                var version = await ResolveVersionAsync(feed);
                string extension = Runtime.StartsWith("win") ? "zip" : "tar.gz";

                string effectiveVersion = await GetEffectiveVersion(feed, version);
                feed.TryGetMetadata("SasToken", out string sasToken);
                if (!string.IsNullOrEmpty(sasToken) && !sasToken.StartsWith("?"))
                {
                    sasToken = "?" + sasToken;
                }

                return PackageType switch
                {
                    "sdk" => $"{feed.ItemSpec}/Sdk/{version}/dotnet-sdk-{effectiveVersion}-{Runtime}.{extension}{sasToken}",
                    "aspnetcore-runtime" =>
                        $"{feed.ItemSpec}/aspnetcore/Runtime/{version}/aspnetcore-runtime-{effectiveVersion}-{Runtime}.{extension}{sasToken}",
                    _ => $"{feed.ItemSpec}/Runtime/{version}/dotnet-runtime-{effectiveVersion}-{Runtime}.{extension}{sasToken}"
                };
            }
            catch (Exception ex)
            {
                Log.LogWarning($"Unable to resolve download link from feed {feed}; {SanitizeString(ex.Message)}");
                return null;
            }
            finally
            {
                Version = oldVersion;
            }
        }

        private async Task<string> GetEffectiveVersion(ITaskItem feed, string version)
        {
            if (NuGetVersion.TryParse(version, out NuGetVersion semanticVersion))
            {
                // Pared down version of the logic from https://github.com/dotnet/install-scripts/blob/main/src/dotnet-install.ps1
                // If this functionality stops working, review changes made there.
                // Current strategy is to start with a runtime-specific name then fall back to 'productVersion.txt'
                string effectiveVersion = version;

                // Do nothing for older runtimes; the file won't exist
                if (semanticVersion >= new NuGetVersion("5.0.0"))
                {
                    feed.TryGetMetadata("sasToken", out string sasToken);
                    var productVersionText = PackageType switch
                    {
                        "sdk" => await GetMatchingProductVersionTxtContents($"{feed.ItemSpec}/Sdk/{version}", "sdk-productVersion.txt", sasToken),
                        "aspnetcore-runtime" => await GetMatchingProductVersionTxtContents($"{feed.ItemSpec}/aspnetcore/Runtime/{version}", "aspnetcore-productVersion.txt", sasToken),
                        _ => await GetMatchingProductVersionTxtContents($"{feed.ItemSpec}/Runtime/{version}", "runtime-productVersion.txt", sasToken)
                    };

                    if (!productVersionText.Equals(version))
                    {
                        effectiveVersion = productVersionText;
                        Log.LogMessage($"Switched to effective .NET Core version '{productVersionText}' from matching productVersion.txt");
                    }
                }
                return effectiveVersion;
            }

            throw new ArgumentException($"'{version}' is not a valid semantic version.");
        }
        private async Task<string> GetMatchingProductVersionTxtContents(string baseUri, string customVersionTextFileName, string sasToken = null)
        {
            Log.LogMessage(MessageImportance.Low, $"Checking for productVersion.txt files under {baseUri}");

            using HttpResponseMessage specificResponse = await GetAsyncWithRetry($"{baseUri}/{customVersionTextFileName}", sasToken);

            if (specificResponse.StatusCode == HttpStatusCode.NotFound)
            {
                using HttpResponseMessage genericResponse = await GetAsyncWithRetry($"{baseUri}/productVersion.txt", sasToken);
                if (genericResponse.StatusCode != HttpStatusCode.NotFound)
                {
                    genericResponse.EnsureSuccessStatusCode();
                    return (await genericResponse.Content.ReadAsStringAsync()).Trim();
                }
                else
                {
                    Log.LogMessage(MessageImportance.Low, $"No *productVersion.txt files found for {Version} under {SanitizeString(baseUri)}");
                }
            }
            else
            {
                specificResponse.EnsureSuccessStatusCode();
                return (await specificResponse.Content.ReadAsStringAsync()).Trim();
            }
            return Version;
        }

        private async Task<HttpResponseMessage> GetAsyncWithRetry(string uri, string sasToken = null)
        {
            HttpResponseMessage response = null;
            if (!string.IsNullOrEmpty(sasToken) && !sasToken.StartsWith("?"))
            {
                sasToken = "?" + sasToken;
            }

            await _retry.RunAsync(async attempt =>
            {
                try
                {
                    response = await _client.GetAsync($"{uri}{sasToken}");

                    return true;
                }
                catch (Exception toLog)
                {
                    Log.LogMessage(MessageImportance.Low, $"Hit exception in GetAsync(); will retry up to 10 times ({SanitizeString(toLog.Message)})");
                    return false;
                }
            });
            if (response == null)  // All retries failed
            {
                throw new Exception($"Failed to GET from {SanitizeString(uri)}, even after retrying");
            }
            return response;
        }

        private async Task<HttpResponseMessage> HeadRequestWithRetry(string uri)
        {
            HttpResponseMessage response = null;
            await _retry.RunAsync(async attempt =>
            {
                try
                {
                    using var req = new HttpRequestMessage(HttpMethod.Head, uri);
                    response = await _client.SendAsync(req);
                    return true;
                }
                catch (Exception toLog)
                {
                    Log.LogMessage(MessageImportance.Low, $"Hit exception in SendAsync(); will retry up to 10 times ({SanitizeString(toLog.Message)})");
                    return false;
                }
            });
            if (response == null) // All retries failed
            {
                throw new Exception($"Failed to make HEAD request to {SanitizeString(uri)}, even after retrying");
            }
            return response;
        }

        private void NormalizeParameters()
        {
            if (string.Equals(Channel, "lts", StringComparison.OrdinalIgnoreCase))
            {
                Channel = "LTS";
            }
            else if (string.Equals(Channel, "current", StringComparison.OrdinalIgnoreCase))
            {
                Channel = "Current";
            }
            else
            {
                throw new ArgumentException($"Invalid value '{Channel}' for parameter {nameof(Channel)}");
            }

            if (string.Equals(Version, "latest", StringComparison.OrdinalIgnoreCase))
            {
                Version = "latest";
            }

            if (string.Equals(PackageType, "sdk", StringComparison.OrdinalIgnoreCase))
            {
                PackageType = "sdk";
            }
            else if (string.Equals(PackageType, "aspnetcore-runtime", StringComparison.OrdinalIgnoreCase))
            {
                PackageType = "aspnetcore-runtime";
            }
            else if (string.Equals(PackageType, "runtime", StringComparison.OrdinalIgnoreCase))
            {
                PackageType = "runtime";
            }
            else
            {
                throw new ArgumentException($"Invalid value '{PackageType}' for parameter {nameof(PackageType)}");
            }
        }

        private async Task<string> ResolveVersionAsync(ITaskItem feed)
        {
            string version = Version;
            if (Version == "latest")
            {
                Log.LogMessage(MessageImportance.Low, "Resolving latest dotnet cli version.");
                string latestVersionUrl = PackageType switch
                {
                    "sdk" => $"{feed.ItemSpec}/Sdk/{Channel}/latest.version",
                    "aspnetcore-runtime" => $"{feed.ItemSpec}/aspnetcore/Runtime/{Channel}/latest.version",
                    _ => $"{feed.ItemSpec}/Runtime/{Channel}/latest.version"
                };

                Log.LogMessage(MessageImportance.Low, $"Resolving latest version from url {latestVersionUrl}");

                feed.TryGetMetadata("sasToken", out string sasToken);
                using HttpResponseMessage versionResponse = await GetAsyncWithRetry(latestVersionUrl, sasToken);
                versionResponse.EnsureSuccessStatusCode();
                string latestVersionContent = await versionResponse.Content.ReadAsStringAsync();
                string[] versionData = latestVersionContent.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
                version = versionData[1];
                Log.LogMessage(MessageImportance.Low, $"Got latest dotnet cli version {version}");
            }

            return version;
        }
    }
}
