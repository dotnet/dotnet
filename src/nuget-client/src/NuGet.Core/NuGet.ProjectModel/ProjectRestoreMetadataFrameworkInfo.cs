// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class ProjectRestoreMetadataFrameworkInfo : IEquatable<ProjectRestoreMetadataFrameworkInfo>
    {
        public string TargetAlias { get; set; } = string.Empty;

        /// <summary>
        /// Target framework
        /// </summary>
        public NuGetFramework FrameworkName { get; set; }

        /// <summary>
        /// Project references
        /// </summary>
        public IList<ProjectRestoreReference> ProjectReferences { get; set; } = new List<ProjectRestoreReference>();

        public ProjectRestoreMetadataFrameworkInfo()
        {
        }

        public ProjectRestoreMetadataFrameworkInfo(NuGetFramework frameworkName)
        {
            FrameworkName = frameworkName;
        }

        public override string ToString()
        {
            return FrameworkName.GetShortFolderName();
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCodeCombiner();

            hashCode.AddObject(FrameworkName);
            hashCode.AddStringIgnoreCase(TargetAlias);
            hashCode.AddSequence(ProjectReferences);

            return hashCode.CombinedHash;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProjectRestoreMetadataFrameworkInfo);
        }

        public bool Equals(ProjectRestoreMetadataFrameworkInfo other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return EqualityUtility.EqualsWithNullCheck(FrameworkName, other.FrameworkName) &&
                   StringComparer.OrdinalIgnoreCase.Equals(TargetAlias, other.TargetAlias) &&
                   ProjectReferences.OrderedEquals(other.ProjectReferences, e => e.ProjectPath, PathUtility.GetStringComparerBasedOnOS());
        }

        public ProjectRestoreMetadataFrameworkInfo Clone()
        {
            var clonedObject = new ProjectRestoreMetadataFrameworkInfo();
            clonedObject.FrameworkName = FrameworkName;
            clonedObject.TargetAlias = TargetAlias;
            clonedObject.ProjectReferences = ProjectReferences?.Select(c => c.Clone()).ToList();
            return clonedObject;
        }
    }
}
