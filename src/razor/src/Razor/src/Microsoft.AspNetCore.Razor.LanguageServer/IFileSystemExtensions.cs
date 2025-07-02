// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal static class IFileSystemExtensions
{
    /// <summary>
    /// Finds all the files in a directory which meet the given criteria.
    /// </summary>
    /// <param name="workspaceDirectory">The directory to be searched.</param>
    /// <param name="searchPattern">The pattern to apply when searching.</param>
    /// <param name="ignoredDirectories">List of directories to skip when recursing.</param>
    /// <param name="fileSystem">Exists for tests only. DO NOT PROVIDE outside of tests.</param>
    /// <param name="logger">An optional logger to report on exceptional situations such as <see cref="PathTooLongException"/>.</param>
    /// <returns>A list of files within the given directory that meet the search criteria.</returns>
    /// <remarks>This method is needed to avoid problematic folders such as "node_modules" which are known not to yield the desired results or may cause performance issues.</remarks>
    internal static IEnumerable<string> GetFilteredFiles(
        this IFileSystem fileSystem,
        string workspaceDirectory,
        string searchPattern,
        IReadOnlyCollection<string> ignoredDirectories,
        ILogger logger)
    {
        IEnumerable<string> files;
        try
        {
            files = fileSystem.GetFiles(workspaceDirectory, searchPattern, SearchOption.TopDirectoryOnly);
        }
        catch (DirectoryNotFoundException)
        {
            // The filesystem may have deleted the directory between us finding it and searching for files in it.
            // This can also happen if the directory is too long for windows.
            files = [];
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning($"UnauthorizedAccess: {ex.Message}");
            yield break;
        }
        catch (PathTooLongException ex)
        {
            logger.LogWarning($"PathTooLong: {ex.Message}");
            yield break;
        }
        catch (IOException ex)
        {
            logger.LogWarning($"IOException: {ex.Message}");
            yield break;
        }

        foreach (var file in files)
        {
            yield return file;
        }

        IEnumerable<string> directories;
        try
        {
            directories = fileSystem.GetDirectories(workspaceDirectory);
        }
        catch (DirectoryNotFoundException)
        {
            // The filesystem may have deleted the directory between us finding it and searching for directories in it.
            // This can also happen if the directory is too long for windows.
            directories = [];
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning($"UnauthorizedAccess: {ex.Message}");
            yield break;
        }
        catch (PathTooLongException ex)
        {
            logger.LogWarning($"PathTooLong: {ex.Message}");
            yield break;
        }
        catch (IOException ex)
        {
            logger.LogWarning($"IOException: {ex.Message}");
            yield break;
        }

        foreach (var path in directories)
        {
            var directory = Path.GetFileName(path);
            if (!ignoredDirectories.Contains(directory, PathUtilities.OSSpecificPathComparer))
            {
                foreach (var result in GetFilteredFiles(fileSystem, path, searchPattern, ignoredDirectories, logger))
                {
                    yield return result;
                }
            }
        }
    }
}
