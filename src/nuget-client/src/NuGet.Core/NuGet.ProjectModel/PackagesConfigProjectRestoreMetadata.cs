// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class PackagesConfigProjectRestoreMetadata : ProjectRestoreMetadata
    {
        /// <summary>
        /// Full path to the packages.config file, if it exists. Only valid when ProjectStyle is PackagesConfig.
        /// </summary>
        public string PackagesConfigPath { get; set; }

        /// <summary>
        /// User packages repository path.
        /// </summary>
        public string RepositoryPath { get; set; }

        public override int GetHashCode()
        {
            var hashCode = new HashCodeCombiner();

            hashCode.AddObject(base.GetHashCode());
            hashCode.AddObject(PackagesConfigPath);

            return hashCode.CombinedHash;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PackagesConfigProjectRestoreMetadata);
        }

        public bool Equals(PackagesConfigProjectRestoreMetadata obj)
        {
            return base.Equals(obj) &&
                PathUtility.GetStringComparerBasedOnOS().Equals(PackagesConfigPath, obj.PackagesConfigPath);
        }

        public override ProjectRestoreMetadata Clone()
        {
            var clone = new PackagesConfigProjectRestoreMetadata();
            FillClone(clone);
            clone.PackagesConfigPath = PackagesConfigPath;
            return clone;
        }
    }
}
