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

using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace System.IdentityModel.Tokens.Jwt.Tests
{
    public class JwtSecurityTokenTests
    {
        [Fact]
        public void Defaults()
        {
            JwtSecurityToken jwt = new JwtSecurityToken();
            List<Claim> claims = jwt.Claims as List<Claim>;
            Assert.NotNull(claims);

            foreach (Claim c in jwt.Claims)
            {
                Assert.True(false, "claims.Count != 0");
                break;
            }

            Assert.Null(jwt.Actor);
            Assert.NotNull(jwt.Audiences);
            foreach (string aud in jwt.Audiences)
            {
                Assert.True(false, "jwt.Audiences should be empty");
            }
            Assert.Null(jwt.Id);
            Assert.Null(jwt.Issuer);
            Assert.Null(jwt.SecurityKey);
            Assert.NotNull(jwt.SignatureAlgorithm);
            Assert.Equal("none", jwt.SignatureAlgorithm);
            Assert.Null(jwt.SigningCredentials);
            Assert.Null(jwt.SigningKey);
            Assert.Null(jwt.Subject);
            Assert.Equal(jwt.ValidFrom, DateTime.MinValue);
            Assert.Equal(jwt.ValidTo, DateTime.MinValue);
            Assert.Null(jwt.RawData);
            Assert.NotNull(jwt.Header);
            Assert.NotNull(jwt.Payload);
            Assert.NotNull(jwt.EncodedHeader);
            Assert.NotNull(jwt.EncodedPayload);
            Assert.Null(jwt.InnerToken);
            Assert.Null(jwt.RawAuthenticationTag);
            Assert.Null(jwt.RawCiphertext);
            Assert.Null(jwt.RawEncryptedKey);
            Assert.Null(jwt.RawInitializationVector);
            Assert.Null(jwt.EncryptingCredentials);
        }

        [Fact]
        public void Constructors()
        {
            Console.WriteLine("Entering: JwtSecurityToken_Constructor");
            RunConstructionTest(
                new JwtSecurityTokenTestVariation
                {
                    Name = "ClaimsSet with all Reserved claim types, ensures that users can add as they see fit",
                    Claims = ClaimSets.AllReserved,
                    ExpectedException = ExpectedException.NoExceptionExpected,
                });

            RunConstructionTest(
                new JwtSecurityTokenTestVariation
                {
                    Name = "All null params",
                    Issuer = null,
                    Audience = null,
                    Claims = null,
                    SigningCredentials = null,
                    ExpectedException = ExpectedException.NoExceptionExpected,
                });

            RunConstructionTest(
                new JwtSecurityTokenTestVariation
                {
                    Name = "NotBefore > Expires, .Net datetime",
                    NotBefore = DateTime.UtcNow + TimeSpan.FromHours(1),
                    Expires = DateTime.UtcNow,
                    ExpectedException = ExpectedException.ArgumentException(substringExpected: "IDX12401"),
                });

            RunConstructionTest(
                new JwtSecurityTokenTestVariation
                {
                    Name = "NotBefore > Expires, UnixEpoch - 1 ms",
                    NotBefore = DateTime.UtcNow,
                    Expires = EpochTime.UnixEpoch - TimeSpan.FromMilliseconds(1),
                    ExpectedException = ExpectedException.ArgumentException(substringExpected: "IDX12401"),
                });

            RunConstructionTest(
                new JwtSecurityTokenTestVariation
                {
                    Name = "NotBefore > Expires, UnixEpoch - 1 s",
                    NotBefore = DateTime.UtcNow,
                    Expires = EpochTime.UnixEpoch - TimeSpan.FromSeconds(1),
                    ExpectedException = ExpectedException.ArgumentException(substringExpected: "IDX12401"),
                });

            RunConstructionTest(
                new JwtSecurityTokenTestVariation
                {
                    Name = "NotBefore == DateItime.MinValue",
                    NotBefore = DateTime.MinValue,
                    Expires = DateTime.UtcNow,
                });
        }

        [Theory, MemberData(nameof(EmbeddedTokenConstructorData))]
        public void EmbeddedTokenConstructor1(string testId, JwtSecurityTokenTestVariation outerTokenVariation, JwtSecurityTokenTestVariation innerTokenVariation, string jwt, ExpectedException ee)
        {
            JwtSecurityToken outerJwt = null;
            JwtSecurityToken innerJwt = null;

            // create inner token
            try
            {
                if (innerTokenVariation != null)
                    innerJwt = CreateToken(innerTokenVariation);
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }

            // create outer token
            try
            {
                if (string.IsNullOrEmpty(jwt))
                    outerJwt = new JwtSecurityToken(
                        header: outerTokenVariation.Header,
                        innerToken: innerJwt,
                        rawHeader: outerTokenVariation.RawHeader,
                        rawEncryptedKey: outerTokenVariation.RawEncryptedKey,
                        rawInitializationVector: outerTokenVariation.RawInitializationVector,
                        rawCiphertext: outerTokenVariation.RawCiphertext,
                        rawAuthenticationTag: outerTokenVariation.RawAuthenticationTag);
                else
                    outerJwt = new JwtSecurityToken(jwt);

                ee.ProcessNoException();
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }

            try
            {
                // ensure we can get to every outer token property
                if (outerJwt != null && (ee == null || ee.TypeExpected == null))
                {
                    TestUtilities.CallAllPublicInstanceAndStaticPropertyGets(outerJwt, testId);
                }

                if (null != outerTokenVariation.ExpectedJwtSecurityToken)
                {
                    Assert.True(IdentityComparer.AreEqual(outerTokenVariation.ExpectedJwtSecurityToken, outerJwt));
                }
            }
            catch (Exception ex)
            {
                Assert.True(false, string.Format("Testcase: {0}. UnExpected when getting a properties: '{1}'", outerTokenVariation.Name, ex.ToString()));
            }

            try
            {
                // ensure we can get to every inner token property
                if (innerJwt != null && (ee == null || ee.TypeExpected == null))
                {
                    TestUtilities.CallAllPublicInstanceAndStaticPropertyGets(innerJwt, testId);
                }

                if (null != innerTokenVariation && null != innerTokenVariation.ExpectedJwtSecurityToken)
                {
                    Assert.True(IdentityComparer.AreEqual(innerTokenVariation.ExpectedJwtSecurityToken, innerJwt));
                }
            }
            catch (Exception ex)
            {
                Assert.True(false, string.Format("Testcase: {0}. UnExpected when getting a properties: '{1}'", testId, ex.ToString()));
            }

            try
            {
                if (outerJwt != null && innerJwt != null && (ee == null || ee.TypeExpected == null))
                {
                    // confirm properties of outer token match our expectation
                    Assert.Equal(outerJwt.InnerToken, innerJwt);
                    CheckPayloadProperties(outerJwt, innerJwt);
                    CheckOuterTokenProperties(outerJwt, outerTokenVariation);
                }
            }
            catch (Exception ex)
            {
                Assert.True(false, string.Format("Testcase: {0}. Unexpected inequality between outer and inner token properties: '{1}'", testId, ex.ToString()));
            }

        }

        public static TheoryData<string, JwtSecurityTokenTestVariation, JwtSecurityTokenTestVariation, string, ExpectedException> EmbeddedTokenConstructorData()
        {
            var dataSet = new TheoryData<string, JwtSecurityTokenTestVariation, JwtSecurityTokenTestVariation, string, ExpectedException>();

            dataSet.Add("Embedded token all properties null",
                // outer token
                new JwtSecurityTokenTestVariation
                {
                    RawHeader = null,
                    RawEncryptedKey = null,
                    RawInitializationVector = null,
                    RawCiphertext = null,
                    RawAuthenticationTag = null,
                },
                // inner token
                new JwtSecurityTokenTestVariation
                {
                    Issuer = null,
                    Audience = null,
                    Claims = null,
                    SigningCredentials = null,
                },
                String.Empty,
                ExpectedException.ArgumentNullException()
            );

            JwtSecurityTokenTestVariation innerToken = new JwtSecurityTokenTestVariation
            {
                NotBefore = DateTime.MinValue,
                Expires = DateTime.UtcNow,
            };

            JwtSecurityTokenTestVariation outerValidJweDirect = CreateVariationOnToken(EncodedJwts.ValidJweDirect);
            dataSet.Add("ValidJweDirect- Construct by parts", outerValidJweDirect, innerToken, String.Empty, ExpectedException.NoExceptionExpected);
            dataSet.Add("ValidJweDirect- Construct by string", outerValidJweDirect, null, EncodedJwts.ValidJweDirect, ExpectedException.NoExceptionExpected);

            JwtSecurityTokenTestVariation outerValidJweDirect2 = CreateVariationOnToken(EncodedJwts.ValidJweDirect2);
            dataSet.Add("ValidJweDirect2- Construct by parts", outerValidJweDirect2, innerToken, String.Empty, ExpectedException.NoExceptionExpected);
            dataSet.Add("ValidJweDirect2- Construct by string", outerValidJweDirect2, null, EncodedJwts.ValidJweDirect2, ExpectedException.NoExceptionExpected);

            JwtSecurityTokenTestVariation outerValidJwe = CreateVariationOnToken(EncodedJwts.ValidJwe);
            dataSet.Add("ValidJwe- Construct by parts", outerValidJwe, innerToken, String.Empty, ExpectedException.NoExceptionExpected);
            dataSet.Add("ValidJwe- Construct by string", outerValidJwe, null, EncodedJwts.ValidJwe, ExpectedException.NoExceptionExpected);

            JwtSecurityTokenTestVariation outerValidJwe2 = CreateVariationOnToken(EncodedJwts.ValidJwe2);
            dataSet.Add("ValidJwe2- Construct by parts", outerValidJwe2, innerToken, String.Empty, ExpectedException.NoExceptionExpected);
            dataSet.Add("ValidJwe2- Construct by string", outerValidJwe2, null, EncodedJwts.ValidJwe2, ExpectedException.NoExceptionExpected);

            // Hand in a valid variation. We should fail before the variation is used.
            dataSet.Add("Invalid outer token 1- Construct by string", outerValidJweDirect, null, EncodedJwts.InvalidJwe, ExpectedException.ArgumentException(substringExpected: "IDX12741"));
            dataSet.Add("Invalid outer token 2- Construct by string", outerValidJweDirect, null, EncodedJwts.InvalidJwe2, ExpectedException.ArgumentException(substringExpected: "IDX12741"));
            dataSet.Add("Invalid outer token 3- Construct by string", outerValidJweDirect, null, EncodedJwts.InvalidJwe3, ExpectedException.ArgumentException(substringExpected: "IDX12740"));
            dataSet.Add("Invalid outer token 4- Construct by string", outerValidJweDirect, null, EncodedJwts.InvalidJwe4, ExpectedException.ArgumentException(substringExpected: "IDX12741"));
            dataSet.Add("Invalid outer token 5- Construct by string", outerValidJweDirect, null, EncodedJwts.InvalidJwe5, ExpectedException.ArgumentException(substringExpected: "IDX12740"));
            dataSet.Add("Invalid outer token 6- Construct by string", outerValidJweDirect, null, EncodedJwts.InvalidJwe6, ExpectedException.ArgumentException(substringExpected: "IDX12740"));

            return dataSet;
        }

        private static JwtSecurityTokenTestVariation CreateVariationOnToken(string jwt)
        {
            string rawHeader, rawEncryptedKey, rawInitializationVector, rawCipherText, rawAuthenticationTag;
            ParseJweParts(jwt, out rawHeader, out rawEncryptedKey, out rawInitializationVector, out rawCipherText, out rawAuthenticationTag);
            return new JwtSecurityTokenTestVariation
            {
                Header = new JwtHeader(Default.SymmetricEncryptingCredentials),
                RawHeader = rawHeader,
                RawEncryptedKey = rawEncryptedKey,
                RawInitializationVector = rawInitializationVector,
                RawCiphertext = rawCipherText,
                RawAuthenticationTag = rawAuthenticationTag
            };
        }

        private static void CheckOuterTokenProperties(JwtSecurityToken token, JwtSecurityTokenTestVariation variation)
        {
            Assert.Equal(token.RawHeader, variation.RawHeader);
            Assert.Equal(token.RawEncryptedKey, variation.RawEncryptedKey);
            Assert.Equal(token.RawInitializationVector, variation.RawInitializationVector);
            Assert.Equal(token.RawCiphertext, variation.RawCiphertext);
            Assert.Equal(token.RawAuthenticationTag, variation.RawAuthenticationTag);
        }

        private static void CheckPayloadProperties(JwtSecurityToken token1, JwtSecurityToken token2)
        {
            Assert.Equal(token1.Payload.Acr, token2.Payload.Acr);
            Assert.Equal(token1.Payload.Actort, token2.Payload.Actort);
            Assert.Equal(token1.Payload.Amr, token2.Payload.Amr);
            Assert.Equal(token1.Payload.Aud, token2.Payload.Aud);
            Assert.Equal(token1.Payload.AuthTime, token2.Payload.AuthTime);
            Assert.Equal(token1.Payload.CHash, token2.Payload.CHash);
            Assert.Equal(token1.Payload.Exp, token2.Payload.Exp);
            Assert.Equal(token1.Payload.Iat, token2.Payload.Iat);
            Assert.Equal(token1.Payload.Iss, token2.Payload.Iss);
            Assert.Equal(token1.Payload.Jti, token2.Payload.Jti);
            Assert.Equal(token1.Payload.Keys, token2.Payload.Keys);
            Assert.Equal(token1.Payload.Nbf, token2.Payload.Nbf);
            Assert.Equal(token1.Payload.Nonce, token2.Payload.Nonce);
            Assert.Equal(token1.Payload.Sub, token2.Payload.Sub);
            Assert.Equal(token1.Payload.ValidFrom, token2.Payload.ValidFrom);
            Assert.Equal(token1.Payload.ValidTo, token2.Payload.ValidTo);
        }

        private static void ParseJweParts(string jwe, out string headerPart, out string encryptedKeyPart, out string initializationVectorPart, out string ciphertextPart, out string authenticationTagPart)
        {
            if (string.IsNullOrEmpty(jwe))
                throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(jwe)));

            string[] parts = jwe.Split(new char[] {'.'}, 6);
            if (parts.Length != 5)
                throw new ArgumentException(string.Format("The JWE token must have 5 parts. The JWE {0} has {1} parts.", jwe, parts.Length));

            headerPart = parts[0];
            encryptedKeyPart = parts[1];
            initializationVectorPart = parts[2];
            ciphertextPart = parts[3];
            authenticationTagPart = parts[4];
        }

        private void RunConstructionTest(JwtSecurityTokenTestVariation variation)
        {
            JwtSecurityToken jwt = null;
            try
            {
                jwt = CreateToken(variation);
                variation.ExpectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                variation.ExpectedException.ProcessException(ex);
            }

            try
            {
                // ensure we can get to every property
                if (jwt != null && (variation.ExpectedException == null || variation.ExpectedException.TypeExpected == null))
                {
                    TestUtilities.CallAllPublicInstanceAndStaticPropertyGets(jwt, variation.Name);
                }

                if (null != variation.ExpectedJwtSecurityToken)
                {
                    Assert.True(IdentityComparer.AreEqual(variation.ExpectedJwtSecurityToken, jwt));
                }
            }
            catch (Exception ex)
            {
                Assert.True(false, string.Format("Testcase: {0}. UnExpected when getting a properties: '{1}'", variation.Name, ex.ToString()));
            }
        }

        private JwtSecurityToken CreateToken(JwtSecurityTokenTestVariation variation)
        {
            return new JwtSecurityToken(
                issuer: variation.Issuer,
                audience: variation.Audience,
                claims: variation.Claims,
                signingCredentials: variation.SigningCredentials,
                notBefore: variation.NotBefore,
                expires: variation.Expires);
        }

        [Theory, MemberData(nameof(JwtSegmentTheoryData))]
        public void JwtSegment(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.JwtSegment", theoryData);
            try
            {
                var jwtToken = new JwtSecurityToken(theoryData.Token);
                theoryData.ExpectedException.ProcessNoException(context);
                TestUtilities.CallAllPublicInstanceAndStaticPropertyGets(jwtToken, theoryData.TestId);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JwtTheoryData> JwtSegmentTheoryData
        {
            get
            {
                var theoryData = new TheoryData<JwtTheoryData>();

                JwtTestData.InvalidRegExSegmentsData(theoryData);
                JwtTestData.InvalidNumberOfSegmentsData("IDX12741:", theoryData);
                JwtTestData.InvalidEncodedSegmentsData("", theoryData);
                JwtTestData.ValidEncodedSegmentsData(theoryData);

                return theoryData;
            }

        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
