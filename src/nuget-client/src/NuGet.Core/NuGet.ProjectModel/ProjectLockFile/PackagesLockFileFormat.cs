// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Shared;
using NuGet.Versioning;
using JsonException = System.Text.Json.JsonException;

namespace NuGet.ProjectModel
{
    public static class PackagesLockFileFormat
    {
        public static readonly int Version = 1;
        internal static readonly int AliasedVersion = 3;

        /// <summary>
        /// The highest format version that this implementation supports.
        /// Version 3 preserves compatibility with older clients that do not support central package versions.
        /// </summary>
        public static readonly int PackagesLockFileVersion = AliasedVersion;

        public static readonly string LockFileName = "packages.lock.json";

        private const string VersionProperty = "version";
        private const string ResolvedProperty = "resolved";
        private const string RequestedProperty = "requested";
        private const string ContentHashProperty = "contentHash";
        private const string DependenciesProperty = "dependencies";
        private const string TypeProperty = "type";
        private const string FrameworkProperty = "framework";

        private static ReadOnlySpan<byte> VersionPropertyUtf8 => "version"u8;
        private static ReadOnlySpan<byte> ResolvedPropertyUtf8 => "resolved"u8;
        private static ReadOnlySpan<byte> RequestedPropertyUtf8 => "requested"u8;
        private static ReadOnlySpan<byte> ContentHashPropertyUtf8 => "contentHash"u8;
        private static ReadOnlySpan<byte> DependenciesPropertyUtf8 => "dependencies"u8;
        private static ReadOnlySpan<byte> TypePropertyUtf8 => "type"u8;
        private static ReadOnlySpan<byte> FrameworkPropertyUtf8 => "framework"u8;

        /// <summary>
        /// Copies the complete string to a UTF-8 buffer before parsing it.
        /// Use the stream overload to avoid a second whole-file allocation.
        /// </summary>
        /// <param name="lockFileContent">The complete package lock file content.</param>
        /// <param name="path">The path to use in diagnostics.</param>
        /// <returns>The parsed package lock file.</returns>
        [Obsolete("Use Read(Stream, ILogger, string) instead to avoid keeping the entire lock file in memory.")]
        public static PackagesLockFile Parse(string lockFileContent, string path)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(lockFileContent), writable: false))
            {
                return Read(stream, NullLogger.Instance, path);
            }
        }

        /// <summary>
        /// Copies the complete string to a UTF-8 buffer before parsing it.
        /// Use the stream overload to avoid a second whole-file allocation.
        /// </summary>
        /// <param name="lockFileContent">The complete package lock file content.</param>
        /// <param name="log">The logger for read diagnostics.</param>
        /// <param name="path">The path to use in diagnostics.</param>
        /// <returns>The parsed package lock file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="log"/> is <see langword="null"/>.</exception>
        [Obsolete("Use Read(Stream, ILogger, string) instead to avoid keeping the entire lock file in memory.")]
        public static PackagesLockFile Parse(string lockFileContent, ILogger log, string path)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(lockFileContent), writable: false))
            {
                return Read(stream, log, path);
            }
        }

        public static PackagesLockFile Read(string filePath)
        {
            return Read(filePath, NullLogger.Instance);
        }

        public static PackagesLockFile Read(string filePath, ILogger log)
        {
            using (var stream = File.OpenRead(filePath))
            {
                return Read(stream, log, filePath);
            }
        }

        /// <summary>
        /// Reads a UTF-8 package lock file and logs parsing failures.
        /// A parsing failure returns a lock file with <see cref="PackagesLockFile.Version"/> set to <see cref="int.MinValue"/>.
        /// </summary>
        /// <param name="stream">
        /// The UTF-8 stream to read. This method takes ownership of the stream and disposes it before the method returns.
        /// </param>
        /// <param name="log">The logger for read diagnostics.</param>
        /// <param name="path">The path to use in diagnostics.</param>
        /// <returns>The parsed package lock file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="log"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// The method does not take ownership of <paramref name="stream"/> when argument validation fails before reading starts.
        /// </remarks>
        public static PackagesLockFile Read(Stream stream, ILogger log, string path)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(log);
#else
            if (log == null) throw new ArgumentNullException(nameof(log));
#endif

            try
            {
                PackagesLockFile lockFile = ReadLockFile(stream);
#pragma warning disable CS0618 // Preserve the behavior of the obsolete PackagesLockFile.Path property.
                lockFile.Path = path;
#pragma warning restore CS0618
                return lockFile;
            }
            catch (Exception ex)
            {
                log.LogInformation(string.Format(CultureInfo.CurrentCulture,
                    Strings.Log_ErrorReadingLockFile,
                    path, ex.Message));

                // Ran into parsing errors, mark it as unlocked and out-of-date
                return new PackagesLockFile
                {
                    Version = int.MinValue,
#pragma warning disable CS0618 // Preserve the behavior of the obsolete PackagesLockFile.Path property.
                    Path = path
#pragma warning restore CS0618
                };
            }
        }

        /// <summary>
        /// Uses the legacy Newtonsoft.Json reader and logs parsing failures.
        /// A parsing failure returns a lock file with <see cref="PackagesLockFile.Version"/> set to <see cref="int.MinValue"/>.
        /// </summary>
        /// <param name="reader">
        /// The text reader to read. This method takes ownership of the reader and disposes it before the method returns.
        /// </param>
        /// <param name="log">The logger for read diagnostics.</param>
        /// <param name="path">The path to use in diagnostics.</param>
        /// <returns>The parsed package lock file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="log"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// The method does not take ownership of <paramref name="reader"/> when argument validation fails before reading starts.
        /// </remarks>
        [Obsolete("Use Read(Stream, ILogger, string) instead.")]
        public static PackagesLockFile Read(TextReader reader, ILogger log, string path)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(log);
#else
            if (log == null) throw new ArgumentNullException(nameof(log));
#endif

            try
            {
                using (reader)
                {
                    JObject json = JsonUtility.LoadJson(reader);
                    PackagesLockFile lockFile = ReadLockFileWithNewtonsoftJson(json);
#pragma warning disable CS0618 // Preserve the behavior of the obsolete PackagesLockFile.Path property.
                    lockFile.Path = path;
#pragma warning restore CS0618
                    return lockFile;
                }
            }
            catch (Exception ex)
            {
                log.LogInformation(string.Format(CultureInfo.CurrentCulture,
                    Strings.Log_ErrorReadingLockFile,
                    path, ex.Message));

                return new PackagesLockFile
                {
                    Version = int.MinValue,
#pragma warning disable CS0618 // Preserve the behavior of the obsolete PackagesLockFile.Path property.
                    Path = path
#pragma warning restore CS0618
                };
            }
        }

        private static PackagesLockFile ReadLockFileWithNewtonsoftJson(JObject cursor)
        {
            int version = JsonUtility.ReadInt(cursor, VersionProperty, defaultValue: int.MinValue);
            IList<PackagesLockFileTarget> targets;

            if (version >= AliasedVersion)
            {
                // V3 format: read from root level (alias/rid keys with framework and dependencies inside)
                targets = new List<PackagesLockFileTarget>();
                foreach (var property in cursor.Properties())
                {
                    if (property.Name != VersionProperty)
                    {
                        var target = ReadTargetV3(property.Name, property.Value);
                        if (target != null)
                        {
                            targets.Add(target);
                        }
                    }
                }
            }
            else
            {
                // V1 and V2 format: read from dependencies property
                targets = JsonUtility.ReadObject(cursor[DependenciesProperty] as JObject, ReadDependencyV2);
            }

            var lockFile = new PackagesLockFile()
            {
                Version = version,
                Targets = targets,
            };

            return lockFile;
        }

        internal static PackagesLockFile ReadLockFile(Stream stream)
        {
            using (stream)
            {
                var reader = new Utf8JsonStreamReader(stream);
                try
                {
                    return ReadLockFile(ref reader);
                }
                finally
                {
                    reader.Dispose();
                }
            }
        }

        public static string Render(PackagesLockFile lockFile)
        {
            using (var writer = new StringWriter())
            {
                Write(writer, lockFile);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Creates the parent directory and replaces an existing destination file.
        /// </summary>
        /// <param name="filePath">The destination file path.</param>
        /// <param name="lockFile">The package lock file to write.</param>
        public static void Write(string filePath, PackagesLockFile lockFile)
        {
            // Create the directory if it does not exist
            var fileInfo = new FileInfo(filePath);
            fileInfo.Directory.Create();

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Write(stream, lockFile);
            }
        }

        /// <summary>
        /// Writes a package lock file and disposes the destination stream before returning.
        /// </summary>
        /// <param name="stream">The destination stream. This method takes ownership of the stream.</param>
        /// <param name="lockFile">The package lock file to write.</param>
        public static void Write(Stream stream, PackagesLockFile lockFile)
        {
#if NET5_0_OR_GREATER
            using (var textWriter = new StreamWriter(stream))
#else
            using (var textWriter = new NoAllocNewLineStreamWriter(stream))
#endif
            {
                Write(textWriter, lockFile);
            }
        }

        /// <summary>
        /// Writes a package lock file without disposing the destination text writer.
        /// </summary>
        /// <param name="textWriter">The destination text writer.</param>
        /// <param name="lockFile">The package lock file to write.</param>
#if NET5_0_OR_GREATER
        [UnconditionalSuppressMessage("AOT", "IL2026", Justification = "WriteTo without converters is safe. See https://github.com/JamesNK/Newtonsoft.Json/blob/13.0.4/Src/Newtonsoft.Json/Linq/JToken.cs")]
        [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "WriteTo without converters is safe. See https://github.com/JamesNK/Newtonsoft.Json/blob/13.0.4/Src/Newtonsoft.Json/Linq/JToken.cs")]
#endif
        public static void Write(TextWriter textWriter, PackagesLockFile lockFile)
        {
            using (var jsonWriter = new JsonTextWriter(textWriter))
            {
                jsonWriter.Formatting = Formatting.Indented;

                var json = WriteLockFile(lockFile);
                json.WriteTo(jsonWriter, Array.Empty<JsonConverter>());
            }
        }

        private static JObject WriteLockFile(PackagesLockFile lockFile)
        {
            var json = new JObject
            {
                [VersionProperty] = new JValue(lockFile.Version)
            };

            if (lockFile.Version >= AliasedVersion)
            {
                // V3 format: write targets at root level with framework and dependencies inside
                foreach (var target in lockFile.Targets)
                {
                    var targetProperty = WriteTargetV3(target);
                    json.Add(targetProperty);
                }
            }
            else
            {
                // V1 and V2 format: write targets under dependencies property
                json[DependenciesProperty] = JsonUtility.WriteObject(lockFile.Targets, WriteTarget);
            }

            return json;
        }

        private static PackagesLockFileTarget ReadDependencyV2(string property, JToken json)
        {
            var parts = property.Split(JsonUtility.PathSplitChars);

            var target = new PackagesLockFileTarget
            {
                TargetFramework = NuGetFramework.Parse(parts[0]),
                Dependencies = JsonUtility.ReadObject(json as JObject, ReadTargetDependency)
            };

            if (parts.Length == 2)
            {
                target.RuntimeIdentifier = parts[1];
            }

            return target;
        }

        private static PackagesLockFileTarget ReadTargetV3(string property, JToken json)
        {
            var jObject = json as JObject;
            if (jObject == null)
            {
                return null;
            }

            var frameworkString = JsonUtility.ReadProperty<string>(jObject, FrameworkProperty);
            if (string.IsNullOrEmpty(frameworkString))
            {
                return null;
            }

            var parts = property.Split(JsonUtility.PathSplitChars);

            var target = new PackagesLockFileTarget
            {
                TargetFramework = NuGetFramework.Parse(frameworkString),
                RuntimeIdentifier = parts.Length == 2 ? parts[1] : null,
                TargetAlias = parts[0],
                Dependencies = JsonUtility.ReadObject(jObject[DependenciesProperty] as JObject, ReadTargetDependency)
            };

            return target;
        }

        private static LockFileDependency ReadTargetDependency(string property, JToken json)
        {
            var dependency = new LockFileDependency
            {
                Id = property,
                Dependencies = JsonUtility.ReadObject(json[DependenciesProperty] as JObject, JsonUtility.ReadPackageDependency)
            };

            var jObject = json as JObject;

            var typeString = JsonUtility.ReadProperty<string>(jObject, TypeProperty);

            if (!string.IsNullOrEmpty(typeString)
                && Enum.TryParse<PackageDependencyType>(typeString, ignoreCase: true, result: out var installationType))
            {
                dependency.Type = installationType;
            }

            var resolvedString = JsonUtility.ReadProperty<string>(jObject, ResolvedProperty);

            if (!string.IsNullOrEmpty(resolvedString))
            {
                dependency.ResolvedVersion = NuGetVersion.Parse(resolvedString);
            }

            var requestedString = JsonUtility.ReadProperty<string>(jObject, RequestedProperty);

            if (!string.IsNullOrEmpty(requestedString))
            {
                dependency.RequestedVersion = VersionRange.Parse(requestedString);
            }

            dependency.ContentHash = JsonUtility.ReadProperty<string>(jObject, ContentHashProperty);

            return dependency;
        }

        private static JProperty WriteTargetV3(PackagesLockFileTarget target)
        {
            var key = target.Name;

            var json = new JObject
            {
                [FrameworkProperty] = target.TargetFramework.ToString(),
                [DependenciesProperty] = JsonUtility.WriteObject(target.Dependencies, WriteTargetDependency)
            };

            return new JProperty(key, json);
        }

        private static JProperty WriteTarget(PackagesLockFileTarget target)
        {
            var json = JsonUtility.WriteObject(target.Dependencies, WriteTargetDependency);

            var key = target.Name;

            return new JProperty(key, json);
        }


        private static JProperty WriteTargetDependency(LockFileDependency dependency)
        {
            var json = new JObject
            {
                [TypeProperty] = dependency.Type.ToString()
            };

            if (dependency.RequestedVersion != null)
            {
                json[RequestedProperty] = dependency.RequestedVersion.ToNormalizedString();
            }

            if (dependency.ResolvedVersion != null)
            {
                json[ResolvedProperty] = dependency.ResolvedVersion.ToNormalizedString();
            }

            if (dependency.ContentHash != null)
            {
                json[ContentHashProperty] = dependency.ContentHash;
            }

            if (dependency.Dependencies?.Count > 0)
            {
                var ordered = dependency.Dependencies.OrderBy(dep => dep.Id, StringComparer.Ordinal);

                json[DependenciesProperty] = JsonUtility.WriteObject(ordered, dependency.Type == PackageDependencyType.Project ?
                    JsonUtility.WritePackageDependency : JsonUtility.WritePackageDependencyWithLegacyString);
            }

            return new JProperty(dependency.Id, json);
        }

#nullable enable

        private static PackagesLockFile ReadLockFile(ref Utf8JsonStreamReader reader)
        {
            if (reader.TokenType == JsonTokenType.None)
            {
                reader.Read();
            }

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            if (!reader.Read()
                || reader.TokenType != JsonTokenType.PropertyName
                || !reader.ValueTextEquals(VersionPropertyUtf8))
            {
                throw new JsonException();
            }

            ReadValue(ref reader);
            int version = ReadVersion(ref reader);
            IList<PackagesLockFileTarget> targets = version >= AliasedVersion
                ? ReadVersion3Targets(ref reader)
                : ReadVersion1Targets(ref reader);

            if (reader.TokenType != JsonTokenType.EndObject || reader.Read())
            {
                throw new JsonException();
            }

            return new PackagesLockFile
            {
                Version = version,
                Targets = targets
            };
        }

        private static IList<PackagesLockFileTarget> ReadVersion1Targets(
            ref Utf8JsonStreamReader reader)
        {
            IList<PackagesLockFileTarget>? targets = null;

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                EnsurePropertyName(ref reader);
                if (reader.ValueTextEquals(DependenciesPropertyUtf8))
                {
                    ReadValue(ref reader);
                    targets = ReadVersion1TargetObject(ref reader);
                }
                else
                {
                    ReadValue(ref reader);
                    reader.Skip();
                }
            }

            if (reader.TokenType != JsonTokenType.EndObject || targets is null)
            {
                throw new JsonException();
            }

            return targets;
        }

        private static IList<PackagesLockFileTarget> ReadVersion1TargetObject(
            ref Utf8JsonStreamReader reader)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var targets = new List<PackagesLockFileTarget>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                string targetName = ReadPropertyName(ref reader);
                ReadValue(ref reader);
                targets.Add(ReadVersion1Target(ref reader, targetName));
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }

            return targets;
        }

        private static PackagesLockFileTarget ReadVersion1Target(
            ref Utf8JsonStreamReader reader,
            string targetName)
        {
            IList<LockFileDependency> dependencies = ReadLockFileDependencies(ref reader);
            SplitTargetName(targetName.AsSpan(), out int frameworkLength, out int runtimeIdentifierIndex);
            string framework = frameworkLength == targetName.Length
                ? targetName
                : targetName.Substring(0, frameworkLength);
            string? runtimeIdentifier = runtimeIdentifierIndex < 0
                ? null
                : targetName.Substring(runtimeIdentifierIndex);

            return new PackagesLockFileTarget
            {
                TargetFramework = NuGetFramework.Parse(framework),
                RuntimeIdentifier = runtimeIdentifier,
                Dependencies = dependencies
            };
        }

        private static IList<PackagesLockFileTarget> ReadVersion3Targets(
            ref Utf8JsonStreamReader reader)
        {
            var targets = new List<PackagesLockFileTarget>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                string targetName = ReadPropertyName(ref reader);
                ReadValue(ref reader);
                targets.Add(ReadVersion3Target(ref reader, targetName));
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }

            return targets;
        }

        private static PackagesLockFileTarget ReadVersion3Target(
            ref Utf8JsonStreamReader reader,
            string targetName)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            string? framework = null;
            IList<LockFileDependency>? dependencies = null;

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                EnsurePropertyName(ref reader);
                if (reader.ValueTextEquals(FrameworkPropertyUtf8))
                {
                    ReadValue(ref reader);
                    framework = ReadRequiredString(ref reader);
                }
                else if (reader.ValueTextEquals(DependenciesPropertyUtf8))
                {
                    ReadValue(ref reader);
                    dependencies = ReadLockFileDependencies(ref reader);
                }
                else
                {
                    ReadValue(ref reader);
                    reader.Skip();
                }
            }

            if (reader.TokenType != JsonTokenType.EndObject
                || framework is null
                || framework.Length == 0
                || dependencies is null)
            {
                throw new JsonException();
            }

            SplitTargetName(targetName.AsSpan(), out int targetAliasLength, out int runtimeIdentifierIndex);
            string targetAlias = targetAliasLength == targetName.Length
                ? targetName
                : targetName.Substring(0, targetAliasLength);
            string? runtimeIdentifier = runtimeIdentifierIndex < 0
                ? null
                : targetName.Substring(runtimeIdentifierIndex);

            return new PackagesLockFileTarget
            {
                TargetFramework = NuGetFramework.Parse(framework),
                RuntimeIdentifier = runtimeIdentifier,
                TargetAlias = targetAlias,
                Dependencies = dependencies
            };
        }

        private static void SplitTargetName(
            ReadOnlySpan<char> targetName,
            out int frameworkLength,
            out int runtimeIdentifierIndex)
        {
            int separatorIndex = targetName.IndexOf('/');
            if (separatorIndex < 0)
            {
                frameworkLength = targetName.Length;
                runtimeIdentifierIndex = -1;
                return;
            }

            frameworkLength = separatorIndex;
            int candidateRuntimeIdentifierIndex = separatorIndex + 1;
            runtimeIdentifierIndex = targetName.Slice(candidateRuntimeIdentifierIndex).IndexOf('/') < 0
                ? candidateRuntimeIdentifierIndex
                : -1;
        }

        private static IList<LockFileDependency> ReadLockFileDependencies(
            ref Utf8JsonStreamReader reader)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var dependencies = new List<LockFileDependency>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                string packageId = ReadPropertyName(ref reader);
                ReadValue(ref reader);
                dependencies.Add(ReadLockFileDependency(ref reader, packageId));
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }

            return dependencies;
        }

        private static LockFileDependency ReadLockFileDependency(
            ref Utf8JsonStreamReader reader,
            string packageId)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            string? type = null;
            string? resolved = null;
            string? requested = null;
            string? contentHash = null;
            IList<PackageDependency> dependencies = Array.Empty<PackageDependency>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                EnsurePropertyName(ref reader);
                if (reader.ValueTextEquals(TypePropertyUtf8))
                {
                    ReadValue(ref reader);
                    type = ReadStringOrNull(ref reader);
                }
                else if (reader.ValueTextEquals(ResolvedPropertyUtf8))
                {
                    ReadValue(ref reader);
                    resolved = ReadStringOrNull(ref reader);
                }
                else if (reader.ValueTextEquals(RequestedPropertyUtf8))
                {
                    ReadValue(ref reader);
                    requested = ReadStringOrNull(ref reader);
                }
                else if (reader.ValueTextEquals(ContentHashPropertyUtf8))
                {
                    ReadValue(ref reader);
                    contentHash = ReadStringOrNull(ref reader);
                }
                else if (reader.ValueTextEquals(DependenciesPropertyUtf8))
                {
                    ReadValue(ref reader);
                    dependencies = ReadPackageDependencies(ref reader);
                }
                else
                {
                    ReadValue(ref reader);
                    reader.Skip();
                }
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }

            var dependency = new LockFileDependency
            {
                Id = packageId,
                ContentHash = contentHash,
                Dependencies = dependencies
            };

            if (!string.IsNullOrEmpty(type)
                && Enum.TryParse(type, ignoreCase: true, out PackageDependencyType dependencyType))
            {
                dependency.Type = dependencyType;
            }

            if (!string.IsNullOrEmpty(resolved))
            {
                if (!NuGetVersion.TryParse(resolved, out NuGetVersion? resolvedVersion))
                {
                    throw new JsonException();
                }

                dependency.ResolvedVersion = resolvedVersion;
            }

            if (requested is not null && requested.Length > 0)
            {
                if (!VersionRange.TryParse(requested, out VersionRange? requestedVersion))
                {
                    throw new JsonException();
                }

                dependency.RequestedVersion = requestedVersion;
            }

            return dependency;
        }

        private static IList<PackageDependency> ReadPackageDependencies(
            ref Utf8JsonStreamReader reader)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var dependencies = new List<PackageDependency>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                string packageId = ReadPropertyName(ref reader);
                ReadValue(ref reader);
                string? versionRangeValue = ReadStringOrNull(ref reader);
                VersionRange? versionRange = null;
                if (versionRangeValue is not null
                    && !VersionRange.TryParse(versionRangeValue, out versionRange))
                {
                    throw new JsonException();
                }

                dependencies.Add(new PackageDependency(packageId, versionRange));
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }

            return dependencies;
        }

        private static int ReadVersion(ref Utf8JsonStreamReader reader)
        {
            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int version))
            {
                return version;
            }

            throw new JsonException();
        }

        private static string ReadRequiredString(ref Utf8JsonStreamReader reader)
        {
            return reader.TokenType == JsonTokenType.String
                ? reader.GetString() ?? throw new JsonException()
                : throw new JsonException();
        }

        private static string? ReadStringOrNull(ref Utf8JsonStreamReader reader)
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString(),
                JsonTokenType.Null => null,
                _ => throw new JsonException(),
            };
        }

        private static string ReadPropertyName(ref Utf8JsonStreamReader reader)
        {
            EnsurePropertyName(ref reader);
            return reader.GetString() ?? throw new JsonException();
        }

        private static void EnsurePropertyName(ref Utf8JsonStreamReader reader)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }
        }

        private static void ReadValue(ref Utf8JsonStreamReader reader)
        {
            if (!reader.Read())
            {
                throw new JsonException();
            }
        }
    }
}
