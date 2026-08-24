// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Xml;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Xml.Tests
{
    public class TransformFactoryTest
    {
        [Theory, MemberData(nameof(GetTransformTestTheoryData), DisableDiscoveryEnumeration = true)]
        public void GetTransformTest(TransformTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.GetTransformTest", theoryData);
            var transformFactory = new TransformFactory();
            try
            {
                transformFactory.GetTransform(theoryData.Algorithm);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<TransformTheoryData> GetTransformTestTheoryData()
        {
            return new TheoryData<TransformTheoryData>()
            {
                new TransformTheoryData
                {
                    TestId = "Unsupported transform",
                    Algorithm = "Unsupported",
                    ExpectedException = ExpectedException.NotSupportedException("IDX30210:")
                },
                new TransformTheoryData
                {
                    TestId = "Supported transform: EnvelopedSignature",
                    Algorithm = SecurityAlgorithms.EnvelopedSignature
                }
            };
        }

        [Theory, MemberData(nameof(GetCanonicalizingTransformTestTheoryData), DisableDiscoveryEnumeration = true)]
        public void GetCanonicalizingTransformTest(TransformTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.GetCanonicalizingTransformTest", theoryData);
            var transformFactory = new TransformFactory();
            try
            {
                transformFactory.GetCanonicalizingTransform(theoryData.Algorithm);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<TransformTheoryData> GetCanonicalizingTransformTestTheoryData()
        {
            return new TheoryData<TransformTheoryData>()
            {
                new TransformTheoryData
                {
                    TestId = "Unsupported transform",
                    Algorithm = "Unsupported",
                    ExpectedException = ExpectedException.NotSupportedException("IDX30211:")
                },
                new TransformTheoryData
                {
                    TestId = "Supported transform: ExclusiveC14nWithComments",
                    Algorithm = SecurityAlgorithms.ExclusiveC14nWithComments
                },
                new TransformTheoryData
                {
                    TestId = "Supported transform: ExclusiveC14n",
                    Algorithm = SecurityAlgorithms.ExclusiveC14n
                }
            };
        }
    }

    public class TransformTheoryData : TheoryDataBase
    {
        public string Algorithm { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
