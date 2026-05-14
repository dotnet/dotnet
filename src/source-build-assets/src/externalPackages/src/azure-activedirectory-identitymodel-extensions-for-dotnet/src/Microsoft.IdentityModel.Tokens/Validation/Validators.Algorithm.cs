// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.IdentityModel.Logging;

#nullable enable
namespace Microsoft.IdentityModel.Tokens
{
    public static partial class Validators
    {
        /// <summary>
        /// Validates a given algorithm for a <see cref="SecurityKey"/>.
        /// </summary>
        /// <param name="algorithm">The algorithm to be validated.</param>
        /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <param name="callContext"></param>
#pragma warning disable CA1801 // TODO: remove pragma disable once callContext is used for logging
        internal static AlgorithmValidationResult ValidateAlgorithm(
            string algorithm,
            SecurityKey securityKey,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
#pragma warning restore CA1801 // TODO: remove pragma disable once callContext is used for logging
        {
            if (validationParameters == null)
            {
                return new AlgorithmValidationResult(
                    algorithm,
                    ValidationFailureType.NullArgument,
                    new ExceptionDetail(
                        new MessageDetail(
                            LogMessages.IDX10000,
                            LogHelper.MarkAsNonPII(nameof(validationParameters))),
                        typeof(ArgumentNullException),
                        new StackFrame(true)));
            }

            if (validationParameters.ValidAlgorithms != null && validationParameters.ValidAlgorithms.Count > 0 && !validationParameters.ValidAlgorithms.Contains(algorithm, StringComparer.Ordinal))
            {
                return new AlgorithmValidationResult(
                    algorithm,
                    ValidationFailureType.AlgorithmValidationFailed,
                    new ExceptionDetail(
                        new MessageDetail(
                            LogMessages.IDX10696,
                            LogHelper.MarkAsNonPII(algorithm)),
                        typeof(SecurityTokenInvalidAlgorithmException),
                        new StackFrame(true)));
            }

            return new AlgorithmValidationResult(algorithm);
        }
    }
}
#nullable restore
