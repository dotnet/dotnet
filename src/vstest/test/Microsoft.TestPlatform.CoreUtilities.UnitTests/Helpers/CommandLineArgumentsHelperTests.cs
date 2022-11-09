// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.CoreUtilities.UnitTests.Helpers;

[TestClass]
public class CommandLineArgumentsHelperTests
{
    [TestMethod]
    public void GetArgumentsDictionaryShouldReturnDictionary()
    {
        var args = new List<string>() { "--port", "12312", "--parentprocessid", "2312", "--testsourcepath", @"C:\temp\1.dll" };
        var argsDictionary = CommandLineArgumentsHelper.GetArgumentsDictionary(args.ToArray());

        Assert.AreEqual("12312", argsDictionary["--port"]);
        Assert.AreEqual("2312", argsDictionary["--parentprocessid"]);
        Assert.AreEqual(@"C:\temp\1.dll", argsDictionary["--testsourcepath"]);
    }

    [TestMethod]
    public void GetArgumentsDictionaryShouldIgnoreValuesWithoutPreceedingHypen()
    {
        var args = new List<string>() { "port", "12312", "--parentprocessid", "2312", "--testsourcepath", @"C:\temp\1.dll" };
        var argsDictionary = CommandLineArgumentsHelper.GetArgumentsDictionary(args.ToArray());

        Assert.IsTrue(argsDictionary.Count == 2);
        Assert.AreEqual("2312", argsDictionary["--parentprocessid"]);
        Assert.AreEqual(@"C:\temp\1.dll", argsDictionary["--testsourcepath"]);

        args = new List<string>() { "--port", "12312", "--parentprocessid", "2312", "testsourcepath", @"C:\temp\1.dll" };
        argsDictionary = CommandLineArgumentsHelper.GetArgumentsDictionary(args.ToArray());

        Assert.IsTrue(argsDictionary.Count == 2);
        Assert.AreEqual("12312", argsDictionary["--port"]);
        Assert.AreEqual("2312", argsDictionary["--parentprocessid"]);
    }

    [TestMethod]
    public void GetStringArgFromDictShouldReturnStringValueOrEmpty()
    {
        var args = new List<string>() { "--port", "12312", "--parentprocessid", "2312", "--testsourcepath", @"C:\temp\1.dll" };
        var argsDictionary = CommandLineArgumentsHelper.GetArgumentsDictionary(args.ToArray());

        string? data = CommandLineArgumentsHelper.GetStringArgFromDict(argsDictionary, "--port");
        Assert.AreEqual("12312", data);
    }

    [TestMethod]
    public void GetStringArgFromDictShouldReturnNullIfValueIsNotPresent()
    {
        var args = new List<string>() { "--hello", "--world" };
        var argsDictionary = CommandLineArgumentsHelper.GetArgumentsDictionary(args.ToArray());

        string? data = CommandLineArgumentsHelper.GetStringArgFromDict(argsDictionary, "--hello");

        Assert.IsTrue(argsDictionary.Count == 2);
        Assert.IsNull(data);
    }

    [TestMethod]
    public void GetStringArgFromDictShouldReturnEmptyStringIfKeyIsNotPresent()
    {
        var args = new List<string>() { "--hello", "--world" };
        var argsDictionary = CommandLineArgumentsHelper.GetArgumentsDictionary(args.ToArray());

        string? data = CommandLineArgumentsHelper.GetStringArgFromDict(argsDictionary, "--port");

        Assert.IsTrue(argsDictionary.Count == 2);
        Assert.AreEqual(string.Empty, data);
    }

    [TestMethod]
    public void GetArgumentsDictionaryShouldReturnEmptyDictionaryIfEmptyArgIsPassed()
    {
        var argsDictionary = CommandLineArgumentsHelper.GetArgumentsDictionary(null);
        Assert.IsTrue(argsDictionary.Count == 0);

        argsDictionary = CommandLineArgumentsHelper.GetArgumentsDictionary(System.Array.Empty<string>());
        Assert.IsTrue(argsDictionary.Count == 0);
    }

    [TestMethod]
    public void GetArgumentsDictionaryShouldTreatValueAsNullIfTwoConsecutiveKeysArePassed()
    {
        var args = new List<string>() { "--hello", "--world" };
        var argsDictionary = CommandLineArgumentsHelper.GetArgumentsDictionary(args.ToArray());

        Assert.IsTrue(argsDictionary.Count == 2);
        Assert.IsNull(argsDictionary["--hello"]);
        Assert.IsNull(argsDictionary["--world"]);
    }

    [TestMethod]
    public void GetIntArgFromDictShouldReturnZeroIfKeyIsNotPresent()
    {
        var args = new List<string>() { "--hello", "--world" };
        var argsDictionary = CommandLineArgumentsHelper.GetArgumentsDictionary(args.ToArray());

        int data = CommandLineArgumentsHelper.GetIntArgFromDict(argsDictionary, "--port");

        Assert.AreEqual(0, data);
    }

    [TestMethod]
    public void GetIntArgFromDictShouldReturnTheValueIfKeyIsPresent()
    {
        var args = new List<string>() { "--port", "1000" };
        var argsDictionary = CommandLineArgumentsHelper.GetArgumentsDictionary(args.ToArray());

        int data = CommandLineArgumentsHelper.GetIntArgFromDict(argsDictionary, "--port");

        Assert.AreEqual(1000, data);
    }

    [TestMethod]
    public void TryGetIntArgFromDictShouldReturnTrueIfKeyIsPresentAndTheValue()
    {
        var args = new List<string>() { "--port", "59870" };
        var argsDictionary = CommandLineArgumentsHelper.GetArgumentsDictionary(args.ToArray());

        bool found = CommandLineArgumentsHelper.TryGetIntArgFromDict(argsDictionary, "--port", out var data);

        Assert.IsTrue(found);
        Assert.AreEqual(59870, data);
    }

    [TestMethod]
    public void TryGetIntArgFromDictShouldReturnFalseIfKeyIsNotPresent()
    {
        var args = new List<string>() { "--hello", "--world" };
        var argsDictionary = CommandLineArgumentsHelper.GetArgumentsDictionary(args.ToArray());
        bool found = CommandLineArgumentsHelper.TryGetIntArgFromDict(argsDictionary, "--port", out _);

        Assert.IsFalse(found);
    }
}
