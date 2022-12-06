// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace NuGet.Test.Utility
{
    public static class ZipExtensions
    {
        public static ZipArchiveEntry AddEntry(this ZipArchive archive, string path, byte[] data)
        {
            var entry = archive.CreateEntry(path);

            using (var stream = entry.Open())
            {
                stream.Write(data, 0, data.Length);
            }

            return entry;
        }

        public static async Task<ZipArchiveEntry> AddEntryAsync(this ZipArchive archive, string path, byte[] data)
        {
            var entry = archive.CreateEntry(path);

            using (var stream = entry.Open())
            {
                await stream.WriteAsync(data, 0, data.Length);
            }

            return entry;
        }

        public static ZipArchiveEntry AddEntry(this ZipArchive archive, string path, string value, Encoding encoding = null)
        {
            var entry = archive.CreateEntry(path);

            using (var stream = entry.Open())
            {
                var data = (encoding ?? Encoding.UTF8).GetBytes(value);
                stream.Write(data, 0, data.Length);
            }

            return entry;
        }

        public static async Task<ZipArchiveEntry> AddEntryAsync(this ZipArchive archive, string path, string value, Encoding encoding = null)
        {
            var entry = archive.CreateEntry(path);

            using (var stream = entry.Open())
            {
                var data = (encoding ?? Encoding.UTF8).GetBytes(value);
                await stream.WriteAsync(data, 0, data.Length);
            }

            return entry;
        }

        public static void ExtractAll(this ZipArchive archive, string targetPath)
        {
            string fullDestDirPath = Path.GetFullPath(targetPath + Path.DirectorySeparatorChar);
            foreach (var entry in archive.Entries)
            {
                var entryFullName = entry.FullName;
                if (entryFullName.StartsWith("/", StringComparison.Ordinal))
                {
                    entryFullName = entryFullName.Substring(1);
                }

                entryFullName = Uri.UnescapeDataString(entryFullName.Replace('/', Path.DirectorySeparatorChar));

                string targetFile = Path.GetFullPath(Path.Combine(targetPath, entryFullName));
                if (!targetFile.StartsWith(fullDestDirPath, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (Path.GetFileName(targetFile).Length == 0)
                {
                    Directory.CreateDirectory(targetFile);
                }
                else
                {
                    var targetEntryPath = Path.GetDirectoryName(targetFile);
                    if (!Directory.Exists(targetEntryPath))
                    {
                        Directory.CreateDirectory(targetEntryPath);
                    }

                    using (var entryStream = entry.Open())
                    {
                        using (var targetStream = new FileStream(targetFile, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            entryStream.CopyTo(targetStream);
                        }
                    }
                }
            }
        }
    }
}
