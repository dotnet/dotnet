// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.AttachedCollections;
using Microsoft.VisualStudio.Shell;
using NuGet.VisualStudio.Implementation.Resources;
using NuGet.VisualStudio.SolutionExplorer.Models;

namespace NuGet.VisualStudio.SolutionExplorer
{
    /// <summary>
    /// Backing object for an assembly within a library within the dependencies tree.
    /// </summary>
    /// <remarks>
    /// Items of this type are grouped within <see cref="PackageAssemblyGroupItem"/>.
    /// </remarks>
    internal sealed class PackageAssemblyItem : RelatableItemBase
    {
        public AssetsFileTarget Target { get; }
        public AssetsFileTargetLibrary Library { get; }
        public string Path { get; }
        public PackageAssemblyGroupType GroupType { get; }

        public PackageAssemblyItem(AssetsFileTarget target, AssetsFileTargetLibrary library, string path, PackageAssemblyGroupType groupType)
            : base(System.IO.Path.GetFileName(path))
        {
            Target = target;
            Library = library;
            Path = path;
            GroupType = groupType;
        }

        public override object Identity => Tuple.Create(Library.Name, Path, GroupType);

        // All siblings are assemblies, so no prioritization needed (sort alphabetically)
        public override int Priority => 0;

        public override ImageMoniker IconMoniker => KnownMonikers.Reference;

        protected override IContextMenuController? ContextMenuController => MenuController.TransitiveAssembly;

        public override object? GetBrowseObject() => new BrowseObject(this);

        private sealed class BrowseObject : LocalizableProperties
        {
            private readonly PackageAssemblyItem _item;

            public BrowseObject(PackageAssemblyItem library) => _item = library;

            public override string GetComponentName() => _item.Text;

            public override string GetClassName() => VsResources.PackageAssemblyBrowseObjectClassName;

            [BrowseObjectDisplayName(nameof(VsResources.PackageAssemblyNameDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.PackageAssemblyNameDescription))]
            public string Name => _item.Text;

            [BrowseObjectDisplayName(nameof(VsResources.PackageAssemblyPathDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.PackageAssemblyPathDescription))]
            public string? Path
            {
                get
                {
                    return _item.Target.TryResolvePackagePath(_item.Library.Name, _item.Library.Version, out string? fullPath)
                        ? System.IO.Path.GetFullPath(System.IO.Path.Combine(fullPath, _item.Path))
                        : null;
                }
            }
        }
    }
}
