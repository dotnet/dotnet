// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.Common.UnitTests.ExtensionFramework.Utilities;

[TestClass]
public class TestPluginInformationTests
{
    private readonly TestableTestPluginInformation _testPluginInformation;

    public TestPluginInformationTests()
    {
        _testPluginInformation = new TestableTestPluginInformation(typeof(TestPluginInformationTests));
    }

    [TestMethod]
    public void AssemblyQualifiedNameShouldReturnTestExtensionTypesName()
    {
        Assert.AreEqual(typeof(TestPluginInformationTests).AssemblyQualifiedName, _testPluginInformation.AssemblyQualifiedName);
    }

    [TestMethod]
    public void IdentifierDataShouldReturnTestExtensionTypesName()
    {
        Assert.AreEqual(typeof(TestPluginInformationTests).AssemblyQualifiedName, _testPluginInformation.IdentifierData);
    }

    [TestMethod]
    public void MetadataShouldReturnTestExtensionTypesAssemblyQualifiedName()
    {
        CollectionAssert.AreEqual(new object[] { typeof(TestPluginInformationTests).AssemblyQualifiedName! }, _testPluginInformation.Metadata.ToArray());
    }
}

#region Implementation

public class TestableTestPluginInformation : TestPluginInformation
{
    public TestableTestPluginInformation(Type testExtensionType) : base(testExtensionType)
    {
    }
}

#endregion
