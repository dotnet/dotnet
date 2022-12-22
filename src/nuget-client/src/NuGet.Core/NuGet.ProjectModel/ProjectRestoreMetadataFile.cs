// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class ProjectRestoreMetadataFile : IEquatable<ProjectRestoreMetadataFile>, IComparable<ProjectRestoreMetadataFile>
    {
        /// <summary>
        /// Relative path that would be used within a package.
        /// This will be used to determine the asset type.
        /// Example: lib/net45/a.dll
        /// </summary>
        public string PackagePath { get; }

        /// <summary>
        /// Absolute path on disk.
        /// </summary>
        public string AbsolutePath { get; }

        public ProjectRestoreMetadataFile(string packagePath, string absolutePath)
        {
            if (packagePath == null)
            {
                throw new ArgumentNullException(nameof(packagePath));
            }

            if (absolutePath == null)
            {
                throw new ArgumentNullException(nameof(absolutePath));
            }

            PackagePath = packagePath;
            AbsolutePath = absolutePath;
        }

        public bool Equals(ProjectRestoreMetadataFile other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return StringComparer.Ordinal.Equals(PackagePath, other.PackagePath)
                && StringComparer.Ordinal.Equals(AbsolutePath, other.AbsolutePath);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProjectRestoreMetadataFile);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCodeCombiner();

            hashCode.AddObject(PackagePath);
            hashCode.AddObject(AbsolutePath);

            return hashCode.CombinedHash;
        }

        public override string ToString()
        {
            return PackagePath;
        }

        public int CompareTo(ProjectRestoreMetadataFile other)
        {
            return StringComparer.Ordinal.Compare(PackagePath, other.PackagePath);
        }

        public ProjectRestoreMetadataFile Clone()
        {
            return new ProjectRestoreMetadataFile(PackagePath, AbsolutePath);
        }

    }
}
