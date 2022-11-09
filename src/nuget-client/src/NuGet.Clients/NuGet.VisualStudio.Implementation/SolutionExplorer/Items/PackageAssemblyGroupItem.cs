// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.ComponentModel;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.AttachedCollections;
using NuGet.VisualStudio.Implementation.Resources;
using NuGet.VisualStudio.SolutionExplorer.Models;

namespace NuGet.VisualStudio.SolutionExplorer
{
    /// <summary>
    /// Backing object for named group of assemblies within the dependencies tree.
    /// </summary>
    /// <remarks>
    /// Items within this group have type <see cref="PackageAssemblyItem"/>.
    /// </remarks>
    internal sealed class PackageAssemblyGroupItem : RelatableItemBase
    {
        public AssetsFileTarget Target { get; }
        public AssetsFileTargetLibrary Library { get; }
        public PackageAssemblyGroupType GroupType { get; }

        public PackageAssemblyGroupItem(AssetsFileTarget target, AssetsFileTargetLibrary library, PackageAssemblyGroupType groupType)
            : base(GetGroupLabel(groupType))
        {
            Target = target;
            Library = library;
            GroupType = groupType;
        }

        private static string GetGroupLabel(PackageAssemblyGroupType groupType)
        {
            return groupType switch
            {
                PackageAssemblyGroupType.CompileTime => VsResources.PackageCompileTimeAssemblyGroupName,
                PackageAssemblyGroupType.Framework => VsResources.PackageFrameworkAssemblyGroupName,
                _ => throw new InvalidEnumArgumentException(nameof(groupType), (int)groupType, typeof(PackageAssemblyGroupType))
            };
        }

        public override object Identity => Tuple.Create(GroupType, Library.Name);

        public override int Priority => GroupType switch
        {
            PackageAssemblyGroupType.CompileTime => AttachedItemPriority.CompileTimeAssemblyGroup,
            PackageAssemblyGroupType.Framework => AttachedItemPriority.FrameworkAssemblyGroup,
            _ => throw new InvalidEnumArgumentException(nameof(GroupType), (int)GroupType, typeof(PackageAssemblyGroupType))
        };

        public override ImageMoniker IconMoniker => KnownMonikers.ReferenceGroup;
    }
}
