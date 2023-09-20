// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Build.Framework;
using Newtonsoft.Json;

namespace Microsoft.Build.NuGetSdkResolver
{
    /// <summary>
    /// Represents an implementation of <see cref="IGlobalJsonReader" /> that reads MSBuild project SDK related sections from a global.json.
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/core/tools/global-json?#msbuild-sdks" />
    /// </summary>
    internal sealed class GlobalJsonReader : IGlobalJsonReader
    {
        /// <summary>
        /// The default name of the file containing configuration information.
        /// </summary>
        public const string GlobalJsonFileName = "global.json";

        /// <summary>
        /// The name of the section in global.json that contains MSBuild project SDK versions.
        /// </summary>
        public const string MSBuildSdksPropertyName = "msbuild-sdks";

        /// <summary>
        /// Represents a thread-safe cache for files based on their full path and last write time.
        /// </summary>
        private static readonly ConcurrentDictionary<FileInfo, (DateTime LastWriteTime, Lazy<Dictionary<string, string>> Lazy)> FileCache = new ConcurrentDictionary<FileInfo, (DateTime, Lazy<Dictionary<string, string>>)>(FileSystemInfoFullNameEqualityComparer.Instance);


        private GlobalJsonReader()
        {
        }

        public static GlobalJsonReader Instance { get; } = new GlobalJsonReader();

        /// <summary>
        /// Occurs when a file is read.
        /// </summary>
        public event EventHandler<string> FileRead;

        /// <inheritdoc cref="IGlobalJsonReader.GetMSBuildSdkVersions(SdkResolverContext, string)" />
        public Dictionary<string, string> GetMSBuildSdkVersions(SdkResolverContext context, string fileName = GlobalJsonFileName)
        {
            // Prefer looking next to the solution file as its more likely to be closer to global.json
            string startingPath = GetStartingPath(context);

            // If the SolutionFilePath and ProjectFilePath are not set, an in-memory project is being evaluated and there's no way to know which directory to start looking for a global.json
            if (string.IsNullOrWhiteSpace(startingPath) || string.IsNullOrWhiteSpace(fileName))
            {
                return null;
            }

            FileInfo globalJsonPath;

            try
            {
                DirectoryInfo projectDirectory = Directory.GetParent(startingPath);

                if (projectDirectory == null || !TryGetPathOfFileAbove(fileName, projectDirectory, out globalJsonPath))
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                // Failed to determine path to global.json from path "{0}". {1}
                context.Logger.LogMessage(string.Format(CultureInfo.CurrentCulture, Strings.FailedToFindPathToGlobalJson, startingPath, e.Message));

                return null;
            }

            // Add a new file to the cache if it doesn't exist.  If the file is already in the cache, read it again if the file has changed
            (DateTime _, Lazy<Dictionary<string, string>> Lazy) cacheEntry = FileCache.AddOrUpdate(
                globalJsonPath,
                key => (key.LastWriteTime, new Lazy<Dictionary<string, string>>(() => ParseMSBuildSdkVersions(key.FullName, context))),
                (key, item) =>
                {
                    DateTime lastWriteTime = key.LastWriteTime;

                    if (item.LastWriteTime < lastWriteTime)
                    {
                        return (lastWriteTime, new Lazy<Dictionary<string, string>>(() => ParseMSBuildSdkVersions(key.FullName, context)));
                    }

                    return item;
                });

            Dictionary<string, string> sdkVersions = cacheEntry.Lazy.Value;

            return sdkVersions;
        }

        internal static string GetStartingPath(SdkResolverContext context)
        {
            if (context == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(context.SolutionFilePath))
            {
                return context.SolutionFilePath;
            }

            if (!string.IsNullOrWhiteSpace(context.ProjectFilePath))
            {
                return context.ProjectFilePath;
            }

            return null;
        }

        /// <summary>
        /// Searches for a file in the specified starting directory and any of the parent directories.
        /// </summary>
        /// <param name="file">The name of the file to search for.</param>
        /// <param name="startingDirectory">The <see cref="DirectoryInfo" /> to look in first and then search the parent directories of.</param>
        /// <param name="fullPath">Receives a <see cref="FileInfo" /> of the file if one is found, otherwise <c>null</c>.</param>
        /// <returns><c>true</c> if the specified file was found in the directory or one of its parents, otherwise <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryGetPathOfFileAbove(string file, DirectoryInfo startingDirectory, out FileInfo fullPath)
        {
            fullPath = null;

            if (string.IsNullOrWhiteSpace(file) || startingDirectory == null || !startingDirectory.Exists)
            {
                return false;
            }

            DirectoryInfo currentDirectory = startingDirectory;

            FileInfo candidatePath;

            do
            {
                candidatePath = new FileInfo(Path.Combine(currentDirectory.FullName, file));

                if (candidatePath.Exists)
                {
                    fullPath = candidatePath;

                    return true;
                }

                currentDirectory = currentDirectory.Parent;
            }
            while (currentDirectory != null);

            return false;
        }

        /// <summary>
        /// Parses the <c>msbuild-sdks</c> section of the specified JSON string.
        /// </summary>
        /// <param name="json">The JSON to parse as a string.</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}" /> containing MSBuild project SDK versions if any were found, otherwise <c>null</c>.</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Dictionary<string, string> ParseMSBuildSdkVersionsFromJson(string json)
        {
            using (var reader = new JsonTextReader(new StringReader(json)))
            {
                // Read to the first {
                while (reader.Read() && reader.TokenType != JsonToken.StartObject)
                {
                }

                if (reader.TokenType != JsonToken.StartObject)
                {
                    // Return null if no { was found
                    return null;
                }

                // Read through each top-level property
                while (reader.Read())
                {
                    // Look for the first "msbuild-sdks" section
                    if (reader.TokenType == JsonToken.PropertyName && reader.Value is string objectName && string.Equals(objectName, MSBuildSdksPropertyName, StringComparison.Ordinal) && reader.Read() && reader.TokenType == JsonToken.StartObject)
                    {
                        Dictionary<string, string> versionsByName = null;

                        // Read each token in the "msbuild-sdks" section until the end
                        while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                        {
                            // Only read properties of type string
                            if (reader.TokenType == JsonToken.PropertyName && reader.Value is string name && reader.Read() && reader.TokenType == JsonToken.String && reader.Value is string value)
                            {
                                versionsByName ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                                versionsByName[name] = value;

                                continue;
                            }

                            // Skips anything under the "mbsuild-sdks" section that wasn't a property of type string
                            reader.Skip();
                        }

                        // Stop reading the global.json once the entire "mbsuild-sdks" section is read
                        return versionsByName;
                    }

                    // Skip any top-level entry that's not a property
                    reader.Skip();
                }
            }

            // Return null if an "msbuild-sdks" section was not found
            return null;
        }

        /// <summary>
        /// Fires the <see cref="FileRead" /> event for the specified file.
        /// </summary>
        /// <param name="filePath">The full path to file that was read.</param>
        private void OnFileRead(string filePath)
        {
            EventHandler<string> fileReadEventHandler = FileRead;

            fileReadEventHandler?.Invoke(this, filePath);
        }

        /// <summary>
        /// Parses the <c>msbuild-sdks</c> section of the specified file.
        /// </summary>
        /// <param name="globalJsonPath"></param>
        /// <param name="sdkResolverContext">The current <see cref="SdkResolverContext" /> to use.</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}" /> containing MSBuild project SDK versions if any were found, otherwise <c>null</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Dictionary<string, string> ParseMSBuildSdkVersions(string globalJsonPath, SdkResolverContext sdkResolverContext)
        {
            // Load the file as a string and check if it has an msbuild-sdks section.  Parsing the contents requires Newtonsoft.Json.dll to be loaded which can be expensive
            string json;

            try
            {
                json = File.ReadAllText(globalJsonPath);
            }
            catch (Exception e)
            {
                // Failed to read file "{0}". {1}
                sdkResolverContext.Logger.LogMessage(string.Format(CultureInfo.CurrentCulture, Strings.FailedToReadGlobalJson, globalJsonPath, e.Message));

                return null;
            }

            OnFileRead(globalJsonPath);

            // Look ahead in the contents to see if there is an msbuild-sdks section.  Deserializing the file requires us to load
            // Newtonsoft.Json which is 500 KB while a global.json is usually ~100 bytes of text.
            if (json.IndexOf(MSBuildSdksPropertyName, StringComparison.Ordinal) == -1)
            {
                return null;
            }

            try
            {
                return ParseMSBuildSdkVersionsFromJson(json);
            }
            catch (Exception e)
            {
                // Failed to parse "{0}". {1}
                sdkResolverContext.Logger.LogMessage(string.Format(CultureInfo.CurrentCulture, Strings.FailedToParseGlobalJson, globalJsonPath, e.Message));

                return null;
            }
        }
    }
}
