// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.AttachedCollections;
using NuGet.VisualStudio.Implementation.Resources;
using NuGet.VisualStudio.SolutionExplorer.Models;

namespace NuGet.VisualStudio.SolutionExplorer
{
    /// <summary>
    /// Backing object for a group of content items within a package within the dependencies tree.
    /// </summary>
    /// <remarks>
    /// Items within this group have type <see cref="PackageContentFileItem"/>.
    /// </remarks>
    internal sealed class PackageContentFileGroupItem : RelatableItemBase
    {
        public AssetsFileTarget Target { get; }
        public AssetsFileTargetLibrary Library { get; }

        public PackageContentFileGroupItem(AssetsFileTarget target, AssetsFileTargetLibrary library)
            : base(VsResources.PackageContentFilesGroupName)
        {
            Target = target;
            Library = library;
        }

        public override object Identity => Library.Name;

        public override int Priority => AttachedItemPriority.ContentFilesGroup;

        public override ImageMoniker IconMoniker => KnownMonikers.PackageFolderClosed;

        public override ImageMoniker ExpandedIconMoniker => KnownMonikers.PackageFolderOpened;
    }
}
