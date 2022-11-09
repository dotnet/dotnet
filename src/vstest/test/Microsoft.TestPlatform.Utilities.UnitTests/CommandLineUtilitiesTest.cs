// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Utilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Utilities.Tests;

[TestClass]
public class CommandLineUtilitiesTest
{
    private static void VerifyCommandLineSplitter(string commandLine, string[] expected)
    {
        CommandLineUtilities.SplitCommandLineIntoArguments(commandLine, out var actual);

        Assert.AreEqual(expected.Length, actual.Length);
        for (int i = 0; i < actual.Length; ++i)
        {
            Assert.AreEqual(expected[i], actual[i]);
        }
    }

    [TestMethod]
    public void TestCommandLineSplitter()
    {
        VerifyCommandLineSplitter("", System.Array.Empty<string>());
        VerifyCommandLineSplitter("/testadapterpath:\"c:\\Path\"", new[] { @"/testadapterpath:c:\Path" });
        VerifyCommandLineSplitter("/testadapterpath:\"c:\\Path\" /logger:\"trx\"", new[] { @"/testadapterpath:c:\Path", "/logger:trx" });
        VerifyCommandLineSplitter("/testadapterpath:\"c:\\Path\" /logger:\"trx\" /diag:\"log.txt\"", new[] { @"/testadapterpath:c:\Path", "/logger:trx", "/diag:log.txt" });
    }
}
