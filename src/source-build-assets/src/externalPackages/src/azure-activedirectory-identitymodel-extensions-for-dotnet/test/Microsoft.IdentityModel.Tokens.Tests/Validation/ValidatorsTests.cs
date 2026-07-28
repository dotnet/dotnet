// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class ValidatorsTests
    {
        [Theory, MemberData(nameof(ValidateAudienceParametersTheoryData), DisableDiscoveryEnumeration = true)]
        public void ValidateAudienceParameters(AudienceValidationTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateAudienceParameters", theoryData);
            try
            {
                Validators.ValidateAudience(theoryData.Audiences, theoryData.SecurityToken, theoryData.TokenValidationParameters);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AudienceValidationTheoryData> ValidateAudienceParametersTheoryData
        {
            get
            {
                return new TheoryData<AudienceValidationTheoryData>
                {
                    new AudienceValidationTheoryData("TokenValidationParametersNull")
                    {
                        Audiences = new List<string> { "audience1" },
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        TokenValidationParameters = null
                    },
                    new AudienceValidationTheoryData("AudiencesEmptyString")
                    {
                        Audiences = new List<string> { "" },
                        ExpectedException =  ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudience = "audience"}
                    },
                    new AudienceValidationTheoryData("AudiencesWhiteSpace")
                    {
                        Audiences = new List<string> { "    " },
                        ExpectedException =  ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudience = "audience"}
                    },
                    new AudienceValidationTheoryData("AudiencesNull")
                    {
                        Audiences = null,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10207:"),
                    },
                    new AudienceValidationTheoryData("AudiencesEmptyList")
                    {
                        Audiences = new List<string>{ },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10206:"),
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudience = "audience"}
                    },
                    new AudienceValidationTheoryData("ValidateAudienceFalseAudiencesEmptyList")
                    {
                        Audiences = new List<string>{ },
                        TokenValidationParameters = new TokenValidationParameters{ ValidateAudience = false }
                    },
                    new AudienceValidationTheoryData("ValidateAudienceFalseAudiencesNull")
                    {
                        Audiences = null,
                        TokenValidationParameters = new TokenValidationParameters{ ValidateAudience = false }
                    },
                    new AudienceValidationTheoryData("ValidAudienceEmptyString")
                    {
                        Audiences = new List<string> { "audience1" },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10208:"),
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudience = "" }
                    },
                    new AudienceValidationTheoryData("ValidAudienceWhiteSpace")
                    {
                        Audiences = new List<string> { "audience1" },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10208:"),
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudience = "    " }
                    },
                    new AudienceValidationTheoryData("ValidAudiencesEmptyString")
                    {
                        Audiences = new List<string> { "audience1" },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudiences = new List<string>{ "" } }
                    },
                    new AudienceValidationTheoryData("ValidAudiencesWhiteSpace")
                    {
                        Audiences = new List<string> { "audience1" },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudiences = new List<string>{ "    " } }
                    },
                    new AudienceValidationTheoryData("ValidateAudienceTrueValidAudienceAndValidAudiencesNull")
                    {
                        Audiences = new List<string> { "audience1" },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10208:"),
                    },
                    new AudienceValidationTheoryData("AudiencesEmpty_RequireAudienceFalse_NoException")
                    {
                        Audiences = new List<string> { },
                        TokenValidationParameters = new TokenValidationParameters{
                            RequireAudience = false
                            // default value of TVP.RequireAudience is true.
                        }
                    },
                    new AudienceValidationTheoryData("ValidAudience_RequireAudienceFalse_NoException")
                    {
                        Audiences = new List<string> { "audience" },
                        TokenValidationParameters = new TokenValidationParameters{
                            ValidAudience = "audience",
                            RequireAudience = false
                        }
                    },
                    new AudienceValidationTheoryData("InvalidAudience_RequireAudienceFalse_ExceptionThrown")
                    {
                        Audiences = new List<string> { "audience1" },
                        ExpectedException =  ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{
                            ValidAudience = "audience",
                            RequireAudience = false
                        }
                    }
                };
            }
        }

        [Theory, MemberData(nameof(ValidateAudienceTheoryData), DisableDiscoveryEnumeration = true)]
        public void ValidateAudience(AudienceValidationTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateAudience", theoryData);
            try
            {
                Validators.ValidateAudience(theoryData.Audiences, theoryData.SecurityToken, theoryData.TokenValidationParameters);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AudienceValidationTheoryData> ValidateAudienceTheoryData
        {
            get
            {
                var audience1 = "http://audience1.com";
                var audience2 = "http://audience2.com";
                List<string> audiences1 = new List<string> { "", audience1 };
                List<string> audiences1WithSlash = new List<string> { "", audience1 + "/" };
                List<string> audiences1WithTwoSlashes = new List<string> { "", audience1 + "//" };
                List<string> audiences2 = new List<string> { "", audience2 };
                List<string> audiences2WithSlash = new List<string> { "", audience2 + "/" };

                return new TheoryData<AudienceValidationTheoryData>
                {
                    new AudienceValidationTheoryData("SameLengthMatched")
                    {
                        Audiences = audiences1,
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 }
                    },
                    new AudienceValidationTheoryData("SameLengthNotMatched")
                    {
                        Audiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudience = audience2 }
                    },
                    new AudienceValidationTheoryData("NoMatchTVPValidateFalse")
                    {
                        Audiences = audiences1,
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudience = audience2, ValidateAudience = false }
                    },
                    new AudienceValidationTheoryData("AudiencesValidAudienceWithSlashNotMatched")
                    {
                        Audiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudience = audience2 + "/" }
                    },
                    new AudienceValidationTheoryData("AudiencesWithSlashValidAudienceSameLengthNotMatched")
                    {
                        Audiences = audiences2WithSlash,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 }
                    },
                    new AudienceValidationTheoryData("ValidAudienceWithSlashTVPFalse")
                    {
                        Audiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{ IgnoreTrailingSlashWhenValidatingAudience = false, ValidAudience = audience1 + "/" }
                    },
                    new AudienceValidationTheoryData("ValidAudienceWithSlashTVPTrue")
                    {
                        Audiences = audiences1,
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 + "/" }
                    },
                    new AudienceValidationTheoryData("ValidAudiencesWithSlashTVPFalse")
                    {
                        Audiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{ IgnoreTrailingSlashWhenValidatingAudience = false, ValidAudiences = audiences1WithSlash }
                    },
                    new AudienceValidationTheoryData("ValidAudiencesWithSlashTVPTrue")
                    {
                        Audiences = audiences1,
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudiences = audiences1WithSlash }
                    },
                    new AudienceValidationTheoryData("ValidAudienceWithExtraChar")
                    {
                        Audiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 + "A" }
                    },
                    new AudienceValidationTheoryData("ValidAudienceWithDoubleSlashTVPTrue")
                    {
                        Audiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 + "//" }
                    },
                    new AudienceValidationTheoryData("ValidAudiencesWithDoubleSlashTVPTrue")
                    {
                        Audiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudiences = audiences1WithTwoSlashes }
                    },
                    new AudienceValidationTheoryData("TokenAudienceWithSlashTVPFalse")
                    {
                        Audiences = audiences1WithSlash,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{ IgnoreTrailingSlashWhenValidatingAudience = false, ValidAudience = audience1 }
                    },
                    new AudienceValidationTheoryData("TokenAudienceWithSlashTVPTrue")
                    {
                        Audiences = audiences1WithSlash,
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 }
                    },
                    new AudienceValidationTheoryData("TokenAudienceWithSlashNotEqual")
                    {
                        Audiences = audiences2WithSlash,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 },
                    },
                    new AudienceValidationTheoryData("TokenAudiencesWithSlashTVPFalse")
                    {
                        Audiences = audiences1WithSlash,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{ IgnoreTrailingSlashWhenValidatingAudience = false, ValidAudience = audience1 }
                    },
                    new AudienceValidationTheoryData("TokenAudiencesWithSlashTVPTrue")
                    {
                        Audiences = audiences1WithSlash,
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 }
                    },
                    new AudienceValidationTheoryData("TokenAudiencesWithSlashValidAudiencesNotMatchedTVPTrue")
                    {
                        Audiences = audiences1WithSlash,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudiences = audiences2 }
                    },
                    new AudienceValidationTheoryData("TokenAudienceWithTwoSlashesTVPTrue")
                    {
                        Audiences = audiences1WithTwoSlashes,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 }
                    },
                    new AudienceValidationTheoryData("ValidAudiencesList_OptimizedPath")
                    {
                        Audiences = new List<string> { "audience1" },
                        TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidAudiences = new List<string> { "invalidAudience", "audience1" }  // Using List<string> to test IList optimization
                        }
                    },
                    new AudienceValidationTheoryData("ValidAudiencesList_EmptyList_OptimizedPath")
                    {
                        Audiences = new List<string> { "audience1" },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidAudiences = new List<string>()  // Empty list should still return false through optimized path
                        }
                    }
                };
            }
        }

        [Theory, MemberData(nameof(IssuerDataSet), DisableDiscoveryEnumeration = true)]
        public void Issuer(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters, BaseConfiguration configuration, ExpectedException ee)
        {
            try
            {
                Validators.ValidateIssuer(issuer, securityToken, validationParameters, configuration);
                ee.ProcessNoException();
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        public static TheoryData<string, SecurityToken, TokenValidationParameters, BaseConfiguration, ExpectedException> IssuerDataSet
        {
            get
            {
                List<string> issuers = new List<string> { null, "", Default.Issuer };
                List<string> invalidIssuers = new List<string> { "", NotDefault.Issuer };
                Dictionary<string, object> properties = new Dictionary<string, object> { { "InvalidIssuer", Default.Issuer } };

                var dataset = new TheoryData<string, SecurityToken, TokenValidationParameters, BaseConfiguration, ExpectedException>();

                dataset.Add(null, null, null, null, ExpectedException.ArgumentNullException());
                dataset.Add(null, null, new TokenValidationParameters { ValidateIssuer = false }, null, ExpectedException.NoExceptionExpected);
                dataset.Add(null, null, new TokenValidationParameters(), null, ExpectedException.SecurityTokenInvalidIssuerException("IDX10211:", propertiesExpected: new Dictionary<string, object> { { "InvalidIssuer", null } }));
                dataset.Add(Default.Issuer, null, new TokenValidationParameters(), null, ExpectedException.SecurityTokenInvalidIssuerException("IDX10204:", propertiesExpected: properties));
                dataset.Add(Default.Issuer, null, new TokenValidationParameters { ValidIssuer = NotDefault.Issuer }, null, ExpectedException.SecurityTokenInvalidIssuerException("IDX10205:", propertiesExpected: properties));
                dataset.Add(Default.Issuer, null, new TokenValidationParameters { ValidIssuers = invalidIssuers }, null, ExpectedException.SecurityTokenInvalidIssuerException("IDX10205:", propertiesExpected: properties));
                dataset.Add(Default.Issuer, null, new TokenValidationParameters { ValidIssuer = Default.Issuer }, null, ExpectedException.NoExceptionExpected);
                dataset.Add(Default.Issuer, null, new TokenValidationParameters { ValidIssuers = issuers }, null, ExpectedException.NoExceptionExpected);
                dataset.Add(Default.Issuer, null, new TokenValidationParameters { ValidIssuer = NotDefault.Issuer }, new OpenIdConnectConfiguration { Issuer = Default.Issuer }, ExpectedException.NoExceptionExpected);
                dataset.Add(Default.Issuer, null, new TokenValidationParameters { ValidIssuers = invalidIssuers }, new OpenIdConnectConfiguration { Issuer = Default.Issuer }, ExpectedException.NoExceptionExpected);

                return dataset;
            }
        }

        [Theory, MemberData(nameof(LifeTimeDataSet), DisableDiscoveryEnumeration = true)]
        public void Lifetime(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters, ExpectedException ee)
        {
            try
            {
                Validators.ValidateLifetime(notBefore, expires, securityToken, validationParameters);
                ee.ProcessNoException();
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        public static TheoryData<DateTime?, DateTime?, SecurityToken, TokenValidationParameters, ExpectedException> LifeTimeDataSet
        {
            get
            {
                List<string> issuers = new List<string> { "", Default.Issuer };
                List<string> invalidIssuers = new List<string> { "", NotDefault.Issuer };
                DateTime? notBefore;
                DateTime? expires;

                //notbefore  expires    
                var dataset = new TheoryData<DateTime?, DateTime?, SecurityToken, TokenValidationParameters, ExpectedException>();

                dataset.Add(null, null, null, null, ExpectedException.ArgumentNullException());
                dataset.Add(null, null, null, new TokenValidationParameters { ValidateLifetime = false }, ExpectedException.NoExceptionExpected);
                dataset.Add(null, null, null, new TokenValidationParameters(), ExpectedException.SecurityTokenNoExpirationException("IDX10225:"));

                notBefore = EpochTime.DateTime(EpochTime.GetIntDate((DateTime.UtcNow + TimeSpan.FromHours(1)).ToUniversalTime()));
                expires = EpochTime.DateTime(EpochTime.GetIntDate(DateTime.UtcNow.ToUniversalTime()));
                dataset.Add(notBefore, expires, null, new TokenValidationParameters(), ExpectedException.SecurityTokenInvalidLifetimeException("IDX10224:", propertiesExpected: new Dictionary<string, object> { { "NotBefore", notBefore }, { "Expires", expires } }));

                notBefore = EpochTime.DateTime(EpochTime.GetIntDate((DateTime.UtcNow + TimeSpan.FromHours(1)).ToUniversalTime()));
                expires = EpochTime.DateTime(EpochTime.GetIntDate((DateTime.UtcNow + TimeSpan.FromHours(2)).ToUniversalTime()));
                dataset.Add(notBefore, expires, null, new TokenValidationParameters(), ExpectedException.SecurityTokenNotYetValidException("IDX10222:", propertiesExpected: new Dictionary<string, object> { { "NotBefore", notBefore } }));

                dataset.Add(DateTime.UtcNow - TimeSpan.FromHours(2), DateTime.UtcNow - TimeSpan.FromHours(1), null, new TokenValidationParameters(), ExpectedException.SecurityTokenExpiredException("IDX10223:"));
                dataset.Add(DateTime.UtcNow - TimeSpan.FromHours(2), DateTime.UtcNow + TimeSpan.FromHours(1), null, new TokenValidationParameters(), ExpectedException.NoExceptionExpected);

                // clock skew, positive then negative
                dataset.Add(DateTime.UtcNow + TimeSpan.FromMinutes(2), DateTime.UtcNow + TimeSpan.FromHours(1), null, new TokenValidationParameters { ClockSkew = TimeSpan.FromMinutes(5) }, ExpectedException.NoExceptionExpected);
                dataset.Add(DateTime.UtcNow - TimeSpan.FromMinutes(2), DateTime.UtcNow - TimeSpan.FromMinutes(1), null, new TokenValidationParameters { ClockSkew = TimeSpan.FromMinutes(5) }, ExpectedException.NoExceptionExpected);

                notBefore = EpochTime.DateTime(EpochTime.GetIntDate((DateTime.UtcNow + TimeSpan.FromMinutes(6)).ToUniversalTime()));
                expires = EpochTime.DateTime(EpochTime.GetIntDate((DateTime.UtcNow + TimeSpan.FromHours(1)).ToUniversalTime()));
                dataset.Add(notBefore, expires, null, new TokenValidationParameters { ClockSkew = TimeSpan.FromMinutes(5) }, ExpectedException.SecurityTokenNotYetValidException("IDX10222:", propertiesExpected: new Dictionary<string, object> { { "NotBefore", notBefore } }));

                dataset.Add(DateTime.UtcNow - TimeSpan.FromHours(2), DateTime.UtcNow - TimeSpan.FromMinutes(6), null, new TokenValidationParameters { ClockSkew = TimeSpan.FromMinutes(5) }, ExpectedException.SecurityTokenExpiredException("IDX10223:"));

                return dataset;
            }
        }

        [Theory, MemberData(nameof(SecurityKeyDataSet), DisableDiscoveryEnumeration = true)]
        public void SecurityKey(SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters, ExpectedException ee)
        {
            try
            {
                Validators.ValidateIssuerSecurityKey(securityKey, securityToken, validationParameters);
                ee.ProcessNoException();
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        public static TheoryData<SecurityKey, SecurityToken, TokenValidationParameters, ExpectedException> SecurityKeyDataSet
        {
            get
            {
                var dataset = new TheoryData<SecurityKey, SecurityToken, TokenValidationParameters, ExpectedException>();

                dataset.Add(null, new JwtSecurityToken(), new TokenValidationParameters { ValidateIssuerSigningKey = true }, ExpectedException.ArgumentNullException());
                dataset.Add(KeyingMaterial.SymmetricSecurityKey2_256, null, new TokenValidationParameters { ValidateIssuerSigningKey = true }, ExpectedException.ArgumentNullException());
                dataset.Add(KeyingMaterial.SymmetricSecurityKey2_256, new JwtSecurityToken(), null, ExpectedException.ArgumentNullException());
                dataset.Add(KeyingMaterial.SymmetricSecurityKey2_256, new JwtSecurityToken(), new TokenValidationParameters { ValidateIssuerSigningKey = true }, ExpectedException.NoExceptionExpected);
                dataset.Add(null, new JwtSecurityToken(), new TokenValidationParameters { ValidateIssuerSigningKey = false }, ExpectedException.NoExceptionExpected);
                dataset.Add(KeyingMaterial.SymmetricSecurityKey2_256, null, new TokenValidationParameters { ValidateIssuerSigningKey = false }, ExpectedException.NoExceptionExpected);
                dataset.Add(KeyingMaterial.ExpiredX509SecurityKey_Public, new JwtSecurityToken(), new TokenValidationParameters { ValidateIssuerSigningKey = true }, ExpectedException.SecurityTokenInvalidSigningKeyException(substringExpected: "IDX10249:"));
                dataset.Add(KeyingMaterial.NotYetValidX509SecurityKey_Public, new JwtSecurityToken(), new TokenValidationParameters { ValidateIssuerSigningKey = true }, ExpectedException.SecurityTokenInvalidSigningKeyException(substringExpected: "IDX10248:"));
                dataset.Add(KeyingMaterial.SymmetricSecurityKey2_256, new JwtSecurityToken(), new TokenValidationParameters { ValidateIssuerSigningKey = true, RequireSignedTokens = true }, ExpectedException.NoExceptionExpected);
                dataset.Add(KeyingMaterial.SymmetricSecurityKey2_256, new JwtSecurityToken(), new TokenValidationParameters { ValidateIssuerSigningKey = true, RequireSignedTokens = false }, ExpectedException.NoExceptionExpected);
                dataset.Add(null, new JwtSecurityToken(), new TokenValidationParameters { ValidateIssuerSigningKey = true, RequireSignedTokens = true }, ExpectedException.ArgumentNullException("IDX10253: "));
                dataset.Add(null, new JwtSecurityToken(), new TokenValidationParameters { ValidateIssuerSigningKey = true, RequireSignedTokens = false }, ExpectedException.NoExceptionExpected);
                return dataset;
            }
        }

        [Theory, MemberData(nameof(TokenReplayDataSet), DisableDiscoveryEnumeration = true)]
        public void TokenReplay(string securityToken, DateTime? expirationTime, TokenValidationParameters validationParameters, ExpectedException ee)
        {
            try
            {
                Validators.ValidateTokenReplay(expirationTime, securityToken, validationParameters);
                ee.ProcessNoException();
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        public static TheoryData<string, DateTime?, TokenValidationParameters, ExpectedException> TokenReplayDataSet
        {
            get
            {
                var dataset = new TheoryData<string, DateTime?, TokenValidationParameters, ExpectedException>();

                dataset.Add(null, null, new TokenValidationParameters { ValidateTokenReplay = true }, ExpectedException.ArgumentNullException());
                dataset.Add(string.Empty, null, new TokenValidationParameters { ValidateTokenReplay = true }, ExpectedException.ArgumentNullException());
                dataset.Add("token", DateTime.UtcNow + TimeSpan.FromDays(1), null, ExpectedException.ArgumentNullException());
                dataset.Add("token", null, new TokenValidationParameters { ValidateTokenReplay = true, TokenReplayCache = new TokenReplayCache { AddRetVal = true, FindRetVal = true } }, ExpectedException.SecurityTokenNoExpirationException());
                dataset.Add("token", DateTime.UtcNow + TimeSpan.FromDays(1), new TokenValidationParameters { ValidateTokenReplay = true, TokenReplayCache = new TokenReplayCache { AddRetVal = true, FindRetVal = true } }, ExpectedException.SecurityTokenReplayDetected("IDX10228:"));
                dataset.Add("token", DateTime.UtcNow + TimeSpan.FromDays(1), new TokenValidationParameters { ValidateTokenReplay = true, TokenReplayCache = new TokenReplayCache { AddRetVal = false, FindRetVal = false } }, ExpectedException.SecurityTokenReplayAddFailed("IDX10229:"));
                dataset.Add("token", DateTime.UtcNow + TimeSpan.FromDays(1), new TokenValidationParameters { ValidateTokenReplay = true, TokenReplayCache = new TokenReplayCache { AddRetVal = true, FindRetVal = false } }, ExpectedException.NoExceptionExpected);

                return dataset;
            }
        }

        // Each TokenReplayValidator in this test checks that the expiration parameter passed into it is equal to the expiration time of the token.
        // If they're not equal, the test will fail.
        [Theory, MemberData(nameof(CheckParametersForTokenReplayTheoryData), DisableDiscoveryEnumeration = true)]
        public void CheckParametersForTokenReplay(TokenReplayTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.CheckParametersForTokenReplay", theoryData);
            var context = new CompareContext($"{this}.CheckParametersForTokenReplay, {theoryData.TestId}");
            var tvp = new TokenValidationParameters();
            tvp.IssuerSigningKey = theoryData.SigningKey;
            tvp.TokenReplayValidator = theoryData.TokenReplayValidator;
            tvp.ValidateTokenReplay = theoryData.ValidateTokenReplay;
            tvp.ValidateAudience = false;
            tvp.ValidateIssuer = false;
            tvp.ValidateLifetime = false;
            var token = theoryData.SecurityToken;
            var tokenValidator = theoryData.SecurityTokenHandler;

            try
            {
                // TokenReplayValidator should always be provided for these tests.
                tokenValidator.ValidateToken(token, tvp, out SecurityToken validatedToken);
                theoryData.ExpectedException.ProcessNoException(context.Diffs);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context.Diffs);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<TokenReplayTheoryData> CheckParametersForTokenReplayTheoryData
        {
            get
            {
                return TestTheoryData.CheckParametersForTokenReplayTheoryData;
            }
        }

        class TokenReplayCache : ITokenReplayCache
        {
            public bool AddRetVal { get; set; }

            public bool FindRetVal { get; set; }

            public bool TryAdd(string securityToken, DateTime expiresOn)
            {
                return AddRetVal;
            }

            public bool TryFind(string securityToken)
            {
                return FindRetVal;
            }
        }

        public class AudienceValidationTheoryData : TheoryDataBase
        {
            public AudienceValidationTheoryData(string testId) : base(testId)
            { }

            public List<string> Audiences { get; set; }

            public SecurityToken SecurityToken { get; set; }

            public TokenValidationParameters TokenValidationParameters { get; set; } = new TokenValidationParameters();
        }
    }
}
