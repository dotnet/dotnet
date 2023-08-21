// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Common
{
    /// <summary>
    /// File operation helpers.
    /// </summary>
    public static class FileUtility
    {
        public static readonly int MaxTries = 3;
        public static readonly FileShare FileSharePermissions = FileShare.ReadWrite | FileShare.Delete;

        /// <summary>
        /// Get the full path to a new temp file
        /// </summary>
        public static string GetTempFilePath(string directory)
        {
            var fileName = $"{Guid.NewGuid()}.tmp".ToLowerInvariant();

            return Path.GetFullPath(Path.Combine(directory, fileName));
        }

        /// <summary>
        /// Lock around the output path.
        /// Delete the existing file with retries.
        /// </summary>
        public static async Task DeleteWithLock(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            await ConcurrencyUtilities.ExecuteWithFileLockedAsync(filePath,
                lockedToken =>
                {
                    Delete(filePath);

                    return TaskResult.Zero;
                },
                // Do not allow this to be cancelled
                CancellationToken.None);
        }

        /// <summary>
        /// Lock around the output path.
        /// Delete the existing file with retries.
        /// Move a file with retries.
        /// </summary>
        public static async Task ReplaceWithLock(Action<string> writeSourceFile, string destFilePath)
        {
            if (writeSourceFile == null)
            {
                throw new ArgumentNullException(nameof(writeSourceFile));
            }

            if (destFilePath == null)
            {
                throw new ArgumentNullException(nameof(destFilePath));
            }

            await ConcurrencyUtilities.ExecuteWithFileLockedAsync(destFilePath,
                lockedToken =>
                {
                    Replace(writeSourceFile, destFilePath);

                    return TaskResult.Zero;
                },
                // Do not allow this to be cancelled
                CancellationToken.None);
        }

        /// <summary>
        /// Delete the existing file with retries.
        /// Move a file with retries.
        /// </summary>
        public static void Replace(Action<string> writeSourceFile, string destFilePath)
        {
            if (writeSourceFile == null)
            {
                throw new ArgumentNullException(nameof(writeSourceFile));
            }

            if (destFilePath == null)
            {
                throw new ArgumentNullException(nameof(destFilePath));
            }

            string? destFileDirectory = Path.GetDirectoryName(destFilePath);
            if (destFileDirectory is null)
            {
                throw new ArgumentException(paramName: destFilePath, message: "Path.GetDirectoryName(destFilePath) returned null");
            }
            var tempPath = GetTempFilePath(destFileDirectory);

            try
            {
                // Write to temp path
                writeSourceFile(tempPath);

                // Delete the previous file and move the temporary file
                // This will throw if there is a failure
                Replace(tempPath, destFilePath);
            }
            catch
            {
                // Clean up the temporary file
                Delete(tempPath);

                // Throw since this failed
                throw;
            }
        }

        public static async Task ReplaceAsync(Func<string, Task> writeSourceFile, string destFilePath)
        {
            if (writeSourceFile == null)
            {
                throw new ArgumentNullException(nameof(writeSourceFile));
            }

            if (destFilePath == null)
            {
                throw new ArgumentNullException(nameof(destFilePath));
            }

            string? destFileDirectory = Path.GetDirectoryName(destFilePath);
            if (destFileDirectory is null)
            {
                throw new ArgumentException(paramName: destFilePath, message: "Path.GetDirectoryName(destFilePath) returned null");
            }
            var tempPath = GetTempFilePath(destFileDirectory);

            try
            {
                // Write to temp path
                await writeSourceFile(tempPath);

                // Delete the previous file and move the temporary file
                // This will throw if there is a failure
                Replace(tempPath, destFilePath);
            }
            catch
            {
                // Clean up the temporary file
                Delete(tempPath);

                // Throw since this failed
                throw;
            }
        }

        /// <summary>
        /// Delete the existing file with retries.
        /// Move a file with retries.
        /// </summary>
        public static void Replace(string sourceFileName, string destFileName)
        {
            // Remove the old file
            Delete(destFileName);

            // Move the file
            Move(sourceFileName, destFileName);
        }

        /// <summary>
        /// Move a file with retries.
        /// This will not overwrite
        /// </summary>
        public static void Move(string sourceFileName, string destFileName)
        {
            if (sourceFileName == null)
            {
                throw new ArgumentNullException(nameof(sourceFileName));
            }

            if (destFileName == null)
            {
                throw new ArgumentNullException(nameof(destFileName));
            }

            // Run up to 3 times
            for (var i = 0; i < MaxTries; i++)
            {
                // Ignore exceptions for the first attempts
                try
                {
                    File.Move(sourceFileName, destFileName);

                    break;
                }
                catch (Exception ex) when ((i < (MaxTries - 1)) && (ex is UnauthorizedAccessException || ex is IOException))
                {
                    Sleep(100);
                }
            }
        }

        /// <summary>
        /// Delete a file with retries.
        /// </summary>
        public static void Delete(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            // Run up to 3 times
            for (var i = 0; i < MaxTries; i++)
            {
                // Ignore exceptions for the first attempts
                try
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    break;
                }
                catch (Exception ex) when ((i < (MaxTries - 1)) && (ex is UnauthorizedAccessException || ex is IOException))
                {
                    Sleep(100);
                }
            }
        }

        public static T SafeRead<T>(string filePath, Func<FileStream, string, T> read)
        {
            var retries = MaxTries;
            for (var i = 1; i <= retries; i++)
            {
                // Ignore exceptions for the first attempts
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileSharePermissions))
                    {
                        return read(stream, filePath);
                    }
                }
                catch (Exception ex) when ((i < retries) && (ex is UnauthorizedAccessException || ex is IOException))
                {
                    Sleep(100);
                }
            }
            // This will never reached, but the compiler can't detect that 
            return default!;
        }

        public static async Task<T> SafeReadAsync<T>(string filePath, Func<FileStream, string, Task<T>> read)
        {
            var retries = MaxTries;
            for (var i = 1; i <= retries; i++)
            {
                // Ignore exceptions for the first attempts
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileSharePermissions))
                    {
                        return await read(stream, filePath);
                    }
                }
                catch (Exception ex) when ((i < retries) && (ex is UnauthorizedAccessException || ex is IOException))
                {
                    Sleep(100);
                }
            }
            // This will never reached, but the compiler can't detect that 
            return default!;
        }


        private static void Sleep(int ms)
        {
            // Sleep sync
            Thread.Sleep(ms);
        }
    }
}
