// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

using OMResources = Microsoft.VisualStudio.TestPlatform.ObjectModel.Resources.CommonResources;
using UtilitiesResources = Microsoft.VisualStudio.TestPlatform.Utilities.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.Utilities;

/// <summary>
/// Utility class for Inferring the runsettings from the current environment and the user specified command line switches.
/// </summary>
public class InferRunSettingsHelper
{
    private const string DesignModeNodeName = "DesignMode";
    private const string BatchSizeNodeName = "BatchSize";
    private const string CollectSourceInformationNodeName = "CollectSourceInformation";
    private const string RunSettingsNodeName = "RunSettings";
    private const string RunConfigurationNodeName = "RunConfiguration";
    private const string ResultsDirectoryNodeName = "ResultsDirectory";
    private const string TargetPlatformNodeName = "TargetPlatform";
    private const string TargetFrameworkNodeName = "TargetFrameworkVersion";
    private const string TargetDevice = "TargetDevice";

    private const string DesignModeNodePath = @"/RunSettings/RunConfiguration/DesignMode";
    private const string BatchSizeNodePath = @"/RunSettings/RunConfiguration/BatchSize";
    private const string CollectSourceInformationNodePath = @"/RunSettings/RunConfiguration/CollectSourceInformation";
    private const string RunConfigurationNodePath = @"/RunSettings/RunConfiguration";
    private const string TargetPlatformNodePath = @"/RunSettings/RunConfiguration/TargetPlatform";
    private const string TargetFrameworkNodePath = @"/RunSettings/RunConfiguration/TargetFrameworkVersion";
    private const string ResultsDirectoryNodePath = @"/RunSettings/RunConfiguration/ResultsDirectory";
    private const string TargetDeviceNodePath = @"/RunSettings/RunConfiguration/TargetDevice";
    private const string EnvironmentVariablesNodePath = @"/RunSettings/RunConfiguration/EnvironmentVariables";
    private const string MultiTargetingForwardLink = @"https://aka.ms/tp/vstest/multitargetingdoc?view=vs-2019";

    // To make things compatible for older runsettings
    private const string MsTestTargetDeviceNodePath = @"/RunSettings/MSPhoneTest/TargetDevice";

    private const string CodeCoverageCollectorUri = @"datacollector://microsoft/CodeCoverage/2.0";
    private const string FakesCollectorUri = @"datacollector://microsoft/unittestisolation/1.0";
    private const string CodeCoverageFriendlyName = "Code Coverage";
    private const string FakesFriendlyName = "UnitTestIsolation";

    private const string LegacyElementsString = "Elements";
    private const string DeploymentAttributesString = "DeploymentAttributes";
    private const string ExecutionAttributesString = "ExecutionAttributes";
    private static readonly List<string> ExecutionNodesPaths = new() { @"/RunSettings/LegacySettings/Execution/TestTypeSpecific/UnitTestRunConfig/AssemblyResolution", @"/RunSettings/LegacySettings/Execution/Timeouts", @"/RunSettings/LegacySettings/Execution/Hosts" };

    /// <summary>
    /// Make runsettings compatible with testhost of version 15.0.0-preview
    /// Due to bug https://github.com/Microsoft/vstest/issues/970 we need this function
    /// </summary>
    /// <param name="runsettingsXml">string content of runsettings </param>
    /// <returns>compatible runsettings</returns>
    public static string? MakeRunsettingsCompatible(string? runsettingsXml)
    {
        // These are the list of valid RunConfiguration setting name which old testhost understand.
        var listOfValidRunConfigurationSettings = new HashSet<string>
        {
            "TargetPlatform",
            "TargetFrameworkVersion",
            "TestAdaptersPaths",
            "ResultsDirectory",
            "SolutionDirectory",
            "MaxCpuCount",
            "DisableParallelization",
            "DisableAppDomain"
        };

        return MakeRunsettingsCompatible(runsettingsXml, listOfValidRunConfigurationSettings, null);
    }

    private static string? MakeRunsettingsCompatible(string? runsettingsXml, HashSet<string>? listOfValidRunConfigurationSettings, HashSet<string>? listOfInValidRunConfigurationSettings)
    {
        var updatedRunSettingsXml = runsettingsXml;

        if (runsettingsXml.IsNullOrWhiteSpace())
        {
            return updatedRunSettingsXml;
        }

        using var stream = new StringReader(runsettingsXml);
        using var reader = XmlReader.Create(stream, XmlRunSettingsUtilities.ReaderSettings);
        var document = new XmlDocument();
        document.Load(reader);

        var runSettingsNavigator = document.CreateNavigator()!;

        // Move navigator to RunConfiguration node
        if (!runSettingsNavigator.MoveToChild(RunSettingsNodeName, string.Empty) ||
            !runSettingsNavigator.MoveToChild(RunConfigurationNodeName, string.Empty))
        {
            EqtTrace.Error("InferRunSettingsHelper.MakeRunsettingsCompatible: Unable to navigate to RunConfiguration. Current node: " + runSettingsNavigator.LocalName);
        }
        else if (runSettingsNavigator.HasChildren)
        {
            listOfInValidRunConfigurationSettings ??= new HashSet<string>();

            // Find all invalid RunConfiguration Settings
            runSettingsNavigator.MoveToFirstChild();
            if (listOfValidRunConfigurationSettings != null)
            {
                do
                {
                    if (!listOfValidRunConfigurationSettings.Contains(runSettingsNavigator.LocalName))
                    {
                        listOfInValidRunConfigurationSettings.Add(runSettingsNavigator.LocalName);
                    }
                } while (runSettingsNavigator.MoveToNext());
            }

            // Delete all invalid RunConfiguration Settings
            if (listOfInValidRunConfigurationSettings.Count > 0)
            {
                string settingsName = string.Join(", ", listOfInValidRunConfigurationSettings);
                EqtTrace.Warning("InferRunSettingsHelper.MakeRunsettingsCompatible: Removing the following settings: {0} from RunSettings file. To use those settings please move to latest version of Microsoft.NET.Test.Sdk", settingsName);

                // move navigator to RunConfiguration node
                runSettingsNavigator.MoveToParent();

                foreach (var s in listOfInValidRunConfigurationSettings)
                {
                    var nodePath = RunConfigurationNodePath + "/" + s;
                    XmlUtilities.RemoveChildNode(runSettingsNavigator, nodePath, s);
                }

                runSettingsNavigator.MoveToRoot();
                updatedRunSettingsXml = runSettingsNavigator.OuterXml;
            }
        }

        return updatedRunSettingsXml;
    }

    public static string? RemoveTargetPlatformElement(string? runsettingsXml)
        => MakeRunsettingsCompatible(runsettingsXml, null, new HashSet<string> { "TargetPlatform" });

    /// <summary>
    /// Updates the run settings XML with the specified values.
    /// </summary>
    /// <param name="runSettingsDocument"> The XmlDocument of the XML. </param>
    /// <param name="architecture"> The architecture. </param>
    /// <param name="framework"> The framework. </param>
    /// <param name="resultsDirectory"> The results directory. </param>
    public static void UpdateRunSettingsWithUserProvidedSwitches(XmlDocument runSettingsDocument, Architecture architecture, Framework framework, string? resultsDirectory)
    {
        var runSettingsNavigator = runSettingsDocument.CreateNavigator()!;

        ValidateRunConfiguration(runSettingsNavigator);

        // when runsettings specifies platform, that takes precedence over the user specified platform via command line arguments.
        var shouldUpdatePlatform = true;

        TryGetPlatformXml(runSettingsNavigator, out var nodeXml);
        if (!nodeXml.IsNullOrEmpty())
        {
            architecture = (Architecture)Enum.Parse(typeof(Architecture), nodeXml, true);
            shouldUpdatePlatform = false;
        }

        // when runsettings specifies framework, that takes precedence over the user specified input framework via the command line arguments.
        var shouldUpdateFramework = true;
        TryGetFrameworkXml(runSettingsNavigator, out nodeXml);

        if (!nodeXml.IsNullOrEmpty())
        {
            var fwkFromString = Framework.FromString(nodeXml);
            TPDebug.Assert(fwkFromString is not null, "fwkFromString is null");
            framework = fwkFromString;
            shouldUpdateFramework = false;
        }

        EqtTrace.Verbose("Using effective platform:{0} effective framework:{1}", architecture, framework);

        // Check if inputRunSettings has results directory configured.
        var hasResultsDirectory = runSettingsDocument.SelectSingleNode(ResultsDirectoryNodePath) != null;

        // Regenerate the effective settings.
        if (shouldUpdatePlatform || shouldUpdateFramework || !hasResultsDirectory)
        {
            UpdateRunConfiguration(runSettingsDocument, architecture, framework, resultsDirectory);
        }
    }

    /// <summary>
    /// Updates the <c>RunConfiguration.DesignMode</c> value for a run settings. Doesn't do anything if the value is already set.
    /// </summary>
    /// <param name="runSettingsDocument">Document for runsettings xml</param>
    /// <param name="designModeValue">Value to set</param>
    public static void UpdateDesignMode(XmlDocument runSettingsDocument, bool designModeValue)
    {
        AddNodeIfNotPresent(runSettingsDocument, DesignModeNodePath, DesignModeNodeName, designModeValue);
    }

    /// <summary>
    /// Updates the <c>RunConfiguration.BatchSize</c> value for a run settings. Doesn't do anything if the value is already set.
    /// </summary>
    /// <param name="runSettingsDocument">Document for runsettings xml</param>
    /// <param name="batchSizeValue">Value to set</param>
    public static void UpdateBatchSize(XmlDocument runSettingsDocument, long batchSizeValue)
    {
        AddNodeIfNotPresent(runSettingsDocument, BatchSizeNodePath, BatchSizeNodeName, batchSizeValue);
    }

    /// <summary>
    /// Updates the <c>RunConfiguration.CollectSourceInformation</c> value for a run settings. Doesn't do anything if the value is already set.
    /// </summary>
    /// <param name="runSettingsDocument">Navigator for runsettings xml</param>
    /// <param name="collectSourceInformationValue">Value to set</param>
    public static void UpdateCollectSourceInformation(XmlDocument runSettingsDocument, bool collectSourceInformationValue)
    {
        AddNodeIfNotPresent(runSettingsDocument, CollectSourceInformationNodePath, CollectSourceInformationNodeName, collectSourceInformationValue);
    }

    /// <summary>
    /// Updates the <c>RunConfiguration.TargetDevice</c> value for a run settings. Doesn't do anything if the value is already set.
    /// </summary>
    /// <param name="runSettingsDocument">XmlDocument for runsettings xml</param>
    /// <param name="targetDevice">Value to set</param>
    public static void UpdateTargetDevice(XmlDocument runSettingsDocument, string targetDevice)
    {
        AddNodeIfNotPresent(runSettingsDocument, TargetDeviceNodePath, TargetDevice, targetDevice);
    }

    /// <summary>
    /// Updates the <c>RunConfiguration.TargetFrameworkVersion</c> value for a run settings. if the value is already set, behavior depends on overwrite.
    /// </summary>
    /// <param name="runSettingsDocument">XmlDocument for runsettings xml</param>
    /// <param name="framework">Value to set</param>
    /// <param name="overwrite">Overwrite option.</param>
    public static void UpdateTargetFramework(XmlDocument runSettingsDocument, string framework, bool overwrite = false)
    {
        AddNodeIfNotPresent(runSettingsDocument, TargetFrameworkNodePath, TargetFrameworkNodeName, framework, overwrite);
    }

    /// <summary>
    /// Validates the collectors in runsettings when an in-lined testsettings is specified
    /// </summary>
    /// <param name="runsettings">RunSettings used for the run</param>
    /// <returns>True if an incompatible collector is found</returns>
    public static bool AreRunSettingsCollectorsIncompatibleWithTestSettings(string? runsettings)
    {
        // If there's no embedded testsettings.. bail out
        if (!IsTestSettingsEnabled(runsettings))
        {
            return false;
        }

        // Explicitly blocking usage of data collectors through modes runsettings and testsettings except
        // for couple of scenarios where the IDE generates the collector settings in the runsettings file even when
        // it has an embedded testsettings file. Longterm runsettings will be the single run configuration source
        // In-proc collectors are incompatible with testsettings
        var inprocDataCollectionSettings = XmlRunSettingsUtilities.GetInProcDataCollectionRunSettings(runsettings);
        if (inprocDataCollectionSettings != null && inprocDataCollectionSettings.IsCollectionEnabled && inprocDataCollectionSettings.DataCollectorSettingsList != null)
        {
            foreach (var collectorSettings in inprocDataCollectionSettings.DataCollectorSettingsList)
            {
                if (collectorSettings.IsEnabled)
                {
                    EqtTrace.Warning($"Incompatible collector found. {collectorSettings.FriendlyName} : {collectorSettings.Uri}");
                    return true;
                }
            }
        }

        // TestSettings and collection is enabled in runsetttings.. the only allowed collectors are code coverage and fakes
        var datacollectionSettings = XmlRunSettingsUtilities.GetDataCollectionRunSettings(runsettings);
        if (datacollectionSettings != null && datacollectionSettings.IsCollectionEnabled && datacollectionSettings.DataCollectorSettingsList != null)
        {
            foreach (var collectorRef in datacollectionSettings.DataCollectorSettingsList)
            {
                // Ignore disabled collector
                if (!collectorRef.IsEnabled)
                {
                    continue;
                }

                // If the configured collector is code coverage or fakes.. ignore
                if (!string.IsNullOrWhiteSpace(collectorRef.FriendlyName) &&
                    (FakesFriendlyName.Equals(collectorRef.FriendlyName, StringComparison.OrdinalIgnoreCase) ||
                     CodeCoverageFriendlyName.Equals(collectorRef.FriendlyName, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                // If the configured collector is code coverage or fakes.. ignore
                if (collectorRef.Uri != null &&
                    (CodeCoverageCollectorUri.Equals(collectorRef.Uri.ToString(), StringComparison.OrdinalIgnoreCase) ||
                     FakesCollectorUri.Equals(collectorRef.Uri.ToString(), StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                EqtTrace.Warning($"Incompatible collector found. {collectorRef.FriendlyName} : {collectorRef.Uri}");
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns true if legacy settings node is present in runsettings
    /// </summary>
    /// <param name="runsettingsXml">The run settings xml string</param>
    /// <param name="legacySettingsTelemetry">The telemetry data that needs to be captured</param>
    /// <returns></returns>
    public static bool TryGetLegacySettingElements(string runsettingsXml, out Dictionary<string, string> legacySettingsTelemetry)
    {
        legacySettingsTelemetry = new Dictionary<string, string>();
        try
        {
            using var stream = new StringReader(runsettingsXml);
            using var reader = XmlReader.Create(stream, XmlRunSettingsUtilities.ReaderSettings);
            var document = new XmlDocument();
            document.Load(reader);
            var runSettingsNavigator = document.CreateNavigator()!;

            var node = runSettingsNavigator.SelectSingleNode(@"/RunSettings/LegacySettings");
            if (node == null)
            {
                return false;
            }

            var childNodes = node.SelectChildren(XPathNodeType.Element);

            var legacySettingElements = new List<string>();
            while (childNodes.MoveNext())
            {
                legacySettingElements.Add(childNodes.Current!.Name);
            }

            foreach (var executionNodePath in ExecutionNodesPaths)
            {
                var executionNode = runSettingsNavigator.SelectSingleNode(executionNodePath);
                if (executionNode != null)
                {
                    legacySettingElements.Add(executionNode.Name);
                }
            }

            if (legacySettingElements.Count > 0)
            {
                legacySettingsTelemetry.Add(LegacyElementsString, string.Join(", ", legacySettingElements));
            }

            var deploymentNode = runSettingsNavigator.SelectSingleNode(@"/RunSettings/LegacySettings/Deployment");
            var deploymentAttributes = GetNodeAttributes(deploymentNode);
            if (deploymentAttributes != null)
            {
                legacySettingsTelemetry.Add(DeploymentAttributesString, string.Join(", ", deploymentAttributes));
            }

            var executiontNode = runSettingsNavigator.SelectSingleNode(@"/RunSettings/LegacySettings/Execution");
            var executiontAttributes = GetNodeAttributes(executiontNode);
            if (executiontAttributes != null)
            {
                legacySettingsTelemetry.Add(ExecutionAttributesString, string.Join(", ", executiontAttributes));
            }
        }
        catch (Exception ex)
        {
            EqtTrace.Error("Error while trying to read legacy settings. Message: {0}", ex.ToString());
            return false;
        }

        return true;
    }

    private static List<string>? GetNodeAttributes(XPathNavigator? node)
    {
        if (node == null || !node.HasAttributes)
        {
            return null;
        }

        var attributes = new List<string>();
        node.MoveToFirstAttribute();
        attributes.Add(node.Name);
        while (node.MoveToNextAttribute())
        {
            attributes.Add(node.Name);
        }
        return attributes;
    }

    /// <summary>
    /// Returns a dictionary of environment variables given in run settings
    /// </summary>
    /// <param name="runsettingsXml">The run settings xml string</param>
    /// <returns>Environment Variables Dictionary</returns>
    public static Dictionary<string, string?>? GetEnvironmentVariables(string? runSettings)
    {
        Dictionary<string, string?>? environmentVariables = null;
        if (runSettings is null)
        {
            return environmentVariables;
        }

        try
        {
            using var stream = new StringReader(runSettings);
            using var reader = XmlReader.Create(stream, XmlRunSettingsUtilities.ReaderSettings);
            var document = new XmlDocument();
            document.Load(reader);
            var runSettingsNavigator = document.CreateNavigator()!;

            var node = runSettingsNavigator.SelectSingleNode(EnvironmentVariablesNodePath);
            if (node == null)
            {
                return null;
            }

            environmentVariables = new Dictionary<string, string?>();
            var childNodes = node.SelectChildren(XPathNodeType.Element);

            while (childNodes.MoveNext())
            {
                if (!environmentVariables.ContainsKey(childNodes.Current!.Name))
                {
                    environmentVariables.Add(childNodes.Current.Name, childNodes.Current?.Value);
                }
            }
        }
        catch (Exception ex)
        {
            EqtTrace.Error("Error while trying to read environment variables settings. Message: {0}", ex.ToString());
            return null;
        }

        return environmentVariables;
    }

    /// <summary>
    /// Updates the <c>RunConfiguration.TargetPlatform</c> value for a run settings. if the value is already set, behavior depends on overwrite.
    /// </summary>
    /// <param name="runSettingsDocument">Navigator for runsettings xml</param>
    /// <param name="platform">Value to set</param>
    /// <param name="overwrite">Overwrite option.</param>
    public static void UpdateTargetPlatform(XmlDocument runSettingsDocument, string platform, bool overwrite = false)
        => AddNodeIfNotPresent(runSettingsDocument, TargetPlatformNodePath, TargetPlatformNodeName, platform, overwrite);

    public static bool TryGetDeviceXml(XPathNavigator runSettingsNavigator, [NotNullWhen(true)] out string? deviceXml)
    {
        ValidateArg.NotNull(runSettingsNavigator, nameof(runSettingsNavigator));

        deviceXml = null;
        var targetDeviceNode = runSettingsNavigator.SelectSingleNode(MsTestTargetDeviceNodePath);
        if (targetDeviceNode != null)
        {
            deviceXml = targetDeviceNode.InnerXml;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Check if testsettings in configured using runsettings.
    /// </summary>
    /// <param name="runsettingsXml">xml string of runsetting</param>
    /// <returns></returns>
    public static bool IsTestSettingsEnabled(string? runsettingsXml)
    {
        if (runsettingsXml.IsNullOrWhiteSpace())
        {
            return false;
        }

        using (var stream = new StringReader(runsettingsXml))
        using (var reader = XmlReader.Create(stream, XmlRunSettingsUtilities.ReaderSettings))
        {
            var document = new XmlDocument();
            document.Load(reader);

            var runSettingsNavigator = document.CreateNavigator()!;

            // Move navigator to MSTest node
            if (!runSettingsNavigator.MoveToChild(RunSettingsNodeName, string.Empty) ||
                !runSettingsNavigator.MoveToChild("MSTest", string.Empty))
            {
                EqtTrace.Info("InferRunSettingsHelper.IsTestSettingsEnabled: Unable to navigate to RunSettings/MSTest. Current node: " + runSettingsNavigator.LocalName);
                return false;
            }

            var node = runSettingsNavigator.SelectSingleNode(@"/RunSettings/MSTest/SettingsFile");
            if (node != null && !node.InnerXml.IsNullOrEmpty())
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Adds node under RunConfiguration setting. No op if node is already present.
    /// </summary>
    private static void AddNodeIfNotPresent<T>(XmlDocument xmlDocument, string nodePath, string nodeName, T nodeValue, bool overwrite = false)
        where T : notnull
    {
        // Navigator should be at Root of runsettings xml, attempt to move to /RunSettings/RunConfiguration
        var root = xmlDocument.DocumentElement;

        if (root?.SelectSingleNode(RunConfigurationNodePath) == null)
        {
            if (root?.Name == RunSettingsNodeName)
            {
                // When just <RunSettings></RunSettings> is provided in the runsettings string, then this will add the common RunConfiguration inner node.
                XmlUtilities.AppendOrModifyChild(xmlDocument, RunConfigurationNodePath, RunConfigurationNodeName, innerXml: null);
            }
            else
            {
                EqtTrace.Error("InferRunSettingsHelper.UpdateNodeIfNotPresent: Unable to navigate to RunConfiguration. Current node: " + xmlDocument.LocalName);
                return;
            }
        }

        var node = xmlDocument.SelectSingleNode(nodePath);
        if (node == null || overwrite)
        {
            XmlUtilities.AppendOrModifyChild(xmlDocument, nodePath, nodeName, nodeValue.ToString());
        }
    }

    /// <summary>
    /// Validates the RunConfiguration setting in run settings.
    /// </summary>
    private static void ValidateRunConfiguration(XPathNavigator runSettingsNavigator)
    {
        if (!runSettingsNavigator.MoveToChild(RunSettingsNodeName, string.Empty))
        {
            throw new XmlException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    UtilitiesResources.RunSettingsParseError,
                    UtilitiesResources.MissingRunSettingsNode));
        }

        if (runSettingsNavigator.MoveToChild(RunConfigurationNodeName, string.Empty))
        {
            if (!TryGetPlatformXml(runSettingsNavigator, out var nodeXml))
            {
                throw new XmlException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        UtilitiesResources.RunSettingsParseError,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            UtilitiesResources.InvalidSettingsIncorrectValue,
                            Constants.RunConfigurationSettingsName,
                            nodeXml,
                            TargetPlatformNodeName)));
            }

            if (!TryGetFrameworkXml(runSettingsNavigator, out nodeXml))
            {
                throw new XmlException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        UtilitiesResources.RunSettingsParseError,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            UtilitiesResources.InvalidSettingsIncorrectValue,
                            Constants.RunConfigurationSettingsName,
                            nodeXml,
                            TargetFrameworkNodeName)));
            }
        }
    }

    /// <summary>
    /// Regenerates the RunConfiguration node with new values under runsettings.
    /// </summary>
    private static void UpdateRunConfiguration(
        XmlDocument xmlDocument,
        Architecture effectivePlatform,
        Framework effectiveFramework,
        string? resultsDirectory)
    {
        var childNode = xmlDocument.SelectSingleNode(ResultsDirectoryNodePath);
        if (childNode != null)
        {
            resultsDirectory = childNode.InnerXml;
        }

        XmlUtilities.AppendOrModifyChild(xmlDocument, RunConfigurationNodePath, RunConfigurationNodeName, null);
        XmlUtilities.AppendOrModifyChild(xmlDocument, ResultsDirectoryNodePath, ResultsDirectoryNodeName, resultsDirectory);

        XmlUtilities.AppendOrModifyChild(xmlDocument, TargetPlatformNodePath, TargetPlatformNodeName, effectivePlatform.ToString());
        XmlUtilities.AppendOrModifyChild(xmlDocument, TargetFrameworkNodePath, TargetFrameworkNodeName, effectiveFramework.ToString());
    }

    public static bool TryGetPlatformXml(XPathNavigator runSettingsNavigator, out string? platformXml)
    {
        platformXml = XmlUtilities.GetNodeXml(runSettingsNavigator, TargetPlatformNodePath);

        if (platformXml == null)
        {
            return true;
        }

        Func<string, bool> validator = (string xml) =>
        {
            var value = (Architecture)Enum.Parse(typeof(Architecture), xml, true);

            return Enum.IsDefined(typeof(Architecture), value) && value != Architecture.Default && value != Architecture.AnyCPU;
        };

        return XmlUtilities.IsValidNodeXmlValue(platformXml, validator);
    }

    /// <summary>
    /// Validate if TargetFrameworkVersion in run settings has valid value.
    /// </summary>
    public static bool TryGetFrameworkXml(XPathNavigator runSettingsNavigator, out string? frameworkXml)
    {
        frameworkXml = XmlUtilities.GetNodeXml(runSettingsNavigator, TargetFrameworkNodePath);

        if (frameworkXml == null)
        {
            return true;
        }

        Func<string, bool> validator = (string xml) =>
        {
            if (Framework.FromString(xml) != null)
            {
                // Allow TargetFrameworkMoniker values like .NETFramework,Version=v4.5, ".NETCoreApp,Version=v1.0
                return true;
            }

            var value = (FrameworkVersion)Enum.Parse(typeof(FrameworkVersion), xml, true);

            return Enum.IsDefined(typeof(FrameworkVersion), value) && value != FrameworkVersion.None;
        };

        return XmlUtilities.IsValidNodeXmlValue(frameworkXml, validator);
    }

    /// <summary>
    /// Returns the sources matching the specified platform and framework settings.
    /// For incompatible sources, warning is added to incompatibleSettingWarning.
    /// </summary>
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Part of the public API")]
    public static IEnumerable<string> FilterCompatibleSources(Architecture chosenPlatform, Architecture defaultArchitecture, Framework chosenFramework, IDictionary<string, Architecture> sourcePlatforms, IDictionary<string, Framework> sourceFrameworks, out string incompatibleSettingWarning)
    {
        incompatibleSettingWarning = string.Empty;
        List<string> compatibleSources = new();
        StringBuilder warnings = new();
        warnings.AppendLine();
        bool incompatiblityFound = false;
        foreach (var source in sourcePlatforms.Keys)
        {
            Architecture actualPlatform = sourcePlatforms[source];
            Framework actualFramework = sourceFrameworks[source];
            bool isSettingIncompatible = IsSettingIncompatible(actualPlatform, chosenPlatform, actualFramework, chosenFramework);
            if (isSettingIncompatible)
            {
                string incompatiblityMessage;
                var onlyFileName = Path.GetFileName(source);
                // Add message for incompatible sources.
                incompatiblityMessage = string.Format(CultureInfo.CurrentCulture, OMResources.SourceIncompatible, onlyFileName, actualFramework.Name, actualPlatform);

                warnings.AppendLine(incompatiblityMessage);
                incompatiblityFound = true;
            }
            else
            {
                compatibleSources.Add(source);
            }
        }

        if (incompatiblityFound)
        {
            incompatibleSettingWarning = string.Format(CultureInfo.CurrentCulture, OMResources.DisplayChosenSettings, chosenFramework, chosenPlatform, warnings.ToString(), MultiTargetingForwardLink);
        }

        return compatibleSources;
    }

    /// <summary>
    /// Returns true if source settings are incompatible with target settings.
    /// </summary>
    private static bool IsSettingIncompatible(Architecture sourcePlatform,
        Architecture targetPlatform,
        Framework sourceFramework,
        Framework targetFramework)
    {
        return IsPlatformIncompatible(sourcePlatform, targetPlatform) || IsFrameworkIncompatible(sourceFramework, targetFramework);
    }

    /// <summary>
    /// Returns true if source Platform is incompatible with target platform.
    /// </summary>
    private static bool IsPlatformIncompatible(Architecture sourcePlatform, Architecture targetPlatform)
    {
        return sourcePlatform is not Architecture.Default and not Architecture.AnyCPU
            && (targetPlatform == Architecture.X64 && !Is64BitOperatingSystem() || sourcePlatform != targetPlatform);

        static bool Is64BitOperatingSystem()
        {
            return Environment.Is64BitOperatingSystem;
        }
    }

    /// <summary>
    /// Returns true if source FrameworkVersion is incompatible with target FrameworkVersion.
    /// </summary>
    private static bool IsFrameworkIncompatible(Framework sourceFramework, Framework targetFramework)
    {
        return !sourceFramework.Name.Equals(Framework.DefaultFramework.Name, StringComparison.OrdinalIgnoreCase)
               && !sourceFramework.Name.Equals(targetFramework.Name, StringComparison.OrdinalIgnoreCase);
    }
}
