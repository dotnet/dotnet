// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using NuGet.Common;

#if NETFRAMEWORK || NETSTANDARD2_0
using System.Buffers;
#endif

namespace NuGet.Packaging
{
    public static class StreamExtensions
    {
        public static string CopyToFile(this Stream inputStream, string fileFullPath)
        {
            return Testable.Default.CopyToFile(inputStream, fileFullPath);
        }

        private static void CopyTo(Stream inputStream, Stream outputStream)
        {
            // .NET Framework allocates an unavoidable byte[] when using
            // Stream.CopyTo. Reimplement it, pulling from the pool similar
            // to .NET 5.

#if NETFRAMEWORK || NETSTANDARD2_0
            const int bufferSize = 81920; // Same as Stream.CopyTo
            byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize);

            int bytesRead;
            while ((bytesRead = inputStream.Read(buffer, offset: 0, buffer.Length)) != 0)
            {
                outputStream.Write(buffer, offset: 0, bytesRead);
            }

            ArrayPool<byte>.Shared.Return(buffer);
#else
            inputStream.CopyTo(outputStream);
#endif
        }

        internal class Testable
        {
            // Only files smaller than this value will be mmap'ed
            private const long MAX_MMAP_SIZE = 10 * 1024 * 1024;

            // Mmap can improve file writing performance, but it can make it slower too.
            // It all depends on a particular hardware configuration, operating system or anti-virus software.
            // From our benchmarks we concluded that mmap is a good choice for Windows,
            // but it is not so for other systems.
            //
            // 1 - always use memory-mapped files
            // 0 - never use memory-mapped files
            // default - use memory-mapped files on Windows, but not on other systems
            private const string MMAP_VARIABLE_NAME = "NUGET_PACKAGE_EXTRACTION_USE_MMAP";

            private bool _isMMapEnabled { get; }

            internal Testable(IEnvironmentVariableReader environmentVariableReader)
            {
                _isMMapEnabled = environmentVariableReader.GetEnvironmentVariable(MMAP_VARIABLE_NAME) switch
                {
                    "0" => false,
                    "1" => true,
                    _ => RuntimeEnvironmentHelper.IsWindows
                };
            }

            public static Testable Default { get; } = new Testable(EnvironmentVariableWrapper.Instance);

            public string CopyToFile(Stream inputStream, string fileFullPath)
            {
                if (Path.GetFileName(fileFullPath).Length == 0)
                {
                    Directory.CreateDirectory(fileFullPath);
                    return fileFullPath;
                }

                var directory = Path.GetDirectoryName(fileFullPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (File.Exists(fileFullPath))
                {
                    // Log and skip adding file
                    return fileFullPath;
                }

                // For files of a certain size, we can do some Cleverness and mmap
                // them instead of writing directly to disk. This can improve
                // performance by a lot on some operating systems and hardware,
                // particularly Windows
                long? size = null;
                try
                {
                    size = inputStream.Length;
                }
                catch (NotSupportedException)
                {
                    // If we can't get Length, just move on.
                }

                if (_isMMapEnabled && size > 0 && size <= MAX_MMAP_SIZE)
                {
                    MmapCopy(inputStream, fileFullPath, size.Value);
                }
                else
                {
                    FileStreamCopy(inputStream, fileFullPath);
                }

                return fileFullPath;
            }

            internal virtual void MmapCopy(Stream inputStream, string fileFullPath, long size)
            {
                using (var outputStream = NuGetExtractionFileIO.CreateFile(fileFullPath))
                {
                    // NOTE: Linux can't create a mmf from outputStream, so we
                    // need to close the file (which now has the desired
                    // perms), and then re-open it as a memory-mapped file.
                    outputStream.Dispose();
                    using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(fileFullPath, FileMode.Open, mapName: null, size))
                    using (MemoryMappedViewStream mmstream = mmf.CreateViewStream())
                    {
                        CopyTo(inputStream, mmstream);
                    }
                }
            }

            internal virtual void FileStreamCopy(Stream inputStream, string fileFullPath)
            {
                using (var outputStream = NuGetExtractionFileIO.CreateFile(fileFullPath))
                {
                    CopyTo(inputStream, outputStream);
                }
            }
        }
    }
}
