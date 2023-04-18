// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.TestRunAttachmentsProcessing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests.DataCollectorAttachmentsProcessorsFactoryTests;

[TestClass]
public class DataCollectorAttachmentsProcessorsFactoryTests : AcceptanceTestBase
{
    private readonly DataCollectorAttachmentsProcessorsFactory _dataCollectorAttachmentsProcessorsFactory = new();

    [TestInitialize]
    public void Init()
    {
        TestPluginCacheHelper.SetupMockExtensions(typeof(DataCollectorAttachmentsProcessorsFactoryTests));
    }

    [TestCleanup]
    public void Cleanup()
    {
        TestPluginCacheHelper.ResetExtensionsCache();
    }

    [TestMethod]
    public void Create_ShouldReturnListOfAttachmentProcessors()
    {
        // arrange
        List<InvokedDataCollector> invokedDataCollectors = new()
        {
            new InvokedDataCollector(new Uri("datacollector://Sample"), "Sample", typeof(SampleDataCollector).AssemblyQualifiedName!, typeof(SampleDataCollector).Assembly.Location, true),
            new InvokedDataCollector(new Uri("datacollector://SampleData2"), "SampleData2", typeof(SampleData2Collector).AssemblyQualifiedName!, typeof(SampleData2Collector).Assembly.Location, true),
            new InvokedDataCollector(new Uri("datacollector://SampleData3"), "SampleData3", typeof(SampleData3Collector).AssemblyQualifiedName!, typeof(SampleData3Collector).Assembly.Location, true)
        };
        // act
        var dataCollectorAttachmentsProcessors = _dataCollectorAttachmentsProcessorsFactory.Create(invokedDataCollectors.ToArray(), null);

        // assert
        Assert.AreEqual(3, dataCollectorAttachmentsProcessors.Length);

        Assert.AreEqual(1, dataCollectorAttachmentsProcessors.Count(x => x.FriendlyName == "Sample"));
        Assert.AreEqual(1, dataCollectorAttachmentsProcessors.Count(x => x.FriendlyName == "SampleData3"));
        Assert.AreEqual(1, dataCollectorAttachmentsProcessors.Count(x => x.FriendlyName == "Code Coverage"));

        Assert.AreEqual(typeof(DataCollectorAttachmentProcessor).AssemblyQualifiedName, dataCollectorAttachmentsProcessors[0].DataCollectorAttachmentProcessorInstance.GetType().AssemblyQualifiedName);
        Assert.AreEqual(typeof(DataCollectorAttachmentProcessor2).AssemblyQualifiedName, dataCollectorAttachmentsProcessors[1].DataCollectorAttachmentProcessorInstance.GetType().AssemblyQualifiedName);
        Assert.AreEqual(typeof(CodeCoverageDataAttachmentsHandler).AssemblyQualifiedName, dataCollectorAttachmentsProcessors[2].DataCollectorAttachmentProcessorInstance.GetType().AssemblyQualifiedName);
    }

    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void Create_EmptyOrNullInvokedDataCollector_ShouldReturnCodeCoverageDataAttachmentsHandler(bool empty)
    {
        // act
        var dataCollectorAttachmentsProcessors = _dataCollectorAttachmentsProcessorsFactory.Create(empty ? Array.Empty<InvokedDataCollector>() : null, null);

        //assert
        Assert.AreEqual(1, dataCollectorAttachmentsProcessors.Length);
        Assert.AreEqual(typeof(CodeCoverageDataAttachmentsHandler).AssemblyQualifiedName, dataCollectorAttachmentsProcessors[0].DataCollectorAttachmentProcessorInstance.GetType().AssemblyQualifiedName);
    }

    [TestMethod]
    public void Create_ShouldNotFailIfWrongDataCollectorAttachmentProcessor()
    {
        // arrange
        List<InvokedDataCollector> invokedDataCollectors = new()
        {
            new InvokedDataCollector(new Uri("datacollector://SampleData4"), "SampleData4", typeof(SampleData4Collector).AssemblyQualifiedName!, typeof(SampleData4Collector).Assembly.Location, true)
        };

        // act
        var dataCollectorAttachmentsProcessors = _dataCollectorAttachmentsProcessorsFactory.Create(invokedDataCollectors.ToArray(), null);

        // assert
        Assert.AreEqual(1, dataCollectorAttachmentsProcessors.Length);
        Assert.AreEqual(typeof(CodeCoverageDataAttachmentsHandler).AssemblyQualifiedName, dataCollectorAttachmentsProcessors[0].DataCollectorAttachmentProcessorInstance.GetType().AssemblyQualifiedName);
    }

    [TestMethod]
    public void Create_ShouldAddTwoTimeCodeCoverageDataAttachmentsHandler()
    {
        // arrange
        List<InvokedDataCollector> invokedDataCollectors = new()
        {
            new InvokedDataCollector(new Uri("datacollector://microsoft/CodeCoverage/2.0"), "SampleData5", typeof(SampleData5Collector).AssemblyQualifiedName!, typeof(SampleData5Collector).Assembly.Location, true)
        };

        // act
        var dataCollectorAttachmentsProcessors = _dataCollectorAttachmentsProcessorsFactory.Create(invokedDataCollectors.ToArray(), null);

        // assert
        Assert.AreEqual(2, dataCollectorAttachmentsProcessors.Length);
        Assert.AreEqual(typeof(DataCollectorAttachmentProcessor).AssemblyQualifiedName, dataCollectorAttachmentsProcessors[0].DataCollectorAttachmentProcessorInstance.GetType().AssemblyQualifiedName);
        Assert.AreEqual(typeof(CodeCoverageDataAttachmentsHandler).AssemblyQualifiedName, dataCollectorAttachmentsProcessors[1].DataCollectorAttachmentProcessorInstance.GetType().AssemblyQualifiedName);
    }

    [TestMethod]
    public void Create_ShouldLoadOrderingByFilePath()
    {
        // arrange
        // We cannot cleanup at the end because assembly will be copied into tmp directory and loaded
        string testAssetsPath = GetTestAssetsFolder();
        var dataCollectorFilePath = GetTestDllForFramework("AttachmentProcessorDataCollector.dll", "netstandard2.0");
#pragma warning disable RS0030 // Do not used banned APIs
        string tmpDir = Path.Combine(Path.GetTempPath(), nameof(Create_ShouldLoadOrderingByFilePath));
#pragma warning restore RS0030 // Do not used banned APIs
        Directory.CreateDirectory(tmpDir);
        string version1 = Path.Combine(tmpDir, "1.0.0");
        Directory.CreateDirectory(version1);
        File.Copy(dataCollectorFilePath, Path.Combine(version1, Path.GetFileName(dataCollectorFilePath)), true);
        string version2 = Path.Combine(tmpDir, "1.0.1");
        Directory.CreateDirectory(version2);
        File.Copy(dataCollectorFilePath, Path.Combine(version2, Path.GetFileName(dataCollectorFilePath)), true);

        List<InvokedDataCollector> invokedDataCollectors = new()
        {
            new InvokedDataCollector(new Uri("my://sample/datacollector"), "sample", "AttachmentProcessorDataCollector.SampleDataCollectorV2", Path.Combine(version1, Path.GetFileName(dataCollectorFilePath)), true),
            new InvokedDataCollector(new Uri("my://sample/datacollector"), "sample", "AttachmentProcessorDataCollector.SampleDataCollectorV2", Path.Combine(version2, Path.GetFileName(dataCollectorFilePath)), true)
        };

        // act
        var dataCollectorAttachmentsProcessors = _dataCollectorAttachmentsProcessorsFactory.Create(invokedDataCollectors.ToArray(), null);

        // assert
        Assert.AreEqual(2, dataCollectorAttachmentsProcessors.Length);
        Assert.IsTrue(Regex.IsMatch(dataCollectorAttachmentsProcessors[0].DataCollectorAttachmentProcessorInstance.GetType().AssemblyQualifiedName!, @"AttachmentProcessorDataCollector\.SampleDataCollectorAttachmentProcessor, AttachmentProcessorDataCollector, Version=.*, Culture=neutral, PublicKeyToken=null"));
        Assert.AreEqual(Path.Combine(version2, Path.GetFileName(dataCollectorFilePath)), dataCollectorAttachmentsProcessors[0].DataCollectorAttachmentProcessorInstance.GetType().Assembly.Location);
        Assert.AreEqual(typeof(CodeCoverageDataAttachmentsHandler).AssemblyQualifiedName, dataCollectorAttachmentsProcessors[1].DataCollectorAttachmentProcessorInstance.GetType().AssemblyQualifiedName);
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

    [DataCollectorFriendlyName("Sample")]
    [DataCollectorTypeUri("datacollector://Sample")]
    [DataCollectorAttachmentProcessor(typeof(DataCollectorAttachmentProcessor))]
    public class SampleDataCollector : DataCollector
    {
        public override void Initialize(
            XmlElement? configurationElement,
            DataCollectionEvents events,
            DataCollectionSink dataSink,
            DataCollectionLogger logger,
            DataCollectionEnvironmentContext? environmentContext)
        {

        }
    }

    [DataCollectorFriendlyName("SampleData2")]
    [DataCollectorTypeUri("datacollector://SampleData2")]
    [DataCollectorAttachmentProcessor(typeof(DataCollectorAttachmentProcessor))]
    public class SampleData2Collector : DataCollector
    {
        public override void Initialize(
            XmlElement? configurationElement,
            DataCollectionEvents events,
            DataCollectionSink dataSink,
            DataCollectionLogger logger,
            DataCollectionEnvironmentContext? environmentContext)
        {

        }
    }

    [DataCollectorFriendlyName("SampleData3")]
    [DataCollectorTypeUri("datacollector://SampleData3")]
    [DataCollectorAttachmentProcessor(typeof(DataCollectorAttachmentProcessor2))]
    public class SampleData3Collector : DataCollector
    {
        public override void Initialize(
            XmlElement? configurationElement,
            DataCollectionEvents events,
            DataCollectionSink dataSink,
            DataCollectionLogger logger,
            DataCollectionEnvironmentContext? environmentContext)
        {

        }
    }

    [DataCollectorFriendlyName("SampleData4")]
    [DataCollectorTypeUri("datacollector://SampleData4")]
    [DataCollectorAttachmentProcessor(typeof(string))]
    public class SampleData4Collector : DataCollector
    {
        public override void Initialize(
            XmlElement? configurationElement,
            DataCollectionEvents events,
            DataCollectionSink dataSink,
            DataCollectionLogger logger,
            DataCollectionEnvironmentContext? environmentContext)
        {

        }
    }

    [DataCollectorFriendlyName("SampleData5")]
    [DataCollectorTypeUri("datacollector://microsoft/CodeCoverage/2.0")]
    [DataCollectorAttachmentProcessor(typeof(DataCollectorAttachmentProcessor))]
    public class SampleData5Collector : DataCollector
    {
        public override void Initialize(
            XmlElement? configurationElement,
            DataCollectionEvents events,
            DataCollectionSink dataSink,
            DataCollectionLogger logger,
            DataCollectionEnvironmentContext? environmentContext)
        {

        }
    }

    public class DataCollectorAttachmentProcessor : IDataCollectorAttachmentProcessor
    {
        public bool SupportsIncrementalProcessing => throw new NotImplementedException();

        public IEnumerable<Uri> GetExtensionUris()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<AttachmentSet>> ProcessAttachmentSetsAsync(XmlElement configurationElement, ICollection<AttachmentSet> attachments, IProgress<int> progressReporter, IMessageLogger logger, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class DataCollectorAttachmentProcessor2 : IDataCollectorAttachmentProcessor
    {
        public bool SupportsIncrementalProcessing => throw new NotImplementedException();

        public IEnumerable<Uri> GetExtensionUris()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<AttachmentSet>> ProcessAttachmentSetsAsync(XmlElement configurationElement, ICollection<AttachmentSet> attachments, IProgress<int> progressReporter, IMessageLogger logger, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
