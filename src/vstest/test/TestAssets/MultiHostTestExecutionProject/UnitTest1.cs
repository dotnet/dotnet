// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// Parallelize the execution
[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]

namespace SerializeTestRunTestProject
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly object Lock = new object();

        private void LogToFile(string testName)
        {
            lock (Lock)
            {
                string folderToLogTo = Environment.GetEnvironmentVariable("VSTEST_LOGFOLDER");
#pragma warning disable CA1837 // Use 'Environment.ProcessId'
                File.AppendAllText(Path.Combine(folderToLogTo, $"TestHost_{Process.GetCurrentProcess().Id}.txt"), testName + "\n");
#pragma warning restore CA1837 // Use 'Environment.ProcessId'
            }
        }

        [TestMethod]
        public void TestMethod1() => LogToFile(nameof(TestMethod1));

        [TestMethod]
        public void TestMethod2() => LogToFile(nameof(TestMethod2));

        [TestMethod]
        public void TestMethod3() => LogToFile(nameof(TestMethod3));

        [TestMethod]
        public void TestMethod4() => LogToFile(nameof(TestMethod4));

        [TestMethod]
        public void TestMethod5() => LogToFile(nameof(TestMethod5));

        [TestMethod]
        public void TestMethod6() => LogToFile(nameof(TestMethod6));

        [TestMethod]
        public void TestMethod7() => LogToFile(nameof(TestMethod7));

        [TestMethod]
        public void TestMethod8() => LogToFile(nameof(TestMethod8));

        [TestMethod]
        public void TestMethod9() => LogToFile(nameof(TestMethod9));

        [TestMethod]
        public void TestMethod10() => LogToFile(nameof(TestMethod10));
    }
}
