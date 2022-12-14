// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using Xunit;

namespace System.Xml.XmlDocumentTests
{
    public class LoadTests
    {
        // Issue reported on https://github.com/dotnet/runtime/issues/14654
        [Fact]
        public void LoadDocumentFromFile()
        {
            TextReader textReader = File.OpenText(Path.Combine("XmlDocument", "example.xml"));
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            settings.DtdProcessing = DtdProcessing.Ignore;
            XmlDocument doc = new XmlDocument();

            using (StringReader sr = new StringReader(textReader.ReadToEnd()))
            using (XmlReader reader = XmlReader.Create(sr, settings))
            {
                doc.Load(reader);
            }
        }
    }
}
