// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.IO;

using Microsoft.TestPlatform.TestUtilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.SmokeTests;

[TestClass]
public class DataCollectorTests : IntegrationTestBase
{
    private static readonly string InProcTestResultFile = Path.Combine(Path.GetTempPath(), "inproctest.txt");
    private const string InProDataCollectorTestProject = "SimpleTestProject.dll";
    [TestMethod]
    public void RunAllWithInProcDataCollectorSettings()
    {
        // Delete File if already exists
        File.Delete(InProcTestResultFile);

        var runSettings = GetInProcDataCollectionRunsettingsFile();

        InvokeVsTestForExecution(_testEnvironment.GetTestAsset(InProDataCollectorTestProject), GetTestAdapterPath(), ".NETFramework,Version=v4.5.1", runSettings);
        ValidateSummaryStatus(1, 1, 1);

        ValidateInProcDataCollectionOutput();
    }

    private static void ValidateInProcDataCollectionOutput()
    {
        Assert.IsTrue(File.Exists(InProcTestResultFile), "Datacollector test file doesn't exist: {0}.", InProcTestResultFile);
        var actual = File.ReadAllText(InProcTestResultFile);
        var expected = @"TestSessionStart : <Configuration><Port>4312</Port></Configuration> TestCaseStart : PassingTest TestCaseEnd : PassingTest TestCaseStart : FailingTest TestCaseEnd : FailingTest TestCaseStart : SkippingTest TestCaseEnd : SkippingTest TestSessionEnd";
        actual = actual.Replace(" ", string.Empty).Replace("\r\n", string.Empty);
        expected = expected.Replace(" ", string.Empty).Replace("\r\n", string.Empty);
        Assert.AreEqual(expected, actual);
    }

    private string GetInProcDataCollectionRunsettingsFile()
    {
        var runSettings = Path.Combine(Path.GetDirectoryName(_testEnvironment.GetTestAsset(InProDataCollectorTestProject))!, "runsettingstest.runsettings");
        var inprocasm = _testEnvironment.GetTestAsset("SimpleDataCollector.dll");
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
                                </RunSettings>";

        fileContents = string.Format(CultureInfo.CurrentCulture, fileContents, AssemblyUtility.GetAssemblyName(inprocasm), inprocasm);
        File.WriteAllText(runSettings, fileContents);

        return runSettings;
    }
}
