// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// The logger run settings.
/// </summary>
public class LoggerRunSettings : TestRunSettings
{
    private readonly string _loggerRunSettingsName = string.Empty;
    private readonly string _loggersSettingName = string.Empty;
    private readonly string _loggerSettingName = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggerRunSettings"/> class.
    /// </summary>
    public LoggerRunSettings() : base(Constants.LoggerRunSettingsName)
    {
        LoggerSettingsList = new Collection<LoggerSettings>();
        _loggerRunSettingsName = Constants.LoggerRunSettingsName;
        _loggersSettingName = Constants.LoggersSettingName;
        _loggerSettingName = Constants.LoggerSettingName;
    }

    /// <summary>
    /// Gets the logger settings list.
    /// </summary>
    public Collection<LoggerSettings> LoggerSettingsList
    {
        get;
        private set;
    }

    public override XmlElement ToXml()
    {
        var doc = new XmlDocument();
        var root = doc.CreateElement(_loggerRunSettingsName);
        var subRoot = doc.CreateElement(_loggersSettingName);
        root.AppendChild(subRoot);

        foreach (var loggerSettings in LoggerSettingsList)
        {
            XmlNode child = doc.ImportNode(loggerSettings.ToXml(_loggerSettingName), true);
            subRoot.AppendChild(child);
        }

        return root;
    }

    /// <summary>
    /// The from xml.
    /// </summary>
    /// <param name="reader">
    /// The reader.
    /// </param>
    /// <returns>
    /// The <see cref="LoggerRunSettings"/>.
    /// </returns>
    /// <exception cref="SettingsException">
    /// Settings exception
    /// </exception>
    internal static LoggerRunSettings FromXml(XmlReader reader)
    {
        ValidateArg.NotNull(reader, nameof(reader));
        return FromXml(reader,
            Constants.LoggersSettingName,
            Constants.LoggerSettingName);
    }

    /// <summary>
    /// The from xml.
    /// </summary>
    /// <param name="reader">
    /// The reader.
    /// </param>
    /// <param name="loggersSettingName">
    /// Loggers setting name.
    /// </param>
    /// <param name="loggerSettingName">
    /// Logger setting name.
    /// </param>
    /// <returns>
    /// The <see cref="LoggerRunSettings"/>
    /// </returns>
    private static LoggerRunSettings FromXml(XmlReader reader, string loggersSettingName, string loggerSettingName)
    {
        // Validation.
        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);

        var elementName = reader.Name;
        var empty = reader.IsEmptyElement;
        var settings = new LoggerRunSettings();

        // Move to next node.
        reader.Read();

        // Return empty settings if previous element empty.
        if (empty)
        {
            return settings;
        }

        // Read inner nodes.
        while (reader.NodeType == XmlNodeType.Element)
        {
            if (reader.Name.Equals(loggersSettingName, StringComparison.OrdinalIgnoreCase))
            {
                var items = ReadListElementFromXml(reader, loggerSettingName);
                foreach (var item in items)
                {
                    settings.LoggerSettingsList.Add(item);
                }
            }
            else
            {
                throw new SettingsException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.Resources.InvalidSettingsXmlElement,
                        elementName,
                        reader.Name));
            }
        }
        reader.ReadEndElement();

        return settings;
    }

    /// <summary>
    /// Reads logger settings list from runSettings
    /// </summary>
    /// <param name="reader">
    /// The reader.
    /// </param>
    /// <param name="loggerSettingName">
    /// Logger setting name.
    /// </param>
    /// <returns>
    /// LoggerSettings List
    /// </returns>
    private static List<LoggerSettings> ReadListElementFromXml(XmlReader reader, string loggerSettingName)
    {
        // Validation.
        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);

        var elementName = reader.Name;
        var empty = reader.IsEmptyElement;
        var settings = new List<LoggerSettings>();

        // Move to next node
        reader.Read();

        // Return empty settings if previous element is empty.
        if (empty)
        {
            return settings;
        }

        // Read inner nodes.
        while (reader.NodeType == XmlNodeType.Element)
        {
            if (reader.Name.Equals(loggerSettingName, StringComparison.OrdinalIgnoreCase))
            {
                settings.Add(LoggerSettings.FromXml(reader));
            }
            else
            {
                throw new SettingsException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.Resources.InvalidSettingsXmlElement,
                        elementName,
                        reader.Name));
            }
        }

        return settings;
    }

    /// <summary>
    /// Gets existing logger index.
    /// </summary>
    /// <param name="loggerSettings">Logger settings.</param>
    /// <returns>Index of given logger settings.</returns>
    public int GetExistingLoggerIndex(LoggerSettings loggerSettings)
    {
        var existingLoggerIndex = -1;

        for (int i = 0; i < LoggerSettingsList.Count; i++)
        {
            var logger = LoggerSettingsList[i];

            if (logger.FriendlyName != null &&
                loggerSettings.FriendlyName != null &&
                logger.FriendlyName.Equals(loggerSettings.FriendlyName, StringComparison.OrdinalIgnoreCase))
            {
                existingLoggerIndex = i;
                break;
            }

            if (logger.Uri?.ToString() != null &&
                loggerSettings.Uri?.ToString() != null &&
                logger.Uri.ToString().Equals(loggerSettings.Uri.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                existingLoggerIndex = i;
                break;
            }
        }

        return existingLoggerIndex;
    }
}
