// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal static class DirectoryHelper
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
        string workspaceDirectory,
        string searchPattern,
        IReadOnlyCollection<string> ignoredDirectories,
#pragma warning disable CS0618 // Type or member is obsolete
        IFileSystem? fileSystem = null,
#pragma warning restore CS0618 // Type or member is obsolete
        ILogger? logger = null)
    {
        if (workspaceDirectory is null)
        {
            throw new ArgumentNullException(nameof(workspaceDirectory));
        }

        if (searchPattern is null)
        {
            throw new ArgumentNullException(nameof(searchPattern));
        }

        if (ignoredDirectories is null || ignoredDirectories.Count == 0)
        {
            throw new ArgumentNullException(nameof(ignoredDirectories));
        }

        if (fileSystem is null)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            fileSystem = new DefaultFileSystem();
#pragma warning restore CS0618 // Type or member is obsolete
        }

        IEnumerable<string> files;
        try
        {
            files = fileSystem.GetFiles(workspaceDirectory, searchPattern, SearchOption.TopDirectoryOnly);
        }
        catch (DirectoryNotFoundException)
        {
            // The filesystem may have deleted the directory between us finding it and searching for files in it.
            // This can also happen if the directory is too long for windows.
            files = Array.Empty<string>();
        }
        catch (UnauthorizedAccessException ex)
        {
            logger?.LogWarning("UnauthorizedAccess: {exception}", ex.Message);
            yield break;
        }
        catch (PathTooLongException ex)
        {
            logger?.LogWarning("PathTooLong: {exception}", ex.Message);
            yield break;
        }
        catch (IOException ex)
        {
            logger?.LogWarning("IOException: {exception}", ex.Message);
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
            directories = Array.Empty<string>();
        }
        catch (UnauthorizedAccessException ex)
        {
            logger?.LogWarning("UnauthorizedAccess: {exception}", ex.Message);
            yield break;
        }
        catch (PathTooLongException ex)
        {
            logger?.LogWarning("PathTooLong: {exception}", ex.Message);
            yield break;
        }
        catch (IOException ex)
        {
            logger?.LogWarning("IOException: {exception}", ex.Message);
            yield break;
        }

        foreach (var path in directories)
        {
            var directory = Path.GetFileName(path);
            if (!ignoredDirectories.Contains(directory, FilePathComparer.Instance))
            {
                foreach (var result in GetFilteredFiles(path, searchPattern, ignoredDirectories, fileSystem, logger))
                {
                    yield return result;
                }
            }
        }
    }

    [Obsolete("This only exists to enable testing, do not use it outside of tests for this class")]
    internal interface IFileSystem
    {
        public IEnumerable<string> GetFiles(string workspaceDirectory, string searchPattern, SearchOption searchOption);

        public IEnumerable<string> GetDirectories(string workspaceDirectory);
    }

    [Obsolete("This only exists to enable testing, do not use it outside of tests for this class")]
    private class DefaultFileSystem : IFileSystem
    {
        public IEnumerable<string> GetFiles(string workspaceDirectory, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetFiles(workspaceDirectory, searchPattern, searchOption);
        }

        public IEnumerable<string> GetDirectories(string workspaceDirectory)
        {
            return Directory.GetDirectories(workspaceDirectory);
        }
    }
}
