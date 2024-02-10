// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Common;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class CacheFile : IEquatable<CacheFile>
    {
        internal const int CurrentVersion = 2;

        public string DgSpecHash { get; }

        public int Version { get; set; }

        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets a list of package paths that must exist in order for the project to be considered up-to-date.
        /// </summary>
        public IList<string> ExpectedPackageFilePaths { get; set; }

        [Obsolete("File existence checks are a function of time not the cache file content.")]
        /// <summary>
        /// Gets or sets a value indicating if one or more of the expected files are missing.
        /// </summary>
        public bool HasAnyMissingPackageFiles
        {
            get => throw new NotImplementedException("This API is no longer support");
            set => throw new NotImplementedException("This API is no longer support");
        }

        /// <summary>
        /// Gets or sets the full path to the project file.
        /// </summary>
        public string ProjectFilePath { get; set; }

        public IList<IAssetsLogMessage> LogMessages { get; set; }

        public bool IsValid { get { return Version == CurrentVersion && Success && DgSpecHash != null; } }

        public CacheFile(string dgSpecHash)
        {
            DgSpecHash = dgSpecHash;
            Version = CurrentVersion;
        }

        public bool Equals(CacheFile other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Version == other.Version && Success == other.Success && StringComparer.Ordinal.Equals(DgSpecHash, other.DgSpecHash) && PathUtility.GetStringComparerBasedOnOS().Equals(ProjectFilePath, other.ProjectFilePath);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CacheFile);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();
            combiner.AddObject(DgSpecHash);
            combiner.AddObject(Version);
            combiner.AddObject(ProjectFilePath);
            return combiner.CombinedHash;
        }
    }
}
