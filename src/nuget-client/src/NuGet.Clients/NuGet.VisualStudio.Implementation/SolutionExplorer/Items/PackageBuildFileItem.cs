// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
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
    /// Backing object for a build file within a library within the dependencies tree.
    /// </summary>
    /// <remarks>
    /// Items of this type are grouped within <see cref="PackageBuildFileGroupItem"/>.
    /// </remarks>
    internal sealed class PackageBuildFileItem : RelatableItemBase, IInvocationPattern, IInvocationController
    {
        private readonly FileOpener _fileOpener;

        public AssetsFileTarget Target { get; }
        public AssetsFileTargetLibrary Library { get; }
        public string Path { get; }
        public PackageBuildFileGroupType GroupType { get; }

        public PackageBuildFileItem(AssetsFileTarget target, AssetsFileTargetLibrary library, string path, PackageBuildFileGroupType groupType, FileOpener fileOpener)
            : base(System.IO.Path.GetFileName(path))
        {
            Target = target;
            Library = library;
            Path = path;
            GroupType = groupType;
            _fileOpener = fileOpener;
        }

        public string? FullPath => Target.TryResolvePackagePath(Library.Name, Library.Version, out string? fullPath)
            ? System.IO.Path.GetFullPath(System.IO.Path.Combine(fullPath, Path))
            : null;

        public override object Identity => Tuple.Create(Library.Name, Path, GroupType);

        // All siblings are build files, so no prioritization needed (sort alphabetically)
        public override int Priority => 0;

        public override ImageMoniker IconMoniker => KnownMonikers.TargetFile;

        public bool CanPreview => true;

        public IInvocationController InvocationController => this;

        public override object? GetBrowseObject() => new BrowseObject(this);

        public bool Invoke(IEnumerable<object> items, InputSource inputSource, bool preview)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            bool result = false;

            foreach (object itemObject in items)
            {
                if (itemObject is PackageBuildFileItem { FullPath: { } path })
                {
                    _fileOpener.OpenFile(path, isReadOnly: true);
                    result = true;
                }
            }
            return result;
        }

        private sealed class BrowseObject : LocalizableProperties
        {
            private readonly PackageBuildFileItem _item;

            public BrowseObject(PackageBuildFileItem library) => _item = library;

            public override string GetComponentName() => _item.Text;

            public override string GetClassName() => VsResources.PackageBuildFileBrowseObjectClassName;

            [BrowseObjectDisplayName(nameof(VsResources.PackageBuildFileNameDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.PackageBuildFileNameDescription))]
            public string Name => _item.Text;

            [BrowseObjectDisplayName(nameof(VsResources.PackageBuildFilePathDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.PackageBuildFilePathDescription))]
            public string? Path => _item.FullPath;
        }
    }
}
