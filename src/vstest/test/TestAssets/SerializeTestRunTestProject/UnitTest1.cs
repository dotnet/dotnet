// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// Parallelize the execution
[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]

namespace SerializeTestRunTestProject
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly object ObjectToAcquire = new object();

        private static void AcquireAndReleaseLock()
        {
            Assert.IsTrue(Monitor.TryEnter(ObjectToAcquire));
            Thread.Sleep(100);
            Monitor.Exit(ObjectToAcquire);
        }

        [TestMethod]
        public void TestMethod1() => AcquireAndReleaseLock();

        [TestMethod]
        public void TestMethod2() => AcquireAndReleaseLock();

        [TestMethod]
        public void TestMethod3() => AcquireAndReleaseLock();

        [TestMethod]
        public void TestMethod4() => AcquireAndReleaseLock();

        [TestMethod]
        public void TestMethod5() => AcquireAndReleaseLock();

        [TestMethod]
        public void TestMethod6() => AcquireAndReleaseLock();

        [TestMethod]
        public void TestMethod7() => AcquireAndReleaseLock();

        [TestMethod]
        public void TestMethod8() => AcquireAndReleaseLock();

        [TestMethod]
        public void TestMethod9() => AcquireAndReleaseLock();

        [TestMethod]
        public void TestMethod10() => AcquireAndReleaseLock();
    }
}
