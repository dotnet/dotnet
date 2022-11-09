// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

/// <summary>
/// The FileHelper interface.
/// </summary>
public interface IFileHelper
{
    /// <summary>
    /// Creates a directory.
    /// </summary>
    /// <param name="path">Path of the directory.</param>
    /// <returns><see cref="DirectoryInfo"/> for the created directory.</returns>
    DirectoryInfo CreateDirectory(string path);

    /// <summary>
    /// Gets the current directory
    /// </summary>
    /// <returns>Current directory</returns>
    string GetCurrentDirectory();

    /// <summary>
    /// Exists utility to check if file exists (case sensitive).
    /// </summary>
    /// <param name="path"> The path of file. </param>
    /// <returns>True if file exists <see cref="bool"/>.</returns>
    bool Exists([NotNullWhen(true)] string? path);

    /// <summary>
    /// Exists utility to check if directory exists (case sensitive).
    /// </summary>
    /// <param name="path"> The path of file. </param>
    /// <returns>True if directory exists <see cref="bool"/>.</returns>
    bool DirectoryExists([NotNullWhen(true)] string? path);

    /// <summary>
    /// Gets a stream for the file.
    /// </summary>
    /// <param name="filePath">Path to the file.</param>
    /// <param name="mode"><see cref="FileMode"/> for file operations.</param>
    /// <param name="access"><see cref="FileAccess"/> for file operations.</param>
    /// <returns>A <see cref="Stream"/> that supports read/write on the file.</returns>
    Stream GetStream(string filePath, FileMode mode, FileAccess access = FileAccess.ReadWrite);

    /// <summary>
    /// Gets a stream for the file.
    /// </summary>
    /// <param name="filePath">Path to the file.</param>
    /// <param name="mode"><see cref="FileMode"/> for file operations.</param>
    /// <param name="access"><see cref="FileAccess"/> for file operations.</param>
    /// <param name="share"><see cref="FileShare"/> for file operations.</param>
    /// <returns>A <see cref="Stream"/> that supports read/write on the file.</returns>
    Stream GetStream(string filePath, FileMode mode, FileAccess access, FileShare share);

    /// <summary>
    /// Enumerates files which match a pattern (case insensitive) in a directory.
    /// </summary>
    /// <param name="directory">Parent directory to search.</param>
    /// <param name="searchOption"><see cref="SearchOption"/> for directory.</param>
    /// <param name="endsWithSearchPatterns">Patterns used to select files using String.EndsWith</param>
    /// <returns>List of files matching the pattern.</returns>
    IEnumerable<string> EnumerateFiles(string directory, SearchOption searchOption, params string[]? endsWithSearchPatterns);

    /// <summary>
    /// Gets attributes of a file.
    /// </summary>
    /// <param name="path">Full path of the file.</param>
    /// <returns>Attributes of the file.</returns>
    FileAttributes GetFileAttributes(string path);

    /// <summary>
    /// Gets the version information of the file.
    /// </summary>
    /// <param name="path">Full path to the file.</param>
    /// <returns>File Version information of the file.</returns>
    Version GetFileVersion(string path);

    /// <summary>
    /// Copy a file in the file system.
    /// </summary>
    /// <param name="sourcePath">Full path of the file.</param>
    /// <param name="destinationPath">Target path for the file.</param>
    void CopyFile(string sourcePath, string destinationPath);

    /// <summary>
    /// Moves a file in the file system.
    /// </summary>
    /// <param name="sourcePath">Full path of the file.</param>
    /// <param name="destinationPath">Target path for the file.</param>
    void MoveFile(string sourcePath, string destinationPath);

    /// <summary>
    /// The write all text to file.
    /// </summary>
    /// <param name="filePath">
    /// The file Path.
    /// </param>
    /// <param name="content">
    /// The content.
    /// </param>
    void WriteAllTextToFile(string filePath, string content);

    /// <summary>
    /// Gets full path if relative path is specified.
    /// </summary>
    /// <param name="path">
    /// The path.
    /// </param>
    /// <returns>
    /// Full path.
    /// </returns>
    string GetFullPath(string path);

    /// <summary>
    /// Helper for deleting a directory. It deletes the directory only if its empty.
    /// </summary>
    /// <param name="directoryPath">
    /// The directory path.
    /// </param>
    void DeleteEmptyDirectroy(string directoryPath);

    /// <summary>
    /// Helper for deleting a directory.
    /// </summary>
    /// <param name="directoryPath">
    /// The directory path.
    /// </param>
    void DeleteDirectory(string directoryPath, bool recursive);

    /// <summary>
    /// Gets all files in directory based on search pattern
    /// </summary>
    /// <param name="path">Directory Path</param>
    /// <param name="searchPattern">Search pattern</param>
    /// <param name="searchOption">Search option</param>
    /// <returns>string[]</returns>
    string[] GetFiles(string path, string searchPattern, SearchOption searchOption);

    /// <summary>
    /// Deletes the specified file
    /// </summary>
    /// <param name="path"></param>
    void Delete(string path);

    /// <summary>
    /// Get temporary file path
    /// </summary>
    /// <param name="path"></param>
    public string GetTempPath();

    /// <summary>
    /// Get file length
    /// </summary>
    /// <param name="path"></param>
    public long GetFileLength(string path);
}
