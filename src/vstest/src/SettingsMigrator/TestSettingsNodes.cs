// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

namespace Microsoft.VisualStudio.TestPlatform.SettingsMigrator;

/// <summary>
/// Contains the test settings nodes that need to be converted.
/// </summary>
public class TestSettingsNodes
{
    public XmlNode? Deployment { get; set; }

    public XmlNode? Script { get; set; }

    public XmlNode? WebSettings { get; set; }

    public XmlNodeList? Datacollectors { get; set; }

    public XmlNode? Timeout { get; set; }

    public XmlNode? UnitTestConfig { get; set; }

    public XmlNode? Hosts { get; set; }

    public XmlNode? Execution { get; set; }
}
