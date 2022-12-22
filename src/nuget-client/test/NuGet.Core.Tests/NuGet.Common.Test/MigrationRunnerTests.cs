// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NuGet.Common.Migrations;
using Xunit;

namespace NuGet.Common.Test
{
    [CollectionDefinition("MigrationRunner", DisableParallelization = true)]
    public class MigrationRunnerTests
    {
        [Fact]
        public void Run_WhenExecutedOnSingleThreadThenOneMigrationFileIsCreated_Success()
        {
            // Arrange
            string directory = MigrationRunner.GetMigrationsDirectory();
            if (Directory.Exists(directory))
                Directory.Delete(path: directory, recursive: true);

            // Act
            MigrationRunner.Run();

            // Assert
            Assert.True(Directory.Exists(directory));
            var files = Directory.GetFiles(directory);
            Assert.Equal(1, files.Length);
            Assert.Equal(Path.Combine(directory, "1"), files[0]);
        }

        [Fact]
        public void Run_WhenExecutedInParallelThenOnlyOneMigrationFileIsCreated_Success()
        {
            var threads = new List<Thread>();
            int numThreads = 5;
            int timeoutInSeconds = 90;

            // Arrange
            string directory = MigrationRunner.GetMigrationsDirectory();
            if (Directory.Exists(directory))
                Directory.Delete(path: directory, recursive: true);

            // Act
            for (int i = 0; i < numThreads; i++)
            {
                var thread = new Thread(MigrationRunner.Run);
                thread.Start();
                threads.Add(thread);
            }

            foreach (var thread in threads)
            {
                thread.Join(timeout: TimeSpan.FromSeconds(timeoutInSeconds));
            }

            // Assert
            Assert.True(Directory.Exists(directory));
            var files = Directory.GetFiles(directory);
            Assert.Equal(1, files.Length);
            Assert.Equal(Path.Combine(directory, "1"), files[0]);
        }
    }
}
