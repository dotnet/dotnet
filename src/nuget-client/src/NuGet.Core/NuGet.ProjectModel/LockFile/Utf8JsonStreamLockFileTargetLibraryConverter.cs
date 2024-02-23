// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// A <see cref="IUtf8JsonStreamReaderConverter{T}"/> to allow read JSON into <see cref="LockFileTargetLibrary"/>
    /// </summary>
    /// <example>
    /// "Lirbary/1.0.0": {
    ///     "type": "package",
    ///     "framework": ".NETCoreApp,Version=v6.0",
    ///     "dependencies": {
    ///         "Library.Name": "1.0.1",
    ///         "Parser.Json": "10.0.0",
    ///     },
    ///     "frameworkAssemblies": [
    ///         "System"
    ///     ],
    ///     "compile": {
    ///         <see cref="Utf8JsonStreamLockFileItemConverter{T}"/>,
    ///     },
    ///     "runtime": {
    ///         <see cref="Utf8JsonStreamLockFileItemConverter{T}"/>,
    ///     },
    ///     "resource": {
    ///         <see cref="Utf8JsonStreamLockFileItemConverter{T}"/>,
    ///     },
    ///     "contentFiles": {
    ///         <see cref="Utf8JsonStreamLockFileItemConverter{T}"/>,
    ///     },
    ///     "runtimeTargets": {
    ///         <see cref="Utf8JsonStreamLockFileItemConverter{T}"/>,
    ///     },
    ///     "tools": {
    ///         <see cref="Utf8JsonStreamLockFileItemConverter{T}"/>,
    ///     },
    ///     "embed": {
    ///         <see cref="Utf8JsonStreamLockFileItemConverter{T}"/>,
    ///     },
    ///     "frameworkReferences": [
    ///         "Framework1",
    ///     ]
    /// }
    /// </example>
    internal class Utf8JsonStreamLockFileTargetLibraryConverter : IUtf8JsonStreamReaderConverter<LockFileTargetLibrary>
    {
        private static readonly byte[] TypePropertyName = Encoding.UTF8.GetBytes("type");
        private static readonly byte[] FrameworkPropertyName = Encoding.UTF8.GetBytes("framework");
        private static readonly byte[] DependenciesPropertyName = Encoding.UTF8.GetBytes("dependencies");
        private static readonly byte[] FrameworkAssembliesPropertyName = Encoding.UTF8.GetBytes("frameworkAssemblies");
        private static readonly byte[] RuntimePropertyName = Encoding.UTF8.GetBytes("runtime");
        private static readonly byte[] CompilePropertyName = Encoding.UTF8.GetBytes("compile");
        private static readonly byte[] ResourcePropertyName = Encoding.UTF8.GetBytes("resource");
        private static readonly byte[] NativePropertyName = Encoding.UTF8.GetBytes("native");
        private static readonly byte[] BuildPropertyName = Encoding.UTF8.GetBytes("build");
        private static readonly byte[] BuildMultiTargetingPropertyName = Encoding.UTF8.GetBytes("buildMultiTargeting");
        private static readonly byte[] ContentFilesPropertyName = Encoding.UTF8.GetBytes("contentFiles");
        private static readonly byte[] RuntimeTargetsPropertyName = Encoding.UTF8.GetBytes("runtimeTargets");
        private static readonly byte[] ToolsPropertyName = Encoding.UTF8.GetBytes("tools");
        private static readonly byte[] EmbedPropertyName = Encoding.UTF8.GetBytes("embed");
        private static readonly byte[] FrameworkReferencesPropertyName = Encoding.UTF8.GetBytes("frameworkReferences");

        public LockFileTargetLibrary Read(ref Utf8JsonStreamReader reader)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected PropertyName, found " + reader.TokenType);
            }

            var lockFileTargetLibrary = new LockFileTargetLibrary();

            //We want to read the property name right away
            var propertyName = reader.GetString();
            var (targetLibraryName, version) = propertyName.SplitInTwo('/');
            lockFileTargetLibrary.Name = targetLibraryName;
            lockFileTargetLibrary.Version = version is null ? null : NuGetVersion.Parse(version);

            reader.Read();
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected StartObject, found " + reader.TokenType);
            }

            while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
            {
                if (reader.ValueTextEquals(TypePropertyName))
                {
                    lockFileTargetLibrary.Type = reader.ReadNextTokenAsString();
                }
                else if (reader.ValueTextEquals(FrameworkPropertyName))
                {
                    lockFileTargetLibrary.Framework = reader.ReadNextTokenAsString();
                }
                else if (reader.ValueTextEquals(DependenciesPropertyName))
                {
                    reader.Read();
                    lockFileTargetLibrary.Dependencies = ReadPackageDependencyList(ref reader);
                }
                else if (reader.ValueTextEquals(FrameworkAssembliesPropertyName))
                {
                    reader.Read();
                    lockFileTargetLibrary.FrameworkAssemblies = reader.ReadStringArrayAsIList() ?? Array.Empty<string>();
                }
                else if (reader.ValueTextEquals(RuntimePropertyName))
                {
                    reader.Read();
                    lockFileTargetLibrary.RuntimeAssemblies = reader.ReadObjectAsList<LockFileItem>(Utf8JsonReaderExtensions.LockFileItemConverter);
                }
                else if (reader.ValueTextEquals(CompilePropertyName))
                {
                    reader.Read();
                    lockFileTargetLibrary.CompileTimeAssemblies = reader.ReadObjectAsList<LockFileItem>(Utf8JsonReaderExtensions.LockFileItemConverter);
                }
                else if (reader.ValueTextEquals(ResourcePropertyName))
                {
                    reader.Read();
                    lockFileTargetLibrary.ResourceAssemblies = reader.ReadObjectAsList<LockFileItem>(Utf8JsonReaderExtensions.LockFileItemConverter);
                }
                else if (reader.ValueTextEquals(NativePropertyName))
                {
                    reader.Read();
                    lockFileTargetLibrary.NativeLibraries = reader.ReadObjectAsList<LockFileItem>(Utf8JsonReaderExtensions.LockFileItemConverter);
                }
                else if (reader.ValueTextEquals(BuildPropertyName))
                {
                    reader.Read();
                    lockFileTargetLibrary.Build = reader.ReadObjectAsList<LockFileItem>(Utf8JsonReaderExtensions.LockFileItemConverter);
                }
                else if (reader.ValueTextEquals(BuildMultiTargetingPropertyName))
                {
                    reader.Read();
                    lockFileTargetLibrary.BuildMultiTargeting = reader.ReadObjectAsList<LockFileItem>(Utf8JsonReaderExtensions.LockFileItemConverter);
                }
                else if (reader.ValueTextEquals(ContentFilesPropertyName))
                {
                    reader.Read();
                    lockFileTargetLibrary.ContentFiles = reader.ReadObjectAsList<LockFileContentFile>(Utf8JsonReaderExtensions.LockFileContentFileConverter);
                }
                else if (reader.ValueTextEquals(RuntimeTargetsPropertyName))
                {
                    reader.Read();
                    lockFileTargetLibrary.RuntimeTargets = reader.ReadObjectAsList<LockFileRuntimeTarget>(Utf8JsonReaderExtensions.LockFileRuntimeTargetConverter);
                }
                else if (reader.ValueTextEquals(ToolsPropertyName))
                {
                    reader.Read();
                    lockFileTargetLibrary.ToolsAssemblies = reader.ReadObjectAsList<LockFileItem>(Utf8JsonReaderExtensions.LockFileItemConverter);
                }
                else if (reader.ValueTextEquals(EmbedPropertyName))
                {
                    reader.Read();
                    lockFileTargetLibrary.EmbedAssemblies = reader.ReadObjectAsList<LockFileItem>(Utf8JsonReaderExtensions.LockFileItemConverter);
                }
                else if (reader.ValueTextEquals(FrameworkReferencesPropertyName))
                {
                    reader.Read();
                    lockFileTargetLibrary.FrameworkReferences = reader.ReadStringArrayAsIList();
                }
                else
                {
                    reader.Skip();
                }
            }
            lockFileTargetLibrary.Freeze();
            return lockFileTargetLibrary;
        }

        private IList<PackageDependency> ReadPackageDependencyList(ref Utf8JsonStreamReader reader)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                return Array.Empty<PackageDependency>();
            }

            var packageDependencies = new List<PackageDependency>(10);
            while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString();
                string versionString = reader.ReadNextTokenAsString();
                packageDependencies.Add(new PackageDependency(
                    propertyName,
                    versionString == null ? null : VersionRange.Parse(versionString)));
            }
            return packageDependencies;
        }
    }
}
