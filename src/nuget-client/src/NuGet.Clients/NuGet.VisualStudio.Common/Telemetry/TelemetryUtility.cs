// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Telemetry;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.Configuration;

using AsyncLazyBool = Microsoft.VisualStudio.Threading.AsyncLazy<bool>;
using Task = System.Threading.Tasks.Task;

namespace NuGet.VisualStudio.Telemetry
{
    public static class TelemetryUtility
    {
        private static long FaultEventCount = 0;
        public static long TotalFaultEvents => FaultEventCount;

        public static async Task PostFaultAsync(Exception e, string callerClassName, [CallerMemberName] string callerMemberName = null, IDictionary<string, object> extraProperties = null)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            var caller = $"{callerClassName}.{callerMemberName}";
            var description = $"{e.GetType().Name} - {e.Message}";

            Interlocked.Increment(ref FaultEventCount);

            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var fault = new FaultEvent(VSTelemetrySession.VSEventNamePrefix + "Fault", description, FaultSeverity.General, e, gatherEventDetails: null);
            fault.Properties[$"{VSTelemetrySession.VSPropertyNamePrefix}Fault.Caller"] = caller;
            if (extraProperties != null)
            {
                foreach (var kvp in extraProperties)
                {
                    fault.Properties[VSTelemetrySession.VSEventNamePrefix + kvp.Key] = kvp.Value;
                }
            }

            TelemetryService.DefaultSession.PostEvent(fault);

            if (await IsShellAvailable.GetValueAsync())
            {
                ActivityLog.TryLogError(caller, description);
            }
        }

        private static readonly AsyncLazyBool IsShellAvailable = new AsyncLazyBool(IsShellAvailableAsync, NuGetUIThreadHelper.JoinableTaskFactory);

        private static async Task<bool> IsShellAvailableAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var sVsShell = await AsyncServiceProvider.GlobalProvider.GetServiceAsync<IVsUIShell, IVsUIShell>(throwOnFailure: false);
            return sVsShell != null;
        }

        /// <summary>
        /// True if the source is http and ends with index.json
        /// </summary>
        public static bool IsHttpV3(PackageSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.IsHttp &&
                (source.Source.EndsWith("index.json", StringComparison.OrdinalIgnoreCase)
                || source.ProtocolVersion == 3);
        }

        /// <summary>
        /// True if the source is an Azure Artifacts (DevOps) feed
        /// </summary>
        public static bool IsAzureArtifacts(PackageSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!source.IsHttp)
            {
                return false;
            }

            var uri = source.TrySourceAsUri;
            if (uri == null)
            {
                return false;
            }

            if (StringComparer.OrdinalIgnoreCase.Equals(uri.Host, "pkgs.dev.azure.com")
                || uri.Host.EndsWith(".pkgs.visualstudio.com", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// True if the source is a GitHub Package Repository (GPR) feed
        /// </summary>
        public static bool IsGitHub(PackageSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!source.IsHttp)
            {
                return false;
            }

            var uri = source.TrySourceAsUri;
            if (uri == null)
            {
                return false;
            }

            if (StringComparer.OrdinalIgnoreCase.Equals(uri.Host, "nuget.pkg.github.com"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// True if the source is the Visual Studio Offline feed
        /// </summary>
        public static bool IsVsOfflineFeed(PackageSource source)
        {
            return IsVsOfflineFeed(source, ExpectedVsOfflinePackagesPathX86.Value) || IsVsOfflineFeed(source, ExpectedVsOfflinePackagesPath.Value);
        }

        internal static bool IsVsOfflineFeed(PackageSource source, string expectedVsOfflinePackagesPath)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!source.IsLocal)
            {
                return false;
            }

            return expectedVsOfflinePackagesPath != null &&
                StringComparer.OrdinalIgnoreCase.Equals(expectedVsOfflinePackagesPath, source.Source?.TrimEnd('\\'));
        }

        private static readonly Lazy<string> ExpectedVsOfflinePackagesPath = new Lazy<string>(() =>
        {
            return ComputeVSOfflineFeedPath(Environment.SpecialFolder.ProgramFiles);
        });

        private static readonly Lazy<string> ExpectedVsOfflinePackagesPathX86 = new Lazy<string>(() =>
        {
            return ComputeVSOfflineFeedPath(Environment.SpecialFolder.ProgramFilesX86);
        });

        private static string ComputeVSOfflineFeedPath(Environment.SpecialFolder folderPath)
        {
            if (!RuntimeEnvironmentHelper.IsWindows)
            {
                return null;
            }

            try
            {
                var programFiles = Environment.GetFolderPath(folderPath);
                return Path.Combine(programFiles, "Microsoft SDKs", "NuGetPackages");
            }
            catch
            {
                // Ignore this check if we fail for any reason to generate the path.
                return null;
            }
        }

        /// <summary>
        /// Converts a collection of timings to a json array formatted string.
        /// Empty if the collection is null or empty.
        /// </summary>
        /// <param name="sourceTimings">The timings to convert.</param>
        /// <returns>A json array of timings, returns string.empty if the collection is null or empty.</returns>
        public static string ToJsonArrayOfTimingsInSeconds(IEnumerable<TimeSpan> sourceTimings)
        {
            if (sourceTimings?.Any() != true)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            sb.Append("[");
            foreach (var item in sourceTimings)
            {
                sb.Append(item.TotalSeconds.ToString(CultureInfo.InvariantCulture));
                sb.Append(",");
            }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Length--;
            }
            sb.Append("]");

            return sb.ToString();
        }
    }
}
