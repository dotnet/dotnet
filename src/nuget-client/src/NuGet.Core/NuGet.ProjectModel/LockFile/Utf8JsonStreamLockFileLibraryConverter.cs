// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Immutable;
using System.Text;
using System.Text.Json;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// A <see cref="IUtf8JsonStreamReaderConverter{T}"/> to allow read JSON into <see cref="LockFileLibrary"/>
    /// </summary>
    /// <example>
    /// "PackageA/1.0.0": {
    ///     "sha512": "ASha512",
    ///     "type": "package",
    ///     "path": "C:\a\test\path",
    ///     "files": [
    ///         "PackageA.nuspec",
    ///         "lib/netstandard2.0/PackageA.dll"
    ///     ],
    ///     "msbuildProject": "bar",
    ///     "servicable": true,
    ///     "hasTools": true,
    /// }
    /// </example>
    internal class Utf8JsonStreamLockFileLibraryConverter : IUtf8JsonStreamReaderConverter<LockFileLibrary>
    {
        private static readonly byte[] Sha512PropertyName = Encoding.UTF8.GetBytes("sha512");
        private static readonly byte[] TypePropertyName = Encoding.UTF8.GetBytes("type");
        private static readonly byte[] PathPropertyName = Encoding.UTF8.GetBytes("path");
        private static readonly byte[] MsbuildProjectPropertyName = Encoding.UTF8.GetBytes("msbuildProject");
        private static readonly byte[] ServicablePropertyName = Encoding.UTF8.GetBytes("servicable");
        private static readonly byte[] HasToolsPropertyName = Encoding.UTF8.GetBytes("hasTools");
        private static readonly byte[] FilesPropertyName = Encoding.UTF8.GetBytes("files");

        public LockFileLibrary Read(ref Utf8JsonStreamReader reader)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected PropertyName, found " + reader.TokenType);
            }

            //We want to read the property name right away
            var propertyName = reader.GetString();
            var (name, version) = propertyName.SplitInTwo(LockFile.DirectorySeparatorChar);

            var nugetVersion = !string.IsNullOrWhiteSpace(version) ? NuGetVersion.Parse(version) : null;

            reader.Read();
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected StartObject, found " + reader.TokenType);
            }

            string type = null;
            string path = null;
            string mSBuildProject = null;
            string sha512 = null;
            var isServiceable = false;
            var hasTools = false;
            var files = ImmutableArray<string>.Empty;
            while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
            {
                if (reader.ValueTextEquals(TypePropertyName))
                {
                    type = reader.ReadNextTokenAsString();
                }
                else if (reader.ValueTextEquals(PathPropertyName))
                {
                    path = reader.ReadNextTokenAsString();
                }
                else if (reader.ValueTextEquals(MsbuildProjectPropertyName))
                {
                    mSBuildProject = reader.ReadNextTokenAsString();
                }
                else if (reader.ValueTextEquals(Sha512PropertyName))
                {
                    sha512 = reader.ReadNextTokenAsString();
                }
                else if (reader.ValueTextEquals(ServicablePropertyName))
                {
                    reader.Read();
                    isServiceable = reader.GetBoolean();
                }
                else if (reader.ValueTextEquals(HasToolsPropertyName))
                {
                    reader.Read();
                    hasTools = reader.GetBoolean();
                }
                else if (reader.ValueTextEquals(FilesPropertyName))
                {
                    reader.Read();
                    files = reader.ReadStringArrayAsImmutableArray();
                }
                else
                {
                    reader.Skip();
                }
            }

            var lockFileLibrary = new LockFileLibrary()
            {
                Files = files,
                HasTools = hasTools,
                IsServiceable = isServiceable,
                MSBuildProject = mSBuildProject,
                Name = name,
                Path = path,
                Sha512 = sha512,
                Type = type,
                Version = nugetVersion
            };

            return lockFileLibrary;
        }
    }
}
