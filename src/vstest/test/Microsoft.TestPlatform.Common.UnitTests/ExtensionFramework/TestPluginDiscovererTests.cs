// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.Common.UnitTests.ExtensionFramework;

[TestClass]
public class TestPluginDiscovererTests
{
    [TestMethod]
    public void GetTestExtensionsInformationShouldNotThrowOnALoadException()
    {
        var pathToExtensions = new List<string> { "foo.dll" };

        // The below should not throw an exception.
        Assert.IsNotNull(TestPluginDiscoverer.GetTestExtensionsInformation<TestLoggerPluginInformation, ITestLogger>(pathToExtensions));
    }

    [TestMethod]
    public void GetTestExtensionsInformationShouldNotConsiderAbstractClasses()
    {
        var pathToExtensions = new List<string> { typeof(TestPluginDiscovererTests).Assembly.Location };

        // The below should not throw an exception.
        var testExtensions = TestPluginDiscoverer.GetTestExtensionsInformation<TestDiscovererPluginInformation, ITestDiscoverer>(pathToExtensions);
        var discovererPluginInformation = new TestDiscovererPluginInformation(typeof(AbstractTestDiscoverer));
        Assert.IsFalse(testExtensions.ContainsKey(discovererPluginInformation.IdentifierData!));
    }

    [TestMethod]
    public void GetTestExtensionsInformationShouldReturnDiscovererExtensions()
    {
        var pathToExtensions = new List<string> { typeof(TestPluginDiscovererTests).Assembly.Location };

        // The below should not throw an exception.
        var testExtensions = TestPluginDiscoverer.GetTestExtensionsInformation<TestDiscovererPluginInformation, ITestDiscoverer>(pathToExtensions);

        var discovererPluginInformation = new TestDiscovererPluginInformation(typeof(ValidDiscoverer));
        var discovererPluginInformation2 = new TestDiscovererPluginInformation(typeof(ValidDiscoverer2));

        Assert.IsTrue(testExtensions.ContainsKey(discovererPluginInformation.IdentifierData!));
        Assert.IsTrue(testExtensions.ContainsKey(discovererPluginInformation2.IdentifierData!));
    }

    [TestMethod]
    public void GetTestExtensionsInformationShouldReturnExecutorExtensions()
    {
        var pathToExtensions = new List<string> { typeof(TestPluginDiscovererTests).Assembly.Location };

        // The below should not throw an exception.
        var testExtensions = TestPluginDiscoverer.GetTestExtensionsInformation<TestExecutorPluginInformation, ITestExecutor>(pathToExtensions);

        var pluginInformation = new TestExecutorPluginInformation(typeof(ValidExecutor));
        var pluginInformation2 = new TestExecutorPluginInformation(typeof(ValidExecutor2));

        Assert.AreEqual(2, testExtensions.Keys.Count(k => k.Contains("ValidExecutor")));
        Assert.IsTrue(testExtensions.ContainsKey(pluginInformation.IdentifierData!));
        Assert.IsTrue(testExtensions.ContainsKey(pluginInformation2.IdentifierData!));
    }

    [TestMethod]
    public void GetTestExtensionsInformationShouldReturnLoggerExtensions()
    {
        var pathToExtensions = new List<string> { typeof(TestPluginDiscovererTests).Assembly.Location };

        // The below should not throw an exception.
        var testExtensions = TestPluginDiscoverer.GetTestExtensionsInformation<TestLoggerPluginInformation, ITestLogger>(pathToExtensions);

        var pluginInformation = new TestLoggerPluginInformation(typeof(ValidLogger));
        var pluginInformation2 = new TestLoggerPluginInformation(typeof(ValidLogger2));

        Assert.AreEqual(1, testExtensions.Keys.Count(k => k.Contains("csv")));
        Assert.IsTrue(testExtensions.ContainsKey(pluginInformation.IdentifierData!));
    }

    [TestMethod]
    public void GetTestExtensionsInformationShouldReturnDataCollectorExtensionsAndIgnoresInvalidDataCollectors()
    {
        var pathToExtensions = new List<string> { typeof(TestPluginDiscovererTests).Assembly.Location };

        // The below should not throw an exception.
        var testExtensions = TestPluginDiscoverer.GetTestExtensionsInformation<DataCollectorConfig, DataCollector>(pathToExtensions);

        var pluginInformation = new DataCollectorConfig(typeof(ValidDataCollector));

        Assert.AreEqual(2, testExtensions.Keys.Count);
        Assert.AreEqual(1, testExtensions.Keys.Count(k => k.Equals("datacollector://foo/bar")));
        Assert.AreEqual(1, testExtensions.Keys.Count(k => k.Equals("datacollector://foo/bar1")));
    }

    [TestMethod]
    public void GetTestExtensionsInformationShouldReturnSettingsProviderExtensions()
    {
        var pathToExtensions = new List<string> { typeof(TestPluginDiscovererTests).Assembly.Location };

        // The below should not throw an exception.
        var testExtensions = TestPluginDiscoverer.GetTestExtensionsInformation<TestSettingsProviderPluginInformation, ISettingsProvider>(pathToExtensions);

        var pluginInformation = new TestSettingsProviderPluginInformation(typeof(ValidSettingsProvider));
        var pluginInformation2 = new TestSettingsProviderPluginInformation(typeof(ValidSettingsProvider2));

        Assert.IsTrue(testExtensions.Keys.Select(k => k.Contains("ValidSettingsProvider")).Count() >= 3);
        Assert.IsTrue(testExtensions.ContainsKey(pluginInformation.IdentifierData!));
        Assert.IsTrue(testExtensions.ContainsKey(pluginInformation2.IdentifierData!));
    }

    [TestMethod]
    public void GetTestExtensionsInformationShouldNotAbortOnFaultyExtensions()
    {
        var pathToExtensions = new List<string>
        {
            typeof(TestPluginDiscovererTests).Assembly.Location,
        };

        _ = TestPluginDiscoverer.GetTestExtensionsInformation<FaultyTestExecutorPluginInformation, ITestExecutor>(pathToExtensions);

        _ = TestPluginDiscoverer.GetTestExtensionsInformation<FaultyTestExecutorPluginInformation, ITestExecutor>(pathToExtensions);
    }

    #region Implementations

    #region Discoverers

    private abstract class AbstractTestDiscoverer : ITestDiscoverer
    {
        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            throw new NotImplementedException();
        }
    }

    private class ValidDiscoverer : ITestDiscoverer
    {
        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            throw new NotImplementedException();
        }
    }

    private class ValidDiscoverer2 : ITestDiscoverer
    {
        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region Executors

    [ExtensionUri("ValidExecutor")]
    private class ValidExecutor : ITestExecutor
    {
        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<string>? sources, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<TestCase>? tests, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            throw new NotImplementedException();
        }
    }

    [ExtensionUri("ValidExecutor2")]
    private class ValidExecutor2 : ITestExecutor
    {
        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<string>? sources, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<TestCase>? tests, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            throw new NotImplementedException();
        }
    }

    [ExtensionUri("ValidExecutor")]
    private class DuplicateExecutor : ITestExecutor
    {
        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<string>? sources, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<TestCase>? tests, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region Loggers

    [ExtensionUri("csv")]
    private class ValidLogger : ITestLogger
    {
        public void Initialize(TestLoggerEvents events, string testRunDirectory)
        {
            throw new NotImplementedException();
        }
    }

    [ExtensionUri("docx")]
    private class ValidLogger2 : ITestLogger
    {
        public void Initialize(TestLoggerEvents events, string testRunDirectory)
        {
            throw new NotImplementedException();
        }
    }

    [ExtensionUri("csv")]
    private class DuplicateLogger : ITestLogger
    {
        public void Initialize(TestLoggerEvents events, string testRunDirectory)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region Settings Providers

    [SettingsName("ValidSettingsProvider")]
    private class ValidSettingsProvider : ISettingsProvider
    {
        public void Load(XmlReader reader)
        {
            throw new NotImplementedException();
        }
    }

    [SettingsName("ValidSettingsProvider2")]
    private class ValidSettingsProvider2 : ISettingsProvider
    {
        public void Load(XmlReader reader)
        {
            throw new NotImplementedException();
        }
    }

    [SettingsName("ValidSettingsProvider")]
    private class DuplicateSettingsProvider : ISettingsProvider
    {
        public void Load(XmlReader reader)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region  DataCollectors

    public class InvalidDataCollector : DataCollector
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

    /// <summary>
    /// The a data collector inheriting from another data collector.
    /// </summary>
    [DataCollectorFriendlyName("Foo1")]
    [DataCollectorTypeUri("datacollector://foo/bar1")]
    public class ADataCollectorInheritingFromAnotherDataCollector : InvalidDataCollector
    {
    }

    [DataCollectorFriendlyName("Foo")]
    [DataCollectorTypeUri("datacollector://foo/bar")]
    [DataCollectorAttachmentProcessor(typeof(DataCollectorAttachmentProcessor))]
    public class ValidDataCollector : DataCollector
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
    #endregion

    internal class FaultyTestExecutorPluginInformation : TestExtensionPluginInformation
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="type"> The Type. </param>
        public FaultyTestExecutorPluginInformation(Type type) : base(type)
        {
            throw new Exception();
        }
    }
    #endregion
}
