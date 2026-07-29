// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace Microsoft.IdentityModel.Tokens.Validation.Tests
{
    public class ValidationParametersTests
    {
        [Fact]
        public void SetValidators_NullValue_ThrowsArgumentNullException()
        {
            var validationParameters = new ValidationParameters();
            Assert.Throws<ArgumentNullException>(() => validationParameters.IssuerValidatorAsync = null);
            Assert.Throws<ArgumentNullException>(() => validationParameters.TokenReplayValidator = null);
            Assert.Throws<ArgumentNullException>(() => validationParameters.LifetimeValidator = null);
            Assert.Throws<ArgumentNullException>(() => validationParameters.TokenTypeValidator = null);
            Assert.Throws<ArgumentNullException>(() => validationParameters.AudienceValidator = null);
            Assert.Throws<ArgumentNullException>(() => validationParameters.SignatureKeyValidator = null);
        }

        [Fact]
        public void ValidIssuers_GetReturnsEmptyList()
        {
            var validationParameters = new ValidationParameters();

            Assert.Empty(validationParameters.ValidIssuers);
        }

        [Fact]
        public void ValidAudiences_Get_ReturnsEmptyList()
        {
            var validationParameters = new ValidationParameters();

            Assert.Empty(validationParameters.ValidAudiences);
            Assert.True(validationParameters.ValidAudiences is IList<string>);
        }

        [Fact]
        public void ValidTypes_Get_ReturnsEmptyList()
        {
            var validationParameters = new ValidationParameters();

            Assert.Empty(validationParameters.ValidTypes);
            Assert.True(validationParameters.ValidTypes is IList<string>);
        }

        [Fact]
        public void Valid_Set_TimeProvider()
        {
            TimeProvider timeProvider = new MockTimeProvider();
            var validationParameters = new ValidationParameters()
            {
                TimeProvider = timeProvider
            };

            Assert.Equal(validationParameters.TimeProvider, timeProvider);
        }

        [Fact]
        public void Valid_NotNull_TimeProvider()
        {
            var validationParameters = new ValidationParameters();

            Assert.NotNull(validationParameters.TimeProvider);
        }
    }
}
