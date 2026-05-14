// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Arcade.Common;
using Microsoft.Build.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

#nullable enable
namespace Microsoft.DotNet.Helix.Sdk
{
    public enum ApplePlatform
    {
        iOS,
        tvOS,
    }

    public interface IProvisioningProfileProvider
    {
        void AddProfileToPayload(string archivePath, string testTarget);
    }

    /// <summary>
    /// This class embeds Apple provisioning profiles into app bundles.
    /// App bundles are directories with files that represent an iOS or tvOS application.
    /// Provisioning profile is a file used for signing and differs per platform (iOS/tvOS).
    /// This class makes sure each app bundle has one before it is sent to Helix.
    /// It injects the profiles into all top-level app bundles in a given .zip archive.
    /// </summary>
    public class ProvisioningProfileProvider : IProvisioningProfileProvider
    {
        // The name of the profile that Apple expects
        private const string ProfileFileName = "embedded.mobileprovision";

        // Matches all paths to .app bundle directories in archive's root
        private static readonly Regex s_topLevelAppPattern = new("^[^" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]+\\.app/.+");
        private static readonly IReadOnlyDictionary<ApplePlatform, string> s_targetNames = new Dictionary<ApplePlatform, string>()
        {
            { ApplePlatform.iOS, "ios-device" },
            { ApplePlatform.tvOS, "tvos-device" },
        };

        private readonly TaskLoggingHelper _log;
        private readonly IHelpers _helpers;
        private readonly IFileSystem _fileSystem;
        private readonly IZipArchiveManager _zipArchiveManager;
        private readonly HttpClient _httpClient;
        private readonly IRetryHandler _retryHandler;
        private readonly string? _profileUrlTemplate;
        private readonly string? _tmpDir;
        private readonly Dictionary<ApplePlatform, string> _downloadedProfiles = new();

        public ProvisioningProfileProvider(
            TaskLoggingHelper log,
            IHelpers helpers,
            IFileSystem fileSystem,
            IZipArchiveManager zipArchiveManager,
            HttpClient httpClient,
            IRetryHandler retryHandler,
            string? profileUrlTemplate,
            string? tmpDir)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _helpers = helpers ?? throw new ArgumentNullException(nameof(helpers));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _zipArchiveManager = zipArchiveManager ?? throw new ArgumentNullException(nameof(zipArchiveManager));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _retryHandler = retryHandler;
            _profileUrlTemplate = profileUrlTemplate;
            _tmpDir = tmpDir;
        }

        public void AddProfileToPayload(string archivePath, string testTarget)
        {
            foreach (var pair in s_targetNames)
            {
                ApplePlatform platform = pair.Key;
                string targetName = pair.Value;

                // Only app bundles that target iOS/tvOS devices need a profile (simulators don't)
                if (!testTarget.Contains(targetName))
                {
                    continue;
                }

                // This makes sure we download the profile the first time we see an app that needs it
                if (!_downloadedProfiles.TryGetValue(platform, out string? profilePath))
                {
                    if (string.IsNullOrEmpty(_tmpDir))
                    {
                        _log.LogError($"{nameof(CreateXHarnessAppleWorkItems.TmpDir)} parameter not set but required for real device targets!");
                        return;
                    }

                    if (string.IsNullOrEmpty(_profileUrlTemplate))
                    {
                        _log.LogError($"{nameof(CreateXHarnessAppleWorkItems.ProvisioningProfileUrl)} parameter not set but required for real device targets!");
                        return;
                    }

                    profilePath = DownloadProvisioningProfile(platform);
                    _downloadedProfiles.Add(platform, profilePath);
                }

                AddProfileToArchive(archivePath, profilePath);
            }
        }

        /// <summary>
        /// Adds a provisioning profile to a given zip archive.
        /// Either adds it to all .app folders inside or to the root of the archive if no app bundles found.
        /// </summary>
        private void AddProfileToArchive(string archivePath, string profilePath)
        {
            // App comes with a profile already
            using ZipArchive zipArchive = _zipArchiveManager.OpenArchive(archivePath, ZipArchiveMode.Update);

            HashSet<string> rootLevelAppBundles = new();
            HashSet<string> appBundlesWithProfile = new();

            foreach (ZipArchiveEntry entry in zipArchive.Entries)
            {
                if (!s_topLevelAppPattern.IsMatch(entry.FullName))
                {
                    continue;
                }

                string appBundleName = entry.FullName.Split(new[] { '/' }, 2).First();
                
                if (entry.FullName == appBundleName + "/" + ProfileFileName)
                {
                    appBundlesWithProfile.Add(appBundleName);
                    _log.LogMessage($"{appBundleName} already contains provisioning profile");
                }
                else
                {
                    rootLevelAppBundles.Add(appBundleName);
                }
            }

            rootLevelAppBundles = rootLevelAppBundles.Except(appBundlesWithProfile).ToHashSet();

            // If no .app bundles, add it to the root
            if (!rootLevelAppBundles.Any())
            {
                _log.LogMessage($"No app bundles found in the archive. Adding provisioning profile to root");

                // Check if archive comes with a profile already
                if (!zipArchive.Entries.Any(e => e.FullName == ProfileFileName))
                {
                    zipArchive.CreateEntryFromFile(profilePath, ProfileFileName);
                }

                return;
            }

            // Else inject profile to every app bundle in the root of the archive
            foreach (string appBundle in rootLevelAppBundles)
            {
                var profileDestPath = appBundle + "/" + ProfileFileName;
                _log.LogMessage($"Adding provisioning profile to {appBundle}");
                zipArchive.CreateEntryFromFile(profilePath, profileDestPath);
            }
        }

        /// <summary>
        /// Process-safe download of the profile (several clashing msbuild processes should download once).
        /// </summary>
        /// <param name="platform">Which platform to download the profile for</param>
        /// <returns>Path where the profile was downloaded to</returns>
        private string DownloadProvisioningProfile(ApplePlatform platform)
        {
            var targetFile = _fileSystem.PathCombine(_tmpDir!, GetProvisioningProfileFileName(platform));

            _helpers.DirectoryMutexExec(async () =>
            {
                if (_fileSystem.FileExists(targetFile))
                {
                    _log.LogMessage($"Using provisioning profile in {targetFile}");
                    return;
                }

                _log.LogMessage($"Downloading {platform} provisioning profile to {targetFile}");

                var uri = new Uri(GetProvisioningProfileUrl(platform));
                HttpResponseMessage? response = null;

                await _retryHandler.RunAsync(async _ =>
                {
                    try
                    {
                        response = await _httpClient.GetAsync(uri);
                        return response.IsSuccessStatusCode;
                    }
                    catch (Exception e)
                    {
                        _log.LogMessage("Failed to download provisioning profile: {error}", e);
                        return false;
                    }
                });

                if (response is null)
                {
                    throw new Exception("Failed to download provisioning profile. More details can be found with higher verbosity or in the binlog");
                }

                using (response)
                using (var fileStream = _fileSystem.GetFileStream(targetFile, FileMode.Create, FileAccess.Write))
                {
                    await response.Content.CopyToAsync(fileStream);
                }
            }, _tmpDir);

            return targetFile;
        }

        private string GetProvisioningProfileFileName(ApplePlatform platform)
            => _fileSystem.GetFileName(GetProvisioningProfileUrl(platform))
                ?? throw new InvalidOperationException("Failed to get provision profile file name");

        private string GetProvisioningProfileUrl(ApplePlatform platform)
            => _profileUrlTemplate!.Replace("{PLATFORM}", platform.ToString());
    }

    public static class ProvisioningProfileProviderRegistration
    {
        public static void TryAddProvisioningProfileProvider(this IServiceCollection collection, string provisioningProfileUrlTemplate, string tmpDir)
        {
            collection.TryAddTransient<IHelpers, Arcade.Common.Helpers>();
            collection.TryAddTransient<IFileSystem, FileSystem>();
            collection.TryAddTransient<IZipArchiveManager, ZipArchiveManager>();
            collection.TryAddTransient<IRetryHandler, ExponentialRetry>();
            collection.TryAddSingleton(_ => new HttpClient(new HttpClientHandler { CheckCertificateRevocationList = true }));
            collection.TryAddSingleton<IProvisioningProfileProvider>(serviceProvider =>
            {
                return new ProvisioningProfileProvider(
                    serviceProvider.GetRequiredService<TaskLoggingHelper>(),
                    serviceProvider.GetRequiredService<IHelpers>(),
                    serviceProvider.GetRequiredService<IFileSystem>(),
                    serviceProvider.GetRequiredService<IZipArchiveManager>(),
                    serviceProvider.GetRequiredService<HttpClient>(),
                    serviceProvider.GetRequiredService<IRetryHandler>(),
                    provisioningProfileUrlTemplate,
                    tmpDir);
            });
        }
    }
}
