// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    internal static class ResolverMetadataClient
    {
        /// <summary>
        /// Retrieve the <see cref="RemoteSourceDependencyInfo" /> for a registration.
        /// </summary>
        /// <returns>Returns an empty sequence if the package does not exist.</returns>
        public static async Task<IEnumerable<RemoteSourceDependencyInfo>> GetDependencies(
            HttpSource httpClient,
            Uri registrationUri,
            string packageId,
            VersionRange range,
            SourceCacheContext cacheContext,
            ILogger log,
            CancellationToken token)
        {
            var ranges = await RegistrationUtility.LoadRanges(httpClient, registrationUri, packageId, range, cacheContext, log, token);

            var results = new HashSet<RemoteSourceDependencyInfo>();
            foreach (var rangeObj in ranges)
            {
                if (rangeObj == null)
                {
                    throw new InvalidDataException(registrationUri.AbsoluteUri);
                }

                foreach (JObject packageObj in rangeObj["items"])
                {
                    var catalogEntry = (JObject)packageObj["catalogEntry"];
                    var version = NuGetVersion.Parse(catalogEntry["version"].ToString());

                    if (range.Satisfies(version))
                    {
                        results.Add(ProcessPackageVersion(packageObj, version));
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Process an individual package version entry
        /// </summary>
        /// <param name="packageObj"></param>
        /// <param name="version"></param>
        /// <returns>Returns the RemoteSourceDependencyInfo object corresponding to this package version</returns>
        private static RemoteSourceDependencyInfo ProcessPackageVersion(JObject packageObj, NuGetVersion version)
        {
            var catalogEntry = (JObject)packageObj["catalogEntry"];

            var listed = catalogEntry.GetBoolean("listed") ?? true;

            var id = catalogEntry.Value<string>("id");

            var identity = new PackageIdentity(id, version);
            var dependencyGroups = new List<PackageDependencyGroup>();

            var dependencyGroupsArray = (JArray)catalogEntry["dependencyGroups"];

            if (dependencyGroupsArray != null)
            {
                foreach (JObject dependencyGroupObj in dependencyGroupsArray)
                {
                    var currentFramework = GetFramework(dependencyGroupObj);

                    var groupDependencies = new List<PackageDependency>();

                    JToken dependenciesObj;

                    // Packages with no dependencies have 'dependencyGroups' but no 'dependencies'
                    if (dependencyGroupObj.TryGetValue("dependencies", out dependenciesObj))
                    {
                        foreach (JObject dependencyObj in dependenciesObj)
                        {
                            var dependencyId = dependencyObj.Value<string>("id");
                            var dependencyRange = RegistrationUtility.CreateVersionRange(dependencyObj.Value<string>("range"));

                            groupDependencies.Add(new PackageDependency(dependencyId, dependencyRange));
                        }
                    }

                    dependencyGroups.Add(new PackageDependencyGroup(currentFramework, groupDependencies));
                }
            }

            var contentUri = packageObj.Value<string>("packageContent");

            return new RemoteSourceDependencyInfo(identity, listed, dependencyGroups, contentUri);
        }

        /// <summary>
        /// Retrieve a registration blob
        /// </summary>
        /// <returns>Returns Null if the package does not exist</returns>
        public static async Task<RegistrationInfo> GetRegistrationInfo(
            HttpSource httpClient,
            Uri registrationUri,
            string packageId,
            VersionRange range,
            SourceCacheContext cacheContext,
            NuGetFramework projectTargetFramework,
            ILogger log,
            CancellationToken token)
        {
            var frameworkComparer = NuGetFrameworkFullComparer.Instance;
            var frameworkReducer = new FrameworkReducer();
            var dependencies = await GetDependencies(httpClient, registrationUri, packageId, range, cacheContext, log, token);

            var result = new HashSet<RegistrationInfo>();
            var registrationInfo = new RegistrationInfo();

            registrationInfo.IncludePrerelease = true;
            foreach (var item in dependencies)
            {
                var packageInfo = new PackageInfo
                {
                    Listed = item.Listed,
                    Version = item.Identity.Version,
                    PackageContent = new Uri(item.ContentUri)
                };

                // only one target framework group will be used at install time, which means
                // we can filter down to that group now by using the project target framework
                var depFrameworks = item.DependencyGroups.Select(e => e.TargetFramework);
                var targetFramework = frameworkReducer.GetNearest(projectTargetFramework, depFrameworks);

                // If no frameworks are compatible we just ignore them - Should this be an exception?
                if (targetFramework != null)
                {
                    var dependencyGroup = item.DependencyGroups.FirstOrDefault(d => frameworkComparer.Equals(targetFramework, d.TargetFramework));
                    if (dependencyGroup != null)
                    {
                        foreach (var dependency in dependencyGroup.Packages)
                        {
                            var dependencyInfo = new DependencyInfo
                            {
                                Id = dependency.Id,
                                Range = dependency.VersionRange
                            };

                            packageInfo.Dependencies.Add(dependencyInfo);
                        }
                    }
                }

                registrationInfo.Add(packageInfo);
                registrationInfo.Id = item.Identity.Id;
            }

            return registrationInfo;
        }

        /// <summary>
        /// Retrieve the target framework from a dependency group obj
        /// </summary>
        private static NuGetFramework GetFramework(JObject dependencyGroupObj)
        {
            var framework = NuGetFramework.AnyFramework;

            if (dependencyGroupObj["targetFramework"] != null)
            {
                framework = NuGetFramework.Parse(dependencyGroupObj["targetFramework"].ToString());
            }

            return framework;
        }
    }
}
