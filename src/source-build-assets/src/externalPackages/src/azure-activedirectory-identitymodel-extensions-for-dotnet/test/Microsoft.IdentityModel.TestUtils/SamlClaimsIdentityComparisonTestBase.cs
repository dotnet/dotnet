// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.TestUtils.TokenValidationExtensibility.Tests;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.TestUtils
{
    public partial class SamlClaimsIdentityComparisonTestBase
    {
        public static async Task ValidateTokenAsync_ClaimsIdentity_Comparison(
            object testInstance, string methodName, string tokenHandlerType)
        {
            var context = new CompareContext($"{testInstance}.{methodName}");

            List<string> issuers = [Default.Issuer];
            List<string> audiences = [Default.Audience];
            var signingKey = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2.Key;

            ValidationParameters validationParameters = CreateValidationParameters(
                issuers, audiences, signingKey);
            TokenValidationParameters tokenValidationParameters = CreateTokenValidationParameters(
                issuers, audiences, signingKey);

            ITestingTokenHandler tokenHandler = ExtensibilityTheoryData
                .CreateSecurityTokenHandlerForType(tokenHandlerType);

            DateTime utcNow = DateTime.UtcNow;
            string token = tokenHandler.CreateStringToken(new()
            {
                Subject = Default.SamlClaimsIdentity,
                Issuer = Default.Issuer,
                Audience = Default.Audience,
                SigningCredentials = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2,
                IssuedAt = utcNow.AddHours(-1),
                Expires = utcNow.AddHours(1),
                NotBefore = utcNow.AddHours(-1),
            });

            ValidationResult<ValidatedToken, ValidationError> validationResult = await tokenHandler.ValidateTokenAsync(
                token,
                validationParameters,
                new CallContext(),
                CancellationToken.None);

            TokenValidationResult tokenValidationResult = await tokenHandler.ValidateTokenAsync(
                token,
                tokenValidationParameters);

            IdentityComparer.AreBoolsEqual(
                validationResult.Succeeded,
                tokenValidationResult.IsValid, context);

            IdentityComparer.AreClaimsIdentitiesEqual(
                validationResult.Result!.ClaimsIdentity,
                tokenValidationResult.ClaimsIdentity, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        static ValidationParameters CreateValidationParameters(
                List<string> issuers,
                List<string> audiences,
                SecurityKey signingKey)
        {
            var validationParameters = new ValidationParameters();
            validationParameters.SigningKeys.Add(signingKey);
            audiences.ForEach(validationParameters.ValidAudiences.Add);
            issuers.ForEach(validationParameters.ValidIssuers.Add);
            return validationParameters;
        }

        static TokenValidationParameters CreateTokenValidationParameters(
            List<string> issuers,
            List<string> audiences,
            SecurityKey signingKey)
        {
            var tokenValidationParameters = new TokenValidationParameters();
            tokenValidationParameters.ValidAudiences = audiences;
            tokenValidationParameters.ValidIssuers = issuers;
            tokenValidationParameters.IssuerSigningKey = signingKey;
            return tokenValidationParameters;
        }
    }
}
