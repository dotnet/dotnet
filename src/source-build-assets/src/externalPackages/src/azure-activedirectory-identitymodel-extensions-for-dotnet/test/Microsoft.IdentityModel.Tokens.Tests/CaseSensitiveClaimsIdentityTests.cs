// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Newtonsoft.Json.Linq;
using Xunit;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class CaseSensitiveClaimsIdentityTests
    {
        private static readonly string LowerCaseClaimName = "tid";
        private static readonly string LowerCaseClaimValue = "tenant";
        private static readonly string UpperCaseClaimName = "TID";
        private static readonly string UpperCaseClaimValue = "TENANT";
        private static readonly JObject _defaultPayload = new()
        {
            [JwtRegisteredClaimNames.Iss] = Default.Issuer,
            [JwtRegisteredClaimNames.Aud] = Default.Audience,
        };

        [Theory, MemberData(nameof(GetCaseSensitiveClaimsIdentityTheoryData), DisableDiscoveryEnumeration = true)]
        public void FindAll_DoesCaseSensitiveSearch(CaseSensitiveClaimsIdentityTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.FindAll_DoesCaseSensitiveSearch", theoryData);

            try
            {
                var actualClaims = theoryData.ClaimsIdentity.FindAll(theoryData.ClaimNameSearch).Select(claim => claim.Type);

                IdentityComparer.AreEqual(theoryData.ExpectedClaims, actualClaims, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(GetCaseSensitiveClaimsIdentityTheoryData), DisableDiscoveryEnumeration = true)]
        public void FindFirst_DoesCaseSensitiveSearch(CaseSensitiveClaimsIdentityTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.FindFirst_DoesCaseSensitiveSearch", theoryData);

            try
            {
                var actualClaim = theoryData.ClaimsIdentity.FindFirst(theoryData.ClaimNameSearch)?.Type;

                IdentityComparer.AreEqual(theoryData.ExpectedClaim, actualClaim, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(GetCaseSensitiveClaimsIdentityTheoryData), DisableDiscoveryEnumeration = true)]
        public void HasClaim_DoesCaseSensitiveSearch(CaseSensitiveClaimsIdentityTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.HasClaim_DoesCaseSensitiveSearch", theoryData);

            try
            {
                var actualHasClaim = theoryData.ClaimsIdentity.HasClaim(theoryData.ClaimNameSearch, theoryData.ClaimValueSearch);

                IdentityComparer.AreEqual(theoryData.ExpectedHasClaim, actualHasClaim, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void AddClaim_AddsClaim()
        {
            var claimsIdentity = new CaseSensitiveClaimsIdentity();

            Assert.Empty(claimsIdentity.Claims);

            var claim = new Claim("claimType", "claimValue");
            claimsIdentity.AddClaim(claim);

            Assert.NotEmpty(claimsIdentity.Claims);
        }

        [Fact]
        public void AddClaim_AddNull_ThrowsException()
        {
            var claimsIdentity = new CaseSensitiveClaimsIdentity();

            Assert.Throws<ArgumentNullException>(() => claimsIdentity.AddClaim(null));
        }

        [Fact]
        public void RemoveClaim_RemovesClaim()
        {
            var claimsIdentity = new CaseSensitiveClaimsIdentity();
            var claim = new Claim("claimType", "claimValue");
            claimsIdentity.AddClaim(claim);

            Assert.NotEmpty(claimsIdentity.Claims);

            var claimToRemove = claimsIdentity.Claims.First();

            claimsIdentity.RemoveClaim(claimToRemove);

            Assert.Empty(claimsIdentity.Claims);
        }

        [Fact]
        public void DefaultProperties_CorrectlySet()
        {
            var validationParameters = new TokenValidationParameters()
            {
                NameClaimType = "tvp_name",
                RoleClaimType = "tvp_role",
            };
            var claimsIdentity = CreateCaseSensitiveClaimsIdentity(new JObject(), validationParameters);

            Assert.Equal(validationParameters.NameClaimType, claimsIdentity.NameClaimType);
            Assert.Equal(validationParameters.RoleClaimType, claimsIdentity.RoleClaimType);
        }

        [Fact]
        public void Clone_ReturnsCaseSensitiveClaimsIdentity()
        {
            var claimsIdentity = new CaseSensitiveClaimsIdentity();
            var clone = claimsIdentity.Clone();

            Assert.True(clone is CaseSensitiveClaimsIdentity);
        }

        public static TheoryData<CaseSensitiveClaimsIdentityTheoryData> GetCaseSensitiveClaimsIdentityTheoryData
        {
            get
            {
                return new TheoryData<CaseSensitiveClaimsIdentityTheoryData>
                {
                    new CaseSensitiveClaimsIdentityTheoryData("UppercaseSearch_ClaimsExist")
                    {
                        ClaimsIdentity = CreateCaseSensitiveClaimsIdentity(new JObject {
                                [LowerCaseClaimName] = LowerCaseClaimValue,
                                [UpperCaseClaimName] = UpperCaseClaimValue,
                            }),
                        ClaimNameSearch = UpperCaseClaimName,
                        ClaimValueSearch = UpperCaseClaimValue,
                        ExpectedHasClaim = true,
                        ExpectedClaim = UpperCaseClaimName,
                        ExpectedClaims = [UpperCaseClaimName],
                    },
                    new CaseSensitiveClaimsIdentityTheoryData("LowercaseSearch_ClaimsExist")
                    {
                         ClaimsIdentity = CreateCaseSensitiveClaimsIdentity(new JObject {
                                [UpperCaseClaimName] = UpperCaseClaimValue,
                                [LowerCaseClaimName] = LowerCaseClaimValue,
                            }),
                        ClaimNameSearch = LowerCaseClaimName,
                        ClaimValueSearch = LowerCaseClaimValue,
                        ExpectedHasClaim = true,
                        ExpectedClaim = LowerCaseClaimName,
                        ExpectedClaims = [LowerCaseClaimName],
                    },
                    new CaseSensitiveClaimsIdentityTheoryData("UppercaseSearch_ClaimsMissing")
                    {
                        ClaimsIdentity = CreateCaseSensitiveClaimsIdentity(new JObject {
                                [LowerCaseClaimName] = LowerCaseClaimValue,
                            }),
                        ClaimNameSearch = UpperCaseClaimName,
                        ClaimValueSearch = UpperCaseClaimValue,
                        ExpectedHasClaim = false,
                        ExpectedClaim = null,
                        ExpectedClaims = [],
                    },
                    new CaseSensitiveClaimsIdentityTheoryData("LowercaseSearch_ClaimsMissing")
                    {
                         ClaimsIdentity = CreateCaseSensitiveClaimsIdentity(new JObject {
                                [UpperCaseClaimName] = UpperCaseClaimValue,
                            }),
                        ClaimNameSearch = LowerCaseClaimName,
                        ClaimValueSearch = LowerCaseClaimValue,
                        ExpectedHasClaim = false,
                        ExpectedClaim = null,
                        ExpectedClaims = [],
                    },
                    new CaseSensitiveClaimsIdentityTheoryData("UppercaseMixedSearch_ClaimsMissing")
                    {
                        ClaimsIdentity = CreateCaseSensitiveClaimsIdentity(new JObject {
                                [LowerCaseClaimName] = UpperCaseClaimValue,
                            }),
                        ClaimNameSearch = UpperCaseClaimName,
                        ClaimValueSearch = UpperCaseClaimValue,
                        ExpectedHasClaim = false,
                        ExpectedClaim = null,
                        ExpectedClaims = [],
                    },
                    new CaseSensitiveClaimsIdentityTheoryData("LowercaseMixedSearch_ClaimsMissing")
                    {
                         ClaimsIdentity = CreateCaseSensitiveClaimsIdentity(new JObject {
                                [UpperCaseClaimName] = LowerCaseClaimValue,
                            }),
                        ClaimNameSearch = LowerCaseClaimName,
                        ClaimValueSearch = LowerCaseClaimValue,
                        ExpectedHasClaim = false,
                        ExpectedClaim = null,
                        ExpectedClaims = [],
                    },
                };
            }
        }

        [Fact]
        public void CaseSensitiveClaimsIdentity_IsSerializableTest()
        {
            // arrange
            CaseSensitiveClaimsIdentity claimsIdentity = (CaseSensitiveClaimsIdentity)CreateCaseSensitiveClaimsIdentity(new JObject
            {
                [UpperCaseClaimName] = LowerCaseClaimValue,
            });
            CaseSensitiveClaimsIdentity deserializedClaimsIdentity;

            // act
            var memoryStream = new MemoryStream();
            var serializerOptions = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };

            JsonSerializer.Serialize(memoryStream, claimsIdentity, typeof(CaseSensitiveClaimsIdentity), serializerOptions);
            memoryStream.Seek(0, SeekOrigin.Begin);
            deserializedClaimsIdentity = (CaseSensitiveClaimsIdentity)JsonSerializer.Deserialize(memoryStream, typeof(CaseSensitiveClaimsIdentity), serializerOptions);

            // assert
            Assert.NotNull(deserializedClaimsIdentity);
            Assert.Equal(claimsIdentity.NameClaimType, deserializedClaimsIdentity.NameClaimType);
            Assert.Equal(claimsIdentity.RoleClaimType, deserializedClaimsIdentity.RoleClaimType);
        }

        [Fact]
        public void CaseSensitiveClaimsIdentity_DerivedClassCanSetToken()
        {
            var jwt = new JwtSecurityToken();
            CaseSensitiveClaimsIdentity claimsIdentity = new TestCaseSensitveClaimsIdentity(jwt);

            Assert.Same(jwt, claimsIdentity.SecurityToken);
        }

        public class CaseSensitiveClaimsIdentityTheoryData(string testId) : TheoryDataBase(testId)
        {
            internal ClaimsIdentity ClaimsIdentity { get; set; }
            internal string ClaimNameSearch { get; set; }
            internal string ClaimValueSearch { get; set; }
            internal bool ExpectedHasClaim { get; set; }
            internal string ExpectedClaim { get; set; }
            internal List<string> ExpectedClaims { get; set; }
        }

        public class TestCaseSensitveClaimsIdentity : CaseSensitiveClaimsIdentity
        {
            public TestCaseSensitveClaimsIdentity(SecurityToken token)
            {
                SecurityToken = token ?? throw new ArgumentNullException(nameof(token));
            }
        }

        private static ClaimsIdentity CreateCaseSensitiveClaimsIdentity(JObject claims, TokenValidationParameters validationParameters = null)
        {
            var handler = new JsonWebTokenHandler();
            return handler.CreateClaimsIdentityInternal(new JsonWebToken(CreateUnsignedToken(claims)), validationParameters ?? new TokenValidationParameters(), Default.Issuer);
        }

        private static string CreateUnsignedToken(JObject payload)
        {
            // Add default claims to the beginning of the payload
            var jObject = new JObject(_defaultPayload);
            jObject.Merge(payload);
            return string.Concat(Base64UrlEncoder.Encode("{}"), ".", Base64UrlEncoder.Encode(jObject.ToString()), ".");
        }
    }
}
