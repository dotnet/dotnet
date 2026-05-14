// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.Common.UnitTests.ExtensionFramework.Utilities;

[TestClass]
public class TestSettingsProviderPluginInformationTests
{
    private TestSettingsProviderPluginInformation? _testPluginInformation;

    private const string DefaultSettingsName = "mstestsettings";

    [TestMethod]
    public void AssemblyQualifiedNameShouldReturnTestExtensionTypesName()
    {
        _testPluginInformation = new TestSettingsProviderPluginInformation(typeof(TestPluginInformationTests));
        Assert.AreEqual(typeof(TestPluginInformationTests).AssemblyQualifiedName, _testPluginInformation.AssemblyQualifiedName);
    }

    [TestMethod]
    public void IdentifierDataShouldReturnSettingsName()
    {
        _testPluginInformation = new TestSettingsProviderPluginInformation(typeof(DummySettingProviderWithSettingsName));
        Assert.AreEqual(DefaultSettingsName, _testPluginInformation.IdentifierData);
    }

    [TestMethod]
    public void MetadataShouldReturnSettingsProviderName()
    {
        _testPluginInformation = new TestSettingsProviderPluginInformation(typeof(DummySettingProviderWithSettingsName));
        CollectionAssert.AreEqual(new object[] { DefaultSettingsName }, _testPluginInformation.Metadata.ToArray());
    }

    [TestMethod]
    public void SettingsNameShouldReturnEmptyIfASettingsProviderDoesNotHaveOne()
    {
        _testPluginInformation = new TestSettingsProviderPluginInformation(typeof(DummySettingProviderWithoutSettingsName));
        Assert.IsNotNull(_testPluginInformation.SettingsName);
        Assert.AreEqual(string.Empty, _testPluginInformation.SettingsName);
    }

    [TestMethod]
    public void SettingsNameShouldReturnExtensionUriOfAnExtension()
    {
        _testPluginInformation = new TestSettingsProviderPluginInformation(typeof(DummySettingProviderWithSettingsName));
        Assert.AreEqual(DefaultSettingsName, _testPluginInformation.SettingsName);
    }

    #region Implementation

    private class DummySettingProviderWithoutSettingsName
    {
    }

    [SettingsName(DefaultSettingsName)]
    private class DummySettingProviderWithSettingsName
    {
    }

    #endregion
}
