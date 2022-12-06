// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.AttachedCollections;
using NuGet.VisualStudio.SolutionExplorer.Models;

namespace NuGet.VisualStudio.SolutionExplorer
{
    [Export(typeof(IRelation))]
    internal sealed class PackageToDocumentGroupRelation : RelationBase<PackageReferenceItem, PackageDocumentGroupItem>
    {
        protected override bool HasContainedItems(PackageReferenceItem parent) => parent.Library.ContentFiles.Length != 0;

        protected override void UpdateContainsCollection(PackageReferenceItem parent, AggregateContainsRelationCollectionSpan span)
        {
            span.UpdateContainsItems(
                parent.Library.DocumentationFiles.Length == 0 ? Enumerable.Empty<AssetsFileTargetLibrary>() : new[] { parent.Library },
                (library, item) => 0,
                (library, item) => false,
                library => new PackageDocumentGroupItem(parent.Target, library));
        }

        protected override IEnumerable<PackageReferenceItem>? CreateContainedByItems(PackageDocumentGroupItem child)
        {
            yield return new PackageReferenceItem(child.Target, child.Library);
        }
    }
}
