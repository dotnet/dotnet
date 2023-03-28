// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("cve-id")]
        [JsonInclude]
        public string Id
        {
            get;
            private set;
        }

        /// <summary>
        /// The URI pointing to a description of the vulnerability.
        /// </summary>
        [JsonPropertyName("cve-url")]
        [JsonInclude]
        public Uri DescriptionLink
        {
            get;
            private set;
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
            new Cve { Id = id, DescriptionLink = new Uri(address) };
    }
}
