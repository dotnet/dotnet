// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens.Experimental;
#nullable enable

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Partial class for Algorithm Validation.
    /// </summary>
    public static partial class Validators
    {
        /// <summary>
        /// Validates a <see cref="SecurityToken"/> is using a valid algorithm.
        /// </summary>
        /// <param name="algorithm">The algorithm to be validated.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters"><see cref="ValidationParameters"/> required for validation.</param>
        /// <param name="callContext">The <see cref="CallContext"/> that contains call information.</param>
        internal static ValidationResult<string, ValidationError> ValidateAlgorithmInternal(
            string? algorithm,
#pragma warning disable CA1801
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
#pragma warning restore CA1801
        {
            if (validationParameters == null)
                return new AlgorithmValidationError(
                    MessageDetail.NullParameter(nameof(validationParameters)),
                    ValidationFailureType.NullArgument,
                    ValidationError.GetCurrentStackFrame(),
                    algorithm,
                    null);

            try
            {
                ValidationResult<string, ValidationError> result =
                    validationParameters.AlgorithmValidator(
                        algorithm,
                        securityToken,
                        validationParameters,
                        callContext);

                if (!result.Succeeded)
                    return result.Error!.AddCurrentStackFrame();

                return result;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return new AlgorithmValidationError(
                    new MessageDetail(LogMessages.IDX10273),
                    AlgorithmValidationFailure.ValidatorThrew,
                    ValidationError.GetCurrentStackFrame(),
                    algorithm,
                    ex);
            }
        }

        /// <summary>
        /// Validates a <see cref="SecurityToken"/> is using a valid algorithm.
        /// </summary>
        /// <param name="algorithm">The algorithm to be validated.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters"><see cref="ValidationParameters"/> required for validation.</param>
        /// <param name="callContext">The <see cref="CallContext"/> that contains call information.</param>
        public static ValidationResult<string, ValidationError> ValidateAlgorithm(
            string? algorithm,
#pragma warning disable CA1801
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
#pragma warning restore CA1801
        {
            if (validationParameters == null)
                return new AlgorithmValidationError(
                    MessageDetail.NullParameter(nameof(validationParameters)),
                    ValidationFailureType.NullArgument,
                    ValidationError.GetCurrentStackFrame(),
                    algorithm,
                    null);

            if (string.IsNullOrEmpty(algorithm) &&
                validationParameters.ValidAlgorithms != null &&
                validationParameters.ValidAlgorithms.Count > 0)
            {
                return new AlgorithmValidationError(
                    new MessageDetail(
                        LogMessages.IDX10696,
                        LogHelper.MarkAsNonPII("null")),
                    AlgorithmValidationFailure.ValidationFailed,
                    ValidationError.GetCurrentStackFrame(),
                    "null",
                    null);

            }

            if (validationParameters.ValidAlgorithms != null &&
                validationParameters.ValidAlgorithms.Count > 0 &&
                !validationParameters.ValidAlgorithms.Contains(algorithm, StringComparer.Ordinal))
                return new AlgorithmValidationError(
                    new MessageDetail(
                        LogMessages.IDX10696,
                        LogHelper.MarkAsNonPII(algorithm)),
                    AlgorithmValidationFailure.ValidationFailed,
                    ValidationError.GetCurrentStackFrame(),
                    algorithm,
                    null);

            return algorithm!;
        }
    }
}
#nullable restore
