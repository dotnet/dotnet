// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;

/// <summary>
/// Utility class that loggers can use to read name/value configuration information from the XML element sent to them
/// </summary>
internal class LoggerNameValueConfigurationManager
{

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggerNameValueConfigurationManager"/> class.
    /// Loads the configuration name/value information from the provided XML element into a dictionary
    /// </summary>
    /// <param name="configurationElement">
    /// XML element containing the configuration
    /// </param>
    public LoggerNameValueConfigurationManager(XmlElement? configurationElement)
    {
        Initialize(configurationElement);
    }

    public Dictionary<string, string?> NameValuePairs { get; } = new();

    private void Initialize(XmlElement? configurationElement)
    {
        if (configurationElement == null)
        {
            // There is no configuration
            return;
        }

        // Iterate through top-level XML elements within the configuration element and store
        // name/value information for elements that have name/value attributes.
        foreach (XmlNode settingNode in configurationElement.ChildNodes)
        {
            // Skip all non-elements
            if (settingNode is not XmlElement settingElement)
            {
                continue;
            }

            // Get the setting name
            string settingName = settingElement.Name;

            // Get the setting value
            string settingValue = settingElement.InnerText;

            if (settingValue.IsNullOrWhiteSpace())
            {
                EqtTrace.Warning("Skipping configuration setting '{0}' due to missing value", settingName);
                continue;
            }

            // Save the name/value pair in the dictionary. Note that duplicate settings are
            // overwritten with the last occurrence's value.
            if (NameValuePairs.ContainsKey(settingName))
            {
                EqtTrace.Verbose(
                    "Duplicate configuration setting found for '{0}'. Using the last setting.",
                    settingName);
            }

            NameValuePairs[settingName] = settingValue;
        }
    }
}
