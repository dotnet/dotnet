// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;
using System.Text.Json;
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

            var lockFileLibrary = new LockFileLibrary();
            //We want to read the property name right away
            var propertyName = reader.GetString();
            var (name, version) = propertyName.SplitInTwo(LockFile.DirectorySeparatorChar);
            lockFileLibrary.Name = name;
            if (!string.IsNullOrWhiteSpace(version))
            {
                lockFileLibrary.Version = NuGetVersion.Parse(version);
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected StartObject, found " + reader.TokenType);
            }

            while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
            {
                if (reader.ValueTextEquals(TypePropertyName))
                {
                    lockFileLibrary.Type = reader.ReadNextTokenAsString();
                }
                else if (reader.ValueTextEquals(PathPropertyName))
                {
                    lockFileLibrary.Path = reader.ReadNextTokenAsString();
                }
                else if (reader.ValueTextEquals(MsbuildProjectPropertyName))
                {
                    lockFileLibrary.MSBuildProject = reader.ReadNextTokenAsString();
                }
                else if (reader.ValueTextEquals(Sha512PropertyName))
                {
                    lockFileLibrary.Sha512 = reader.ReadNextTokenAsString();
                }
                else if (reader.ValueTextEquals(ServicablePropertyName))
                {
                    reader.Read();
                    lockFileLibrary.IsServiceable = reader.GetBoolean();
                }
                else if (reader.ValueTextEquals(HasToolsPropertyName))
                {
                    reader.Read();
                    lockFileLibrary.HasTools = reader.GetBoolean();
                }
                else if (reader.ValueTextEquals(FilesPropertyName))
                {
                    reader.Read();
                    reader.ReadStringArrayAsIList(lockFileLibrary.Files);
                }
                else
                {
                    reader.Skip();
                }
            }
            return lockFileLibrary;
        }
    }
}
