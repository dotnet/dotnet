// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

[TestClass]
public class RunConfigurationTests
{
    [TestMethod]
    public void RunConfigurationDefaultValuesMustBeUsedOnCreation()
    {
        var runConfiguration = new RunConfiguration();

        // Verify Default
        Assert.AreEqual(Constants.DefaultPlatform, runConfiguration.TargetPlatform);
        Assert.AreEqual(Framework.DefaultFramework, runConfiguration.TargetFramework);
        Assert.AreEqual(Constants.DefaultBatchSize, runConfiguration.BatchSize);
        Assert.AreEqual(0, runConfiguration.TestSessionTimeout);
        Assert.AreEqual(Constants.DefaultResultsDirectory, runConfiguration.ResultsDirectory);
        Assert.IsNull(runConfiguration.SolutionDirectory);
        Assert.AreEqual(Constants.DefaultTreatTestAdapterErrorsAsWarnings, runConfiguration.TreatTestAdapterErrorsAsWarnings);
        Assert.IsNull(runConfiguration.BinariesRoot);
        Assert.IsNull(runConfiguration.TestAdaptersPaths);
        Assert.AreEqual(Constants.DefaultCpuCount, runConfiguration.MaxCpuCount);
        Assert.IsFalse(runConfiguration.DisableAppDomain);
        Assert.IsFalse(runConfiguration.DisableParallelization);
        Assert.IsFalse(runConfiguration.DesignMode);
        Assert.IsFalse(runConfiguration.InIsolation);
        Assert.AreEqual(runConfiguration.DesignMode, runConfiguration.ShouldCollectSourceInformation);
        Assert.AreEqual(Constants.DefaultExecutionThreadApartmentState, runConfiguration.ExecutionThreadApartmentState);
    }

    [TestMethod]
    public void RunConfigurationShouldNotThrowExceptionOnUnknownElements()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <BadElement>TestResults</BadElement>
                       <DesignMode>true</DesignMode>
                     </RunConfiguration>
                </RunSettings>";

        var runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(settingsXml);

        Assert.IsNotNull(runConfiguration);
        Assert.IsTrue(runConfiguration.DesignMode);
    }

    [TestMethod]
    public void RunConfigurationReadsValuesCorrectlyFromXml()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <ResultsDirectory>TestResults</ResultsDirectory>
                       <TargetPlatform>x64</TargetPlatform>
                       <TargetFrameworkVersion>FrameworkCore10</TargetFrameworkVersion>
                       <SolutionDirectory>%temp%</SolutionDirectory>
                       <TreatTestAdapterErrorsAsWarnings>true</TreatTestAdapterErrorsAsWarnings>
                       <DisableAppDomain>true</DisableAppDomain>
                       <DisableParallelization>true</DisableParallelization>
                       <MaxCpuCount>2</MaxCpuCount>
                       <BatchSize>5</BatchSize>
                       <TestSessionTimeout>10000</TestSessionTimeout>
                       <TestAdaptersPaths>C:\a\b;D:\x\y</TestAdaptersPaths>
                       <BinariesRoot>E:\x\z</BinariesRoot>
                       <DesignMode>true</DesignMode>
                       <InIsolation>true</InIsolation>
                       <CollectSourceInformation>false</CollectSourceInformation>
                       <ExecutionThreadApartmentState>STA</ExecutionThreadApartmentState>
                     </RunConfiguration>
                </RunSettings>";

        var runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(settingsXml);

        // Verify Default
        Assert.AreEqual(Architecture.X64, runConfiguration.TargetPlatform);

        var expectedFramework = Framework.FromString("FrameworkCore10")!;
        _ = runConfiguration.TargetFramework;
        Assert.AreEqual(expectedFramework.Name, runConfiguration.TargetFramework!.Name);
        Assert.AreEqual(expectedFramework.Version, runConfiguration.TargetFramework.Version);

        Assert.AreEqual("TestResults", runConfiguration.ResultsDirectory);

        var expectedSolutionPath = Environment.GetEnvironmentVariable("temp");
        Assert.AreEqual(expectedSolutionPath, runConfiguration.SolutionDirectory);

        Assert.IsTrue(runConfiguration.TreatTestAdapterErrorsAsWarnings);
        Assert.AreEqual(@"E:\x\z", runConfiguration.BinariesRoot);
        Assert.AreEqual(@"C:\a\b;D:\x\y", runConfiguration.TestAdaptersPaths);
        Assert.AreEqual(2, runConfiguration.MaxCpuCount);
        Assert.AreEqual(5, runConfiguration.BatchSize);
        Assert.AreEqual(10000, runConfiguration.TestSessionTimeout);
        Assert.IsTrue(runConfiguration.DisableAppDomain);
        Assert.IsTrue(runConfiguration.DisableParallelization);
        Assert.IsTrue(runConfiguration.DesignMode);
        Assert.IsTrue(runConfiguration.InIsolation);
        Assert.IsFalse(runConfiguration.ShouldCollectSourceInformation);
        Assert.AreEqual(PlatformApartmentState.STA, runConfiguration.ExecutionThreadApartmentState);
    }

    [TestMethod]
    [Obsolete("TargetFrameworkVersion is Obsolete but we want to test its behavior")]
    public void SetTargetFrameworkVersionShouldSetTargetFramework()
    {
        var runConfiguration = new RunConfiguration();
        runConfiguration.TargetFrameworkVersion = FrameworkVersion.Framework35;
        Assert.AreEqual(Framework.FromString("Framework35")!.Name, runConfiguration.TargetFramework!.Name);
        Assert.AreEqual(FrameworkVersion.Framework35, runConfiguration.TargetFrameworkVersion);

        runConfiguration.TargetFrameworkVersion = FrameworkVersion.Framework40;
        Assert.AreEqual(Framework.FromString("Framework40")!.Name, runConfiguration.TargetFramework.Name);
        Assert.AreEqual(FrameworkVersion.Framework40, runConfiguration.TargetFrameworkVersion);

        runConfiguration.TargetFrameworkVersion = FrameworkVersion.Framework45;
        Assert.AreEqual(Framework.FromString("Framework45")!.Name, runConfiguration.TargetFramework.Name);
        Assert.AreEqual(FrameworkVersion.Framework45, runConfiguration.TargetFrameworkVersion);

        runConfiguration.TargetFrameworkVersion = FrameworkVersion.FrameworkCore10;
        Assert.AreEqual(Framework.FromString("FrameworkCore10")!.Name, runConfiguration.TargetFramework.Name);
        Assert.AreEqual(FrameworkVersion.FrameworkCore10, runConfiguration.TargetFrameworkVersion);

        runConfiguration.TargetFrameworkVersion = FrameworkVersion.FrameworkUap10;
        Assert.AreEqual(Framework.FromString("FrameworkUap10")!.Name, runConfiguration.TargetFramework.Name);
        Assert.AreEqual(FrameworkVersion.FrameworkUap10, runConfiguration.TargetFrameworkVersion);
    }

    [TestMethod]
    [Obsolete("TargetFrameworkVersion is Obsolete but we want to test its behavior")]
    public void SetTargetFrameworkShouldSetTargetFrameworkVersion()
    {
        var runConfiguration = new RunConfiguration();
        runConfiguration.TargetFramework = Framework.FromString("Framework35");
        Assert.AreEqual(FrameworkVersion.Framework35, runConfiguration.TargetFrameworkVersion);

        runConfiguration.TargetFramework = Framework.FromString("Framework40");
        Assert.AreEqual(FrameworkVersion.Framework40, runConfiguration.TargetFrameworkVersion);

        runConfiguration.TargetFramework = Framework.FromString("Framework45");
        Assert.AreEqual(FrameworkVersion.Framework45, runConfiguration.TargetFrameworkVersion);

        runConfiguration.TargetFramework = Framework.FromString("FrameworkCore10");
        Assert.AreEqual(FrameworkVersion.FrameworkCore10, runConfiguration.TargetFrameworkVersion);

        runConfiguration.TargetFramework = Framework.FromString("FrameworkUap10");
        Assert.AreEqual(FrameworkVersion.FrameworkUap10, runConfiguration.TargetFrameworkVersion);
    }

    [TestMethod]
    public void RunConfigurationFromXmlThrowsSettingsExceptionIfBatchSizeIsInvalid()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <BatchSize>Foo</BatchSize>
                     </RunConfiguration>
                </RunSettings>";


        var exception = Assert.ThrowsExactly<SettingsException>(
                () => XmlRunSettingsUtilities.GetRunConfigurationNode(settingsXml));
        Assert.AreEqual("Invalid settings 'RunConfiguration'.  Invalid value 'Foo' specified for 'BatchSize'.", exception.Message);
    }

    [TestMethod]
    public void RunConfigurationFromXmlThrowsSettingsExceptionIfTestSessionTimeoutIsInvalid()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <TestSessionTimeout>-1</TestSessionTimeout>
                     </RunConfiguration>
                </RunSettings>";


        var exception = Assert.ThrowsExactly<SettingsException>(
                () => XmlRunSettingsUtilities.GetRunConfigurationNode(settingsXml));
        Assert.AreEqual("Invalid settings 'RunConfiguration'.  Invalid value '-1' specified for 'TestSessionTimeout'.", exception.Message);
    }

    [TestMethod]
    public void RunConfigurationFromXmlShouldNotThrowsSettingsExceptionIfTestSessionTimeoutIsZero()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <TestSessionTimeout>0</TestSessionTimeout>
                     </RunConfiguration>
                </RunSettings>";


        XmlRunSettingsUtilities.GetRunConfigurationNode(settingsXml);
    }

    [TestMethod]
    public void RunConfigurationFromXmlThrowsSettingsExceptionIfExecutionThreadApartmentStateIsInvalid()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <ExecutionThreadApartmentState>RandomValue</ExecutionThreadApartmentState>
                     </RunConfiguration>
                </RunSettings>";


        var exception = Assert.ThrowsExactly<SettingsException>(
                () => XmlRunSettingsUtilities.GetRunConfigurationNode(settingsXml));
        Assert.AreEqual("Invalid settings 'RunConfiguration'.  Invalid value 'RandomValue' specified for 'ExecutionThreadApartmentState'.", exception.Message);
    }

    [TestMethod]
    public void RunConfigurationFromXmlThrowsSettingsExceptionIfBatchSizeIsNegativeInteger()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <BatchSize>-10</BatchSize>
                     </RunConfiguration>
                </RunSettings>";

        var exception = Assert.ThrowsExactly<SettingsException>(
                () => XmlRunSettingsUtilities.GetRunConfigurationNode(settingsXml));
        Assert.AreEqual("Invalid settings 'RunConfiguration'.  Invalid value '-10' specified for 'BatchSize'.", exception.Message);
    }

    [DataRow(true)]
    [DataRow(false)]
    [DataTestMethod]
    public void RunConfigurationShouldReadValueForDesignMode(bool designModeValue)
    {
        string settingsXml = string.Format(
            CultureInfo.CurrentCulture,
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DesignMode>{0}</DesignMode>
                     </RunConfiguration>
                </RunSettings>", designModeValue);

        var runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(settingsXml);

        Assert.AreEqual(designModeValue, runConfiguration.DesignMode);
    }

    [TestMethod]
    public void RunConfigurationShouldSetDesignModeAsFalseByDefault()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <TargetPlatform>x64</TargetPlatform>
                     </RunConfiguration>
                </RunSettings>";

        var runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(settingsXml);

        Assert.IsFalse(runConfiguration.DesignMode);
    }

    [TestMethod]
    public void RunConfigurationToXmlShouldProvideDesignMode()
    {
        var runConfiguration = new RunConfiguration { DesignMode = true };

        StringAssert.Contains(runConfiguration.ToXml().InnerXml, "<DesignMode>True</DesignMode>");
    }

    [DataRow(true)]
    [DataRow(false)]
    [DataTestMethod]
    public void RunConfigurationShouldReadValueForCollectSourceInformation(bool val)
    {
        string settingsXml = string.Format(
            CultureInfo.CurrentCulture,
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <CollectSourceInformation>{0}</CollectSourceInformation>
                     </RunConfiguration>
                </RunSettings>", val);

        var runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(settingsXml);

        Assert.AreEqual(val, runConfiguration.ShouldCollectSourceInformation);
    }

    [TestMethod]
    public void RunConfigurationShouldSetCollectSourceInformationSameAsDesignModeByDefault()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <TargetPlatform>x64</TargetPlatform>
                     </RunConfiguration>
                </RunSettings>";

        var runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(settingsXml);

        Assert.AreEqual(runConfiguration.DesignMode, runConfiguration.ShouldCollectSourceInformation);
    }

    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void RunConfigurationToXmlShouldProvideCollectSourceInformationSameAsDesignMode(bool val)
    {
        var runConfiguration = new RunConfiguration { DesignMode = val };
        StringAssert.Contains(runConfiguration.ToXml().InnerXml.ToUpperInvariant(), $"<CollectSourceInformation>{val}</CollectSourceInformation>".ToUpperInvariant());
    }

    [TestMethod]
    public void RunConfigurationToXmlShouldProvideExecutionThreadApartmentState()
    {
        var runConfiguration = new RunConfiguration { ExecutionThreadApartmentState = PlatformApartmentState.STA };

        StringAssert.Contains(runConfiguration.ToXml().InnerXml, "<ExecutionThreadApartmentState>STA</ExecutionThreadApartmentState>");
    }

    [TestMethod]
    public void RunConfigurationShouldThrowSettingsExceptionIfResultsirectoryIsEmpty()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <ResultsDirectory/>
                     </RunConfiguration>
                </RunSettings>";

        var exception = Assert.ThrowsExactly<SettingsException>(
                () => XmlRunSettingsUtilities.GetRunConfigurationNode(settingsXml));
        Assert.AreEqual("Invalid settings 'RunConfiguration'.  Invalid value '' specified for 'ResultsDirectory'.", exception.Message);
    }
}
