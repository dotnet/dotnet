// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

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
}
