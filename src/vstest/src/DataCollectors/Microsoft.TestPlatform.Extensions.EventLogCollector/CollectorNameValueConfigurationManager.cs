// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.TestPlatform.Extensions.EventLogCollector;

/// <summary>
/// Utility class that collectors can use to read name/value configuration information from
/// the XML element sent to them
/// </summary>
internal class CollectorNameValueConfigurationManager
{
    // Configuration XML constants
    private const string SettingNameAttributeName = "name";

    private const string SettingValueAttributeName = "value";

    /// <summary>
    /// The name/value pairs loaded from the configuration XML element
    /// </summary>

    /// <summary>
    /// Initializes a new instance of the <see cref="CollectorNameValueConfigurationManager"/> class.
    /// Loads the configuration name/value information from the provided XML element into a dictionary
    /// </summary>
    /// <param name="configurationElement">
    /// XML element containing the configuration
    /// </param>
    public CollectorNameValueConfigurationManager(XmlElement? configurationElement)
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
            string settingName = settingElement.GetAttribute(SettingNameAttributeName);
            if (settingName.IsNullOrWhiteSpace())
            {
                EqtTrace.Warning("Skipping configuration setting due to missing setting name");
                continue;
            }

            // Get the setting value
            string settingValue = settingElement.GetAttribute(SettingValueAttributeName);
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

    internal IDictionary<string, string?> NameValuePairs { get; } = new Dictionary<string, string?>();

    /// <summary>
    /// Gets the value of the setting specified by name, or null if it was not found
    /// </summary>
    /// <param name="name">The setting name</param>
    /// <returns>The setting value, or null if the setting was not found</returns>
    public string? this[string name]
    {
        get
        {
            if (name == null)
            {
                return null;
            }

            NameValuePairs.TryGetValue(name, out var settingValue);
            return settingValue;
        }

        set => NameValuePairs[name] = value;
    }
}
