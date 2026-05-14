// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.Common.UnitTests.ExtensionFramework.Utilities;

[TestClass]
public class TestLoggerPluginInformationTests
{
    private TestLoggerPluginInformation? _testPluginInformation;

    internal const string DefaultExtensionUri = "executor://unittest";

    internal const string DefaultFriendlyName = "excel";

    [TestMethod]
    public void AssemblyQualifiedNameShouldReturnTestExtensionTypesName()
    {
        _testPluginInformation = new TestLoggerPluginInformation(typeof(DummyTestExtensionWithNoFriendlyName));
        Assert.AreEqual(typeof(DummyTestExtensionWithNoFriendlyName).AssemblyQualifiedName, _testPluginInformation.AssemblyQualifiedName);
    }

    [TestMethod]
    public void IdentifierDataShouldReturnExtensionUri()
    {
        _testPluginInformation = new TestLoggerPluginInformation(typeof(DummyTestExtensionWithFriendlyName));
        Assert.AreEqual(DefaultExtensionUri, _testPluginInformation.IdentifierData);
    }

    [TestMethod]
    public void FriendlyNameShouldReturnEmptyIfALoggerDoesNotHaveOne()
    {
        _testPluginInformation = new TestLoggerPluginInformation(typeof(DummyTestExtensionWithNoFriendlyName));
        Assert.IsNotNull(_testPluginInformation.FriendlyName);
        Assert.AreEqual(string.Empty, _testPluginInformation.FriendlyName);
    }

    [TestMethod]
    public void FriendlyNameShouldReturnFriendlyNameOfALogger()
    {
        _testPluginInformation = new TestLoggerPluginInformation(typeof(DummyTestExtensionWithFriendlyName));
        Assert.AreEqual(DefaultFriendlyName, _testPluginInformation.FriendlyName);
    }

    [TestMethod]
    public void MetadataShouldReturnExtensionUriAndFriendlyName()
    {
        _testPluginInformation = new TestLoggerPluginInformation(typeof(DummyTestExtensionWithFriendlyName));

        CollectionAssert.AreEqual(new object[] { DefaultExtensionUri, DefaultFriendlyName }, _testPluginInformation.Metadata.ToArray());
    }

    #region Implementation

    private class DummyTestExtensionWithNoFriendlyName
    {
    }

    [FriendlyName(DefaultFriendlyName)]
    [ExtensionUri(DefaultExtensionUri)]
    private class DummyTestExtensionWithFriendlyName
    {
    }

    #endregion
}
