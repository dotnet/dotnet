// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks.Dataflow;
using EnvDTE;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.VS;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.AttachedCollections;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using NuGet.VisualStudio.SolutionExplorer.Models;

namespace NuGet.VisualStudio.SolutionExplorer
{
    /// <summary>
    /// Base class for attaching children to top-level dependencies that come from the assets file.
    /// </summary>
    /// <remarks>
    /// Templates out common code with a bunch of protected methods to override for specific item types.
    /// </remarks>
    internal abstract class AssetsFileTopLevelDependenciesCollectionSourceProvider<TItem> : DependenciesAttachedCollectionSourceProviderBase
        where TItem : class, IRelatableItem
    {
        protected AssetsFileTopLevelDependenciesCollectionSourceProvider(ProjectTreeFlags flags)
            : base(flags)
        {
        }

        protected abstract bool TryGetLibraryName(Properties properties, [NotNullWhen(returnValue: true)] out string? libraryName);

        protected abstract bool TryGetLibrary(AssetsFileTarget target, string libraryName, [NotNullWhen(returnValue: true)] out AssetsFileTargetLibrary? library);

        protected abstract TItem CreateItem(AssetsFileTarget targetData, AssetsFileTargetLibrary library);

        protected abstract bool TryUpdateItem(TItem item, AssetsFileTarget targetData, AssetsFileTargetLibrary library);

        protected override bool TryCreateCollectionSource(
            IVsHierarchyItem hierarchyItem,
            string flagsString,
            string? target,
            IRelationProvider relationProvider,
            [NotNullWhen(returnValue: true)] out AggregateRelationCollectionSource? containsCollectionSource)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            string? libraryName;

            if (!ErrorHandler.Succeeded(hierarchyItem.HierarchyIdentity.Hierarchy.GetProperty(
                hierarchyItem.HierarchyIdentity.ItemID, (int)__VSHPROPID.VSHPROPID_ExtObject, out object projectItemObject)))
            {
                containsCollectionSource = null;
                return false;
            }
            else
            {
                Properties? properties = (projectItemObject as ProjectItem)?.Properties;

                if (properties == null || !TryGetLibraryName(properties, out libraryName))
                {
                    containsCollectionSource = null;
                    return false;
                }
            }

            UnconfiguredProject? unconfiguredProject = hierarchyItem.HierarchyIdentity.Hierarchy.AsUnconfiguredProject();

            // Find the data source
            IAssetsFileDependenciesDataSource? dataSource = unconfiguredProject?.Services.ExportProvider.GetExportedValueOrDefault<IAssetsFileDependenciesDataSource>();

            if (unconfiguredProject == null || dataSource == null)
            {
                containsCollectionSource = null;
                return false;
            }

            IProjectThreadingService projectThreadingService = unconfiguredProject.Services.ThreadingPolicy;

            projectThreadingService.VerifyOnUIThread();

            // Items for top-level dependencies do not appear in the tree directly. They "shadow" hierarchy items
            // for purposes of bridging between the hierarchy items (top-level dependencies) and attached items (transitive
            // dependencies).
            TItem? item = null;

            var collectionSource = new AggregateRelationCollectionSource(hierarchyItem);
            AggregateContainsRelationCollection? collection = null;
            AssetsFileDependenciesSnapshot? lastSnapshot = null;

            var actionBlock = new ActionBlock<IProjectVersionedValue<AssetsFileDependenciesSnapshot>>(
                async versionedValue =>
                {
                    AssetsFileDependenciesSnapshot snapshot = versionedValue.Value;

                    if (ReferenceEquals(snapshot, lastSnapshot))
                    {
                        // Skip version-only updates.
                        return;
                    }

                    lastSnapshot = snapshot;

                    if (snapshot.TryGetTarget(target, out AssetsFileTarget? targetData))
                    {
                        if (TryGetLibrary(targetData, libraryName, out AssetsFileTargetLibrary? library))
                        {
                            if (item == null)
                            {
                                // This is the first update
                                item = CreateItem(targetData, library);
                                if (AggregateContainsRelationCollection.TryCreate(item, relationProvider, out collection))
                                {
                                    await projectThreadingService.JoinableTaskContext.Factory.SwitchToMainThreadAsync();
                                    collectionSource.SetCollection(collection);
                                }
                            }
                            else if (TryUpdateItem(item, targetData, library) && collection != null)
                            {
                                await projectThreadingService.JoinableTaskContext.Factory.SwitchToMainThreadAsync();
                                collection.OnStateUpdated();
                            }
                        }
                    }
                });

            IDisposable link = dataSource.SourceBlock.LinkTo(actionBlock, new DataflowLinkOptions { PropagateCompletion = true });

            Assumes.False(hierarchyItem.IsDisposed);
            hierarchyItem.PropertyChanged += OnItemPropertyChanged;

            void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                // We are notified when the IVsHierarchyItem is removed from the tree via its INotifyPropertyChanged
                // event for the IsDisposed property. When this fires, we dispose our dataflow link and release the
                // subscription.
                if (e.PropertyName == nameof(ISupportDisposalNotification.IsDisposed) && hierarchyItem.IsDisposed)
                {
                    link.Dispose();
                    hierarchyItem.PropertyChanged -= OnItemPropertyChanged;
                }
            }

            containsCollectionSource = collectionSource;
            return true;
        }
    }
}
