// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors.Utilities;

[TestClass]
public class ArgumentProcessorUtilitiesTests
{
    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(";;;;")]
    public void GetArgumentListShouldThrowErrorOnInvalidArgument(string argument)
    {
        try
        {
            ArgumentProcessorUtilities.GetArgumentList(argument, ArgumentProcessorUtilities.SemiColonArgumentSeparator, "test exception.");
        }
        catch (Exception e)
        {
            Assert.IsTrue(e.GetType().Equals(typeof(CommandLineException)));
            Assert.IsTrue(e.Message.Contains("test exception."));
        }
    }

    [TestMethod]
    [DataRow("abc.txt;tracelevel=info;newkey=newvalue")]
    [DataRow(";;;abc.txt;;;tracelevel=info;;;newkey=newvalue;;;;")]
    public void GetArgumentListShouldReturnCorrectArgumentList(string argument)
    {
        var argumentList = ArgumentProcessorUtilities.GetArgumentList(argument, ArgumentProcessorUtilities.SemiColonArgumentSeparator, "test exception.");
        Assert.IsTrue(argumentList.SequenceEqual(new string[] { "abc.txt", "tracelevel=info", "newkey=newvalue" }));
    }

    [TestMethod]
    [DataRow(new string[] { "key1=value1", "invalidPair", "key2=value2" })]
    public void GetArgumentParametersShouldThrowErrorOnInvalidParameters(string[] parameterArgs)
    {
        try
        {
            ArgumentProcessorUtilities.GetArgumentParameters(parameterArgs, ArgumentProcessorUtilities.EqualNameValueSeparator, "test exception.");
        }
        catch (Exception e)
        {
            Assert.IsTrue(e.GetType().Equals(typeof(CommandLineException)));
            Assert.IsTrue(e.Message.Contains("test exception."));
        }
    }

    [TestMethod]
    public void GetArgumentParametersShouldReturnCorrectParameterDictionary()
    {
        var parameterDict = ArgumentProcessorUtilities.GetArgumentParameters(new string[] { "key1=value1", "key2=value2", "key3=value3" }, ArgumentProcessorUtilities.EqualNameValueSeparator, "test exception.");

        var expectedDict = new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" }, { "key3", "value3" } };
        CollectionAssert.AreEqual(parameterDict.OrderBy(kv => kv.Key).ToList(), expectedDict.OrderBy(kv => kv.Key).ToList());
    }
}
