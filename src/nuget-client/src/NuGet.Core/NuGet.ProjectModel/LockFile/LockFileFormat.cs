// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.RuntimeModel;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    public class LockFileFormat
    {
        public static readonly int Version = 3;
        public static readonly string LockFileName = "project.lock.json";
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

        public LockFile Read(Stream stream, ILogger log, string path)
        {
            return Read(stream, log, path, EnvironmentVariableWrapper.Instance);
        }

        internal LockFile Read(Stream stream, ILogger log, string path, IEnvironmentVariableReader environmentVariableReader, bool bypassCache = false)
        {
            if (!JsonUtility.UseNewtonsoftJsonForParsing(environmentVariableReader, bypassCache))
            {
                return Utf8JsonRead(stream, log, path);
            }
            else
            {
                using (var reader = new StreamReader(stream))
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    return Read(reader, log, path);
#pragma warning restore CS0618 // Type or member is obsolete
                }
            }
        }

        [Obsolete("This method is deprecated. Use Read(Stream, string) instead.")]
        public LockFile Read(TextReader reader, string path)
        {
            return Read(reader, NullLogger.Instance, path);
        }

        [Obsolete("This method is deprecated. Use Read(Stream, ILogger, string) instead.")]
        public LockFile Read(TextReader reader, ILogger log, string path)
        {
            try
            {
                var json = JsonUtility.LoadJson(reader);
                var lockFile = ReadLockFile(json, path);
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

        private LockFile Utf8JsonRead(Stream stream, ILogger log, string path)
        {
            try
            {
                var lockFile = JsonUtility.LoadJson<LockFile>(stream, Utf8JsonReaderExtensions.LockFileConverter);
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

        [Obsolete]
        private static LockFile ReadLockFile(JObject cursor, string path)
        {
            var lockFile = new LockFile()
            {
                Version = JsonUtility.ReadInt(cursor, VersionProperty, defaultValue: int.MinValue),
                Libraries = JsonUtility.ReadObject(cursor[LibrariesProperty] as JObject, ReadLibrary),
                Targets = JsonUtility.ReadObject(cursor[TargetsProperty] as JObject, ReadTarget),
                ProjectFileDependencyGroups = JsonUtility.ReadObject(cursor[ProjectFileDependencyGroupsProperty] as JObject, ReadProjectFileDependencyGroup),
                PackageFolders = JsonUtility.ReadObject(cursor[PackageFoldersProperty] as JObject, ReadFileItem),
                PackageSpec = ReadPackageSpec(cursor[PackageSpecProperty] as JObject),
                CentralTransitiveDependencyGroups = ReadProjectFileTransitiveDependencyGroup(cursor[CentralTransitiveDependencyGroupsProperty] as JObject, path)
            };

            lockFile.LogMessages = ReadLogMessageArray(cursor[LogsProperty] as JArray,
                lockFile?.PackageSpec?.RestoreMetadata?.ProjectPath);

            return lockFile;
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

                    PackageSpecWriter.Write(lockFile.PackageSpec, jsonObjectWriter);

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

        private static LockFileLibrary ReadLibrary(string property, JToken json)
        {
            var library = new LockFileLibrary();
            var parts = property.Split(new[] { '/' }, 2);
            library.Name = parts[0];
            if (parts.Length == 2)
            {
                library.Version = NuGetVersion.Parse(parts[1]);
            }

            library.Type = ReadString(json[TypeProperty]);

            var jObject = json as JObject;

            library.Path = JsonUtility.ReadProperty<string>(jObject, PathProperty);
            library.MSBuildProject = JsonUtility.ReadProperty<string>(jObject, MSBuildProjectProperty);
            library.Sha512 = JsonUtility.ReadProperty<string>(jObject, Sha512Property);

            library.IsServiceable = ReadBool(json, ServicableProperty, defaultValue: false);
            library.Files = ReadPathArray(json[FilesProperty] as JArray);

            library.HasTools = ReadBool(json, HasToolsProperty, defaultValue: false);
            return library;
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

        private static LockFileTarget ReadTarget(string property, JToken json)
        {
            var target = new LockFileTarget();
            var parts = property.Split(JsonUtility.PathSplitChars, 2);
            target.TargetFramework = NuGetFramework.Parse(parts[0]);
            if (parts.Length == 2)
            {
                target.RuntimeIdentifier = parts[1];
            }

            target.Libraries = JsonUtility.ReadObject(json as JObject, ReadTargetLibrary);

            return target;
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

        /// <summary>
        /// Converts an <code>JObject</code> into an <code>IAssetsLogMessage</code>.
        /// </summary>
        /// <param name="json"><code>JObject</code> containg the json representation of the log message.</param>
        /// <returns><code>IAssetsLogMessage</code> representing the log message.</returns>
        private static IAssetsLogMessage ReadLogMessage(JObject json, string projectPath)
        {
            AssetsLogMessage assetsLogMessage = null;

            if (json != null)
            {

                var levelJson = json[LogMessageProperties.LEVEL];
                var codeJson = json[LogMessageProperties.CODE];
                var warningLevelJson = json[LogMessageProperties.WARNING_LEVEL];
                var filePathJson = json[LogMessageProperties.FILE_PATH];
                var startLineNumberJson = json[LogMessageProperties.START_LINE_NUMBER];
                var startColumnNumberJson = json[LogMessageProperties.START_COLUMN_NUMBER];
                var endLineNumberJson = json[LogMessageProperties.END_LINE_NUMBER];
                var endColumnNumberJson = json[LogMessageProperties.END_COLUMN_NUMBER];
                var messageJson = json[LogMessageProperties.MESSAGE];
                var libraryIdJson = json[LogMessageProperties.LIBRARY_ID];

                var isValid = true;

                isValid &= Enum.TryParse(levelJson.Value<string>(), out LogLevel level);
                isValid &= Enum.TryParse(codeJson.Value<string>(), out NuGetLogCode code);

                if (isValid)
                {
                    assetsLogMessage = new AssetsLogMessage(level, code, messageJson.Value<string>())
                    {
                        TargetGraphs = (IReadOnlyList<string>)ReadArray(json[LogMessageProperties.TARGET_GRAPHS] as JArray, ReadString)
                    };

                    if (level == LogLevel.Warning && warningLevelJson != null)
                    {
                        assetsLogMessage.WarningLevel = (WarningLevel)Enum.ToObject(typeof(WarningLevel), warningLevelJson.Value<int>());
                    }

                    assetsLogMessage.ProjectPath = projectPath;

                    if (filePathJson != null)
                    {
                        assetsLogMessage.FilePath = filePathJson.Value<string>();
                    }
                    else
                    {
                        assetsLogMessage.FilePath = projectPath;
                    }

                    if (startLineNumberJson != null)
                    {
                        assetsLogMessage.StartLineNumber = startLineNumberJson.Value<int>();
                    }

                    if (startColumnNumberJson != null)
                    {
                        assetsLogMessage.StartColumnNumber = startColumnNumberJson.Value<int>();
                    }

                    if (endLineNumberJson != null)
                    {
                        assetsLogMessage.EndLineNumber = endLineNumberJson.Value<int>();
                    }

                    if (endColumnNumberJson != null)
                    {
                        assetsLogMessage.EndColumnNumber = endColumnNumberJson.Value<int>();
                    }

                    if (libraryIdJson != null)
                    {
                        assetsLogMessage.LibraryId = libraryIdJson.Value<string>();
                    }
                }
            }

            return assetsLogMessage;
        }

        internal static JArray WriteLogMessages(IEnumerable<IAssetsLogMessage> logMessages, string projectPath)
        {
            using var writer = new JTokenWriter();

            WriteLogMessages(writer, logMessages, projectPath);

            return (JArray)writer.Token;
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

        private static LockFileTargetLibrary ReadTargetLibrary(string property, JToken json)
        {
            var library = new LockFileTargetLibrary();

#pragma warning disable CA1307 // Specify StringComparison
            int slashIndex = property.IndexOf('/');
#pragma warning restore CA1307 // Specify StringComparison
            if (slashIndex == -1)
            {
                library.Name = property;
            }
            else
            {
                library.Name = property.Substring(0, slashIndex);
                library.Version = NuGetVersion.Parse(property.Substring(slashIndex + 1));
            }

            var jObject = json as JObject;
            library.Type = JsonUtility.ReadProperty<string>(jObject, TypeProperty);
            library.Framework = JsonUtility.ReadProperty<string>(jObject, FrameworkProperty);

            if (JsonUtility.ReadObject(json[DependenciesProperty] as JObject, JsonUtility.ReadPackageDependency) is { Count: not 0 } dependencies)
            {
                library.Dependencies = dependencies;
            }

            if (ReadArray(json[FrameworkAssembliesProperty] as JArray, ReadString) is { Count: not 0 } frameworkAssemblies)
            {
                library.FrameworkAssemblies = frameworkAssemblies;
            }

            if (JsonUtility.ReadObject(json[RuntimeProperty] as JObject, ReadFileItem) is { Count: not 0 } runtimeAssemblies)
            {
                library.RuntimeAssemblies = runtimeAssemblies;
            }

            if (JsonUtility.ReadObject(json[CompileProperty] as JObject, ReadFileItem) is { Count: not 0 } compileTimeAssemblies)
            {
                library.CompileTimeAssemblies = compileTimeAssemblies;
            }

            if (JsonUtility.ReadObject(json[ResourceProperty] as JObject, ReadFileItem) is { Count: not 0 } resourceAssemblies)
            {
                library.ResourceAssemblies = resourceAssemblies;
            }

            if (JsonUtility.ReadObject(json[NativeProperty] as JObject, ReadFileItem) is { Count: not 0 } nativeLibraries)
            {
                library.NativeLibraries = nativeLibraries;
            }

            if (JsonUtility.ReadObject(json[BuildProperty] as JObject, ReadFileItem) is { Count: not 0 } build)
            {
                library.Build = build;
            }

            if (JsonUtility.ReadObject(json[BuildMultiTargetingProperty] as JObject, ReadFileItem) is { Count: not 0 } buildMultiTargeting)
            {
                library.BuildMultiTargeting = buildMultiTargeting;
            }

            if (JsonUtility.ReadObject(json[ContentFilesProperty] as JObject, ReadContentFile) is { Count: not 0 } contentFiles)
            {
                library.ContentFiles = contentFiles;
            }

            if (JsonUtility.ReadObject(json[RuntimeTargetsProperty] as JObject, ReadRuntimeTarget) is { Count: not 0 } runtimeTargets)
            {
                library.RuntimeTargets = runtimeTargets;
            }

            if (JsonUtility.ReadObject(json[ToolsProperty] as JObject, ReadFileItem) is { Count: not 0 } toolsAssemblies)
            {
                library.ToolsAssemblies = toolsAssemblies;
            }

            if (JsonUtility.ReadObject(json[EmbedProperty] as JObject, ReadFileItem) is { Count: not 0 } embedAssemblies)
            {
                library.EmbedAssemblies = embedAssemblies;
            }

            if (ReadArray(json[FrameworkReferencesProperty] as JArray, ReadString) is { Count: not 0 } frameworkReferences)
            {
                library.FrameworkReferences = frameworkReferences;
            }

            library.Freeze();

            return library;
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

        private static LockFileRuntimeTarget ReadRuntimeTarget(string property, JToken json)
        {
            return ReadFileItem(property, json, path => new LockFileRuntimeTarget(path));
        }

        private static LockFileContentFile ReadContentFile(string property, JToken json)
        {
            return ReadFileItem(property, json, path => new LockFileContentFile(path));
        }

        private static ProjectFileDependencyGroup ReadProjectFileDependencyGroup(string property, JToken json)
        {
            return new ProjectFileDependencyGroup(
                property,
                ReadArray(json as JArray, ReadString));
        }

        private static PackageSpec ReadPackageSpec(JObject json)
        {
            if (json == null)
            {
                return null;
            }

#pragma warning disable CS0618
            return JsonPackageSpecReader.GetPackageSpec(
                json,
                name: null,
                packageSpecPath: null,
                snapshotValue: null);
#pragma warning restore CS0618
        }

        private static void WriteProjectFileDependencyGroup(JsonWriter writer, ProjectFileDependencyGroup frameworkInfo)
        {
            writer.WritePropertyName(frameworkInfo.FrameworkName);
            WriteArray(writer, frameworkInfo.Dependencies);
        }

        private static LockFileItem ReadFileItem(string property, JToken json)
        {
            return ReadFileItem(property, json, path => new LockFileItem(path));
        }

        private static T ReadFileItem<T>(string property, JToken json, Func<string, T> factory) where T : LockFileItem
        {
            var item = factory(property);
            foreach (var subProperty in json.OfType<JProperty>())
            {
                item.Properties[subProperty.Name] = subProperty.Value.Value<string>();
            }
            return item;
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

        private static IList<TItem> ReadArray<TItem>(JArray json, Func<JToken, TItem> readItem)
        {
            if (json == null)
            {
                return new List<TItem>(0);
            }
            var items = new List<TItem>(json.Count);
            foreach (var child in json)
            {
                var item = readItem(child);
                if (item != null)
                {
                    items.Add(item);
                }
            }
            return items;
        }

        internal static IList<IAssetsLogMessage> ReadLogMessageArray(JArray json, string projectPath)
        {
            if (json == null)
            {
                return new List<IAssetsLogMessage>();
            }

            var items = new List<IAssetsLogMessage>(json.Count);
            foreach (var child in json)
            {
                var logMessage = ReadLogMessage(child as JObject, projectPath);
                if (logMessage != null)
                {
                    items.Add(logMessage);
                }
            }
            return items;
        }

        private static IList<string> ReadPathArray(JArray json)
        {
            return ReadArray(json, f => GetPathWithForwardSlashes(ReadString(f)));
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

        private static bool ReadBool(JToken cursor, string property, bool defaultValue)
        {
            var valueToken = cursor[property];
            if (valueToken == null)
            {
                return defaultValue;
            }
            return valueToken.Value<bool>();
        }

        private static string ReadString(JToken json)
        {
            return json.Value<string>();
        }

        private static SemanticVersion ReadSemanticVersion(JToken json, string property)
        {
            var valueToken = json[property];
            if (valueToken == null)
            {
                throw new Exception(string.Format(CultureInfo.CurrentCulture, "TODO: lock file missing required property {0}", property));
            }
            return SemanticVersion.Parse(valueToken.Value<string>());
        }

        private static string GetPathWithForwardSlashes(string path)
        {
            return path.Replace('\\', '/');
        }

        private static string GetPathWithBackSlashes(string path)
        {
            return path.Replace('/', '\\');
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

        [Obsolete]
        private static List<CentralTransitiveDependencyGroup> ReadProjectFileTransitiveDependencyGroup(JObject json, string path)
        {
            var results = new List<CentralTransitiveDependencyGroup>();

            if (json == null)
            {
                return results;
            }

            using (var stringReader = new StringReader(json.ToString()))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                jsonReader.ReadObject(frameworkPropertyName =>
                {
                    NuGetFramework framework = NuGetFramework.Parse(frameworkPropertyName);
                    var dependencies = new List<LibraryDependency>();

                    JsonPackageSpecReader.ReadCentralTransitiveDependencyGroup(
                        jsonReader: jsonReader,
                        results: dependencies,
                        packageSpecPath: path);
                    results.Add(new CentralTransitiveDependencyGroup(framework, dependencies));
                });
            }
            return results;
        }
    }
}
