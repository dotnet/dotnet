// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens.Experimental;
using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    public partial class SamlSecurityTokenHandler : SecurityTokenHandler
    {
        /// <summary>
        /// Converts a string into an instance of <see cref="SamlSecurityToken"/>, returned inside of a <see cref="ValidationResult{SecurityToken, ValidationError}"/>.
        /// </summary>
        /// <param name="token">A Saml token as a string.</param>
        /// <param name="callContext"></param>
        /// <returns>A <see cref="ValidationResult{SecurityToken, ValidationError}"/> with the <see cref="SamlSecurityToken"/> or a <see cref="ValidationError"/>.</returns>
        internal virtual ValidationResult<SecurityToken, ValidationError> ReadSamlToken(string token, CallContext callContext)
        {
            if (string.IsNullOrEmpty(token))
                return ValidationError.NullParameter(
                    nameof(token),
                    ValidationError.GetCurrentStackFrame());

            if (token.Length > MaximumTokenSizeInBytes)
                return new ValidationError(
                    new MessageDetail(
                        TokenLogMessages.IDX10209,
                        LogHelper.MarkAsNonPII(token.Length),
                        LogHelper.MarkAsNonPII(MaximumTokenSizeInBytes)),
                    ValidationFailureType.TokenExceedsMaximumSize,
                    ValidationError.GetCurrentStackFrame());

            try
            {
                using (var reader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(token), BoundedXmlDictionaryReaderQuotas.Quotas))
                {
                    return ReadSamlToken(reader);
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return new ValidationError(
                    new MessageDetail(LogMessages.IDX11402, ex.Message),
                    ValidationFailureType.TokenReadingFailed,
                    ValidationError.GetCurrentStackFrame(),
                    ex);
            }
        }
    }
}
