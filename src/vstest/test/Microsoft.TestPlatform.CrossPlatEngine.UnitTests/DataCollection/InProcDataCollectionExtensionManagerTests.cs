// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollector.InProcDataCollector;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Constants = Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Constants;

namespace TestPlatform.CrossPlatEngine.UnitTests.DataCollection;

[TestClass]
public class InProcDataCollectionExtensionManagerTests
{
    private static readonly string Temp = Path.GetTempPath();
    private readonly string _settingsXml = @"<RunSettings>
                                    <InProcDataCollectionRunSettings>
                                        <InProcDataCollectors>
                                            <InProcDataCollector friendlyName='Test Impact' uri='InProcDataCollector://Microsoft/TestImpact/1.0' assemblyQualifiedName='TestImpactListener.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7ccb7239ffde675a'  codebase='E:\repos\MSTest\src\managed\TestPlatform\TestImpactListener.Tests\bin\Debug\TestImpactListener.Tests.dll'>
                                                <Configuration>
                                                    <Port>4312</Port>
                                                </Configuration>
                                            </InProcDataCollector>
                                        </InProcDataCollectors>
                                    </InProcDataCollectionRunSettings>
                                </RunSettings>";
    private readonly Mock<ITestEventsPublisher> _mockTestEventsPublisher;
    private readonly string _defaultCodebase = Path.Combine(Temp, "repos", "MSTest", "src", "managed", "TestPlatform", "TestImpactListener.Tests", "bin", "Debug");
    private readonly Mock<IFileHelper> _mockFileHelper;
    private readonly TestPluginCache _testPluginCache;

    private TestableInProcDataCollectionExtensionManager _inProcDataCollectionManager;

    public InProcDataCollectionExtensionManagerTests()
    {
        _mockTestEventsPublisher = new Mock<ITestEventsPublisher>();
        _mockFileHelper = new Mock<IFileHelper>();
        _testPluginCache = TestPluginCache.Instance;
        _inProcDataCollectionManager = new TestableInProcDataCollectionExtensionManager(_settingsXml, _mockTestEventsPublisher.Object, _defaultCodebase, _testPluginCache, _mockFileHelper.Object);
    }

    [TestMethod]
    public void CodeBasePathsAreDeduplicatedWithCaseIgnoring()
    {
        var testPluginCache = new TestableTestPluginCache();
        // the boolean argument refers to adding the paths to which list(we have two lists)and the duplicate happened when we merged the two lists and they had the same path
        testPluginCache.UpdateExtensions(new List<string> { Path.Combine(Temp, "DEDUPLICATINGWITHCASEIGNORING1", "Collector.dll") }, false);
        var directory1 = Path.Combine(Temp, "DeduplicatingWithCaseIgnoring1");
        var directory2 = Path.Combine(Temp, "DeduplicatingWithCaseIgnoring2");
        testPluginCache.UpdateExtensions(new List<string> { Path.Combine(directory1, "Collector.dll"), Path.Combine(directory2, "Collector.dll") }, true);

        var inProcDataCollectionExtensionManager = new TestableInProcDataCollectionExtensionManager(_settingsXml, _mockTestEventsPublisher.Object, null, testPluginCache, _mockFileHelper.Object);

        Assert.AreEqual(3, inProcDataCollectionExtensionManager.CodeBasePaths.Count); // "CodeBasePaths" contains the two extensions(after removing duplicates) and the "_defaultCodebase"

        Assert.IsTrue(inProcDataCollectionExtensionManager.CodeBasePaths.Contains(null));
        Assert.IsTrue(inProcDataCollectionExtensionManager.CodeBasePaths.Contains(directory1));
        Assert.IsTrue(inProcDataCollectionExtensionManager.CodeBasePaths.Contains(directory2));
    }


    [TestMethod]
    public void InProcDataCollectionExtensionManagerShouldLoadsDataCollectorsFromRunSettings()
    {
        var dataCollector = (MockDataCollector)_inProcDataCollectionManager.InProcDataCollectors.First().Value;

        Assert.IsTrue(_inProcDataCollectionManager.IsInProcDataCollectionEnabled, "InProcDataCollection must be enabled if runsettings contains inproc datacollectors.");
        Assert.AreEqual(1, _inProcDataCollectionManager.InProcDataCollectors.Count, "One Datacollector must be registered");

        Equals(dataCollector.AssemblyQualifiedName, "TestImpactListener.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7ccb7239ffde675a");
        Equals(dataCollector.CodeBase, Path.Combine(Temp, "repos", "MSTest", "src", "managed", "TestPlatform", "TestImpactListener.Tests", "bin", "Debug", "TestImpactListener.Tests.dll"));
        Equals(dataCollector.Configuration.OuterXml, @"<Configuration><Port>4312</Port></Configuration>");
    }

    [TestMethod]
    public void InProcDataCollectionExtensionManagerLoadsDataCollectorFromDefaultCodebaseIfExistsAndCodebaseIsRelative()
    {
        string settingsXml = @"<RunSettings>
                                    <InProcDataCollectionRunSettings>
                                        <InProcDataCollectors>
                                            <InProcDataCollector friendlyName='Test Impact' uri='InProcDataCollector://Microsoft/TestImpact/1.0' assemblyQualifiedName='TestImpactListener.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7ccb7239ffde675a'  codebase='TestImpactListener.Tests.dll'>
                                                <Configuration>
                                                    <Port>4312</Port>
                                                </Configuration>
                                            </InProcDataCollector>
                                        </InProcDataCollectors>
                                    </InProcDataCollectionRunSettings>
                                </RunSettings>";

        _mockFileHelper.Setup(fh => fh.Exists(Path.Combine(new[] { Temp, "repos", "MSTest", "src", "managed", "TestPlatform", "TestImpactListener.Tests", "bin", "Debug", "TestImpactListener.Tests.dll" }))).Returns(true);
        _inProcDataCollectionManager = new TestableInProcDataCollectionExtensionManager(settingsXml, _mockTestEventsPublisher.Object, _defaultCodebase, _testPluginCache, _mockFileHelper.Object);

        var codebase = ((MockDataCollector)_inProcDataCollectionManager.InProcDataCollectors.Values.First()).CodeBase;
        Assert.AreEqual(Path.Combine(Temp, "repos", "MSTest", "src", "managed", "TestPlatform", "TestImpactListener.Tests", "bin", "Debug", "TestImpactListener.Tests.dll"), codebase);
    }

    [TestMethod]
    public void InProcDataCollectionExtensionManagerLoadsDataCollectorFromTestPluginCacheIfExistsAndCodebaseIsRelative()
    {
        string settingsXml = @"<RunSettings>
                                    <InProcDataCollectionRunSettings>
                                        <InProcDataCollectors>
                                            <InProcDataCollector friendlyName='Test Impact' uri='InProcDataCollector://Microsoft/TestImpact/1.0' assemblyQualifiedName='TestImpactListener.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7ccb7239ffde675a'  codebase='TestImpactListenerDataCollector.dll'>
                                                <Configuration>
                                                    <Port>4312</Port>
                                                </Configuration>
                                            </InProcDataCollector>
                                        </InProcDataCollectors>
                                    </InProcDataCollectionRunSettings>
                                </RunSettings>";

        _testPluginCache.UpdateExtensions(new List<string> { Path.Combine(Temp, "source", ".nuget", "TestImpactListenerDataCollector.dll") }, true);
        _mockFileHelper.Setup(fh => fh.Exists(Path.Combine(Temp, "source", ".nuget", "TestImpactListenerDataCollector.dll"))).Returns(true);

        _inProcDataCollectionManager = new TestableInProcDataCollectionExtensionManager(settingsXml, _mockTestEventsPublisher.Object, _defaultCodebase, _testPluginCache, _mockFileHelper.Object);

        var codebase = ((MockDataCollector)_inProcDataCollectionManager.InProcDataCollectors.Values.First()).CodeBase;
        Assert.AreEqual(Path.Combine(Temp, "source", ".nuget", "TestImpactListenerDataCollector.dll"), codebase);
    }

    [TestMethod]
    public void InProcDataCollectionExtensionManagerLoadsDataCollectorFromGivenCodebaseIfCodebaseIsAbsolute()
    {
        string settingsXml = @"<RunSettings>
                                    <InProcDataCollectionRunSettings>
                                        <InProcDataCollectors>
                                            <InProcDataCollector friendlyName='Test Impact' uri='InProcDataCollector://Microsoft/TestImpact/1.0' assemblyQualifiedName='TestImpactListener.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7ccb7239ffde675a'  codebase='\\DummyPath\TestImpactListener.Tests.dll'>
                                                <Configuration>
                                                    <Port>4312</Port>
                                                </Configuration>
                                            </InProcDataCollector>
                                        </InProcDataCollectors>
                                    </InProcDataCollectionRunSettings>
                                </RunSettings>";
        _inProcDataCollectionManager = new TestableInProcDataCollectionExtensionManager(settingsXml, _mockTestEventsPublisher.Object, _defaultCodebase, _testPluginCache, _mockFileHelper.Object);

        var codebase = ((MockDataCollector)_inProcDataCollectionManager.InProcDataCollectors.Values.First()).CodeBase;
        Assert.AreEqual("\\\\DummyPath\\TestImpactListener.Tests.dll", codebase);
    }

    [TestMethod]
    public void InProcDataCollectorIsReadingMultipleDataCollector()
    {
        var temp = Path.GetTempPath();
        var path1 = Path.Combine(temp, "repos", "MSTest", "src", "managed", "TestPlatform", "TestImpactListener.Tests", "bin", "Debug", "TestImpactListener.Tests1.dll");
        var path2 = Path.Combine(temp, "repos", "MSTest", "src", "managed", "TestPlatform", "TestImpactListener.Tests", "bin", "Debug", "TestImpactListener.Tests2.dll");
        var multiSettingsXml = $@"<RunSettings>
                                    <InProcDataCollectionRunSettings>
                                        <InProcDataCollectors>
                                            <InProcDataCollector friendlyName='Test Impact' uri='InProcDataCollector://Microsoft/TestImpact/1.0' assemblyQualifiedName='TestImpactListener.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7ccb7239ffde675a'  codebase='{path1}'>
                                                <Configuration>
                                                    <Port>4312</Port>
                                                </Configuration>
                                            </InProcDataCollector>
                                            <InProcDataCollector friendlyName='InProcDataCol' uri='InProcDataCollector://Microsoft/InProcDataCol/2.0' assemblyQualifiedName='TestImpactListener.Tests, Version=2.0.0.0, Culture=neutral, PublicKeyToken=7ccb7239ffde675a'  codebase='{path2}'>
                                                <Configuration>
                                                    <Port>4313</Port>
                                                </Configuration>
                                            </InProcDataCollector>
                                        </InProcDataCollectors>
                                    </InProcDataCollectionRunSettings>
                                </RunSettings>";

        _inProcDataCollectionManager = new TestableInProcDataCollectionExtensionManager(multiSettingsXml, _mockTestEventsPublisher.Object, _defaultCodebase, _testPluginCache, _mockFileHelper.Object);
        bool secondOne = false;
        MockDataCollector? dataCollector1 = null;
        MockDataCollector? dataCollector2 = null;

        foreach (var inProcDc in _inProcDataCollectionManager.InProcDataCollectors.Values)
        {
            if (secondOne)
            {
                dataCollector2 = inProcDc as MockDataCollector;
            }
            else
            {
                dataCollector1 = inProcDc as MockDataCollector;
                secondOne = true;
            }
        }

        Assert.IsTrue(string.Equals(dataCollector1!.AssemblyQualifiedName, "TestImpactListener.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7ccb7239ffde675a", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(string.Equals(dataCollector1.CodeBase, Path.Combine(temp, "repos", "MSTest", "src", "managed", "TestPlatform", "TestImpactListener.Tests", "bin", "Debug", "TestImpactListener.Tests1.dll"), StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(string.Equals(dataCollector1.Configuration.OuterXml, @"<Configuration><Port>4312</Port></Configuration>", StringComparison.OrdinalIgnoreCase));

        Assert.IsTrue(string.Equals(dataCollector2!.AssemblyQualifiedName, "TestImpactListener.Tests, Version=2.0.0.0, Culture=neutral, PublicKeyToken=7ccb7239ffde675a", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(string.Equals(dataCollector2.CodeBase, Path.Combine(temp, "repos", "MSTest", "src", "managed", "TestPlatform", "TestImpactListener.Tests", "bin", "Debug", "TestImpactListener.Tests2.dll"), StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(string.Equals(dataCollector2.Configuration.OuterXml, @"<Configuration><Port>4313</Port></Configuration>", StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    public void InProcDataCollectionExtensionManagerWillNotEnableDataCollectionForInavlidSettingsXml()
    {
        var invalidSettingsXml = @"<RunSettings>
                                    <InProcDataCollectionRunSettings>
                                        <InProcDataCollectors>
                                            <InProcDataCollector friendlyName='Test Impact' uri='InProcDataCollector://Microsoft/TestImpact/1.0' assemblyQualifiedName='TestImpactListener.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7ccb7239ffde675a'  codebase='E:\repos\MSTest\src\managed\TestPlatform\TestImpactListener.Tests\bin\Debug\TestImpactListener.Tests.dll' value='Invalid'>
                                                <Configuration>
                                                    <Port>4312</Port>
                                                </Configuration>
                                            </InProcDataCollector>
                                        </InProcDataCollectors>
                                    </InProcDataCollectionRunSettings>
                                </RunSettings>";

        var manager = new InProcDataCollectionExtensionManager(invalidSettingsXml, _mockTestEventsPublisher.Object, _defaultCodebase, _testPluginCache);
        Assert.IsFalse(manager.IsInProcDataCollectionEnabled, "InProcDataCollection must be disabled on invalid settings.");
    }
    [TestMethod]
    public void TriggerSessionStartShouldBeCalledWithCorrectTestSources()
    {
        var properties = new Dictionary<string, object?>
        {
            { "TestSources", new List<string>() { "testsource1.dll", "testsource2.dll" } }
        };

        var mockDataCollector = (MockDataCollector)_inProcDataCollectionManager.InProcDataCollectors.Values.First();

        _mockTestEventsPublisher.Raise(x => x.SessionStart += null, new SessionStartEventArgs(properties));
        Assert.IsTrue((mockDataCollector.TestSessionStartCalled == 1), "TestSessionStart must be called on datacollector");

        Assert.IsNotNull(mockDataCollector.TestSources);
        Assert.IsTrue(mockDataCollector.TestSources.Contains("testsource1.dll"));
        Assert.IsTrue(mockDataCollector.TestSources.Contains("testsource2.dll"));
    }


    [TestMethod]
    public void TriggerSessionStartShouldCallInProcDataCollector()
    {
        _mockTestEventsPublisher.Raise(x => x.SessionStart += null, new SessionStartEventArgs());

        var mockDataCollector = (MockDataCollector)_inProcDataCollectionManager.InProcDataCollectors.Values.First();
        Assert.IsTrue((mockDataCollector.TestSessionStartCalled == 1), "TestSessionStart must be called on datacollector");
        Assert.IsTrue((mockDataCollector.TestSessionEndCalled == 0), "TestSessionEnd must NOT be called on datacollector");
        Assert.IsTrue((mockDataCollector.TestCaseStartCalled == 0), "TestCaseStart must NOT be called on datacollector");
        Assert.IsTrue((mockDataCollector.TestCaseEndCalled == 0), "TestCaseEnd must NOT be called on datacollector");
    }

    [TestMethod]
    public void TriggerSessionEndShouldCallInProcDataCollector()
    {
        _mockTestEventsPublisher.Raise(x => x.SessionEnd += null, new SessionEndEventArgs());

        var mockDataCollector = (MockDataCollector)_inProcDataCollectionManager.InProcDataCollectors.Values.First();
        Assert.IsTrue((mockDataCollector.TestSessionStartCalled == 0), "TestSessionEnd must NOT be called on datacollector");
        Assert.IsTrue((mockDataCollector.TestSessionEndCalled == 1), "TestSessionStart must be called on datacollector");
        Assert.IsTrue((mockDataCollector.TestCaseStartCalled == 0), "TestCaseStart must NOT be called on datacollector");
        Assert.IsTrue((mockDataCollector.TestCaseEndCalled == 0), "TestCaseEnd must NOT be called on datacollector");
    }

    [TestMethod]
    public void TriggerTestCaseStartShouldCallInProcDataCollector()
    {
        var testCase = new TestCase("x.y.z", new Uri("uri://dummy"), "x.dll");
        // random guid
        testCase.Id = new Guid("3871B3B0-2853-406B-BB61-1FE1764116FD");
        _mockTestEventsPublisher.Raise(x => x.TestCaseStart += null, new TestCaseStartEventArgs(testCase));

        var mockDataCollector = (MockDataCollector)_inProcDataCollectionManager.InProcDataCollectors.Values.First();

        Assert.IsTrue((mockDataCollector.TestCaseStartCalled == 1), "TestCaseStart must be called on datacollector");
        Assert.IsTrue((mockDataCollector.TestCaseEndCalled == 0), "TestCaseEnd must NOT be called on datacollector");
    }

    [TestMethod]
    public void TriggerTestCaseEndShouldtBeCalledMultipleTimesInDataDrivenScenario()
    {
        var testCase = new TestCase("x.y.z", new Uri("uri://dummy"), "x.dll");
        // random guid
        testCase.Id = new Guid("3871B3B0-2853-406B-BB61-1FE1764116FD");
        _mockTestEventsPublisher.Raise(x => x.TestCaseStart += null, new TestCaseStartEventArgs(testCase));
        _mockTestEventsPublisher.Raise(x => x.TestCaseEnd += null, new TestCaseEndEventArgs(testCase, TestOutcome.Passed));
        _mockTestEventsPublisher.Raise(x => x.TestCaseStart += null, new TestCaseStartEventArgs(testCase));
        _mockTestEventsPublisher.Raise(x => x.TestCaseEnd += null, new TestCaseEndEventArgs(testCase, TestOutcome.Failed));

        var mockDataCollector = (MockDataCollector)_inProcDataCollectionManager.InProcDataCollectors.Values.First();
        Assert.IsTrue((mockDataCollector.TestCaseStartCalled == 2), "TestCaseStart must only be called once");
        Assert.IsTrue((mockDataCollector.TestCaseEndCalled == 2), "TestCaseEnd must only be called once");
    }

    internal class TestableInProcDataCollectionExtensionManager : InProcDataCollectionExtensionManager
    {
        public TestableInProcDataCollectionExtensionManager(string runSettings, ITestEventsPublisher mockTestEventsPublisher, string? defaultCodebase, TestPluginCache testPluginCache, IFileHelper fileHelper)
            : base(runSettings, mockTestEventsPublisher, defaultCodebase, testPluginCache, fileHelper)
        {
        }

        protected override IInProcDataCollector CreateDataCollector(string assemblyQualifiedName, string codebase, XmlElement configuration, Type interfaceType)
        {
            return new MockDataCollector(assemblyQualifiedName, codebase, configuration);
        }
    }

    public class MockDataCollector : IInProcDataCollector
    {
        public MockDataCollector(string assemblyQualifiedName, string codebase, XmlElement configuration)
        {
            AssemblyQualifiedName = assemblyQualifiedName;
            CodeBase = codebase;
            Configuration = configuration;
        }

        public string AssemblyQualifiedName { get; private set; }
        public string CodeBase { get; private set; }
        public XmlElement Configuration { get; private set; }
        public int TestSessionStartCalled { get; private set; }
        public int TestSessionEndCalled { get; private set; }
        public int TestCaseStartCalled { get; private set; }
        public int TestCaseEndCalled { get; private set; }
        public IEnumerable<string>? TestSources { get; private set; }

        public void LoadDataCollector(IDataCollectionSink inProcDataCollectionSink)
        {
            // Do Nothing
        }

        public void TriggerInProcDataCollectionMethod(string methodName, InProcDataCollectionArgs methodArg)
        {
            switch (methodName)
            {
                case Constants.TestSessionStartMethodName: TestSessionStartMethodCalled((TestSessionStartArgs)methodArg); break;
                case Constants.TestSessionEndMethodName: TestSessionEndCalled++; break;
                case Constants.TestCaseStartMethodName: TestCaseStartCalled++; break;
                case Constants.TestCaseEndMethodName: TestCaseEndCalled++; break;
                default: break;
            }
        }

        private void TestSessionStartMethodCalled(TestSessionStartArgs testSessionStartArgs)
        {
            TestSessionStartCalled++;
            TestSources = testSessionStartArgs.GetPropertyValue<IEnumerable<string>>("TestSources");
        }
    }
}
