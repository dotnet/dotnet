// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.IO;
using System.IO.Compression;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Arcade.Sdk
{
    public class CreateCompressedTar : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string? SourceDir { get; set; }

        [Required]
        public string? DestinationFile { get; set; }

        [Required]
        public string CompressionType { get; set; } = "gz";

        [Required]
        public string Format { get; set; } = "pax";

        public bool Deterministic { get; set; } = true;

        public string DeterministicTimestamp { get; set; } = "true";

        public bool DereferenceSymlinks { get; set; }

        public override bool Execute()
        {
            try
            {
                if (string.IsNullOrEmpty(DestinationFile))
                {
                    Log.LogError("Destination file not specified.");
                    return false;
                }

                if (File.Exists(DestinationFile))
                {
                    File.Delete(DestinationFile);
                }

                if (!Directory.Exists(SourceDir))
                {
                    Log.LogError($"Source directory does not exist: '{SourceDir}'");
                    return false;
                }

                if (!Enum.TryParse<TarEntryFormat>(Format, true, out TarEntryFormat targetFormat))
                {
                    Log.LogError($"Invalid Tar Format flag: '{Format}'. Valid options are Gnu, Pax, Ustar, or V7.");
                    return false;
                }

                string algorithm = CompressionType.Trim().ToLowerInvariant();
                if (algorithm is not "gz")
                {
                    Log.LogError($"Unsupported compression type: '{CompressionType}'. Valid choices are 'gz'.");
                    return false;
                }

                using FileStream fs = new(DestinationFile, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.SequentialScan);

                using GZipStream compressed = new(fs, CompressionLevel.Optimal);
                WriteTar(compressed, targetFormat);

                Log.LogMessage(MessageImportance.High, $"Successfully generated a .{algorithm} tar archive format [{targetFormat}] at: {DestinationFile}");
            }
            catch (Exception ex)
            {
                Log.LogError($"Failed to create archive: {ex.Message}");
                return false;
            }

            return !Log.HasLoggedErrors;
        }

        private void WriteTar(Stream destination, TarEntryFormat format)
        {
            DateTimeOffset? fixedTimestamp = Deterministic ? ParseDeterministicTimestamp() : null;

            using TarWriter writer = new(destination, format, leaveOpen: true);

            List<string> entries = new();
            CollectEntries(SourceDir!, entries);

            if (Deterministic)
            {
                entries.Sort(StringComparer.Ordinal);
            }

            foreach (string fullPath in entries)
            {
                string relativePath = Path.GetRelativePath(SourceDir!, fullPath);
                relativePath = relativePath.Replace('\\', '/');

                FileSystemInfo info = File.GetAttributes(fullPath).HasFlag(FileAttributes.Directory)
                    ? new DirectoryInfo(fullPath)
                    : new FileInfo(fullPath);

                string? linkTarget = info.LinkTarget;
                bool isSymlink = linkTarget != null && !DereferenceSymlinks;
                bool isDirectory = info is DirectoryInfo && !isSymlink;

                TarEntryType entryType;
                if (isSymlink)
                    entryType = TarEntryType.SymbolicLink;
                else if (isDirectory)
                    entryType = TarEntryType.Directory;
                else
                    entryType = TarEntryType.RegularFile;

                if (isDirectory && !relativePath.EndsWith('/'))
                    relativePath += '/';

                TarEntry entry = CreateEntry(format, entryType, relativePath);

                entry.ModificationTime = fixedTimestamp ?? new DateTimeOffset(info.LastWriteTimeUtc, TimeSpan.Zero);

                if (Deterministic)
                {
                    entry.Uid = 0;
                    entry.Gid = 0;
                    if (entry is PosixTarEntry posixEntry)
                    {
                        posixEntry.UserName = "";
                        posixEntry.GroupName = "";
                    }
                }

                if (!OperatingSystem.IsWindows())
                {
                    if (isDirectory)
                    {
                        entry.Mode = new DirectoryInfo(fullPath).UnixFileMode;
                    }
                    else
                    {
                        entry.Mode = new FileInfo(fullPath).UnixFileMode;
                    }
                }

                if (isSymlink)
                {
                    entry.LinkName = linkTarget!;
                }
                else if (entryType == TarEntryType.RegularFile)
                {
                    using FileStream fileStream = new(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    entry.DataStream = fileStream;
                    writer.WriteEntry(entry);
                    continue;
                }

                writer.WriteEntry(entry);
            }
        }

        private static TarEntry CreateEntry(TarEntryFormat format, TarEntryType entryType, string entryName)
        {
            return format switch
            {
                TarEntryFormat.Pax => new PaxTarEntry(entryType, entryName),
                TarEntryFormat.Gnu => new GnuTarEntry(entryType, entryName),
                TarEntryFormat.Ustar => new UstarTarEntry(entryType, entryName),
                TarEntryFormat.V7 => new V7TarEntry(entryType, entryName),
                _ => throw new ArgumentException($"Unsupported tar format: {format}")
            };
        }

        private void CollectEntries(string directory, List<string> results, HashSet<string>? visitedRealPaths = null)
        {
            foreach (string entry in Directory.GetFileSystemEntries(directory))
            {
                results.Add(entry);
                bool isSymlink = File.GetAttributes(entry).HasFlag(FileAttributes.ReparsePoint);
                if (Directory.Exists(entry) && (!isSymlink || DereferenceSymlinks))
                {
                    if (isSymlink)
                    {
                        visitedRealPaths ??= new HashSet<string>(StringComparer.Ordinal);
                        string? resolved = new DirectoryInfo(entry).ResolveLinkTarget(returnFinalTarget: true)?.FullName;
                        if (resolved == null || !visitedRealPaths.Add(resolved))
                            continue;
                    }
                    CollectEntries(entry, results, visitedRealPaths);
                }
            }
        }

        private DateTimeOffset? ParseDeterministicTimestamp()
        {
            if (string.IsNullOrEmpty(DeterministicTimestamp) ||
                DeterministicTimestamp.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                return DateTimeOffset.UtcNow;
            }

            if (DeterministicTimestamp.Equals("false", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (long.TryParse(DeterministicTimestamp, out long epoch))
            {
                return DateTimeOffset.FromUnixTimeSeconds(epoch);
            }

            if (DateTimeOffset.TryParse(DeterministicTimestamp, out DateTimeOffset dto))
            {
                return dto;
            }

            Log.LogWarning($"Could not parse DeterministicTimestamp '{DeterministicTimestamp}', falling back to DateTimeOffset.UtcNow");
            return DateTimeOffset.UtcNow;
        }
    }
}
