// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Threading;

namespace DiscoveryTestProject3
{
    [TestClass]
    public class LongDiscoveryTestClass
    {
        // This is for discovery cancellation test.
        // LongDiscoveryTestMethod has attribute attribute that
        // takes a very long time to create, which prolongs the
        // discovery time and keeps us discovering while we
        // are cancelling the discovery from the CancelTestDiscovery test.

        [TestMethodWithDelay]
        public void LongDiscoveryTestMethod()
        {

        }
    }

    internal class TestMethodWithDelayAttribute : TestMethodAttribute
    {
        public TestMethodWithDelayAttribute()
        {
            // This will be multiplied by 3 because the framework will internally create this
            // attribute 3 times.
            Thread.Sleep(1000);
        }
    }
}
