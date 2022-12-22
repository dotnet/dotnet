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
    /// Backing object for transitive package reference nodes in the dependencies tree.
    /// </summary>
    internal sealed class PackageReferenceItem : RelatableItemBase
    {
        public AssetsFileTarget Target { get; private set; }
        public AssetsFileTargetLibrary Library { get; private set; }

        public PackageReferenceItem(AssetsFileTarget target, AssetsFileTargetLibrary library)
            : base(GetCaption(library))
        {
            Library = library;
            Target = target;
        }

        internal bool TryUpdateState(AssetsFileTarget target, AssetsFileTargetLibrary library)
        {
            if (ReferenceEquals(Target, target) && ReferenceEquals(Library, library))
            {
                return false;
            }

            Target = target;
            Library = library;
            Text = GetCaption(library);
            return true;
        }

        private static string GetCaption(AssetsFileTargetLibrary library) => $"{library.Name} ({library.Version})";

        public override object Identity => Library.Name;

        public override int Priority => AttachedItemPriority.Package;

        public override ImageMoniker IconMoniker => KnownMonikers.NuGetNoColor;

        protected override IContextMenuController? ContextMenuController => MenuController.TransitivePackage;

        protected override bool TryGetProjectNode(IProjectTree targetRootNode, IRelatableItem item, [NotNullWhen(returnValue: true)] out IProjectTree? projectTree)
        {
            IProjectTree? typeGroupNode = targetRootNode.FindChildWithFlags(DependencyTreeFlags.PackageDependencyGroup);

            projectTree = typeGroupNode?.FindChildWithFlags(ProjectTreeFlags.Create("$ID:" + Library.Name));

            return projectTree != null;
        }

        public override object? GetBrowseObject() => new BrowseObject(this);

        private sealed class BrowseObject : LocalizableProperties
        {
            private readonly PackageReferenceItem _item;

            public BrowseObject(PackageReferenceItem item) => _item = item;

            public override string GetComponentName() => $"{_item.Library.Name} ({_item.Library.Version})";

            public override string GetClassName() => VsResources.PackageReferenceBrowseObjectClassName;

            [BrowseObjectDisplayName(nameof(VsResources.PackageReferenceNameDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.PackageReferenceNameDescription))]
            public string Name => _item.Library.Name;

            [BrowseObjectDisplayName(nameof(VsResources.PackageReferenceVersionDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.PackageReferenceVersionDescription))]
            public string Version => _item.Library.Version;

            [BrowseObjectDisplayName(nameof(VsResources.PackageReferencePathDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.PackageReferencePathDescription))]
            public string? Path => _item.Target.TryResolvePackagePath(_item.Library.Name, _item.Library.Version, out string? fullPath) ? fullPath : null;
        }
    }
}
