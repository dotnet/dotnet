// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Build.Framework;
using NuGet.Shared;

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
        /// <param name="fullPath">Receives a <see cref="FileInfo" /> of the file if one is found, otherwise <see langword="null" />.</param>
        /// <returns><see langword="true" /> if the specified file was found in the directory or one of its parents, otherwise <see langword="false" />.</returns>
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
        /// Check the given stream for a json file containing a <c>msbuild-sdks</c> section and return the SDK versions if any are found.
        /// </summary>
        /// <param name="stream">The stream that will be checked for global.json msbuild-sdks content. The stream must be UTF8. The stream will not be disposed, but it will be advanced.</param>
        /// <returns>A dictionary mapping SDK names to their versions, or <c>null</c> if no <c>msbuild-sdks</c> section is found.</returns>
        internal static Dictionary<string, string> ParseMSBuildSdkVersionsFromJson(Stream stream)
        {
            var reader = new Utf8JsonStreamReader(stream);

            try
            {
                while (reader.TokenType != JsonTokenType.StartObject && reader.Read())
                {
                }

                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    return null;
                }

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        bool isMSBuildSdksProperty = reader.ValueTextEquals("msbuild-sdks"u8);

                        reader.Read();

                        if (isMSBuildSdksProperty && reader.TokenType == JsonTokenType.StartObject)
                        {
                            return ReadMSBuildSdkVersions(ref reader);
                        }

                        reader.Skip();
                    }
                    else
                    {
                        reader.Skip();
                    }
                }

                return null;
            }
            finally
            {
                reader.Dispose();
            }
        }

        private static Dictionary<string, string> ReadMSBuildSdkVersions(ref Utf8JsonStreamReader reader)
        {
            Dictionary<string, string> versionsByName = null;

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string name = reader.GetString();

                    reader.Read();

                    if (reader.TokenType == JsonTokenType.String)
                    {
                        versionsByName ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        versionsByName[name] = reader.GetString();

                        continue;
                    }
                }

                reader.Skip();
            }

            return versionsByName;
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
        /// <returns>A <see cref="Dictionary{TKey, TValue}" /> containing MSBuild project SDK versions if any were found, otherwise <see langword="null" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Dictionary<string, string> ParseMSBuildSdkVersions(string globalJsonPath, SdkResolverContext sdkResolverContext)
        {
            Stream jsonStream = null;

            if (SdkResolverEventSource.Instance.IsEnabled()) SdkResolverEventSource.Instance.GlobalJsonReadStart(globalJsonPath, sdkResolverContext.ProjectFilePath, sdkResolverContext.SolutionFilePath);

            try
            {
                try
                {
                    jsonStream = File.OpenRead(globalJsonPath);
                }
                catch (Exception e)
                {
                    // Failed to read file "{0}". {1}
                    sdkResolverContext.Logger.LogMessage(string.Format(CultureInfo.CurrentCulture, Strings.FailedToReadGlobalJson, globalJsonPath, e.Message));

                    return null;
                }

                OnFileRead(globalJsonPath);

                try
                {
                    return ParseMSBuildSdkVersionsFromJson(jsonStream);
                }
                catch (Exception e)
                {
                    // Failed to parse "{0}". {1}
                    sdkResolverContext.Logger.LogMessage(string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.FailedToParseGlobalJson,
                        globalJsonPath,
                        e.Message));

                    return null;
                }
            }
            finally
            {
                jsonStream?.Dispose();

                if (SdkResolverEventSource.Instance.IsEnabled()) SdkResolverEventSource.Instance.GlobalJsonReadStop(globalJsonPath, sdkResolverContext.ProjectFilePath, sdkResolverContext.SolutionFilePath);
            }
        }

    }
}
