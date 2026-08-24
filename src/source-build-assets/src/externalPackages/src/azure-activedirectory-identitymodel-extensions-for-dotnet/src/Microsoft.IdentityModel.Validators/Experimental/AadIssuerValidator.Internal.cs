// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Threading;
using Microsoft.IdentityModel.Logging;
using System.Diagnostics;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace Microsoft.IdentityModel.Validators
{
    /// <summary>
    /// Generic class that validates the issuer for either JsonWebTokens or JwtSecurityTokens issued from the Microsoft identity platform (AAD).
    /// </summary>
    public partial class AadIssuerValidator
    {
        /// <summary>
        /// Validate the issuer for single and multi-tenant applications of various audiences (Work and School accounts, or Work and School accounts +
        /// Personal accounts) and the various clouds.
        /// </summary>
        /// <param name="issuer">Issuer to validate (will be tenanted).</param>
        /// <param name="securityToken">Received security token.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext">The call context used for logging.</param>
        /// <param name="cancellationToken">CancellationToken used to cancel call.</param>
        /// <returns>An <see cref="ValidationResult{ValidatedIssuer, IssuerValidationError}"/> that contains either the issuer that was validated or an error.</returns>
        /// <remarks>An EXACT match is required.</remarks>
        internal async Task<ValidationResult<ValidatedIssuer, IssuerValidationError>> ValidateIssuerAsync(
            string issuer,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
#pragma warning disable CA1801 // Review unused parameters
            CallContext callContext,
            CancellationToken cancellationToken)
#pragma warning restore CA1801 // Review unused parameters
        {
            _ = issuer ?? throw LogHelper.LogArgumentNullException(nameof(issuer));
            _ = securityToken ?? throw LogHelper.LogArgumentNullException(nameof(securityToken));
            _ = validationParameters ?? throw LogHelper.LogArgumentNullException(nameof(validationParameters));
            string tenantId;

            try
            {
                tenantId = GetTenantIdFromToken(securityToken);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return new IssuerValidationError(
                    new MessageDetail(
                        ex.Message,
                        LogHelper.MarkAsNonPII(issuer)),
                        IssuerValidationFailure.ValidationFailed,
                        new StackFrame(true),
                        issuer,
                        ex);
            }

            if (string.IsNullOrWhiteSpace(tenantId))
                return new IssuerValidationError(
                    new MessageDetail(
                        LogMessages.IDX40003,
                        LogHelper.MarkAsNonPII(issuer)),
                        IssuerValidationFailure.ValidationFailed,
                        new StackFrame(true),
                        issuer);

            for (int i = 0; i < validationParameters.ValidIssuers.Count; i++)
            {
                if (IsValidIssuer(validationParameters.ValidIssuers[i], tenantId, issuer))
                    return new ValidatedIssuer(issuer!, IssuerValidationSource.IssuerMatchedConfiguration);
            }

            try
            {
                var issuerVersion = GetTokenIssuerVersion(securityToken);
                var effectiveConfigurationManager = GetEffectiveConfigurationManager(issuerVersion);

                string aadIssuer;
                if (validationParameters.ValidateWithLKG)
                {
                    // returns null if LKG issuer expired
                    aadIssuer = GetEffectiveLKGIssuer(issuerVersion);
                }
                else
                {
                    var baseConfiguration = await GetBaseConfigurationAsync(effectiveConfigurationManager, validationParameters).ConfigureAwait(false);
                    aadIssuer = baseConfiguration.Issuer;
                }

                if (aadIssuer != null)
                {
                    var isIssuerValid = IsValidIssuer(aadIssuer, tenantId, issuer);

                    // The original LKG assignment behavior for previous self-state management.
                    if (isIssuerValid && !validationParameters.ValidateWithLKG)
                        SetEffectiveLKGIssuer(aadIssuer, issuerVersion, effectiveConfigurationManager.LastKnownGoodLifetime);

                    if (isIssuerValid)
                        return new ValidatedIssuer(issuer!, IssuerValidationSource.IssuerMatchedConfiguration);
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return new IssuerValidationError(
                    new MessageDetail(
                        LogMessages.IDX40001,
                        LogHelper.MarkAsNonPII(issuer)),
                        IssuerValidationFailure.ValidationFailed,
                        new StackFrame(true),
                        issuer,
                        ex);
            }

            // If a valid issuer is not found, return an error.
            return new IssuerValidationError(
                new MessageDetail(
                    LogMessages.IDX40001,
                    LogHelper.MarkAsNonPII(issuer)),
                    IssuerValidationFailure.ValidationFailed,
                    new StackFrame(true),
                    issuer);
        }

        private static async Task<BaseConfiguration> GetBaseConfigurationAsync(BaseConfigurationManager configurationManager, ValidationParameters validationParameters)
        {
            if (validationParameters.RefreshBeforeValidation)
                configurationManager.RequestRefresh();

            return await configurationManager.GetBaseConfigurationAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}
