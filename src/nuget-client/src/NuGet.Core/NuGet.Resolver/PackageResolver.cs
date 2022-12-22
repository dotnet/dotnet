// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Resolver
{
    /// <summary>
    /// A core package dependency resolver.
    /// </summary>
    /// <remarks>Not thread safe</remarks>
    public class PackageResolver
    {
        /// <summary>
        /// Resolve a package closure
        /// </summary>
        public IEnumerable<PackageIdentity> Resolve(PackageResolverContext context, CancellationToken token)
        {
            var stopWatch = new Stopwatch();
            token.ThrowIfCancellationRequested();

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // validation 
            foreach (var requiredId in context.RequiredPackageIds)
            {
                if (!context.AvailablePackages.Any(p => StringComparer.OrdinalIgnoreCase.Equals(p.Id, requiredId)))
                {
                    throw new NuGetResolverInputException(String.Format(CultureInfo.CurrentCulture, Strings.MissingDependencyInfo, requiredId));
                }
            }

            var invalidExistingPackages = new List<string>();

            var installedPackages = context.PackagesConfig.Select(p => p.PackageIdentity).ToArray();

            // validate existing package.config for any invalid dependency
            foreach (var package in installedPackages)
            {
                var existingPackage =
                    context.AvailablePackages.FirstOrDefault(
                        p =>
                            StringComparer.OrdinalIgnoreCase.Equals(p.Id, package.Id) &&
                            p.Version.Equals(package.Version));

                if (existingPackage != null)
                {
                    // check if each dependency can be satisfied with existing packages
                    var brokenDependencies = GetBrokenDependencies(existingPackage, installedPackages);

                    if (brokenDependencies != null && brokenDependencies.Any())
                    {
                        invalidExistingPackages.AddRange(brokenDependencies.Select(dependency => FormatDependencyConstraint(existingPackage, dependency)));
                    }
                }
                else
                {
                    // check same package is being updated and we've a higher version then 
                    // ignore logging warning for that.
                    existingPackage =
                        context.AvailablePackages.FirstOrDefault(
                            p =>
                                StringComparer.OrdinalIgnoreCase.Equals(p.Id, package.Id) &&
                                VersionComparer.Default.Compare(p.Version, package.Version) > 0);

                    if (existingPackage == null)
                    {
                        var packageString = $"'{package.Id} {package.Version.ToNormalizedString()}'";
                        invalidExistingPackages.Add(packageString);
                    }
                }
            }
            // log warning message for all the invalid package dependencies
            if (invalidExistingPackages.Count > 0)
            {
                context.Log.LogWarning(
                    string.Format(
                        CultureInfo.CurrentCulture, Strings.InvalidPackageConfig, string.Join(", ", invalidExistingPackages)));
            }

            // convert the available packages into ResolverPackages
            var resolverPackages = new List<ResolverPackage>();

            // pre-process the available packages to remove any packages that can't possibly form part of a solution
            var availablePackages = RemoveImpossiblePackages(context.AvailablePackages, context.RequiredPackageIds);

            foreach (var package in availablePackages)
            {
                IEnumerable<PackageDependency> dependencies = null;

                // clear out the dependencies if the behavior is set to ignore
                if (context.DependencyBehavior == DependencyBehavior.Ignore)
                {
                    dependencies = Enumerable.Empty<PackageDependency>();
                }
                else
                {
                    dependencies = package.Dependencies ?? Enumerable.Empty<PackageDependency>();
                }

                resolverPackages.Add(new ResolverPackage(package.Id, package.Version, dependencies, package.Listed, false));
            }

            // Sort the packages to make this process as deterministic as possible
            resolverPackages.Sort(PackageIdentityComparer.Default);

            // Keep track of the ids we have added
            var groupsAdded = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var grouped = new List<List<ResolverPackage>>();

            // group the packages by id
            foreach (var group in resolverPackages.GroupBy(e => e.Id, StringComparer.OrdinalIgnoreCase))
            {
                groupsAdded.Add(group.Key);

                var curSet = group.ToList();

                // add an absent package for non-targets
                // being absent allows the resolver to throw it out if it is not needed
                if (!context.RequiredPackageIds.Contains(group.Key, StringComparer.OrdinalIgnoreCase))
                {
                    curSet.Add(new ResolverPackage(id: group.Key, version: null, dependencies: null, listed: true, absent: true));
                }

                grouped.Add(curSet);
            }

            // find all needed dependencies
            var dependencyIds = resolverPackages.Where(e => e.Dependencies != null)
                .SelectMany(e => e.Dependencies.Select(d => d.Id).Distinct(StringComparer.OrdinalIgnoreCase));

            foreach (string depId in dependencyIds)
            {
                // packages which are unavailable need to be added as absent packages
                // ex: if A -> B  and B is not found anywhere in the source repositories we add B as absent
                if (!groupsAdded.Contains(depId))
                {
                    groupsAdded.Add(depId);
                    grouped.Add(new List<ResolverPackage>() { new ResolverPackage(id: depId, version: null, dependencies: null, listed: true, absent: true) });
                }
            }

            token.ThrowIfCancellationRequested();

            // keep track of the best partial solution
            var bestSolution = Enumerable.Empty<ResolverPackage>();

            Action<IEnumerable<ResolverPackage>> diagnosticOutput = (partialSolution) =>
            {
                // store each solution as they pass through.
                // the combination solver verifies that the last one returned is the best
                bestSolution = partialSolution;
            };

            // Run solver
            var comparer = new ResolverComparer(context.DependencyBehavior, context.PreferredVersions, context.TargetIds);

            var sortedGroups = ResolverInputSort.TreeFlatten(grouped, context);

            var solution = CombinationSolver<ResolverPackage>.FindSolution(
                groupedItems: sortedGroups,
                itemSorter: comparer,
                shouldRejectPairFunc: ResolverUtility.ShouldRejectPackagePair,
                diagnosticOutput: diagnosticOutput);

            // check if a solution was found
            if (solution != null)
            {
                var nonAbsentCandidates = solution.Where(c => !c.Absent);

                if (nonAbsentCandidates.Any())
                {
                    // topologically sort non absent packages
                    var sortedSolution = ResolverUtility.TopologicalSort(nonAbsentCandidates);

                    // Find circular dependency for topologically sorted non absent packages since it will help maintain cache of 
                    // already processed packages
                    var circularReferences = ResolverUtility.FindFirstCircularDependency(sortedSolution);

                    if (circularReferences.Any())
                    {
                        // the resolver is able to handle circular dependencies, however we should throw here to keep these from happening
                        throw new NuGetResolverConstraintException(
                            String.Format(CultureInfo.CurrentCulture, Strings.CircularDependencyDetected,
                            String.Join(" => ", circularReferences.Select(package => $"{package.Id} {package.Version.ToNormalizedString()}"))));
                    }

                    // solution found!
                    stopWatch.Stop();
                    context.Log.LogMinimal(
                        string.Format(CultureInfo.CurrentCulture, Strings.ResolverTotalTime, DatetimeUtility.ToReadableTimeFormat(stopWatch.Elapsed)));
                    return sortedSolution.ToArray();
                }
            }

            // no solution was found, throw an error with a diagnostic message
            var message = ResolverUtility.GetDiagnosticMessage(bestSolution, context.AvailablePackages, context.PackagesConfig, context.TargetIds, context.PackageSources);
            throw new NuGetResolverConstraintException(message);
        }

        private static IEnumerable<PackageDependency> GetBrokenDependencies(SourcePackageDependencyInfo package, IEnumerable<PackageIdentity> packages)
        {
            foreach (var dependency in package.Dependencies)
            {
                var target = packages.FirstOrDefault(targetPackage => StringComparer.OrdinalIgnoreCase.Equals(targetPackage.Id, dependency.Id));

                if (!ResolverUtility.IsDependencySatisfied(dependency, target))
                {
                    yield return dependency;
                }
            }

            yield break;
        }

        private static string FormatDependencyConstraint(SourcePackageDependencyInfo package, PackageDependency dependency)
        {
            var range = dependency.VersionRange;
            var dependencyString = $"{dependency.Id} {range?.ToNonSnapshotRange().PrettyPrint() ?? string.Empty}";

            // A 1.0.0 dependency: B (= 1.5)
            return $"'{package.Id} {package.Version.ToNormalizedString()} {Strings.DependencyConstraint}: {dependencyString}'";
        }

        /// <summary>
        /// Remove packages that can't possibly form part of a solution
        /// </summary>
        private static IEnumerable<SourcePackageDependencyInfo> RemoveImpossiblePackages(IEnumerable<SourcePackageDependencyInfo> packages, ISet<string> mustKeep)
        {
            List<SourcePackageDependencyInfo> before;
            List<SourcePackageDependencyInfo> after = new List<SourcePackageDependencyInfo>(packages);

            do
            {
                before = after;
                after = InnerPruneImpossiblePackages(before, mustKeep);
            }
            while (after.Count < before.Count);

            return after;
        }

        private static List<SourcePackageDependencyInfo> InnerPruneImpossiblePackages(List<SourcePackageDependencyInfo> packages, ISet<string> mustKeep)
        {
            if (packages.Count == 0)
            {
                return packages;
            }

            var dependencyRangesByPackageId = new Dictionary<string, IList<VersionRange>>(StringComparer.OrdinalIgnoreCase);

            //  (1) Adds all package Ids including leaf nodes that have no dependencies
            foreach (var package in packages)
            {
                if (!dependencyRangesByPackageId.ContainsKey(package.Id))
                {
                    dependencyRangesByPackageId.Add(package.Id, new List<VersionRange>());
                }
            }

            //  (2) Create a look-up of every dependency that refers to a particular package Id
            foreach (var package in packages)
            {
                foreach (var dependency in package?.Dependencies)
                {
                    IList<VersionRange> dependencyVersionRanges;
                    if (dependencyRangesByPackageId.TryGetValue(dependency.Id, out dependencyVersionRanges))
                    {
                        dependencyVersionRanges.Add(dependency.VersionRange);
                    }
                }
            }

            //  (3) Per package Id combine all the dependency ranges into a wider 'worst-case' range
            var dependencyByPackageId = new Dictionary<string, VersionRange>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in dependencyRangesByPackageId)
            {
                dependencyByPackageId.Add(item.Key, VersionRange.Combine(item.Value));
            }

            //  (4) Remove any packages that fall out side of the worst case range while making sure not to remove the packages we must keep
            var result = packages.Where(
                package => dependencyByPackageId[package.Id].Satisfies(package.Version) || mustKeep.Contains(package.Id))
                .ToList();

            return result;
        }
    }
}
