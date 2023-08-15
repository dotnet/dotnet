// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text.Json;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// Represents a Common Vulnerabilities and Exposures (CVE) entry. CVEs can be associated with one or more <see cref="ProductRelease"/>.
    /// </summary>
    public class Cve : IEquatable<Cve>
    {
        /// <summary>
        /// The identifier of the CVE.
        /// </summary>
        public string Id
        {
            get;
            private set;
        }

        /// <summary>
        /// The URI pointing to a description of the vulnerability.
        /// </summary>
        public Uri DescriptionLink
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new <see cref="Cve"/> instance from a <see cref="JsonElement"/>.
        /// </summary>
        /// <param name="cveElement">The <see cref="JsonElement"/> to deserialize.</param>
        internal Cve(JsonElement cveElement)
        {
            DescriptionLink = cveElement.GetUriOrDefault("cve-url");
            Id = cveElement.GetStringOrDefault("cve-id");
        }

        /// <summary>
        /// Creates a new <see cref="Cve"/> instance.
        /// </summary>
        /// <param name="id">The CVE identifier.</param>
        /// <param name="address">The URI pointing to the CVE description.</param>
        internal Cve(string id, Uri address)
        {
            DescriptionLink = address;
            Id = id;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Cve"/> is equal to this instance. 
        /// </summary>
        /// <param name="other">The <see cref="Cve"/> to this instance.</param>
        /// <returns><see langword="true"/> if the specified <see cref="Cve"/> is equal to this instance; <see langword="false"/> otherwise.</returns>
        public bool Equals(Cve other)
        {
            return ReferenceEquals(this, other) ||
                Id == other.Id && DescriptionLink == other.DescriptionLink;
        }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare to the current object.</param>
        /// <returns><see langword="true"/> if the specified object is equal to the current object; <see langword="false"/> otherwise.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Cve);
        }

        /// <summary>
        /// Returns the hash code for this CVE.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => Id.GetHashCode() ^ DescriptionLink.GetHashCode();

        internal static Cve Create(string id, string address) => 
            new(id, new Uri(address));
    }
}
