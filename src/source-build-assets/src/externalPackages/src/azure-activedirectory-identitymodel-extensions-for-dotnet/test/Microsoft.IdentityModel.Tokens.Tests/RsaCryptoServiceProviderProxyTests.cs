// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#if NET462 || NET472
using System.Security.Cryptography.X509Certificates;
#endif

#if NET462 || NET472

using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class RsaCryptoServiceProviderProxyTests
    {
        [Theory, MemberData(nameof(RSADecryptTheoryData), DisableDiscoveryEnumeration = true)]
        public void RSADecrypt(RSACryptoServiceProviderProxyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RSADecrypt", theoryData);

            try
            {
                var proxy = new RSACryptoServiceProviderProxy(theoryData.RsaCryptoServiceProvider);
                proxy.Decrypt(theoryData.Input, theoryData.UseOAEP);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(RSADecryptTheoryData), DisableDiscoveryEnumeration = true)]
        public void RSADecryptValue(RSACryptoServiceProviderProxyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RSADecryptValue", theoryData);

            try
            {
                var proxy = new RSACryptoServiceProviderProxy(theoryData.RsaCryptoServiceProvider);
                proxy.DecryptValue(theoryData.Input);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        // just check parameters, EncryptDecrypt checks round trip
        public static TheoryData<RSACryptoServiceProviderProxyTheoryData> RSADecryptTheoryData
        {
            get
            {
#if NET462 || NET472
                var rsaCsp = new RSACryptoServiceProvider();
                rsaCsp.ImportParameters(KeyingMaterial.RsaParameters_2048);
#else
                var rsaCsp = KeyingMaterial.DefaultCert_2048.PrivateKey as RSACryptoServiceProvider;
#endif
                return new TheoryData<RSACryptoServiceProviderProxyTheoryData>
                {
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("input"),
                        RsaCryptoServiceProvider = rsaCsp,
                        TestId = "Test1"
                    },
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("input"),
                        Input = new byte[0],
                        RsaCryptoServiceProvider = rsaCsp,
                        TestId = "Test2"
                    }
                };
            }
        }

        [Theory, MemberData(nameof(RSAEncryptDecryptTheoryData), DisableDiscoveryEnumeration = true)]
        public void RSAEncryptDecrypt(RSACryptoServiceProviderProxyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RSAEncryptDecrypt", theoryData);

            try
            {
                var proxy = new RSACryptoServiceProviderProxy(theoryData.RsaCryptoServiceProvider);
                var cipherTextProxy = proxy.Encrypt(theoryData.Input, theoryData.UseOAEP);
                var cipherTextRsa = theoryData.RsaCryptoServiceProvider.Encrypt(theoryData.Input, theoryData.UseOAEP);
                IdentityComparer.AreBytesEqual(
                    proxy.Decrypt(cipherTextProxy, theoryData.UseOAEP),
                    theoryData.RsaCryptoServiceProvider.Decrypt(cipherTextRsa, theoryData.UseOAEP),
                    context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<RSACryptoServiceProviderProxyTheoryData> RSAEncryptDecryptTheoryData
        {
            get
            {
#if NET462 || NET472
                var rsaFromX509Cert = new RSACryptoServiceProvider();
                var rsaCng = KeyingMaterial.DefaultCert_2048.GetRSAPrivateKey() as RSACng;
                var parameters = rsaCng.ExportParameters(true);
                rsaFromX509Cert.ImportParameters(parameters);
#else
                var rsaFromX509Cert = KeyingMaterial.DefaultCert_2048.PrivateKey as RSACryptoServiceProvider;
#endif
                var guid = Guid.NewGuid().ToByteArray();
                return new TheoryData<RSACryptoServiceProviderProxyTheoryData>
                {
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("rsa"),
                        TestId = "Test1"
                    },
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("input"),
                        RsaCryptoServiceProvider = rsaFromX509Cert,
                        TestId = "Test2"
                    },
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("input"),
                        Input = new byte[0],
                        RsaCryptoServiceProvider = rsaFromX509Cert,
                        TestId = "Test3"
                    },
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        Input = guid,
                        RsaCryptoServiceProvider = rsaFromX509Cert,
                        TestId = "Test4",
                        UseOAEP = true
                    },
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        Input = guid,
                        RsaCryptoServiceProvider = rsaFromX509Cert,
                        TestId = "Test5",
                        UseOAEP = false
                    }
                };
            }
        }

        [Theory, MemberData(nameof(RSAEncryptDecryptValueTheoryData), DisableDiscoveryEnumeration = true)]
        public void RSAEncryptDecryptValue(RSACryptoServiceProviderProxyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RSAEncryptDecryptValue", theoryData);

            try
            {
                var proxy = new RSACryptoServiceProviderProxy(theoryData.RsaCryptoServiceProvider);
                var cipherTextProxy = proxy.EncryptValue(theoryData.Input);
                var cipherTextRsa = theoryData.RsaCryptoServiceProvider.EncryptValue(theoryData.Input);
                IdentityComparer.AreBytesEqual(
                    proxy.DecryptValue(cipherTextProxy),
                    theoryData.RsaCryptoServiceProvider.DecryptValue(cipherTextRsa),
                    context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<RSACryptoServiceProviderProxyTheoryData> RSAEncryptDecryptValueTheoryData
        {
            get
            {
#if NET462 || NET472
                var rsaCsp = new RSACryptoServiceProvider();
                rsaCsp.ImportParameters(KeyingMaterial.RsaParameters_2048);
#else
                var rsaCsp = KeyingMaterial.DefaultCert_2048.PrivateKey as RSACryptoServiceProvider;
#endif
                var guid = Guid.NewGuid().ToByteArray();
                return new TheoryData<RSACryptoServiceProviderProxyTheoryData>
                {
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("rsa"),
                        TestId = "Test1"
                    },
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("input"),
                        RsaCryptoServiceProvider = rsaCsp,
                        TestId = "Test2"
                    },
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("input"),
                        Input = new byte[0],
                        RsaCryptoServiceProvider = rsaCsp,
                        TestId = "Test3"
                    },
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.NotSupportedException(),
                        Input = guid,
                        RsaCryptoServiceProvider = rsaCsp,
                        TestId = "Test4",
                        UseOAEP = true
                    }
                };
            }
        }

        [Theory, MemberData(nameof(RSASignVerifyDataTheoryData), DisableDiscoveryEnumeration = true)]
        public void RSASignVerifyData(RSACryptoServiceProviderProxyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RSASignVerifyData", theoryData);

            try
            {
                var proxy = new RSACryptoServiceProviderProxy(theoryData.RsaCryptoServiceProvider);
                var signatureProxy = proxy.SignData(theoryData.Input, theoryData.HashAlgorithm);
                var signatureRsa = theoryData.RsaCryptoServiceProvider.SignData(theoryData.Input, theoryData.HashAlgorithm);
                IdentityComparer.AreBytesEqual(signatureProxy, signatureRsa, context);
                if (!proxy.VerifyData(theoryData.Input, theoryData.HashAlgorithm, signatureRsa))
                    context.AddDiff("!proxy.VerifyData(theoryData.Input, theoryData.HashAlgorithm, signatureRsa)");

                if (!theoryData.RsaCryptoServiceProvider.VerifyData(theoryData.Input, theoryData.HashAlgorithm, signatureProxy))
                    context.AddDiff("!theoryData.RsaCryptoServiceProvider.VerifyData(theoryData.Input, theoryData.HashAlgorithm, signatureProxy)");
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);

        }

        public static TheoryData<RSACryptoServiceProviderProxyTheoryData> RSASignVerifyDataTheoryData
        {
            get
            {
#if NET462 || NET472
                var rsaCsp = new RSACryptoServiceProvider();
                rsaCsp.ImportParameters(KeyingMaterial.RsaParameters_2048);
#else
                var rsaCsp = KeyingMaterial.CertSelfSigned2048_SHA256_2.PrivateKey as RSACryptoServiceProvider;
#endif

                var guid = Guid.NewGuid().ToByteArray();
                var hashAlgorithm = SHA256.Create();

                return new TheoryData<RSACryptoServiceProviderProxyTheoryData>
                {
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("rsa"),
                        TestId = "Test1"
                    },
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("input"),
                        RsaCryptoServiceProvider = rsaCsp,
                        TestId = "Test2"
                    },
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("input"),
                        Input = new byte[0],
                        RsaCryptoServiceProvider = rsaCsp,
                        TestId = "Test3"
                    },
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("hash"),
                        Input = guid,
                        RsaCryptoServiceProvider = rsaCsp,
                        TestId = "Test4",
                        UseOAEP = true
                    },
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        HashAlgorithm = hashAlgorithm,
                        Input = guid,
                        RsaCryptoServiceProvider = rsaCsp,
                        TestId = "Test5",
                        UseOAEP = true
                    }
                };
            }
        }

        [Theory, MemberData(nameof(RSAVerifyDataTheoryData), DisableDiscoveryEnumeration = true)]
        public void RSAVerifyData(RSACryptoServiceProviderProxyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RSAVerifyData", theoryData);

            try
            {
                var proxy = new RSACryptoServiceProviderProxy(theoryData.RsaCryptoServiceProvider);
                proxy.VerifyData(theoryData.Input, theoryData.HashAlgorithm, theoryData.Signature);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);

        }

        // just check parameters, SignVerifyData checks Sign parameters and values
        public static TheoryData<RSACryptoServiceProviderProxyTheoryData> RSAVerifyDataTheoryData
        {
            get
            {
#if NET462 || NET472
                var rsaCsp = new RSACryptoServiceProvider();
                rsaCsp.ImportParameters(KeyingMaterial.RsaParameters_2048);
#else
                var rsaCsp = KeyingMaterial.CertSelfSigned1024_SHA256.PrivateKey as RSACryptoServiceProvider;
#endif

                var hashAlgorithm = SHA256.Create();
                return new TheoryData<RSACryptoServiceProviderProxyTheoryData>
                {
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("input"),
                        HashAlgorithm = hashAlgorithm,
                        RsaCryptoServiceProvider = rsaCsp,
                        TestId = "Test1"
                    },
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("input"),
                        HashAlgorithm = hashAlgorithm,
                        Input = new byte[0],
                        RsaCryptoServiceProvider = rsaCsp,
                        TestId = "Test2"
                    },
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("hash"),
                        Input = new byte[1],
                        RsaCryptoServiceProvider = rsaCsp,
                        TestId = "Test3"
                    },
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("signature"),
                        HashAlgorithm = hashAlgorithm,
                        Input = new byte[1],
                        RsaCryptoServiceProvider = rsaCsp,
                        TestId = "Test4"
                    },
                    new RSACryptoServiceProviderProxyTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("signature"),
                        HashAlgorithm = hashAlgorithm,
                        Input = new byte[1],
                        RsaCryptoServiceProvider = rsaCsp,
                        Signature = new byte[0],
                        TestId = "Test5"
                    }
                };
            }
        }
    }

    public class RSACryptoServiceProviderProxyTheoryData : TheoryDataBase
    {
        public HashAlgorithm HashAlgorithm { get; set; }

        public byte[] Input { get; set; }

        public RSACryptoServiceProvider RsaCryptoServiceProvider { get; set; }

        public byte[] Signature { get; set; }

        public bool UseOAEP { get; set; }
    }
}
#endif

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
