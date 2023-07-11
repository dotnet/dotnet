// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using NuGet.Versioning;

namespace NuGet.RuntimeModel
{
    public static class JsonRuntimeFormat
    {
        public static RuntimeGraph ReadRuntimeGraph(string filePath)
        {
            using (var fileStream = File.OpenRead(filePath))
            {
                return ReadRuntimeGraph(fileStream);
            }
        }

        public static RuntimeGraph ReadRuntimeGraph(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                return ReadRuntimeGraph(streamReader);
            }
        }

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
            List<string> inheritedRuntimes = null;
            List<RuntimeDependencySet> additionalDependencies = null;
            foreach (var property in EachProperty(json.Value))
            {
                if (property.Key == "#import")
                {
                    var imports = property.Value as JArray;
                    foreach (var import in imports)
                    {
                        inheritedRuntimes ??= new();
                        inheritedRuntimes.Add(import.Value<string>());
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
            return new RuntimePackageDependency(json.Key, VersionRange.Parse(json.Value.Value<string>()));
        }

        private static IEnumerable<KeyValuePair<string, JToken>> EachProperty(JToken json)
        {
            return (json as IEnumerable<KeyValuePair<string, JToken>>)
                   ?? Enumerable.Empty<KeyValuePair<string, JToken>>();
        }
    }
}
