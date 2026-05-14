// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Reflection;

using Coverlet.Collector.DataCollection;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollector.InProcDataCollector;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.InProcDataCollector;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.CrossPlatEngine.UnitTests.DataCollection;

[TestClass]
public class InProcDataCollectorTests
{
    private readonly Mock<IAssemblyLoadContext> _assemblyLoadContext;

    private IInProcDataCollector? _inProcDataCollector;

    public InProcDataCollectorTests()
    {
        _assemblyLoadContext = new Mock<IAssemblyLoadContext>();
    }

    [TestMethod]
    public void InProcDataCollectorShouldNotThrowExceptionIfInvalidAssemblyIsProvided()
    {
        _assemblyLoadContext.Setup(alc => alc.LoadAssemblyFromPath(It.IsAny<string>()))
            .Throws<FileNotFoundException>();

        _inProcDataCollector = new InProcDataCollector(
            string.Empty,
            string.Empty,
            null!,
            string.Empty,
            _assemblyLoadContext.Object,
            TestPluginCache.Instance);

        Assert.IsNull(_inProcDataCollector.AssemblyQualifiedName);
    }

    [TestMethod]
    public void InProcDataCollectorShouldNotThrowExceptionIfAssemblyDoesNotContainAnyInProcDataCollector()
    {
        _assemblyLoadContext.Setup(alc => alc.LoadAssemblyFromPath(It.IsAny<string>()))
            .Returns(Assembly.GetEntryAssembly()!);

        _inProcDataCollector = new InProcDataCollector(
            string.Empty,
            string.Empty,
            null!,
            string.Empty,
            _assemblyLoadContext.Object,
            TestPluginCache.Instance);

        Assert.IsNull(_inProcDataCollector.AssemblyQualifiedName);
    }

    [TestMethod]
    public void InProcDataCollectorShouldInitializeIfAssemblyContainsAnyInProcDataCollector()
    {
        var type = typeof(TestableInProcDataCollector);

        _assemblyLoadContext.Setup(alc => alc.LoadAssemblyFromPath(It.IsAny<string>()))
            .Returns(type.Assembly);

        _inProcDataCollector = new InProcDataCollector(
            string.Empty,
            type.AssemblyQualifiedName!,
            type,
            string.Empty,
            _assemblyLoadContext.Object,
            TestPluginCache.Instance);

        Assert.IsNotNull(_inProcDataCollector.AssemblyQualifiedName);
        Assert.AreEqual(_inProcDataCollector.AssemblyQualifiedName, type.AssemblyQualifiedName);
    }

    [TestMethod]
    public void InProcDataCollectorLoadCoverlet()
    {
        var type = typeof(CoverletInProcDataCollector);

        Assert.AreEqual("9.9.9.9", type.Assembly.GetName().Version!.ToString());

        _assemblyLoadContext.Setup(alc => alc.LoadAssemblyFromPath(It.IsAny<string>()))
            .Returns(type.Assembly);

        // We need to mock TestPluginCache because we have to create assembly resolver instance
        // using SetupAssemblyResolver method, we don't use any other method of class(like DiscoverTestExtensions etc...)
        // that fire creation
        TestableTestPluginCache testablePlugin = new();
        testablePlugin.SetupAssemblyResolver(type.Assembly.Location);

        _inProcDataCollector = new InProcDataCollector(
            type.Assembly.Location,
            "Coverlet.Collector.DataCollection.CoverletInProcDataCollector, coverlet.collector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            typeof(InProcDataCollection),
            string.Empty,
            _assemblyLoadContext.Object,
            testablePlugin);

        Assert.IsNotNull(_inProcDataCollector.AssemblyQualifiedName);
        Assert.AreEqual(_inProcDataCollector.AssemblyQualifiedName, type.AssemblyQualifiedName);
    }

    private class TestableInProcDataCollector : InProcDataCollection
    {
        public void Initialize(IDataCollectionSink dataCollectionSink)
        {
            throw new System.NotImplementedException();
        }

        public void TestSessionStart(TestSessionStartArgs testSessionStartArgs)
        {
            throw new System.NotImplementedException();
        }

        public void TestCaseStart(TestCaseStartArgs testCaseStartArgs)
        {
            throw new System.NotImplementedException();
        }

        public void TestCaseEnd(TestCaseEndArgs testCaseEndArgs)
        {
            throw new System.NotImplementedException();
        }

        public void TestSessionEnd(TestSessionEndArgs testSessionEndArgs)
        {
            throw new System.NotImplementedException();
        }
    }
}
