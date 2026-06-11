// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using Xunit;

#nullable enable
namespace Microsoft.IdentityModel.TestUtils
{
    public class TestCaseProvider
    {
        #region Algorithm
        internal static TheoryData<ValidateTokenTheoryData> GenerateInvalidAlgorithmTestCases(ITestingTokenHandler testingTokenHandler)
        {
            TheoryData<ValidateTokenTheoryData> testCases = new();
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Audience = Default.Audience,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Issuer = Default.Issuer,
                Subject = Default.ClaimsIdentityLongNames
            };

            TokenValidationParameters tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(algorithms: [SecurityAlgorithms.EcdsaSha256]);
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters(algorithms: [SecurityAlgorithms.EcdsaSha256]);
            testCases.Add(new ValidateTokenTheoryData(SecurityAlgorithms.EcdsaSha256)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10511:"),
                ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidAlgorithmException("IDX10696:"),
                RelaxTVPException = true,
                Token = testingTokenHandler.CreateStringToken(securityTokenDescriptor),
                TestingTokenHandler = testingTokenHandler,
                TokenValidationParameters = tokenValidationParameters,
                ValidationParameters = validationParameters
            });

            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(algorithms: [SecurityAlgorithms.HmacSha256]);
            validationParameters = ValidationUtils.CreateValidationParameters(algorithms: [SecurityAlgorithms.HmacSha256]);
            testCases.Add(new ValidateTokenTheoryData(SecurityAlgorithms.HmacSha256)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10511:"),
                ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidAlgorithmException("IDX10696:"),
                RelaxTVPException = true,
                Token = testingTokenHandler.CreateStringToken(securityTokenDescriptor),
                TestingTokenHandler = testingTokenHandler,
                TokenValidationParameters = tokenValidationParameters,
                ValidationParameters = validationParameters
            });

            return testCases;
        }

        internal static TheoryData<ValidateTokenTheoryData> GenerateValidAlgorithmTestCases(ITestingTokenHandler testingTokenHandler)
        {
            TheoryData<ValidateTokenTheoryData> testCases = new TheoryData<ValidateTokenTheoryData>();

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Audience = Default.Audience,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Issuer = Default.Issuer,
                Subject = Default.ClaimsIdentityLongNames
            };

            TokenValidationParameters tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(algorithms: [Default.AsymmetricSigningCredentials.Algorithm]);
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters(algorithms: [Default.AsymmetricSigningCredentials.Algorithm]);
            testCases.Add(new ValidateTokenTheoryData(Default.AsymmetricSigningCredentials.Algorithm)
            {
                Token = testingTokenHandler.CreateStringToken(securityTokenDescriptor),
                TestingTokenHandler = testingTokenHandler,
                TokenValidationParameters = tokenValidationParameters,
                ValidationParameters = validationParameters
            });

            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            tokenValidationParameters.ValidAlgorithms = null;
            validationParameters = ValidationUtils.CreateValidationParameters(algorithms: []);
            validationParameters.ValidAlgorithms.Clear();

            testCases.Add(new ValidateTokenTheoryData("ValidAlgorithmsNull")
            {
                Token = testingTokenHandler.CreateStringToken(securityTokenDescriptor),
                TestingTokenHandler = testingTokenHandler,
                TokenValidationParameters = tokenValidationParameters,
                ValidationParameters = validationParameters
            });

            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(algorithms: []);
            validationParameters = ValidationUtils.CreateValidationParameters(algorithms: []);

            testCases.Add(new ValidateTokenTheoryData("ValidAlgorithmsEmptyList")
            {
                Token = testingTokenHandler.CreateStringToken(securityTokenDescriptor),
                TestingTokenHandler = testingTokenHandler,
                TokenValidationParameters = tokenValidationParameters,
                ValidationParameters = validationParameters
            });


            return testCases;
        }
        #endregion  

        #region Audience
        internal static TheoryData<ValidateTokenTheoryData> GenerateInvalidAudienceTestCases(ITestingTokenHandler tokenHandler)
        {
            TheoryData<ValidateTokenTheoryData> theoryData = new TheoryData<ValidateTokenTheoryData>();

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Audience = Default.Audience,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Issuer = Default.Issuer,
                Subject = Default.ClaimsIdentityLongNames
            };

            string guid = Guid.NewGuid().ToString();
            TokenValidationParameters tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(audiences: []);
            tokenValidationParameters.ValidAudience = guid;
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters(audiences: [guid]);
            theoryData.Add(
                new ValidateTokenTheoryData("AudiencesDoNotMatch")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            tokenValidationParameters.ValidAudience = null;
            tokenValidationParameters.ValidAudiences = null;
            validationParameters = ValidationUtils.CreateValidationParameters(audiences: []);
            theoryData.Add(
                new ValidateTokenTheoryData("AudiencesNullInParameters")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10208:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidAudienceException("IDX10268:"),
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(audiences: []);
            validationParameters = ValidationUtils.CreateValidationParameters(audiences: []);
            theoryData.Add(
                new ValidateTokenTheoryData("AudiencesEmptyInParameters")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidAudienceException("IDX10268:"),
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Issuer = Default.Issuer,
                Subject = Default.ClaimsIdentityLongNames
            };

            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            validationParameters = ValidationUtils.CreateValidationParameters();
            theoryData.Add(
                new ValidateTokenTheoryData("AudienceNullInToken")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10206:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidAudienceException("IDX10206:"),
                    RelaxTVPException = true,
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            return theoryData;
        }

        internal static TheoryData<ValidateTokenTheoryData> GenerateValidAudienceTestCases(ITestingTokenHandler tokenHandler)
        {
            TheoryData<ValidateTokenTheoryData> theoryData = new TheoryData<ValidateTokenTheoryData>();

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            TokenValidationParameters tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters();
            theoryData.Add(
                new ValidateTokenTheoryData("AudiencesMatch")
                {
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });


            string guid = Guid.NewGuid().ToString();
            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(audiences: [guid, Default.Audience, guid]);
            validationParameters = ValidationUtils.CreateValidationParameters(audiences: [guid, Default.Audience, guid]);
            theoryData.Add(
                new ValidateTokenTheoryData("AudienceInAudiences")
                {
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(audiences: [guid, Default.Audience + "/", guid]);
            validationParameters = ValidationUtils.CreateValidationParameters(audiences: [guid, Default.Audience + "/", guid]);
            theoryData.Add(
                new ValidateTokenTheoryData("AudienceSlashInValidationParameters")
                {
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience + "/",
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(audiences: [guid, Default.Audience, guid]);
            validationParameters = ValidationUtils.CreateValidationParameters(audiences: [guid, Default.Audience, guid]);
            theoryData.Add(
                new ValidateTokenTheoryData("AudienceSlashInToken")
                {
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            return theoryData;
        }
        #endregion

        #region Issuer
        internal static TheoryData<ValidateTokenTheoryData> GenerateInvalidIssuerTestCases(ITestingTokenHandler tokenHandler)
        {
            TheoryData<ValidateTokenTheoryData> theoryData = new TheoryData<ValidateTokenTheoryData>();

            string guid = Guid.NewGuid().ToString();
            // Token with issuer that does not match the valid issuer in parameters
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Issuer = guid,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            TokenValidationParameters tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters();
            theoryData.Add(
                new ValidateTokenTheoryData("IssuerDoesNotMatch")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX10205:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidIssuerException("IDX10212:"),
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            // Token with null issuer, should fail validation
            // SAML and SAML2 tokens do not allow setting a null or empty issuer, so this test is only for JWT tokens.
            // TODO add a specific test for SAML/SAML2 tokens.
            if (tokenHandler is JsonWebTestingTokenHandler)
            {
                // Token with issuer, but no valid issuers in parameters
                securityTokenDescriptor = new SecurityTokenDescriptor
                {
                    Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                    Audience = Default.Audience,
                    SigningCredentials = Default.AsymmetricSigningCredentials,
                    Issuer = "",
                    Subject = Default.ClaimsIdentityLongNames
                };

                tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
                validationParameters = ValidationUtils.CreateValidationParameters();
                theoryData.Add(
                    new ValidateTokenTheoryData("IssuerWhiteSpaceInToken")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX10211:"),
                        ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidIssuerException("IDX10211:"),
                        Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                        TestingTokenHandler = tokenHandler,
                        TokenValidationParameters = tokenValidationParameters,
                        ValidationParameters = validationParameters,
                    });

                securityTokenDescriptor = new SecurityTokenDescriptor
                {
                    Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                    Audience = Default.Audience,
                    SigningCredentials = Default.AsymmetricSigningCredentials,
                    Subject = Default.ClaimsIdentityLongNames
                };

                tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
                validationParameters = ValidationUtils.CreateValidationParameters();

                theoryData.Add(
                    new ValidateTokenTheoryData("IssuerNullInToken")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX10211:"),
                        ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidIssuerException("IDX10211:"),
                        Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                        TestingTokenHandler = tokenHandler,
                        TokenValidationParameters = tokenValidationParameters,
                        ValidationParameters = validationParameters,
                    });
            }

            // Token with issuer, but no valid issuers in parameters
            securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Audience = Default.Audience,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Issuer = Default.Issuer,
                Subject = Default.ClaimsIdentityLongNames
            };

            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(issuers: []);
            tokenValidationParameters.ValidIssuers = null;
            validationParameters = ValidationUtils.CreateValidationParameters(issuers: []);
            theoryData.Add(
                new ValidateTokenTheoryData("ValidIssuersNullInParameters")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX10204:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidIssuerException("IDX10212:"),
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            // Token with issuer, but empty valid issuers in parameters
            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(issuers: []);
            validationParameters = ValidationUtils.CreateValidationParameters(issuers: []);
            theoryData.Add(
                new ValidateTokenTheoryData("ValidIssuersEmptyInParameters")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX10204:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidIssuerException("IDX10212:"),
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            return theoryData;
        }

        internal static TheoryData<ValidateTokenTheoryData> GenerateValidIssuerTestCases(ITestingTokenHandler tokenHandler)
        {
            TheoryData<ValidateTokenTheoryData> theoryData = new TheoryData<ValidateTokenTheoryData>();

            // TODO - add tests for configuration
            // Valid_IssuerIsConfigurationIssuer

            string guid = Guid.NewGuid().ToString();
            // Token with issuer matching valid issuer
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Audience = Default.Audience,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Issuer = Default.Issuer,
                Subject = Default.ClaimsIdentityLongNames
            };

            TokenValidationParameters tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters();
            theoryData.Add(
                new ValidateTokenTheoryData("IssuerMatches")
                {
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            // Token with issuer in valid issuers list
            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(issuers: [guid, Default.Issuer, guid]);
            validationParameters = ValidationUtils.CreateValidationParameters(issuers: [guid, Default.Issuer, guid]);
            theoryData.Add(
                new ValidateTokenTheoryData("IssuerInValidIssuersList")
                {
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(issuers: []);
            tokenValidationParameters.ConfigurationManager = validationParameters.ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(ValidationUtils.CreateDeaultOpenIdConntectConfiguration());
            validationParameters = ValidationUtils.CreateValidationParameters(issuers: []);
            validationParameters.ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(ValidationUtils.CreateDeaultOpenIdConntectConfiguration());
            theoryData.Add(
                new ValidateTokenTheoryData("IssuerInConfig")
                {
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });
            return theoryData;
        }
        #endregion

        #region Lifetime
        internal static TheoryData<ValidateTokenTheoryData> GenerateInvalidLifetimeTestCases(ITestingTokenHandler tokenHandler)
        {
            TheoryData<ValidateTokenTheoryData> theoryData = new TheoryData<ValidateTokenTheoryData>();

            // Token expired (Expires in the past)
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow.AddMinutes(-10),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            TokenValidationParameters tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters();
            theoryData.Add(
                new ValidateTokenTheoryData("TokenExpired")
                {
                    ExpectedException = ExpectedException.SecurityTokenExpiredException("IDX10223:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidLifetimeException("IDX10223:"),
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            // Token expired (Expires in the past, but NOT within clock skew)
            securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow.AddMinutes(-3),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(clockSkew: TimeSpan.FromMinutes(2));
            validationParameters = ValidationUtils.CreateValidationParameters(clockSkew: TimeSpan.FromMinutes(2));
            theoryData.Add(
                new ValidateTokenTheoryData("TokenExpiredClockSkew")
                {
                    ExpectedException = ExpectedException.SecurityTokenExpiredException("IDX10223:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidLifetimeException("IDX10223:"),
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            // Token not yet valid (NotBefore in the future)
            securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow.AddMinutes(10),
                NotBefore = DateTime.UtcNow.AddMinutes(8),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            validationParameters = ValidationUtils.CreateValidationParameters();
            theoryData.Add(
                new ValidateTokenTheoryData("TokenNotYetValid")
                {
                    ExpectedException = ExpectedException.SecurityTokenNotYetValidException("IDX10222:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidLifetimeException("IDX10222:"),
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            // TODO SAML sets an expiation time by defaault when reading conditions, we need to reason about that.
            if (tokenHandler is not SamlSecurityTestingTokenHandler)
            {
                // Token with no expiration, should fail if RequireExpirationTime is true
                securityTokenDescriptor = new SecurityTokenDescriptor
                {
                    Audience = Default.Audience,
                    Issuer = Default.Issuer,
                    SigningCredentials = Default.AsymmetricSigningCredentials,
                    Subject = Default.ClaimsIdentityLongNames
                };

                tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
                validationParameters = ValidationUtils.CreateValidationParameters();
                theoryData.Add(
                    new ValidateTokenTheoryData("TokenNoExpiration")
                    {
                        ExpectedException = ExpectedException.SecurityTokenNoExpirationException("IDX10225:"),
                        ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidLifetimeException("IDX10225:"),
                        Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                        TestingTokenHandler = tokenHandler,
                        TokenValidationParameters = tokenValidationParameters,
                        ValidationParameters = validationParameters,
                    });
            }
            return theoryData;
        }

        internal static TheoryData<ValidateTokenTheoryData> GenerateValidLifetimeTestCases(ITestingTokenHandler tokenHandler)
        {
            TheoryData<ValidateTokenTheoryData> theoryData = new TheoryData<ValidateTokenTheoryData>();

            // Token valid (Expires in the future, NotBefore in the past)
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow.AddMinutes(10),
                NotBefore = DateTime.UtcNow.AddMinutes(-5),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };
            TokenValidationParameters tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters();
            theoryData.Add(
                new ValidateTokenTheoryData("TokenValidLifetime")
                {
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            // Token valid (Expires in the future, NotBefore in the past)
            securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow.AddMinutes(3),
                NotBefore = DateTime.UtcNow.AddMinutes(-5),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            theoryData.Add(
                new ValidateTokenTheoryData("TokenValidWithinClockSkew")
                {
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            // Token issued in the future
            securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow.AddMinutes(10),
                NotBefore = DateTime.UtcNow.AddMinutes(-1),
                IssuedAt = DateTime.UtcNow.AddMinutes(60),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            validationParameters = ValidationUtils.CreateValidationParameters();
            theoryData.Add(
                new ValidateTokenTheoryData("TokenIssuedAtInFuture")
                {
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            // Token with no expiration, but RequireExpirationTime is false, LifetimeValidator is set to skip validation
            securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            tokenValidationParameters.RequireExpirationTime = false;
            validationParameters = ValidationUtils.CreateValidationParameters();
            validationParameters.LifetimeValidator = SkipValidationDelegates.SkipLifetimeValidation;
            theoryData.Add(
                new ValidateTokenTheoryData("TokenWithoutExpiration")
                {
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            return theoryData;
        }
        #endregion

        #region ReadToken
        internal static TheoryData<ValidateTokenTheoryData> GenerateInvalidReadTokenTestCases(ITestingTokenHandler tokenHandler)
        {
            TheoryData<ValidateTokenTheoryData> theoryData = new TheoryData<ValidateTokenTheoryData>();
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            TokenValidationParameters tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters();
            theoryData.Add(
                new ValidateTokenTheoryData("TokenNull")
                {
                    ExpectedException = new ExpectedException(typeof(ArgumentNullException), "IDX10000:"),
                    ExpectedExceptionValidationParameters = new ExpectedException(typeof(ArgumentNullException), "IDX10000:"),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            theoryData.Add(
                new ValidateTokenTheoryData("EmptyToken")
                {
                    ExpectedException = new ExpectedException(typeof(ArgumentNullException), "IDX10000:"),
                    ExpectedExceptionValidationParameters = new ExpectedException(typeof(ArgumentNullException), "IDX10000:"),
                    Token = string.Empty,
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            theoryData.Add(
                new ValidateTokenTheoryData("MalformedToken")
                {
                    ExpectedException = new ExpectedException(typeof(SecurityTokenMalformedException), "IDX14100:"),
                    ExpectedExceptionValidationParameters = new ExpectedException(
                        typeof(SecurityTokenValidationException),
                        "IDX14107:",
                        typeof(SecurityTokenMalformedException)),
                    Token = "malformed_token",
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            return theoryData;
        }

        internal static TheoryData<ValidateTokenTheoryData> GenerateValidReadTokenTestCases(ITestingTokenHandler tokenHandler)
        {
            TheoryData<ValidateTokenTheoryData> theoryData = new TheoryData<ValidateTokenTheoryData>();
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            TokenValidationParameters tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters();
            theoryData.Add(
                new ValidateTokenTheoryData("ReadValidToken")
                {
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            return theoryData;
        }
        #endregion

        #region Signature
        internal static TheoryData<ValidateTokenTheoryData> GenerateInvalidSignatureTestCases(ITestingTokenHandler tokenHandler)
        {
            TheoryData<ValidateTokenTheoryData> theoryData = new TheoryData<ValidateTokenTheoryData>();

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            TokenValidationParameters tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters();

            // Signature is tampered
            theoryData.Add(
                new ValidateTokenTheoryData("TamperedSignature")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10511:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidSignatureException("IDX10520:"),
                    RelaxTVPException = true,
                    Token = tokenHandler.CreateTamperedSignature(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            // Token is not signed
            theoryData.Add(
                new ValidateTokenTheoryData("TokenIsNotSigned")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10504:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidSignatureException("IDX10504:"),
                    RelaxTVPException = true,
                    Token = tokenHandler.CreateWithoutSignature(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            // Payload has changed
            if (tokenHandler is JsonWebTestingTokenHandler)
            {
                theoryData.Add(
                new ValidateTokenTheoryData("DifferentPayload")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10511:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidSignatureException("IDX10520:"),
                    Token = tokenHandler.CreateWithTamperedPayload(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });
            }
            else
            {
                // for SAML tokens this will show up as a Reference validation error
                theoryData.Add(
                new ValidateTokenTheoryData("DifferentPayload")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10514:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidSignatureException("IDX30201:"),
                    Token = tokenHandler.CreateWithTamperedPayload(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            }

            // Header has changed
            if (tokenHandler is JsonWebTestingTokenHandler)
            {
                theoryData.Add(
                new ValidateTokenTheoryData("DifferentHeader")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10511:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidSignatureException("IDX10520:"),
                    Token = tokenHandler.CreateWithTamperedHeader(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });
            }

            // Token signed with a key that is not available, KeyId NOT present
            securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Issuer = Default.Issuer,
                SigningCredentials = Default.RsaSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            validationParameters = ValidationUtils.CreateValidationParameters();

            theoryData.Add(
                new ValidateTokenTheoryData("KeyNotAvailable")
                {
                    ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10503:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10527"),
                    RelaxTVPException = true,
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            // Token signed with a key that is not available, multiple siging signingKeys, KeyId present
            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(keys: [Default.AsymmetricSigningKey, Default.AsymmetricSigningKey]);
            validationParameters = ValidationUtils.CreateValidationParameters(signingKeys: [Default.AsymmetricSigningKey, Default.AsymmetricSigningKey]);
            ExpectedException expectedException;

            if (tokenHandler is SamlSecurityTestingTokenHandler || tokenHandler is Saml2SecurityTestingTokenHandler)
                expectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10512:");
            else
                expectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10503:");

            theoryData.Add(
                new ValidateTokenTheoryData("KeyNotAvailableMultipleSigningKeys")
                {
                    ExpectedException = expectedException,
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10527"),
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            // Token signed with a key that is not available, multiple siging signingKeys, KeyId NOT present
            securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Issuer = Default.Issuer,
                SigningCredentials = new SigningCredentials(
                    new RsaSecurityKey(KeyingMaterial.RsaParameters_2048),
                    SecurityAlgorithms.RsaSha256,
                    SecurityAlgorithms.Sha256),
                Subject = Default.ClaimsIdentityLongNames
            };

            if (tokenHandler is SamlSecurityTestingTokenHandler || tokenHandler is Saml2SecurityTestingTokenHandler)
                expectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10512:");
            else
                expectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10517:");

            theoryData.Add(
                new ValidateTokenTheoryData("KeyNotAvailableNoKid")
                {
                    ExpectedException = expectedException,
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10526:"),
                    Token = tokenHandler.CreateStringTokenNoKid(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            // Token signed with a key that is not available, multiple siging signingKeys, KeyId NOT present, TryAllSigningKeys false
            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(keys: [Default.AsymmetricSigningKey, Default.AsymmetricSigningKey]);
            tokenValidationParameters.TryAllIssuerSigningKeys = false;
            validationParameters = ValidationUtils.CreateValidationParameters(signingKeys: [Default.AsymmetricSigningKey, Default.AsymmetricSigningKey]);
            validationParameters.TryAllSigningKeys = false;

            if (tokenHandler is SamlSecurityTestingTokenHandler)
                expectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10500:");
            else
                expectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10517:");

            theoryData.Add(
                new ValidateTokenTheoryData("NoMatchingKeys_NoKid_DoNotTryAllKeys")
                {
                    ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10500:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10526:"),
                    Token = tokenHandler.CreateStringTokenNoKid(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    ValidationParameters = validationParameters,
                    TokenValidationParameters = tokenValidationParameters
                });

            // Token signed with a key that is not available, multiple siging signingKeys, KeyId NOT present, TryAllSigningKeys true
            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(keys: [Default.AsymmetricSigningKey, Default.AsymmetricSigningKey]);
            tokenValidationParameters.TryAllIssuerSigningKeys = true;
            validationParameters = ValidationUtils.CreateValidationParameters(signingKeys: [Default.AsymmetricSigningKey, Default.AsymmetricSigningKey]);
            validationParameters.TryAllSigningKeys = true;

            if (tokenHandler is SamlSecurityTestingTokenHandler)
                expectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10512:");
            else
                expectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10517:");

            theoryData.Add(
                new ValidateTokenTheoryData("NoMatchingKeys_NoKid_TryAllKeys")
                {
                    ExpectedException = expectedException,
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10523:"),
                    Token = tokenHandler.CreateStringTokenNoKid(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    ValidationParameters = validationParameters,
                    TokenValidationParameters = tokenValidationParameters
                });

            // No signingKeys available, KeyId NOT present
            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(keys: []);
            validationParameters = ValidationUtils.CreateValidationParameters(signingKeys: []);
            theoryData.Add(
                new ValidateTokenTheoryData("NoValidationKeys_NoKid_DontTryAllKeys")
                {
                    ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10500:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10526:"),
                    Token = tokenHandler.CreateStringTokenNoKid(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    ValidationParameters = validationParameters,
                    TokenValidationParameters = tokenValidationParameters
                });

            theoryData.Add(
                new ValidateTokenTheoryData("NoKeyId_NoKeys")
                {
                    ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10500:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10526:"),
                    Token = tokenHandler.CreateStringTokenNoKid(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    ValidationParameters = validationParameters,
                    TokenValidationParameters = tokenValidationParameters
                });

            //Invalid_TokenSignedWithDifferentKey_KeyIdPresent_TryAllKeysFalse
            //Invalid_TokenSignedWithDifferentKey_KeyIdPresent_TryAllKeysTrue
            // Token signed with different key, KeyId present, TryAllKeys true

            // TODO need tests with config.

            return theoryData;
        }

        internal static TheoryData<ValidateTokenTheoryData> GenerateValidSignatureTestCases(ITestingTokenHandler tokenHandler)
        {
            TheoryData<ValidateTokenTheoryData> theoryData = new TheoryData<ValidateTokenTheoryData>();

            // Token with valid signature
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            TokenValidationParameters tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters();
            theoryData.Add(
                new ValidateTokenTheoryData("ValidSignature")
                {
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(keys: []);
            tokenValidationParameters.ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(ValidationUtils.CreateDeaultOpenIdConntectConfiguration());
            validationParameters = ValidationUtils.CreateValidationParameters(signingKeys: []);
            validationParameters.ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(ValidationUtils.CreateDeaultOpenIdConntectConfiguration());
            theoryData.Add(
                new ValidateTokenTheoryData("ValidSignatureUsingConfig")
                {
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            return theoryData;
        }
        #endregion

        #region IssuerSigningKey
        internal static TheoryData<ValidateTokenTheoryData> GenerateInvalidIssuerSigningKeyTestCases(ITestingTokenHandler tokenHandler)
        {
            TheoryData<ValidateTokenTheoryData> theoryData = new TheoryData<ValidateTokenTheoryData>();

            int currentYear = DateTime.UtcNow.Year;
            // Mock time provider, 100 years in the future
            TimeProvider futureTimeProvider = new MockTimeProvider(new DateTimeOffset(currentYear + 100, 1, 1, 0, 0, 0, new(0)));
            // Mock time provider, 100 years in the past
            TimeProvider pastTimeProvider = new MockTimeProvider(new DateTimeOffset(currentYear - 100, 9, 16, 0, 0, 0, new(0)));

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            TokenValidationParameters tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(timeProvider: futureTimeProvider);
            tokenValidationParameters.ValidateLifetime = false;
            tokenValidationParameters.ValidateIssuerSigningKey = true;
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters(timeProvider: futureTimeProvider);
            validationParameters.LifetimeValidator = SkipValidationDelegates.SkipLifetimeValidation;
            theoryData.Add(
                new ValidateTokenTheoryData("IssuerSigningKeyExpired")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidSigningKeyException("IDX10249:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidSigningKeyException("IDX10249:"),
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(timeProvider: pastTimeProvider);
            tokenValidationParameters.ValidateLifetime = false;
            tokenValidationParameters.ValidateIssuerSigningKey = true;
            validationParameters = ValidationUtils.CreateValidationParameters(timeProvider: pastTimeProvider);
            validationParameters.LifetimeValidator = SkipValidationDelegates.SkipLifetimeValidation;
            theoryData.Add(
                new ValidateTokenTheoryData("IssuerSigningNotYetValid")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidSigningKeyException("IDX10248:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenInvalidSigningKeyException("IDX10248:"),
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            // TokenValidationParametersAndValidationParametersAreNull
            return theoryData;
        }

        internal static TheoryData<ValidateTokenTheoryData> GenerateValidIssuerSigningKeyTestCases(ITestingTokenHandler tokenHandler)
        {
            TheoryData<ValidateTokenTheoryData> theoryData = new TheoryData<ValidateTokenTheoryData>();
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            TokenValidationParameters tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            tokenValidationParameters.ValidateIssuerSigningKey = true;
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters();
            validationParameters.LifetimeValidator = SkipValidationDelegates.SkipLifetimeValidation;
            theoryData.Add(
                new ValidateTokenTheoryData("IssuerSigningKey")
                {
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            return theoryData;
        }
        #endregion

        #region TokenReplay
        internal static TheoryData<ValidateTokenTheoryData> GenerateInvalidTokenReplayTestCases(ITestingTokenHandler tokenHandler)
        {
            TheoryData<ValidateTokenTheoryData> theoryData = new TheoryData<ValidateTokenTheoryData>();

            // TokenReplayCache is set, but expires is null
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            var tokenReplayCache = new TokenReplayCache { OnFindReturnValue = false, OnAddReturnValue = true };
            TokenValidationParameters tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(tokenReplayCache: tokenReplayCache);
            tokenValidationParameters.ValidateLifetime = false;
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters(tokenReplayCache: tokenReplayCache);
            validationParameters.LifetimeValidator = SkipValidationDelegates.SkipLifetimeValidation;

            // TODO SAML sets an expiation time by defaault when reading conditions, we need to reason about that.
            if (tokenHandler is not SamlSecurityTestingTokenHandler)
            {
                theoryData.Add(
                    new ValidateTokenTheoryData("NoExpiration")
                    {
                        ExpectedException = ExpectedException.SecurityTokenNoExpirationException("IDX10227:"),
                        ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenReplayDetectedException("IDX10227:"),
                        Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                        TestingTokenHandler = tokenHandler,
                        TokenValidationParameters = tokenValidationParameters,
                        ValidationParameters = validationParameters,
                    });
            }
            // TokenReplayCache finds token (replay detected)
            securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow.AddMinutes(10),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            tokenReplayCache = new TokenReplayCache { OnFindReturnValue = true, OnAddReturnValue = true };
            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(tokenReplayCache: tokenReplayCache);
            validationParameters = ValidationUtils.CreateValidationParameters(tokenReplayCache: tokenReplayCache);
            theoryData.Add(
                new ValidateTokenTheoryData("TokenReplayTokenFoundInCache")
                {
                    ExpectedException = ExpectedException.SecurityTokenReplayDetectedException("IDX10228:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenReplayDetectedException("IDX10228:"),
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            // TokenReplayCache fails to add token
            tokenReplayCache = new TokenReplayCache { OnFindReturnValue = false, OnAddReturnValue = false };
            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(tokenReplayCache: tokenReplayCache);
            validationParameters = ValidationUtils.CreateValidationParameters(tokenReplayCache: tokenReplayCache);
            theoryData.Add(
                new ValidateTokenTheoryData("AddToCacheFailed")
                {
                    ExpectedException = ExpectedException.SecurityTokenReplayAddFailedException("IDX10229:"),
                    ExpectedExceptionValidationParameters = ExpectedException.SecurityTokenReplayDetectedException("IDX10229:"),
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            return theoryData;
        }

        internal static TheoryData<ValidateTokenTheoryData> GenerateValidTokenReplayTestCases(ITestingTokenHandler tokenHandler)
        {
            TheoryData<ValidateTokenTheoryData> theoryData = new TheoryData<ValidateTokenTheoryData>();

            // TokenReplayCache is set, token is not found, add succeeds
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow.AddMinutes(10),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            };

            var tokenReplayCache = new TokenReplayCache { OnFindReturnValue = false, OnAddReturnValue = true };
            TokenValidationParameters tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters(tokenReplayCache: tokenReplayCache);
            ValidationParameters validationParameters = ValidationUtils.CreateValidationParameters(tokenReplayCache: tokenReplayCache);
            theoryData.Add(
                new ValidateTokenTheoryData("TokenReplayValid")
                {
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            // No TokenReplayCache set, should succeed
            tokenValidationParameters = ValidationUtils.CreateTokenValidationParameters();
            tokenValidationParameters.TokenReplayCache = null;
            validationParameters = ValidationUtils.CreateValidationParameters();
            validationParameters.TokenReplayCache = null;

            theoryData.Add(
                new ValidateTokenTheoryData("TokenReplayNoCache")
                {
                    Token = tokenHandler.CreateStringToken(securityTokenDescriptor),
                    TestingTokenHandler = tokenHandler,
                    TokenValidationParameters = tokenValidationParameters,
                    ValidationParameters = validationParameters,
                });

            return theoryData;
        }
        #endregion
    }
}
