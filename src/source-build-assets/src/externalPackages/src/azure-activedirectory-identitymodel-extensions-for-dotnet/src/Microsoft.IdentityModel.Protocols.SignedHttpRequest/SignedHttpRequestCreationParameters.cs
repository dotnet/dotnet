// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest
{
    /// <summary>
    /// Defines a set of parameters that are used by a <see cref="SignedHttpRequestHandler"/> when creating a SignedHttpRequest.
    /// </summary>
    public class SignedHttpRequestCreationParameters
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="ConfirmationClaimTypes.Cnf"/> claim should be created and added or not.
        /// </summary>
        /// <remarks>
        /// <see cref="SignedHttpRequestDescriptor.CnfClaimValue"/> will be used as a "cnf" claim value, if set. 
        /// Otherwise, a "cnf" claim value will be derived from <see cref="SignedHttpRequestDescriptor.SigningCredentials"/>.
        /// </remarks>
        public bool CreateCnf { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SignedHttpRequestClaimTypes.Nonce"/> claim should be created and added or not.
        /// </summary>
        public bool CreateNonce { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SignedHttpRequestClaimTypes.Ts"/> claim should be created and added or not.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3</remarks> 
        public bool CreateTs { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SignedHttpRequestClaimTypes.M"/> claim should be created and added or not.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3</remarks> 
        public bool CreateM { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SignedHttpRequestClaimTypes.U"/> claim should be created and added or not.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3</remarks> 
        public bool CreateU { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SignedHttpRequestClaimTypes.P"/> claim should be created and added or not.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3</remarks>
        public bool CreateP { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SignedHttpRequestClaimTypes.Q"/> claim should be created and added or not.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3</remarks> 
        public bool CreateQ { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SignedHttpRequestClaimTypes.H"/> claim should be created and added or not.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3</remarks> 
        public bool CreateH { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SignedHttpRequestClaimTypes.B"/> claim should be created and added or not.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3</remarks> 
        public bool CreateB { get; set; }

        /// <summary>
        /// Default value for the <see cref="TimeAdjustment"/>.
        /// </summary>
        public static readonly TimeSpan DefaultTimeAdjustment = TimeSpan.Zero;

        /// <summary>
        /// Gets or sets a time adjustment to apply when creating the timestamp ("ts") claim.
        /// </summary>
        /// <remarks>Allows for adjusting the local time so it matches a server time.</remarks>
        public TimeSpan TimeAdjustment { get; set; } = DefaultTimeAdjustment;
    }
}
