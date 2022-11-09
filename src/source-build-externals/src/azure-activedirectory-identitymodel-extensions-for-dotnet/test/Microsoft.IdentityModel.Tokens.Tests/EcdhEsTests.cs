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
using Microsoft.IdentityModel.TestUtils;
using Xunit;

using KEY = Microsoft.IdentityModel.TestUtils.KeyingMaterial;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
#if NET472 || NET6_0
namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class EcdhEsTests
    {
        [Theory, MemberData(nameof(CreateEcdhEsTestCases))]
        public void EcdhEsKeyExchangeProviderTests(EcdhEsTheoryData theoryData)
        {
            var context = new CompareContext();
            // arrange
            string alg = theoryData.Algorithm;
            string enc = theoryData.Encryption;
            string apuProducer = theoryData.ApuProducer;
            string apvProducer = theoryData.ApvProducer;
            string apuConsumer = theoryData.ApuConsumer;
            string apvConsumer = theoryData.ApvConsumer;

            var aliceKeyExchangeProvider = new EcdhKeyExchangeProvider(theoryData.PrivateKeySender, theoryData.PublicKeyReceiver, alg, enc);
            var bobKeyExchangeProvider = new EcdhKeyExchangeProvider(theoryData.PrivateKeyReceiver, theoryData.PublicKeySender, alg, enc);

            // act
            SecurityKey aliceCek = aliceKeyExchangeProvider.GenerateKdf(apuProducer, apvProducer);
            SecurityKey bobCek = bobKeyExchangeProvider.GenerateKdf(apuConsumer, apvConsumer);

            // assert
            // compare KDFs are the same
            if (theoryData.MatchingKdfs && !Utility.AreEqual(((SymmetricSecurityKey)aliceCek).Key, ((SymmetricSecurityKey)bobCek).Key))
                context.AddDiff($"theoryData.MatchingKdfs && !Utility.AreEqual(aliceCek, bobCek)");
            if (!theoryData.MatchingKdfs && Utility.AreEqual(((SymmetricSecurityKey)aliceCek).Key, ((SymmetricSecurityKey)bobCek).Key))
                context.AddDiff($"!theoryData.MatchingKdfs && Utility.AreEqual(aliceCek, bobCek)");

            TestUtilities.AssertFailIfErrors(context);
        }


        [Theory, MemberData(nameof(CreateEcdhEsTestCases))]
        public void CreateEcdhEsTests(EcdhEsTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateEcdhEsTests", theoryData);
            try
            {
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<EcdhEsTheoryData> CreateEcdhEsTestCases
        {
            get
            {
                TheoryData<EcdhEsTheoryData> theoryData = new TheoryData<EcdhEsTheoryData>();
                theoryData.Add(KeyExchangeEcdhEsA256kwTestPass());
                theoryData.Add(KeyExchangeEcdhEsA256kwTestPassNullApuApv());
                theoryData.Add(KeyExchangeEcdhEsA256kwTestFailDifferentApu());
                theoryData.Add(KeyExchangeEcdhEsA256kwTestFailDifferentApv());
                theoryData.Add(KeyExchangeEcdhEsA256kwTestFailDifferentApuApv());
                return theoryData;
            }
        }

        private static EcdhEsTheoryData KeyExchangeEcdhEsA256kwTestPass() => new EcdhEsTheoryData("KeyExchangeEcdhEsA256kwTestPass")
        {
            First = true,
            Algorithm = SecurityAlgorithms.EcdhEsA256kw,
            Encryption = SecurityAlgorithms.Aes128CbcHmacSha256,
            ApuProducer = KEY.ApuExample1,
            ApvProducer = KEY.ApvExample1,
            ApuConsumer = KEY.ApuExample1,
            ApvConsumer = KEY.ApvExample1,
            PrivateKeySender = new ECDsaSecurityKey(KEY.JsonWebKeyP256, true),
            PublicKeyReceiver = KEY.JsonWebKeyP256_Public,
            PublicKeySender = KEY.JsonWebKeyP256_Public,
            PrivateKeyReceiver = new ECDsaSecurityKey(KEY.JsonWebKeyP256, true),
            MatchingKdfs = true
        };

        private static EcdhEsTheoryData KeyExchangeEcdhEsA256kwTestPassNullApuApv() => new EcdhEsTheoryData("KeyExchangeEcdhEsA256kwTestPassNullApuApv")
        {
            Algorithm = SecurityAlgorithms.EcdhEsA256kw,
            Encryption = SecurityAlgorithms.Aes128CbcHmacSha256,
            ApuProducer = null,
            ApvProducer = null,
            ApuConsumer = null,
            ApvConsumer = null,
            PrivateKeySender = new ECDsaSecurityKey(KEY.JsonWebKeyP256, true),
            PublicKeyReceiver = KEY.JsonWebKeyP256_Public,
            PublicKeySender = KEY.JsonWebKeyP256_Public,
            PrivateKeyReceiver = new ECDsaSecurityKey(KEY.JsonWebKeyP256, true),
            MatchingKdfs = true
        };

        private static EcdhEsTheoryData KeyExchangeEcdhEsA256kwTestFailDifferentApu() => new EcdhEsTheoryData("KeyExchangeEcdhEsA256kwTestFailDifferentApu")
        {
            Algorithm = SecurityAlgorithms.EcdhEsA256kw,
            Encryption = SecurityAlgorithms.Aes128CbcHmacSha256,
            ApuProducer = KEY.ApuExample1,
            ApvProducer = KEY.ApvExample1,
            ApuConsumer = KEY.ApuExample2,
            ApvConsumer = KEY.ApvExample1,
            PrivateKeySender = new ECDsaSecurityKey(KEY.JsonWebKeyP256, true),
            PublicKeyReceiver = KEY.JsonWebKeyP256_Public,
            PublicKeySender = KEY.JsonWebKeyP256_Public,
            PrivateKeyReceiver = new ECDsaSecurityKey(KEY.JsonWebKeyP256, true),
            MatchingKdfs = false
        };

        private static EcdhEsTheoryData KeyExchangeEcdhEsA256kwTestFailDifferentApv() => new EcdhEsTheoryData("KeyExchangeEcdhEsA256kwTestFailDifferentApv")
        {
            Algorithm = SecurityAlgorithms.EcdhEsA256kw,
            Encryption = SecurityAlgorithms.Aes128CbcHmacSha256,
            ApuProducer = KEY.ApuExample1,
            ApvProducer = KEY.ApvExample1,
            ApuConsumer = KEY.ApuExample1,
            ApvConsumer = KEY.ApvExample2,
            PrivateKeySender = new ECDsaSecurityKey(KEY.JsonWebKeyP256, true),
            PublicKeyReceiver = KEY.JsonWebKeyP256_Public,
            PublicKeySender = KEY.JsonWebKeyP256_Public,
            PrivateKeyReceiver = new ECDsaSecurityKey(KEY.JsonWebKeyP256, true),
            MatchingKdfs = false
        };

        private static EcdhEsTheoryData KeyExchangeEcdhEsA256kwTestFailDifferentApuApv() => new EcdhEsTheoryData("KeyExchangeEcdhEsA256kwTestFailDifferentApuApv")
        {
            Algorithm = SecurityAlgorithms.EcdhEsA256kw,
            Encryption = SecurityAlgorithms.Aes128CbcHmacSha256,
            ApuProducer = KEY.ApuExample1,
            ApvProducer = KEY.ApvExample1,
            ApuConsumer = KEY.ApuExample2,
            ApvConsumer = KEY.ApvExample2,
            PrivateKeySender = new ECDsaSecurityKey(KEY.JsonWebKeyP256, true),
            PublicKeyReceiver = KEY.JsonWebKeyP256_Public,
            PublicKeySender = KEY.JsonWebKeyP256_Public,
            PrivateKeyReceiver = new ECDsaSecurityKey(KEY.JsonWebKeyP256, true),
            MatchingKdfs = false
        };

        public class EcdhEsTheoryData : TheoryDataBase
        {
            public EcdhEsTheoryData(string testId)
            {
                TestId = testId;
            }
            public ECDsaSecurityKey PrivateKeySender { get; set; }
            public ECDsaSecurityKey PrivateKeyReceiver { get; set; }
            public JsonWebKey PublicKeyReceiver { get; set; }
            public JsonWebKey PublicKeySender { get; set; }
            public string Algorithm { get; set; }
            public string Encryption { get; set; }
            public string ApuProducer { get; set; }
            public string ApvProducer { get; set; }
            public string ApuConsumer { get; set; }
            public string ApvConsumer { get; set; }
            public bool MatchingKdfs { get; set; }
        }
    }
}
#endif
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
