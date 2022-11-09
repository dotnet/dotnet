// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    public static class PackagesLockFileFormat
    {
        public static readonly int Version = 1;

        // Diverge the lockfile version to allow evolving the lock file schema
        public static readonly int PackagesLockFileVersion = 2;

        public static readonly string LockFileName = "packages.lock.json";

        private const string VersionProperty = "version";
        private const string ResolvedProperty = "resolved";
        private const string RequestedProperty = "requested";
        private const string ContentHashProperty = "contentHash";
        private const string DependenciesProperty = "dependencies";
        private const string TypeProperty = "type";

        public static PackagesLockFile Parse(string lockFileContent, string path)
        {
            return Parse(lockFileContent, NullLogger.Instance, path);
        }

        public static PackagesLockFile Parse(string lockFileContent, ILogger log, string path)
        {
            using (var reader = new StringReader(lockFileContent))
            {
                return Read(reader, log, path);
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

        public static PackagesLockFile Read(Stream stream, ILogger log, string path)
        {
            using (var textReader = new StreamReader(stream))
            {
                return Read(textReader, log, path);
            }
        }

        public static PackagesLockFile Read(TextReader reader, ILogger log, string path)
        {
            try
            {
                var json = JsonUtility.LoadJson(reader);
                var lockFile = ReadLockFile(json);
                lockFile.Path = path;
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
                    Path = path
                };
            }
        }

        private static PackagesLockFile ReadLockFile(JObject cursor)
        {
            var lockFile = new PackagesLockFile()
            {
                Version = JsonUtility.ReadInt(cursor, VersionProperty, defaultValue: int.MinValue),
                Targets = JsonUtility.ReadObject(cursor[DependenciesProperty] as JObject, ReadDependency),
            };

            return lockFile;
        }

        public static string Render(PackagesLockFile lockFile)
        {
            using (var writer = new StringWriter())
            {
                Write(writer, lockFile);
                return writer.ToString();
            }
        }

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

        public static void Write(Stream stream, PackagesLockFile lockFile)
        {
            using (var textWriter = new StreamWriter(stream))
            {
                Write(textWriter, lockFile);
            }
        }

        public static void Write(TextWriter textWriter, PackagesLockFile lockFile)
        {
            using (var jsonWriter = new JsonTextWriter(textWriter))
            {
                jsonWriter.Formatting = Formatting.Indented;

                var json = WriteLockFile(lockFile);
                json.WriteTo(jsonWriter);
            }
        }

        private static JObject WriteLockFile(PackagesLockFile lockFile)
        {
            var json = new JObject
            {
                [VersionProperty] = new JValue(lockFile.Version),
                [DependenciesProperty] = JsonUtility.WriteObject(lockFile.Targets, WriteTarget),
            };

            return json;
        }

        private static PackagesLockFileTarget ReadDependency(string property, JToken json)
        {
            var parts = property.Split(JsonUtility.PathSplitChars, 2);

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

    }
}
