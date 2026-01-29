// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
#if !NETFRAMEWORK
using System.Runtime.Loader;
#else
using System.Reflection;
#endif
using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
[TestCategory("Windows-Review")]
public class AppDomainTests : AcceptanceTestBase
{
    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    public void RunTestExecutionWithDisableAppDomain(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testAppDomainDetailFileName = Path.Combine(TempDirectory.Path, "appdomain_test.txt");
        var dataCollectorAppDomainDetailFileName = Path.Combine(TempDirectory.Path, "appdomain_datacollector.txt");

        // Delete test output files if already exist
        File.Delete(testAppDomainDetailFileName);
        File.Delete(dataCollectorAppDomainDetailFileName);

        var runsettingsFilePath = GetInProcDataCollectionRunsettingsFile(true, TempDirectory);
        var arguments = PrepareArguments(
            GetSampleTestAssembly(),
            GetTestAdapterPath(),
            runsettingsFilePath,
            FrameworkArgValue,
            runnerInfo.InIsolationValue,
            TempDirectory.Path);

        // Sets the environment variables used by the test project and test data collector.
        var env = new Dictionary<string, string?>
        {
            ["TEST_ASSET_APPDOMAIN_TEST_PATH"] = testAppDomainDetailFileName,
            ["TEST_ASSET_APPDOMAIN_COLLECTOR_PATH"] = dataCollectorAppDomainDetailFileName,
        };

        InvokeVsTest(arguments, env);

        Assert.IsTrue(
            IsFilesContentEqual(testAppDomainDetailFileName, dataCollectorAppDomainDetailFileName),
            "Different AppDomains, test: {0} datacollector: {1}",
            File.ReadAllText(testAppDomainDetailFileName),
            File.ReadAllText(dataCollectorAppDomainDetailFileName));
        ValidateSummaryStatus(1, 1, 1);
        File.Delete(runsettingsFilePath);
    }

    private static bool IsFilesContentEqual(string filePath1, string filePath2)
    {
        Assert.IsTrue(File.Exists(filePath1), "File doesn't exist: {0}.", filePath1);
        Assert.IsTrue(File.Exists(filePath2), "File doesn't exist: {0}.", filePath2);
        var content1 = File.ReadAllText(filePath1);
        var content2 = File.ReadAllText(filePath2);
        Assert.IsTrue(string.Equals(content1, content2, StringComparison.Ordinal), "Content mismatch{2}- file1 content:{2}{0}{2}- file2 content:{2}{1}{2}", content1, content2, Environment.NewLine);
        return string.Equals(content1, content2, StringComparison.Ordinal);
    }

    private string GetInProcDataCollectionRunsettingsFile(bool disableAppDomain, TempDirectory tempDirectory)
    {
        var runSettings = Path.Combine(tempDirectory.Path, "test_" + Guid.NewGuid() + ".runsettings");
        var inprocasm = _testEnvironment.GetTestAsset("SimpleDataCollector.dll", "netstandard2.0");
#if !NETFRAMEWORK
        var assemblyName = AssemblyLoadContext.GetAssemblyName(inprocasm);
#else
        var assemblyName = AssemblyName.GetAssemblyName(inprocasm);
#endif
        var fileContents = @"<RunSettings>
                                    <InProcDataCollectionRunSettings>
                                        <InProcDataCollectors>
                                            <InProcDataCollector friendlyName='Test Impact' uri='InProcDataCollector://Microsoft/TestImpact/1.0' assemblyQualifiedName='SimpleDataCollector.SimpleDataCollector, {0}'  codebase='{1}'>
                                                <Configuration>
                                                    <Port>4312</Port>
                                                </Configuration>
                                            </InProcDataCollector>
                                        </InProcDataCollectors>
                                    </InProcDataCollectionRunSettings>
                                    <RunConfiguration>
                                       <DisableAppDomain>" + disableAppDomain + @"</DisableAppDomain>
                                    </RunConfiguration>
                                </RunSettings>";

        fileContents = string.Format(CultureInfo.CurrentCulture, fileContents, assemblyName, inprocasm);
        File.WriteAllText(runSettings, fileContents);

        return runSettings;
    }
}
