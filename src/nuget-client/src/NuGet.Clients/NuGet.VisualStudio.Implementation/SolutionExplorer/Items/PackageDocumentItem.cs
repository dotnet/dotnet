// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.AttachedCollections;
using Microsoft.VisualStudio.Shell;
using NuGet.VisualStudio.Implementation.Resources;
using NuGet.VisualStudio.SolutionExplorer.Models;

namespace NuGet.VisualStudio.SolutionExplorer
{
    /// <summary>
    /// Backing object for documents within a package within the dependencies tree.
    /// </summary>
    /// <remarks>
    /// Items of this type are grouped within <see cref="PackageDocumentGroupItem"/>.
    /// </remarks>
    internal sealed class PackageDocumentItem : RelatableItemBase, IInvocationPattern, IInvocationController
    {
        public AssetsFileTarget Target { get; }
        public AssetsFileTargetLibrary Library { get; }
        public string Path { get; }

        private readonly FileOpener _fileOpener;
        private readonly IFileIconProvider _fileIconProvider;

        public PackageDocumentItem(AssetsFileTarget target, AssetsFileTargetLibrary library, string path, FileOpener fileOpener, IFileIconProvider fileIconProvider)
            : base(path)
        {
            Target = target;
            Library = library;
            Path = path;
            _fileOpener = fileOpener;
            _fileIconProvider = fileIconProvider;
        }

        public override object Identity => Tuple.Create(Library.Name, Path);

        // All siblings are documentation files, so no prioritization needed (sort alphabetically)
        public override int Priority => 0;

        public override ImageMoniker IconMoniker => _fileIconProvider.GetFileExtensionImageMoniker(Path);

        public bool CanPreview => true;

        public IInvocationController InvocationController => this;

        public bool Invoke(IEnumerable<object> items, InputSource inputSource, bool preview)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            bool result = false;

            foreach (object itemObject in items)
            {
                if (itemObject is PackageDocumentItem { FullPath: { } path })
                {
                    _fileOpener.OpenFile(path, isReadOnly: true);
                    result = true;
                }
            }
            return result;
        }

        public string? FullPath => Library.Version is not null && Target.TryResolvePackagePath(Library.Name, Library.Version, out string? fullPath)
            ? System.IO.Path.GetFullPath(System.IO.Path.Combine(fullPath, Path))
            : null;

        public override object? GetBrowseObject() => new BrowseObject(this);

        private sealed class BrowseObject : LocalizableProperties
        {
            private readonly PackageDocumentItem _item;

            public BrowseObject(PackageDocumentItem item) => _item = item;

            public override string GetComponentName() => _item.Text;

            public override string GetClassName() => VsResources.PackageDocumentBrowseObjectClassName;

            [BrowseObjectDisplayName(nameof(VsResources.PackageDocumentPathDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.PackageDocumentPathDescription))]
            public string Path => _item.Path;
        }
    }
}
