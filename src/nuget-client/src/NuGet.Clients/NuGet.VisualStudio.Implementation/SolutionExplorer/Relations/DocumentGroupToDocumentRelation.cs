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
    internal sealed class DocumentGroupToDocumentRelation : RelationBase<PackageDocumentGroupItem, PackageDocumentItem>
    {
        private readonly FileOpener _fileOpener;
        private readonly IFileIconProvider _fileIconProvider;

        [ImportingConstructor]
        public DocumentGroupToDocumentRelation(
            FileOpener fileOpener,
            IFileIconProvider fileIconProvider)
        {
            _fileOpener = fileOpener;
            _fileIconProvider = fileIconProvider;
        }

        protected override bool HasContainedItems(PackageDocumentGroupItem parent)
        {
            return parent.Library.DocumentationFiles.Length != 0;
        }

        protected override void UpdateContainsCollection(PackageDocumentGroupItem parent, AggregateContainsRelationCollectionSpan span)
        {
            span.UpdateContainsItems(
                parent.Library.DocumentationFiles.OrderBy(path => path),
                (path, item) => StringComparer.Ordinal.Compare(path, item.Path),
                (path, item) => false,
                path => new PackageDocumentItem(parent.Target, parent.Library, path, _fileOpener, _fileIconProvider));
        }

        protected override IEnumerable<PackageDocumentGroupItem>? CreateContainedByItems(PackageDocumentItem child)
        {
            yield return new PackageDocumentGroupItem(child.Target, child.Library);
        }
    }
}
