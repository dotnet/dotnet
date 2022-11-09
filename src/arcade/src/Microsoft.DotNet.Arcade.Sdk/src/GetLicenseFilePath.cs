// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Arcade.Sdk
{
    /// <summary>
    /// Finds a license file in the given directory.
    /// File is considered a license file if its name matches 'license(.txt|.md|)', ignoring case.
    /// </summary>
    public class GetLicenseFilePath : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Full path to the directory to search for the license file.
        /// </summary>
        [Required]
        public string Directory { get; set; }

        /// <summary>
        /// Full path to the license file, or empty if it is not found.
        /// </summary>
        [Output]
        public string Path { get; private set; }

        public override bool Execute()
        {
            ExecuteImpl();
            return !Log.HasLoggedErrors;
        }

        private void ExecuteImpl()
        {
            const string fileName = "license";

#if NET472
            IEnumerable<string> enumerateFiles(string extension)
            {
                var fileNameWithExtension = fileName + extension;
                return System.IO.Directory.EnumerateFiles(Directory, "*", SearchOption.TopDirectoryOnly)
                        .Where(path => string.Equals(fileNameWithExtension, System.IO.Path.GetFileName(path), System.StringComparison.OrdinalIgnoreCase));
            }

#else
            var options = new EnumerationOptions
            {
                MatchCasing = MatchCasing.CaseInsensitive,
                RecurseSubdirectories = false,
                MatchType = MatchType.Simple
            };

            options.AttributesToSkip |= FileAttributes.Directory;

            IEnumerable<string> enumerateFiles(string extension) =>
                System.IO.Directory.EnumerateFileSystemEntries(Directory, fileName + extension, options);
#endif
            var matches = 
                (from extension in new[] { ".txt", ".md", "" }
                 from path in enumerateFiles(extension)
                 select path).ToArray();

            if (matches.Length == 0)
            {
                Log.LogError($"No license file found in '{Directory}'.");
            }
            else if (matches.Length > 1)
            {
                Log.LogError($"Multiple license files found in '{Directory}': '{string.Join("', '", matches)}'.");
            }
            else 
            {
                Path = matches[0];
            }
        }
    }
}
