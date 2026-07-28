// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.TestUtils
{
    /// <summary>
    /// Stub for starting out new tests
    /// </summary>
    public class TestStubTests
    {
        [Theory, MemberData(nameof(TestStubTheoryData), DisableDiscoveryEnumeration = true)]
        public void TestStubTest1(TestStubTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.TestStubTest1", theoryData);

            try
            {
                var obj = new object();
                theoryData.ExpectedException.ProcessNoException(context);
                if (theoryData.CompareTo != null)
                    IdentityComparer.AreEqual(obj, theoryData.CompareTo, context);

            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<TestStubTheoryData> TestStubTheoryData
        {
            get
            {
                return new TheoryData<TestStubTheoryData>
                {
                    new TestStubTheoryData
                    {
                        First = true,
                        TestId = "TestStub1"
                    }
                };
            }
        }
    }

    public class TestStubTheoryData : TheoryDataBase
    {
        public object CompareTo { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant

