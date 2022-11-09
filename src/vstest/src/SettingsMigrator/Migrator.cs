// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Xml;

using SettingsMigrator;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.SettingsMigrator.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.SettingsMigrator;

/// <summary>
/// Migrator used to migrate test settings and run settings with embedded testsettings to run settings.
/// </summary>
public class Migrator
{
    private const string TestTimeoutAttributeName = "testTimeout";

    private const string ParallelTestCountAttributeName = "parallelTestCount";

    private const string HostProcessPlatformAttributeName = "hostProcessPlatform";

    private const string RunTimeoutAttributeName = "runTimeout";

    private const string LegacySettingsNodeName = "LegacySettings";

    private const string MSTestNodeName = "MSTest";

    private const string ForcedLegacyModeName = "ForcedLegacyMode";

    private const string ExecutionNodeName = "Execution";

    private const string TimeoutsNodeName = "Timeouts";

    private const string TestTypeSpecificNodeName = "TestTypeSpecific";

    private const string RunConfigurationNodeName = "RunConfiguration";

    private const string TestSessionTimeoutNodeName = "TestSessionTimeout";

    private const string DataCollectionRunSettingsNodeName = "DataCollectionRunSettings";

    private const string DataCollectorsNodeName = "DataCollectors";

    private const string SampleRunSettingsContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                    "<RunSettings></RunSettings>";

    private const string AgentNotRespondingTimeoutAttribute = "agentNotRespondingTimeout";

    private const string DeploymentTimeoutAttribute = "deploymentTimeout";

    private const string ScriptTimeoutAttribute = "scriptTimeout";

    private const string TestSettingsExtension = ".testsettings";

    private const string RunSettingsExtension = ".runsettings";

    /// <summary>
    /// Migrates the nodes from given settings to run settings format.
    /// </summary>
    /// <param name="oldFilePath">Path to old file</param>
    /// <param name="newFilePath">Path to new file</param>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Part of the public API")]
    public void Migrate(string oldFilePath, string newFilePath)
    {
        if (!Path.IsPathRooted(oldFilePath))
        {
            Console.WriteLine(CommandLineResources.ValidUsage);
        }

        if (string.Equals(Path.GetExtension(oldFilePath), TestSettingsExtension, StringComparison.OrdinalIgnoreCase))
        {
            MigrateTestSettings(oldFilePath, newFilePath);
        }
        else if (string.Equals(Path.GetExtension(oldFilePath), RunSettingsExtension, StringComparison.OrdinalIgnoreCase))
        {
            MigrateRunSettings(oldFilePath, newFilePath);
        }
        else
        {
            Console.WriteLine(CommandLineResources.ValidUsage);
        }
    }

    /// <summary>
    /// Given a runSettings with an embedded testSettings, converts it to runSettings.
    /// </summary>
    /// <param name="oldRunSettingsPath"> Path to old runsettings.</param>
    /// <param name="newRunSettingsPath">Path to new runsettings.</param>
    private static void MigrateRunSettings(string oldRunSettingsPath, string newRunSettingsPath)
    {
        string? testSettingsPath = null;
        using XmlTextReader reader = new(oldRunSettingsPath);
        reader.Namespaces = false;

        var runSettingsXmlDoc = new XmlDocument();
        runSettingsXmlDoc.Load(reader);
        var root = runSettingsXmlDoc.DocumentElement;

        var testSettingsNode = root?.SelectSingleNode(@"/RunSettings/MSTest/SettingsFile");
        if (testSettingsNode != null)
        {
            testSettingsPath = testSettingsNode.InnerText;
        }

        if (!testSettingsPath.IsNullOrWhiteSpace())
        {
            // Expand path relative to runSettings location.
            if (!Path.IsPathRooted(testSettingsPath))
            {
                testSettingsPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(oldRunSettingsPath)!, testSettingsPath));
            }

            // Remove the embedded testSettings node if it exists.
            RemoveEmbeddedTestSettings(runSettingsXmlDoc);

            MigrateTestSettingsNodesToRunSettings(testSettingsPath, runSettingsXmlDoc);

            runSettingsXmlDoc.Save(newRunSettingsPath);
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.RunSettingsCreated, newRunSettingsPath));
        }
        else
        {
            Console.WriteLine(CommandLineResources.NoEmbeddedSettings);
        }
    }

    /// <summary>
    /// Given a testSettings, converts it to runSettings.
    /// </summary>
    /// <param name="oldTestSettingsPath">Path to old testsettings.</param>
    /// <param name="newRunSettingsPath">Path to new runsettings.</param>
    private static void MigrateTestSettings(string oldTestSettingsPath, string newRunSettingsPath)
    {
        var runSettingsXmlDoc = new XmlDocument();
        runSettingsXmlDoc.LoadXml(SampleRunSettingsContent);

        MigrateTestSettingsNodesToRunSettings(oldTestSettingsPath, runSettingsXmlDoc);

        runSettingsXmlDoc.Save(newRunSettingsPath);
        Console.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.RunSettingsCreated, newRunSettingsPath));
    }

    /// <summary>
    /// Given a testSettings, converts it to runSettings
    /// </summary>
    /// <param name="testSettingsPath">Path to test settings</param>
    /// <param name="runSettingsXmlDoc">Runsettings Xml</param>
    private static void MigrateTestSettingsNodesToRunSettings(string testSettingsPath, XmlDocument runSettingsXmlDoc)
    {
        TestSettingsNodes testSettingsNodes = ReadTestSettingsNodes(testSettingsPath);

        string? testTimeout = null;
        string? runTimeout = null;
        if (testSettingsNodes.Timeout is not null)
        {
            if (testSettingsNodes.Timeout.Attributes?[TestTimeoutAttributeName] is XmlAttribute testTimeoutAttribute)
            {
                testTimeout = testTimeoutAttribute.Value;
            }

            if (testSettingsNodes.Timeout.Attributes?[RunTimeoutAttributeName] is XmlAttribute runTimeoutAttribute)
            {
                runTimeout = runTimeoutAttribute.Value;
            }
        }

        string? parallelTestCount = null;
        string? hostProcessPlatform = null;
        if (testSettingsNodes.Execution is not null)
        {
            if (testSettingsNodes.Execution.Attributes?[ParallelTestCountAttributeName] is XmlAttribute parallelTestCountAttribute)
            {
                parallelTestCount = parallelTestCountAttribute.Value;
            }

            if (testSettingsNodes.Execution.Attributes?[HostProcessPlatformAttributeName] is XmlAttribute hostProcessPlatformAttribute)
            {
                hostProcessPlatform = hostProcessPlatformAttribute.Value;
            }
        }

        // WebTestRunConfiguration node.
        if (testSettingsNodes.WebSettings != null)
        {
            runSettingsXmlDoc.DocumentElement?.AppendChild(runSettingsXmlDoc.ImportNode(testSettingsNodes.WebSettings, deep: true));
        }

        // LegacySettings node.
        AddLegacyNodes(testSettingsNodes, testTimeout, parallelTestCount, hostProcessPlatform, runSettingsXmlDoc);

        // TestSessionTimeout node.
        if (!runTimeout.IsNullOrEmpty())
        {
            AddRunTimeoutNode(runTimeout, runSettingsXmlDoc!);
        }

        // DataCollectors node.
        if (testSettingsNodes?.Datacollectors != null && testSettingsNodes.Datacollectors.Count > 0)
        {
            AddDataCollectorNodes(testSettingsNodes.Datacollectors, runSettingsXmlDoc);
        }
    }

    private static TestSettingsNodes ReadTestSettingsNodes(string testSettingsPath)
    {
        TestSettingsNodes testSettingsNodes = new();

        using (XmlTextReader reader = new(testSettingsPath))
        {
            reader.Namespaces = false;

            var testSettingsXmlDoc = new XmlDocument();
            testSettingsXmlDoc.Load(reader);
            var testSettingsRoot = testSettingsXmlDoc.DocumentElement!;

            // Select the interesting nodes from the xml.
            testSettingsNodes.Deployment = testSettingsRoot.SelectSingleNode(@"/TestSettings/Deployment");
            testSettingsNodes.Script = testSettingsRoot.SelectSingleNode(@"/TestSettings/Scripts");
            testSettingsNodes.WebSettings = testSettingsRoot.SelectSingleNode(@"/TestSettings/Execution/TestTypeSpecific/WebTestRunConfiguration");
            testSettingsNodes.Datacollectors = testSettingsRoot.SelectNodes(@"/TestSettings/Execution/AgentRule/DataCollectors/DataCollector");
            testSettingsNodes.Timeout = testSettingsRoot.SelectSingleNode(@"/TestSettings/Execution/Timeouts");
            testSettingsNodes.UnitTestConfig = testSettingsRoot.SelectSingleNode(@"/TestSettings/Execution/TestTypeSpecific/UnitTestRunConfig");
            testSettingsNodes.Hosts = testSettingsRoot.SelectSingleNode(@"/TestSettings/Execution/Hosts");
            testSettingsNodes.Execution = testSettingsRoot.SelectSingleNode(@"/TestSettings/Execution");

            if (testSettingsNodes.Timeout != null
                && (testSettingsNodes.Timeout.Attributes?[AgentNotRespondingTimeoutAttribute] != null
                    || testSettingsNodes.Timeout.Attributes?[DeploymentTimeoutAttribute] != null
                    || testSettingsNodes.Timeout.Attributes?[ScriptTimeoutAttribute] != null))
            {
                Console.WriteLine(CommandLineResources.UnsupportedAttributes);
            }
        }

        return testSettingsNodes!;
    }

    /// <summary>
    /// Removes the embedded testSettings node if present.
    /// </summary>
    /// <param name="newXmlDoc">Xml doc to process</param>
    private static void RemoveEmbeddedTestSettings(XmlDocument newXmlDoc)
    {
        var testSettingsNode = newXmlDoc.DocumentElement?.SelectSingleNode(@"/RunSettings/MSTest/SettingsFile");
        if (testSettingsNode != null)
        {
            testSettingsNode.ParentNode?.RemoveChild(testSettingsNode);
        }
    }

    /// <summary>
    /// Adds the legacy nodes to runSettings xml.
    /// </summary>
    /// <param name="testSettingsNodes">testSettingsNodes</param>
    /// <param name="testTimeout">testTimeout</param>
    /// <param name="parallelTestCount">parallelTestCount</param>
    /// <param name="hostProcessPlatform">hostProcessPlatform</param>
    /// <param name="newXmlDoc">newXmlDoc</param>
    private static void AddLegacyNodes(TestSettingsNodes testSettingsNodes, string? testTimeout, string? parallelTestCount, string? hostProcessPlatform, XmlDocument newXmlDoc)
    {
        if (testSettingsNodes.Deployment == null
            && testSettingsNodes.Script == null
            && testSettingsNodes.UnitTestConfig == null
            && parallelTestCount.IsNullOrEmpty()
            && testTimeout.IsNullOrEmpty()
            && hostProcessPlatform.IsNullOrEmpty()
            && testSettingsNodes.Hosts == null)
        {
            return;
        }

        if (newXmlDoc.DocumentElement == null)
        {
            return;
        }

        // Add ForcedLegacy node.
        var mstestNode = newXmlDoc.DocumentElement.SelectSingleNode(@"/RunSettings/MSTest");
        if (mstestNode == null)
        {
            mstestNode = newXmlDoc.CreateNode(XmlNodeType.Element, MSTestNodeName, null);
            newXmlDoc.DocumentElement.AppendChild(mstestNode);
            mstestNode = newXmlDoc.DocumentElement.SelectSingleNode(@"/RunSettings/MSTest");
        }

        if (mstestNode == null)
        {
            return;
        }

        var forcedLegacyNode = newXmlDoc.DocumentElement.SelectSingleNode(@"/RunSettings/MSTest/ForcedLegacyMode");
        if (forcedLegacyNode == null)
        {
            forcedLegacyNode = newXmlDoc.CreateNode(XmlNodeType.Element, ForcedLegacyModeName, null);
            mstestNode.AppendChild(newXmlDoc.ImportNode(forcedLegacyNode, deep: true));
            forcedLegacyNode = newXmlDoc.DocumentElement.SelectSingleNode(@"/RunSettings/MSTest/ForcedLegacyMode")!;
        }

        forcedLegacyNode.InnerText = "true";

        // Remove if the legacy node already exists.
        var legacyNode = newXmlDoc.DocumentElement.SelectSingleNode(@"/RunSettings/LegacySettings");
        if (legacyNode?.ParentNode != null)
        {
            Console.WriteLine(CommandLineResources.IgnoringLegacySettings);
            legacyNode.ParentNode.RemoveChild(legacyNode);
        }

        legacyNode = newXmlDoc.CreateNode(XmlNodeType.Element, LegacySettingsNodeName, null);

        if (testSettingsNodes.Deployment != null)
        {
            legacyNode.AppendChild(newXmlDoc.ImportNode(testSettingsNodes.Deployment, deep: true));
        }

        if (testSettingsNodes.Script != null)
        {
            legacyNode.AppendChild(newXmlDoc.ImportNode(testSettingsNodes.Script, deep: true));
        }

        // Execution node.
        if (testSettingsNodes.UnitTestConfig != null || !parallelTestCount.IsNullOrEmpty() || !testTimeout.IsNullOrEmpty() || testSettingsNodes.Hosts != null)
        {
            XmlNode newExecutionNode = newXmlDoc.CreateNode(XmlNodeType.Element, ExecutionNodeName, null);

            if (!parallelTestCount.IsNullOrEmpty())
            {
                var parallelAttribute = newXmlDoc.CreateAttribute(ParallelTestCountAttributeName);
                parallelAttribute.Value = parallelTestCount;
                newExecutionNode.Attributes?.Append(parallelAttribute);
            }

            if (!hostProcessPlatform.IsNullOrEmpty())
            {
                var hostProcessPlatformAttribute = newXmlDoc.CreateAttribute(HostProcessPlatformAttributeName);
                hostProcessPlatformAttribute.Value = hostProcessPlatform;
                newExecutionNode.Attributes?.Append(hostProcessPlatformAttribute);
            }

            if (!testTimeout.IsNullOrEmpty())
            {
                var newTimeoutsNode = newXmlDoc.CreateNode(XmlNodeType.Element, TimeoutsNodeName, null);
                var testTimeoutAttribute = newXmlDoc.CreateAttribute(TestTimeoutAttributeName);
                testTimeoutAttribute.Value = testTimeout;
                newTimeoutsNode.Attributes?.Append(testTimeoutAttribute);
                newExecutionNode.AppendChild(newXmlDoc.ImportNode(newTimeoutsNode, deep: true));
            }

            if (testSettingsNodes.Hosts != null)
            {
                newExecutionNode.AppendChild(newXmlDoc.ImportNode(testSettingsNodes.Hosts, deep: true));
            }

            if (testSettingsNodes.UnitTestConfig != null)
            {
                var testTypeSpecificNode = newXmlDoc.CreateNode(XmlNodeType.Element, TestTypeSpecificNodeName, null);
                testTypeSpecificNode.AppendChild(newXmlDoc.ImportNode(testSettingsNodes.UnitTestConfig, deep: true));
                newExecutionNode.AppendChild(newXmlDoc.ImportNode(testTypeSpecificNode, deep: true));
            }

            legacyNode.AppendChild(newXmlDoc.ImportNode(newExecutionNode, deep: true));
        }

        newXmlDoc.DocumentElement.AppendChild(legacyNode);
    }

    /// <summary>
    /// Adds the dataCollector nodes to the runSettings xml.
    /// </summary>
    /// <param name="oldDataCollectorNodes"> Datacollector Nodes</param>
    /// <param name="newXmlDoc">Xml doc to process</param>
    private static void AddDataCollectorNodes(XmlNodeList oldDataCollectorNodes, XmlDocument? newXmlDoc)
    {
        if (newXmlDoc?.DocumentElement == null)
        {
            return;
        }

        var dataCollectionRunSettingsNode = newXmlDoc.DocumentElement.SelectSingleNode(@"/RunSettings/DataCollectionRunSettings");
        dataCollectionRunSettingsNode ??= newXmlDoc.CreateNode(XmlNodeType.Element, DataCollectionRunSettingsNodeName, null);

        var dataCollectorsNode = newXmlDoc.DocumentElement.SelectSingleNode(@"/RunSettings/DataCollectionRunSettings/DataCollectors");
        if (dataCollectorsNode == null)
        {
            dataCollectorsNode = newXmlDoc.CreateNode(XmlNodeType.Element, DataCollectorsNodeName, null);
            dataCollectionRunSettingsNode.AppendChild(newXmlDoc.ImportNode(dataCollectorsNode, deep: true));
            newXmlDoc.DocumentElement.AppendChild(dataCollectionRunSettingsNode);
            dataCollectorsNode = newXmlDoc.DocumentElement.SelectSingleNode(@"/RunSettings/DataCollectionRunSettings/DataCollectors")!;
        }

        foreach (XmlNode dataCollector in oldDataCollectorNodes)
        {
            dataCollectorsNode.AppendChild(newXmlDoc.ImportNode(dataCollector, deep: true));
        }
    }

    /// <summary>
    /// Adds run session timeout node.
    /// </summary>
    /// <param name="runTimeout">Run Timeout</param>
    /// <param name="newXmlDoc">Xml doc to process</param>
    private static void AddRunTimeoutNode(string runTimeout, XmlDocument newXmlDoc)
    {
        if (newXmlDoc.DocumentElement == null)
        {
            return;
        }

        var runConfigurationNode = newXmlDoc.DocumentElement.SelectSingleNode(@"/RunSettings/RunConfiguration");
        runConfigurationNode ??= newXmlDoc.CreateNode(XmlNodeType.Element, RunConfigurationNodeName, null);

        var testSessionTimeoutNode = newXmlDoc.CreateNode(XmlNodeType.Element, TestSessionTimeoutNodeName, null);
        testSessionTimeoutNode.InnerText = runTimeout;
        runConfigurationNode.AppendChild(newXmlDoc.ImportNode(testSessionTimeoutNode, deep: true));

        newXmlDoc.DocumentElement.AppendChild(runConfigurationNode);
    }
}
