// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class X509SecurityKeyTests
    {
        [Fact]
        public void Constructor()
        {
            var context = new CompareContext();
            var expectedException = new ExpectedException(typeExpected: typeof(ArgumentNullException), substringExpected: "certificate");
            try
            {
                new X509SecurityKey((X509Certificate2)null);
                expectedException.ProcessNoException(context);
            }
            catch (Exception exception)
            {
                expectedException.ProcessException(exception, context);
            }

            var certificate = KeyingMaterial.DefaultCert_2048;
            expectedException = new ExpectedException(typeExpected: typeof(ArgumentNullException), substringExpected: "keyId");
            try
            {
                new X509SecurityKey(certificate, null);
                expectedException.ProcessNoException(context);
            }
            catch (Exception exception)
            {
                expectedException.ProcessException(exception, context);
            }

            try
            {
                new X509SecurityKey(certificate, string.Empty);
                expectedException.ProcessNoException(context);
            }
            catch (Exception exception)
            {
                expectedException.ProcessException(exception, context);
            }

            expectedException = ExpectedException.NoExceptionExpected;
            try
            {
                var x509SecurityKey = new X509SecurityKey(certificate);
                IdentityComparer.AreEqual(x509SecurityKey.KeyId, certificate.Thumbprint, context);
                IdentityComparer.AreEqual(x509SecurityKey.X5t, Base64UrlEncoder.Encode(certificate.GetCertHash()), context);
                IdentityComparer.AreEqual(certificate, x509SecurityKey.Certificate, context);
            }
            catch (Exception exception)
            {
                expectedException.ProcessException(exception, context);
            }

            try
            {
                var x509SecurityKey = new X509SecurityKey(certificate, "KID");
                IdentityComparer.AreEqual(x509SecurityKey.KeyId, "KID", context);
                IdentityComparer.AreEqual(x509SecurityKey.X5t, Base64UrlEncoder.Encode(certificate.GetCertHash()), context);
                IdentityComparer.AreEqual(certificate, x509SecurityKey.Certificate, context);
            }
            catch (Exception exception)
            {
                expectedException.ProcessException(exception, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void CanComputeJwkThumbprint()
        {
            Assert.True(KeyingMaterial.DefaultX509Key_2048.CanComputeJwkThumbprint(), "Couldn't compute JWK thumbprint on an X509SecurityKey.");
        }
    }

    public class X509SecurityKeyTheoryData : TheoryDataBase
    {
        public X509SecurityKeyTheoryData(X509Certificate2 certificate, string algorithm, bool isSupported, string testId)
        {
            X509Certificate = certificate;
            Algorithm = algorithm;
            IsSupported = isSupported;
        }

        public X509SecurityKeyTheoryData(X509SecurityKey key, string algorithm, bool isSupported, string testId)
        {
            X509SecurityKey = key;
            Algorithm = algorithm;
            IsSupported = isSupported;
        }

        public string Algorithm { get; set; }

        string KeyId { get; set; }

        public bool IsSupported { get; set; }

        public X509Certificate X509Certificate { get; set; }

        public X509SecurityKey X509SecurityKey { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
