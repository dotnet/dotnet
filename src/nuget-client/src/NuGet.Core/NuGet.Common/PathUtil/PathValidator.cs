// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;

namespace NuGet.Common
{
    public static class PathValidator
    {
        /// <summary>
        /// Validates that a source is a valid path or url.
        /// </summary>
        /// <param name="source">The path to validate.</param>
        /// <returns>True if valid, False if invalid.</returns>
        public static bool IsValidSource(string source)
        {
            return IsValidLocalPath(source) || IsValidUncPath(source) || IsValidUrl(source) || IsValidRelativePath(source);
        }

        /// <summary>
        /// Validates that path is properly formatted as a local path. 
        /// </summary>
        /// <remarks>
        /// On Windows, a valid local path must starts with the drive letter.
        /// Example: C:\, C:\path, C:\path\to\
        /// Bad: C:\invalid\*\"\chars
        /// </remarks>
        /// <param name="path">The path to validate.</param>
        /// <returns>True if valid, False if invalid.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "We don't want to throw during detection")]
        public static bool IsValidLocalPath(string path)
        {
            try
            {
                if (RuntimeEnvironmentHelper.IsWindows)
                {
                    // Checking driver letter on Windows
                    if (!Regex.IsMatch(path.Trim(), @"^[A-Za-z]:\\"))
                    {
                        return false;
                    }
                }
                Path.GetFullPath(path);
                // If paht is rooted and it's not a unc path, it's a local path.
                return Path.IsPathRooted(path) && !path.StartsWith(@"\\", StringComparison.Ordinal);
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Validates that path is properly formatted as an UNC path. 
        /// </summary>
        /// <remarks>
        /// Example: \\server\share, \\server\share\path, \\server\share\path\to\
        /// Bad: \\server\invalid\*\"\chars
        /// </remarks>
        /// <param name="path">The path to validate.</param>
        /// <returns>True if valid, False if invalid.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "We don't want to throw during detection")]
        public static bool IsValidUncPath(string path)
        {
            try
            {
                Path.GetFullPath(path);
                return path.StartsWith(@"\\", StringComparison.Ordinal);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validates that url is properly formatted as an URL based on .NET <see cref="System.Uri">Uri</see> class.
        /// </summary>
        /// <param name="url">The url to validate.</param>
        /// <returns>True if valid, False if invalid.</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "We're trying to validate that a stirng is infct a uri")]
        public static bool IsValidUrl(string url)
        {
            Uri result;

            // Make sure url starts with protocol:// because Uri.TryCreate() returns true for local and UNC paths even if badly formed.
            return Regex.IsMatch(url, @"^\w+://", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) && Uri.TryCreate(url, UriKind.Absolute, out result);
        }

        public static bool IsValidRelativePath(string path)
        {
            try
            {
                Path.GetFullPath(path);
                // Becasue IsPathRooted() returns false for Url path, so if path is not rooted and is not a Url, it's relative path. 
                return !Regex.IsMatch(path, @"^\w+://", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) && !Path.IsPathRooted(path);
            }
            catch
            {
                return false;
            }
        }
    }
}
