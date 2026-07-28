// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.TestUtils
{
    public class ValidationUtils
    {
        internal static async Task RunValidTest(ValidateTokenTheoryData theoryData, CompareContext context)
        {
            if (theoryData.TestingTokenHandler != null)
            {
                TokenValidationResult tokenValidationResult =
                    await theoryData.TestingTokenHandler.ValidateTokenAsync(
                        theoryData.Token,
                        theoryData.TokenValidationParameters!);

                ValidationResult<ValidatedToken, ValidationError> validationResult =
                    await theoryData.TestingTokenHandler.ValidateTokenAsync(
                        theoryData.Token,
                        theoryData.ValidationParameters!,
                        theoryData.CallContext,
                        CancellationToken.None);

                if (tokenValidationResult.IsValid != validationResult.Succeeded)
                    context.AddDiff($"tokenValidationResult.IsValid: '{tokenValidationResult.IsValid}' != validationResult.Succeeded: '{validationResult.Succeeded}'.");

                if (!validationResult.Succeeded)
                {
                    context.AddDiff($"Expected test to succeed, test failed: {theoryData.TestId}.");
                    context.AddDiff($"Message: {validationResult.Error!.Message}");
                }
                else
                {
                    IdentityComparer.AreEqual(validationResult.Result!.SecurityToken, tokenValidationResult.SecurityToken, context);
                }
            }
            else
            {
                context.AddDiff("TestingTokenHandler is null, cannot run test.");
            }
        }

        internal static async Task RunInvalidTest(ValidateTokenTheoryData theoryData, CompareContext context)
        {
            if (theoryData.TestingTokenHandler != null)
            {
                TokenValidationResult tokenValidationResult =
                    await theoryData.TestingTokenHandler!.ValidateTokenAsync(
                        theoryData.Token,
                        theoryData.TokenValidationParameters!);

                ValidationResult<ValidatedToken, ValidationError> validationResult =
                    await theoryData.TestingTokenHandler.ValidateTokenAsync(
                        theoryData.Token,
                        theoryData.ValidationParameters!,
                        theoryData.CallContext,
                        CancellationToken.None);

                // TODO add check for ValidationErrorType in the validationResult.Error

                if (tokenValidationResult.IsValid != validationResult.Succeeded)
                    context.AddDiff($"tokenValidationResult.IsValid: '{tokenValidationResult.IsValid}' != OperationResult.Succeeded: '{validationResult.Succeeded}'.");

                if (tokenValidationResult.IsValid)
                    context.AddDiff($"Expected test to fail, test succeeded (TokenValidationResult): {theoryData.TestId}.");

                if (validationResult.Succeeded)
                {
                    context.AddDiff($"Expected test to fail, test succeeded (OperationResult): {theoryData.TestId}.");
                }
                else
                {
                    // The default value is set for JWT tokens as these are the most commonly used tokens.
                    // RelaxTVPException property is used to accommodate exception differences TVP validation errors between JWT, SAML and SAML2 tokens.
                    // The model using ValidationParameters, all tokens will provide the same exception for validation errors.
                    if (!theoryData.RelaxTVPException)
                    {
                        theoryData.ExpectedException.ProcessException(tokenValidationResult.Exception, context, "TokenValidationResult");
                    }
                    else
                    {
                        // check that the exception is derived from SecurityTokenException
                        if (tokenValidationResult.Exception is not SecurityTokenException securityTokenException)
                        {
                            context.AddDiff($"Expected SecurityTokenException, got: {tokenValidationResult.Exception.GetType().FullName}.");
                        }
                    }

                    theoryData.ExpectedExceptionValidationParameters!.ProcessException(validationResult.Error!.GetException(), context, "OperationResult");
                }
            }
            else
            {
                context.AddDiff("TestingTokenHandler is null, cannot run test.");
            }
        }

        public static OpenIdConnectConfiguration CreateDeaultOpenIdConntectConfiguration()
        {
            OpenIdConnectConfiguration openIdConnectConfiguration = new OpenIdConnectConfiguration();
            openIdConnectConfiguration.SigningKeys.Add(Default.AsymmetricSigningCredentials.Key);
            openIdConnectConfiguration.Issuer = Default.Issuer;

            return openIdConnectConfiguration;
        }

        public static TokenValidationParameters CreateTokenValidationParameters(
            IList<string>? algorithms = null,
            IList<string>? audiences = null,
            TimeSpan? clockSkew = null,
            IList<string>? issuers = null,
            IList<SecurityKey>? keys = null,
            TimeProvider? timeProvider = null,
            TokenReplayCache? tokenReplayCache = null)
        {

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters();

            if (algorithms != null)
                tokenValidationParameters.ValidAlgorithms = algorithms;

            if (audiences != null)
                tokenValidationParameters.ValidAudiences = audiences;
            else
                tokenValidationParameters.ValidAudience = Default.Audience;

            if (clockSkew.HasValue)
                tokenValidationParameters.ClockSkew = clockSkew.Value;

            if (issuers != null)
                tokenValidationParameters.ValidIssuers = issuers;
            else
                tokenValidationParameters.ValidIssuer = Default.Issuer;


            if (keys != null)
                tokenValidationParameters.IssuerSigningKeys = keys;
            else
                tokenValidationParameters.IssuerSigningKey = Default.AsymmetricSigningCredentials.Key;


            if (timeProvider != null)
                tokenValidationParameters.TimeProvider = timeProvider;

            if (tokenReplayCache != null)
            {
                tokenValidationParameters.TokenReplayCache = tokenReplayCache;
                tokenValidationParameters.ValidateTokenReplay = true;
            }

            return tokenValidationParameters;
        }

        public static ValidationParameters CreateValidationParameters(
            List<string>? algorithms = null,
            List<string>? audiences = null,
            List<string>? issuers = null,
            List<SecurityKey>? decryptionKeys = null,
            List<SecurityKey>? signingKeys = null,
            TimeSpan? clockSkew = null,
            TimeProvider? timeProvider = null,
            TokenReplayCache? tokenReplayCache = null)
        {
            ValidationParameters validationParameters = new ValidationParameters();

            if (algorithms != null)
                algorithms.ForEach(validationParameters.ValidAlgorithms.Add);

            if (audiences != null)
                audiences.ForEach(validationParameters.ValidAudiences.Add);
            else
                validationParameters.ValidAudiences.Add(Default.Audience);

            if (clockSkew.HasValue)
                validationParameters.ClockSkew = clockSkew.Value;

            if (issuers != null)
                issuers.ForEach(validationParameters.ValidIssuers.Add);
            else
                validationParameters.ValidIssuers.Add(Default.Issuer);

            if (signingKeys != null)
                signingKeys.ForEach(validationParameters.SigningKeys.Add);
            else
                validationParameters.SigningKeys.Add(Default.AsymmetricSigningCredentials.Key);

            if (decryptionKeys != null)
                decryptionKeys.ForEach(validationParameters.DecryptionKeys.Add);

            if (tokenReplayCache != null)
                validationParameters.TokenReplayCache = tokenReplayCache;

            if (timeProvider != null)
                validationParameters.TimeProvider = timeProvider;

            return validationParameters;
        }
    }
}
