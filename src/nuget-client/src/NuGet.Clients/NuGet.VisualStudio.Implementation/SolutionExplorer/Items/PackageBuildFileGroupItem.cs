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
    /// Backing object for named group of build files (.props and .targets) within the dependencies tree.
    /// </summary>
    /// <remarks>
    /// Items within this group have type <see cref="PackageBuildFileItem"/>.
    /// </remarks>
    internal sealed class PackageBuildFileGroupItem : RelatableItemBase
    {
        public AssetsFileTarget Target { get; }
        public AssetsFileTargetLibrary Library { get; }
        public PackageBuildFileGroupType GroupType { get; }

        public PackageBuildFileGroupItem(AssetsFileTarget target, AssetsFileTargetLibrary library, PackageBuildFileGroupType groupType)
            : base(GetGroupLabel(groupType))
        {
            Target = target;
            Library = library;
            GroupType = groupType;
        }

        private static string GetGroupLabel(PackageBuildFileGroupType groupType)
        {
            return groupType switch
            {
                PackageBuildFileGroupType.Build => VsResources.PackageBuildFileGroupName,
                PackageBuildFileGroupType.BuildMultiTargeting => VsResources.PackageBuildMultiTargetingFileGroupName,
                _ => throw new InvalidEnumArgumentException(nameof(groupType), (int)groupType, typeof(PackageBuildFileGroupType))
            };
        }

        public override object Identity => Tuple.Create(GroupType, Library.Name);

        public override int Priority => GroupType switch
        {
            PackageBuildFileGroupType.Build => AttachedItemPriority.PackageBuildFileGroup,
            PackageBuildFileGroupType.BuildMultiTargeting => AttachedItemPriority.PackageBuildMultiTargetingFileGroup,
            _ => throw new InvalidEnumArgumentException(nameof(GroupType), (int)GroupType, typeof(PackageBuildFileGroupType))
        };

        public override ImageMoniker IconMoniker => KnownMonikers.TargetFile;
    }
}
