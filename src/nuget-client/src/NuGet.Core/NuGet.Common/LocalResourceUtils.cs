// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace NuGet.Common
{
    public static class LocalResourceUtils
    {
        public static void DeleteDirectoryTree(string folderPath, List<string> failedDeletes)
        {
            if (!Directory.Exists(folderPath))
            {
                // Non-issue.
                return;
            }

            bool fallbackToSafeDelete = false;
            try
            {
                // Try most-performant path first (initially avoiding Directory.EnumerateDirectories)
                Directory.Delete(folderPath, recursive: true);
            }
            catch (DirectoryNotFoundException)
            {
                // Should not happen, but it is a non-issue.
            }
            catch (IOException)
            {
                fallbackToSafeDelete = true;
            }
            catch (UnauthorizedAccessException)
            {
                fallbackToSafeDelete = true;
            }

            if (fallbackToSafeDelete)
            {
                DeleteFilesInDirectoryTree(folderPath, failedDeletes);

                try
                {
                    SafeDeleteDirectoryTree(folderPath);
                }
                catch (DirectoryNotFoundException)
                {
                    // Should not happen, but it is a non-issue.
                }
                catch (PathTooLongException)
                {
                    failedDeletes.Add(folderPath);
                }
                catch (UnauthorizedAccessException)
                {
                    failedDeletes.Add(folderPath);
                }
            }
        }

        private static void SafeDeleteDirectoryTree(string folderPath)
        {
            try
            {
                // Deletes the specified directory and any subdirectories and files in the directory.
                // When deleting a directory that contains a reparse point, such as a symbolic link or a mount point:
                // * If the reparse point is a directory, such as a mount point,
                //   it is unmounted and the mount point is deleted.
                //   This method does not recurse through the reparse point.
                // * If the reparse point is a symbolic link to a file,
                //   the reparse point is deleted and not the target of the symbolic link.
                Directory.Delete(folderPath, recursive: true);
            }
            catch (DirectoryNotFoundException)
            {
                // Should not happen, but it is a non-issue.
            }
            catch (IOException)
            {
                // Try once more.
                // The directory may be in use by another process and cause an IOException.
                Thread.Sleep(500);
                Directory.Delete(folderPath, recursive: true);
            }
        }

        private static void DeleteFilesInDirectoryTree(string folderPath, List<string> failedDeletes)
        {
            // Using the default SearchOption.TopDirectoryOnly, as SearchOption.AllDirectories would also
            // include reparse points such as mounted drives and symbolic links in the search.
            foreach (var subFolderPath in Directory.EnumerateDirectories(folderPath))
            {
                var directoryInfo = new DirectoryInfo(subFolderPath);
                if (!directoryInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                {
                    DeleteFilesInDirectoryTree(subFolderPath, failedDeletes);
                }
            }

            foreach (var file in Directory.EnumerateFiles(folderPath))
            {
                var filePath = Path.Combine(folderPath, Path.GetFileName(file));
                try
                {
                    // When files or folders are readonly, the File.Delete method may not be able to delete it.
                    var attributes = File.GetAttributes(filePath);
                    if (attributes.HasFlag(FileAttributes.ReadOnly))
                    {
                        // Remove the readonly flag when set.
                        attributes &= ~FileAttributes.ReadOnly;
                        File.SetAttributes(filePath, attributes);
                    }

                    File.Delete(filePath);
                }
                catch (PathTooLongException)
                {
                    failedDeletes.Add(filePath);
                }
                catch (UnauthorizedAccessException)
                {
                    failedDeletes.Add(filePath);
                }
                catch (IOException)
                {
                    // The file is being used by another process.
                    failedDeletes.Add(filePath);
                }
            }
        }
    }
}
