// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;

#nullable enable
namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Definition for delegate that will validate the lifetime of a <see cref="SecurityToken"/>.
    /// </summary>
    /// <param name="notBefore">The 'notBefore' time found in the <see cref="SecurityToken"/>.</param>
    /// <param name="expires">The 'expiration' time found in the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <param name="callContext"></param>
    /// <returns>A <see cref="IssuerValidationResult"/>that contains the results of validating the issuer.</returns>
    /// <remarks>This delegate is not expected to throw.</remarks>
    internal delegate LifetimeValidationResult LifetimeValidationDelegate(
        DateTime? notBefore,
        DateTime? expires,
        SecurityToken? securityToken,
        TokenValidationParameters validationParameters,
        CallContext callContext);

    /// <summary>
    /// IssuerValidation
    /// </summary>
    public static partial class Validators
    {
        /// <summary>
        /// Validates the lifetime of a <see cref="SecurityToken"/>.
        /// </summary>
        /// <param name="notBefore">The 'notBefore' time found in the <see cref="SecurityToken"/>.</param>
        /// <param name="expires">The 'expiration' time found in the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext"></param>
        /// <returns>A <see cref="LifetimeValidationResult"/> indicating whether validation was successful, and providing a <see cref="SecurityTokenInvalidLifetimeException"/> if it was not.</returns>
        /// <exception cref="ArgumentNullException">If 'validationParameters' is null.</exception>
        /// <exception cref="SecurityTokenNoExpirationException">If 'expires.HasValue' is false and <see cref="TokenValidationParameters.RequireExpirationTime"/> is true.</exception>
        /// <exception cref="SecurityTokenInvalidLifetimeException">If 'notBefore' is &gt; 'expires'.</exception>
        /// <exception cref="SecurityTokenNotYetValidException">If 'notBefore' is &gt; DateTime.UtcNow.</exception>
        /// <exception cref="SecurityTokenExpiredException">If 'expires' is &lt; DateTime.UtcNow.</exception>
        /// <remarks>All time comparisons apply <see cref="TokenValidationParameters.ClockSkew"/>.</remarks>
        /// <remarks>Exceptions are not thrown, but embedded in <see cref="LifetimeValidationResult.Exception"/>.</remarks>
#pragma warning disable CA1801 // TODO: remove pragma disable once callContext is used for logging
        internal static LifetimeValidationResult ValidateLifetime(DateTime? notBefore, DateTime? expires, SecurityToken? securityToken, TokenValidationParameters validationParameters, CallContext callContext)
#pragma warning restore CA1801
        {
            if (validationParameters == null)
                return new LifetimeValidationResult(
                    notBefore,
                    expires,
                    ValidationFailureType.NullArgument,
                    new ExceptionDetail(
                        new MessageDetail(
                            LogMessages.IDX10000,
                            LogHelper.MarkAsNonPII(nameof(validationParameters))),
                        typeof(ArgumentNullException),
                        new StackFrame(true)));

            if (validationParameters.LifetimeValidator != null)
                return ValidateLifetimeUsingDelegate(notBefore, expires, securityToken, validationParameters);

            if (!validationParameters.ValidateLifetime)
            {
                LogHelper.LogInformation(LogMessages.IDX10238);
                return new LifetimeValidationResult(notBefore, expires);
            }

            if (!expires.HasValue && validationParameters.RequireExpirationTime)
                return new LifetimeValidationResult(
                    notBefore,
                    expires,
                    ValidationFailureType.LifetimeValidationFailed,
                    new ExceptionDetail(
                        new MessageDetail(
                            LogMessages.IDX10225,
                            LogHelper.MarkAsNonPII(securityToken == null ? "null" : securityToken.GetType().ToString())),
                        typeof(SecurityTokenNoExpirationException),
                        new StackFrame(true)));

            if (notBefore.HasValue && expires.HasValue && (notBefore.Value > expires.Value))
                return new LifetimeValidationResult(
                    notBefore,
                    expires,
                    ValidationFailureType.LifetimeValidationFailed,
                    new ExceptionDetail(
                        new MessageDetail(
                            LogMessages.IDX10224,
                            LogHelper.MarkAsNonPII(notBefore.Value),
                            LogHelper.MarkAsNonPII(expires.Value)),
                        typeof(SecurityTokenInvalidLifetimeException),
                        new StackFrame(true)));                

            DateTime utcNow = DateTime.UtcNow;
            if (notBefore.HasValue && (notBefore.Value > DateTimeUtil.Add(utcNow, validationParameters.ClockSkew)))
                return new LifetimeValidationResult(
                    notBefore,
                    expires,
                    ValidationFailureType.LifetimeValidationFailed,
                    new ExceptionDetail(
                        new MessageDetail(
                            LogMessages.IDX10222,
                            LogHelper.MarkAsNonPII(notBefore.Value),
                            LogHelper.MarkAsNonPII(utcNow)),
                        typeof(SecurityTokenNotYetValidException),
                        new StackFrame(true)));

            if (expires.HasValue && (expires.Value < DateTimeUtil.Add(utcNow, validationParameters.ClockSkew.Negate())))
                return new LifetimeValidationResult(
                    notBefore,
                    expires,
                    ValidationFailureType.LifetimeValidationFailed,
                    new ExceptionDetail(
                        new MessageDetail(
                            LogMessages.IDX10223,
                            LogHelper.MarkAsNonPII(expires.Value),
                            LogHelper.MarkAsNonPII(utcNow)),
                        typeof(SecurityTokenExpiredException),
                        new StackFrame(true)));

            // if it reaches here, that means lifetime of the token is valid
            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                LogHelper.LogInformation(LogMessages.IDX10239);

            return new LifetimeValidationResult(notBefore, expires);
        }

        private static LifetimeValidationResult ValidateLifetimeUsingDelegate(DateTime? notBefore, DateTime? expires, SecurityToken? securityToken, TokenValidationParameters validationParameters)
        {
            try
            {
                if (!validationParameters.LifetimeValidator(notBefore, expires, securityToken, validationParameters))
                    return new LifetimeValidationResult(
                    notBefore,
                    expires,
                    ValidationFailureType.LifetimeValidationFailed,
                    new ExceptionDetail(
                        new MessageDetail(
                            LogMessages.IDX10230,
                            securityToken),
                        typeof(SecurityTokenInvalidLifetimeException),
                        new StackFrame(true)));

                return new LifetimeValidationResult(notBefore, expires);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception delegateException)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return new LifetimeValidationResult(
                    notBefore,
                    expires,
                    ValidationFailureType.LifetimeValidationFailed,
                    new ExceptionDetail(
                        new MessageDetail(
                            LogMessages.IDX10230,
                            securityToken),
                        delegateException.GetType(),
                        new StackFrame(true),
                        delegateException));
            }
        }
    }
}
#nullable restore
