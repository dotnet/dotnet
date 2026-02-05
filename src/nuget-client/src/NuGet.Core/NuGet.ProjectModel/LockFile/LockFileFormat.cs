// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.RuntimeModel;

namespace NuGet.ProjectModel
{
    public class LockFileFormat
    {
        public static readonly int Version = 4;

        /// <summary>
        /// The assets file version that supports the aliased format.
        /// This means, the targets section and project section are using the alias as a pivot when there's multiple frameworks instead of the framework name.
        /// </summary>
        public const int AliasedVersion = 4;

        /// <summary>
        /// The assets file version that is used for classic csproj, or legacy project PackageReference.
        /// This is also the assets file version used when the .NET SDK used for the project is not 10.0.300 or newer.
        /// </summary>
        public const int LegacyVersion = 3;

        // If this is ever renamed, you should also rename NoOpRestoreUtilities.NoOpCacheFileName to keep them in sync.
        public static readonly string AssetsFileName = "project.assets.json";

        private const string VersionProperty = "version";
        private const string LibrariesProperty = "libraries";
        private const string TargetsProperty = "targets";
        private const string ProjectFileDependencyGroupsProperty = "projectFileDependencyGroups";
        private const string ServicableProperty = "servicable";
        private const string Sha512Property = "sha512";
        private const string FilesProperty = "files";
        private const string HasToolsProperty = "hasTools";
        private const string DependenciesProperty = "dependencies";
        private const string FrameworkAssembliesProperty = "frameworkAssemblies";
        private const string RuntimeProperty = "runtime";
        private const string CompileProperty = "compile";
        private const string NativeProperty = "native";
        private const string BuildProperty = "build";
        private const string BuildMultiTargetingProperty = "buildMultiTargeting";
        private const string ContentFilesProperty = "contentFiles";
        private const string RuntimeTargetsProperty = "runtimeTargets";
        private const string ResourceProperty = "resource";
        private const string TypeProperty = "type";
        private const string PathProperty = "path";
        private const string MSBuildProjectProperty = "msbuildProject";
        private const string FrameworkProperty = "framework";
        private const string ToolsProperty = "tools";
        private const string PackageFoldersProperty = "packageFolders";
        private const string PackageSpecProperty = "project";
        internal const string LogsProperty = "logs";
        private const string EmbedProperty = "embed";
        private const string FrameworkReferencesProperty = "frameworkReferences";
        private const string CentralTransitiveDependencyGroupsProperty = "centralTransitiveDependencyGroups";

        public LockFile Parse(string lockFileContent, string path)
        {
            return Parse(lockFileContent, NullLogger.Instance, path);
        }

        public LockFile Parse(string lockFileContent, ILogger log, string path)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(lockFileContent);
            using (var stream = new MemoryStream(byteArray))
            {
                return Read(stream, log, path);
            }
        }

        public LockFile Read(string filePath)
        {
            return Read(filePath, NullLogger.Instance);
        }

        public LockFile Read(string filePath, ILogger log)
        {
            using (var stream = File.OpenRead(filePath))
            {
                return Read(stream, log, filePath);
            }
        }

        public LockFile Read(Stream stream, string path)
        {
            return Read(stream, NullLogger.Instance, path);
        }

#pragma warning disable CA1822 // Mark members as static - public API
        public LockFile Read(Stream stream, ILogger log, string path)
#pragma warning restore CA1822 // Mark members as static
        {
            return Read(stream, log, path, flags: LockFileReadFlags.All);
        }

        internal static LockFile Read(Stream stream, ILogger log, string path, LockFileReadFlags flags)
        {
            return Utf8JsonRead(stream, log, path, flags);
        }

        public void Write(string filePath, LockFile lockFile)
        {
            // Create the directory if it does not exist
            var fileInfo = new FileInfo(filePath);
            fileInfo.Directory.Create();

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Write(stream, lockFile);
            }
        }

        public void Write(Stream stream, LockFile lockFile)
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

        public void Write(TextWriter textWriter, LockFile lockFile)
        {
            using (var jsonWriter = new JsonTextWriter(textWriter))
            using (var jsonObjectWriter = new JsonObjectWriter(jsonWriter))
            {
                jsonWriter.Formatting = Formatting.Indented;

                WriteLockFile(jsonWriter, jsonObjectWriter, lockFile);
            }
        }

        public string Render(LockFile lockFile)
        {
            using (var writer = new StringWriter())
            {
                Write(writer, lockFile);
                return writer.ToString();
            }
        }

        private static LockFile Utf8JsonRead(Stream stream, ILogger log, string path, LockFileReadFlags flags)
        {
            try
            {
                var lockFile = JsonUtility.LoadJson(stream, Utf8JsonStreamLockFileConverters.LockFileConverter, flags);
                lockFile.Path = path;
                return lockFile;
            }
            catch (Exception ex)
            {
                log.LogError(string.Format(CultureInfo.CurrentCulture,
                    Strings.Log_ErrorReadingLockFile,
                    path, ex.Message));

                // Ran into parsing errors, mark it as unlocked and out-of-date
                return new LockFile
                {
                    Version = int.MinValue,
                    Path = path
                };
            }
        }

        private static void WriteLockFile(JsonWriter writer, IObjectWriter jsonObjectWriter, LockFile lockFile)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(VersionProperty);
            writer.WriteValue(lockFile.Version);

            writer.WritePropertyName(TargetsProperty);
            JsonUtility.WriteObject(writer, lockFile.Targets, WriteTarget);

            writer.WritePropertyName(LibrariesProperty);
            JsonUtility.WriteObject(writer, lockFile.Libraries, WriteLibrary);

            writer.WritePropertyName(ProjectFileDependencyGroupsProperty);
            JsonUtility.WriteObject(writer, lockFile.ProjectFileDependencyGroups, WriteProjectFileDependencyGroup);

            if (lockFile.PackageFolders?.Any() == true)
            {
                writer.WritePropertyName(PackageFoldersProperty);
                JsonUtility.WriteObject(writer, lockFile.PackageFolders, WriteFileItem);
            }

            if (lockFile.Version >= 2)
            {
                if (lockFile.PackageSpec != null)
                {
                    writer.WritePropertyName(PackageSpecProperty);

                    jsonObjectWriter.WriteObjectStart();

                    PackageSpecWriter.Write(lockFile.PackageSpec, jsonObjectWriter, hashing: false, EnvironmentVariableWrapper.Instance, useLegacyWriter: lockFile.Version <= LegacyVersion);

                    jsonObjectWriter.WriteObjectEnd();
                }
            }

            if (lockFile.Version >= 3)
            {
                if (lockFile.LogMessages.Count > 0)
                {
                    var projectPath = lockFile.PackageSpec?.RestoreMetadata?.ProjectPath;
                    writer.WritePropertyName(LogsProperty);
                    WriteLogMessages(writer, lockFile.LogMessages, projectPath);
                }
            }

            if (lockFile.CentralTransitiveDependencyGroups.Any())
            {
                writer.WritePropertyName(CentralTransitiveDependencyGroupsProperty);
                WriteCentralTransitiveDependencyGroup(jsonObjectWriter, lockFile.CentralTransitiveDependencyGroups);
            }

            writer.WriteEndObject();
        }

        private static void WriteLibrary(JsonWriter writer, LockFileLibrary library)
        {
            writer.WritePropertyName(library.Name + "/" + library.Version.ToNormalizedString());

            writer.WriteStartObject();

            if (library.IsServiceable)
            {
                writer.WritePropertyName(ServicableProperty);
                writer.WriteValue(library.IsServiceable);
            }

            if (library.Sha512 != null)
            {
                writer.WritePropertyName(Sha512Property);
                writer.WriteValue(library.Sha512);
            }

            writer.WritePropertyName(TypeProperty);
            writer.WriteValue(library.Type);

            if (library.Path != null)
            {
                writer.WritePropertyName(PathProperty);
                writer.WriteValue(library.Path);
            }

            if (library.MSBuildProject != null)
            {
                writer.WritePropertyName(MSBuildProjectProperty);
                writer.WriteValue(library.MSBuildProject);
            }

            if (library.HasTools)
            {
                writer.WritePropertyName(HasToolsProperty);
                writer.WriteValue(library.HasTools);
            }

            WritePathArray(writer, FilesProperty, library.Files);

            writer.WriteEndObject();
        }

        private static void WriteTarget(JsonWriter writer, LockFileTarget target)
        {
            var key = target.Name;

            writer.WritePropertyName(key);

            JsonUtility.WriteObject(writer, target.Libraries, WriteTargetLibrary);
        }

        /// <summary>
        /// Writes the <see cref="IAssetsLogMessage"/> object to the <see cref="JsonWriter"/>.
        /// </summary>
        /// <param name="logMessage"><code>IAssetsLogMessage</code> representing the log message.</param>
        private static void WriteLogMessage(JsonWriter writer, IAssetsLogMessage logMessage, string projectPath)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(LogMessageProperties.CODE);
            writer.WriteValue(Enum.GetName(typeof(NuGetLogCode), logMessage.Code));

            writer.WritePropertyName(LogMessageProperties.LEVEL);
            writer.WriteValue(Enum.GetName(typeof(LogLevel), logMessage.Level));

            if (logMessage.Level == LogLevel.Warning)
            {
                writer.WritePropertyName(LogMessageProperties.WARNING_LEVEL);
                writer.WriteValue((int)logMessage.WarningLevel);
            }

            if (logMessage.FilePath != null &&
               (projectPath == null || !PathUtility.GetStringComparerBasedOnOS().Equals(logMessage.FilePath, projectPath)))
            {
                // Do not write the file path if it is the same as the project path.
                // This prevents duplicate information in the lock file.
                writer.WritePropertyName(LogMessageProperties.FILE_PATH);
                writer.WriteValue(logMessage.FilePath);
            }

            if (logMessage.StartLineNumber > 0)
            {
                writer.WritePropertyName(LogMessageProperties.START_LINE_NUMBER);
                writer.WriteValue(logMessage.StartLineNumber);
            }

            if (logMessage.StartColumnNumber > 0)
            {
                writer.WritePropertyName(LogMessageProperties.START_COLUMN_NUMBER);
                writer.WriteValue(logMessage.StartColumnNumber);
            }

            if (logMessage.EndLineNumber > 0)
            {
                writer.WritePropertyName(LogMessageProperties.END_LINE_NUMBER);
                writer.WriteValue(logMessage.EndLineNumber);
            }

            if (logMessage.EndColumnNumber > 0)
            {
                writer.WritePropertyName(LogMessageProperties.END_COLUMN_NUMBER);
                writer.WriteValue(logMessage.EndColumnNumber);
            }

            if (logMessage.Message != null)
            {
                writer.WritePropertyName(LogMessageProperties.MESSAGE);
                writer.WriteValue(logMessage.Message);
            }

            if (logMessage.LibraryId != null)
            {
                writer.WritePropertyName(LogMessageProperties.LIBRARY_ID);
                writer.WriteValue(logMessage.LibraryId);
            }

            if (logMessage.TargetGraphs != null &&
                logMessage.TargetGraphs.Any() &&
                logMessage.TargetGraphs.All(l => !string.IsNullOrEmpty(l)))
            {
                writer.WritePropertyName(LogMessageProperties.TARGET_GRAPHS);
                WriteArray(writer, logMessage.TargetGraphs);
            }

            writer.WriteEndObject();
        }

        internal static void WriteLogMessages(JsonWriter writer, IEnumerable<IAssetsLogMessage> logMessages, string projectPath)
        {
            writer.WriteStartArray();

            foreach (var logMessage in logMessages)
            {
                WriteLogMessage(writer, logMessage, projectPath);
            }

            writer.WriteEndArray();
        }

        private static void WriteTargetLibrary(JsonWriter writer, LockFileTargetLibrary library)
        {
            writer.WritePropertyName(library.Name + "/" + library.Version.ToNormalizedString());

            writer.WriteStartObject();

            if (library.Type != null)
            {
                writer.WritePropertyName(TypeProperty);
                writer.WriteValue(library.Type);
            }

            if (library.Framework != null)
            {
                writer.WritePropertyName(FrameworkProperty);
                writer.WriteValue(library.Framework);
            }

            if (library.Dependencies.Count > 0)
            {
                var ordered = library.Dependencies.OrderBy(dependency => dependency.Id, StringComparer.Ordinal);

                writer.WritePropertyName(DependenciesProperty);
                JsonUtility.WriteObject(writer, ordered, JsonUtility.WritePackageDependencyWithLegacyString);
            }

            if (library.FrameworkAssemblies.Count > 0)
            {
                var ordered = library.FrameworkAssemblies.OrderBy(assembly => assembly, StringComparer.Ordinal);

                writer.WritePropertyName(FrameworkAssembliesProperty);
                WriteArray(writer, ordered);
            }

            if (library.CompileTimeAssemblies.Count > 0)
            {
                var ordered = library.CompileTimeAssemblies.OrderBy(assembly => assembly.Path, StringComparer.Ordinal);

                writer.WritePropertyName(CompileProperty);
                JsonUtility.WriteObject(writer, ordered, WriteFileItem);
            }

            if (library.RuntimeAssemblies.Count > 0)
            {
                var ordered = library.RuntimeAssemblies.OrderBy(assembly => assembly.Path, StringComparer.Ordinal);

                writer.WritePropertyName(RuntimeProperty);
                JsonUtility.WriteObject(writer, ordered, WriteFileItem);
            }

            if (library.FrameworkReferences.Count > 0)
            {
                var ordered = library.FrameworkReferences.OrderBy(reference => reference, StringComparer.Ordinal);

                writer.WritePropertyName(FrameworkReferencesProperty);
                WriteArray(writer, ordered);
            }

            if (library.ResourceAssemblies.Count > 0)
            {
                var ordered = library.ResourceAssemblies.OrderBy(assembly => assembly.Path, StringComparer.Ordinal);

                writer.WritePropertyName(ResourceProperty);
                JsonUtility.WriteObject(writer, ordered, WriteFileItem);
            }

            if (library.NativeLibraries.Count > 0)
            {
                var ordered = library.NativeLibraries.OrderBy(assembly => assembly.Path, StringComparer.Ordinal);

                writer.WritePropertyName(NativeProperty);
                JsonUtility.WriteObject(writer, ordered, WriteFileItem);
            }

            if (library.ContentFiles.Count > 0)
            {
                var ordered = library.ContentFiles.OrderBy(assembly => assembly.Path, StringComparer.Ordinal);

                writer.WritePropertyName(ContentFilesProperty);
                JsonUtility.WriteObject(writer, ordered, WriteFileItem);
            }

            if (library.Build.Count > 0)
            {
                var ordered = library.Build.OrderBy(assembly => assembly.Path, StringComparer.Ordinal);

                writer.WritePropertyName(BuildProperty);
                JsonUtility.WriteObject(writer, ordered, WriteFileItem);
            }

            if (library.BuildMultiTargeting.Count > 0)
            {
                var ordered = library.BuildMultiTargeting.OrderBy(assembly => assembly.Path, StringComparer.Ordinal);

                writer.WritePropertyName(BuildMultiTargetingProperty);
                JsonUtility.WriteObject(writer, ordered, WriteFileItem);
            }

            if (library.RuntimeTargets.Count > 0)
            {
                var ordered = library.RuntimeTargets.OrderBy(assembly => assembly.Path, StringComparer.Ordinal);

                writer.WritePropertyName(RuntimeTargetsProperty);
                JsonUtility.WriteObject(writer, ordered, WriteFileItem);
            }

            if (library.ToolsAssemblies.Count > 0)
            {
                var ordered = library.ToolsAssemblies.OrderBy(assembly => assembly.Path, StringComparer.Ordinal);

                writer.WritePropertyName(ToolsProperty);
                JsonUtility.WriteObject(writer, ordered, WriteFileItem);
            }

            if (library.EmbedAssemblies.Count > 0)
            {
                var ordered = library.EmbedAssemblies.OrderBy(assembly => assembly.Path, StringComparer.Ordinal);

                writer.WritePropertyName(EmbedProperty);
                JsonUtility.WriteObject(writer, ordered, WriteFileItem);
            }

            writer.WriteEndObject();
        }

        private static void WriteProjectFileDependencyGroup(JsonWriter writer, ProjectFileDependencyGroup frameworkInfo)
        {
            writer.WritePropertyName(frameworkInfo.FrameworkName);
            WriteArray(writer, frameworkInfo.Dependencies);
        }

        private static void WriteFileItem(JsonWriter writer, LockFileItem item)
        {
            writer.WritePropertyName(item.Path);

            writer.WriteStartObject();

            foreach (var property in item.Properties.OrderBy(x => x.Key, StringComparer.Ordinal))
            {
                writer.WritePropertyName(property.Key);

                if (bool.TrueString.Equals(property.Value, StringComparison.OrdinalIgnoreCase))
                {
                    writer.WriteValue(true);
                }
                else if (bool.FalseString.Equals(property.Value, StringComparison.OrdinalIgnoreCase))
                {
                    writer.WriteValue(false);
                }
                else
                {
                    writer.WriteValue(property.Value);
                }
            }

            writer.WriteEndObject();
        }

        private static void WritePathArray(JsonWriter writer, string property, IEnumerable<string> items)
        {
            using var itemsEnumerator = items.NoAllocEnumerate().GetEnumerator();
            if (itemsEnumerator.MoveNext())
            {
                var orderedItems = items
                    .Select(f => GetPathWithForwardSlashes(f))
                    .OrderBy(f => f, StringComparer.Ordinal);

                writer.WritePropertyName(property);
                WriteArray(writer, orderedItems);
            }
        }

        internal static void WriteArray(JsonWriter writer, IEnumerable<string> values)
        {
            writer.WriteStartArray();
            foreach (var value in values)
            {
                writer.WriteValue(value);
            }
            writer.WriteEndArray();
        }

        private static string GetPathWithForwardSlashes(string path)
        {
            return path.Replace('\\', '/');
        }

        private static void WriteCentralTransitiveDependencyGroup(IObjectWriter writer, IList<CentralTransitiveDependencyGroup> centralTransitiveDependencyGroups)
        {
            writer.WriteObjectStart();

            foreach (var centralTransitiveDepGroup in centralTransitiveDependencyGroups.OrderBy(ptdg => ptdg.FrameworkName))
            {
                PackageSpecWriter.SetCentralTransitveDependencyGroup(writer, centralTransitiveDepGroup.FrameworkName, centralTransitiveDepGroup.TransitiveDependencies);
            }

            writer.WriteObjectEnd();
        }
    }
}
