// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.Common.UnitTests.ExtensionFramework.Utilities;

[TestClass]
public class TestExtensionPluginInformationTests
{
    private TestableTestExtensionPluginInformation? _testPluginInformation;

    internal const string DefaultExtensionUri = "executor://unittest";

    [TestMethod]
    public void AssemblyQualifiedNameShouldReturnTestExtensionTypesName()
    {
        _testPluginInformation = new TestableTestExtensionPluginInformation(typeof(DummyTestExtensionWithNoExtensionUri));
        Assert.AreEqual(typeof(DummyTestExtensionWithNoExtensionUri).AssemblyQualifiedName, _testPluginInformation.AssemblyQualifiedName);
    }

    [TestMethod]
    public void IdentifierDataShouldReturnExtensionUri()
    {
        _testPluginInformation = new TestableTestExtensionPluginInformation(typeof(DummyTestExtensionWithExtensionUri));
        Assert.AreEqual(DefaultExtensionUri, _testPluginInformation.IdentifierData);
    }

    [TestMethod]
    public void ExtensionUriShouldReturnEmptyIfAnExtensionDoesNotHaveOne()
    {
        _testPluginInformation = new TestableTestExtensionPluginInformation(typeof(DummyTestExtensionWithNoExtensionUri));
        Assert.IsNotNull(_testPluginInformation.ExtensionUri);
        Assert.AreEqual(string.Empty, _testPluginInformation.ExtensionUri);
    }

    [TestMethod]
    public void ExtensionUriShouldReturnExtensionUriOfAnExtension()
    {
        _testPluginInformation = new TestableTestExtensionPluginInformation(typeof(DummyTestExtensionWithExtensionUri));
        Assert.AreEqual(DefaultExtensionUri, _testPluginInformation.ExtensionUri);
    }

    [TestMethod]
    public void MetadataShouldReturnExtensionUri()
    {
        _testPluginInformation = new TestableTestExtensionPluginInformation(typeof(DummyTestExtensionWithExtensionUri));

        CollectionAssert.AreEqual(new object[] { DefaultExtensionUri }, _testPluginInformation.Metadata.ToArray());
    }

    #region Implementation

    private class TestableTestExtensionPluginInformation : TestExtensionPluginInformation
    {
        public TestableTestExtensionPluginInformation(Type testExtensionType) : base(testExtensionType)
        {
        }
    }

    private class DummyTestExtensionWithNoExtensionUri
    {
    }

    [ExtensionUri(DefaultExtensionUri)]
    private class DummyTestExtensionWithExtensionUri
    {
    }

    #endregion
}
