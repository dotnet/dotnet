// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using NuGet.Packaging;

namespace NuGet.Protocol
{
    public static class HttpStreamValidation
    {
        public static void ValidateJObject(string uri, Stream stream)
        {
            try
            {
                using (var reader = new StreamReader(
                    stream: stream,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 4096,
                    leaveOpen: true))
                using (var jsonReader = new JsonTextReader(reader) { CloseInput = false })
                {
                    var firstTokenFound = jsonReader.Read();
                    if (!firstTokenFound || jsonReader.TokenType != JsonToken.StartObject)
                    {
                        throw new JsonReaderException("The JSON document is not an object.");
                    }

                    while (jsonReader.Read())
                    {
                    }

                    if (jsonReader.Depth != 0)
                    {
                        throw new JsonReaderException("The JSON document is not complete.");
                    }
                }
            }
            catch (Exception e) when (!(e is InvalidDataException))
            {
                string message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.Protocol_InvalidJsonObject,
                    uri);

                throw new InvalidDataException(message, e);
            }
        }

        public static void ValidateNupkg(string uri, Stream stream)
        {
            try
            {
                using (var reader = new PackageArchiveReader(
                    stream: stream,
                    leaveStreamOpen: true))
                using (var nuspec = reader.GetNuspec()) // This method throws if no .nuspec exists.
                {
                    _ = new NuspecReader(nuspec); // This method throws if reading the .nuspec fails
                }
            }
            catch (Exception e)
            {
                string message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.Log_InvalidNupkgFromUrl,
                    uri);

                throw new InvalidDataException(message, e);
            }
        }

        public static void ValidateXml(string uri, Stream stream)
        {
            try
            {
                using (var xmlReader = XmlReader.Create(
                    input: stream,
                    settings: new XmlReaderSettings { CloseInput = false }))
                {
                    while (xmlReader.Read())
                    {
                    }

                    if (xmlReader.Depth != 0)
                    {
                        throw new JsonReaderException("The XML document is not complete.");
                    }
                }
            }
            catch (Exception e)
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.Protocol_InvalidXml,
                    uri);

                throw new InvalidDataException(message, e);
            }
        }
    }
}
