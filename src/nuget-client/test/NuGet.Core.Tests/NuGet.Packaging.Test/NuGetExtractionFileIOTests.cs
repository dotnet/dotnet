// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class NuGetExtractionFileIOTests
    {
        // On Unix, a file descriptor is inherited by any process forked/exec'd while it's open,
        // unless the descriptor is marked close-on-exec. For example, if MSBuild spawns a new
        // worker node while NuGet is still writing an extracted file, that worker node inherits
        // NuGet's open, writable file descriptor for it. Even after NuGet closes its own handle
        // once extraction finishes, the copy of the descriptor inherited by the worker node keeps
        // the file open for writing, so the kernel refuses to execute the file - failing with
        // ETXTBSY ("Text file busy") - until the worker node closes its handle or exits.
        [PlatformFact(Platform.Darwin, Platform.Linux)]
        public void CreateFile_ExtractedFileExecutedAfterConcurrentProcessStart_DoesNotFailWithTextFileBusy()
        {
            using TestDirectory testDirectory = TestDirectory.Create();
            string filePath = Path.Combine(testDirectory, Guid.NewGuid().ToString());
            Process? concurrentProcess = null;

            try
            {
                using (FileStream fileStream = NuGetExtractionFileIO.CreateFile(filePath))
                {
                    // Write a minimal, valid, executable script while the file is still open for
                    // writing, just like NuGet writes package content during extraction.
                    byte[] scriptBytes = Encoding.ASCII.GetBytes("#!/bin/sh\nexit 0\n");
                    fileStream.Write(scriptBytes, 0, scriptBytes.Length);
                    fileStream.Flush();

                    // Simulate another process being forked/exec'd concurrently with extraction
                    // (e.g. a build tool spawning a worker process) while NuGet's file descriptor
                    // is still open.
                    concurrentProcess = Process.Start(new ProcessStartInfo("sleep", "5")
                    {
                        UseShellExecute = false
                    }) ?? throw new InvalidOperationException("Failed to start the concurrent process.");
                }

                // NuGet has now finished extracting the file and closed its own descriptor, exactly
                // as it would once package extraction completes.

                // Executing the file should succeed immediately. It must not fail with ETXTBSY just
                // because the unrelated, still-running process above happened to start while the file
                // was being written.
                using Process executeProcess = Process.Start(new ProcessStartInfo(filePath)
                {
                    UseShellExecute = false
                }) ?? throw new InvalidOperationException("Failed to start the process to execute the extracted file.");

                Assert.True(executeProcess.WaitForExit(10_000), "Executing the extracted file did not complete in time.");
                Assert.Equal(0, executeProcess.ExitCode);
            }
            catch (Win32Exception ex) when (ex.NativeErrorCode == ETXTBSY)
            {
                Assert.Fail(
                    "Executing the extracted file failed with ETXTBSY (Text file busy) because a concurrently " +
                    "started, unrelated process inherited a writable file descriptor for it. " +
                    "See https://github.com/dotnet/dotnet/issues/8079.");
            }
            finally
            {
                if (concurrentProcess != null)
                {
                    if (!concurrentProcess.HasExited)
                    {
                        concurrentProcess.Kill();
                    }

                    concurrentProcess.Dispose();
                }
            }
        }

        private const int ETXTBSY = 26;
    }
}
