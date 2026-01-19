// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using NuGet.Common;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class CacheFile : IEquatable<CacheFile>
    {
        internal const int CurrentVersion = 2;

        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("dgSpecHash")]
        public string DgSpecHash { get; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the full path to the project file.
        /// </summary>
        [JsonPropertyName("projectFilePath")]
        public string ProjectFilePath { get; set; }

        /// <summary>
        /// Gets or sets a list of package paths that must exist in order for the project to be considered up-to-date.
        /// </summary>
        [JsonPropertyName("expectedPackageFiles")]
        public IList<string> ExpectedPackageFilePaths { get; set; }

        [JsonPropertyName("logs")]
        public IList<IAssetsLogMessage> LogMessages { get; set; }

        [JsonIgnore]
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
