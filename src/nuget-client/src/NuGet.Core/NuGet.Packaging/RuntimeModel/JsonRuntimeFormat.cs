// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using NuGet.Shared;
using NuGet.Versioning;
using JsonException = System.Text.Json.JsonException;

namespace NuGet.RuntimeModel
{
    public static class JsonRuntimeFormat
    {
        /// <summary>
        /// Reads a runtime graph from a UTF-8 file with or without a byte order mark.
        /// </summary>
        public static RuntimeGraph ReadRuntimeGraph(string filePath)
        {
            using (var fileStream = File.OpenRead(filePath))
            {
                return ReadRuntimeGraph(fileStream);
            }
        }

        /// <summary>
        /// Reads a runtime graph from a UTF-8 stream with or without a byte order mark.
        /// The stream will be disposed at the end of reading.
        /// </summary>
        public static RuntimeGraph ReadRuntimeGraph(Stream stream)
        {
            using (stream)
            {
                return ReadUtf8RuntimeGraph(stream);
            }
        }

        private static RuntimeGraph ReadUtf8RuntimeGraph(Stream stream)
        {
            var reader = new Utf8JsonStreamReader(stream);
            try
            {
                return ReadRuntimeGraph(ref reader);
            }
            finally
            {
                reader.Dispose();
            }
        }

        /// <summary>
        /// Reads a runtime graph from a JSON stream in a single pass.
        /// </summary>
        private static RuntimeGraph ReadRuntimeGraph(ref Utf8JsonStreamReader reader)
        {
            if (reader.TokenType == JsonTokenType.None)
            {
                reader.Read();
            }

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            List<RuntimeDescription>? runtimes = null;
            List<CompatibilityProfile>? supports = null;

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                bool isRuntimes = reader.ValueTextEquals("runtimes"u8);
                bool isSupports = reader.ValueTextEquals("supports"u8);
                ReadValue(ref reader);

                if (isRuntimes)
                {
                    runtimes = ReadRuntimeDescriptions(ref reader);
                }
                else if (isSupports)
                {
                    supports = ReadCompatibilityProfiles(ref reader);
                }
                else
                {
                    reader.Skip();
                }
            }

            if (reader.TokenType != JsonTokenType.EndObject
                || reader.Read())
            {
                throw new JsonException();
            }

            return new RuntimeGraph(
                runtimes is null ? Array.Empty<RuntimeDescription>() : runtimes,
                supports is null ? Array.Empty<CompatibilityProfile>() : supports);
        }

        private static List<RuntimeDescription> ReadRuntimeDescriptions(ref Utf8JsonStreamReader reader)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                reader.Skip();
                return [];
            }

            List<RuntimeDescription> values = [];

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                string runtimeIdentifier = ReadPropertyName(ref reader);
                ReadValue(ref reader);
                RuntimeDescription? runtime = ReadRuntimeDescription(ref reader, runtimeIdentifier);
                if (runtime is null)
                {
                    throw new JsonException();
                }

                values.Add(runtime);
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }

            return values;
        }

        private static RuntimeDescription? ReadRuntimeDescription(
            ref Utf8JsonStreamReader reader,
            string runtimeIdentifier)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                reader.Skip();
                return new RuntimeDescription(runtimeIdentifier);
            }

            List<string>? inheritedRuntimes = null;
            List<RuntimeDependencySet> dependencySets = [];

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType == JsonTokenType.PropertyName
                    && reader.ValueTextEquals("#import"u8))
                {
                    ReadValue(ref reader);
                    inheritedRuntimes = ReadStringArray(ref reader);
                    if (inheritedRuntimes is null)
                    {
                        return null;
                    }
                }
                else
                {
                    string propertyName = ReadPropertyName(ref reader);
                    ReadValue(ref reader);
                    RuntimeDependencySet? dependencySet = ReadRuntimeDependencySet(ref reader, propertyName);
                    if (dependencySet is null)
                    {
                        return null;
                    }

                    dependencySets.Add(dependencySet);
                }
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                return null;
            }

            return new RuntimeDescription(runtimeIdentifier, inheritedRuntimes, dependencySets);
        }

        private static RuntimeDependencySet? ReadRuntimeDependencySet(
            ref Utf8JsonStreamReader reader,
            string dependencySetId)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                reader.Skip();
                return new RuntimeDependencySet(dependencySetId);
            }

            List<RuntimePackageDependency> dependencies = [];

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                string packageId = ReadPropertyName(ref reader);
                string? versionRange = ReadNextScalarAsString(ref reader);
                reader.Skip();
                if (versionRange is null)
                {
                    return null;
                }

                dependencies.Add(new RuntimePackageDependency(packageId, VersionRange.Parse(versionRange)));
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                return null;
            }

            return new RuntimeDependencySet(dependencySetId, dependencies);
        }

        private static List<CompatibilityProfile> ReadCompatibilityProfiles(ref Utf8JsonStreamReader reader)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                reader.Skip();
                return [];
            }

            List<CompatibilityProfile> values = [];

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                string profileName = ReadPropertyName(ref reader);
                ReadValue(ref reader);
                CompatibilityProfile? profile = ReadCompatibilityProfile(ref reader, profileName);
                if (profile is null)
                {
                    throw new JsonException();
                }

                values.Add(profile);
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }

            return values;
        }

        private static CompatibilityProfile? ReadCompatibilityProfile(
            ref Utf8JsonStreamReader reader,
            string profileName)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                reader.Skip();
                return new CompatibilityProfile(profileName);
            }

            List<FrameworkRuntimePair> restoreContexts = [];

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                string frameworkName = ReadPropertyName(ref reader);
                ReadValue(ref reader);
                List<FrameworkRuntimePair>? pairs = ReadFrameworkRuntimePairs(
                    ref reader,
                    NuGetFramework.Parse(frameworkName));
                if (pairs is null)
                {
                    return null;
                }

                restoreContexts.AddRange(pairs);
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                return null;
            }

            return new CompatibilityProfile(profileName, restoreContexts);
        }

        private static List<FrameworkRuntimePair>? ReadFrameworkRuntimePairs(
            ref Utf8JsonStreamReader reader,
            NuGetFramework framework)
        {
            var values = new List<FrameworkRuntimePair>();
            if (reader.TokenType == JsonTokenType.String)
            {
                values.Add(new FrameworkRuntimePair(framework, reader.GetString()));
            }
            else if (reader.TokenType == JsonTokenType.StartArray)
            {
                List<string?>? runtimeIdentifiers = ReadScalarArrayAsStrings(ref reader);
                if (runtimeIdentifiers is not null)
                {
                    foreach (string? runtimeIdentifier in runtimeIdentifiers)
                    {
                        values.Add(new FrameworkRuntimePair(framework, runtimeIdentifier));
                    }
                }
            }
            else
            {
                reader.Skip();
            }

            return values;
        }

        private static List<string>? ReadStringArray(ref Utf8JsonStreamReader reader)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                reader.Skip();
                return null;
            }

            List<string?>? values = ReadScalarArrayAsStrings(ref reader);
            if (values is null)
            {
                return [];
            }

            // RuntimeDescription historically permits null entries despite its non-nullable public contract.
            return values.ConvertAll(value => value!);
        }

        private static string? ReadNextScalarAsString(ref Utf8JsonStreamReader reader)
        {
            if (!reader.Read())
            {
                throw new JsonException();
            }

            return reader.ReadScalarAsInvariantString();
        }

        private static List<string?>? ReadScalarArrayAsStrings(ref Utf8JsonStreamReader reader)
        {
            List<string?>? values = null;
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                values ??= [];
                values.Add(reader.ReadScalarAsInvariantString());
            }

            if (reader.TokenType != JsonTokenType.EndArray)
            {
                throw new JsonException();
            }

            return values;
        }

        private static string ReadPropertyName(ref Utf8JsonStreamReader reader)
        {
            return reader.TokenType == JsonTokenType.PropertyName
                ? reader.GetString() ?? throw new JsonException()
                : throw new JsonException();
        }

        private static void ReadValue(ref Utf8JsonStreamReader reader)
        {
            if (!reader.Read())
            {
                throw new JsonException();
            }
        }

        [Obsolete("Use ReadRuntimeGraph(Stream) instead.")]
        public static RuntimeGraph ReadRuntimeGraph(TextReader textReader)
        {
            var loadSettings = new JsonLoadSettings()
            {
                LineInfoHandling = LineInfoHandling.Ignore,
                CommentHandling = CommentHandling.Ignore
            };

            using (var jsonReader = new JsonTextReader(textReader))
            {
                return ReadRuntimeGraph(JToken.Load(jsonReader, loadSettings));
            }
        }

        public static void WriteRuntimeGraph(string filePath, RuntimeGraph runtimeGraph)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            using (var textWriter = new StreamWriter(fileStream))
            using (var jsonWriter = new JsonTextWriter(textWriter))
            using (var writer = new JsonObjectWriter(jsonWriter))
            {
                jsonWriter.Formatting = Formatting.Indented;
                jsonWriter.WriteStartObject();
                WriteRuntimeGraph(writer, runtimeGraph);
                jsonWriter.WriteEndObject();
            }
        }

        public static RuntimeGraph ReadRuntimeGraph(JToken json)
        {
            return new RuntimeGraph(
                EachProperty(json["runtimes"]).Select(ReadRuntimeDescription),
                EachProperty(json["supports"]).Select(ReadCompatibilityProfile));
        }

        public static void WriteRuntimeGraph(IObjectWriter writer, RuntimeGraph runtimeGraph)
        {
            if (runtimeGraph != null)
            {
                if (runtimeGraph.Runtimes.Any() == true)
                {
                    writer.WriteObjectStart("runtimes");

                    IOrderedEnumerable<RuntimeDescription> sortedRuntimes = runtimeGraph.Runtimes.Values
                        .OrderBy(runtime => runtime.RuntimeIdentifier, StringComparer.Ordinal);

                    foreach (RuntimeDescription runtime in sortedRuntimes)
                    {
                        WriteRuntimeDescription(writer, runtime);
                    }

                    writer.WriteObjectEnd();
                }

                if (runtimeGraph.Supports.Any() == true)
                {
                    writer.WriteObjectStart("supports");

                    IOrderedEnumerable<CompatibilityProfile> sortedSupports = runtimeGraph.Supports.Values
                        .OrderBy(runtime => runtime.Name, StringComparer.Ordinal);

                    foreach (CompatibilityProfile support in sortedSupports)
                    {
                        WriteCompatibilityProfile(writer, support);
                    }

                    writer.WriteObjectEnd();
                }
            }
        }

        private static void WriteRuntimeDescription(IObjectWriter writer, RuntimeDescription data)
        {
            writer.WriteObjectStart(data.RuntimeIdentifier);

            writer.WriteNameArray("#import", data.InheritedRuntimes);

            var sortedDependencySets = data.RuntimeDependencySets
                                           .OrderBy(pair => pair.Key, StringComparer.Ordinal)
                                           .Select(pair => pair.Value);

            foreach (var set in sortedDependencySets)
            {
                WriteRuntimeDependencySet(writer, set);
            }

            writer.WriteObjectEnd();
        }

        private static void WriteRuntimeDependencySet(IObjectWriter writer, RuntimeDependencySet data)
        {
            writer.WriteObjectStart(data.Id);

            var sortedDependencies = data.Dependencies
                                         .OrderBy(pair => pair.Key, StringComparer.Ordinal)
                                         .Select(pair => pair.Value);

            foreach (var dependency in sortedDependencies)
            {
                WritePackageDependency(writer, dependency);
            }

            writer.WriteObjectEnd();
        }

        private static void WritePackageDependency(IObjectWriter writer, RuntimePackageDependency data)
        {
            writer.WriteNameValue(data.Id, data.VersionRange.ToNormalizedString());
        }

        private static void WriteCompatibilityProfile(IObjectWriter writer, CompatibilityProfile data)
        {
            writer.WriteObjectStart(data.Name);

            var frameworkGroups = data.RestoreContexts.GroupBy(context => context.Framework);

            foreach (var frameworkGroup in frameworkGroups)
            {
                var name = frameworkGroup.Key.GetShortFolderName();
                var runtimes = frameworkGroup.ToList();
                if (runtimes.Count == 1)
                {
                    // Write a string
                    writer.WriteNameValue(name, runtimes[0].RuntimeIdentifier);
                }
                else if (runtimes.Count > 0)
                {
                    writer.WriteNameArray(name, runtimes.Select(rt => rt.RuntimeIdentifier));
                }
            }

            writer.WriteObjectEnd();
        }

        private static CompatibilityProfile ReadCompatibilityProfile(KeyValuePair<string, JToken> json)
        {
            var name = json.Key;
            var sets = new List<FrameworkRuntimePair>();
            foreach (var property in EachProperty(json.Value))
            {
                var profiles = ReadCompatibilitySets(property);
                sets.AddRange(profiles);
            }
            return new CompatibilityProfile(name, sets);
        }

        private static IEnumerable<FrameworkRuntimePair> ReadCompatibilitySets(KeyValuePair<string, JToken> property)
        {
            var framework = NuGetFramework.Parse(property.Key);
            switch (property.Value.Type)
            {
                case JTokenType.Array:
                    foreach (var value in (JArray)property.Value)
                    {
                        yield return new FrameworkRuntimePair(framework, value.Value<string>());
                    }
                    break;
                case JTokenType.String:
                    yield return new FrameworkRuntimePair(framework, property.Value.ToString());
                    break;
                    // Other token types are not supported
            }
        }

        private static RuntimeDescription ReadRuntimeDescription(KeyValuePair<string, JToken> json)
        {
            var name = json.Key;
            List<string>? inheritedRuntimes = null;
            List<RuntimeDependencySet>? additionalDependencies = null;
            foreach (var property in EachProperty(json.Value))
            {
                if (property.Key == "#import")
                {
                    var imports = (JArray)property.Value;
                    foreach (var import in imports)
                    {
                        inheritedRuntimes ??= new List<string>(imports.Count);
                        inheritedRuntimes.Add(import.Value<string>()!);
                    }
                }
                else
                {
                    var dependency = ReadRuntimeDependencySet(property);
                    additionalDependencies ??= new();
                    additionalDependencies.Add(dependency);
                }
            }
            return new RuntimeDescription(name, inheritedRuntimes, additionalDependencies);
        }

        private static RuntimeDependencySet ReadRuntimeDependencySet(KeyValuePair<string, JToken> json)
        {
            return new RuntimeDependencySet(
                json.Key,
                EachProperty(json.Value).Select(ReadRuntimePackageDependency));
        }

        private static RuntimePackageDependency ReadRuntimePackageDependency(KeyValuePair<string, JToken> json)
        {
            return new RuntimePackageDependency(json.Key, VersionRange.Parse(json.Value.Value<string>()!));
        }

        private static IEnumerable<KeyValuePair<string, JToken>> EachProperty(JToken? json)
        {
            return (json as IEnumerable<KeyValuePair<string, JToken>>)
                   ?? Enumerable.Empty<KeyValuePair<string, JToken>>();
        }
    }
}
