// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using NuGet.Client;
using NuGet.ContentModel;
using NuGet.Frameworks;
using NuGet.RuntimeModel;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.TargetFramework
{
    /// <summary>
    /// This class uses NuGet's asset selection logic to choose the best TargetFramework given the list of supported TargetFrameworks.
    /// This behaves in a same way as NuGet selects lib files from a nuget package for a particular TargetFramework.
    /// </summary>
    internal class TargetFrameworkResolver
    {
        private static readonly Dictionary<string, TargetFrameworkResolver> s_targetFrameworkResolverCache = new();
        private readonly ManagedCodeConventions _conventions;
        private readonly PatternSet _configStringPattern;

        private TargetFrameworkResolver(string runtimeGraph)
        {
            _conventions = new ManagedCodeConventions(JsonRuntimeFormat.ReadRuntimeGraph(runtimeGraph));
            _configStringPattern = new PatternSet(
                _conventions.Properties,
                groupPatterns: new PatternDefinition[]
                {
                    // In order to use Nuget's asset allocation, the input needs to be file paths and should contain a trailing slash.
                    new PatternDefinition("{tfm}/"),
                    new PatternDefinition("{tfm}-{rid}/")
                },
                pathPatterns: new PatternDefinition[]
                {
                    new PatternDefinition("{tfm}/"),
                    new PatternDefinition("{tfm}-{rid}/")
                });
        }

        public static TargetFrameworkResolver CreateOrGet(string runtimeGraph)
        {
            if (!s_targetFrameworkResolverCache.TryGetValue(runtimeGraph, out TargetFrameworkResolver? targetFrameworkResolver))
            {
                targetFrameworkResolver = new TargetFrameworkResolver(runtimeGraph);
                s_targetFrameworkResolverCache.Add(runtimeGraph, targetFrameworkResolver);
            }

            return targetFrameworkResolver!;
        }

        public string? GetNearest(IEnumerable<string> frameworks, NuGetFramework framework)
        {
            NuGetFramework frameworkWithoutPlatform = NuGetFramework.Parse(framework.DotNetFrameworkName);

            ContentItemCollection contentCollection = new();
            contentCollection.Load(frameworks.Select(f => f + '/').ToArray());

            // The platform is expected to be passed-in lower-case but the SDK normalizes "windows" to "Windows" which is why it is lowered again.
            SelectionCriteria criteria = _conventions.Criteria.ForFrameworkAndRuntime(frameworkWithoutPlatform, framework.Platform.ToLowerInvariant());
            string? bestTargetFrameworkString = contentCollection.FindBestItemGroup(criteria, _configStringPattern)?.Items[0].Path;

            return bestTargetFrameworkString?.Remove(bestTargetFrameworkString.Length - 1);
        }
    }
}
