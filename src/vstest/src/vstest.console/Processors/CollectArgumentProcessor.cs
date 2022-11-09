// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// The argument processor for enabling data collectors.
/// </summary>
internal class CollectArgumentProcessor : IArgumentProcessor
{
    /// <summary>
    /// The name of command for enabling code coverage.
    /// </summary>
    public const string CommandName = "/Collect";

    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() =>
            new CollectArgumentProcessorCapabilities());

    /// <summary>
    /// Gets or sets the executor.
    /// </summary>
    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() =>
            new CollectArgumentExecutor(RunSettingsManager.Instance, new FileHelper()));

        set => _executor = value;
    }
}

internal class CollectArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override string CommandName => CollectArgumentProcessor.CommandName;

    public override bool AllowMultiple => true;

    public override bool IsAction => false;

    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.AutoUpdateRunSettings;

    public override string HelpContentResourceName => CommandLineResources.CollectArgumentHelp;

    public override HelpContentPriority HelpPriority => HelpContentPriority.CollectArgumentProcessorHelpPriority;
}

/// <inheritdoc />
internal class CollectArgumentExecutor : IArgumentExecutor
{
    private readonly IRunSettingsProvider _runSettingsManager;
    private readonly IFileHelper _fileHelper;
    internal static List<string> EnabledDataCollectors = new();
    internal CollectArgumentExecutor(IRunSettingsProvider runSettingsManager, IFileHelper fileHelper)
    {
        _runSettingsManager = runSettingsManager;
        _fileHelper = fileHelper;
    }

    /// <inheritdoc />
    public void Initialize(string? argument)
    {
        // 1. Disable all other data collectors. Enable only those data collectors that are explicitly specified by user.
        // 2. Check if Code Coverage Data Collector is specified in runsettings, if not add it and also set enable to true.

        string exceptionMessage = string.Format(CultureInfo.CurrentCulture, CommandLineResources.DataCollectorFriendlyNameInvalid, argument);

        // if argument is null or doesn't contain any element, don't do anything.
        if (argument.IsNullOrWhiteSpace())
        {
            throw new CommandLineException(exceptionMessage);
        }

        // Get collect argument list.
        var collectArgumentList = ArgumentProcessorUtilities.GetArgumentList(argument, ArgumentProcessorUtilities.SemiColonArgumentSeparator, exceptionMessage);

        // First argument is collector name. Remaining are key value pairs for configurations.
        if (collectArgumentList[0].Contains("="))
        {
            throw new CommandLineException(exceptionMessage);
        }

        if (InferRunSettingsHelper.IsTestSettingsEnabled(_runSettingsManager.ActiveRunSettings?.SettingsXml))
        {
            throw new SettingsException(string.Format(CultureInfo.CurrentCulture, CommandLineResources.CollectWithTestSettingErrorMessage, argument));
        }
        AddDataCollectorToRunSettings(collectArgumentList, _runSettingsManager, _fileHelper, exceptionMessage);
    }

    /// <summary>
    /// Returns coverlet code base searching coverlet.collector.dll assembly inside adaptersPaths
    /// </summary>
    private static string? GetCoverletCodeBasePath(IRunSettingsProvider runSettingProvider, IFileHelper fileHelper)
    {
        foreach (string adapterPath in RunSettingsUtilities.GetTestAdaptersPaths(runSettingProvider.ActiveRunSettings?.SettingsXml))
        {
            string collectorPath = Path.Combine(adapterPath, CoverletConstants.CoverletDataCollectorCodebase);
            if (fileHelper.Exists(collectorPath))
            {
                EqtTrace.Verbose("CoverletDataCollector in-process codeBase path '{0}'", collectorPath);
                return collectorPath;
            }
        }

        return null;
    }

    /// <inheritdoc />
    public ArgumentProcessorResult Execute()
    {
        return ArgumentProcessorResult.Success;
    }

    internal static DataCollectorSettings EnableDataCollectorUsingFriendlyName(string argument, DataCollectionRunSettings dataCollectionRunSettings)
    {

        if (!DoesDataCollectorSettingsExist(argument, dataCollectionRunSettings, out var dataCollectorSettings))
        {
            dataCollectorSettings = new DataCollectorSettings();
            dataCollectorSettings.FriendlyName = argument;
            dataCollectorSettings.IsEnabled = true;
            dataCollectionRunSettings.DataCollectorSettingsList.Add(dataCollectorSettings);
        }
        else
        {
            dataCollectorSettings.IsEnabled = true;
        }

        return dataCollectorSettings;
    }

    private static void AddDataCollectorConfigurations(string[] configurations, DataCollectorSettings dataCollectorSettings, string exceptionMessage)
    {
        if (dataCollectorSettings.Configuration == null)
        {
            XmlDocument doc = new();
            dataCollectorSettings.Configuration = doc.CreateElement("Configuration");
        }

        foreach (var configuration in configurations)
        {
            var keyValuePair = ArgumentProcessorUtilities.GetArgumentList(configuration, ArgumentProcessorUtilities.EqualNameValueSeparator, exceptionMessage);

            if (keyValuePair.Length == 2)
            {
                AddOrUpdateConfiguration(dataCollectorSettings.Configuration, keyValuePair[0], keyValuePair[1]);
            }
            else
            {
                throw new CommandLineException(exceptionMessage);
            }
        }
    }

    private static void AddOrUpdateConfiguration(XmlElement configuration, string configurationName, string configurationValue)
    {
        var existingConfigurations = configuration.GetElementsByTagName(configurationName);

        // Update existing configuration if present.
        if (existingConfigurations.Count == 0)
        {
            XmlElement newConfiguration = configuration.OwnerDocument.CreateElement(configurationName);
            newConfiguration.InnerText = configurationValue;
            configuration.AppendChild(newConfiguration);
            return;
        }

        foreach (XmlNode? existingConfiguration in existingConfigurations)
        {
            TPDebug.Assert(existingConfiguration is not null, "existingConfiguration is null");
            existingConfiguration.InnerText = configurationValue;
        }
    }

    /// <summary>
    /// Enables coverlet in-proc datacollector
    /// </summary>
    internal static void EnableCoverletInProcDataCollector(string argument, DataCollectionRunSettings dataCollectionRunSettings, IRunSettingsProvider runSettingProvider, IFileHelper fileHelper)
    {

        if (!DoesDataCollectorSettingsExist(argument, dataCollectionRunSettings, out DataCollectorSettings? dataCollectorSettings))
        {
            // Create a new setting with default values
            dataCollectorSettings = new DataCollectorSettings();
            dataCollectorSettings.FriendlyName = argument;
            dataCollectorSettings.AssemblyQualifiedName = CoverletConstants.CoverletDataCollectorAssemblyQualifiedName;
            dataCollectorSettings.CodeBase = GetCoverletCodeBasePath(runSettingProvider, fileHelper) ?? CoverletConstants.CoverletDataCollectorCodebase;
            dataCollectorSettings.IsEnabled = true;
            dataCollectionRunSettings.DataCollectorSettingsList.Add(dataCollectorSettings);
        }
        else
        {
            // Set Assembly qualified name and code base if not already set
            dataCollectorSettings.AssemblyQualifiedName ??= CoverletConstants.CoverletDataCollectorAssemblyQualifiedName;
            dataCollectorSettings.CodeBase = (dataCollectorSettings.CodeBase ?? GetCoverletCodeBasePath(runSettingProvider, fileHelper)) ?? CoverletConstants.CoverletDataCollectorCodebase;
            dataCollectorSettings.IsEnabled = true;
        }
    }

    private static bool DoesDataCollectorSettingsExist(string friendlyName,
        DataCollectionRunSettings dataCollectionRunSettings,
        [NotNullWhen(returnValue: true)] out DataCollectorSettings? dataCollectorSettings)
    {
        dataCollectorSettings = null;
        foreach (var dataCollectorSetting in dataCollectionRunSettings.DataCollectorSettingsList)
        {
            if (string.Equals(dataCollectorSetting.FriendlyName, friendlyName, StringComparison.OrdinalIgnoreCase))
            {
                dataCollectorSettings = dataCollectorSetting;
                return true;
            }
        }

        return false;
    }

    internal static void AddDataCollectorToRunSettings(string arguments, IRunSettingsProvider runSettingsManager, IFileHelper fileHelper)
    {
        AddDataCollectorToRunSettings(new string[] { arguments }, runSettingsManager, fileHelper, string.Empty);
    }

    internal static void AddDataCollectorToRunSettings(string[] arguments, IRunSettingsProvider runSettingsManager, IFileHelper fileHelper, string exceptionMessage)
    {
        var collectorName = arguments[0];
        var additionalConfigurations = arguments.Skip(1).ToArray();
        EnabledDataCollectors.Add(collectorName.ToLower(CultureInfo.CurrentCulture));

        var settings = runSettingsManager.ActiveRunSettings?.SettingsXml;
        if (settings == null)
        {
            runSettingsManager.AddDefaultRunSettings();
            settings = runSettingsManager.ActiveRunSettings?.SettingsXml;
        }

        var dataCollectionRunSettings = XmlRunSettingsUtilities.GetDataCollectionRunSettings(settings) ?? new DataCollectionRunSettings();
        var inProcDataCollectionRunSettings = XmlRunSettingsUtilities.GetInProcDataCollectionRunSettings(settings)
                                              ?? new DataCollectionRunSettings(
                                                  Constants.InProcDataCollectionRunSettingsName,
                                                  Constants.InProcDataCollectorsSettingName,
                                                  Constants.InProcDataCollectorSettingName);

        // Add data collectors if not already present, enable if already present.
        var dataCollectorSettings = EnableDataCollectorUsingFriendlyName(collectorName, dataCollectionRunSettings);

        if (additionalConfigurations.Length > 0)
        {
            AddDataCollectorConfigurations(additionalConfigurations, dataCollectorSettings, exceptionMessage);
        }

        runSettingsManager.UpdateRunSettingsNodeInnerXml(Constants.DataCollectionRunSettingsName, dataCollectionRunSettings.ToXml().InnerXml);

        if (string.Equals(collectorName, CoverletConstants.CoverletDataCollectorFriendlyName, StringComparison.OrdinalIgnoreCase))
        {
            // Add in-proc data collector to runsettings if coverlet code coverage is enabled
            EnableCoverletInProcDataCollector(collectorName, inProcDataCollectionRunSettings, runSettingsManager, fileHelper);
            runSettingsManager.UpdateRunSettingsNodeInnerXml(Constants.InProcDataCollectionRunSettingsName, inProcDataCollectionRunSettings.ToXml().InnerXml);
        }
    }

    internal static void AddDataCollectorFriendlyName(string friendlyName)
    {
        EnabledDataCollectors.Add(friendlyName.ToLower(CultureInfo.CurrentCulture));
    }

    internal static class CoverletConstants
    {
        /// <summary>
        /// Coverlet in-proc data collector friendly name
        /// </summary>
        public const string CoverletDataCollectorFriendlyName = "XPlat Code Coverage";

        /// <summary>
        /// Coverlet in-proc data collector assembly qualified name
        /// </summary>
        public const string CoverletDataCollectorAssemblyQualifiedName = "Coverlet.Collector.DataCollection.CoverletInProcDataCollector, coverlet.collector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

        /// <summary>
        /// Coverlet in-proc data collector code base
        /// </summary>
        public const string CoverletDataCollectorCodebase = "coverlet.collector.dll";
    }
}
