// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;

namespace NuGet.ProjectModel
{
    public static class CacheFileFormat
    {
        private const string VersionProperty = "version";
        private const string DGSpecHashProperty = "dgSpecHash";
        private const string SuccessProperty = "success";
        private const string ExpectedPackageFilesProperty = "expectedPackageFiles";
        private const string ProjectFilePathProperty = "projectFilePath";

        public static CacheFile Read(Stream stream, ILogger log, string path)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (path == null) throw new ArgumentNullException(nameof(path));

            using (var textReader = new StreamReader(stream))
            {
                return Read(textReader, log, path);
            }
        }

        private static CacheFile Read(TextReader reader, ILogger log, string path)
        {
            try
            {
                var json = JsonUtility.LoadJson(reader);
                var cacheFile = ReadCacheFile(json);
                return cacheFile;
            }
            catch (Exception ex)
            {
                log.LogWarning(string.Format(CultureInfo.CurrentCulture,
                    Strings.Log_ProblemReadingCacheFile,
                    path, ex.Message));

                // Parsing error, the cache file is invalid. 
                return new CacheFile(null);
            }
        }

        public static void Write(string filePath, CacheFile lockFile)
        {
            // Create the directory if it does not exist
            var fileInfo = new FileInfo(filePath);
            fileInfo.Directory.Create();

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Write(stream, lockFile);
            }
        }

        public static void Write(Stream stream, CacheFile cacheFile)
        {
            using (var textWriter = new StreamWriter(stream))
            {
                Write(textWriter, cacheFile);
            }
        }

        private static void Write(TextWriter textWriter, CacheFile cacheFile)
        {
            using (var jsonWriter = new JsonTextWriter(textWriter))
            {
                jsonWriter.Formatting = Formatting.Indented;
                var json = GetCacheFile(cacheFile);
                json.WriteTo(jsonWriter);
            }
        }

        private static CacheFile ReadCacheFile(JObject cursor)
        {
            var version = ReadInt(cursor[VersionProperty]);
            var hash = ReadString(cursor[DGSpecHashProperty]);
            var success = ReadBool(cursor[SuccessProperty]);
            var cacheFile = new CacheFile(hash);
            cacheFile.Version = version;
            cacheFile.Success = success;

            if (version >= 2)
            {
                cacheFile.ProjectFilePath = ReadString(cursor[ProjectFilePathProperty]);
                cacheFile.ExpectedPackageFilePaths = new List<string>();
                foreach (JToken expectedFile in cursor[ExpectedPackageFilesProperty])
                {
                    string path = ReadString(expectedFile);

                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        cacheFile.ExpectedPackageFilePaths.Add(path);
                    }
                }

                cacheFile.LogMessages = LockFileFormat.ReadLogMessageArray(cursor[LockFileFormat.LogsProperty] as JArray, cacheFile.ProjectFilePath);
            }

            return cacheFile;
        }

        private static JObject GetCacheFile(CacheFile cacheFile)
        {
            var json = new JObject();
            json[VersionProperty] = WriteInt(cacheFile.Version);
            json[DGSpecHashProperty] = WriteString(cacheFile.DgSpecHash);
            json[SuccessProperty] = WriteBool(cacheFile.Success);

            if (cacheFile.Version >= 2)
            {
                json[ProjectFilePathProperty] = cacheFile.ProjectFilePath;
                json[ExpectedPackageFilesProperty] = new JArray(cacheFile.ExpectedPackageFilePaths);
                json[LockFileFormat.LogsProperty] = cacheFile.LogMessages == null ? new JArray() : LockFileFormat.WriteLogMessages(cacheFile.LogMessages, cacheFile.ProjectFilePath);
            }

            return json;
        }

        private static string ReadString(JToken json)
        {
            return json.Value<string>();
        }

        private static JToken WriteString(string item)
        {
            return item != null ? new JValue(item) : JValue.CreateNull();
        }

        private static int ReadInt(JToken json)
        {
            return json.Value<int>();
        }

        private static JToken WriteInt(int item)
        {
            return new JValue(item);
        }

        private static bool ReadBool(JToken json)
        {
            return json.Value<bool>();
        }

        private static JToken WriteBool(bool item)
        {
            return new JValue(item);
        }
    }
}
