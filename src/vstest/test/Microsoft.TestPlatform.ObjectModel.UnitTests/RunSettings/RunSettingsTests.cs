// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;
using System.Xml.Serialization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

[TestClass]
public class RunSettingsTests
{
    [TestMethod]
    public void RunSettingsNameSerialization()
    {
        var chilRunSettings = new ChildRunSettings();
        var xml = chilRunSettings.ToXml();
        Assert.IsNotNull(xml);
    }

    public class ChildRunSettings : TestRunSettings
    {
        public ChildRunSettings() : base("SomeName")
        {
        }

        public override XmlElement ToXml()
        {
            var document = new XmlDocument();
            using (XmlWriter writer = document.CreateNavigator()!.AppendChild())
            {
                new XmlSerializer(typeof(ChildRunSettings)).Serialize(writer, this);
            }
            return document.DocumentElement!;
        }
    }
}
