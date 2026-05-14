// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector;

/// <summary>
/// Encapsulates datacollector object and other objects required to facilitate datacollection.
/// </summary>
internal class DataCollectorInformation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectorInformation"/> class.
    /// </summary>
    /// <param name="dataCollector">
    /// The data collector.
    /// </param>
    /// <param name="configurationElement">
    /// The configuration element.
    /// </param>
    /// <param name="dataCollectorConfig">
    /// The data collector config.
    /// </param>
    /// <param name="environmentContext">
    /// The environment Context.
    /// </param>
    /// <param name="attachmentManager">
    /// The attachment Manager.
    /// </param>
    /// <param name="events">
    /// The events.
    /// </param>
    /// <param name="messageSink">
    /// The message Sink.
    /// </param>
    /// <param name="settingsXml"></param>
    internal DataCollectorInformation(ObjectModel.DataCollection.DataCollector dataCollector, XmlElement? configurationElement, DataCollectorConfig dataCollectorConfig, DataCollectionEnvironmentContext? environmentContext, IDataCollectionAttachmentManager attachmentManager, TestPlatformDataCollectionEvents events, IMessageSink messageSink, string settingsXml)
    {
        DataCollector = dataCollector;
        ConfigurationElement = configurationElement;
        DataCollectorConfig = dataCollectorConfig;
        Events = events;
        EnvironmentContext = environmentContext;
        DataCollectionSink = new TestPlatformDataCollectionSink(attachmentManager, dataCollectorConfig);
        Logger = new TestPlatformDataCollectionLogger(messageSink, dataCollectorConfig);
        SettingsXml = settingsXml;
    }

    /// <summary>
    /// Gets or sets the data collector.
    /// </summary>
    public ObjectModel.DataCollection.DataCollector DataCollector { get; set; }

    /// <summary>
    /// Gets or sets the configuration element.
    /// </summary>
    public XmlElement? ConfigurationElement { get; set; }

    /// <summary>
    /// Gets or sets the data collector config.
    /// </summary>
    public DataCollectorConfig DataCollectorConfig { get; set; }

    /// <summary>
    /// Gets the events object on which the collector registers for events
    /// </summary>
    public TestPlatformDataCollectionEvents Events { get; private set; }

    /// <summary>
    /// Gets the datacollection sink.
    /// </summary>
    public TestPlatformDataCollectionSink DataCollectionSink { get; private set; }

    /// <summary>
    /// Gets the data collection environment context.
    /// </summary>
    public DataCollectionEnvironmentContext? EnvironmentContext { get; private set; }

    /// <summary>
    /// Gets the data collection logger
    /// </summary>
    public TestPlatformDataCollectionLogger Logger { get; private set; }

    /// <summary>
    /// Gets the data collection logger
    /// </summary>
    private string SettingsXml { get; set; }

    private const string DefaultConfigurationSettings = @"<Configuration />";

    /// <summary>
    /// Gets or sets environment variables supplied by the data collector.
    /// These are available after the collector has been initialized.
    /// </summary>
    public IEnumerable<KeyValuePair<string, string>>? TestExecutionEnvironmentVariables
    {
        get;
        set;
    }

    /// <summary>
    /// Initializes datacollectors.
    /// </summary>
    internal void InitializeDataCollector(ITelemetryReporter telemetryReporter)
    {
        UpdateConfigurationElement();

        DataCollector.Initialize(ConfigurationElement, Events, DataCollectionSink, Logger, EnvironmentContext);

        if (DataCollector is ITelemetryInitializer telemetryInitializer)
        {
            telemetryInitializer.Initialize(telemetryReporter);
        }
    }

    private void UpdateConfigurationElement()
    {
        var frameWork = XmlRunSettingsUtilities.GetRunConfigurationNode(SettingsXml).TargetFramework;

        if (ConfigurationElement == null)
        {
            var doc = new XmlDocument();
            using (
                var xmlReader = XmlReader.Create(
                    new StringReader(DefaultConfigurationSettings),
                    new XmlReaderSettings { CloseInput = true, DtdProcessing = DtdProcessing.Prohibit }))
            {
                doc.Load(xmlReader);
            }

            ConfigurationElement = doc.DocumentElement!;
        }

        // Add Framework config, since it could be required by DataCollector, to determine whether they support this Framework or not
        if (frameWork != null)
        {
            AppendChildNodeOrInnerText(ConfigurationElement.OwnerDocument, ConfigurationElement, "Framework", string.Empty, frameWork.Name);
        }
    }

    private static void AppendChildNodeOrInnerText(XmlDocument doc, XmlElement owner, string elementName, string nameSpaceUri, string innerText)
    {
        var node = owner.SelectSingleNode(elementName) ?? doc.CreateNode("element", elementName, nameSpaceUri);
        node.InnerText = innerText;
        owner.AppendChild(node);
    }

    /// <summary>
    /// Disposes datacollector.
    /// </summary>
    internal void DisposeDataCollector()
    {
        try
        {
            EqtTrace.Verbose("dataCollectorInfo.DisposeDataCollector: calling Dispose() on {0}", DataCollector.GetType());

            DataCollector.Dispose();
        }
        catch (Exception ex)
        {
            EqtTrace.Error("DataCollectorInfo.DisposeDataCollector: exception while calling Dispose() on {0}: " + ex, DataCollector.GetType());
        }
    }

    /// <summary>
    /// The get test execution environment variables sync.
    /// </summary>
    public void SetTestExecutionEnvironmentVariables()
    {
        if (DataCollector is ITestExecutionEnvironmentSpecifier testExecutionEnvironmentSpecifier)
        {
            // Get the environment variables the data collector wants set in the test execution environment
            TestExecutionEnvironmentVariables = testExecutionEnvironmentSpecifier.GetTestExecutionEnvironmentVariables();
        }
    }
}
