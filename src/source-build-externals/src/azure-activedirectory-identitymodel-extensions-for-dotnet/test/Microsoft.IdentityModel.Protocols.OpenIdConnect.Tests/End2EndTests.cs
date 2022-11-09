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

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class End2EndTests
    {
        [Theory, MemberData(nameof(OpenIdConnectTheoryData))]
        public void OpenIdConnect(OpenIdConnectTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.OpenIdConnect", theoryData);
            try
            {
                OpenIdConnectConfiguration configuration = OpenIdConnectConfigurationRetriever.GetAsync(theoryData.OpenIdConnectMetadataFileName, new FileDocumentRetriever(), CancellationToken.None).Result;
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken =
                    tokenHandler.CreateJwtSecurityToken(
                        configuration.Issuer,
                        Default.Audience,
                        ClaimSets.DefaultClaimsIdentity,
                        DateTime.UtcNow,
                        DateTime.UtcNow + TimeSpan.FromHours(1),
                        DateTime.UtcNow + TimeSpan.FromHours(1),
                        theoryData.SigningCredentials);

                tokenHandler.WriteToken(jwtToken);

                TokenValidationParameters validationParameters =
                        new TokenValidationParameters
                        {
                            IssuerSigningKeys = configuration.SigningKeys,
                            ValidAudience = Default.Audience,
                            ValidIssuer = configuration.Issuer,
                        };

                tokenHandler.ValidateToken(jwtToken.RawData, validationParameters, out SecurityToken _);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<OpenIdConnectTheoryData> OpenIdConnectTheoryData()
        {
            return new TheoryData<OpenIdConnectTheoryData>() {
                new OpenIdConnectTheoryData
                {
                    OpenIdConnectMetadataFileName = OpenIdConfigData.OpenIdConnectMetadataFileEnd2End,
                    SigningCredentials = new SigningCredentials(
                            KeyingMaterial.RsaSecurityKey_2048,
                            SecurityAlgorithms.RsaSha256
                        ),
                    TestId = "validRS256"
                },
                new OpenIdConnectTheoryData
                {
                    OpenIdConnectMetadataFileName = OpenIdConfigData.OpenIdConnectMetadataFileEnd2EndEC,
                    SigningCredentials = new SigningCredentials(
                            KeyingMaterial.JsonWebKeyP256,
                            SecurityAlgorithms.EcdsaSha256
                        ),
                    TestId = "validES256"
                },
                new OpenIdConnectTheoryData
                {
                    OpenIdConnectMetadataFileName = OpenIdConfigData.OpenIdConnectMetadataFileEnd2EndEC,
                    SigningCredentials = new SigningCredentials(
                            KeyingMaterial.JsonWebKeyP384,
                            SecurityAlgorithms.EcdsaSha384
                        ),
                    TestId = "validES384"
                },
                new OpenIdConnectTheoryData
                {
                    OpenIdConnectMetadataFileName = OpenIdConfigData.OpenIdConnectMetadataFileEnd2EndEC,
                    SigningCredentials = new SigningCredentials(
                            KeyingMaterial.JsonWebKeyP521,
                            SecurityAlgorithms.EcdsaSha512
                        ),
                    TestId = "validES521"
                },
                new OpenIdConnectTheoryData
                {
                    OpenIdConnectMetadataFileName = OpenIdConfigData.OpenIdConnectMetadataFileEnd2EndEC,
                    SigningCredentials = new SigningCredentials(
                            KeyingMaterial.Ecdsa384Key,
                            SecurityAlgorithms.EcdsaSha384
                        ),
                    ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException(),
                    TestId = "Ecdsa384KeyNotPartOfJWKS"
                }
            };
        }
    }

    public class OpenIdConnectTheoryData : TheoryDataBase
    {
        public string OpenIdConnectMetadataFileName { get; set; }

        public SigningCredentials SigningCredentials { get; set; }
    }
}
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
