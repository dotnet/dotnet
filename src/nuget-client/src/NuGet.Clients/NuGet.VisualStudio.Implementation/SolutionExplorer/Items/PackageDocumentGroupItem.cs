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
    /// Backing object for a group of documents within a package within the dependencies tree.
    /// </summary>
    /// <remarks>
    /// Items within this group have type <see cref="PackageDocumentItem"/>.
    /// </remarks>
    internal sealed class PackageDocumentGroupItem : RelatableItemBase
    {
        public AssetsFileTarget Target { get; }
        public AssetsFileTargetLibrary Library { get; }

        public PackageDocumentGroupItem(AssetsFileTarget target, AssetsFileTargetLibrary library)
            : base(VsResources.PackageDocumentGroupName)
        {
            Target = target;
            Library = library;
        }

        public override object Identity => Library.Name;

        public override int Priority => AttachedItemPriority.DocumentGroup;

        public override ImageMoniker IconMoniker => KnownMonikers.DocumentsFolder;

        public override ImageMoniker ExpandedIconMoniker => KnownMonikers.DocumentsFolder;
    }
}
