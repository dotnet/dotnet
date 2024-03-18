// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.VersionTools.Dependencies.BuildManifest
{
    public class OrchestratedBuildIdentityMatch
    {
        public static OrchestratedBuildIdentityMatch Find(
            string buildName,
            IEnumerable<IDependencyInfo> dependencyInfos)
        {
            OrchestratedBuildIdentityMatch[] matches = dependencyInfos
                .OfType<OrchestratedBuildDependencyInfo>()
                .SelectMany(info => info.OrchestratedBuildModel.Builds
                    .Where(b => b.Name.Equals(buildName, StringComparison.OrdinalIgnoreCase))
                    .Select(b => new OrchestratedBuildIdentityMatch { Info = info, Match = b }))
                .ToArray();

            if (matches.Length > 1)
            {
                throw new ArgumentException(
                    $"Expected only 1 build matching '{buildName}', but found {matches.Length}: " +
                    $"'{string.Join(", ", matches.AsEnumerable())}'");
            }

            return matches.FirstOrDefault();
        }

        public OrchestratedBuildDependencyInfo Info { get; set; }
        public BuildIdentity Match { get; set; }

        public void EnsureMatchHasCommit()
        {
            if (string.IsNullOrEmpty(Match.Commit))
            {
                throw new ArgumentException($"Match '{this}' has no commit.");
            }
        }

        public string GetAttributeValue(string name)
        {
            string value;

            if (!Match.Attributes.TryGetValue(name, out value))
            {
                throw new ArgumentException($"Could not find attribute '{name}' in '{this}'");
            }

            return value;
        }

        public override string ToString() => $"'{Match}' from '{Info.SimpleName}'";
    }
}
