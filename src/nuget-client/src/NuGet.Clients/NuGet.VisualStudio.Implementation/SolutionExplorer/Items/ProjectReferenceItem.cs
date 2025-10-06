// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Diagnostics.CodeAnalysis;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.AttachedCollections;
using Microsoft.VisualStudio.Shell;
using NuGet.VisualStudio.Implementation.Resources;
using NuGet.VisualStudio.SolutionExplorer.Models;

namespace NuGet.VisualStudio.SolutionExplorer
{
    /// <summary>
    /// Backing object for transitive project reference nodes in the dependencies tree.
    /// </summary>
    internal sealed class ProjectReferenceItem : RelatableItemBase
    {
        public AssetsFileTarget Target { get; private set; }
        public AssetsFileTargetLibrary Library { get; private set; }

        public ProjectReferenceItem(AssetsFileTarget target, AssetsFileTargetLibrary library)
            : base(library.Name)
        {
            Target = target;
            Library = library;
        }

        internal bool TryUpdateState(AssetsFileTarget target, AssetsFileTargetLibrary library)
        {
            if (ReferenceEquals(Target, target) && ReferenceEquals(Library, library))
            {
                return false;
            }

            Target = target;
            Library = library;
            return true;
        }

        public override object Identity => Library.Name;

        public override int Priority => AttachedItemPriority.Project;

        public override ImageMoniker IconMoniker => Library.LogLevel switch
        {
            NuGet.Common.LogLevel.Warning => KnownMonikers.ApplicationWarning,
            NuGet.Common.LogLevel.Error => KnownMonikers.ApplicationError,
            _ => KnownMonikers.Application
        };

        protected override IContextMenuController? ContextMenuController => MenuController.TransitiveProject;

        protected override bool TryGetProjectNode(IProjectTree targetRootNode, IRelatableItem item, [NotNullWhen(returnValue: true)] out IProjectTree? projectTree)
        {
            IProjectTree? typeGroupNode = targetRootNode.FindChildWithFlags(DependencyTreeFlags.ProjectDependencyGroup);

            projectTree = typeGroupNode?.FindChildWithFlags(ProjectTreeFlags.Create("$ID:" + Library.Name));

            return projectTree != null;
        }

        public override object? GetBrowseObject() => new BrowseObject(Library);

        private sealed class BrowseObject : LocalizableProperties
        {
            private readonly AssetsFileTargetLibrary _library;

            public BrowseObject(AssetsFileTargetLibrary library) => _library = library;

            public override string GetComponentName() => _library.Name;

            public override string GetClassName() => VsResources.ProjectReferenceBrowseObjectClassName;

            [BrowseObjectDisplayName(nameof(VsResources.ProjectReferenceNameDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.ProjectReferenceNameDescription))]
            public string Name => _library.Name;
        }
    }
}
