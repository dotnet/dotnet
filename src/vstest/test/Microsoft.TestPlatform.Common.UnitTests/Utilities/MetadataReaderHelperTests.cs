// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.Common.UnitTests.Utilities;

[TestClass]
public class MetadataReaderHelperTests
{
    [TestMethod]
    public void MetadataReaderHelper_GetCollectorExtensionTypes()
    {
        string testAssetsPath = GetTestAssetsFolder();
        var dataCollectorFilePath =
            Directory.GetFiles(testAssetsPath, "AttachmentProcessorDataCollector.dll", SearchOption.AllDirectories)
            .Where(x => x.Contains("bin") && x.Contains(IntegrationTestEnvironment.BuildConfiguration))
            .Single();
        var types = MetadataReaderExtensionsHelper.DiscoverTestExtensionTypesV2Attribute(Assembly.LoadFile(dataCollectorFilePath), dataCollectorFilePath);
        Assert.IsTrue(types.Any(), $"File {dataCollectorFilePath}");
        Assert.IsTrue(types[0].AssemblyQualifiedName!.StartsWith("AttachmentProcessorDataCollector.SampleDataCollectorV2"), $"File {dataCollectorFilePath}");
        Assert.AreEqual(dataCollectorFilePath.Replace("/", @"\"), types[0].Assembly.Location.Replace("/", @"\"), $"File {dataCollectorFilePath}");
        Assert.IsTrue(types[1].AssemblyQualifiedName!.StartsWith("AttachmentProcessorDataCollector.SampleDataCollectorV1"), $"File {dataCollectorFilePath}");
        Assert.AreEqual(dataCollectorFilePath.Replace("/", @"\"), types[1].Assembly.Location.Replace("/", @"\"), $"File {dataCollectorFilePath}");
    }

    private static string GetTestAssetsFolder()
    {
        string current = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        while (true)
        {
            if (File.Exists(Path.Combine(current, "TestPlatform.sln")))
            {
                string testAssetsPath = Path.Combine(current, @"test/TestAssets");
                Assert.IsTrue(Directory.Exists(testAssetsPath), $"Directory not found '{testAssetsPath}'");
                return testAssetsPath;
            }
            current = Path.GetDirectoryName(current)!;
            if (current == Path.GetPathRoot(current))
            {
                throw new Exception("Repo root path not tound");
            }
        }
    }
}
