// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;

namespace NuGet.Protocol.Converters
{
    /// <remarks>
    /// NSJ deserializes <see cref="PackageDependencyGroup"/> via a private constructor attributed with
    /// <c>[JsonConstructor]</c>. STJ source generation cannot access private constructors, so this
    /// converter handles deserialization explicitly.
    /// </remarks>
    internal sealed class PackageDependencyGroupStjConverter : JsonConverter<PackageDependencyGroup>
    {
        public override PackageDependencyGroup Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException(string.Format(CultureInfo.CurrentCulture, Strings.Error_UnexpectedJsonToken, reader.TokenType));
            }

            NuGetFramework? targetFramework = null;
            var packages = new List<PackageDependency>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException(string.Format(CultureInfo.CurrentCulture, Strings.Error_UnexpectedJsonToken, reader.TokenType));
                }

                var propName = reader.GetString();
                reader.Read();

                if (string.Equals(propName, JsonProperties.TargetFramework, StringComparison.OrdinalIgnoreCase))
                {
                    if (reader.TokenType != JsonTokenType.Null)
                    {
                        string? fw = reader.GetString();
                        targetFramework = string.IsNullOrEmpty(fw) ? null : NuGetFramework.Parse(fw!);
                    }
                }
                else if (string.Equals(propName, JsonProperties.Dependencies, StringComparison.OrdinalIgnoreCase))
                {
                    if (reader.TokenType == JsonTokenType.StartArray)
                    {
                        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                        {
                            PackageDependency? dependency = JsonSerializer.Deserialize(ref reader, PackageSearchJsonContext.Default.PackageDependency);
                            if (dependency != null)
                            {
                                packages.Add(dependency);
                            }
                        }
                    }
                    else if (reader.TokenType != JsonTokenType.Null)
                    {
                        throw new JsonException(string.Format(CultureInfo.CurrentCulture, Strings.Error_UnexpectedJsonToken, reader.TokenType));
                    }
                }
                else
                {
                    reader.Skip();
                }
            }

            return new PackageDependencyGroup(targetFramework ?? NuGetFramework.AnyFramework, packages);
        }

        public override void Write(Utf8JsonWriter writer, PackageDependencyGroup value, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
        }
    }
}
