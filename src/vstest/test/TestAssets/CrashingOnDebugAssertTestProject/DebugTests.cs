// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CrashingOnDebugAssertTestProject
{
    // Release profile in this project needs to be the same as 
    // Debug profile, to define TRACE and DEBUG constants and don't
    // optimize the code, otherwise we will not run Debug.Assert in 
    // our acceptance tests on build server, because that is built as Release
    [TestClass]
    public class DebugTests
    {
        [TestMethod]
        public void DebugAssertFailsTheTest()
        {
            Debug.Assert(false);
        }

        [TestMethod]
        public void DebugFailFailsTheTest()
        {
            Debug.Fail("fail");
        }

        [TestMethod]
        public void TraceAssertFailsTheTest()
        {
            Trace.Assert(false);
        }

        [TestMethod]
        public void TraceFailFailsThetest()
        {
            Trace.Fail("fail");
        }

        [TestMethod]
        public void TraceWriteDoesNotFailTheTest()
        {
            Trace.Write("hello");
        }

        [TestMethod]
        public void TraceWriteLineDoesNotFailTheTest()
        {
            Trace.WriteLine("hello");
        }

        [TestMethod]
        public void DebugWriteDoesNotFailTheTest()
        {
            Debug.Write("hello");
        }

        [TestMethod]
        public void DebugWriteLineDoesNotFailTheTest()
        {
            Debug.WriteLine("hello");
        }
    }
}
