// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.CoreUtilities.UnitTests;

[TestClass]
public class TimeSpanParserTests
{
    [TestMethod]
    // core use cases
    [DataRow("5400000")]
    [DataRow("5400000ms")]
    [DataRow("5400s")]
    [DataRow("90m")]
    [DataRow("1.5h")]
    [DataRow("0.0625d")]

    // with space for parsing from xml
    [DataRow("5400000 ms")]
    [DataRow("5400 s")]
    [DataRow("90 m")]
    [DataRow("1.5 h")]
    [DataRow("0.0625 d")]

    // nice to haves
    [DataRow("5400000MS")]
    [DataRow("5400000millisecond")]
    [DataRow("5400000milliseconds")]
    [DataRow("5400000mil")]
    [DataRow("5400000milisecond")]
    [DataRow("5400000miliseconds")]
    [DataRow("5400000mils")]
    [DataRow("5400000millis")]
    [DataRow("5400000millisecs")]
    [DataRow("5400000milisecs")]
    [DataRow("5400S")]
    [DataRow("5400second")]
    [DataRow("5400seconds")]
    [DataRow("5400sec")]
    [DataRow("5400secs")]
    [DataRow("90M")]
    [DataRow("90minute")]
    [DataRow("90minutes")]
    [DataRow("90min")]
    [DataRow("90mins")]
    [DataRow("1.5H")]
    [DataRow("1.5hour")]
    [DataRow("1.5hours")]
    [DataRow("1.5hrs")]
    [DataRow("1.5hr")]
    [DataRow("0.0625D")]
    [DataRow("0.0625day")]
    [DataRow("0.0625days")]
    public void Parses90Minutes(string time)
    {
        Assert.IsTrue(TimeSpanParser.TryParse(time, out var t));
        Assert.AreEqual(TimeSpan.FromMinutes(90), t);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("\n")]
    [DataRow("\t")]
    public void ReturnsEmptyTimeSpanOnNullOrWhiteSpace(string time)
    {
        Assert.IsTrue(TimeSpanParser.TryParse(time, out var t));
        Assert.AreEqual(TimeSpan.Zero, t);
    }

    [TestMethod]
    [DataRow("09808asf")]
    [DataRow("asfsadf")]
    [DataRow("min")]
    [DataRow("ms")]
    [DataRow("1.1.1")]
    public void ReturnsFalseForInvalidInput(string time)
    {
        Assert.IsFalse(TimeSpanParser.TryParse(time, out var _));
    }
}
