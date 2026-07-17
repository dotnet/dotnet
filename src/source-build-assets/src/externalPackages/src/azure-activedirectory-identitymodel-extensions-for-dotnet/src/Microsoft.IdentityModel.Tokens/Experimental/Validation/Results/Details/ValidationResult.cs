// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

#if NET
using System.Diagnostics.CodeAnalysis;
#endif

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Experimental
{
    /// <summary>
    /// Represents the result of an validation that can either succeed with a valid result or failed with a specific error.
    /// </summary>
    /// <typeparam name="TResult">The type of the successful result value.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <remarks>
    /// This struct implements a discriminated union pattern for validation results.
    /// It ensures that either a valid result or an error is available, but never both.
    /// This pattern helps with clear error handling and propagation.
    /// </remarks>
    public readonly record struct ValidationResult<TResult, TError>
        where TError : ValidationError
    {
        /// <summary>
        /// Creates a new instance of <see cref="ValidationResult{TResult, TError}"/> indicating a successful validation
        /// and containing an object of the associated type.
        /// </summary>
        /// <param name="result">The value associated with the success.</param>
        public ValidationResult(TResult result)
        {
            Result = result;
            Error = default;
            Succeeded = true;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ValidationResult{TResult, TError}"/>
        /// indicating a failed validation and containing error information.
        /// </summary>
        /// <param name="error">The error associated with the failure.</param>
        public ValidationResult(TError error)
        {
            Result = default;
            Error = error;
            Succeeded = false;
        }

        /// <summary>
        /// Empty constructor implementation to prevent creating an empty result.
        /// </summary>
        /// <remarks>Throws an <see cref="InvalidOperationException"/> when called as this should never be used. Always initialize Result with either a value or error.</remarks>
        /// <exception cref="InvalidOperationException">Thrown when called.</exception>
        [Obsolete("Cannot create an empty validation result", true)]
        public ValidationResult() => throw new InvalidOperationException("Cannot create an empty validation result");

        /// <summary>
        /// Gets a value indicating whether the validation succeeded.
        /// </summary>
        /// <value>
        /// <c>true</c> if the validation succeeded and <see cref="Result"/> contains a valid value;
        /// <c>false</c> if the validation failed and <see cref="Error"/> contains the error.
        /// </value>
#if NET
        [MemberNotNullWhen(true, nameof(Result))]
        [MemberNotNullWhen(false, nameof(Error))]
#endif
        public readonly bool Succeeded { get; }

        /// <summary>
        /// Gets the error associated with the validation result.
        /// </summary>
        /// <returns>The error associated with the validation result.</returns>
        /// <remarks>This property is only valid if the result type is not valid.</remarks>
        public readonly TError? Error { get; }

        /// <summary>
        /// Gets the result associated with the validation result.
        /// </summary>
        /// <returns>The result associated with the validation result.</returns>
        /// <remarks>This property is only valid if the result type is valid.</remarks>
        public readonly TResult? Result { get; }

        /// <summary>
        /// Creates an <see cref="ValidationResult{TResult, TError}"/>, successful result implicitly from the value.
        /// </summary>
        /// <param name="result">The value to be stored in the result.</param>
        public static implicit operator ValidationResult<TResult, TError>(TResult result) => new(result);

        /// <summary>
        /// Creates an <see cref="ValidationResult{TResult, TError}"/> result implicitly from the error value.
        /// </summary>
        /// <param name="error">The error to be stored in the result.</param>
        public static implicit operator ValidationResult<TResult, TError>(TError error) => new(error);

        /// <summary>
        /// Casts the result to a <see cref="ValidationResult{TResult, TError}"/>.
        /// </summary>
        /// <remarks>Required for compatibility, see CA2225 for more information</remarks>
        /// <returns>The existing instance.</returns>
        public ValidationResult<TResult, TError> ToValidationResult() => this;
    }
}
#nullable restore

