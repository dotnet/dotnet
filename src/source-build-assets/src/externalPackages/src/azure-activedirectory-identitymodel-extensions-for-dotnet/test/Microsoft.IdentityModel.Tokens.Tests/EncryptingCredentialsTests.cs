// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class EncryptingCredentialsTests
    {
        //public EncryptingCredentials(SecurityKey key, string alg, string enc)
        [Theory, MemberData(nameof(ConstructorATheoryData))]
        public void ConstructorA(EncryptingCredentialsTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ConstructorA", theoryData);
            try
            {
                var encryptingCredentials = new EncryptingCredentials(theoryData.Key, theoryData.Alg, theoryData.Enc);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception exception)
            {
                theoryData.ExpectedException.ProcessException(exception, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        //Used in scenarios when a key represents a 'shared' symmetric key
        //public EncryptingCredentials(SecurityKey key, string enc)
        [Theory, MemberData(nameof(ConstructorBTheoryData))]
        public void ConstructorB(EncryptingCredentialsTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ConstructorB", theoryData);
            try
            {
                var encryptingCredentials = new EncryptingCredentials((SymmetricSecurityKey)theoryData.Key, theoryData.Enc);
                //Algorithm value should be 'None'
                IdentityComparer.AreEqual(encryptingCredentials.Alg, SecurityAlgorithms.None, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception exception)
            {
                theoryData.ExpectedException.ProcessException(exception, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<EncryptingCredentialsTheoryData> ConstructorATheoryData()
        {
            return new TheoryData<EncryptingCredentialsTheoryData>
            {
                new EncryptingCredentialsTheoryData
                {
                    Key = null,
                    Alg = SecurityAlgorithms.RsaOAEP,
                    Enc = SecurityAlgorithms.Aes128CbcHmacSha256,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'key'"),
                    TestId = "NullKey"
                },
                new EncryptingCredentialsTheoryData
                {
                    Key = Default.AsymmetricEncryptionKeyPublic,
                    Alg = String.Empty,
                    Enc = SecurityAlgorithms.Aes128CbcHmacSha256,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'alg'"),
                    TestId = "EmptyAlgString"
                },
                new EncryptingCredentialsTheoryData
                {
                    Key = Default.AsymmetricEncryptionKeyPublic,
                    Alg = SecurityAlgorithms.RsaOAEP,
                    Enc = String.Empty,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'enc'"),
                    TestId = "EmptyEncString"
                },
                new EncryptingCredentialsTheoryData
                {
                    Key = Default.AsymmetricEncryptionKeyPublic,
                    Alg = null,
                    Enc = SecurityAlgorithms.Aes128CbcHmacSha256,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'alg'"),
                    TestId = "NullAlgString"
                },
                new EncryptingCredentialsTheoryData
                {
                    Key = Default.AsymmetricEncryptionKeyPublic,
                    Alg = SecurityAlgorithms.RsaOAEP,
                    Enc = null,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'enc'"),
                    TestId = "NullEncString"
                },
                new EncryptingCredentialsTheoryData
                {
                    Key = Default.AsymmetricEncryptionKeyPublic,
                    Alg = SecurityAlgorithms.RsaOAEP,
                    Enc = SecurityAlgorithms.Aes128CbcHmacSha256,
                    TestId = "ValidTest"
                }
            };
        }

        public static TheoryData<EncryptingCredentialsTheoryData> ConstructorBTheoryData()
        {
            return new TheoryData<EncryptingCredentialsTheoryData>
            {
                new EncryptingCredentialsTheoryData
                {
                    Key = null,
                    Enc = SecurityAlgorithms.Aes128CbcHmacSha256,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'key'"),
                    TestId = "NullKey"
                },
                new EncryptingCredentialsTheoryData
                {
                    Key = Default.SymmetricEncryptionKey128,
                    Enc = String.Empty,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'enc'"),
                    TestId = "EmptyEncString"
                },
                new EncryptingCredentialsTheoryData
                {
                    Key = Default.SymmetricEncryptionKey128,
                    Enc = null,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'enc'"),
                    TestId = "NullEncString"
                },
                new EncryptingCredentialsTheoryData
                {
                    Key = Default.SymmetricEncryptionKey128,
                    Enc = SecurityAlgorithms.Aes128CbcHmacSha256,
                    TestId = "ValidTest"
                }
            };
        }
    }

    public class EncryptingCredentialsTheoryData : TheoryDataBase
    {
        public SecurityKey Key { get; set; }
        public string Alg { get; set; }
        public string Enc { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
