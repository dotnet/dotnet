// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Microsoft.IdentityModel.TestExtensions;
using Microsoft.IdentityModel.Tokens;
using Xunit;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class JsonWebTokenHandlerValidateTokenAsyncTestsE2e
    {
        private TestTokenCreator testTokenCreator = new TestTokenCreator()
        {
            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials
        };
        private JsonWebTokenHandler jsonWebTokenHandler = new JsonWebTokenHandler();

        private IResultBasedValidation resultBasedJsonWebTokenHandler => jsonWebTokenHandler;

        [Fact]
        public async Task TestNotYetValidToken()
        {
            string token = testTokenCreator.CreateNotYetValidToken();
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters(
                audiences: ["http://Default.Audience.com"],
                issuers: ["http://Default.Issuer.com"],
                signingKeys: [KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key]);
            CallContext callContext = new CallContext();

            ValidationResult<ValidatedToken, ValidationError> validationResult = await resultBasedJsonWebTokenHandler.ValidateTokenAsync(token, validationParameters, callContext, default);

            Assert.False(validationResult.Succeeded);
            Assert.Null(validationResult.Result);
            Assert.NotNull(validationResult.Error);
            Assert.IsType<LifetimeValidationError>(validationResult.Error);
            Assert.Contains("IDX10222:", validationResult.Error.Message);
            // IDX10222: Lifetime validation failed. The token is not yet valid. ValidFrom (UTC): '1/6/2025 9:03:26 PM', Current time (UTC): '1/6/2025 5:03:26 PM'.
        }

        [Fact]
        public async Task TestTokenWithFutureIssuedAt()
        {
            string token = testTokenCreator.CreateTokenWithFutureIssuedAt();
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters(
                audiences: ["http://Default.Audience.com"],
                issuers: ["http://Default.Issuer.com"],
                signingKeys: [KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key]
            );
            CallContext callContext = new CallContext();

            ValidationResult<ValidatedToken, ValidationError> validationResult = await resultBasedJsonWebTokenHandler.ValidateTokenAsync(token, validationParameters, callContext, default);

            Assert.True(validationResult.Succeeded);
            Assert.NotNull(validationResult.Result);
            Assert.Null(validationResult.Error);
            // TODO: Define potentially adding a setting to reject tokens issued in the future.
            // As it is not part of the specification, it should be optional.
        }

        [Fact]
        public async Task TestTokenWithMissingIssuedAt()
        {
            string token = testTokenCreator.CreateTokenWithMissingIssuedAt();
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters(
                audiences: ["http://Default.Audience.com"],
                issuers: ["http://Default.Issuer.com"],
                signingKeys: [KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key]
            );
            CallContext callContext = new CallContext();

            ValidationResult<ValidatedToken, ValidationError> validationResult = await resultBasedJsonWebTokenHandler.ValidateTokenAsync(token, validationParameters, callContext, default);

            Assert.True(validationResult.Succeeded);
            Assert.NotNull(validationResult.Result);
            Assert.Null(validationResult.Error);
            // The iat claim is optional.
        }

        [Fact]
        public async Task TestTokenWithMissingNotBefore()
        {
            string token = testTokenCreator.CreateTokenWithMissingNotBefore();
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters(
                audiences: ["http://Default.Audience.com"],
                issuers: ["http://Default.Issuer.com"],
                signingKeys: [KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key]
            );
            CallContext callContext = new CallContext();

            ValidationResult<ValidatedToken, ValidationError> validationResult = await resultBasedJsonWebTokenHandler.ValidateTokenAsync(token, validationParameters, callContext, default);

            Assert.True(validationResult.Succeeded);
            Assert.NotNull(validationResult.Result);
            Assert.Null(validationResult.Error);
            // The nbf claim is optional.
        }
    }
}
