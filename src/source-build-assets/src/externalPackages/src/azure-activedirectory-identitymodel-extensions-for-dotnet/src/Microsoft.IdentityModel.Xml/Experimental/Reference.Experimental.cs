// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Represents a XmlDsig Reference element as per: https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-Reference
    /// </summary>
    public partial class Reference : DSigElement
    {
#nullable enable
        /// <summary>
        /// Verifies that the <see cref="DigestValue" /> equals the hashed value of the <see cref="TokenStream"/> after
        /// <see cref="Transforms"/> have been applied.
        /// </summary>
        /// <param name="cryptoProviderFactory">supplies the <see cref="HashAlgorithm"/>.</param>
        /// <param name="callContext"> contextual information for diagnostics.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="cryptoProviderFactory"/> is null.</exception>
        internal ValidationError? Verify(
            CryptoProviderFactory cryptoProviderFactory,
#pragma warning disable CA1801 // Review unused parameters
            CallContext callContext)
#pragma warning restore CA1801
        {
            if (cryptoProviderFactory == null)
                return ValidationError.NullParameter(
                    nameof(cryptoProviderFactory),
                    ValidationError.GetCurrentStackFrame());

            if (!Utility.AreEqual(ComputeDigest(cryptoProviderFactory), Convert.FromBase64String(DigestValue)))
                return new SignatureValidationError(
                    new MessageDetail(
                        LogMessages.IDX30201,
                        Uri ?? Id),
                    SignatureValidationFailure.ReferenceDigestValidationFailed,
                    ValidationError.GetCurrentStackFrame());

            return null;
        }
#nullable restore
    }
}
