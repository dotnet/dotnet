// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using NuGet.Common;

namespace NuGet.Packaging
{
    public static class NupkgMetadataFileFormat
    {
        public static readonly int Version = 2;

        private const string VersionProperty = "version";
        private const string HashProperty = "contentHash";
        private const string SourceProperty = "source";

        private static readonly Newtonsoft.Json.JsonSerializer JsonSerializer = Newtonsoft.Json.JsonSerializer.Create(GetSerializerSettings());
        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            // This file is not intended to be parsed in javascript, and the contentHash property contains a Base64 encoded string that
            // can contain characters such as + that has not been encoded in the past.
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        public static NupkgMetadataFile Read(string filePath)
        {
            return Read(filePath, NullLogger.Instance);
        }

        public static NupkgMetadataFile Read(string filePath, ILogger log)
        {
            return FileUtility.SafeRead(filePath, (stream, nupkgMetadataFilePath) => Read(stream, log, nupkgMetadataFilePath));
        }

        public static NupkgMetadataFile Read(Stream stream, ILogger log, string path)
        {
            if (stream is null) { throw new ArgumentNullException(nameof(stream)); }

            try
            {
                NupkgMetadataFile nupkgMetadata = System.Text.Json.JsonSerializer.Deserialize<NupkgMetadataFile>(stream, JsonSerializerOptions);
                if (nupkgMetadata == null)
                {
                    throw new InvalidDataException();
                }
                return nupkgMetadata;
            }
            catch (System.Text.Json.JsonException ex)
            {
                LogMessage(log, path, ex);
                throw new InvalidDataException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                LogMessage(log, path, ex);
                throw;
            }

            static void LogMessage(ILogger log, string path, Exception ex)
            {
                log.LogWarning(string.Format(CultureInfo.CurrentCulture,
                    Strings.Error_LoadingHashFile,
                    path, ex.Message));
            }
        }

        [Obsolete("Use a different overload for better performance. This overload will get deleted in the future.")]
        public static NupkgMetadataFile Read(TextReader reader, ILogger log, string path)
        {
            try
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var nupkgMetadata = JsonSerializer.Deserialize<NupkgMetadataFile>(jsonReader);
                    if (nupkgMetadata == null)
                    {
                        throw new InvalidDataException();
                    }
                    return nupkgMetadata;
                }
            }
            catch (Exception ex)
            {
                log.LogWarning(string.Format(CultureInfo.CurrentCulture,
                    Strings.Error_LoadingHashFile,
                    path, ex.Message));

                throw;
            }
        }

        public static void Write(string filePath, NupkgMetadataFile hashFile)
        {
            // Create the directory if it does not exist
            var fileInfo = new FileInfo(filePath);
            fileInfo.Directory.Create();

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Write(stream, hashFile);
            }
        }

        public static void Write(Stream stream, NupkgMetadataFile hashFile)
        {
            if (stream is null) { throw new ArgumentNullException(nameof(stream)); }

            System.Text.Json.JsonSerializer.Serialize(stream, hashFile, JsonSerializerOptions);
        }

        [Obsolete("Use a different overload for better performance. This overload will get deleted in the future.")]
        public static void Write(TextWriter textWriter, NupkgMetadataFile hashFile)
        {
            using (var jsonWriter = new JsonTextWriter(textWriter))
            {
                jsonWriter.Formatting = Formatting.Indented;

                JsonSerializer.Serialize(jsonWriter, hashFile);
            }
        }

        private static JsonSerializerSettings GetSerializerSettings()
        {
            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
            settings.Converters.Add(NupkgMetadataConverter.Default);
            return settings;
        }

        private class NupkgMetadataConverter : Newtonsoft.Json.JsonConverter
        {
            internal static NupkgMetadataConverter Default { get; } = new NupkgMetadataConverter();

            private static readonly Type TargetType = typeof(NupkgMetadataFile);
            public override bool CanConvert(Type objectType)
            {
                return objectType == TargetType;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
            {
                NupkgMetadataFile nupkgMetadataFile = existingValue as NupkgMetadataFile;
                if (nupkgMetadataFile == null)
                {
                    nupkgMetadataFile = new NupkgMetadataFile();
                }

                if (reader.TokenType != JsonToken.StartObject)
                {
                    throw new InvalidDataException();
                }

                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonToken.EndObject:
                            return nupkgMetadataFile;

                        case JsonToken.PropertyName:
                            switch ((string)reader.Value)
                            {
                                case VersionProperty:
                                    var intValue = reader.ReadAsInt32();
                                    if (intValue.HasValue)
                                    {
                                        nupkgMetadataFile.Version = intValue.Value;
                                    }
                                    break;

                                case HashProperty:
                                    nupkgMetadataFile.ContentHash = reader.ReadAsString();
                                    break;

                                case SourceProperty:
                                    nupkgMetadataFile.Source = reader.ReadAsString();
                                    break;
                            }
                            break;

                        default:
                            reader.Skip();
                            break;
                    }
                }

                throw new JsonReaderException();
            }

            public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
            {
                if (!(value is NupkgMetadataFile nupkgMetadataFile))
                {
                    throw new ArgumentException(message: "value is not of type NupkgMetadataFile", paramName: nameof(value));
                }

                writer.WriteStartObject();

                writer.WritePropertyName(VersionProperty);
                writer.WriteValue(nupkgMetadataFile.Version);

                writer.WritePropertyName(HashProperty);
                writer.WriteValue(nupkgMetadataFile.ContentHash);

                writer.WritePropertyName(SourceProperty);
                writer.WriteValue(nupkgMetadataFile.Source);

                writer.WriteEndObject();
            }
        }
    }
}
