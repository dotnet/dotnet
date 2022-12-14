// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace System.Xml.XmlWriterTests
{
    public class XmlWriterTests_Encoding
    {
        [Fact]
        public static void WriteWithEncoding()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.CloseOutput = false;
            settings.Encoding = Encoding.GetEncoding("Windows-1252");
            MemoryStream strm = new MemoryStream();

            using (XmlWriter writer = XmlWriter.Create(strm, settings))
            {
                writer.WriteElementString("orderID", "1-456-ab\u0661");
                writer.WriteElementString("orderID", "2-36-00a\uD800\uDC00\uD801\uDC01");
                writer.Flush();
            }

            strm.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[strm.Length];
            int bytesCount = strm.Read(bytes, 0, (int)strm.Length);
            string s = settings.Encoding.GetString(bytes, 0, bytesCount);

            Assert.Equal("<orderID>1-456-ab&#x661;</orderID><orderID>2-36-00a&#x10000;&#x10401;</orderID>", s);
        }

        [Fact]
        public void WriteWithUtf32EncodingNoBom()
        {
            //Given, encoding set to UTF32 with no BOM
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = false,
                ConformanceLevel = ConformanceLevel.Document,
                Encoding = new UTF32Encoding(false, false, true)
            };

            string resultString;
            using (var result = new MemoryStream())
            {
                // BOM can be written in this call
                var writer = XmlWriter.Create(result, settings);

                // When, do work and get result
                writer.WriteStartDocument();
                writer.WriteStartElement("orders");
                writer.WriteElementString("orderID", "1-456-ab\u0661");
                writer.WriteElementString("orderID", "2-36-00a\uD800\uDC00\uD801\uDC01");
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                result.Position = 0;
                resultString = settings.Encoding.GetString(result.ToArray());
            }

            // Then, last '>' will be cut off in resulting string if BOM is present
            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-32\"?>", string.Concat(resultString.Take(39)));
        }

        [Fact]
        public static async Task AsyncSyncWrite_StreamResult_ShouldMatch()
        {
            using (var syncStream = new MemoryStream())
            using (var asyncStream = new MemoryStream())
            {
                await using (var writer = XmlWriter.Create(asyncStream, new XmlWriterSettings() { Async = true }))
                {
                    await writer.WriteStartDocumentAsync();
                    await writer.WriteStartElementAsync(string.Empty, "root", null);
                    await writer.WriteStartElementAsync(null, "test", null);
                    await writer.WriteAttributeStringAsync(string.Empty, "abc", string.Empty, "1");
                    await writer.WriteStringAsync("value");
                    await writer.WriteEndElementAsync();
                    await writer.WriteEndElementAsync();
                }

                using (var writer = XmlWriter.Create(syncStream, new XmlWriterSettings() { Async = false }))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("root");
                    writer.WriteStartElement("test");
                    writer.WriteAttributeString("abc", "1");
                    writer.WriteString("value");
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }

                Assert.Equal(syncStream.ToArray(), asyncStream.ToArray());
            }
        }
    }
}
