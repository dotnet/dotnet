// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test.PackageExtraction
{
    public class ZipArchiveExtensionsTests
    {
        // Trying to change a file timestamp when the file is open only throws on Windows
        [PlatformFact(Platform.Windows)]
        public void UpdateFileTimeFromEntry_FileBusyForShortTime_Retries()
        {
            // Arrange
            string tempFile = Path.GetTempFileName();
            using CancellationTokenSource cts = new();
            FileStream fileStream = null;
            try
            {
                // At the time this test was written, UpdateFileTimeFromEntry's retry delay is exponential, with the max retries waiting
                // Math.Pow(2, MaxRetries) - 1 milliseconds. Given this test depends on timing of multiple tasks that are not synchronised,
                // this test is at high risk of being flakey, especially when tests are run in parallel, meaning the CPU is busy and might
                // not run continuations as soon as they're ready. Therefore, use 13 retries so retries keep happening for about 8.2 seconds
                // which should be plenty given the simulated AV locks the file for only 5 milliseconds + task scheduling latency.
                Mock<IEnvironmentVariableReader> environmentVariableReader = new Mock<IEnvironmentVariableReader>();
                environmentVariableReader.Setup(x => x.GetEnvironmentVariable("NUGET_UPDATEFILETIME_MAXRETRIES"))
                    .Returns("13");
                ZipArchiveExtensions.Testable zipArchiveExtensions = new ZipArchiveExtensions.Testable(environmentVariableReader.Object);

                using MemoryStream memoryStream = new();
                using ZipArchive zipArchive = new(memoryStream, ZipArchiveMode.Create);

                ZipArchiveEntry zipEntry = zipArchive.CreateEntry("test.file");
                DateTime expectedTime = DateTime.UtcNow.AddHours(-5);
                zipEntry.LastWriteTime = expectedTime;

                fileStream = File.Open(tempFile, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete);
                Task avSimulation = Task.Run(async () =>
                {
                    await Task.Delay(5, cts.Token);
                    fileStream.Dispose();
                });

                // Act
                zipArchiveExtensions.UpdateFileTimeFromEntry(zipEntry, tempFile, NullLogger.Instance);

                // Assert
                DateTime fileLastWriteTime = File.GetLastWriteTimeUtc(tempFile);
                Assert.Equal(expectedTime, fileLastWriteTime);
            }
            finally
            {
                fileStream?.Dispose();
                cts.Cancel();
                File.Delete(tempFile);
            }
        }
#if !NET8_0_OR_GREATER
        // Trying to change a file timestamp when the file is open only throws on Windows
        [PlatformFact(Platform.Windows)]
        public async Task UpdateFileTimeFromEntry_FileBusyForLongTime_Throws()
        {
            // Arrange
            string tempFile = Path.GetTempFileName();
            try
            {
                Mock<IEnvironmentVariableReader> environmentVariableReader = new Mock<IEnvironmentVariableReader>();
                ZipArchiveExtensions.Testable zipArchiveExtensions = new ZipArchiveExtensions.Testable(environmentVariableReader.Object);

                using MemoryStream memoryStream = new();
                using ZipArchive zipArchive = new(memoryStream, ZipArchiveMode.Create);

                ZipArchiveEntry zipEntry = zipArchive.CreateEntry("test.file");
                DateTime expectedTime = DateTime.UtcNow.AddHours(-5);
                zipEntry.LastWriteTime = expectedTime;

                using FileStream fileStream = File.Open(tempFile, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete);

                // Act & Assert
                await Assert.ThrowsAsync<IOException>(async () =>
                {
                    Task task = Task.Run(() => zipEntry.UpdateFileTimeFromEntry(tempFile, NullLogger.Instance));

                    // If the above deadlocks until the file is unlocked, then we need another way to fail the test within a reasonable time.
                    using CancellationTokenSource cts = new();
                    Task delay = Task.Delay(TimeSpan.FromSeconds(10), cts.Token);
                    var completed = await Task.WhenAny(task, delay);
                    cts.Cancel();
                    if (completed == delay)
                    {
                        throw new TimeoutException();
                    }
                    await completed;
                });
            }
            finally
            {
                File.Delete(tempFile);
            }
        }
#endif
    }
}
