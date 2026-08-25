// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.TestPlatform.TestUtilities;

/// <summary>
/// Copies the files a failed test wants to attach into the directory where the test results are written.
/// <c>TestContext.AddResultFile</c> only records the path it is given, and the diagnostic logs are written
/// into a temporary directory that CI does not publish, so attaching them where they are leaves the trx
/// pointing at files that no longer exist by the time anyone reads it.
/// </summary>
public static class AttachmentUtils
{
    /// <summary>
    /// How much of a test name we keep when we use it as a directory name. The names of data driven tests
    /// contain the whole data row, and the resulting path has to stay within the limits of the file system.
    /// </summary>
    public const int MaxNameLength = 60;

    /// <summary>
    /// Replaces the characters that are not safe in a path, and shortens the name, so it can be used as a
    /// single directory or file name.
    /// </summary>
    public static string SanitizePathSegment(string? name, int maxLength = MaxNameLength)
    {
        var builder = new StringBuilder(maxLength);
        foreach (var character in name ?? string.Empty)
        {
            var safe = char.IsLetterOrDigit(character) || character is '.' or '-' or '_' ? character : '_';

            // One separator is enough, the names of data driven tests are full of spaces and punctuation.
            if (safe == '_' && builder.Length > 0 && builder[builder.Length - 1] == '_')
            {
                continue;
            }

            builder.Append(safe);
            if (builder.Length == maxLength)
            {
                break;
            }
        }

        // Windows does not allow a name to end with a dot or a space.
        var sanitized = builder.ToString().Trim('_', '.', ' ');
        return sanitized.Length == 0 ? "test" : sanitized;
    }

    /// <summary>
    /// Returns every file in the given attachments, which are files or directories, together with the path
    /// the file should get relative to the directory we copy it into. Directories that cannot be listed are
    /// reported to <paramref name="log"/> and skipped.
    /// </summary>
    public static List<(string Path, string RelativePath)> EnumerateAttachments(IEnumerable<string> attachments, Action<string> log)
    {
        var files = new List<(string Path, string RelativePath)>();

        // The same log directory is added once per run of vstest.console, and a test can run it more than once.
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var attachment in attachments)
        {
            if (Directory.Exists(attachment))
            {
                var root = attachment.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                foreach (var file in EnumerateFiles(root, log))
                {
                    if (seen.Add(file))
                    {
                        files.Add((file, GetRelativePath(root, file)));
                    }
                }
            }
            else if (File.Exists(attachment) && seen.Add(attachment))
            {
                files.Add((attachment, Path.GetFileName(attachment)));
            }
        }

        return files;
    }

    /// <summary>
    /// Copies the given attachments into <paramref name="destinationDirectory"/> and returns the paths of the
    /// copies. A file that cannot be copied is reported to <paramref name="log"/> and skipped. This runs in test
    /// cleanup, where throwing would replace the failure the test reported with an unrelated one.
    /// </summary>
    public static List<string> CopyAttachments(IEnumerable<string> attachments, string destinationDirectory, Action<string> log)
    {
        var copies = new List<string>();
        foreach (var (path, relativePath) in EnumerateAttachments(attachments, log))
        {
            var copy = CopyFile(path, Path.Combine(destinationDirectory, relativePath), log);
            if (copy is not null)
            {
                copies.Add(copy);
            }
        }

        return copies;
    }

    private static string? CopyFile(string source, string destination, Action<string> log)
    {
        try
        {
            destination = GetNonExistingPath(destination);
            var directory = Path.GetDirectoryName(destination);
            if (directory is not null)
            {
                Directory.CreateDirectory(directory);
            }

            // A process that did not exit yet can still hold the log file open, and File.Copy asks for more
            // access than it needs, so it fails on exactly the logs we care about the most.
            using (var from = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            using (var to = new FileStream(destination, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                from.CopyTo(to);
            }

            return destination;
        }
        catch (Exception ex)
        {
            log($"Could not copy '{source}' to '{destination}': {ex.Message}");
            return null;
        }
    }

    private static IEnumerable<string> EnumerateFiles(string directory, Action<string> log)
    {
        try
        {
            // Materialize the result, an error in the middle of a lazy enumeration would escape this try.
            return new List<string>(Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories));
        }
        catch (Exception ex)
        {
            log($"Could not list the files in '{directory}': {ex.Message}");
            return new List<string>();
        }
    }

    private static string GetRelativePath(string root, string file)
        => file.Length > root.Length && file.StartsWith(root, StringComparison.OrdinalIgnoreCase)
            ? file.Substring(root.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            : Path.GetFileName(file);

    private static string GetNonExistingPath(string path)
    {
        if (!File.Exists(path))
        {
            return path;
        }

        var directory = Path.GetDirectoryName(path) ?? string.Empty;
        var name = Path.GetFileNameWithoutExtension(path);
        var extension = Path.GetExtension(path);
        for (var suffix = 2; suffix <= 100; suffix++)
        {
            var candidate = Path.Combine(directory, $"{name}_{suffix}{extension}");
            if (!File.Exists(candidate))
            {
                return candidate;
            }
        }

        return Path.Combine(directory, $"{name}_{RandomId.Next()}{extension}");
    }
}
