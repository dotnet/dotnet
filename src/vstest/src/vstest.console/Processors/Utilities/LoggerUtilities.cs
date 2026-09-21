// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors.Utilities;

internal class LoggerUtilities
{
    /// <summary>
    /// Add logger to run settings.
    /// </summary>
    /// <param name="loggerIdentifier">Logger Identifier.</param>
    /// <param name="loggerParameters">Logger parameters.</param>
    /// <param name="runSettingsManager">Run settings manager.</param>
    public static void AddLoggerToRunSettings(string loggerIdentifier, Dictionary<string, string>? loggerParameters, IRunSettingsProvider runSettingsManager)
    {
        // Creating default run settings if required.
        var settings = runSettingsManager.ActiveRunSettings?.SettingsXml;
        if (settings == null)
        {
            runSettingsManager.AddDefaultRunSettings();
            settings = runSettingsManager.ActiveRunSettings?.SettingsXml;
        }

        var loggerRunSettings = XmlRunSettingsUtilities.GetLoggerRunSettings(settings) ?? new LoggerRunSettings();

        LoggerSettings logger;
        try
        {
            // Logger as uri in command line.
            var loggerUri = new Uri(loggerIdentifier);
            logger = new LoggerSettings
            {
                Uri = loggerUri,
                IsEnabled = true
            };
        }
        catch (UriFormatException)
        {
            // Logger as friendlyName in command line.
            logger = new LoggerSettings
            {
                FriendlyName = loggerIdentifier,
                IsEnabled = true
            };
        }

        // Converting logger console params to Configuration element
        if (loggerParameters != null && loggerParameters.Count > 0)
        {
            var xmlDocument = new XmlDocument();
            var outerNode = xmlDocument.CreateElement("Configuration");
            foreach (KeyValuePair<string, string> entry in loggerParameters)
            {
                var node = xmlDocument.CreateElement(entry.Key);
                node.InnerText = entry.Value;
                outerNode.AppendChild(node);
            }

            logger.Configuration = outerNode;
        }

        // Reuse the entry that is already in LoggerRunSettings when it describes the same logger,
        // so that everything the command line does not mention (Configuration, CodeBase,
        // AssemblyQualifiedName, and the friendlyName/uri pairing) survives. Rebuilding the entry
        // from the command line alone silently discards those.
        var existingLoggerIndex = loggerRunSettings.GetExistingLoggerIndex(logger);
        if (existingLoggerIndex >= 0)
        {
            var existingLogger = loggerRunSettings.LoggerSettingsList[existingLoggerIndex];

            // Whatever the command line did spell out wins over the settings file.
            if (logger.FriendlyName is not null)
            {
                existingLogger.FriendlyName = logger.FriendlyName;
            }

            if (logger.Uri is not null)
            {
                existingLogger.Uri = logger.Uri;
            }

            if (logger.Configuration is not null)
            {
                existingLogger.Configuration = logger.Configuration;
            }

            // Naming the logger on the command line enables it, even if the settings disabled it.
            existingLogger.IsEnabled = true;
            logger = existingLogger;

            loggerRunSettings.LoggerSettingsList.RemoveAt(existingLoggerIndex);
        }

        loggerRunSettings.LoggerSettingsList.Add(logger);

        runSettingsManager.UpdateRunSettingsNodeInnerXml(Constants.LoggerRunSettingsName, loggerRunSettings.ToXml().InnerXml);
    }
}
