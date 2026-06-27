// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace vstest.console.UnitTests.Internal;

/// <summary>
/// Round-trip tests for MSBuildLogger Escape/Unescape.
/// </summary>
[TestClass]
public class MSBuildLoggerEncoderTests
{
    [TestMethod]
    public void FormatMessage_TildeCharsInMessage_SurviveRoundTrip()
    {
        var rawMessage = new string('~', 5);

        var encoded = MSBuildLogger.FormatMessage("test-failed", rawMessage, "TestFile.cs", "42");
        var decoded = DecodeFirstField(encoded);

        Assert.AreEqual(rawMessage, decoded);
    }

    [TestMethod]
    public void FormatMessage_ExclamationCharsInMessage_SurviveRoundTrip()
    {
        var rawMessage = new string('!', 4);

        var encoded = MSBuildLogger.FormatMessage("test-failed", rawMessage, "TestFile.cs", "42");
        var decoded = DecodeFirstField(encoded);

        Assert.AreEqual(rawMessage, decoded);
    }

    [TestMethod]
    public void FormatMessage_NewlinesInMessage_SurviveRoundTrip()
    {
        var rawMessage = "Assert failed.\r\n  at MyTest()";

        var encoded = MSBuildLogger.FormatMessage("test-failed", rawMessage, "TestFile.cs", "42");
        var decoded = DecodeFirstField(encoded);

        Assert.AreEqual(rawMessage, decoded);
    }

    [TestMethod]
    public void FormatMessage_PipesInMessage_SurviveRoundTrip()
    {
        // 4 pipes would have been eaten by the old ||||→____ replacement.
        var rawMessage = "expected: a]||||[b";

        var encoded = MSBuildLogger.FormatMessage("test-failed", rawMessage, "TestFile.cs", "42");
        var decoded = DecodeFirstField(encoded);

        Assert.AreEqual(rawMessage, decoded);
    }

    [TestMethod]
    public void FormatMessage_PercentInMessage_SurviveRoundTrip()
    {
        // The escape char itself must round-trip.
        var rawMessage = "100% done";

        var encoded = MSBuildLogger.FormatMessage("test-failed", rawMessage, "TestFile.cs", "42");
        var decoded = DecodeFirstField(encoded);

        Assert.AreEqual(rawMessage, decoded);
    }

    [TestMethod]
    public void FormatMessage_PercentN_NotConfusedWithNewline()
    {
        // "%n" in user data must not become a newline after round-trip.
        var rawMessage = "url-encoded: %n %r %p";

        var encoded = MSBuildLogger.FormatMessage("test-failed", rawMessage, "TestFile.cs", "42");
        var decoded = DecodeFirstField(encoded);

        Assert.AreEqual(rawMessage, decoded);
    }

    [TestMethod]
    public void FormatMessage_MultipleFields_AllFieldsSurviveRoundTrip()
    {
        var rawMessage = "Assert failed.\r\n  at MyTest()";
        var rawFile = @"C:\src\TestFile.cs";
        var rawLine = "42";

        var encoded = MSBuildLogger.FormatMessage("test-failed", rawMessage, rawFile, rawLine);
        var parts = encoded.Split(new[] { "||||" }, System.StringSplitOptions.None);
        // parts[0] = empty, parts[1] = name, parts[2..] = fields
        var decoded = new[] { MSBuildLogger.Unescape(parts[2]), MSBuildLogger.Unescape(parts[3]), MSBuildLogger.Unescape(parts[4]) };

        Assert.AreEqual(rawMessage, decoded[0]);
        Assert.AreEqual(rawFile, decoded[1]);
        Assert.AreEqual(rawLine, decoded[2]);
    }

    private static string? DecodeFirstField(string encoded)
    {
        var parts = encoded.Split(new[] { "||||" }, System.StringSplitOptions.None);
        return MSBuildLogger.Unescape(parts[2]);
    }
}
