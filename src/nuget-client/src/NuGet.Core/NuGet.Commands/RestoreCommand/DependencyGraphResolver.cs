// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.ProjectModel;
using NuGet.Repositories;
using NuGet.RuntimeModel;
using NuGet.Versioning;
using LibraryDependencyIndex = NuGet.Commands.DependencyGraphResolver.LibraryDependencyInterningTable.LibraryDependencyIndex;
using LibraryRangeIndex = NuGet.Commands.DependencyGraphResolver.LibraryRangeInterningTable.LibraryRangeIndex;

namespace NuGet.Commands
{
    /// <summary>
    /// Represents a class that can resolve a dependency graph.
    /// </summary>
    internal sealed class DependencyGraphResolver
    {
        private const int DependencyGraphItemQueueSize = 4096;
        private const int EvictionsDictionarySize = 1024;
        private const int FindLibraryEntryResultCacheSize = 2048;
        private const int OverridesDictionarySize = 1024;
        private const int ResolvedDependencyGraphItemQueueSize = 2048;
        private readonly RestoreCollectorLogger _logger;
        private readonly Guid _operationId;
        private readonly RestoreRequest _request;
        private readonly TelemetryActivity _telemetryActivity;

        public DependencyGraphResolver(RestoreCollectorLogger logger, RestoreRequest restoreRequest, TelemetryActivity telemetryActivity, Guid operationId)
        {
            _logger = logger;
            _request = restoreRequest;
            _telemetryActivity = telemetryActivity;
            _operationId = operationId;
        }

#pragma warning disable CA1505 // 'ResolveAsync' has a maintainability index of '0'. Rewrite or refactor the code to increase its maintainability index (MI) above '9'.  This will be refactored in a future change.
        public async Task<ValueTuple<bool, List<RestoreTargetGraph>, RuntimeGraph>> ResolveAsync(NuGetv3LocalRepository userPackageFolder, IReadOnlyList<NuGetv3LocalRepository> fallbackPackageFolders, RemoteWalkContext context, ProjectRestoreCommand projectRestoreCommand, List<NuGetv3LocalRepository> localRepositories, CancellationToken token)
#pragma warning restore CA1505
        {
            bool _success = true;
            bool isCentralPackageTransitivePinningEnabled = _request.Project.RestoreMetadata != null && _request.Project.RestoreMetadata.CentralPackageVersionsEnabled & _request.Project.RestoreMetadata.CentralPackageTransitivePinningEnabled;

            var uniquePackages = new HashSet<LibraryIdentity>();

            var projectRange = new LibraryRange()
            {
                Name = _request.Project.Name,
                VersionRange = new VersionRange(_request.Project.Version),
                TypeConstraint = LibraryDependencyTarget.Project | LibraryDependencyTarget.ExternalProject
            };

            // Resolve dependency graphs
            var allGraphs = new List<RestoreTargetGraph>();
            var runtimeGraphs = new List<RestoreTargetGraph>();
            var graphByTFM = new Dictionary<NuGetFramework, RestoreTargetGraph>();
            var runtimeIds = RequestRuntimeUtility.GetRestoreRuntimes(_request);
            IEnumerable<FrameworkRuntimePair> projectFrameworkRuntimePairs = RestoreCommand.CreateFrameworkRuntimePairs(_request.Project, runtimeIds);
            RuntimeGraph allRuntimes = RuntimeGraph.Empty;

            LibraryRangeInterningTable libraryRangeInterningTable = new LibraryRangeInterningTable();
            LibraryDependencyInterningTable libraryDependencyInterningTable = new LibraryDependencyInterningTable();

            _telemetryActivity.StartIntervalMeasure();

            bool hasInstallBeenCalledAlready = false;
            DownloadDependencyResolutionResult[]? downloadDependencyResolutionResults = default;

            foreach (var pair in projectFrameworkRuntimePairs)
            {
                if (!string.IsNullOrWhiteSpace(pair.RuntimeIdentifier) && !hasInstallBeenCalledAlready)
                {
                    downloadDependencyResolutionResults = await ProjectRestoreCommand.DownloadDependenciesAsync(_request.Project, context, _telemetryActivity, telemetryPrefix: string.Empty, token);

                    _success &= await projectRestoreCommand.InstallPackagesAsync(uniquePackages, allGraphs, downloadDependencyResolutionResults, userPackageFolder, token);

                    hasInstallBeenCalledAlready = true;
                }

                TargetFrameworkInformation? projectTargetFramework = _request.Project.TargetFrameworks.FirstOrDefault(i => NuGetFramework.Comparer.Equals(i.FrameworkName, pair.Framework));

                //Build up our new RestoreTargetGraph.
                //This is done by making everything on RTG setable.  We should move to using a normal constructor.
                var newRTG = new RestoreTargetGraph();
                //Set the statically setable stuff
                newRTG.AnalyzeResult = new AnalyzeResult<RemoteResolveResult>();
                newRTG.Conflicts = new List<ResolverConflict>();
                newRTG.InConflict = false; //its never set for substrate fwiw...
                newRTG.Install = new HashSet<RemoteMatch>();
                newRTG.ResolvedDependencies = new HashSet<ResolvedDependencyKey>();
                newRTG.Unresolved = new HashSet<LibraryRange>();

                //Set the natively settable things.
                newRTG.Framework = pair.Framework;
                newRTG.RuntimeIdentifier = (pair.RuntimeIdentifier == "" ? null : pair.RuntimeIdentifier);
                newRTG.Name = FrameworkRuntimePair.GetName(newRTG.Framework, newRTG.RuntimeIdentifier);
                newRTG.TargetGraphName = FrameworkRuntimePair.GetTargetGraphName(newRTG.Framework, newRTG.RuntimeIdentifier);

                RuntimeGraph? runtimeGraph = default;
                if (!string.IsNullOrEmpty(pair.RuntimeIdentifier) && graphByTFM.TryGetValue(pair.Framework, out var tfmNonRidGraph))
                {
                    // We start with the non-RID TFM graph.
                    // This is guaranteed to be computed before any graph with a RID, so we can assume this will return a value.

                    // PCL Projects with Supports have a runtime graph but no matching framework.
                    var runtimeGraphPath = projectTargetFramework?.RuntimeIdentifierGraphPath;

                    RuntimeGraph? projectProviderRuntimeGraph = default;
                    if (runtimeGraphPath != null)
                    {
                        projectProviderRuntimeGraph = ProjectRestoreCommand.GetRuntimeGraph(runtimeGraphPath, _logger);
                    }

                    runtimeGraph = ProjectRestoreCommand.GetRuntimeGraph(tfmNonRidGraph, localRepositories, projectRuntimeGraph: projectProviderRuntimeGraph, _logger);
                    allRuntimes = RuntimeGraph.Merge(allRuntimes, runtimeGraph);
                }

                newRTG.Conventions = new Client.ManagedCodeConventions(runtimeGraph);

                //Now build up our new flattened graph
                var initialProject = new LibraryDependency(new LibraryRange()
                {
                    Name = _request.Project.Name,
                    VersionRange = new VersionRange(_request.Project.Version),
                    TypeConstraint = LibraryDependencyTarget.Project | LibraryDependencyTarget.ExternalProject
                });

                //If we find newer nodes of things while we walk, we'll evict them.
                //A subset of evictions cause a re-import.  For instance if a newer chosen node has fewer refs,
                //then we might have a dangling over-elevated ref on the old one, it'll be hard evicted.
                //The second item here is the path to root.  When we add a hard-evictee, we'll also remove anything
                //added to the eviction list that contains the evictee on their path to root.
                Queue<DependencyGraphItem> refImport =
                  new Queue<DependencyGraphItem>(DependencyGraphItemQueueSize);

                Dictionary<LibraryRangeIndex, FindLibraryEntryResult> findLibraryEntryCache =
                    new Dictionary<LibraryRangeIndex, FindLibraryEntryResult>(FindLibraryEntryResultCacheSize);

                Dictionary<LibraryDependencyIndex, ResolvedDependencyGraphItem> chosenResolvedItems = new(ResolvedDependencyGraphItemQueueSize);

                Dictionary<LibraryRangeIndex, (LibraryRangeIndex[], LibraryDependencyIndex, LibraryDependencyTarget)> evictions = new Dictionary<LibraryRangeIndex, (LibraryRangeIndex[], LibraryDependencyIndex, LibraryDependencyTarget)>(EvictionsDictionarySize);


                Dictionary<LibraryDependencyIndex, VersionRange>? pinnedPackageVersions = null;

                if (isCentralPackageTransitivePinningEnabled && projectTargetFramework != null && projectTargetFramework.CentralPackageVersions != null)
                {
                    pinnedPackageVersions = new Dictionary<LibraryDependencyIndex, VersionRange>(capacity: projectTargetFramework.CentralPackageVersions.Count);

                    foreach (var item in projectTargetFramework.CentralPackageVersions)
                    {
                        LibraryDependencyIndex depIndex = libraryDependencyInterningTable.Intern(item.Value);

                        pinnedPackageVersions[depIndex] = item.Value.VersionRange;
                    }
                }

                DependencyGraphItem rootProjectRefItem = new DependencyGraphItem()
                {
                    LibraryDependency = initialProject,
                    LibraryDependencyIndex = libraryDependencyInterningTable.Intern(initialProject),
                    LibraryRangeIndex = libraryRangeInterningTable.Intern(initialProject.LibraryRange),
                    Suppressions = new HashSet<LibraryDependencyIndex>(),
                    VersionOverrides = new Dictionary<LibraryDependencyIndex, VersionRange>(),
                    IsDirectPackageReferenceFromRootProject = false,
                };

            ProcessDeepEviction:

                refImport.Clear();
                chosenResolvedItems.Clear();

                refImport.Enqueue(rootProjectRefItem);

                while (refImport.Count > 0)
                {
                    DependencyGraphItem importRefItem = refImport.Dequeue();
                    var currentRef = importRefItem.LibraryDependency!;
                    var currentRefDependencyIndex = importRefItem.LibraryDependencyIndex;
                    var currentRefRangeIndex = importRefItem.LibraryRangeIndex;
                    var pathToCurrentRef = importRefItem.Path;
                    var currentSuppressions = importRefItem.Suppressions;
                    var currentOverrides = importRefItem.VersionOverrides!;
                    var isCentrallyPinnedTransitivePackage = importRefItem.IsCentrallyPinnedTransitivePackage;
                    var directPackageReferenceFromRootProject = importRefItem.IsDirectPackageReferenceFromRootProject;
                    LibraryRangeIndex libraryRangeOfCurrentRef = importRefItem.LibraryRangeIndex;

                    LibraryDependencyTarget typeConstraint = currentRef.LibraryRange.TypeConstraint;
                    bool isProject = ((typeConstraint == LibraryDependencyTarget.Project) ||
                                      (typeConstraint == (LibraryDependencyTarget.Project | LibraryDependencyTarget.ExternalProject)));

                    if (evictions.TryGetValue(libraryRangeOfCurrentRef, out var eviction))
                    {
                        (LibraryRangeIndex[] evictedPath, LibraryDependencyIndex evictedDepIndex, LibraryDependencyTarget evictedTypeConstraint) = eviction;

                        // If we evicted this same version previously, but the type constraint of currentRef is more stringent (package), then do not skip the current item - this is the one we want.
                        if (!((evictedTypeConstraint == LibraryDependencyTarget.PackageProjectExternal || evictedTypeConstraint == LibraryDependencyTarget.ExternalProject) &&
                            currentRef.LibraryRange.TypeConstraint == LibraryDependencyTarget.Package))
                        {
                            continue;
                        }
                    }

                    if (currentOverrides.TryGetValue(currentRefDependencyIndex, out var ov))
                    {
                        if (!ov.Equals(currentRef.LibraryRange.VersionRange))
                        {
                            continue;
                        }
                    }

                    //else if we've seen this ref (but maybe not version) before check to see if we need to upgrade
                    if (chosenResolvedItems.TryGetValue(currentRefDependencyIndex, out ResolvedDependencyGraphItem chosenResolvedItem))
                    {
                        LibraryDependency chosenRef = chosenResolvedItem.LibraryDependency;
                        LibraryRangeIndex chosenRefRangeIndex = chosenResolvedItem.LibraryRangeIndex;
                        LibraryRangeIndex[] pathChosenRef = chosenResolvedItem.Path;
                        bool packageReferenceFromRootProject = chosenResolvedItem.IsDirectPackageReferenceFromRootProject;
                        bool isChosenItemCentrallyPinnedTransitivePackage = chosenResolvedItem.IsCentrallyPinnedTransitivePackage;
                        List<SuppressionsAndVersionOverrides> chosenSuppressions = chosenResolvedItem.SuppressionsAndVersionOverrides;

                        if (packageReferenceFromRootProject)
                        {
                            continue;
                        }

                        // We should evict on type constraint if the type constraint of the current ref is more stringent than the chosen ref.
                        // This happens when a previous type constraint is broader (e.g. PackageProjectExternal) than the current type constraint (e.g. Package).
                        bool evictOnTypeConstraint = false;
                        if ((chosenRefRangeIndex == currentRefRangeIndex) && EvictOnTypeConstraint(currentRef.LibraryRange.TypeConstraint, chosenRef.LibraryRange.TypeConstraint))
                        {
                            if (findLibraryEntryCache.TryGetValue(chosenRefRangeIndex, out FindLibraryEntryResult? resolvedItem) && resolvedItem.Item.Key.Type == LibraryType.Project)
                            {
                                // We need to evict the chosen item because this one has a more stringent type constraint.
                                evictOnTypeConstraint = true;
                            }
                        }

                        // TODO: Handle null version ranges
                        VersionRange nvr = currentRef.LibraryRange.VersionRange ?? VersionRange.All;
                        VersionRange ovr = chosenRef.LibraryRange.VersionRange ?? VersionRange.All;

                        if (evictOnTypeConstraint || !RemoteDependencyWalker.IsGreaterThanOrEqualTo(ovr, nvr))

                        {
                            if (chosenRef.LibraryRange.TypeConstraint == LibraryDependencyTarget.Package && currentRef.LibraryRange.TypeConstraint == LibraryDependencyTarget.PackageProjectExternal)
                            {
                                bool commonAncestry = true;

                                for (int i = 0; i < importRefItem.Path.Length && i < chosenResolvedItem.Path.Length; i++)
                                {
                                    if (importRefItem.Path[i] != chosenResolvedItem.Path[i])
                                    {
                                        commonAncestry = false;
                                        break;
                                    }
                                }

                                if (commonAncestry)
                                {
                                    continue;
                                }
                            }

                            //If we think the newer thing we are looking at is better, remove the old one and let it fall thru.

                            chosenResolvedItems.Remove(currentRefDependencyIndex);
                            //Record an eviction for the node we are replacing.  The eviction path is for the current node.
                            LibraryRangeIndex evictedLR = chosenRefRangeIndex;

                            // If we're evicting on typeconstraint, then there is already an item in allResolvedItems that matches the old typeconstraint.
                            // We must remove it, otherwise we won't call FindLibraryCachedAsync again to load the correct item and save it into allResolvedItems.
                            if (evictOnTypeConstraint)
                            {
                                findLibraryEntryCache.Remove(evictedLR);
                            }

                            int deepEvictions = 0;
                            //unwind anything chosen by the node we're evicting..
                            HashSet<LibraryRangeIndex>? evicteesToRemove = default;
                            foreach (var evictee in evictions)
                            {
                                (LibraryRangeIndex[] evicteePath, LibraryDependencyIndex evicteeDepIndex, LibraryDependencyTarget evicteeTypeConstraint) = evictee.Value;

                                if (evicteePath.Contains(evictedLR))
                                {
                                    // if evictee.Key (depIndex) == currentDepIndex && evictee.TypeConstraint == ExternalProject --> Don't remove it.  It must remain evicted.
                                    // If the evictee to remove is the same dependency, but the project version of said dependency, then do not remove it - it must remain evicted in favor of the package.
                                    if (!(evicteeDepIndex == currentRefDependencyIndex &&
                                        (evicteeTypeConstraint == LibraryDependencyTarget.ExternalProject || evicteeTypeConstraint == LibraryDependencyTarget.PackageProjectExternal)))
                                    {
                                        if (evicteesToRemove == null)
                                            evicteesToRemove = new HashSet<LibraryRangeIndex>();
                                        evicteesToRemove.Add(evictee.Key);
                                    }
                                }
                            }
                            if (evicteesToRemove != null)
                            {
                                foreach (var evicteeToRemove in evicteesToRemove)
                                {
                                    evictions.Remove(evicteeToRemove);
                                    deepEvictions++;
                                }
                            }
                            foreach (var chosenItem in chosenResolvedItems)
                            {
                                if (chosenItem.Value.Path.Contains(evictedLR))
                                {
                                    deepEvictions++;
                                    break;
                                }
                            }
                            evictions[evictedLR] = (LibraryRangeInterningTable.CreatePathToRef(pathToCurrentRef, libraryRangeOfCurrentRef), currentRefDependencyIndex, chosenRef.LibraryRange.TypeConstraint);

                            if (deepEvictions > 0)
                            {
                                goto ProcessDeepEviction;
                            }
                            //Since this is a "new" choice, its gets a new import context list
                            chosenResolvedItems.Add(
                                currentRefDependencyIndex,
                                new ResolvedDependencyGraphItem
                                {
                                    LibraryDependency = currentRef,
                                    LibraryRangeIndex = currentRefRangeIndex,
                                    Parents = isChosenItemCentrallyPinnedTransitivePackage ? new HashSet<LibraryRangeIndex>() { pathToCurrentRef[pathToCurrentRef.Length - 1] } : null,
                                    Path = pathToCurrentRef,
                                    IsCentrallyPinnedTransitivePackage = isChosenItemCentrallyPinnedTransitivePackage,
                                    IsDirectPackageReferenceFromRootProject = directPackageReferenceFromRootProject,
                                    SuppressionsAndVersionOverrides = new List<SuppressionsAndVersionOverrides>
                                    {
                                        new SuppressionsAndVersionOverrides
                                        {
                                            Suppressions = currentSuppressions!,
                                            VersionOverrides = currentOverrides
                                        }
                                    }
                                });

                            //if we are going to live with this queue and chosen state, we need to also kick
                            // any queue members who were descendants of the thing we just evicted.
                            var newRefImport =
                                new Queue<DependencyGraphItem>(DependencyGraphItemQueueSize);
                            while (refImport.Count > 0)
                            {
                                DependencyGraphItem item = refImport.Dequeue();
                                if (!item.Path.Contains(evictedLR))
                                    newRefImport.Enqueue(item);
                            }
                            refImport = newRefImport;
                        }
                        //if its lower we'll never do anything other than skip it.
                        else if (!VersionRangePreciseEquals(ovr, nvr))
                        {
                            continue;
                        }
                        else
                        //we are looking at same.  consider if its an upgrade.
                        {
                            chosenResolvedItem.Parents?.Add(pathToCurrentRef[pathToCurrentRef.Length - 1]);

                            //If the one we already have chosen is pure, then we can skip this one.  Processing it wont bring any new info
                            if ((chosenSuppressions.Count == 1) && (chosenSuppressions[0].Suppressions.Count == 0) &&
                                (chosenSuppressions[0].VersionOverrides.Count == 0))
                            {
                                continue;
                            }
                            //if the one we are now looking at is pure, then we should replace the one we have chosen because if we're here it isnt pure.
                            else if ((currentSuppressions!.Count == 0) && (currentOverrides.Count == 0))
                            {
                                chosenResolvedItems.Remove(currentRefDependencyIndex);

                                //slightly evil, but works.. we should just shift to the current thing as ref?
                                chosenResolvedItems.Add(
                                    currentRefDependencyIndex,
                                    new ResolvedDependencyGraphItem
                                    {
                                        LibraryDependency = currentRef,
                                        LibraryRangeIndex = currentRefRangeIndex,
                                        Parents = chosenResolvedItem.Parents,
                                        Path = pathToCurrentRef,
                                        IsCentrallyPinnedTransitivePackage = isChosenItemCentrallyPinnedTransitivePackage,
                                        IsDirectPackageReferenceFromRootProject = packageReferenceFromRootProject,
                                        SuppressionsAndVersionOverrides = new List<SuppressionsAndVersionOverrides>
                                        {
                                            new SuppressionsAndVersionOverrides
                                            {
                                                Suppressions = currentSuppressions,
                                                VersionOverrides = currentOverrides
                                            }
                                        }
                                    });
                            }
                            else
                            //check to see if we are equal to one of the dispositions or if we are less restrictive than one
                            {
                                bool isEqualOrSuperSetDisposition = false;
                                foreach (var chosenImportDisposition in chosenSuppressions)
                                {
                                    bool localIsEqualOrSuperSetDisposition = currentSuppressions.IsSupersetOf(chosenImportDisposition.Suppressions);

                                    bool localIsEqualOrSuperSetOverride = currentOverrides.Count >= chosenImportDisposition.VersionOverrides.Count;
                                    if (localIsEqualOrSuperSetOverride)
                                    {
                                        foreach (var chosenOverride in chosenImportDisposition.VersionOverrides)
                                        {
                                            if (!currentOverrides.TryGetValue(chosenOverride.Key, out VersionRange? currentOverride))
                                            {
                                                localIsEqualOrSuperSetOverride = false;
                                                break;
                                            }
                                            if (!VersionRangePreciseEquals(currentOverride, chosenOverride.Value))
                                            {
                                                localIsEqualOrSuperSetOverride = false;
                                                break;
                                            }
                                        }
                                    }

                                    if (localIsEqualOrSuperSetDisposition && localIsEqualOrSuperSetOverride)
                                    {
                                        isEqualOrSuperSetDisposition = true;
                                    }
                                }

                                chosenResolvedItem.Parents?.Add(pathToCurrentRef[pathToCurrentRef.Length - 1]);

                                if (isEqualOrSuperSetDisposition)
                                {
                                    continue;
                                }
                                else
                                {
                                    //case of differently restrictive dispositions or less restrictive... we should technically be able to remove
                                    //a disposition if its less restrictive than another.  But we'll just add it to the list.
                                    chosenResolvedItems.Remove(currentRefDependencyIndex);
                                    var newImportDisposition =
                                        new List<SuppressionsAndVersionOverrides> {
                                            new SuppressionsAndVersionOverrides
                                            {
                                                Suppressions = currentSuppressions,
                                                VersionOverrides = currentOverrides
                                            }
                                        };
                                    newImportDisposition.AddRange(chosenSuppressions);
                                    //slightly evil, but works.. we should just shift to the current thing as ref?
                                    chosenResolvedItems.Add(
                                        currentRefDependencyIndex,
                                        new ResolvedDependencyGraphItem
                                        {
                                            LibraryDependency = currentRef,
                                            LibraryRangeIndex = currentRefRangeIndex,
                                            Parents = chosenResolvedItem.Parents,
                                            Path = pathToCurrentRef,
                                            IsCentrallyPinnedTransitivePackage = isChosenItemCentrallyPinnedTransitivePackage,
                                            IsDirectPackageReferenceFromRootProject = packageReferenceFromRootProject,
                                            SuppressionsAndVersionOverrides = newImportDisposition
                                        });
                                }
                            }
                        }
                    }
                    else
                    {
                        //This is now the thing we think is the highest version of this ref
                        chosenResolvedItems.Add(
                            currentRefDependencyIndex,
                            new ResolvedDependencyGraphItem
                            {
                                LibraryDependency = currentRef,
                                LibraryRangeIndex = currentRefRangeIndex,
                                Parents = isCentrallyPinnedTransitivePackage ? new HashSet<LibraryRangeIndex>() { pathToCurrentRef[pathToCurrentRef.Length - 1] } : null,
                                Path = pathToCurrentRef,
                                IsCentrallyPinnedTransitivePackage = isCentrallyPinnedTransitivePackage,
                                IsDirectPackageReferenceFromRootProject = directPackageReferenceFromRootProject,
                                SuppressionsAndVersionOverrides = new List<SuppressionsAndVersionOverrides>
                                {
                                    new SuppressionsAndVersionOverrides
                                    {
                                        Suppressions = currentSuppressions!,
                                        VersionOverrides = currentOverrides
                                    }
                                }
                            });
                    }

                    if (!findLibraryEntryCache.TryGetValue(libraryRangeOfCurrentRef, out FindLibraryEntryResult? refItemResult))
                    {
                        GraphItem<RemoteResolveResult> refItem = ResolverUtility.FindLibraryEntryAsync(
                                currentRef.LibraryRange,
                                newRTG.Framework,
                                runtimeIdentifier: null,
                                context,
                                CancellationToken.None).GetAwaiter().GetResult();

                        refItemResult = new FindLibraryEntryResult(
                            currentRef,
                            refItem,
                            currentRefDependencyIndex,
                            currentRefRangeIndex,
                            libraryDependencyInterningTable,
                            libraryRangeInterningTable);

                        findLibraryEntryCache.Add(libraryRangeOfCurrentRef, refItemResult);

                        // If the package came from a remote library provider, it needs to be installed locally.
                        var isRemote = context.RemoteLibraryProviders.Contains(refItem.Data.Match.Provider);
                        if (isRemote)
                        {
                            newRTG.Install.Add(refItem.Data.Match);
                        }
                    }

                    HashSet<LibraryDependencyIndex>? suppressions = default;
                    IReadOnlyDictionary<LibraryDependencyIndex, VersionRange>? finalVersionOverrides = default;
                    Dictionary<LibraryDependencyIndex, VersionRange>? newOverrides = default;
                    //Scan for suppressions and overrides
                    for (int i = 0; i < refItemResult.Item.Data.Dependencies.Count; i++)
                    {
                        var dep = refItemResult.Item.Data.Dependencies[i];
                        LibraryDependencyIndex depIndex = refItemResult.GetDependencyIndexForDependency(i);
                        if ((dep.SuppressParent == LibraryIncludeFlags.All) && (isProject == false))
                        {
                            if (suppressions == null)
                            {
                                suppressions = new HashSet<LibraryDependencyIndex>();
                            }
                            suppressions.Add(depIndex);
                        }

                        if (isCentralPackageTransitivePinningEnabled)
                        {
                            bool isTransitive = currentRefRangeIndex != rootProjectRefItem.LibraryRangeIndex && dep.LibraryRange.TypeConstraint == LibraryDependencyTarget.Package;
                            bool isPinned = pinnedPackageVersions != null && pinnedPackageVersions.ContainsKey(depIndex);

                            if (dep.VersionOverride != null && (!isTransitive || !isPinned))
                            {
                                if (newOverrides == null)
                                {
                                    newOverrides = new Dictionary<LibraryDependencyIndex, VersionRange>(OverridesDictionarySize);
                                }
                                newOverrides[depIndex] = dep.VersionOverride;
                            }
                        }
                        else if (dep.VersionOverride != null)
                        {
                            if (newOverrides == null)
                            {
                                newOverrides = new Dictionary<LibraryDependencyIndex, VersionRange>(OverridesDictionarySize);
                            }
                            newOverrides[depIndex] = dep.VersionOverride;
                        }
                    }

                    // If the override set has been mutated, then add the rest of the overrides.
                    // Otherwise, just use the incoming set of overrides.
                    if (newOverrides != null)
                    {
                        Dictionary<LibraryDependencyIndex, VersionRange> allOverrides =
                            new Dictionary<LibraryDependencyIndex, VersionRange>(currentOverrides.Count + newOverrides.Count);
                        allOverrides.AddRange(currentOverrides);
                        foreach (var overridePair in newOverrides)
                        {
                            allOverrides[overridePair.Key] = overridePair.Value;
                        }
                        finalVersionOverrides = allOverrides;
                    }
                    else
                    {
                        finalVersionOverrides = currentOverrides;
                    }

                    // If the suppressions have been mutated, then add the rest of the suppressions.
                    // Otherwise just use teh incoming set of suppressions.
                    if (suppressions != null)
                    {
                        suppressions.AddRange(currentSuppressions);
                    }
                    else
                    {
                        suppressions = currentSuppressions;
                    }

                    for (int i = 0; i < refItemResult.Item.Data.Dependencies.Count; i++)
                    {
                        var dep = refItemResult.Item.Data.Dependencies[i];
                        LibraryDependencyIndex depIndex = refItemResult.GetDependencyIndexForDependency(i);

                        //Suppress this node
                        if (suppressions!.Contains(depIndex))
                        {
                            continue;
                        }

                        bool isCentrallyPinnedTransitiveDependency = false;
                        bool isDirectPackageReferenceFromRootProject = (currentRefRangeIndex == rootProjectRefItem.LibraryRangeIndex) && (dep.LibraryRange.TypeConstraint == LibraryDependencyTarget.Package);

                        LibraryRangeIndex rangeIndex = LibraryRangeIndex.Invalid;

                        LibraryDependency actualLibraryDependency = dep;

                        if (!isDirectPackageReferenceFromRootProject && isCentralPackageTransitivePinningEnabled && pinnedPackageVersions?.TryGetValue(depIndex, out VersionRange? pinnedPackageVersion) == true)
                        {
                            actualLibraryDependency = dep.Clone();

                            actualLibraryDependency.LibraryRange.VersionRange = pinnedPackageVersion;

                            isCentrallyPinnedTransitiveDependency = true;

                            rangeIndex = libraryRangeInterningTable.Intern(actualLibraryDependency.LibraryRange);
                        }
                        else
                        {
                            rangeIndex = refItemResult.GetRangeIndexForDependency(i);
                        }

                        refImport.Enqueue(new DependencyGraphItem()
                        {
                            LibraryDependency = actualLibraryDependency,
                            LibraryDependencyIndex = depIndex,
                            LibraryRangeIndex = rangeIndex,
                            Path = LibraryRangeInterningTable.CreatePathToRef(pathToCurrentRef, libraryRangeOfCurrentRef),
                            Suppressions = suppressions,
                            VersionOverrides = finalVersionOverrides,
                            IsDirectPackageReferenceFromRootProject = isDirectPackageReferenceFromRootProject,
                            IsCentrallyPinnedTransitivePackage = isCentrallyPinnedTransitiveDependency
                        });
                    }

                    // Add runtime dependencies of the current node if a runtime identifier has been specified.
                    if (!string.IsNullOrEmpty(pair.RuntimeIdentifier))
                    {
                        // Check for runtime dependencies.
                        HashSet<LibraryDependency> runtimeDependencies = new HashSet<LibraryDependency>();
                        LibraryRange libraryRange = currentRef.LibraryRange;

                        FindLibraryEntryResult? findLibraryCachedAsyncResult = default;
                        RemoteDependencyWalker.EvaluateRuntimeDependencies(ref libraryRange, pair.RuntimeIdentifier, runtimeGraph, ref runtimeDependencies);

                        if (runtimeDependencies.Count > 0)
                        {
                            // Runtime dependencies start after non-runtime dependencies.
                            // Keep track of the first index for any runtime dependencies so that it can be used to enqueue later.
                            int runtimeDependencyIndex = refItemResult.Item.Data.Dependencies.Count;

                            // If there are runtime dependencies that need to be added, remove the currentRef from allResolvedItems,
                            // and add the newly created version that contains the previously detected dependencies and newly detected runtime dependencies.
                            findLibraryEntryCache.Remove(libraryRangeOfCurrentRef);
                            bool rootHasInnerNodes = (refItemResult.Item.Data.Dependencies.Count + (runtimeDependencies == null ? 0 : runtimeDependencies.Count)) > 0;
                            GraphNode<RemoteResolveResult> rootNode = new GraphNode<RemoteResolveResult>(libraryRange, rootHasInnerNodes, false)
                            {
                                Item = refItemResult.Item,
                            };
                            RemoteDependencyWalker.MergeRuntimeDependencies(runtimeDependencies, rootNode);
                            findLibraryCachedAsyncResult = new FindLibraryEntryResult(
                                currentRef,
                                rootNode.Item,
                                currentRefDependencyIndex,
                                currentRefRangeIndex,
                                libraryDependencyInterningTable,
                                libraryRangeInterningTable);
                            findLibraryEntryCache.Add(libraryRangeOfCurrentRef, findLibraryCachedAsyncResult);

                            // Enqueue each of the runtime dependencies, but only if they weren't already present in refItemResult before merging the runtime dependencies above.
                            if ((rootNode.Item.Data.Dependencies.Count - runtimeDependencyIndex) == runtimeDependencies!.Count)
                            {
                                foreach (var dep in runtimeDependencies)
                                {
                                    refImport.Enqueue(new DependencyGraphItem()
                                    {
                                        LibraryDependency = dep,
                                        LibraryDependencyIndex = findLibraryCachedAsyncResult.GetDependencyIndexForDependency(runtimeDependencyIndex),
                                        LibraryRangeIndex = findLibraryCachedAsyncResult.GetRangeIndexForDependency(runtimeDependencyIndex),
                                        Path = LibraryRangeInterningTable.CreatePathToRef(pathToCurrentRef, libraryRangeOfCurrentRef),
                                        Suppressions = suppressions,
                                        VersionOverrides = finalVersionOverrides,
                                        IsDirectPackageReferenceFromRootProject = false,
                                    });

                                    runtimeDependencyIndex++;
                                }
                            }
                        }
                    }
                }

                //Now that we've completed import, figure out the short real flattened list
                var newFlattened = new HashSet<GraphItem<RemoteResolveResult>>();
                HashSet<LibraryDependencyIndex> visitedItems = new HashSet<LibraryDependencyIndex>();
                Queue<(LibraryDependencyIndex, GraphNode<RemoteResolveResult>)> itemsToFlatten = new Queue<(LibraryDependencyIndex, GraphNode<RemoteResolveResult>)>();
                var nGraph = new List<GraphNode<RemoteResolveResult>>();

                LibraryDependencyIndex initialProjectIndex = rootProjectRefItem.LibraryDependencyIndex;
                var cri = chosenResolvedItems[initialProjectIndex];
                LibraryDependency startRef = cri.LibraryDependency;

                var rootGraphNode = new GraphNode<RemoteResolveResult>(startRef.LibraryRange);
                LibraryRangeIndex startRefLibraryRangeIndex = cri.LibraryRangeIndex;
                rootGraphNode.Item = findLibraryEntryCache[startRefLibraryRangeIndex].Item;
                nGraph.Add(rootGraphNode);

                var nodesById = new Dictionary<LibraryRangeIndex, GraphNode<RemoteResolveResult>>();

                var downgrades = new Dictionary<LibraryRangeIndex, (GraphNode<RemoteResolveResult> OuterNode, LibraryDependency LibraryDependency)>();

                var versionConflicts = new Dictionary<LibraryRangeIndex, GraphNode<RemoteResolveResult>>();

                itemsToFlatten.Enqueue((initialProjectIndex, rootGraphNode));
                while (itemsToFlatten.Count > 0)
                {
                    (LibraryDependencyIndex currentDependencyIndex, GraphNode<RemoteResolveResult> currentGraphNode) = itemsToFlatten.Dequeue();
                    if (!chosenResolvedItems.TryGetValue(currentDependencyIndex, out var foundItem))
                    {
                        continue;
                    }
                    LibraryDependency chosenRef = foundItem.LibraryDependency;
                    LibraryRangeIndex chosenRefRangeIndex = foundItem.LibraryRangeIndex;
                    LibraryRangeIndex[] pathToChosenRef = foundItem.Path;
                    bool directPackageReferenceFromRootProject = foundItem.IsDirectPackageReferenceFromRootProject;
                    List<SuppressionsAndVersionOverrides> chosenSuppressions = foundItem.SuppressionsAndVersionOverrides;

                    if (findLibraryEntryCache.TryGetValue(chosenRefRangeIndex, out var node))
                    {
                        newFlattened.Add(node.Item);

                        for (int i = 0; i < node.Item.Data.Dependencies.Count; i++)
                        {
                            var dep = node.Item.Data.Dependencies[i];
                            if (StringComparer.OrdinalIgnoreCase.Equals(dep.Name, node.Item.Key.Name) || StringComparer.OrdinalIgnoreCase.Equals(dep.Name, rootGraphNode.Key.Name))
                            {
                                // Cycle
                                var nodeWithCycle = new GraphNode<RemoteResolveResult>(dep.LibraryRange)
                                {
                                    OuterNode = currentGraphNode,
                                    Disposition = Disposition.Cycle
                                };

                                newRTG.AnalyzeResult.Cycles.Add(nodeWithCycle);

                                continue;
                            }

                            LibraryDependencyIndex depIndex = node.GetDependencyIndexForDependency(i);

                            if (!chosenResolvedItems.TryGetValue(depIndex, out var chosenItem))
                            {
                                continue;
                            }

                            var chosenItemRangeIndex = chosenItem.LibraryRangeIndex;
                            LibraryDependency actualDep = chosenItem.LibraryDependency;

                            if (!visitedItems.Add(depIndex))
                            {
                                LibraryRangeIndex currentRangeIndex = node.GetRangeIndexForDependency(i);

                                if (pathToChosenRef.Contains(currentRangeIndex))
                                {
                                    // Cycle
                                    var nodeWithCycle = new GraphNode<RemoteResolveResult>(dep.LibraryRange);
                                    nodeWithCycle.OuterNode = currentGraphNode;
                                    nodeWithCycle.Disposition = Disposition.Cycle;
                                    newRTG.AnalyzeResult.Cycles.Add(nodeWithCycle);

                                    continue;
                                }

                                if (!RemoteDependencyWalker.IsGreaterThanOrEqualTo(actualDep.LibraryRange.VersionRange, dep.LibraryRange.VersionRange))
                                {
                                    if (node.DependencyIndex != rootProjectRefItem.LibraryDependencyIndex && dep.SuppressParent == LibraryIncludeFlags.All)
                                    {
                                        continue;
                                    }

                                    if (chosenSuppressions.Count > 0 && chosenSuppressions[0].Suppressions.Contains(depIndex))
                                    {
                                        continue;
                                    }

                                    // Downgrade
                                    if (!downgrades.ContainsKey(chosenItemRangeIndex))
                                    {
                                        downgrades.Add(chosenItemRangeIndex, (currentGraphNode, dep));
                                    }

                                    continue;
                                }

                                if (versionConflicts.ContainsKey(chosenItemRangeIndex) && !nodesById.ContainsKey(currentRangeIndex))
                                {
                                    // Version conflict
                                    var selectedConflictingNode = new GraphNode<RemoteResolveResult>(actualDep.LibraryRange)
                                    {
                                        Item = findLibraryEntryCache[chosenItemRangeIndex].Item,
                                        Disposition = Disposition.Acceptable,
                                        OuterNode = currentGraphNode,
                                    };
                                    currentGraphNode.InnerNodes.Add(selectedConflictingNode);

                                    nodesById.Add(currentRangeIndex, selectedConflictingNode);

                                    continue;
                                }

                                continue;
                            }

                            var newGraphNode = new GraphNode<RemoteResolveResult>(actualDep.LibraryRange);
                            newGraphNode.Item = findLibraryEntryCache[chosenItemRangeIndex].Item;

                            if (chosenItem.IsCentrallyPinnedTransitivePackage)
                            {
                                newGraphNode.Disposition = Disposition.Accepted;
                                newGraphNode.Item.IsCentralTransitive = true;
                                newGraphNode.OuterNode = rootGraphNode;
                                rootGraphNode.InnerNodes.Add(newGraphNode);
                            }
                            else
                            {
                                newGraphNode.OuterNode = currentGraphNode;
                                currentGraphNode.InnerNodes.Add(newGraphNode);
                            }

                            if (dep.SuppressParent != LibraryIncludeFlags.All && isCentralPackageTransitivePinningEnabled && !downgrades.ContainsKey(chosenItemRangeIndex) && !RemoteDependencyWalker.IsGreaterThanOrEqualTo(chosenItem.LibraryDependency.LibraryRange.VersionRange, dep.LibraryRange.VersionRange))
                            {
                                downgrades.Add(chosenItem.LibraryRangeIndex, (currentGraphNode, dep));
                            }

                            if (newGraphNode.Item.Key.Type != LibraryType.Project && newGraphNode.Item.Key.Type != LibraryType.ExternalProject && newGraphNode.Item.Key.Type != LibraryType.Unresolved && !versionConflicts.ContainsKey(chosenItemRangeIndex) && dep.SuppressParent != LibraryIncludeFlags.All && dep.LibraryRange.VersionRange != null && !dep.LibraryRange.VersionRange!.Satisfies(newGraphNode.Item.Key.Version) && !downgrades.ContainsKey(chosenItemRangeIndex))
                            {
                                currentGraphNode.InnerNodes.Remove(newGraphNode);

                                // Conflict
                                var conflictingNode = new GraphNode<RemoteResolveResult>(dep.LibraryRange)
                                {
                                    Disposition = Disposition.Acceptable
                                };

                                conflictingNode.Item = new GraphItem<RemoteResolveResult>(new LibraryIdentity(dep.Name, dep.LibraryRange.VersionRange.MinVersion!, LibraryType.Package));
                                currentGraphNode.InnerNodes.Add(conflictingNode);
                                conflictingNode.OuterNode = currentGraphNode;

                                versionConflicts.Add(chosenItemRangeIndex, conflictingNode);

                                continue;
                            }

                            nodesById.Add(chosenItemRangeIndex, newGraphNode);
                            itemsToFlatten.Enqueue((depIndex, newGraphNode));

                            if (newGraphNode.Item.Key.Type == LibraryType.Unresolved)
                            {
                                newRTG.Unresolved.Add(actualDep.LibraryRange);

                                _success = false;

                                continue;
                            }

                            newRTG.ResolvedDependencies.Add(new ResolvedDependencyKey(
                                parent: newGraphNode.OuterNode.Item.Key,
                                range: newGraphNode.Key.VersionRange,
                                child: newGraphNode.Item.Key));
                        }
                    }
                }

                if (versionConflicts.Count > 0)
                {
                    foreach (var versionConflict in versionConflicts)
                    {
                        if (nodesById.TryGetValue(versionConflict.Key, out var selected))
                        {
                            newRTG.AnalyzeResult.VersionConflicts.Add(new VersionConflictResult<RemoteResolveResult>
                            {
                                Conflicting = versionConflict.Value,
                                Selected = selected
                            });
                        }
                    }
                }

                if (downgrades.Count > 0)
                {
                    foreach (var downgrade in downgrades)
                    {
                        if (!nodesById.TryGetValue(downgrade.Key, out var downgradedTo))
                        {
                            continue;
                        }

                        var downgradedFrom = new GraphNode<RemoteResolveResult>(downgrade.Value.LibraryDependency.LibraryRange)
                        {
                            Item = new GraphItem<RemoteResolveResult>(new LibraryIdentity(downgrade.Value.LibraryDependency.Name, downgrade.Value.LibraryDependency.LibraryRange.VersionRange?.MinVersion!, LibraryType.Package)),
                            OuterNode = downgrade.Value.OuterNode
                        };

                        newRTG.AnalyzeResult.Downgrades.Add(new DowngradeResult<RemoteResolveResult>
                        {
                            DowngradedFrom = downgradedFrom,
                            DowngradedTo = downgradedTo
                        });
                    }
                }

                if (isCentralPackageTransitivePinningEnabled)
                {
                    foreach (KeyValuePair<LibraryDependencyIndex, ResolvedDependencyGraphItem> item in chosenResolvedItems)
                    {
                        ResolvedDependencyGraphItem chosenResolvedItem = item.Value;

                        if (!chosenResolvedItem.IsCentrallyPinnedTransitivePackage || chosenResolvedItem.Parents == null || chosenResolvedItem.Parents.Count == 0)
                        {
                            continue;
                        }

                        if (nodesById.TryGetValue(chosenResolvedItem.LibraryRangeIndex, out GraphNode<RemoteResolveResult>? currentNode))
                        {
                            foreach (LibraryRangeIndex parent in chosenResolvedItem.Parents)
                            {
                                if (nodesById.TryGetValue(parent, out GraphNode<RemoteResolveResult>? parentNode))
                                {
                                    currentNode.ParentNodes.Add(parentNode);
                                }
                            }
                        }
                    }
                }

                newRTG.Flattened = newFlattened;

                newRTG.Graphs = nGraph;

                allGraphs.Add(newRTG);

                if (!string.IsNullOrWhiteSpace(pair.RuntimeIdentifier))
                {
                    runtimeGraphs.Add(newRTG);
                }

                if (string.IsNullOrEmpty(pair.RuntimeIdentifier))
                {
                    graphByTFM.Add(pair.Framework, newRTG);
                }
            }

            _telemetryActivity.EndIntervalMeasure(ProjectRestoreCommand.WalkFrameworkDependencyDuration);

            if (!hasInstallBeenCalledAlready)
            {
                downloadDependencyResolutionResults = await ProjectRestoreCommand.DownloadDependenciesAsync(_request.Project, context, _telemetryActivity, telemetryPrefix: string.Empty, token);

                _success &= await projectRestoreCommand.InstallPackagesAsync(uniquePackages, allGraphs, downloadDependencyResolutionResults, userPackageFolder, token);

                hasInstallBeenCalledAlready = true;
            }

            if (runtimeGraphs.Count > 0)
            {
                _success &= await projectRestoreCommand.InstallPackagesAsync(uniquePackages, runtimeGraphs, Array.Empty<DownloadDependencyResolutionResult>(), userPackageFolder, token);
            }

            foreach (var profile in _request.Project.RuntimeGraph.Supports)
            {
                var runtimes = allRuntimes;

                CompatibilityProfile? compatProfile;
                if (profile.Value.RestoreContexts.Any())
                {
                    // Just use the contexts from the project definition
                    compatProfile = profile.Value;
                }
                else if (!runtimes.Supports.TryGetValue(profile.Value.Name, out compatProfile))
                {
                    // No definition of this profile found, so just continue to the next one
                    var message = string.Format(CultureInfo.CurrentCulture, Strings.Log_UnknownCompatibilityProfile, profile.Key);

                    await _logger.LogAsync(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1502, message));
                    continue;
                }

                foreach (var frameworkRuntimePair in compatProfile.RestoreContexts)
                {
                    _logger.LogDebug($" {profile.Value.Name} -> +{frameworkRuntimePair}");
                    _request.CompatibilityProfiles.Add(frameworkRuntimePair);
                }
            }

            // Update the logger with the restore target graphs
            // This allows lazy initialization for the Transitive Warning Properties
            _logger.ApplyRestoreOutput(allGraphs);

            await UnexpectedDependencyMessages.LogAsync(allGraphs, _request.Project, _logger);

            _success &= await projectRestoreCommand.ResolutionSucceeded(allGraphs, downloadDependencyResolutionResults, context, token);

            return (_success, allGraphs, allRuntimes);
        }

        private static bool EvictOnTypeConstraint(LibraryDependencyTarget current, LibraryDependencyTarget previous)
        {
            if (current == previous)
            {
                return false;
            }

            if (previous == LibraryDependencyTarget.PackageProjectExternal)
            {
                LibraryDependencyTarget ppeFlags = current & LibraryDependencyTarget.PackageProjectExternal;
                LibraryDependencyTarget nonPpeFlags = current & ~LibraryDependencyTarget.PackageProjectExternal;
                return (ppeFlags != LibraryDependencyTarget.None && nonPpeFlags == LibraryDependencyTarget.None);
            }

            // TODO: Should there be other cases here?
            return false;
        }

        private static bool VersionRangePreciseEquals(VersionRange a, VersionRange b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            if ((a.MinVersion != null) != (b.MinVersion != null))
            {
                return false;
            }
            if (a.MinVersion != b.MinVersion)
            {
                return false;
            }
            if ((a.MaxVersion != null) != (b.MaxVersion != null))
            {
                return false;
            }
            if (a.MaxVersion != b.MaxVersion)
            {
                return false;
            }
            if (a.IsMinInclusive != b.IsMinInclusive)
            {
                return false;
            }
            if (a.IsMaxInclusive != b.IsMaxInclusive)
            {
                return false;
            }
            if ((a.Float != null) != (b.Float != null))
            {
                return false;
            }
            if (a.Float != b.Float)
            {
                return false;
            }

            return true;
        }

        private struct ResolvedDependencyGraphItem
        {
            public bool IsCentrallyPinnedTransitivePackage { get; set; }

            public bool IsDirectPackageReferenceFromRootProject { get; set; }

            public LibraryDependency LibraryDependency { get; set; }

            public LibraryRangeIndex LibraryRangeIndex { get; set; }

            public HashSet<LibraryRangeIndex>? Parents { get; set; }

            public LibraryRangeIndex[] Path { get; set; }

            public List<SuppressionsAndVersionOverrides> SuppressionsAndVersionOverrides { get; set; }
        }

        private struct SuppressionsAndVersionOverrides
        {
            public HashSet<LibraryDependencyIndex> Suppressions { get; set; }

            public IReadOnlyDictionary<LibraryDependencyIndex, VersionRange> VersionOverrides { get; set; }
        }

        internal sealed class LibraryDependencyInterningTable
        {
            private readonly Dictionary<string, LibraryDependencyIndex> _table = new Dictionary<string, LibraryDependencyIndex>(StringComparer.OrdinalIgnoreCase);
            private int _nextIndex = 0;

            public enum LibraryDependencyIndex : int
            {
                Invalid = -1,
            }

            public LibraryDependencyIndex Intern(LibraryDependency libraryDependency)
            {
                string key = libraryDependency.Name;
                if (!_table.TryGetValue(key, out LibraryDependencyIndex index))
                {
                    index = (LibraryDependencyIndex)_nextIndex++;
                    _table.Add(key, index);
                }

                return index;
            }

            public LibraryDependencyIndex Intern(CentralPackageVersion centralPackageVersion)
            {
                string key = centralPackageVersion.Name;
                if (!_table.TryGetValue(key, out LibraryDependencyIndex index))
                {
                    index = (LibraryDependencyIndex)_nextIndex++;
                    _table.Add(key, index);
                }

                return index;
            }
        }

        internal sealed class LibraryRangeInterningTable
        {
            private readonly Dictionary<string, LibraryRangeIndex> _table = new Dictionary<string, LibraryRangeIndex>(StringComparer.OrdinalIgnoreCase);
            private int _nextIndex = 0;

            public enum LibraryRangeIndex : int
            {
                Invalid = -1,
            }

            public LibraryRangeIndex Intern(LibraryRange libraryRange)
            {
                string key = libraryRange.ToString();
                if (!_table.TryGetValue(key, out LibraryRangeIndex index))
                {
                    index = (LibraryRangeIndex)_nextIndex++;
                    _table.Add(key, index);
                }

                return index;
            }

            internal static LibraryRangeIndex[] CreatePathToRef(LibraryRangeIndex[] existingPath, LibraryRangeIndex currentRef)
            {
                LibraryRangeIndex[] newPath = new LibraryRangeIndex[existingPath.Length + 1];
                Array.Copy(existingPath, newPath, existingPath.Length);
                newPath[newPath.Length - 1] = currentRef;

                return newPath;
            }
        }

        [DebuggerDisplay("{LibraryDependency}")]
        private class DependencyGraphItem
        {
            public bool IsCentrallyPinnedTransitivePackage { get; set; }

            public bool IsDirectPackageReferenceFromRootProject { get; set; }

            public LibraryDependency? LibraryDependency { get; set; }

            public LibraryDependencyIndex LibraryDependencyIndex { get; set; } = LibraryDependencyIndex.Invalid;

            public LibraryRangeIndex LibraryRangeIndex { get; set; } = LibraryRangeIndex.Invalid;

            public LibraryRangeIndex[] Path { get; set; } = Array.Empty<LibraryRangeIndex>();

            public HashSet<LibraryDependencyIndex>? Suppressions { get; set; }

            public IReadOnlyDictionary<LibraryDependencyIndex, VersionRange>? VersionOverrides { get; set; }
        }

        private class FindLibraryEntryResult
        {
            private LibraryDependencyIndex[] _dependencyIndices;
            private LibraryRangeIndex[] _rangeIndices;

            public FindLibraryEntryResult(
                LibraryDependency libraryDependency,
                GraphItem<RemoteResolveResult> resolvedItem,
                LibraryDependencyIndex itemDependencyIndex,
                LibraryRangeIndex itemRangeIndex,
                LibraryDependencyInterningTable libraryDependencyInterningTable,
                LibraryRangeInterningTable libraryRangeInterningTable)
            {
                Item = resolvedItem;
                DependencyIndex = itemDependencyIndex;
                RangeIndex = itemRangeIndex;
                int dependencyCount = resolvedItem.Data.Dependencies.Count;
                _dependencyIndices = new LibraryDependencyIndex[dependencyCount];
                _rangeIndices = new LibraryRangeIndex[dependencyCount];
                for (int i = 0; i < dependencyCount; i++)
                {
                    LibraryDependency dependency = resolvedItem.Data.Dependencies[i];
                    _dependencyIndices[i] = libraryDependencyInterningTable.Intern(dependency);
                    _rangeIndices[i] = libraryRangeInterningTable.Intern(dependency.LibraryRange);
                }
            }

            public LibraryDependencyIndex DependencyIndex { get; }

            public GraphItem<RemoteResolveResult> Item { get; }

            public LibraryRangeIndex RangeIndex { get; }

            public LibraryDependencyIndex GetDependencyIndexForDependency(int dependencyIndex)
            {
                return _dependencyIndices[dependencyIndex];
            }

            public LibraryRangeIndex GetRangeIndexForDependency(int dependencyIndex)
            {
                return _rangeIndices[dependencyIndex];
            }
        }
    }
}
