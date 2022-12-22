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
    internal sealed class AssemblyGroupToAssemblyRelation : RelationBase<PackageAssemblyGroupItem, PackageAssemblyItem>
    {
        protected override bool HasContainedItems(PackageAssemblyGroupItem parent)
        {
            return GetAssemblies(parent).Length != 0;
        }

        protected override void UpdateContainsCollection(PackageAssemblyGroupItem parent, AggregateContainsRelationCollectionSpan span)
        {
            span.UpdateContainsItems(
                GetAssemblies(parent).OrderBy(assembly => assembly),
                (assembly, item) => StringComparer.Ordinal.Compare(assembly, item.Path),
                (library, item) => false,
                assembly => new PackageAssemblyItem(parent.Target, parent.Library, assembly, parent.GroupType));
        }

        protected override IEnumerable<PackageAssemblyGroupItem>? CreateContainedByItems(PackageAssemblyItem child)
        {
            yield return new PackageAssemblyGroupItem(child.Target, child.Library, child.GroupType);
        }

        private static ImmutableArray<string> GetAssemblies(PackageAssemblyGroupItem groupItem)
        {
            return groupItem.GroupType switch
            {
                PackageAssemblyGroupType.CompileTime => groupItem.Library.CompileTimeAssemblies,
                PackageAssemblyGroupType.Framework => groupItem.Library.FrameworkAssemblies,
                _ => throw new InvalidEnumArgumentException(nameof(groupItem.GroupType), (int)groupItem.GroupType, typeof(PackageAssemblyGroupType))
            };
        }
    }
}
