// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.SettingsMigrator.UnitTests;

[TestClass]
public class ProgramTests
{
    [TestMethod]
    public void MoreThanTwoArgumentsShouldNotBeAccepted()
    {
        int returnCode = Program.Main(new string[] { "asd", "asd", "asd" });
        Assert.AreEqual(1, returnCode, "More than 2 arguments should not be accepted.");
    }

    [TestMethod]
    public void NoArgumentsShouldNotBeAccepted()
    {
        int returnCode = Program.Main(System.Array.Empty<string>());
        Assert.AreEqual(1, returnCode, "No arguments should not be accepted.");
    }
}
