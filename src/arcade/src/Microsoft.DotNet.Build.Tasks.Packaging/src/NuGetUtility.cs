// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.RuntimeModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public static class NuGetUtility
    {
        internal static IEnumerable<Version> GetAllVersionsForPackageId(string packageId, bool includePrerelease, bool includeUnlisted, Log log, CancellationToken cancellationToken)
        {
            List<Version> result = new List<Version>();
            ISettings settings = Settings.LoadDefaultSettings(Directory.GetCurrentDirectory());
            IEnumerable<PackageSource> enabledSources = GetEnabledSources(settings);
            var logger = new NuGetLogger(log);
            Parallel.ForEach(enabledSources, (packageSource) =>
            {
                 using (var sourceCacheContext = new SourceCacheContext())
                 {
                    bool loadedData = false;
                    int retriesRemaining = 2;
                    IEnumerable<IPackageSearchMetadata> searchMetadata = null;
                    while (!loadedData) {
                        try
                        {
                            var sourceRepository = new SourceRepository(packageSource, Repository.Provider.GetCoreV3());
                            var packageMetadataResource = sourceRepository.GetResourceAsync<PackageMetadataResource>().GetAwaiter().GetResult();
                            searchMetadata = packageMetadataResource.GetMetadataAsync(packageId, includePrerelease, includeUnlisted, sourceCacheContext, logger, cancellationToken).GetAwaiter().GetResult();
                            loadedData = true;
                        }
                        catch (Exception e)
                        {
                            retriesRemaining--;
                            if (retriesRemaining <= 0) {
                                logger.Log(LogLevel.Error, "Encountered Connection Issue: " + e.ToString() + ", retries exhausted");
                                throw;
                            }
                            logger.Log(LogLevel.Information, "Encountered Connection Issue: " + e.ToString() + ", retrying...");
                            // returns to start of while loop to retry after a delay
                            Thread.Sleep(5000);
                        }
                    }

                    foreach (IPackageSearchMetadata packageMetadata in searchMetadata)
                    {
                        lock (result)
                        {
                            Version threePartVersion = VersionUtility.As3PartVersion(packageMetadata.Identity.Version.Version);
                            if (!result.Contains(threePartVersion))
                                result.Add(threePartVersion);
                        }
                    }
                }
            });
            // Given we are looking in different sources, we reorder all versions.
            return result.OrderBy(v => v);
        }

        private static IEnumerable<PackageSource> GetEnabledSources(ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var provider = new PackageSourceProvider(settings);
            return provider.LoadPackageSources().Where(e => e.IsEnabled == true).ToList();
        }

        public static Version GetLatestPatchStableVersionForRelease(this IEnumerable<Version> versions, int eraMajorVersion, int eraMinorVersion)
        {
            return versions.Where(v => VersionUtility.As2PartVersion(v) == new Version(eraMajorVersion, eraMinorVersion))
                           .OrderByDescending(v => v)
                           .FirstOrDefault();
        }

        public static void WriteRuntimeGraph(string filePath, RuntimeGraph runtimeGraph)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            using (var textWriter = new StreamWriter(fileStream))
            using (var jsonWriter = new JsonTextWriter(textWriter))
            using (var writer = new JsonObjectWriter(jsonWriter))
            {
                jsonWriter.Formatting = Formatting.Indented;

                // workaround https://github.com/NuGet/Home/issues/9532
                writer.WriteObjectStart();

                JsonRuntimeFormat.WriteRuntimeGraph(writer, runtimeGraph);

                writer.WriteObjectEnd();
            }
        }

        internal class NuGetLogger : ILogger
        {
            private readonly Log _log;

            public NuGetLogger(Log log)
            {
                _log = log;
            }

            public void Log(LogLevel level, string data) => _log.LogMessage($"{level.ToString()} - {data}");

            public void Log(ILogMessage message) => _log.LogMessage(message.ToString());

            public Task LogAsync(LogLevel level, string data) => Task.Run(() => Log(level, data));

            public Task LogAsync(ILogMessage message) => Task.Run(() => Log(message));

            public void LogDebug(string data) => _log.LogMessage(data);

            public void LogError(string data) => _log.LogError(data);

            public void LogInformation(string data) => _log.LogMessage(data);

            public void LogInformationSummary(string data) => _log.LogMessage(data);

            public void LogMinimal(string data) => _log.LogMessage(data);

            public void LogVerbose(string data) => _log.LogMessage(data);

            public void LogWarning(string data) => _log.LogWarning(data);
        }
    }
}
