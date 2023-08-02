// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Shared;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// A collection of package dependency groups with the content (nupkg url).
    /// </summary>
    public class RemoteSourceDependencyInfo : IEquatable<RemoteSourceDependencyInfo>
    {
        /// <summary>
        /// DependencyInfo
        /// </summary>
        /// <param name="identity">package identity</param>
        /// <param name="dependencyGroups">package dependency groups</param>
        /// <param name="contentUri">The content uri for the dependency.</param>
        public RemoteSourceDependencyInfo(
            PackageIdentity identity,
            bool listed,
            IEnumerable<PackageDependencyGroup> dependencyGroups,
            string contentUri)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            if (dependencyGroups == null)
            {
                throw new ArgumentNullException(nameof(dependencyGroups));
            }

            Identity = identity;
            Listed = listed;
            DependencyGroups = dependencyGroups.ToList();
            ContentUri = contentUri;
        }

        /// <summary>
        /// Package identity
        /// </summary>
        public PackageIdentity Identity { get; }

        /// <summary>
        /// IsListed
        /// </summary>
        public bool Listed { get; }

        /// <summary>
        /// Package dependency groups
        /// </summary>
        public IEnumerable<PackageDependencyGroup> DependencyGroups { get; }

        /// <summary>
        /// The content url of this resource.
        /// </summary>
        public string ContentUri { get; set; }

        public bool Equals(RemoteSourceDependencyInfo other)
        {
            return other != null &&
                   Identity.Equals(other.Identity) &&
                   new HashSet<PackageDependencyGroup>(DependencyGroups).SetEquals(other.DependencyGroups) &&
                   string.Equals(ContentUri, other.ContentUri, StringComparison.Ordinal);
        }

        public override bool Equals(object obj) => Equals(obj as PackageDependencyInfo);

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(Identity);
            combiner.AddUnorderedSequence(DependencyGroups);
            combiner.AddObject(ContentUri);

            return combiner.CombinedHash;
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0} : {1}", Identity, String.Join(" ,", DependencyGroups));
        }
    }
}
