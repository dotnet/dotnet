// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackslashParameterTestProject;

[TestClass]
public class BackslashTests
{
    public TestContext TestContext { get; set; } = null!;

    [TestMethod]
    public void ParameterContainsBackslashes()
    {
        // The value passed via CLI run settings should preserve backslashes. When VSTestCLIRunSettings is
        // typed as string[] MSBuild binds it as ITaskItem[], which rewrites \ to / on Unix.
        var pattern = TestContext.Properties["pattern"] as string;

        Assert.IsNotNull(pattern, "The 'pattern' test run parameter did not reach the test host at all.");
        Assert.IsFalse(pattern.Contains("/"),
            $"Backslashes were normalized to forward slashes. Got '{pattern}'");
        // Command-line escaping may double the backslashes, so only check that they survived as backslashes.
        Assert.IsTrue(pattern.Contains("\\"),
            $"Expected backslashes in the value. Got '{pattern}'");
    }
}
