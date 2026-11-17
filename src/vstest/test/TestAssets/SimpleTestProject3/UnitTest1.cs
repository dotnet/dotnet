// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SampleUnitTestProject3
{

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void WorkingDirectoryTest()
        {
            Assert.AreEqual(Path.GetDirectoryName(typeof(UnitTest1).GetTypeInfo().Assembly.Location), Directory.GetCurrentDirectory());
        }

        [TestMethod]
        public void ExitwithUnhandleException()
        {
            Action fail = () => throw new InvalidOperationException();
            var thread = new Thread(new ThreadStart(fail));
            thread.Start();
            thread.Join();
        }

        [TestMethod]
        public void ExitWithStackoverFlow()
        {
            // Recurse until the stack is exhausted. One huge stackalloc is quicker, but on threads other than the main one it can
            // end the process with an access violation before the runtime reports "Stack overflow." or writes a crash dump.
            Recurse(0);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int Recurse(int depth) => Recurse(depth + 1) + 1;

#if NETFRAMEWORK
        [TestMethod]
        public void UITestMethod()
        {
            Assert.AreEqual(ApartmentState.STA, Thread.CurrentThread.GetApartmentState());
        }

        [TestMethod]
        public void UITestWithSleep1()
        {
            Assert.AreEqual(ApartmentState.STA, Thread.CurrentThread.GetApartmentState());
            Thread.Sleep(1000 * 3);
        }
#endif
    }
}
