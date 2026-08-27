// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace Microsoft.DotNet.ScenarioTests.SdkTemplateTests;

public class DotNetSdkHelperTests
{
    [Fact]
    [Trait("Category", "Unit")]
    public void GetNewArgumentsCanDisableImplicitRestore()
    {
        string arguments = DotNetSdkHelper.GetNewArguments(
            "webapi",
            "WebApiProject",
            "project-directory",
            "C#",
            "--no-https",
            noRestore: true);

        VerifyEqual(
            "new webapi --name WebApiProject --output project-directory --language \"C#\" --no-https --no-restore",
            arguments);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void GetRestoreArgumentsUseDiagnosticVerbosityBinlogAndConfigFile()
    {
        string arguments = DotNetSdkHelper.GetRestoreArguments(
            "/bl:restore.binlog",
            "NuGet.Config");

        VerifyEqual(
            "restore --verbosity diagnostic /bl:restore.binlog --configfile \"NuGet.Config\"",
            arguments);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void SanitizeNuGetSourceOutputRemovesUrlCredentialsAndQuery()
    {
        const string output =
            "Feed [Enabled]\n    https://user:password@example.test:8443/v3/index.json?token=secret";

        string sanitized = DotNetSdkHelper.SanitizeNuGetSourceOutput(output);

        VerifyContains("https://example.test:8443/<redacted-path>", sanitized);
        VerifyDoesNotContain("user", sanitized);
        VerifyDoesNotContain("password", sanitized);
        VerifyDoesNotContain("token", sanitized);
        VerifyDoesNotContain("secret", sanitized);
    }

    private static void VerifyEqual(string expected, string actual)
    {
        if (!string.Equals(expected, actual, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Expected '{expected}', but got '{actual}'.");
        }
    }

    private static void VerifyContains(string expected, string actual)
    {
        if (!actual.Contains(expected, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Expected '{actual}' to contain '{expected}'.");
        }
    }

    private static void VerifyDoesNotContain(string unexpected, string actual)
    {
        if (actual.Contains(unexpected, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Expected '{actual}' not to contain '{unexpected}'.");
        }
    }
}
