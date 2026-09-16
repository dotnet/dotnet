// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Microsoft.SignCheck.Verification;
using Xunit;

namespace Microsoft.DotNet.SignCheckLibrary.Tests;

public class ExclusionsTests
{
    [Theory]
    [InlineData("*.js|!signed.js")]
    [InlineData("!signed.js|*.js")]
    public void PatternExceptionOverridesPositivePatternRegardlessOfOrder(string patterns)
    {
        Exclusions exclusions = CreateDoNotSignExclusions(patterns);

        Assert.False(IsDoNotSign(exclusions, "signed.js"));
        Assert.True(IsDoNotSign(exclusions, "unsigned.js"));
        Assert.False(IsDoNotSign(exclusions, "unsigned.css"));
    }

    [Fact]
    public void MultiplePatternExceptionsAreSupported()
    {
        Exclusions exclusions = CreateDoNotSignExclusions("*.js|!signed.js|!also-signed.js");

        Assert.False(IsDoNotSign(exclusions, "signed.js"));
        Assert.False(IsDoNotSign(exclusions, "also-signed.js"));
        Assert.True(IsDoNotSign(exclusions, "unsigned.js"));
    }

    [Fact]
    public void PatternExceptionCanMatchAPath()
    {
        Exclusions exclusions = CreateDoNotSignExclusions("*.js|!*tools/signed.js");

        Assert.False(IsDoNotSign(exclusions, "/repo/tools/signed.js"));
        Assert.True(IsDoNotSign(exclusions, "/repo/other/signed.js"));
    }

    [Fact]
    public void PatternExceptionRespectsParentScope()
    {
        Exclusions exclusions = CreateDoNotSignExclusions("*.js|!signed.js", "SomePackage.nupkg");

        Assert.False(exclusions.IsDoNotSign("signed.js", "SomePackage.nupkg", null, "signed.js"));
        Assert.True(exclusions.IsDoNotSign("unsigned.js", "SomePackage.nupkg", null, "unsigned.js"));
        Assert.False(exclusions.IsDoNotSign("unsigned.js", "OtherPackage.nupkg", null, "unsigned.js"));
    }

    [Theory]
    [InlineData("!signed.js")]
    [InlineData("!")]
    public void PatternExceptionRequiresPositivePattern(string patterns)
    {
        Assert.Throws<ArgumentException>(() => new Exclusion($"{patterns};;DO-NOT-SIGN"));
    }

    [Fact]
    public void InvalidFileEntryReportsPathLineAndContent()
    {
        string path = Path.GetTempFileName();

        try
        {
            File.WriteAllLines(path, new[]
            {
                "*.dll;;DO-NOT-SIGN",
                "!signed.js;;DO-NOT-SIGN",
            });

            ArgumentException exception = Assert.Throws<ArgumentException>(() => new Exclusions(path));

            Assert.Contains(path, exception.Message);
            Assert.Contains("line 2", exception.Message);
            Assert.Contains("!signed.js;;DO-NOT-SIGN", exception.Message);
        }
        finally
        {
            File.Delete(path);
        }
    }

    private static Exclusions CreateDoNotSignExclusions(string patterns, string parent = "")
    {
        Exclusions exclusions = new Exclusions();
        exclusions.Add(new Exclusion($"{patterns};{parent};DO-NOT-SIGN"));
        return exclusions;
    }

    private static bool IsDoNotSign(Exclusions exclusions, string path)
        => exclusions.IsDoNotSign(path, null, null, null);
}
