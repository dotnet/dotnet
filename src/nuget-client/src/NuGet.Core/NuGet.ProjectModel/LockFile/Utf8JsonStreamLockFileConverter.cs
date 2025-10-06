// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using NuGet.Common;
using NuGet.Frameworks;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// A <see cref="IUtf8JsonStreamReaderConverter{T}"/> to allow read JSON into <see cref="LockFile"/>
    /// </summary>
    /// <example>
    /// {
    ///     "version": 3,
    ///     "targets": { <see cref="Utf8JsonStreamLockFileTargetConverter"/> },
    ///     "libraries": { <see cref="Utf8JsonStreamLockFileLibraryConverter"/> },
    ///     "projectFileDependencyGroups": { <see cref="Utf8JsonStreamProjectFileDependencyGroupConverter"/> },
    ///     "packageFolders": { <see cref="Utf8JsonStreamLockFileItemConverter{T}"/> },
    ///     "project": { <see cref="JsonPackageSpecReader.GetPackageSpec(ref Utf8JsonStreamReader, string, string, IEnvironmentVariableReader, string)"/> },
    ///     "logs": [ <see cref="Utf8JsonStreamIAssetsLogMessageConverter"/> ]
    /// }
    /// </example>
    internal class Utf8JsonStreamLockFileConverter : IUtf8JsonStreamReaderConverter<LockFile>
    {
        private static readonly byte[] VersionPropertyName = Encoding.UTF8.GetBytes("version");
        private static readonly byte[] LibrariesPropertyName = Encoding.UTF8.GetBytes("libraries");
        private static readonly byte[] TargetsPropertyName = Encoding.UTF8.GetBytes("targets");
        private static readonly byte[] ProjectFileDependencyGroupsPropertyName = Encoding.UTF8.GetBytes("projectFileDependencyGroups");
        private static readonly byte[] PackageFoldersPropertyName = Encoding.UTF8.GetBytes("packageFolders");
        private static readonly byte[] ProjectPropertyName = Encoding.UTF8.GetBytes("project");
        private static readonly byte[] CentralTransitiveDependencyGroupsPropertyName = Encoding.UTF8.GetBytes("centralTransitiveDependencyGroups");
        private static readonly byte[] LogsPropertyName = Encoding.UTF8.GetBytes("logs");

        public LockFile Read(ref Utf8JsonStreamReader reader)
        {
            return Read(ref reader, LockFileReadFlags.All);
        }

        public LockFile Read(ref Utf8JsonStreamReader reader, LockFileReadFlags flags)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected StartObject, found " + reader.TokenType);
            }

            var lockFile = new LockFile();

            while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
            {
                if (reader.ValueTextEquals(VersionPropertyName))
                {
                    reader.Read();
                    if (reader.TryGetInt32(out int version))
                    {
                        lockFile.Version = version;
                    }
                    else
                    {
                        lockFile.Version = int.MinValue;
                    }
                }
                else if (reader.ValueTextEquals(LibrariesPropertyName))
                {
                    reader.Read();

                    if ((flags & LockFileReadFlags.Libraries) == LockFileReadFlags.Libraries)
                    {
                        lockFile.Libraries = reader.ReadObjectAsList<LockFileLibrary>(Utf8JsonReaderExtensions.LockFileLibraryConverter);
                    }
                    else
                    {
                        reader.Skip();
                        lockFile.Libraries = Array.Empty<LockFileLibrary>();
                    }
                }
                else if (reader.ValueTextEquals(TargetsPropertyName))
                {
                    reader.Read();

                    if ((flags & LockFileReadFlags.Targets) == LockFileReadFlags.Targets)
                    {
                        lockFile.Targets = reader.ReadObjectAsList<LockFileTarget>(Utf8JsonReaderExtensions.LockFileTargetConverter);
                    }
                    else
                    {
                        reader.Skip();
                        lockFile.Targets = Array.Empty<LockFileTarget>();
                    }
                }
                else if (reader.ValueTextEquals(ProjectFileDependencyGroupsPropertyName))
                {
                    reader.Read();

                    if ((flags & LockFileReadFlags.ProjectFileDependencyGroups) == LockFileReadFlags.ProjectFileDependencyGroups)
                    {
                        lockFile.ProjectFileDependencyGroups = reader.ReadObjectAsList<ProjectFileDependencyGroup>(Utf8JsonReaderExtensions.ProjectFileDepencencyGroupConverter);
                    }
                    else
                    {
                        reader.Skip();
                        lockFile.ProjectFileDependencyGroups = Array.Empty<ProjectFileDependencyGroup>();
                    }
                }
                else if (reader.ValueTextEquals(PackageFoldersPropertyName))
                {
                    reader.Read();

                    if ((flags & LockFileReadFlags.PackageFolders) == LockFileReadFlags.PackageFolders)
                    {
                        lockFile.PackageFolders = reader.ReadObjectAsList<LockFileItem>(Utf8JsonReaderExtensions.LockFileItemConverter);
                    }
                    else
                    {
                        reader.Skip();
                        lockFile.PackageFolders = Array.Empty<LockFileItem>();
                    }
                }
                else if (reader.ValueTextEquals(ProjectPropertyName))
                {
                    reader.Read();

                    if ((flags & LockFileReadFlags.PackageSpec) == LockFileReadFlags.PackageSpec)
                    {
                        lockFile.PackageSpec = JsonPackageSpecReader.GetPackageSpec(
                            ref reader,
                            name: null,
                            packageSpecPath: null,
                            EnvironmentVariableWrapper.Instance,
                            snapshotValue: null);
                    }
                    else
                    {
                        reader.Skip();
                        lockFile.PackageSpec = new PackageSpec(Array.Empty<TargetFrameworkInformation>());
                    }
                }
                else if (reader.ValueTextEquals(CentralTransitiveDependencyGroupsPropertyName))
                {
                    IList<CentralTransitiveDependencyGroup> results = null;
                    if ((flags & LockFileReadFlags.CentralTransitiveDependencyGroups) == LockFileReadFlags.CentralTransitiveDependencyGroups)
                    {
                        if (reader.Read() && reader.TokenType == JsonTokenType.StartObject)
                        {
                            while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
                            {
                                results ??= new List<CentralTransitiveDependencyGroup>();
                                var frameworkPropertyName = reader.GetString();
                                NuGetFramework framework = NuGetFramework.Parse(frameworkPropertyName);

                                JsonPackageSpecReader.ReadCentralTransitiveDependencyGroup(
                                    jsonReader: ref reader,
                                    results: out var dependencies,
                                    packageSpecPath: string.Empty);
                                results.Add(new CentralTransitiveDependencyGroup(framework, dependencies));
                            }
                        }
                    }
                    else
                    {
                        reader.Skip();
                    }

                    lockFile.CentralTransitiveDependencyGroups = results ?? Array.Empty<CentralTransitiveDependencyGroup>();
                }
                else if (reader.ValueTextEquals(LogsPropertyName))
                {
                    reader.Read();
                    if ((flags & LockFileReadFlags.LogMessages) == LockFileReadFlags.LogMessages)
                    {
                        lockFile.LogMessages = reader.ReadListOfObjects<IAssetsLogMessage>(Utf8JsonReaderExtensions.IAssetsLogMessageConverter);
                    }
                    else
                    {
                        reader.Skip();
                        lockFile.LogMessages = Array.Empty<IAssetsLogMessage>();
                    }
                }
                else
                {
                    reader.Skip();
                }
            }

            var projectPath = lockFile.PackageSpec?.RestoreMetadata?.ProjectPath;
            if (!string.IsNullOrEmpty(projectPath) && lockFile.LogMessages.Count > 0)
            {
                foreach (AssetsLogMessage message in lockFile.LogMessages.Where(x => string.IsNullOrEmpty(x.ProjectPath)))
                {
                    message.ProjectPath = projectPath;
                    message.FilePath = projectPath;
                }
            }

            return lockFile;
        }
    }
}
