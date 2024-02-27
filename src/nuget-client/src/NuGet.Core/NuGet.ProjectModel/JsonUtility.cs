// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    internal static class JsonUtility
    {
        internal static string NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING = nameof(NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING);
        internal static bool? UseNewtonsoftJson = null;
        internal static readonly char[] PathSplitChars = new[] { LockFile.DirectorySeparatorChar };

        /// <summary>
        /// JsonLoadSettings with line info and comments ignored.
        /// </summary>
        internal static readonly JsonLoadSettings DefaultLoadSettings = new JsonLoadSettings()
        {
            LineInfoHandling = LineInfoHandling.Ignore,
            CommentHandling = CommentHandling.Ignore
        };

        /// <summary>
        /// Load json from a file to a JObject using the default load settings.
        /// </summary>
        internal static JObject LoadJson(TextReader reader)
        {
            using (var jsonReader = new JsonTextReader(reader))
            {
                while (jsonReader.TokenType != JsonToken.StartObject)
                {
                    if (!jsonReader.Read())
                    {
                        throw new InvalidDataException();
                    }
                }

                return JObject.Load(jsonReader, DefaultLoadSettings);
            }
        }

        internal static T LoadJson<T>(Stream stream, IUtf8JsonStreamReaderConverter<T> converter)
        {
            var streamingJsonReader = new Utf8JsonStreamReader(stream);
            return converter.Read(ref streamingJsonReader);
        }

        internal static PackageDependency ReadPackageDependency(string property, JToken json)
        {
            var versionStr = json.Value<string>();
            return new PackageDependency(
                property,
                versionStr == null ? null : VersionRange.Parse(versionStr));
        }

        internal static bool UseNewtonsoftJsonForParsing(IEnvironmentVariableReader environmentVariableReader, bool bypassCache)
        {
            if (!UseNewtonsoftJson.HasValue || bypassCache)
            {
                if (bool.TryParse(environmentVariableReader.GetEnvironmentVariable(NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING), out var useNj))
                {
                    UseNewtonsoftJson = useNj;
                }
                else
                {
                    UseNewtonsoftJson = false;
                }
            }

            return UseNewtonsoftJson.Value;
        }

        internal static JProperty WritePackageDependencyWithLegacyString(PackageDependency item)
        {
            return new JProperty(
                item.Id,
                WriteString(item.VersionRange?.ToNonSnapshotRange().ToLegacyShortString()));
        }

        internal static void WritePackageDependencyWithLegacyString(JsonWriter writer, PackageDependency item)
        {
            writer.WritePropertyName(item.Id);
            writer.WriteValue(item.VersionRange?.ToNonSnapshotRange().ToLegacyShortString());
        }

        internal static JProperty WritePackageDependency(PackageDependency item)
        {
            return new JProperty(
                item.Id,
                WriteString(item.VersionRange?.ToString()));
        }

        internal static void WritePackageDependency(JsonWriter writer, PackageDependency item)
        {
            writer.WritePropertyName(item.Id);
            writer.WriteValue(item.VersionRange?.ToString());
        }

        internal static TItem ReadProperty<TItem>(JObject jObject, string propertyName)
        {
            if (jObject != null)
            {
                JToken value;
                if (jObject.TryGetValue(propertyName, out value) && value != null)
                {
                    return value.Value<TItem>();
                }
            }

            return default(TItem);
        }

        internal static IList<TItem> ReadObject<TItem>(JObject jObject, Func<string, JToken, TItem> readItem)
        {
            if (jObject == null)
            {
                return new List<TItem>(0);
            }
            var items = new List<TItem>(jObject.Count);
            foreach (var child in jObject)
            {
                items.Add(readItem(child.Key, child.Value));
            }
            return items;
        }

        internal static JObject WriteObject<TItem>(IEnumerable<TItem> items, Func<TItem, JProperty> writeItem)
        {
            var array = new JObject();
            foreach (var item in items)
            {
                array.Add(writeItem(item));
            }
            return array;
        }

        internal static void WriteObject<TItem>(JsonWriter writer, IEnumerable<TItem> items, Action<JsonWriter, TItem> writeItem)
        {
            writer.WriteStartObject();

            foreach (var item in items)
            {
                writeItem(writer, item);
            }

            writer.WriteEndObject();
        }

        internal static int ReadInt(JToken cursor, string property, int defaultValue)
        {
            var valueToken = cursor[property];
            if (valueToken == null)
            {
                return defaultValue;
            }
            return valueToken.Value<int>();
        }

        internal static JToken WriteString(string item)
        {
            return item != null ? new JValue(item) : JValue.CreateNull();
        }
    }
}
