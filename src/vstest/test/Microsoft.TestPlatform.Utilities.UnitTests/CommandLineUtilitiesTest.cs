// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Utilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Utilities.Tests;

[TestClass]
public class CommandLineUtilitiesTest
{
    [TestMethod]
    [DataRow("", new string[] { })]
    [DataRow(" /a:b ", new string[] { "/a:b" })]
    [DataRow("""
        /param1
        /param2:value2
        /param3:"value with spaces"
        """, new string[] { "/param1", "/param2:value2", "/param3:value with spaces" })]
    [DataRow("""/param3 #comment""", new string[] { "/param3" })]
    [DataRow("""
        /param3 #comment ends with newline \" \\
        /param4
        """, new string[] { "/param3", "/param4" })]
    [DataRow("""/testadapterpath:"c:\Path" """, new string[] { @"/testadapterpath:c:\Path" })]
    [DataRow("""/testadapterpath:"c:\Path" /logger:"trx" """, new string[] { @"/testadapterpath:c:\Path", "/logger:trx" })]
    [DataRow("""/testadapterpath:"c:\Path" /logger:"trx" /diag:"log.txt" """, new string[] { @"/testadapterpath:c:\Path", "/logger:trx", "/diag:log.txt" })]
    [DataRow("""/Tests:"Test(\"iCT 256\")" """, new string[] { """/Tests:Test("iCT 256")""" })]
    public void VerifyCommandLineSplitter(string input, string[] expected)
    {
        CommandLineUtilities.SplitCommandLineIntoArguments(input, out var actual);

        CollectionAssert.AreEqual(expected, actual);
    }

}
