// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Saml2;
using Microsoft.IdentityModel.Tokens.Saml2.Tests;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    /// <summary>
    /// Verifies that the SAML and SAML2 handlers invoke <c>ValidateTokenReplay</c>
    /// uniformly, regardless of whether the assertion contains a <c>Conditions</c> element.
    /// </summary>
    public class SamlConditionsHandlingTests
    {
        // ———————————————————— SAML 2.0 ————————————————————

        [Theory, MemberData(nameof(Saml2ConditionsHandlingTheoryData), DisableDiscoveryEnumeration = true)]
        public void Saml2ConditionsHandling(Saml2TheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.Saml2ConditionsHandling", theoryData);
            try
            {
                theoryData.Handler.ValidateToken(theoryData.Token, theoryData.ValidationParameters, out SecurityToken validatedToken);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> Saml2ConditionsHandlingTheoryData
        {
            get
            {
                return new TheoryData<Saml2TheoryData>
                {
                    // Conditions absent + replay cache configured: replay validation
                    // still runs and surfaces the missing expiration.
                    new Saml2TheoryData("Saml2_NoConditions_WithReplayCache_ThrowsNoExpiration")
                    {
                        Token = ReferenceTokens.Saml2Token_NoConditions_NoSignature,
                        ExpectedException = ExpectedException.SecurityTokenNoExpirationException("IDX10227:"),
                        ValidationParameters = new TokenValidationParameters
                        {
                            RequireSignedTokens = false,
                            RequireAudience = false,
                            ValidateLifetime = false,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateTokenReplay = true,
                            TokenReplayCache = new TokenReplayCache
                            {
                                OnAddReturnValue = true,
                                OnFindReturnValue = false
                            }
                        }
                    },
                    // Token with Conditions and a replay cache continues to validate normally.
                    new Saml2TheoryData("Saml2_WithConditions_WithReplayCache_Succeeds")
                    {
                        Token = ReferenceTokens.Saml2Token_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            ValidateTokenReplay = true,
                            TokenReplayCache = new TokenReplayCache
                            {
                                OnAddReturnValue = true,
                                OnFindReturnValue = false
                            }
                        }
                    },
                    // No Conditions and no replay cache: validation succeeds because
                    // replay enforcement is not requested.
                    new Saml2TheoryData("Saml2_NoConditions_NoReplayCache_Succeeds")
                    {
                        Token = ReferenceTokens.Saml2Token_NoConditions_NoSignature,
                        ValidationParameters = new TokenValidationParameters
                        {
                            RequireSignedTokens = false,
                            RequireAudience = false,
                            ValidateLifetime = false,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                        }
                    },
                    // Conditions element present but NotOnOrAfter is unset — replay validation
                    // still runs and surfaces the missing expiration.
                    new Saml2TheoryData("Saml2_ConditionsNoExpiration_WithReplayCache_ThrowsNoExpiration")
                    {
                        Token = ReferenceTokens.Saml2Token_ConditionsNoExpiration_NoSignature,
                        ExpectedException = ExpectedException.SecurityTokenNoExpirationException("IDX10227:"),
                        ValidationParameters = new TokenValidationParameters
                        {
                            RequireSignedTokens = false,
                            RequireAudience = false,
                            ValidateLifetime = false,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateTokenReplay = true,
                            TokenReplayCache = new TokenReplayCache
                            {
                                OnAddReturnValue = true,
                                OnFindReturnValue = false
                            }
                        }
                    },
                };
            }
        }

        // ———————————————————— SAML 1.x ————————————————————

        [Theory, MemberData(nameof(SamlConditionsHandlingTheoryData), DisableDiscoveryEnumeration = true)]
        public void SamlConditionsHandling(SamlTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.SamlConditionsHandling", theoryData);
            try
            {
                (theoryData.Handler as SamlSecurityTokenHandler).ValidateToken(theoryData.Token, theoryData.ValidationParameters, out SecurityToken validatedToken);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> SamlConditionsHandlingTheoryData
        {
            get
            {
                return new TheoryData<SamlTheoryData>
                {
                    // Conditions absent + replay cache configured: replay validation
                    // still runs and surfaces the missing expiration.
                    new SamlTheoryData("Saml_NoConditions_WithReplayCache_ThrowsNoExpiration")
                    {
                        Token = ReferenceTokens.SamlToken_NoConditions_NoSignature,
                        ExpectedException = ExpectedException.SecurityTokenNoExpirationException("IDX10227:"),
                        ValidationParameters = new TokenValidationParameters
                        {
                            RequireSignedTokens = false,
                            RequireAudience = false,
                            ValidateLifetime = false,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateTokenReplay = true,
                            TokenReplayCache = new TokenReplayCache
                            {
                                OnAddReturnValue = true,
                                OnFindReturnValue = false
                            }
                        }
                    },
                    // Token with Conditions and a replay cache continues to validate normally.
                    new SamlTheoryData("Saml_WithConditions_WithReplayCache_Succeeds")
                    {
                        Token = ReferenceTokens.SamlToken_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2.Key,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            ValidateTokenReplay = true,
                            TokenReplayCache = new TokenReplayCache
                            {
                                OnAddReturnValue = true,
                                OnFindReturnValue = false
                            }
                        }
                    },
                    // No Conditions and no replay cache: validation succeeds because
                    // replay enforcement is not requested.
                    new SamlTheoryData("Saml_NoConditions_NoReplayCache_Succeeds")
                    {
                        Token = ReferenceTokens.SamlToken_NoConditions_NoSignature,
                        ValidationParameters = new TokenValidationParameters
                        {
                            RequireSignedTokens = false,
                            RequireAudience = false,
                            ValidateLifetime = false,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                        }
                    },
                    // Conditions element present but NotOnOrAfter is unset. SAML 1.x
                    // defaults to DateTime.MaxValue here, so replay validation succeeds
                    // with a far-future expiration.
                    new SamlTheoryData("Saml_ConditionsNoExpiration_WithReplayCache_Succeeds")
                    {
                        Token = ReferenceTokens.SamlToken_ConditionsNoExpiration_NoSignature,
                        ValidationParameters = new TokenValidationParameters
                        {
                            RequireSignedTokens = false,
                            RequireAudience = false,
                            ValidateLifetime = false,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateTokenReplay = true,
                            TokenReplayCache = new TokenReplayCache
                            {
                                OnAddReturnValue = true,
                                OnFindReturnValue = false
                            }
                        }
                    },
                };
            }
        }
    }
}
