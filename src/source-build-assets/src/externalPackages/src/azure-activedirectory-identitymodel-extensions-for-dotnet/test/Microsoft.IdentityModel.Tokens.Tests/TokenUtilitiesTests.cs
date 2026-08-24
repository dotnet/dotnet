// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class TokenUtilitiesTests
    {
        [Theory]
        [InlineData(typeof(SecurityTokenInvalidSignatureException), false, true)]
        [InlineData(typeof(SecurityTokenInvalidIssuerException), false, true)]
        [InlineData(typeof(SecurityTokenSignatureKeyNotFoundException), false, true)]
        [InlineData(typeof(SecurityTokenDecryptionFailedException), false, false)]
        [InlineData(typeof(SecurityTokenDecryptionFailedException), true, true)]
        [InlineData(typeof(ArgumentNullException), false, false)]
        [InlineData(typeof(ArgumentException), false, false)]
        [InlineData(typeof(SecurityTokenValidationException), false, false)]
        public void IsRecoverableException_ReturnsExpectedResult(Type exceptionType, bool configContainsDecryptionKeys, bool expected)
        {
            Exception exception = (Exception)Activator.CreateInstance(exceptionType);

            bool result = TokenUtilities.IsRecoverableException(exception, configContainsDecryptionKeys);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsRecoverableException_WithoutDecryptionKeysParameter_ReturnsFalse()
        {
            var exception = new SecurityTokenDecryptionFailedException();

            bool result = TokenUtilities.IsRecoverableException(exception);

            Assert.False(result);
        }

        [Fact]
        public void IsRecoverableException_WithNullException_ReturnsFalse()
        {
            bool result = TokenUtilities.IsRecoverableException(null);

            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(SecurityTokenInvalidSignatureException), false, true)]
        [InlineData(typeof(SecurityTokenInvalidIssuerException), false, true)]
        [InlineData(typeof(SecurityTokenSignatureKeyNotFoundException), false, true)]
        [InlineData(typeof(SecurityTokenDecryptionFailedException), false, false)]
        [InlineData(typeof(SecurityTokenDecryptionFailedException), true, true)]
        [InlineData(typeof(ArgumentNullException), false, false)]
        [InlineData(typeof(ArgumentException), false, false)]
        [InlineData(typeof(SecurityTokenValidationException), false, false)]
        [InlineData(null, false, false)]
        public void IsRecoverableExceptionType_ReturnsExpectedResult(Type exceptionType, bool configContainsDecryptionKeys, bool expected)
        {
            bool result = TokenUtilities.IsRecoverableExceptionType(exceptionType, configContainsDecryptionKeys);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsRecoverableExceptionType_WithoutDecryptionKeysParameter_ReturnsFalse()
        {
            var exceptionType = typeof(SecurityTokenDecryptionFailedException);

            bool result = TokenUtilities.IsRecoverableExceptionType(exceptionType);

            Assert.False(result);
        }
    }
}
