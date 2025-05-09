// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;
using Microsoft.VisualStudio.SolutionPersistence.Model;
using Microsoft.VisualStudio.SolutionPersistence.Serializer.SlnV12;
using Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml;

internal partial class SlnXmlSerializer
{
    private static class Writer
    {
        internal static async Task SaveAsync(
            string? fullPath,
            SolutionModel model,
            Stream streamWriter)
        {
            model.ThrowIfProjectValidationSuspended();

            SlnXmlModelExtension? modelExtension = model.SerializerExtension as SlnXmlModelExtension;

            // If converting from Sln always remove legacy values.
            bool convertingFromSln = model.SerializerExtension is SlnV12ModelExtension;

            SlnxSerializerSettings xmlSerializerSettings = modelExtension?.Settings ??
                new SlnxSerializerSettings()
                {
                    // For new documents want to do standard indentation.
                    PreserveWhitespace = false,
                    IndentChars = "  ",
                    NewLine = Environment.NewLine,
                    TrimVisualStudioProperties = convertingFromSln,
                };

            if (xmlSerializerSettings.TrimVisualStudioProperties == true)
            {
                model.TrimVisualStudioProperties();
            }
            else
            {
                model.RemoveObsoleteProperties();
            }

            model.DistillProjectConfigurations();

            // If this started as an XML document, merge the changes back into the original document.
            SlnxFile root = modelExtension?.Root ?? CreateNewSlnFile(fullPath, xmlSerializerSettings, model.StringTable);

            // Update the XML to reflect the model.
            _ = root.ApplyModel(model);

            // Always use UTF-8 without BOM
            UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

            using (MemoryStream memoryStream = new MemoryStream(10 * 1024))
            using (TextWriter textWriter = new StreamWriter(memoryStream, encoding))
            using (XmlWriter xmlWriter = CreateXmlWriter(xmlSerializerSettings, textWriter))
            {
                // First copy the model to memory, if any exceptions occur this
                // won't corrupt the original file.
                root.Document.Save(xmlWriter);

                // If the XML is newly formatted, make sure we have a newline at the end of the file.
                if (!root.Document.PreserveWhitespace)
                {
                    await textWriter.WriteLineAsync();
                    await textWriter.FlushAsync();
                }

                memoryStream.Position = 0;

                await memoryStream.CopyToAsync(streamWriter);
                streamWriter.SetLength(streamWriter.Position);
            }

            static XmlWriter CreateXmlWriter(SlnxSerializerSettings settings, TextWriter writer)
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
                {
                    Async = true,
                    OmitXmlDeclaration = true,
                    CloseOutput = false,
                    Encoding = writer.Encoding,
                };

                if (settings.PreserveWhitespace == true)
                {
                    xmlWriterSettings.Indent = false;
                    return XmlWriter.Create(writer, xmlWriterSettings);
                }
                else
                {
                    xmlWriterSettings.Indent = true;

                    if (settings.IndentChars is not null)
                    {
                        xmlWriterSettings.IndentChars = settings.IndentChars;
                    }

                    if (settings.NewLine is not null)
                    {
                        xmlWriterSettings.NewLineChars = settings.NewLine;
                        xmlWriterSettings.NewLineHandling = NewLineHandling.Replace;
                    }

                    return XmlWriter.Create(writer, xmlWriterSettings);
                }
            }

            static SlnxFile CreateNewSlnFile(string? fullPath, SlnxSerializerSettings xmlSerializerSettings, StringTable stringTable)
            {
                XmlDocument xmlDocument = new LineInfoXmlDocument() { PreserveWhitespace = xmlSerializerSettings.PreserveWhitespace ?? false, };

                XmlElement slnElement = xmlDocument.CreateElement(Keyword.Solution.ToXmlString());
                _ = xmlDocument.AppendChild(slnElement);

                return new SlnxFile(xmlDocument, xmlSerializerSettings, stringTable, fullPath);
            }
        }
    }
}
