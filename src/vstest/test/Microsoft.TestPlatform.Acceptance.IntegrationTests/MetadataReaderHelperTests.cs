// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class MetadataReaderHelperTests : AcceptanceTestBase
{
    [TestMethod]
    public void MetadataReaderHelper_GetCollectorExtensionTypes()
    {
        var dataCollectorFilePath = GetTestDllForFramework("AttachmentProcessorDataCollector.dll", "netstandard2.0");
        var types = MetadataReaderExtensionsHelper.DiscoverTestExtensionTypesV2Attribute(Assembly.LoadFile(dataCollectorFilePath), dataCollectorFilePath);
        Assert.IsTrue(types.Any(), $"File {dataCollectorFilePath}");
        Assert.IsTrue(types[0].AssemblyQualifiedName!.StartsWith("AttachmentProcessorDataCollector.SampleDataCollectorV2"), $"File {dataCollectorFilePath}");
        Assert.AreEqual(dataCollectorFilePath.Replace("/", @"\"), types[0].Assembly.Location.Replace("/", @"\"), $"File {dataCollectorFilePath}");
        Assert.IsTrue(types[1].AssemblyQualifiedName!.StartsWith("AttachmentProcessorDataCollector.SampleDataCollectorV1"), $"File {dataCollectorFilePath}");
        Assert.AreEqual(dataCollectorFilePath.Replace("/", @"\"), types[1].Assembly.Location.Replace("/", @"\"), $"File {dataCollectorFilePath}");
    }
}
