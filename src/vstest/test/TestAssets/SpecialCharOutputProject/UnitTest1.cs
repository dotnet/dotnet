// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpecialCharOutputProject;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestWithSpecialCharOutput()
    {
        // Write invalid XML character U+FFFF to test output. Before the GH-3136 fix,
        // the HTML logger would throw XmlException when processing this character.
        using StreamWriter writer = new StreamWriter(Console.OpenStandardOutput());
        writer.WriteLine("Special: \uFFFE \uFFFF");
    }
}
