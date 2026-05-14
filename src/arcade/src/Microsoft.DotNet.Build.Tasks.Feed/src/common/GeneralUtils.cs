// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Arcade.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
#if !DOTNET_BUILD_SOURCE_ONLY
using Microsoft.DotNet.Build.CloudTestTasks;
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public static class GeneralUtils
    {
        public const string SymbolPackageSuffix = ".symbols.nupkg";
        public const string SnupkgPackageSuffix = ".snupkg";
        public const string PackageSuffix = ".nupkg";
        public const string PackagesCategory = "PACKAGE";
        public static TimeSpan NugetFeedPublisherHttpClientTimeout => TimeSpan.FromSeconds(300);

        public static ExponentialRetry CreateDefaultRetryHandler()
            => new ExponentialRetry
            {
                DelayBase = 5,
                MaxAttempts = 5
            };

        /// <summary>
        ///     Compare a local stream and a remote stream for quality
        /// </summary>
        /// <param name="localFileStream">Local stream</param>
        /// <param name="remoteStream">Remote stream</param>
        /// <param name="bufferSize">Buffer to keep around</param>
        /// <returns>True if the streams are equal, false otherwise.</returns>
        public static async Task<bool> CompareStreamsAsync(Stream localFileStream, Stream remoteStream, int bufferSize)
        {
            byte[] localBuffer = new byte[bufferSize];
            byte[] remoteBuffer = new byte[bufferSize];
            int localBufferWriteOffset = 0;
            int remoteBufferWriteOffset = 0;
            int localBufferReadOffset = 0;
            int remoteBufferReadOffset = 0;

            do
            {
                int localBytesToRead = bufferSize - localBufferWriteOffset;
                int remoteBytesToRead = bufferSize - remoteBufferWriteOffset;

                int bytesRemoteFile = 0;
                int bytesLocalFile = 0;
                if (remoteBytesToRead > 0)
                {
                    bytesRemoteFile = await remoteStream.ReadAsync(remoteBuffer, remoteBufferWriteOffset, remoteBytesToRead);
                }

                if (localBytesToRead > 0)
                {
                    bytesLocalFile = await localFileStream.ReadAsync(localBuffer, localBufferWriteOffset, localBytesToRead);
                }

                int bytesLocalAvailable = bytesLocalFile + (localBufferWriteOffset - localBufferReadOffset);
                int bytesRemoteAvailable = bytesRemoteFile + (remoteBufferWriteOffset - remoteBufferReadOffset);
                int minBytesAvailable = Math.Min(bytesLocalAvailable, bytesRemoteAvailable);

                if (minBytesAvailable == 0)
                {
                    // If there is nothing left to compare (EOS), then good to go.
                    // Otherwise, one stream reached EOS before the other.
                    return bytesLocalFile == bytesRemoteFile;
                }

                // Compare the minimum number of bytes between the two streams, starting at the offset,
                // then advance the offsets for the next pass
                for (int i = 0; i < minBytesAvailable; i++)
                {
                    if (remoteBuffer[remoteBufferReadOffset + i] != localBuffer[localBufferReadOffset + i])
                    {
                        return false;
                    }
                }

                // Advance the offsets. The read offset gets advanced by the amount that we actually compared,
                // While the write offset gets advanced by the amount each of the streams returned.
                localBufferReadOffset += minBytesAvailable;
                remoteBufferReadOffset += minBytesAvailable;

                localBufferWriteOffset += bytesLocalFile;
                remoteBufferWriteOffset += bytesRemoteFile;

                if (localBufferReadOffset == bufferSize)
                {
                    localBufferReadOffset = 0;
                    localBufferWriteOffset = 0;
                }

                if (remoteBufferReadOffset == bufferSize)
                {
                    remoteBufferReadOffset = 0;
                    remoteBufferWriteOffset = 0;
                }
            }
            while (true);
        }

        /// <summary>
        ///     Determine whether the feed is public or private.
        /// </summary>
        /// <param name="feedUrl">Feed url to test</param>
        /// <returns>True if the feed is public, false if it is private, and null if it was not possible to determine.</returns>
        /// <remarks>
        /// Do an unauthenticated GET on the feed URL. If it succeeds, the feed is not public.
        /// If it fails with a 4* error, assume it is internal.
        /// </remarks>
        public static Task<bool?> IsFeedPublicAsync(
            string feedUrl,
            HttpClient httpClient,
            TaskLoggingHelper log)
        {
            return IsFeedPublicAsync(feedUrl, httpClient, log, CreateDefaultRetryHandler());
        }

        /// <summary>
        ///     Determine whether the feed is public or private.
        /// </summary>
        /// <param name="feedUrl">Feed url to test</param>
        /// <returns>True if the feed is public, false if it is private, and null if it was not possible to determine.</returns>
        /// <remarks>
        /// Do an unauthenticated GET on the feed URL. If it succeeds, the feed is not public.
        /// If it fails with a 4* error, assume it is internal.
        /// </remarks>
        public static async Task<bool?> IsFeedPublicAsync(
            string feedUrl,
            HttpClient httpClient,
            TaskLoggingHelper log,
            IRetryHandler retryHandler)
        {
            bool? isPublic = null;

            bool success = await retryHandler.RunAsync(async attempt =>
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(feedUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        isPublic = true;
                        return true;
                    }
                    else if (response.StatusCode >= (System.Net.HttpStatusCode)400 &&
                              response.StatusCode < (System.Net.HttpStatusCode)500)
                    {
                        isPublic = false;
                        return true;
                    }
                    else
                    {
                        // Don't know for certain, retry
                        return false;
                    }
                }
                catch (Exception e)
                {
                    log.LogMessage(MessageImportance.Low, $"Unexpected exception {e.Message} when attempting to determine whether feed is internal.");
                    return false;
                }
            });

            if (!success)
            {
                // We couldn't determine anything.  We'd be unlikely to be able to push to this feed either,
                // since it's 5xx'ing.
                log.LogError($"Unable to determine whether '{feedUrl}' is public or internal.");
            }

            return isPublic;
        }

        /// <summary>
        ///     Infers the category based on the extension of the particular asset
        ///     
        ///     If no category can be inferred, then "OTHER" is used.
        /// </summary>
        /// <param name="assetId">ID of asset</param>
        /// <returns>Asset cateogry</returns>
        public static string InferCategory(string assetId, TaskLoggingHelper log)
        {
            var extension = Path.GetExtension(assetId).ToUpper();

            var whichCategory = new Dictionary<string, string>()
            {
                { ".NUPKG", PackagesCategory },
                { ".PKG", "OSX" },
                { ".DEB", "DEB" },
                { ".RPM", "RPM" },
                { ".NPM", "NODE" },
                { ".ZIP", "BINARYLAYOUT" },
                { ".MSI", "INSTALLER" },
                { ".SHA", "CHECKSUM" },
                { ".SHA512", "CHECKSUM" },
                { ".POM", "MAVEN" },
                { ".VSIX", "VSIX" },
                { ".CAB", "BINARYLAYOUT" },
                { ".TAR", "BINARYLAYOUT" },
                { ".GZ", "BINARYLAYOUT" },
                { ".TGZ", "BINARYLAYOUT" },
                { ".EXE", "INSTALLER" },
                { ".SVG", "BADGE"},
                { ".WIXLIB", "INSTALLER" },
                { ".WIXPDB", "INSTALLER" },
                { ".JAR", "INSTALLER" },
                { ".VERSION", "INSTALLER"},
                { ".SWR", "INSTALLER" }
            };

            if (whichCategory.TryGetValue(extension, out var category))
            {
                // Special handling for symbols.nupkg. There are typically plenty of
                // periods in package names. We get the extension to identify nupkg
                // assets. But symbol packages have the extension '.symbols.nupkg'.
                // We want to divide these into a separate category because for stabilized builds,
                // they should go to an isolated location. In a non-stabilized build, they can go straight
                // to blob feeds because the blob feed push tasks will automatically push them to the assets.
                if (assetId.EndsWith(SymbolPackageSuffix, StringComparison.OrdinalIgnoreCase))
                {
                    return "SYMBOLS";
                }
                return category;
            }
            else
            {
                log.LogMessage(MessageImportance.High, $"Defaulting to category 'OTHER' for asset {assetId}");
                return "OTHER";
            } 
        }

        /// <summary>
        ///     Determine whether a file name or path is a symbol package.
        /// </summary>
        /// <param name="name">File anme or path</param>
        /// <returns>True if the item is a symbol package, false otherwise</returns>
        public static bool IsSymbolPackage(string name)
        {
            return name.EndsWith(SymbolPackageSuffix, StringComparison.OrdinalIgnoreCase) ||
                name.EndsWith(SnupkgPackageSuffix, StringComparison.OrdinalIgnoreCase);
        }

        private static System.Threading.Tasks.Task WaitForProcessExitAsync(Process process)
        {
            return System.Threading.Tasks.Task.Run(() => process.WaitForExit());
        }

        /// <summary>
        ///   Run, and wait on a process synchronously, returning its full console output and exit code
        /// </summary>
        /// <param name="path">Path to process</param>
        /// <param name="arguments">Process arguments</param>
        /// <returns>Process return code</returns>
        public static async Task<ProcessExecutionResult> RunProcessAndGetOutputsAsync(string path, string arguments)
        {
            ProcessStartInfo info = new ProcessStartInfo(path, arguments)
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            Process process = new Process
            {
                StartInfo = info,
                EnableRaisingEvents = true
            };

            var stdOut = new StringBuilder();
            var stdErr = new StringBuilder();
            var stdoutCompletion = new TaskCompletionSource<bool>();
            var stderrCompletion = new TaskCompletionSource<bool>();

            process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    lock (stdOut)
                    {
                        stdOut.AppendLine(e.Data);
                    }
                }
                else
                {
                    stdoutCompletion.TrySetResult(true);
                }

            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    lock (stdErr)
                    {
                        stdErr.AppendLine(e.Data);
                    }
                }
                else
                {
                    stderrCompletion.TrySetResult(true);
                }
            };

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            
            // Creates task to wait for process exit using timeout
            await WaitForProcessExitAsync(process);

            // Wait for the last outputs to flush before returning

            System.Threading.Tasks.Task.WaitAll(new System.Threading.Tasks.Task[] { stderrCompletion.Task, stdoutCompletion.Task }, TimeSpan.FromSeconds(5));
            return new ProcessExecutionResult()
            {
                ExitCode = process.ExitCode,
                StandardOut = stdOut.ToString(),
                StandardError = stdErr.ToString()
            };
        }

        public class ProcessExecutionResult
        {
            public int ExitCode;
            public string StandardOut;
            public string StandardError;
        }
    }
}
