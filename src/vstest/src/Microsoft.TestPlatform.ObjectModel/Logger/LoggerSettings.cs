// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// The logger settings.
/// </summary>
public class LoggerSettings
{
    /// <summary>
    /// Gets or sets the uri.
    /// </summary>
    public Uri? Uri
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the assembly qualified name.
    /// </summary>
    public string? AssemblyQualifiedName
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets value CodeBase of logger DLL. The syntax is same as Code Base in AssemblyName class.
    /// </summary>
    public string? CodeBase
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the friendly name.
    /// </summary>
    public string? FriendlyName
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether is enabled.
    /// </summary>
    public bool IsEnabled
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the configuration.
    /// </summary>
    public XmlElement? Configuration
    {
        get;
        set;
    }

    /// <summary>
    /// The to xml.
    /// </summary>
    /// <returns>
    /// The <see cref="XmlElement"/>.
    /// </returns>
    public XmlElement ToXml()
    {
        return ToXml(Constants.LoggerSettingName);
    }

    /// <summary>
    /// The to xml.
    /// </summary>
    /// <param name="loggerName">
    /// The logger name.
    /// </param>
    /// <returns>
    /// The <see cref="XmlElement"/>.
    /// </returns>
    public XmlElement ToXml(string loggerName)
    {
        var doc = new XmlDocument();
        var root = doc.CreateElement(loggerName);

        AppendAttribute(doc, root, Constants.LoggerFriendlyName, FriendlyName);
        AppendAttribute(doc, root, Constants.LoggerUriName, Uri?.ToString());
        AppendAttribute(doc, root, Constants.LoggerAssemblyQualifiedName, AssemblyQualifiedName);
        AppendAttribute(doc, root, Constants.LoggerCodeBase, CodeBase);
        AppendAttribute(doc, root, Constants.LoggerEnabledName, IsEnabled.ToString());

        if (Configuration != null)
        {
            root.AppendChild(doc.ImportNode(Configuration, true));
        }

        return root;
    }

    private static void AppendAttribute(XmlDocument doc, XmlElement owner, string attributeName, string? attributeValue)
    {
        if (StringUtils.IsNullOrWhiteSpace(attributeValue))
        {
            return;
        }

        XmlAttribute attribute = doc.CreateAttribute(attributeName);
        attribute.Value = attributeValue;
        owner.Attributes.Append(attribute);
    }

    internal static LoggerSettings FromXml(XmlReader reader)
    {
        var elementName = reader.Name;
        var empty = reader.IsEmptyElement;
        var settings = new LoggerSettings
        {
            IsEnabled = true
        };

        // Read attributes.
        if (reader.HasAttributes)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name.ToLowerInvariant())
                {
                    case Constants.LoggerFriendlyNameLower:
                        settings.FriendlyName = reader.Value;
                        break;

                    case Constants.LoggerUriName:
                        try
                        {
                            settings.Uri = new Uri(reader.Value);
                        }
                        catch (UriFormatException)
                        {
                            throw new SettingsException(
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Resources.Resources.InvalidUriInSettings,
                                    reader.Value,
                                    elementName));
                        }
                        break;

                    case Constants.LoggerAssemblyQualifiedNameLower:
                        settings.AssemblyQualifiedName = reader.Value;
                        break;

                    case Constants.LoggerCodeBaseLower:
                        settings.CodeBase = reader.Value;
                        break;

                    case Constants.LoggerEnabledName:
                        _ = bool.TryParse(reader.Value, out var value);
                        settings.IsEnabled = value;
                        break;

                    default:
                        throw new SettingsException(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Resources.Resources.InvalidSettingsXmlAttribute,
                                elementName,
                                reader.Name));
                }
            }
        }

        // Check for required attributes.
        if (StringUtils.IsNullOrWhiteSpace(settings.FriendlyName) &&
            StringUtils.IsNullOrWhiteSpace(settings.Uri?.ToString()) &&
            StringUtils.IsNullOrWhiteSpace(settings.AssemblyQualifiedName))
        {
            throw new SettingsException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    Resources.Resources.MissingLoggerAttributes,
                    elementName,
                    Constants.LoggerFriendlyName));
        }

        // Move to next node.
        reader.Read();

        // Return empty settings if previous element is empty.
        if (empty)
        {
            return settings;
        }

        // Read inner elements.
        while (reader.NodeType == XmlNodeType.Element)
        {
            switch (reader.Name.ToLowerInvariant())
            {
                case Constants.LoggerConfigurationNameLower:
                    var document = new XmlDocument();
                    var element = document.CreateElement(reader.Name);
                    element.InnerXml = reader.ReadInnerXml();
                    settings.Configuration = element;
                    break;
                default:
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
}
