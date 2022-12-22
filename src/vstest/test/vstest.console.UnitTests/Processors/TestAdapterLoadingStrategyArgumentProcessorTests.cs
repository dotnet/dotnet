// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class TestAdapterLoadingStrategyArgumentProcessorTests
{
    private readonly RunSettings _currentActiveSetting;

    public TestAdapterLoadingStrategyArgumentProcessorTests()
    {
        _currentActiveSetting = RunSettingsManager.Instance.ActiveRunSettings;
    }

    [TestCleanup]
    public void TestClean()
    {
        RunSettingsManager.Instance.SetActiveRunSettings(_currentActiveSetting);
    }

    [TestMethod]
    [TestCategory("Windows")]
    public void InitializeShouldHonorEnvironmentVariablesInTestAdapterPaths()
    {
        var runSettingsXml = "<RunSettings><RunConfiguration><TestAdaptersPaths>%temp%\\adapters1;%temp%\\adapters2</TestAdaptersPaths></RunConfiguration></RunSettings>";
        var runSettings = new RunSettings();
        runSettings.LoadSettingsXml(runSettingsXml);
        RunSettingsManager.Instance.SetActiveRunSettings(runSettings);
        var mockFileHelper = new Mock<IFileHelper>();
        var mockOutput = new Mock<IOutput>();

        mockFileHelper.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(true);
        mockFileHelper.Setup(x => x.GetFullPath(It.IsAny<string>())).Returns((Func<string, string>)(s => Path.GetFullPath(s)));

        var executor = new TestAdapterLoadingStrategyArgumentExecutor(CommandLineOptions.Instance, RunSettingsManager.Instance, mockOutput.Object, mockFileHelper.Object);

        executor.Initialize(nameof(TestAdapterLoadingStrategy.Default));
        var runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(RunSettingsManager.Instance.ActiveRunSettings.SettingsXml);

        var tempPath = Path.GetFullPath(Environment.ExpandEnvironmentVariables("%temp%"));
        Assert.AreEqual($"{tempPath}\\adapters1;{tempPath}\\adapters2", runConfiguration.TestAdaptersPaths);
    }

    [TestMethod]
    [TestCategory("Windows")]
    public void InitializeShouldAddRightAdapterPathInErrorMessage()
    {
        var runSettingsXml = "<RunSettings><RunConfiguration><TestAdaptersPaths>d:\\users</TestAdaptersPaths></RunConfiguration></RunSettings>";
        var runSettings = new RunSettings();
        runSettings.LoadSettingsXml(runSettingsXml);
        RunSettingsManager.Instance.SetActiveRunSettings(runSettings);
        var mockFileHelper = new Mock<IFileHelper>();
        var mockOutput = new Mock<IOutput>();

        mockFileHelper.Setup(x => x.DirectoryExists("d:\\users")).Returns(false);
        mockFileHelper.Setup(x => x.DirectoryExists("c:\\users")).Returns(true);
        var executor = new TestAdapterLoadingStrategyArgumentExecutor(CommandLineOptions.Instance, RunSettingsManager.Instance, mockOutput.Object, mockFileHelper.Object);

        var message = "The path 'd:\\users' specified in the 'TestAdapterPath' is invalid. Error: The custom test adapter search path provided was not found, provide a valid path and try again.";

        var isExceptionThrown = false;
        try
        {
            executor.Initialize(nameof(TestAdapterLoadingStrategy.Default));
        }
        catch (Exception ex)
        {
            isExceptionThrown = true;
            Assert.IsTrue(ex is CommandLineException);
            Assert.AreEqual(message, ex.Message);
        }

        Assert.IsTrue(isExceptionThrown);
    }


    [TestMethod]
    public void InitializeShouldThrowIfPathDoesNotExist()
    {
        var folder = "C:\\temp\\thisfolderdoesnotexist";
        var runSettingsXml = "<RunSettings><RunConfiguration><TestAdaptersPaths>" + folder + "</TestAdaptersPaths></RunConfiguration></RunSettings>";
        var runSettings = new RunSettings();

        runSettings.LoadSettingsXml(runSettingsXml);
        RunSettingsManager.Instance.SetActiveRunSettings(runSettings);

        var mockOutput = new Mock<IOutput>();
        var executor = new TestAdapterLoadingStrategyArgumentExecutor(CommandLineOptions.Instance, RunSettingsManager.Instance, mockOutput.Object, new FileHelper());

        var message = $"The path '{folder}' specified in the 'TestAdapterPath' is invalid. Error: The custom test adapter search path provided was not found, provide a valid path and try again.";

        var isExceptionThrown = false;

        try
        {
            executor.Initialize(nameof(TestAdapterLoadingStrategy.Default));
        }
        catch (Exception ex)
        {
            isExceptionThrown = true;
            Assert.IsTrue(ex is CommandLineException);
            Assert.AreEqual(message, ex.Message);
        }

        Assert.IsTrue(isExceptionThrown);
    }
}
