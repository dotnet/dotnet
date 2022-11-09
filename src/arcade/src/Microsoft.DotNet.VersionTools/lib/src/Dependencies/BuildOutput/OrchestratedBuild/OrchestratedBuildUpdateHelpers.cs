// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using Microsoft.DotNet.VersionTools.Dependencies.BuildManifest;
using System;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.DotNet.VersionTools.Dependencies.BuildOutput.OrchestratedBuild
{
    public static class OrchestratedBuildUpdateHelpers
    {
        public static Func<OrchestratedBuildDependencyInfo[], DependencyReplacement>
            BuildAttribute(string buildName, string attributeName)
        {
            return infos =>
            {
                var match = OrchestratedBuildIdentityMatch.Find(buildName, infos);

                if (match == null)
                {
                    Trace.TraceInformation($"No build identity match for '{buildName}'.");
                    return null;
                }

                string value;
                if (match.Match.Attributes.TryGetValue(attributeName, out value))
                {
                    return new DependencyReplacement(value, new[] { match.Info });
                }

                Trace.TraceInformation(
                    $"In build identity '{buildName}', no attribute '{attributeName}'.");
                return null;
            };
        }

        public static Func<OrchestratedBuildDependencyInfo[], DependencyReplacement>
            OrchestratedFeedAttribute(string attributeName)
        {
            return infos => infos
                .Select(info =>
                {
                    EndpointModel feed = info.GetBlobFeed();

                    string value;
                    if (feed != null && feed.Attributes.TryGetValue(attributeName, out value))
                    {
                        return new DependencyReplacement(value, new[] { info });
                    }

                    Trace.TraceInformation(
                        $"No blob feed or '{attributeName}' attribute for '{info}'.");
                    return null;
                })
                .FirstOrDefault(r => r != null);
        }

        public static Func<OrchestratedBuildDependencyInfo[], DependencyReplacement>
            OrchestratedFeedPackageVersion(string packageId)
        {
            return infos => infos
                .Select(info =>
                {
                    PackageArtifactModel match = info.GetBlobFeed()?.Artifacts.Packages
                        .FirstOrDefault(p => p.Id.Equals(packageId, StringComparison.OrdinalIgnoreCase));

                    if (match != null)
                    {
                        return new DependencyReplacement(match.Version, new[] { info });
                    }

                    Trace.TraceInformation($"No package '{packageId}' match in '{info}'.");
                    return null;
                })
                .FirstOrDefault(r => r != null);
        }

        private static EndpointModel GetBlobFeed(this OrchestratedBuildDependencyInfo info)
        {
            return info.OrchestratedBuildModel.Endpoints
                .FirstOrDefault(e => e.IsOrchestratedBlobFeed);
        }
    }
}
