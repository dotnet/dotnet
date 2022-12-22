// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.AttachedCollections;

namespace NuGet.VisualStudio.SolutionExplorer
{
    [Export(typeof(IRelation))]
    internal sealed class ContentFilesGroupToContentFilesRelation : RelationBase<PackageContentFileGroupItem, PackageContentFileItem>
    {
        private readonly IFileIconProvider _fileIconProvider;

        [ImportingConstructor]
        public ContentFilesGroupToContentFilesRelation(IFileIconProvider fileIconProvider)
        {
            _fileIconProvider = fileIconProvider;
        }

        protected override bool HasContainedItems(PackageContentFileGroupItem parent)
        {
            return parent.Library.ContentFiles.Length != 0;
        }

        protected override void UpdateContainsCollection(PackageContentFileGroupItem parent, AggregateContainsRelationCollectionSpan span)
        {
            span.UpdateContainsItems(
                parent.Library.ContentFiles.OrderBy(contentFile => contentFile.Path),
                (contentFile, item) => StringComparer.Ordinal.Compare(contentFile.Path, item.ContentFile.Path),
                (library, item) => false,
                contentFile => new PackageContentFileItem(parent.Target, parent.Library, contentFile, _fileIconProvider));
        }

        protected override IEnumerable<PackageContentFileGroupItem>? CreateContainedByItems(PackageContentFileItem child)
        {
            yield return new PackageContentFileGroupItem(child.Target, child.Library);
        }
    }
}
