// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.AttachedCollections;
using Microsoft.VisualStudio.Shell;
using NuGet.VisualStudio.Implementation.Resources;
using NuGet.VisualStudio.SolutionExplorer.Models;

namespace NuGet.VisualStudio.SolutionExplorer
{
    /// <summary>
    /// Backing object for content files within a package within the dependencies tree.
    /// </summary>
    /// <remarks>
    /// Items of this type are grouped within <see cref="PackageContentFileGroupItem"/>.
    /// </remarks>
    internal sealed class PackageContentFileItem : RelatableItemBase
    {
        public AssetsFileTarget Target { get; }
        public AssetsFileTargetLibrary Library { get; }
        public AssetsFileTargetLibraryContentFile ContentFile { get; }

        private readonly IFileIconProvider _fileIconProvider;

        public PackageContentFileItem(AssetsFileTarget target, AssetsFileTargetLibrary library, AssetsFileTargetLibraryContentFile contentFile, IFileIconProvider fileIconProvider)
            : base(GetProcessedContentFilePath(contentFile.Path))
        {
            Target = target;
            Library = library;
            ContentFile = contentFile;
            _fileIconProvider = fileIconProvider;
        }

        private static string GetProcessedContentFilePath(string rawPath)
        {
            // Content file paths always start with "contentFiles/" so remove it from display strings
            const string Prefix = "contentFiles/";
            return rawPath.StartsWith(Prefix, StringComparison.Ordinal) ? rawPath.Substring(Prefix.Length) : rawPath;
        }

        public override object Identity => Tuple.Create(Library.Name, ContentFile.Path);

        // All siblings are content files, so no prioritization needed (sort alphabetically)
        public override int Priority => 0;

        public override ImageMoniker IconMoniker => _fileIconProvider.GetFileExtensionImageMoniker(Text);

        public override object? GetBrowseObject() => new BrowseObject(this);

        private sealed class BrowseObject : LocalizableProperties
        {
            private readonly PackageContentFileItem _item;

            public BrowseObject(PackageContentFileItem item) => _item = item;

            public override string GetComponentName() => _item.Text;

            public override string GetClassName() => VsResources.PackageContentFileBrowseObjectClassName;

            [BrowseObjectDisplayName(nameof(VsResources.PackageContentFilePathDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.PackageContentFilePathDescription))]
            public string Path => _item.ContentFile.Path;

            [BrowseObjectDisplayName(nameof(VsResources.PackageContentFileOutputPathDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.PackageContentFileOutputPathDescription))]
            public string? OutputPath => _item.ContentFile.OutputPath;

            [BrowseObjectDisplayName(nameof(VsResources.PackageContentFilePPOutputPathDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.PackageContentFilePPOutputPathDescription))]
            public string? PPOutputPath => _item.ContentFile.PPOutputPath;

            [BrowseObjectDisplayName(nameof(VsResources.PackageContentFileCodeLanguageDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.PackageContentFileCodeLanguageDescription))]
            public string? CodeLanguage => _item.ContentFile.CodeLanguage;

            [BrowseObjectDisplayName(nameof(VsResources.PackageContentFileBuildActionDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.PackageContentFileBuildActionDescription))]
            public string? BuildAction => _item.ContentFile.BuildAction;

            [BrowseObjectDisplayName(nameof(VsResources.PackageContentFileCopyToOutputDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.PackageContentFileCopyToOutputDescription))]
            public bool CopyToOutput => _item.ContentFile.CopyToOutput;
        }
    }
}
