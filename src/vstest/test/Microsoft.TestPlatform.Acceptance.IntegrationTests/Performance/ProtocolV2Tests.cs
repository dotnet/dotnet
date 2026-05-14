// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

# if NETFRAMEWORK
using System;
using System.Diagnostics;

using FluentAssertions;
using FluentAssertions.Extensions;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestResult = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;

namespace Microsoft.TestPlatform.AcceptanceTests.Performance;

[TestClass]
[DoNotParallelize]
[Ignore("The timing can vary significantly based on the system running the test. Convert them to report the results and not fail.")]
public class ProtocolV2Tests
{
    private static readonly TestCase TestCase = new(
        "sampleTestClass.sampleTestCase",
        new Uri("executor://sampleTestExecutor"),
        "sampleTest.dll")
    {
        CodeFilePath = "/user/src/testFile.cs",
        DisplayName = "sampleTestCase",
        Id = new Guid("be78d6fc-61b0-4882-9d07-40d796fd96ce"),
        LineNumber = 999,
        Traits = { new Trait("Priority", "0"), new Trait("Category", "unit") }
    };

    private static readonly DateTimeOffset StartTime = new(new DateTime(2007, 3, 10, 0, 0, 0, DateTimeKind.Utc));

    private static readonly TestResult TestResult = new(TestCase)
    {
        // Attachments = ?
        // Messages = ?
        Outcome = TestOutcome.Passed,
        ErrorMessage = "sampleError",
        ErrorStackTrace = "sampleStackTrace",
        DisplayName = "sampleTestResult",
        ComputerName = "sampleComputerName",
        Duration = TimeSpan.MaxValue,
        StartTime = StartTime,
        EndTime = DateTimeOffset.MaxValue
    };

    [TestMethod]
    public void TestCaseSerialize2()
    {
        SerializeV2(TestCase);
        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < 10000; i++)
        {
            SerializeV2(TestCase);
        }
        sw.Stop();

        var actualDuration = sw.Elapsed;
        actualDuration.Should().BeLessThanOrEqualTo(2.Seconds(), $"when serializing 10k test cases");
    }

    [TestMethod]
    public void TestCaseDeserialize2()
    {
        var json = SerializeV2(TestCase);
        DeserializeV2<TestCase>(json);

        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < 10000; i++)
        {
            DeserializeV2<TestCase>(json);
        }
        sw.Stop();

        var actualDuration = sw.Elapsed;
        actualDuration.Should().BeLessThanOrEqualTo(2.Seconds(), $"when de-serializing 10k test cases");
    }

    [TestMethod]
    public void TestResultSerialize2()
    {
        SerializeV2(TestResult);
        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < 10000; i++)
        {
            SerializeV2(TestResult);
        }
        sw.Stop();

        var actualDuration = sw.Elapsed;
        actualDuration.Should().BeLessThanOrEqualTo(2.Seconds(), $"when serializing 10k test results");
    }

    [TestMethod]
    public void TestResultDeserialize2()
    {
        var json = SerializeV2(TestResult);
        DeserializeV2<TestResult>(json);

        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < 10000; i++)
        {
            DeserializeV2<TestResult>(json);
        }
        sw.Stop();

        var actualDuration = sw.Elapsed;
        actualDuration.Should().BeLessThanOrEqualTo(2.Seconds(), $"when de-serializing 10k test results");
    }

    private static string SerializeV2<T>(T data)
    {
        return JsonDataSerializer.Instance.Serialize(data, version: 2);
    }

    private static T DeserializeV2<T>(string json)
    {
        return JsonDataSerializer.Instance.Deserialize<T>(json, version: 2)!;
    }
}
#endif
