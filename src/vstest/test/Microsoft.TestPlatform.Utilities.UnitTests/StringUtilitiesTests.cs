// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;

using Castle.Core.Internal;

using Microsoft.VisualStudio.TestPlatform.Utilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Utilities.UnitTests;

[TestClass]
public class StringUtilitiesTests
{
    [TestMethod]
    public void SplitShouldReturnWhenStringisNullOrEmpty()
    {
        var argsList = string.Empty.Tokenize(SplitChar, EscapeChar);

        Assert.IsTrue(argsList.IsNullOrEmpty());
    }

    [TestMethod]
    public void SplitShouldReturnWhenStringDoesntContainSplitChar()
    {
        var data = "foobar";
        var argsList = data.Tokenize(SplitChar, EscapeChar);
        var enumerable = argsList as string[] ?? argsList.ToArray();

        Assert.IsTrue(enumerable.Length == 1);
        Assert.IsTrue(enumerable.First().Equals(data));
    }

    [TestMethod]
    public void SplitShouldSplitWhenStringContainsSplitChar()
    {
        var data = "foo,bar";
        var argsList = data.Tokenize(SplitChar, EscapeChar);
        var enumerable = argsList as string[] ?? argsList.ToArray();

        Assert.IsTrue(enumerable.Length == 2);
    }

    [TestMethod]
    public void SplitShouldSplitWhenStringWithSplitCharStartEnd()
    {
        var data = ",foo,bar,";
        var argsList = data.Tokenize(SplitChar, EscapeChar);
        var enumerable = argsList as string[] ?? argsList.ToArray();

        Assert.IsTrue(enumerable.Length == 4);
    }

    [TestMethod]
    public void SplitShouldEscapeSplitCharWhenEscapedCharPresent()
    {
        var data = "foo\\,bar";
        var argsList = data.Tokenize(SplitChar, EscapeChar);
        var enumerable = argsList as string[] ?? argsList.ToArray();

        Assert.IsTrue(enumerable.Length == 1);
        Assert.IsTrue(enumerable.First().Equals("foo,bar"));
    }

    [TestMethod]
    public void SplitShouldEscapeSplitCharWhenEscapedNonEscapedCharPresent()
    {
        var data = "foo\\,,bar";
        var argsList = data.Tokenize(SplitChar, EscapeChar);
        var enumerable = argsList as string[] ?? argsList.ToArray();
        Assert.IsTrue(enumerable.Length == 2);
        Assert.IsTrue(enumerable.First().Equals("foo,"));
    }

    [TestMethod]
    public void SplitShouldSplitWhenOnlySplitCharPresent()
    {
        var data = ",";
        var argsList = data.Tokenize(SplitChar, EscapeChar);
        var enumerable = argsList as string[] ?? argsList.ToArray();

        Assert.IsTrue(enumerable.Length == 2);
    }

    [TestMethod]
    public void SplitShouldNotSplitWhenNoSplitCharPresent()
    {
        var data = "foo\\bar";
        var argsList = data.Tokenize(SplitChar, EscapeChar);
        var enumerable = argsList as string[] ?? argsList.ToArray();

        Assert.IsTrue(enumerable.Length == 1);
    }

    private const char SplitChar = ',';
    private const char EscapeChar = '\\';
}
