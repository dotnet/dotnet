// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NuGet.Versioning
{
    /// <summary>
    /// A strict SemVer implementation
    /// </summary>
    [TypeConverter(typeof(SemanticVersionConverter))]
    public partial class SemanticVersion
    {
        // store as array to avoid enumerator allocations
        internal readonly string[]? _releaseLabels;
        internal readonly string? _metadata;

        /// <summary>
        /// Creates a SemanticVersion from an existing SemanticVersion
        /// </summary>
        /// <param name="version">Version to clone.</param>
        public SemanticVersion(SemanticVersion version)
            : this(version.Major, version.Minor, version.Patch, version.ReleaseLabels, version.Metadata)
        {
        }

        /// <summary>
        /// Creates a SemanticVersion X.Y.Z
        /// </summary>
        /// <param name="major">X.y.z</param>
        /// <param name="minor">x.Y.z</param>
        /// <param name="patch">x.y.Z</param>
        public SemanticVersion(int major, int minor, int patch)
            : this(major, minor, patch, Enumerable.Empty<string>(), null)
        {
        }

        /// <summary>
        /// Creates a NuGetVersion X.Y.Z-alpha
        /// </summary>
        /// <param name="major">X.y.z</param>
        /// <param name="minor">x.Y.z</param>
        /// <param name="patch">x.y.Z</param>
        /// <param name="releaseLabel">Prerelease label</param>
        public SemanticVersion(int major, int minor, int patch, string? releaseLabel)
            : this(major, minor, patch, ParseReleaseLabels(releaseLabel), null)
        {
        }

        /// <summary>
        /// Creates a NuGetVersion X.Y.Z-alpha#build01
        /// </summary>
        /// <param name="major">X.y.z</param>
        /// <param name="minor">x.Y.z</param>
        /// <param name="patch">x.y.Z</param>
        /// <param name="releaseLabel">Prerelease label</param>
        /// <param name="metadata">Build metadata</param>
        public SemanticVersion(int major, int minor, int patch, string? releaseLabel, string? metadata)
            : this(major, minor, patch, ParseReleaseLabels(releaseLabel), metadata)
        {
        }

        /// <summary>
        /// Creates a NuGetVersion X.Y.Z-alpha.1.2#build01
        /// </summary>
        /// <param name="major">X.y.z</param>
        /// <param name="minor">x.Y.z</param>
        /// <param name="patch">x.y.Z</param>
        /// <param name="releaseLabels">Release labels that have been split by the dot separator</param>
        /// <param name="metadata">Build metadata</param>
        public SemanticVersion(int major, int minor, int patch, IEnumerable<string>? releaseLabels, string? metadata)
            : this(new Version(major, minor, patch, 0), releaseLabels, metadata)
        {
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="version">Version</param>
        /// <param name="releaseLabel">Full release label</param>
        /// <param name="metadata">Build metadata</param>
        protected SemanticVersion(Version version, string? releaseLabel = null, string? metadata = null)
            : this(version, ParseReleaseLabels(releaseLabel), metadata)
        {
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="major">X.y.z</param>
        /// <param name="minor">x.Y.z</param>
        /// <param name="patch">x.y.Z</param>
        /// <param name="revision">x.y.z.R</param>
        /// <param name="releaseLabel">Prerelease label</param>
        /// <param name="metadata">Build metadata</param>
        protected SemanticVersion(int major, int minor, int patch, int revision, string? releaseLabel, string? metadata)
            : this(major, minor, patch, revision, ParseReleaseLabels(releaseLabel), metadata)
        {
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="patch"></param>
        /// <param name="revision"></param>
        /// <param name="releaseLabels"></param>
        /// <param name="metadata"></param>
        protected SemanticVersion(int major, int minor, int patch, int revision, IEnumerable<string>? releaseLabels, string? metadata)
            : this(new Version(major, minor, patch, revision), releaseLabels, metadata)
        {
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="version">Version</param>
        /// <param name="releaseLabels">Release labels</param>
        /// <param name="metadata">Build metadata</param>
        protected SemanticVersion(Version version, IEnumerable<string>? releaseLabels, string? metadata)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            var normalizedVersion = NormalizeVersionValue(version);
            Major = normalizedVersion.Major;
            Minor = normalizedVersion.Minor;
            Patch = normalizedVersion.Build;

            _metadata = metadata;

            if (releaseLabels != null)
            {
                // If the labels are already an array use it
                var asArray = releaseLabels as string[];

                if (asArray != null)
                {
                    _releaseLabels = asArray;
                }
                else
                {
                    // enumerate the list
                    _releaseLabels = releaseLabels.ToArray();
                }

                if (_releaseLabels.Length < 1)
                {
                    // Avoid storing an empty array of labels, this field is checked for null everywhere
                    _releaseLabels = null;
                }
            }
        }

        /// <summary>
        /// Major version X (X.y.z)
        /// </summary>
        public int Major { get; }

        /// <summary>
        /// Minor version Y (x.Y.z)
        /// </summary>
        public int Minor { get; }

        /// <summary>
        /// Patch version Z (x.y.Z)
        /// </summary>
        public int Patch { get; }

        /// <summary>
        /// A collection of pre-release labels attached to the version.
        /// </summary>
        public IEnumerable<string> ReleaseLabels
        {
            get { return _releaseLabels ?? EmptyReleaseLabels; }
        }

        /// <summary>
        /// The full pre-release label for the version.
        /// </summary>
        public string Release
        {
            get
            {
                if (_releaseLabels != null)
                {
                    if (_releaseLabels.Length == 1)
                    {
                        // There is exactly 1 label
                        return _releaseLabels[0];
                    }
                    else
                    {
                        // Join all labels
                        return string.Join(".", _releaseLabels);
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// True if pre-release labels exist for the version.
        /// </summary>
        public virtual bool IsPrerelease
        {
            get
            {
                if (_releaseLabels != null)
                {
                    for (int i = 0; i < _releaseLabels.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(_releaseLabels[i]))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// True if metadata exists for the version.
        /// </summary>
        public virtual bool HasMetadata
        {
            get { return !string.IsNullOrEmpty(Metadata); }
        }

        /// <summary>
        /// Build metadata attached to the version.
        /// </summary>
        public virtual string? Metadata
        {
            get { return _metadata; }
        }
    }
}
