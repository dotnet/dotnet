// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// The in procedure data collector settings.
/// </summary>
public class DataCollectorSettings
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
    /// Gets or sets value CodeBase of collector DLL. The syntax is same as Code Base in AssemblyName class.
    /// </summary>
    public string? CodeBase
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
        XmlDocument doc = new();
        XmlElement root = doc.CreateElement(Constants.DataCollectorSettingName);
        AppendAttribute(doc, root, "uri", Uri?.ToString());
        AppendAttribute(doc, root, "assemblyQualifiedName", AssemblyQualifiedName);
        AppendAttribute(doc, root, "friendlyName", FriendlyName);

        root.AppendChild(doc.ImportNode(Configuration!, true));

        return root;
    }

    /// <summary>
    /// The to xml.
    /// </summary>
    /// <param name="dataCollectorName">
    /// The data collector name.
    /// </param>
    /// <returns>
    /// The <see cref="XmlElement"/>.
    /// </returns>
    public XmlElement ToXml(string dataCollectorName)
    {
        XmlDocument doc = new();
        XmlElement root = doc.CreateElement(dataCollectorName);
        if (Uri != null)
        {
            AppendAttribute(doc, root, "uri", Uri.ToString());
        }

        if (!StringUtils.IsNullOrWhiteSpace(AssemblyQualifiedName))
        {
            AppendAttribute(doc, root, "assemblyQualifiedName", AssemblyQualifiedName);
        }

        if (!StringUtils.IsNullOrWhiteSpace(FriendlyName))
        {
            AppendAttribute(doc, root, "friendlyName", FriendlyName);
        }

        AppendAttribute(doc, root, "enabled", IsEnabled.ToString());

        if (!StringUtils.IsNullOrWhiteSpace(CodeBase))
        {
            AppendAttribute(doc, root, "codebase", CodeBase);
        }

        if (Configuration != null)
        {
            root.AppendChild(doc.ImportNode(Configuration, true));
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
    /// The <see cref="InProcDataCollectorSettings"/>.
    /// </returns>
    /// <exception cref="SettingsException">
    /// Settings exception
    /// </exception>
    internal static DataCollectorSettings FromXml(XmlReader reader)
    {
        DataCollectorSettings settings = new();
        settings.IsEnabled = true;
        bool empty = reader.IsEmptyElement;
        if (reader.HasAttributes)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "uri":
                        ValidateArg.NotNullOrEmpty(reader.Value, "uri");
                        try
                        {
                            settings.Uri = new Uri(reader.Value);
                        }
                        catch (UriFormatException)
                        {
                            throw new SettingsException(string.Format(CultureInfo.CurrentCulture, Resources.Resources.InvalidDataCollectorUriInSettings, reader.Value));
                        }

                        break;

                    case "assemblyQualifiedName":
                        ValidateArg.NotNullOrEmpty(reader.Value, "assemblyQualifiedName");
                        settings.AssemblyQualifiedName = reader.Value;
                        break;

                    case "friendlyName":
                        ValidateArg.NotNullOrEmpty(reader.Value, "FriendlyName");
                        settings.FriendlyName = reader.Value;
                        break;

                    case "enabled":
                        settings.IsEnabled = bool.Parse(reader.Value);
                        break;

                    case "codebase":
                        settings.CodeBase = reader.Value; // Optional.
                        break;

                    default:
                        throw new SettingsException(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Resources.Resources.InvalidSettingsXmlAttribute,
                                Constants.DataCollectionRunSettingsName,
                                reader.Name));
                }
            }

        }

        reader.Read();
        if (!empty)
        {
            while (reader.NodeType == XmlNodeType.Element)
            {
                switch (reader.Name)
                {
                    case "Configuration":
                        XmlDocument doc = new();
                        XmlElement element = doc.CreateElement("Configuration");
                        element.InnerXml = reader.ReadInnerXml();
                        settings.Configuration = element;
                        break;

                    default:
                        throw new SettingsException(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Resources.Resources.InvalidSettingsXmlElement,
                                Constants.DataCollectionRunSettingsName,
                                reader.Name));
                }
            }
            reader.ReadEndElement();
        }
        return settings;
    }

    private static void AppendAttribute(XmlDocument doc, XmlElement owner, string attributeName, string? attributeValue)
    {
        XmlAttribute attribute = doc.CreateAttribute(attributeName);
        attribute.Value = attributeValue;
        owner.Attributes.Append(attribute);
    }
}
