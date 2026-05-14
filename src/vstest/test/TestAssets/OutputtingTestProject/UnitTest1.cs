// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;

namespace OutputtingTestProject;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestThatWritesOutput()
    {
        // This is a trick to bypass the console output capturing that MSTest does, and write directly
        // to console output. The same bypass can be configured by <MSTest><CaptureTraceOutput>false</CaptureTraceOutput></MSTest>
        // in MSTest 3.3.2 and newer.
        using StreamWriter writer = new StreamWriter(Console.OpenStandardOutput());
        writer.WriteLine("MY OUTPUT FROM TEST");
    }
}
