// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.TestUtils
{
    #region CustomValidationFailures
    public class CustomValidationFailure
    {
        /// <summary>
        /// A custom validation failure type.
        /// </summary>
        public static readonly ValidationFailureType AlgorithmValidationFailed = new AlgorithmValidatorFailure("AlgorithmValidationFailed");
        private class AlgorithmValidatorFailure : ValidationFailureType { internal AlgorithmValidatorFailure(string name) : base(name) { } }

        /// <summary>
        /// Audience failure type.
        /// </summary>
        public static readonly ValidationFailureType AudienceValidationFailed = new AudienceValidatorFailure("AudienceValidationFailed");

        private class AudienceValidatorFailure : ValidationFailureType { internal AudienceValidatorFailure(string name) : base(name) { } }

        /// <summary>
        /// Lifetime failure type.
        /// </summary>
        public static readonly ValidationFailureType LifetimeValidationFailed = new LifetimeValidationFailure("ValidationFailed");

        private class LifetimeValidationFailure : ValidationFailureType { internal LifetimeValidationFailure(string name) : base(name) { } }

        /// <summary>
        /// IssuerSigningKey failure type.
        /// </summary>
        public static readonly ValidationFailureType IssuerSigningKeyValidationFailed = new IssuerSigningKeyValidationFailure("IssuerSigningKeyValidationFailed");

        private class IssuerSigningKeyValidationFailure : ValidationFailureType { internal IssuerSigningKeyValidationFailure(string name) : base(name) { } }

        /// <summary>
        /// Issuer failure type.
        /// </summary>
        public static readonly ValidationFailureType IssuerValidationFailed = new IssuerValidationFailure("IssuerValidationFailed");

        private class IssuerValidationFailure : ValidationFailureType { internal IssuerValidationFailure(string name) : base(name) { } }

        /// <summary>
        /// Signature failure type.
        /// </summary>
        public static readonly ValidationFailureType SignatureValidationFailed = new SignatureValidatorFailure("SignatureValidationFailed");

        private class SignatureValidatorFailure : ValidationFailureType { internal SignatureValidatorFailure(string name) : base(name) { } }

        /// <summary>
        /// TokenReplay failure type.
        /// </summary>
        public static readonly ValidationFailureType TokenReplayValidationFailed = new TokenReplayValidationFailure("TokenReplayValidationFailed");

        private class TokenReplayValidationFailure : ValidationFailureType { internal TokenReplayValidationFailure(string name) : base(name) { } }

        /// <summary>
        /// TokenType failure type.
        /// </summary>
        public static readonly ValidationFailureType TokenTypeValidationFailed = new TokenTypeValidationFailure("TokenTypeValidationFailed");

        private class TokenTypeValidationFailure : ValidationFailureType { internal TokenTypeValidationFailure(string name) : base(name) { } }
    }
    #endregion

    #region IssuerValidationErrors
    internal class CustomIssuerValidationError : IssuerValidationError
    {
        public CustomIssuerValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            string? invalidIssuer,
            Exception? innerException = null)
            : base(messageDetail, validationFailure, stackFrame, invalidIssuer, innerException)
        {
        }

        public override Exception GetException()
        {
            if (Exception != null)
                return Exception;

            if (FailureType == CustomValidationFailure.IssuerValidationFailed)
                Exception = new CustomSecurityTokenInvalidIssuerException(MessageDetail.Message, this, null);

            return base.GetException();
        }

    }

    internal class CustomIssuerWithoutGetExceptionValidationOverrideError : IssuerValidationError
    {
        public CustomIssuerWithoutGetExceptionValidationOverrideError(MessageDetail messageDetail,
            StackFrame stackFrame,
            string? invalidIssuer) :
            base(messageDetail, IssuerValidationFailure.ValidationFailed, stackFrame, invalidIssuer)
        {
        }
    }
    #endregion

    #region AudienceValidationErrors
    internal class CustomAudienceValidationError : AudienceValidationError
    {
        public CustomAudienceValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            IList<string>? tokenAudiences,
            IList<string>? validAudiences,
            Exception? innerException = null)
            : base(messageDetail, validationFailure, stackFrame, tokenAudiences, validAudiences, innerException)
        {
        }

        public override Exception GetException()
        {
            if (Exception != null)
                return Exception;

            if (FailureType == CustomValidationFailure.AudienceValidationFailed)
                Exception = new CustomSecurityTokenInvalidAudienceException(MessageDetail.Message, this, null);

            return base.GetException();
        }
    }

    internal class CustomAudienceWithoutGetExceptionValidationOverrideError : AudienceValidationError
    {
        public CustomAudienceWithoutGetExceptionValidationOverrideError(
            MessageDetail messageDetail,
            StackFrame stackFrame,
            IList<string>? tokenAudiences,
            IList<string>? validAudiences,
            Exception? innerException = null) :
            base(messageDetail, AudienceValidationFailure.AudienceDidNotMatch, stackFrame, tokenAudiences, validAudiences, innerException)
        {
        }
    }
    #endregion

    #region LifetimeValidationErrors
    internal class CustomLifetimeValidationError : LifetimeValidationError
    {
        public CustomLifetimeValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            DateTime? notBefore,
            DateTime? expires,
            Exception? innerException = null)
            : base(messageDetail, validationFailure, stackFrame, notBefore, expires)
        {
        }

        public override Exception GetException()
        {
            if (FailureType == CustomValidationFailure.LifetimeValidationFailed)
                return new CustomSecurityTokenInvalidLifetimeException(MessageDetail.Message, this, InnerException);

            return base.GetException();
        }
    }

    internal class CustomLifetimeWithoutGetExceptionValidationOverrideError : LifetimeValidationError
    {
        public CustomLifetimeWithoutGetExceptionValidationOverrideError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            DateTime? notBefore,
            DateTime? expires,
            Exception? innerException = null)
            : base(messageDetail, validationFailure, stackFrame, notBefore, expires, innerException)
        {
        }
    }
    #endregion

    #region SignatureValidationErrors
    internal class CustomIssuerSigningKeyValidationError : SignatureKeyValidationError
    {
        public CustomIssuerSigningKeyValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            SecurityKey? securityKey,
            Exception? innerException = null)
            : base(messageDetail, validationFailure, stackFrame, securityKey, innerException)
        {
        }

        public override Exception GetException()
        {
            if (FailureType == CustomValidationFailure.IssuerSigningKeyValidationFailed)
                return new CustomSecurityTokenInvalidSigningKeyException(MessageDetail.Message, this, InnerException);

            return base.GetException();
        }
    }

    internal class CustomIssuerSigningKeyWithoutGetExceptionValidationOverrideError : SignatureKeyValidationError
    {
        public CustomIssuerSigningKeyWithoutGetExceptionValidationOverrideError(
            MessageDetail messageDetail,
            StackFrame stackFrame,
            SecurityKey? securityKey,
            Exception? innerException = null)
            : base(messageDetail, SignatureKeyValidationFailure.ValidationFailed, stackFrame, securityKey, innerException)
        {
        }
    }
    #endregion

    #region TokenTypeValidationErrors
    internal class CustomTokenTypeValidationError : TokenTypeValidationError
    {
        public CustomTokenTypeValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            string? invalidTokenType,
            Exception? innerException = null)
            : base(messageDetail, validationFailure, stackFrame, invalidTokenType, innerException)
        {
        }

        public override Exception GetException()
        {
            if (FailureType == CustomValidationFailure.TokenTypeValidationFailed)
                return new CustomSecurityTokenInvalidTypeException(MessageDetail.Message, this, InnerException);

            return base.GetException();
        }
    }

    internal class CustomTokenTypeWithoutGetExceptionValidationOverrideError : TokenTypeValidationError
    {
        public CustomTokenTypeWithoutGetExceptionValidationOverrideError(
            MessageDetail messageDetail,
            StackFrame stackFrame,
            string? invalidTokenType,
            Exception? innerException = null)
            : base(messageDetail, TokenTypeValidationFailure.ValidationFailed, stackFrame, invalidTokenType, innerException)
        {
        }
    }
    #endregion // TokenTypeValidationErrors

    #region SignatureValidationErrors
    internal class CustomSignatureValidationError : SignatureValidationError
    {
        public CustomSignatureValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            Exception? innerException = null) :
            base(messageDetail, validationFailure, stackFrame, innerException)
        {
        }

        public override Exception GetException()
        {
            if (FailureType == CustomValidationFailure.SignatureValidationFailed)
                return new CustomSecurityTokenInvalidSignatureException(MessageDetail.Message, this, InnerException);

            return base.GetException();
        }
    }
    #endregion // SignatureValidationErrors

    #region AlgorithmValidationErrors
    internal class CustomAlgorithmValidationError : AlgorithmValidationError
    {
        public CustomAlgorithmValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            string? algorithm,
            Exception? innerException = null)
            : base(messageDetail, validationFailure, stackFrame, algorithm, innerException)
        {
        }

        public override Exception GetException()
        {
            if (FailureType == CustomValidationFailure.AlgorithmValidationFailed)
                return new CustomSecurityTokenInvalidAlgorithmException(MessageDetail.Message, this, InnerException);

            return base.GetException();
        }
    }

    internal class CustomAlgorithmWithoutGetExceptionValidationOverrideError : AlgorithmValidationError
    {
        public CustomAlgorithmWithoutGetExceptionValidationOverrideError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            string? invalidAlgorithm,
            Exception? innerException = null) :
            base(messageDetail, validationFailure, stackFrame, invalidAlgorithm, innerException)
        {
        }
    }
    #endregion // AlgorithmValidationErrors

    #region TokenReplayValidationErrors
    internal class CustomTokenReplayValidationError : TokenReplayValidationError
    {
        public CustomTokenReplayValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            DateTime? expirationTime,
            Exception? innerException = null)
            : base(messageDetail, validationFailure, stackFrame, expirationTime, innerException)
        {
        }

        public override Exception GetException()
        {
            if (FailureType == CustomValidationFailure.TokenReplayValidationFailed)
                return new CustomSecurityTokenReplayDetectedException(MessageDetail.Message, this, InnerException);

            return base.GetException();
        }
    }

    internal class CustomTokenReplayWithoutGetExceptionValidationOverrideError : TokenReplayValidationError
    {
        public CustomTokenReplayWithoutGetExceptionValidationOverrideError(
            MessageDetail messageDetail,
            StackFrame stackFrame,
            DateTime? expirationTime,
            Exception? innerException = null)
            : base(messageDetail, TokenReplayValidationFailure.ValidationFailed, stackFrame, expirationTime, innerException)
        {
        }
    }
    #endregion
}
#nullable restore
