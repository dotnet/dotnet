// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// A derived <see cref="ClaimsIdentity"/> where claim retrieval is case-sensitive. The current <see cref="ClaimsIdentity"/> retrieves claims in a case-insensitive manner which is different than querying the underlying <see cref="SecurityToken"/>. The <see cref="CaseSensitiveClaimsIdentity"/> provides consistent retrieval logic between the <see cref="SecurityToken"/> and <see cref="ClaimsIdentity"/>.
    /// </summary>
    public class CaseSensitiveClaimsIdentity : ClaimsIdentity
    {
        /// <summary>
        /// Gets the <see cref="SecurityToken"/> associated with this claims identity.
        /// </summary>
        public SecurityToken SecurityToken { get; internal set; }

        /// <summary>
        /// Initializes an instance of <see cref="CaseSensitiveClaimsIdentity"/>.
        /// </summary>
        public CaseSensitiveClaimsIdentity() : base()
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="CaseSensitiveClaimsIdentity"/>.
        /// </summary>
        /// <param name="authenticationType">The authentication method used to establish this identity.</param>
        public CaseSensitiveClaimsIdentity(string authenticationType) : base(authenticationType)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="CaseSensitiveClaimsIdentity"/>.
        /// </summary>
        /// <param name="claimsIdentity"><see cref="ClaimsIdentity"/> to copy.</param>
        public CaseSensitiveClaimsIdentity(ClaimsIdentity claimsIdentity) : base(claimsIdentity)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="CaseSensitiveClaimsIdentity"/>.
        /// </summary>
        /// <param name="claims"><see cref="IEnumerable{Claim}"/> associated with this instance.</param>
        public CaseSensitiveClaimsIdentity(IEnumerable<Claim> claims) : base(claims)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="CaseSensitiveClaimsIdentity"/>.
        /// </summary>
        /// <param name="claims"><see cref="IEnumerable{Claim}"/> associated with this instance.</param>
        /// <param name="authenticationType">The authentication method used to establish this identity.</param>
        public CaseSensitiveClaimsIdentity(IEnumerable<Claim> claims, string authenticationType) : base(claims, authenticationType)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="CaseSensitiveClaimsIdentity"/>.
        /// </summary>
        /// <param name="claims"><see cref="IEnumerable{Claim}"/> associated with this instance.</param>
        /// <param name="authenticationType">The authentication method used to establish this identity.</param>
        /// <param name="nameType">The <see cref="Claim.Type"/> used when obtaining the value of <see cref="ClaimsIdentity.Name"/>.</param>
        /// <param name="roleType">The <see cref="Claim.Type"/> used when performing logic for <see cref="ClaimsPrincipal.IsInRole"/>.</param>
        public CaseSensitiveClaimsIdentity(IEnumerable<Claim> claims, string authenticationType, string nameType, string roleType) :
            base(claims, authenticationType, nameType, roleType)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="CaseSensitiveClaimsIdentity"/>.
        /// </summary>
        /// <param name="authenticationType">The authentication method used to establish this identity.</param>
        /// <param name="nameType">The <see cref="Claim.Type"/> used when obtaining the value of <see cref="ClaimsIdentity.Name"/>.</param>
        /// <param name="roleType">The <see cref="Claim.Type"/> used when performing logic for <see cref="ClaimsPrincipal.IsInRole"/>.</param>
        public CaseSensitiveClaimsIdentity(string authenticationType, string nameType, string roleType) :
            base(authenticationType, nameType, roleType)
        {
        }

        /// <summary>
        /// Retrieves a <see cref="IEnumerable{Claim}"/> where each <see cref="Claim.Type"/> equals <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of the claim to match.</param>
        /// <returns>A <see cref="IEnumerable{Claim}"/> of matched claims.</returns>
        /// <remarks>Comparison is <see cref="StringComparison.Ordinal"/>.</remarks>
        /// <exception cref="ArgumentNullException">if <paramref name="type"/> is null.</exception>
        public override IEnumerable<Claim> FindAll(string type)
        {
            return base.FindAll(claim => claim?.Type.Equals(type, StringComparison.Ordinal) == true);
        }

        /// <summary>
        /// Retrieves the first <see cref="Claim"/> where <see cref="Claim.Type"/> equals <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of the claim to match.</param>
        /// <returns>A <see cref="Claim"/>, <see langword="null"/> if nothing matches.</returns>
        /// <remarks>Comparison is <see cref="StringComparison.Ordinal"/>.</remarks>
        /// <exception cref="ArgumentNullException">if <paramref name="type"/> is null.</exception>
        public override Claim FindFirst(string type)
        {
            return base.FindFirst(claim => claim?.Type.Equals(type, StringComparison.Ordinal) == true);
        }

        /// <summary>
        /// Determines if a claim with type AND value is contained within this claims identity.
        /// </summary>
        /// <param name="type">The type of the claim to match.</param>
        /// <param name="value">The value of the claim to match.</param>
        /// <returns><c>true</c> if a claim is matched, <c>false</c> otherwise.</returns>
        /// <remarks>Comparison is <see cref="StringComparison.Ordinal"/> for <see cref="Claim.Type"/> and <see cref="Claim.Value"/>.</remarks>
        /// <exception cref="ArgumentNullException">if <paramref name="type"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="value"/> is null.</exception>
        public override bool HasClaim(string type, string value)
        {
            return base.HasClaim(claim => claim?.Type.Equals(type, StringComparison.Ordinal) == true
                && claim?.Value.Equals(value, StringComparison.Ordinal) == true);
        }
    }
}
