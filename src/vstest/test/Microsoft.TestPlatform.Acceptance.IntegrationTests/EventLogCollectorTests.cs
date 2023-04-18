// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
[TestCategory("Windows-Review")]
public class EventLogCollectorTests : AcceptanceTestBase
{
    // Fails randomly https://ci.dot.net/job/Microsoft_vstest/job/master/job/Windows_NT_Release_prtest/2084/console
    // https://ci.dot.net/job/Microsoft_vstest/job/master/job/Windows_NT_Debug_prtest/2085/console
    [Ignore]
    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    public void EventLogDataCollectorShoudCreateLogFileHavingEvents(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = _testEnvironment.GetTestAsset("EventLogUnitTestProject.dll");

        string runSettings = GetRunsettingsFilePath(TempDirectory);
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), runSettings, FrameworkArgValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);

        ValidateSummaryStatus(3, 0, 0);
        VaildateDataCollectorOutput(TempDirectory);
        StdOutputDoesNotContains("An exception occurred while collecting final entries from the event log");
        StdErrorDoesNotContains("event log has encountered an exception, some events might get lost");
        StdOutputDoesNotContains("event log may have been cleared during collection; some events may not have been collected");
        StdErrorDoesNotContains("Unable to read event log");
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    public void EventLogDataCollectorShoudCreateLogFileWithoutEventsIfEventsAreNotLogged(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = _testEnvironment.GetTestAsset("SimpleTestProject.dll");

        string runSettings = GetRunsettingsFilePath(TempDirectory);
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), runSettings, FrameworkArgValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);

        ValidateSummaryStatus(1, 1, 1);
        StdOutputDoesNotContains("An exception occurred while collecting final entries from the event log");
        StdErrorDoesNotContains("event log has encountered an exception, some events might get lost");
        StdOutputDoesNotContains("event log may have been cleared during collection; some events may not have been collected");
        StdErrorDoesNotContains("Unable to read event log");
    }

    private static string GetRunsettingsFilePath(TempDirectory tempDirectory)
    {
        var runsettingsPath = Path.Combine(tempDirectory.Path, "test_" + Guid.NewGuid() + ".runsettings");

        string runSettingsXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <RunSettings>
      <RunConfiguration>
        <MaxCpuCount>0</MaxCpuCount>
        <TargetPlatform> x64 </TargetPlatform>
        <TargetFrameworkVersion> Framework45 </TargetFrameworkVersion>
      </RunConfiguration>
      <DataCollectionRunSettings>
        <DataCollectors>
            <DataCollector friendlyName=""Event Log"" >
                <Configuration><Setting name = ""EventLogs"" value = ""Application,System"" /><Setting name=""EntryTypes"" value=""Error,Warning"" /></Configuration>
            </DataCollector>
        </DataCollectors>
      </DataCollectionRunSettings>
    </RunSettings> ";

        File.WriteAllText(runsettingsPath, runSettingsXml);
        return runsettingsPath;
    }

    private void VaildateDataCollectorOutput(TempDirectory tempDirectory)
    {
        // Verify attachments
        var di = new DirectoryInfo(tempDirectory.Path);
        var resultFiles = di.EnumerateFiles("Event Log.xml", SearchOption.AllDirectories)
            .OrderBy(d => d.CreationTime)
            .Select(d => d.FullName)
            .ToList();

        Assert.AreEqual(4, resultFiles.Count);
        StdOutputContains("Event Log.xml");

        var fileContent1 = File.ReadAllText(resultFiles[0]);
        var fileContent2 = File.ReadAllText(resultFiles[1]);
        var fileContent3 = File.ReadAllText(resultFiles[2]);
        var fileContent4 = File.ReadAllText(resultFiles[3]);

        var eventIdsDics = new Dictionary<string[], bool>
        {
            { new[] { "110", "111", "112" }, false },
            { new[] { "220", "221", "222", "223" }, false },
            { new[] { "330", "331", "332" }, false }
        };

        // Since there is no guaranty that test will run in a particular order, we will check file for all available list of ids
        Assert.IsTrue(VerifyOrder2(fileContent1, eventIdsDics), $"Event log file content: {fileContent1}");
        Assert.IsTrue(VerifyOrder2(fileContent2, eventIdsDics), $"Event log file content: {fileContent2}");
        Assert.IsTrue(VerifyOrder2(fileContent3, eventIdsDics), $"Event log file content: {fileContent3}");

        Assert.IsTrue(VerifyOrder(fileContent4, new[] { "110", "111", "112", "220", "221", "222", "223", "330", "331", "332" }), $"Event log file content: {fileContent4}");
    }

    private static bool VerifyOrder2(string content, Dictionary<string[], bool> eventIdsDics)
    {
        foreach (var eventIds in eventIdsDics)
        {
            if (eventIds.Value == false)
            {
                if (VerifyOrder(content, eventIds.Key))
                {
                    eventIdsDics[eventIds.Key] = true;
                    return true;
                }
            }
        }
        return false;
    }

    private static bool VerifyOrder(string content, string[] eventIds)
    {
        for (int i = 0; i < eventIds.Length; i++)
        {
            int currentIndex = 0;
            currentIndex = content.IndexOf(eventIds[i], currentIndex);
            if (currentIndex == -1)
            {
                return false;
            }
        }

        return true;
    }
}
