// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class PackageReport
    {
        public PackageReport()
        {
            Targets = new Dictionary<string, Target>();
        }

        public string Id { get; set; }
        public string Version { get; set; }

        public Dictionary<string, string> SupportedFrameworks { get; set; }
        public Dictionary<string,Target> Targets { get; set; }
        public PackageAsset[] UnusedAssets { get; set; }

        public void Save(string path)
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var file = File.CreateText(path))
            {
                var serializer = new JsonSerializer();
                serializer.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;
                serializer.Formatting = Formatting.Indented;
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
                serializer.Converters.Add(new VersionConverter());
                serializer.Converters.Add(new NuGetFrameworkConverter());
                serializer.Serialize(file, this);
            }
        }

        public static PackageReport Load(string path)
        {
            using (var file = File.OpenText(path))
            using (var jsonTextReader = new JsonTextReader(file))
            {
                var serializer = new JsonSerializer();
                serializer.Converters.Add(new VersionConverter());
                serializer.Converters.Add(new NuGetFrameworkConverter());
                return serializer.Deserialize<PackageReport>(jsonTextReader);
            }
        }
    }

    internal class NuGetFrameworkConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(NuGetFramework);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var lineInfo = reader as IJsonLineInfo;
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    return null;
                case JsonToken.String:
                    try
                    {
                        return NuGetFramework.Parse((string)reader.Value);
                    }
                    catch(Exception exception)
                    {
                        throw new JsonSerializationException($"Failed to parse {nameof(NuGetFramework)} {reader.Value}.  Line {lineInfo.LineNumber}, position {lineInfo.LinePosition}", exception);
                    }
                default:
                    throw new JsonSerializationException($"Unexpected token type {reader.TokenType} when parsing {nameof(NuGetFramework)}.  Line {lineInfo.LineNumber}, position {lineInfo.LinePosition}");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else if (value is NuGetFramework)
            {
                writer.WriteValue(value.ToString());
            }
            else
            {
                throw new JsonSerializationException($"Expected {nameof(NuGetFramework)} but got {value.GetType()}");
            }
        }
    }

    public class Target
    {
        private static readonly PackageAsset[] s_emptyPackageAsset = new PackageAsset[0];
        public Target()
        {
            CompileAssets = RuntimeAssets = NativeAssets = s_emptyPackageAsset;
        }

        public string Framework { get; set; }
        public string RuntimeID { get; set; }

        public PackageAsset[] CompileAssets { get; set; }
        public PackageAsset[] RuntimeAssets { get; set; }
        public PackageAsset[] NativeAssets { get; set; }

        public bool ShouldSerializeCompileAssets() { return CompileAssets != null && CompileAssets.Length != 0; }
        public bool ShouldSerializeRuntimeAssets() { return RuntimeAssets != null && RuntimeAssets.Length != 0; }
        public bool ShouldSerializeNativeAssets() { return NativeAssets != null && NativeAssets.Length != 0; }
    }

    public class PackageAsset
    {
        public string HarvestedFrom { get; set; }
        public string LocalPath { get; set; }
        public string PackagePath { get; set; }
        public BuildProject SourceProject { get; set; }
        public NuGetFramework TargetFramework { get; set;}
        public Version Version { get; set; }
    }

    public class BuildProject
    {
        public string Project { get; set; }
        public string AdditionalProperties { get; set; }
        public string UndefineProperties { get; set; }

        public override bool Equals(object other)
        {
            var otherProject = other as BuildProject;

            return otherProject != null &&
                    Project == otherProject.Project &&
                    AdditionalProperties == otherProject.AdditionalProperties &&
                    UndefineProperties == otherProject.UndefineProperties;
        }

        public override int GetHashCode()
        {
            var hash = Project.GetHashCode();
            hash ^= AdditionalProperties.GetHashCode();
            hash ^= UndefineProperties.GetHashCode();
            return hash;
        }

        public ITaskItem ToItem()
        {
            var item = new TaskItem(Project);
            item.SetMetadata(nameof(AdditionalProperties), AdditionalProperties);
            item.SetMetadata(nameof(UndefineProperties), UndefineProperties);
            return item;
        }
    }
}
