//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

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
        } = null;
        public bool ValidateTokenReplay
        {
            get;
            set;
        } = false;

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
