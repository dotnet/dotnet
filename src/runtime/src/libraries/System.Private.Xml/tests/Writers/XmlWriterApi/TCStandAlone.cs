// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using XmlCoreTest.Common;
using Xunit;

namespace System.Xml.XmlWriterApiTests
{
    public class TCStandAlone
    {
        [Theory]
        [XmlWriterInlineData]
        public void standalone_1(XmlWriterUtils utils)
        {
            XmlWriterSettings wSettings = new XmlWriterSettings();
            wSettings.ConformanceLevel = ConformanceLevel.Document;
            wSettings.CloseOutput = false;

            XmlWriter w = utils.CreateWriter(wSettings);
            w.WriteStartDocument(true);
            w.WriteStartElement("Root");
            w.WriteEndElement();
            w.WriteEndDocument();
            w.Dispose();

            bool ret = false;

            if (utils.WriterType == WriterType.UnicodeWriter)
                ret = utils.CompareReader("<?xml version=\"1.0\" encoding=\"unicode\" standalone=\"yes\"?><Root />");
            else
                ret = utils.CompareReader("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?><Root />");

            Assert.True(ret);
        }

        [Theory]
        [XmlWriterInlineData]
        public void standalone_2(XmlWriterUtils utils)
        {
            XmlWriterSettings wSettings = new XmlWriterSettings();
            wSettings.ConformanceLevel = ConformanceLevel.Document;
            wSettings.CloseOutput = false;

            XmlWriter w = utils.CreateWriter(wSettings);
            w.WriteStartDocument(false);
            w.WriteStartElement("Root");
            w.WriteEndElement();
            w.WriteEndDocument();
            w.Dispose();

            bool ret = false;

            if (utils.WriterType == WriterType.UnicodeWriter)
                ret = utils.CompareReader("<?xml version=\"1.0\" encoding=\"unicode\" standalone=\"no\"?><Root />");
            else
                ret = utils.CompareReader("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?><Root />");

            Assert.True(ret);
        }
    }
}
