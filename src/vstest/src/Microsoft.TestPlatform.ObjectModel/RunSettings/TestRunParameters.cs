// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// This class deals with the run settings, Test Run Parameters node and the extracting of Key Value Pair's from the parameters listed.
/// </summary>
internal class TestRunParameters
{
    /// <summary>
    /// Get the test run parameters from the provided reader.
    /// </summary>
    /// <param name="reader"> The reader. </param>
    /// <returns> The dictionary of test run parameters. </returns>
    /// <exception cref="SettingsException"> Throws when format is incorrect. </exception>
    internal static Dictionary<string, object> FromXml(XmlReader reader)
    {
        var testParameters = new Dictionary<string, object>();

        if (reader.IsEmptyElement)
        {
            return testParameters;
        }

        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);
        reader.Read();

        while (reader.NodeType == XmlNodeType.Element)
        {
            string elementName = reader.Name;
            switch (elementName)
            {
                case "Parameter":
                    string? paramName = null;
                    string? paramValue = null;
                    for (int attIndex = 0; attIndex < reader.AttributeCount; attIndex++)
                    {
                        reader.MoveToAttribute(attIndex);
                        if (string.Equals(reader.Name, "Name", StringComparison.OrdinalIgnoreCase))
                        {
                            paramName = reader.Value;
                        }
                        else if (string.Equals(reader.Name, "Value", StringComparison.OrdinalIgnoreCase))
                        {
                            paramValue = reader.Value;
                        }
                    }

                    if (paramName != null && paramValue != null)
                    {
                        testParameters[paramName] = paramValue;
                    }

                    break;
                default:
                    throw new SettingsException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.Resources.InvalidSettingsXmlElement,
                            Constants.TestRunParametersName,
                            reader.Name));
            }

            reader.Read();
        }

        return testParameters;
    }
}
