// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

        [JsonConstructor]
        internal Cve([JsonProperty(PropertyName = "cve-id")] string id, [JsonProperty(PropertyName = "cve-url")] string address)
        {
            Id = id;
            DescriptionLink = new Uri(address);
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
        /// The default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            int hashCode = 315393214;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<Uri>.Default.GetHashCode(DescriptionLink);
            return hashCode;
        }
    }
}
