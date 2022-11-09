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
    internal sealed class PackageToBuildMultiTargetingFileGroupRelation : RelationBase<PackageReferenceItem, PackageBuildFileGroupItem>
    {
        protected override bool HasContainedItems(PackageReferenceItem parent)
        {
            return parent.Library.BuildMultiTargetingFiles.Length != 0;
        }

        protected override void UpdateContainsCollection(PackageReferenceItem parent, AggregateContainsRelationCollectionSpan span)
        {
            span.UpdateContainsItems(
                parent.Library.BuildMultiTargetingFiles.Length == 0 ? Enumerable.Empty<AssetsFileTargetLibrary>() : new[] { parent.Library },
                (library, item) => 0,
                (library, item) => false,
                library => new PackageBuildFileGroupItem(parent.Target, library, PackageBuildFileGroupType.BuildMultiTargeting));
        }

        protected override IEnumerable<PackageReferenceItem>? CreateContainedByItems(PackageBuildFileGroupItem child)
        {
            if (child.GroupType == PackageBuildFileGroupType.BuildMultiTargeting)
            {
                yield return new PackageReferenceItem(child.Target, child.Library);
            }
        }
    }
}
