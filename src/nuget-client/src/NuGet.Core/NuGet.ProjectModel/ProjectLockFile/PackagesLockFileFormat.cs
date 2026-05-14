// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
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
        internal static readonly int AliasedVersion = 3;

        // This allows us to maintain compatibility with older clients that don't understand the concept of central package versions.
        public static readonly int PackagesLockFileVersion = AliasedVersion;

        public static readonly string LockFileName = "packages.lock.json";

        private const string VersionProperty = "version";
        private const string ResolvedProperty = "resolved";
        private const string RequestedProperty = "requested";
        private const string ContentHashProperty = "contentHash";
        private const string DependenciesProperty = "dependencies";
        private const string TypeProperty = "type";
        private const string FrameworkProperty = "framework";

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
#if NET5_0_OR_GREATER
            using (var textWriter = new StreamWriter(stream))
#else
            using (var textWriter = new NoAllocNewLineStreamWriter(stream))
#endif
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

    }
}
