// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// partial class for the IssuerValidation delegate.
    /// </summary>
    public partial class TokenValidationParameters
    {
        /// <summary>
        /// Gets or sets a delegate that will be used to validate the issuer of a <see cref="SecurityToken"/>.
        /// </summary>
        internal IssuerValidationDelegateAsync IssuerValidationDelegateAsync { get; set; }
    }
}
