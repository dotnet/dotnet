// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.VisualStudio.TestPlatform.CommandLine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

#pragma warning disable IDE1006 // Naming Styles
namespace vstest.console.Internal;
#pragma warning restore IDE1006 // Naming Styles

/// <summary>
/// Class for getting matching files from wild card pattern file name
/// Microsoft.Extensions.FileSystemGlobbing methods used to get matching file names
/// </summary>
public class FilePatternParser
{
    private readonly Matcher _matcher;
    private readonly IFileHelper _fileHelper;
    private readonly char[] _wildCardCharacters = { '*' };

    public FilePatternParser()
        : this(new Matcher(), new FileHelper())
    {
    }

    internal FilePatternParser(Matcher matcher, IFileHelper fileHelper)
    {
        _matcher = matcher;
        _fileHelper = fileHelper;
    }

    /// <summary>
    /// Used to get matching files with pattern
    /// </summary>
    /// <returns>Returns the list of matching files</returns>
    public List<string> GetMatchingFiles(string filePattern)
    {
        var matchingFiles = new List<string>();

        // Convert the relative path to absolute path
        if (!Path.IsPathRooted(filePattern))
        {
            filePattern = Path.Combine(_fileHelper.GetCurrentDirectory(), filePattern);
        }

        // If there is no wild card simply add the file to the list of matching files.
        if (filePattern.IndexOfAny(_wildCardCharacters) == -1)
        {
            EqtTrace.Info($"FilePatternParser: The given file {filePattern} is a full path.");

            // Check if the file exists.
            if (!_fileHelper.Exists(filePattern))
            {
                throw new TestSourceException(
                    string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestSourceFileNotFound, filePattern));
            }

            matchingFiles.Add(filePattern);

            return matchingFiles;
        }

        // Split the given wild card into search directory and pattern to be searched.
        var splitPattern = SplitFilePatternOnWildCard(filePattern);
        EqtTrace.Info($"FilePatternParser: Matching file pattern '{splitPattern.Item2}' within directory '{splitPattern.Item1}'");

        _matcher.AddInclude(splitPattern.Item2);

        // Execute the given pattern in the search directory.
        var matches = _matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(splitPattern.Item1)));

        // Add all the files to the list of matching files.
        foreach (var match in matches.Files)
        {
            matchingFiles.Add(Path.Combine(splitPattern.Item1, match.Path));
        }

        return matchingFiles;
    }

    /// <summary>
    /// Splits full pattern into search directory and pattern.
    /// </summary>
    private Tuple<string, string> SplitFilePatternOnWildCard(string filePattern)
    {
        // Split the pattern based on first wild card character found.
        var splitOnWildCardIndex = filePattern.IndexOfAny(_wildCardCharacters);
        var directorySeparatorIndex = filePattern.Substring(0, splitOnWildCardIndex).LastIndexOf(Path.DirectorySeparatorChar);

        string searchDir = filePattern.Substring(0, directorySeparatorIndex);
        string pattern = filePattern.Substring(directorySeparatorIndex + 1);

        Tuple<string, string> splitPattern = new(searchDir, pattern);
        return splitPattern;
    }
}
