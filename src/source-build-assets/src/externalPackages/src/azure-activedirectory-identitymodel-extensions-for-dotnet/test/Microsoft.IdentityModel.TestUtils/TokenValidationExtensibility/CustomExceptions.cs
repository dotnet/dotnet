// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.TestUtils
{
    internal class CustomSecurityTokenException : SecurityTokenException
    {
        public CustomSecurityTokenException(string message, ValidationError validationError, Exception? innerException)
            : base(message, validationError, innerException)
        {
        }
    }

    internal class CustomSecurityTokenInvalidIssuerException : SecurityTokenInvalidIssuerException
    {
        public CustomSecurityTokenInvalidIssuerException(string message, ValidationError validationError, Exception? innerException)
            : base(message, validationError, innerException)
        {
        }
    }

    internal class CustomSecurityTokenInvalidAudienceException : SecurityTokenInvalidAudienceException
    {
        public CustomSecurityTokenInvalidAudienceException(string message, ValidationError validationError, Exception? innerException)
            : base(message, validationError, innerException)
        {
        }
    }

    internal class CustomSecurityTokenInvalidLifetimeException : SecurityTokenInvalidLifetimeException
    {
        public CustomSecurityTokenInvalidLifetimeException(string message, ValidationError validationError, Exception? innerException)
            : base(message, validationError, innerException)
        {
        }
    }

    internal class CustomSecurityTokenInvalidSignatureException : SecurityTokenInvalidSignatureException
    {
        public CustomSecurityTokenInvalidSignatureException(string message, ValidationError validationError, Exception? innerException)
            : base(message, validationError, innerException)
        {
        }
    }

    internal class CustomSecurityTokenInvalidAlgorithmException : SecurityTokenInvalidAlgorithmException
    {
        public CustomSecurityTokenInvalidAlgorithmException(string message, ValidationError validationError, Exception? innerException)
            : base(message, validationError, innerException)
        {
        }
        public CustomSecurityTokenInvalidAlgorithmException(string message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }

    internal class CustomSecurityTokenInvalidTypeException : SecurityTokenInvalidTypeException
    {
        public CustomSecurityTokenInvalidTypeException(string message, ValidationError validationError, Exception? innerException)
            : base(message, validationError, innerException)
        {
        }

        public CustomSecurityTokenInvalidTypeException(string message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }

    internal class CustomSecurityTokenInvalidSigningKeyException : SecurityTokenInvalidSigningKeyException
    {
        public CustomSecurityTokenInvalidSigningKeyException(string message, ValidationError validationError, Exception? innerException)
            : base(message, validationError, innerException)
        {
        }
    }

    internal class CustomSecurityTokenReplayDetectedException : SecurityTokenReplayDetectedException
    {
        public CustomSecurityTokenReplayDetectedException(string message, ValidationError validationError, Exception? innerException)
            : base(message, validationError, innerException)
        {
        }
    }

    internal class CustomSecurityTokenDecryptionFailedException : SecurityTokenDecryptionFailedException
    {
        public CustomSecurityTokenDecryptionFailedException(string message, ValidationError validationError, Exception? innerException)
            : base(message, validationError, innerException)
        {
        }
    }
}
#nullable restore
