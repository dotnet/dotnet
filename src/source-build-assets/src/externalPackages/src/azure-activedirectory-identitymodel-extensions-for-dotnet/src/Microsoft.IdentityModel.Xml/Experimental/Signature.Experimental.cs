// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Represents a XmlDsig Signature element as per: https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-Signature
    /// </summary>
    public partial class Signature : DSigElement
    {
#nullable enable
        internal ValidationError? Verify(
            SecurityKey key,
            CryptoProviderFactory cryptoProviderFactory,
#pragma warning disable CA1801 // Review unused parameters
            CallContext callContext)
#pragma warning restore CA1801
        {
            if (key is null)
                return ValidationError.NullParameter(
                    nameof(key),
                    ValidationError.GetCurrentStackFrame());

            if (cryptoProviderFactory is null)
                return ValidationError.NullParameter(
                    nameof(cryptoProviderFactory),
                    ValidationError.GetCurrentStackFrame());

            if (SignedInfo is null)
                return new SignatureValidationError(
                    new MessageDetail(LogMessages.IDX30212),
                    ValidationFailureType.SignedInfoNull,
                    ValidationError.GetCurrentStackFrame());

            if (!cryptoProviderFactory.IsSupportedAlgorithm(SignedInfo.SignatureMethod, key))
                return new SignatureValidationError(
                    new MessageDetail(LogMessages.IDX30207, SignedInfo.SignatureMethod, cryptoProviderFactory.GetType()),
                    AlgorithmValidationFailure.AlgorithmIsNotSupported,
                    ValidationError.GetCurrentStackFrame());

            var signatureProvider = cryptoProviderFactory.CreateForVerifying(key, SignedInfo.SignatureMethod);
            if (signatureProvider is null)
                return new SignatureValidationError(
                    new MessageDetail(LogMessages.IDX30203, cryptoProviderFactory, LogHelper.MarkAsNonPII(key.KeyId), SignedInfo.SignatureMethod),
                    ValidationFailureType.CryptoProviderReturnedNull,
                    ValidationError.GetCurrentStackFrame());

            ValidationError? validationError = null;

            try
            {
                // TODO - follow JsonWebTokenHandler for IDX10511 parameters.
                using (var memoryStream = new MemoryStream())
                {
                    SignedInfo.GetCanonicalBytes(memoryStream);
                    if (!signatureProvider.Verify(memoryStream.ToArray(), Convert.FromBase64String(SignatureValue)))
                    {
                        validationError = new SignatureValidationError(
                            new MessageDetail(Tokens.LogMessages.IDX10511, "1", "1", "1", "1", "1", cryptoProviderFactory, LogHelper.MarkAsNonPII(key.KeyId)),
                            SignatureValidationFailure.ValidationFailed,
                            ValidationError.GetCurrentStackFrame());
                    }
                }

                if (validationError is null)
                {
                    validationError = SignedInfo.Verify(cryptoProviderFactory, callContext);
                    validationError?.AddCurrentStackFrame();
                }
            }
            finally
            {
                if (signatureProvider is not null)
                    cryptoProviderFactory.ReleaseSignatureProvider(signatureProvider);
            }

            if (validationError is not null)
                return validationError;

            return null; // no error
        }
#nullable restore
    }
}
