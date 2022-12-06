// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionDecorators;
using Microsoft.VisualStudio.TestPlatform.Common.Resources;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.Common.UnitTests.ExtensionFramework;

[TestClass]
public class ExtensionDecoratorTests
{
    private readonly Mock<IFeatureFlag> _featureFlagMock = new();
    private readonly Mock<ITestExecutor2> _testExecutorMock = new();
    private readonly Mock<IRunContext> _contextMock = new();
    private readonly Mock<IFrameworkHandle> _frameworkWorkHandleMock = new();
    private readonly Mock<IRunSettings> _settingsMock = new();
    private readonly string _runsettings = @"
    <RunSettings>
        <RunConfiguration>
            <ForceOneTestAtTimePerTestHost>true</ForceOneTestAtTimePerTestHost>
        </RunConfiguration>
    </RunSettings>";

    [TestMethod]
    public void ExtensionDecoratorFactory_DisabledByFlag()
    {
        // Arrange
        _featureFlagMock.Setup(x => x.IsSet(FeatureFlag.DISABLE_SERIALTESTRUN_DECORATOR)).Returns(true);

        // Run test and assert
        ExtensionDecoratorFactory extensionDecoratorFactory = new(_featureFlagMock.Object);
        Mock<ITestExecutor> featureFlagMock = new();
        Assert.AreEqual(featureFlagMock.Object, extensionDecoratorFactory.Decorate(featureFlagMock.Object));
    }

    [TestMethod]
    public void SerialTestRunDecorator_ShouldSerializeTests()
    {
        // Arrange
        List<TestCase> testCases = new();
        for (int i = 0; i < 50; i++)
        {
            testCases.Add(new TestCase() { Id = Guid.NewGuid() });
        }

        long currentCount = 0;
        List<TestCase> testCasesRan = new();
        _settingsMock.Setup(x => x.SettingsXml).Returns(_runsettings);
        _contextMock.Setup(x => x.RunSettings).Returns(_settingsMock.Object);
        _testExecutorMock.Setup(x => x.RunTests(It.IsAny<IEnumerable<TestCase>?>(), It.IsAny<IRunContext?>(), It.IsAny<IFrameworkHandle?>()))
        .Callback((IEnumerable<TestCase>? tests, IRunContext? runContext, IFrameworkHandle? frameworkHandle) =>
        {
            Assert.AreEqual(0, Interlocked.Read(ref currentCount));
            currentCount = Interlocked.Increment(ref currentCount);
            TestCase tc = tests!.First();
            Task.Run(() =>
            {
                Thread.Sleep(100);
                currentCount = Interlocked.Decrement(ref currentCount);
                frameworkHandle!.RecordEnd(tc, TestOutcome.Passed);
            });
            testCasesRan.Add(tc);
        });

        // Run test
        SerialTestRunDecorator serialTestRunDecorator = new(_testExecutorMock.Object);
        serialTestRunDecorator.RunTests(testCases, _contextMock.Object, _frameworkWorkHandleMock.Object);

        // Assert
        Assert.AreEqual(0, testCases.Except(testCasesRan).Count());
    }

    [TestMethod]
    public void SerialTestRunDecorator_DoesNotSupportSources()
    {
        // Arrange
        _settingsMock.Setup(x => x.SettingsXml).Returns(_runsettings);
        _contextMock.Setup(x => x.RunSettings).Returns(_settingsMock.Object);

        // Run test
        SerialTestRunDecorator serialTestRunDecorator = new(_testExecutorMock.Object);
        serialTestRunDecorator.RunTests(new List<string>() { "samplesource.dll" }, _contextMock.Object, _frameworkWorkHandleMock.Object);

        // Assert
        _testExecutorMock.Verify(x => x.RunTests(It.IsAny<IEnumerable<string>?>(), It.IsAny<IRunContext?>(), It.IsAny<IFrameworkHandle?>()), Times.Never());
        _frameworkWorkHandleMock.Verify(x => x.SendMessage(TestMessageLevel.Error, Resources.SerialTestRunInvalidScenario), Times.Once());
    }


    [TestMethod]
    [DataRow("FaLsE", false)]
    [DataRow("false", false)]
    [DataRow("FALSE", false)]
    [DataRow(null, true)]
    public void SerialTestRunDecorator_Disabled(string falseValue, bool nullRunSettings)
    {
        // Arrange
        string runsettings = $@"
        <RunSettings>
            <RunConfiguration>
                <ForceOneTestAtTimePerTestHost>{falseValue}</ForceOneTestAtTimePerTestHost>
            </RunConfiguration>
        </RunSettings>";

        List<TestCase> testCases = new();
        for (int i = 0; i < 50; i++)
        {
            testCases.Add(new TestCase() { Id = Guid.NewGuid() });
        }

        string[] sourcesName = new string[] { "testSource.dll" };

        _settingsMock.Setup(x => x.SettingsXml).Returns(nullRunSettings ? null : runsettings);
        _contextMock.Setup(x => x.RunSettings).Returns(_settingsMock.Object);
        _testExecutorMock.Setup(x => x.RunTests(It.IsAny<IEnumerable<TestCase>?>(), It.IsAny<IRunContext?>(), It.IsAny<IFrameworkHandle?>()))
        .Callback((IEnumerable<TestCase>? tests, IRunContext? runContext, IFrameworkHandle? frameworkHandle) => Assert.AreEqual(testCases, tests));
        _testExecutorMock.Setup(x => x.RunTests(It.IsAny<IEnumerable<string>?>(), It.IsAny<IRunContext?>(), It.IsAny<IFrameworkHandle?>()))
        .Callback((IEnumerable<string>? tests, IRunContext? runContext, IFrameworkHandle? frameworkHandle) => Assert.AreEqual(sourcesName, tests));

        // Run test
        SerialTestRunDecorator serialTestRunDecorator = new(_testExecutorMock.Object);
        serialTestRunDecorator.RunTests(testCases, _contextMock.Object, _frameworkWorkHandleMock.Object);
        serialTestRunDecorator.RunTests(sourcesName, _contextMock.Object, _frameworkWorkHandleMock.Object);

        // Assert
        _testExecutorMock.Verify(x => x.RunTests(It.IsAny<IEnumerable<TestCase>?>(), It.IsAny<IRunContext?>(), It.IsAny<IFrameworkHandle?>()), Times.Once());
        _testExecutorMock.Verify(x => x.RunTests(It.IsAny<IEnumerable<string>?>(), It.IsAny<IRunContext?>(), It.IsAny<IFrameworkHandle?>()), Times.Once());
    }
}
