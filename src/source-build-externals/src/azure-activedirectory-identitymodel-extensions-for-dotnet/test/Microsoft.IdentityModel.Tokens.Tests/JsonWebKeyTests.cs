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
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class JsonWebKeyTests
    {
        [Theory, MemberData(nameof(JsonWebKeyDataSet))]
        public void Constructors(string json, JsonWebKey compareTo, ExpectedException ee)
        {
            var context = new CompareContext();
            try
            {
                var jsonWebKey = new JsonWebKey(json);
                ee.ProcessNoException(context);
                if (compareTo != null)
                    IdentityComparer.AreEqual(jsonWebKey, compareTo, context);
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex, context.Diffs);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<string, JsonWebKey, ExpectedException> JsonWebKeyDataSet
        {
            get
            {
                var dataset = new TheoryData<string, JsonWebKey, ExpectedException>();

                dataset.Add(null, null, ExpectedException.ArgumentNullException(substringExpected: "json"));
                dataset.Add(DataSets.JsonWebKeyFromPingString1, DataSets.JsonWebKeyFromPing1, ExpectedException.NoExceptionExpected);
                dataset.Add(DataSets.JsonWebKeyString1, DataSets.JsonWebKey1, ExpectedException.NoExceptionExpected);
                dataset.Add(DataSets.JsonWebKeyString2, DataSets.JsonWebKey2, ExpectedException.NoExceptionExpected);
                dataset.Add(DataSets.JsonWebKeyBadFormatString1, null, ExpectedException.ArgumentException(inner: typeof(JsonReaderException)));
                dataset.Add(DataSets.JsonWebKeyBadFormatString2, null, ExpectedException.ArgumentException(inner: typeof(JsonSerializationException)));
                dataset.Add(DataSets.JsonWebKeyBadX509String, DataSets.JsonWebKeyBadX509Data, ExpectedException.NoExceptionExpected);

                return dataset;
            }
        }

        [Fact]
        public void Defaults()
        {
            var context = new CompareContext();
            JsonWebKey jsonWebKey = new JsonWebKey();

            if (jsonWebKey.Alg != null)
                context.Diffs.Add("jsonWebKey.Alg != null");

            if (jsonWebKey.KeyOps.Count != 0)
                context.Diffs.Add("jsonWebKey.KeyOps.Count != 0");

            if (jsonWebKey.Kid != null)
                context.Diffs.Add("jsonWebKey.Kid != null");

            if (jsonWebKey.Kty != null)
                context.Diffs.Add("jsonWebKey.Kty != null");

            if (jsonWebKey.X5c == null)
                context.Diffs.Add("jsonWebKey.X5c == null");

            if (jsonWebKey.X5c.Count != 0)
                context.Diffs.Add("jsonWebKey.X5c.Count != 0");

            if (jsonWebKey.X5t != null)
                context.Diffs.Add("jsonWebKey.X5t != null");

            if (jsonWebKey.X5u != null)
                context.Diffs.Add("jsonWebKey.X5u != null");

            if (jsonWebKey.Use != null)
                context.Diffs.Add("jsonWebKey.Use != null");

            if (jsonWebKey.AdditionalData == null)
                context.Diffs.Add("jsonWebKey.AdditionalData == null");
            else if (jsonWebKey.AdditionalData.Count != 0)
                context.Diffs.Add("jsonWebKey.AdditionalData.Count != 0");

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void GetSets()
        {
            JsonWebKey jsonWebKey = new JsonWebKey();
            TestUtilities.CallAllPublicInstanceAndStaticPropertyGets(jsonWebKey, "JsonWebKey_GetSets");
            List<string> methods = new List<string> { "Alg", "Kid", "Kty", "X5t", "X5u", "Use" };
            List<string> errors = new List<string>();
            foreach (string method in methods)
            {
                TestUtilities.GetSet(jsonWebKey, method, null, new object[] { Guid.NewGuid().ToString(), null, Guid.NewGuid().ToString() }, errors);
                jsonWebKey.X5c.Add(method);
            }

            CompareContext context = new CompareContext();
            if (IdentityComparer.AreEqual(jsonWebKey.X5c, methods, context))
            {
                errors.AddRange(context.Diffs);
            }

            TestUtilities.AssertFailIfErrors("JsonWebKey_GetSets", errors);
        }

        [Fact]
        public void Publics()
        {
        }

        // Tests to make sure conditional property serialization for JsonWebKeys is working properly.
        [Fact]
        public void ConditionalPropertySerialization()
        {
            var context = new CompareContext();

            var jsonWebKeyEmptyCollections = new JsonWebKey
            {
                Alg = "SHA256",
                E = "AQAB",
                Kid = "kriMPdmBvx68skT8-mPAB3BseeA",
                Kty = "RSA",
                N = "kSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuw==",
                X5t = "kriMPdmBvx68skT8-mPAB3BseeA",
                Use = "sig",
            };

            var jsonString1 = JsonConvert.SerializeObject(jsonWebKeyEmptyCollections);
            if (jsonString1.Contains("key_ops"))
                context.Diffs.Add("key_ops is empty and should not be present in serialized JsonWebKey");
            if (jsonString1.Contains("x5c"))
                context.Diffs.Add("x5c is empty and should not be present in serialized JsonWebKey");

            var jsonWebKeyWithCollections = new JsonWebKey();
            jsonWebKeyWithCollections.X5c.Add("MIIDPjCCAiqgAwIBAgIQVWmXY/+9RqFA/OG9kFulHDAJBgUrDgMCHQUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMTIwNjA3MDcwMDAwWhcNMTQwNjA3MDcwMDAwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVwIDAQABo2IwYDBeBgNVHQEEVzBVgBCxDDsLd8xkfOLKm4Q/SzjtoS8wLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldIIQVWmXY/+9RqFA/OG9kFulHDAJBgUrDgMCHQUAA4IBAQAkJtxxm/ErgySlNk69+1odTMP8Oy6L0H17z7XGG3w4TqvTUSWaxD4hSFJ0e7mHLQLQD7oV/erACXwSZn2pMoZ89MBDjOMQA+e6QzGB7jmSzPTNmQgMLA8fWCfqPrz6zgH+1F1gNp8hJY57kfeVPBiyjuBmlTEBsBlzolY9dd/55qqfQk6cgSeCbHCy/RU/iep0+UsRMlSgPNNmqhj5gmN2AFVCN96zF694LwuPae5CeR2ZcVknexOWHYjFM0MgUSw0ubnGl0h9AJgGyhvNGcjQqu9vd1xkupFgaN+f7P3p3EVN5csBg5H94jEcQZT7EKeTiZ6bTrpDAnrr8tDCy8ng");
            jsonWebKeyWithCollections.KeyOps.Add("signing");
            var jsonString2 = JsonConvert.SerializeObject(jsonWebKeyWithCollections);
            if (!jsonString2.Contains("key_ops"))
                context.Diffs.Add("key_ops is non-empty and should be present in serialized JsonWebKey");
            if (!jsonString2.Contains("x5c"))
                context.Diffs.Add("x5c is non-empty and should be present in serialized JsonWebKey");

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void ComputeJwkThumbprintSpec()
        {
            // https://datatracker.ietf.org/doc/html/rfc7638#section-3.1
            var context = TestUtilities.WriteHeader($"{this}.ComputeJwkThumbprintSpec", "", true);

            var jwk = new JsonWebKey()
            {
                Kty = JsonWebAlgorithmsKeyTypes.RSA,
                E = "AQAB",
                N = "0vx7agoebGcQSuuPiLJXZptN9nndrQmbXEps2aiAFbWhM78LhWx4cbbfAAtVT86zwu1RK7aPFFxuhDR1L6tSoc_BJECPebWKRXjBZCiFV4n3oknjhMstn64tZ_2W-5JsGY4Hc5n9yBXArwl93lqt7_RN5w6Cf0h4QyQ5v-65YGjQR0_FDW2QvzqY368QQMicAtaSqzs8KJZgnYb9c7d0zgdAZHzu6qMQvRL5hajrn1n91CbOpbISD08qNLyrdkt-bFTWhAI4vMQFh6WeZu0fM4lFd2NcRwr3XPksINHaQ-G_xBniIqbw0Ls1jF44-csFCur-kEgU8awapJzKnqDKgw"
            };

            var jwkThumbprint = jwk.ComputeJwkThumbprint();
            var base64UrlEncodedJwkThumbprint = Base64UrlEncoder.Encode(jwkThumbprint);

            var expectedJwkThumbprint = new byte[]
            {
                55, 54, 203, 177, 120, 124, 184, 48, 156, 119, 238, 140, 55, 5, 197,
                225, 111, 251, 158, 133, 151, 21, 144, 31, 30, 76, 89, 177, 17, 130,
                245, 123
            };
            var expectedBase64UrlEncodedThumbprint = "NzbLsXh8uDCcd-6MNwXF4W_7noWXFZAfHkxZsRGC9Xs";

            IdentityComparer.AreBytesEqual(jwkThumbprint, expectedJwkThumbprint, context);
            IdentityComparer.AreStringsEqual(base64UrlEncodedJwkThumbprint, expectedBase64UrlEncodedThumbprint, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(ComputeJwkThumbprintTheoryData))]
        public void ComputeJwkThumbprint(JwkThumbprintTheoryData theoryData)
        {
            Logging.IdentityModelEventSource.ShowPII = true;
            var context = TestUtilities.WriteHeader($"{this}.ComputeJwkThumbprint", theoryData);
            try
            {
                var jwkThumbprint = Base64UrlEncoder.Encode(theoryData.JWK.ComputeJwkThumbprint());
                IdentityComparer.AreStringsEqual(jwkThumbprint, theoryData.ExpectedBase64UrlEncodedJwkThumbprint, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(ComputeJwkThumbprintTheoryData))]
        public void CanComputeJwkThumbprint(JwkThumbprintTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CanComputeJwkThumbprint", theoryData);
            if (theoryData.CanComputeJwkThumbprint != theoryData.JWK.CanComputeJwkThumbprint())
                context.AddDiff($"theoryData.CanComputeJwkThumbprint ({theoryData.CanComputeJwkThumbprint}) != theoryData.JWK.CanComputeJwkThumbprint ({theoryData.JWK.CanComputeJwkThumbprint()})");

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JwkThumbprintTheoryData> ComputeJwkThumbprintTheoryData
        {
            get
            {
                return new TheoryData<JwkThumbprintTheoryData>
                {
                    new JwkThumbprintTheoryData
                    {
                        First = true,
                        JWK = new JsonWebKey() { },
                        CanComputeJwkThumbprint = false,
                        ExpectedException = ExpectedException.ArgumentException("IDX10705: Cannot create a JWK thumbprint, 'Kty' is null or empty."),
                        TestId = "InvalidKtyIsNull",
                    },
                    new JwkThumbprintTheoryData
                    {
                        JWK = new JsonWebKey()
                        {
                            Kty = string.Empty
                        },
                        CanComputeJwkThumbprint = false,
                        ExpectedException = ExpectedException.ArgumentException("IDX10705: Cannot create a JWK thumbprint, 'Kty' is null or empty."),
                        TestId = "InvalidKtyIsEmpty",
                    },
                    new JwkThumbprintTheoryData
                    {
                        JWK = new JsonWebKey()
                        {
                            Kty = "INVALID_DATA"
                        },
                        CanComputeJwkThumbprint = false,
                        ExpectedException = ExpectedException.ArgumentException("IDX10706: Cannot create a JWK thumbprint, 'Kty'"),
                        TestId = "InvalidKtyNotAsExpected",
                    },
                    new JwkThumbprintTheoryData
                    {
                        JWK = new JsonWebKey()
                        {
                            Kty = JsonWebAlgorithmsKeyTypes.RSA
                        },
                        CanComputeJwkThumbprint = false,
                        ExpectedException = ExpectedException.ArgumentException("IDX10705: Cannot create a JWK thumbprint, 'E'"),
                        TestId = "InvalidEIsNullOrEmpty"
                    },
                    new JwkThumbprintTheoryData
                    {
                        JWK = new JsonWebKey()
                        {
                            Kty = JsonWebAlgorithmsKeyTypes.RSA,
                            E = "AQAB"
                        },
                        CanComputeJwkThumbprint = false,
                        ExpectedException = ExpectedException.ArgumentException("IDX10705: Cannot create a JWK thumbprint, 'N'"),
                        TestId = "InvalidNIsNullOrEmpty"
                    },
                    new JwkThumbprintTheoryData
                    {
                        JWK = new JsonWebKey()
                        {
                            Kty = JsonWebAlgorithmsKeyTypes.Octet
                        },
                        CanComputeJwkThumbprint = false,
                        ExpectedException = ExpectedException.ArgumentException("IDX10705: Cannot create a JWK thumbprint, 'K'"),
                        TestId = "InvalidKIsNullOrEmpty"
                    },
                    new JwkThumbprintTheoryData
                    {
                        JWK = new JsonWebKey()
                        {
                            Kty = JsonWebAlgorithmsKeyTypes.EllipticCurve
                        },
                        CanComputeJwkThumbprint = false,
                        ExpectedException = ExpectedException.ArgumentException("IDX10705: Cannot create a JWK thumbprint, 'Crv'"),
                        TestId = "InvalidCrvIsNullOrEmpty"
                    },
                    new JwkThumbprintTheoryData
                    {
                        JWK = new JsonWebKey()
                        {
                            Kty = JsonWebAlgorithmsKeyTypes.EllipticCurve,
                            Crv = "P-256"
                        },
                        CanComputeJwkThumbprint = false,
                        ExpectedException = ExpectedException.ArgumentException("IDX10705: Cannot create a JWK thumbprint, 'X'"),
                        TestId = "InvalidCrvIsNullOrEmpty"
                    },
                    new JwkThumbprintTheoryData
                    {
                        JWK = new JsonWebKey()
                        {
                            Kty = JsonWebAlgorithmsKeyTypes.EllipticCurve,
                            Crv = "P-256",
                            X = "f83OJ3D2xF1Bg8vub9tLe1gHMzV76e8Tus9uPHvRVEU",
                        },
                        CanComputeJwkThumbprint = false,
                        ExpectedException = ExpectedException.ArgumentException("IDX10705: Cannot create a JWK thumbprint, 'Y'"),
                        TestId = "InvalidCrvIsNullOrEmpty"
                    },
                    new JwkThumbprintTheoryData
                    {
                        JWK = new JsonWebKey()
                        {
                            Kty = JsonWebAlgorithmsKeyTypes.RSA,
                            E = "AQAB",
                            N = "0vx7agoebGcQSuuPiLJXZptN9nndrQmbXEps2aiAFbWhM78LhWx4cbbfAAtVT86zwu1RK7aPFFxuhDR1L6tSoc_BJECPebWKRXjBZCiFV4n3oknjhMstn64tZ_2W-5JsGY4Hc5n9yBXArwl93lqt7_RN5w6Cf0h4QyQ5v-65YGjQR0_FDW2QvzqY368QQMicAtaSqzs8KJZgnYb9c7d0zgdAZHzu6qMQvRL5hajrn1n91CbOpbISD08qNLyrdkt-bFTWhAI4vMQFh6WeZu0fM4lFd2NcRwr3XPksINHaQ-G_xBniIqbw0Ls1jF44-csFCur-kEgU8awapJzKnqDKgw"
                        },
                        CanComputeJwkThumbprint = true,
                        ExpectedBase64UrlEncodedJwkThumbprint = "NzbLsXh8uDCcd-6MNwXF4W_7noWXFZAfHkxZsRGC9Xs",
                        TestId = "ValidRsa"
                    },
                    new JwkThumbprintTheoryData
                    { 
                        JWK = new JsonWebKey()
                        {
                            Kty = JsonWebAlgorithmsKeyTypes.EllipticCurve,
                            Crv = "P-256",
                            X = "f83OJ3D2xF1Bg8vub9tLe1gHMzV76e8Tus9uPHvRVEU",
                            Y = "x_FEzRu9m36HLN_tue659LNpXW6pCyStikYjKIWI5a0"
                        },
                        CanComputeJwkThumbprint = true,
                        ExpectedBase64UrlEncodedJwkThumbprint = "oKIywvGUpTVTyxMQ3bwIIeQUudfr_CkLMjCE19ECD-U",
                        TestId = "ValidEc"
                    },
                    new JwkThumbprintTheoryData
                    {
                        JWK = new JsonWebKey()
                        {
                            Kty = JsonWebAlgorithmsKeyTypes.Octet,
                            K = "Vbxq2mlbGJw8XH+ZoYBnUHmHga8/o/IduvU/Tht70iE=" // KeyingMaterial.DefaultSymmetricKeyEncoded_256
                        },
                        CanComputeJwkThumbprint = true,
                        ExpectedBase64UrlEncodedJwkThumbprint = "uQcNOQPV2rRRS-R_VQnj7gRR_19AaHlGbU0f9F5hkUs",
                        TestId = "ValidOctet"
                    },
                };
            }
        }

        public class JwkThumbprintTheoryData : TheoryDataBase
        {
            public JsonWebKey JWK { get; set; }

            public string ExpectedBase64UrlEncodedJwkThumbprint { get; set; }

            public bool CanComputeJwkThumbprint { get; set; }
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
