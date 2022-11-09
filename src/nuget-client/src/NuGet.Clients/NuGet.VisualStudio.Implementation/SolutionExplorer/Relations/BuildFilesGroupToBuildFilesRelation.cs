// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.AttachedCollections;

namespace NuGet.VisualStudio.SolutionExplorer
{
    [Export(typeof(IRelation))]
    internal sealed class BuildFilesGroupToBuildFilesRelation : RelationBase<PackageBuildFileGroupItem, PackageBuildFileItem>
    {
        private readonly FileOpener _fileOpener;

        [ImportingConstructor]
        public BuildFilesGroupToBuildFilesRelation(FileOpener fileOpener)
        {
            _fileOpener = fileOpener;
        }

        protected override bool HasContainedItems(PackageBuildFileGroupItem parent)
        {
            return GetBuildFiles(parent).Length != 0;
        }

        protected override void UpdateContainsCollection(PackageBuildFileGroupItem parent, AggregateContainsRelationCollectionSpan span)
        {
            span.UpdateContainsItems(
                GetBuildFiles(parent).OrderBy(buildFile => buildFile),
                (buildFile, item) => StringComparer.Ordinal.Compare(buildFile, item.Path),
                (library, item) => false,
                buildFile => new PackageBuildFileItem(parent.Target, parent.Library, buildFile, parent.GroupType, _fileOpener));
        }

        protected override IEnumerable<PackageBuildFileGroupItem>? CreateContainedByItems(PackageBuildFileItem child)
        {
            yield return new PackageBuildFileGroupItem(child.Target, child.Library, child.GroupType);
        }

        private static ImmutableArray<string> GetBuildFiles(PackageBuildFileGroupItem groupItem)
        {
            return groupItem.GroupType switch
            {
                PackageBuildFileGroupType.Build => groupItem.Library.BuildFiles,
                PackageBuildFileGroupType.BuildMultiTargeting => groupItem.Library.BuildMultiTargetingFiles,
                _ => throw new InvalidEnumArgumentException(nameof(groupItem.GroupType), (int)groupItem.GroupType, typeof(PackageAssemblyGroupType))
            };
        }
    }
}
