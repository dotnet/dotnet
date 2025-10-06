// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Build.Locator
{
    internal class SemanticVersion : IComparable<SemanticVersion>
    {
        private readonly IEnumerable<string> _releaseLabels;
        private Version _version;
        private string _originalValue;

        public SemanticVersion(Version version, IEnumerable<string> releaseLabels, string originalValue)
        {
            _version = version ?? throw new ArgumentNullException(nameof(version));

            if (releaseLabels != null)
            {
                _releaseLabels = releaseLabels.ToArray();
            }

            _originalValue = originalValue;
        }

        /// <summary>
        /// Major version X (X.y.z)
        /// </summary>
        public int Major => _version.Major;

        /// <summary>
        /// Minor version Y (x.Y.z)
        /// </summary>
        public int Minor => _version.Minor;

        /// <summary>
        /// Patch version Z (x.y.Z)
        /// </summary>
        public int Patch => _version.Build;

        /// <summary>
        /// A collection of pre-release labels attached to the version.
        /// </summary>
        public IEnumerable<string> ReleaseLabels => _releaseLabels ?? Enumerable.Empty<string>();

        public string OriginalValue => _originalValue;

        /// <summary>
        /// The full pre-release label for the version.
        /// </summary>
        public string Release => _releaseLabels != null ? String.Join(".", _releaseLabels) : String.Empty;

        /// <summary>
        /// True if pre-release labels exist for the version.
        /// </summary>
        public bool IsPrerelease
        {
            get
            {
                if (ReleaseLabels != null)
                {
                    var enumerator = ReleaseLabels.GetEnumerator();
                    return (enumerator.MoveNext() && !String.IsNullOrEmpty(enumerator.Current));
                }

                return false;
            }
        }

        /// <summary>
        /// Compare versions.
        /// </summary>
        public int CompareTo(SemanticVersion other)
        {
            return VersionComparer.Compare(this, other);
        }
    }
}
