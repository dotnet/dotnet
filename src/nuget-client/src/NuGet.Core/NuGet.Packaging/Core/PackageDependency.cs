// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using NuGet.Versioning;

namespace NuGet.Packaging.Core
{
    /// <summary>
    /// Represents a package dependency Id and allowed version range.
    /// </summary>
    public class PackageDependency : IEquatable<PackageDependency>
    {
        private VersionRange _versionRange;
        private static readonly List<string> EmptyList = new List<string>();

        /// <summary>
        /// Dependency package Id
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Types to include from the dependency package.
        /// </summary>
        public IReadOnlyList<string> Include { get; }

        /// <summary>
        /// Types to exclude from the dependency package.
        /// </summary>
        public IReadOnlyList<string> Exclude { get; }

        /// <summary>
        /// Range of versions allowed for the depenency
        /// </summary>
        [JsonProperty(PropertyName = "range")]
        public VersionRange VersionRange
        {
            get { return _versionRange; }
        }

        public PackageDependency(string id)
            : this(id, VersionRange.All)
        {
        }

        [JsonConstructor]
        public PackageDependency(string id, VersionRange versionRange)
            : this(id, versionRange, include: null, exclude: null)
        {
        }

        public PackageDependency(
            string id,
            VersionRange versionRange,
            IReadOnlyList<string> include,
            IReadOnlyList<string> exclude)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(id));
            }

            Id = id;
            _versionRange = versionRange ?? VersionRange.All;
            Include = include ?? EmptyList;
            Exclude = exclude ?? EmptyList;
        }

        public bool Equals(PackageDependency other)
        {
            return PackageDependencyComparer.Default.Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            var dependency = obj as PackageDependency;

            if (dependency != null)
            {
                return Equals(dependency);
            }

            return false;
        }

        /// <summary>
        /// Hash code from the default PackageDependencyComparer
        /// </summary>
        public override int GetHashCode()
        {
            return PackageDependencyComparer.Default.GetHashCode(this);
        }

        /// <summary>
        /// Id and Version range string
        /// </summary>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", Id, VersionRange.ToNormalizedString());
        }
    }
}
