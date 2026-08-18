// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml;
using Microsoft.IdentityModel.Tokens.Saml2;
using Xunit;

namespace Microsoft.IdentityModel.TestUtils
{
    public class TestTheoryData
    {
        public static TheoryData<TokenReplayTheoryData> TokenReplayValidationTheoryData
        {
            get
            {
                return new TheoryData<TokenReplayTheoryData>
                {
                    new TokenReplayTheoryData
                    {
                        TestId = "ValidateTokenReplay: false, TokenReplayValidator: null",
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = $"ValidateTokenReplay: false, {nameof(ValidationDelegates.TokenReplayValidatorReturnsTrue)}",
                        TokenReplayValidator = ValidationDelegates.TokenReplayValidatorReturnsTrue
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = $"ValidateTokenReplay: false, {nameof(ValidationDelegates.TokenReplayValidatorReturnsFalse)}",
                        TokenReplayValidator = ValidationDelegates.TokenReplayValidatorReturnsFalse,
                        ExpectedException = ExpectedException.SecurityTokenReplayDetected("IDX10228:")
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = $"ValidateTokenReplay: false, {nameof(ValidationDelegates.TokenReplayValidatorThrows)}",
                        TokenReplayValidator = ValidationDelegates.TokenReplayValidatorThrows,
                        ExpectedException = ExpectedException.SecurityTokenReplayDetected("TokenReplayValidatorThrows")
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = $"ValidateTokenReplay: true, TokenReplayValidator: null",
                        ValidateTokenReplay = true
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = $"ValidateTokenReplay: true, {nameof(ValidationDelegates.TokenReplayValidatorReturnsTrue)}",
                        TokenReplayValidator = ValidationDelegates.TokenReplayValidatorReturnsTrue,
                        ValidateTokenReplay = true
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = $"ValidateTokenReplay: true, {nameof(ValidationDelegates.TokenReplayValidatorReturnsFalse)}",
                        TokenReplayValidator = ValidationDelegates.TokenReplayValidatorReturnsFalse,
                        ValidateTokenReplay = true,
                        ExpectedException = ExpectedException.SecurityTokenReplayDetected("IDX10228:")
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = $"ValidateTokenReplay: true, {nameof(ValidationDelegates.TokenReplayValidatorThrows)}",
                        TokenReplayValidator = ValidationDelegates.TokenReplayValidatorThrows,
                        ValidateTokenReplay = true,
                        ExpectedException = ExpectedException.SecurityTokenReplayDetected("TokenReplayValidatorThrows")
                    }
                };
            }
        }

        public static TheoryData<TokenReplayTheoryData> CheckParametersForTokenReplayTheoryData
        {
            get
            {
                return new TheoryData<TokenReplayTheoryData>
                {
                    new TokenReplayTheoryData
                    {
                        First = true,
                        TestId = $"ValidateTokenReplay: true, {nameof(ValidationDelegates.TokenReplayValidatorChecksExpirationTimeJwt)}",
                        SecurityToken = Default.AsymmetricJwt,
                        SecurityTokenHandler = new JwtSecurityTokenHandler(),
                        SigningKey = Default.AsymmetricSigningKey,
                        TokenReplayValidator = ValidationDelegates.TokenReplayValidatorChecksExpirationTimeJwt,
                        ValidateTokenReplay = true
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = $"ValidateTokenReplay: true, {nameof(ValidationDelegates.TokenReplayValidatorChecksExpirationTimeSaml)}",
                        SecurityToken = ReferenceTokens.SamlToken_Valid,
                        SecurityTokenHandler = new SamlSecurityTokenHandler(),
                        SigningKey = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2.Key,
                        TokenReplayValidator = ValidationDelegates.TokenReplayValidatorChecksExpirationTimeSaml,
                        ValidateTokenReplay = true,
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = $"ValidateTokenReplay: true, {nameof(ValidationDelegates.TokenReplayValidatorChecksExpirationTimeSaml2)}",
                        SecurityToken = ReferenceTokens.Saml2Token_Valid,
                        SecurityTokenHandler = new Saml2SecurityTokenHandler(),
                        SigningKey = KeyingMaterial.DefaultAADSigningKey,
                        TokenReplayValidator = ValidationDelegates.TokenReplayValidatorChecksExpirationTimeSaml2,
                        ValidateTokenReplay = true,
                    }
                };
            }
        }
    }

    public class TokenReplayTheoryData : TheoryDataBase
    {
        public TokenReplayValidator TokenReplayValidator
        {
            get;
            set;
        }
        public bool ValidateTokenReplay
        {
            get;
            set;
        }

        public string SecurityToken
        {
            get;
            set;
        }

        public SecurityTokenHandler SecurityTokenHandler
        {
            get;
            set;
        }

        public SecurityKey SigningKey
        {
            get;
            set;
        }
    }
}
