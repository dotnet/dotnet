// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Reflection;
using System.Threading;
#if NETFRAMEWORK
using System.Windows.Forms;
#endif

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
            // a fast way to cause stack overflow, takes one method call instead of 9k that you need when calling a method recursively
            Span<byte> s = stackalloc byte[int.MaxValue];
        }

#if NETFRAMEWORK
        [TestMethod]
        public void UITestMethod()
        {
            Clipboard.SetText("Clipboard");
        }

        [TestMethod]
        public void UITestWithSleep1()
        {
            Clipboard.SetText("Clipboard");
            Thread.Sleep(1000 * 3);
        }
#endif
    }
}
