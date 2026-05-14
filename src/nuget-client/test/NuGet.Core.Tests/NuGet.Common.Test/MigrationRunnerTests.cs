// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using FluentAssertions;
using NuGet.Common.Migrations;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.Common.Test
{
    public class MigrationRunnerTests
    {
        [Fact]
        public void Run_WhenExecutedOnSingleThreadThenOneMigrationFileIsCreated_Success()
        {
            // Arrange
            using var testDirectory = TestDirectory.Create();

            string migrationsDirectory = Path.Combine(testDirectory, "migrations");

            // Act
            MigrationRunner.Run(migrationsDirectory, TestEnvironmentVariableReader.EmptyInstance);

            // Assert
            AssertSingleMigrationDirectory(migrationsDirectory);
        }

        [Fact]
        public void Run_WhenExecutedInParallelThenOnlyOneMigrationFileIsCreated_Success()
        {
            var threads = new List<Thread>();
            const int numThreads = 5;
            TimeSpan timeout = TimeSpan.FromSeconds(90);

            // Arrange
            using var testDirectory = TestDirectory.Create();

            string migrationsDirectory = Path.Combine(testDirectory, "migrations");

            // Act
            for (int i = 0; i < numThreads; i++)
            {
                var thread = new Thread(() => MigrationRunner.Run(migrationsDirectory, TestEnvironmentVariableReader.EmptyInstance));
                thread.Start();
                threads.Add(thread);
            }

            foreach (var thread in threads)
            {
                thread.Join(timeout);
            }

            // Assert
            AssertSingleMigrationDirectory(migrationsDirectory);
        }

        [Fact]
        public void Run_WhenAThreadAbandonsMutexThenNextMigrationRunReleasesMutexAndCreatesMigrationFile_Success()
        {
            Mutex _orphan = new Mutex(false, "NuGet-Migrations");
            bool signal = false;

            // Arrange
            using var testDirectory = TestDirectory.Create();

            string migrationsDirectory = Path.Combine(testDirectory, "migrations");

            Thread t = new Thread(new ThreadStart(AbandonMutex));
            t.Start();
            t.Join();
            signal.Should().BeTrue(because: "A mutex should have been acquired.");

            // Act
            MigrationRunner.Run(migrationsDirectory, TestEnvironmentVariableReader.EmptyInstance);

            // Assert
            AssertSingleMigrationDirectory(migrationsDirectory);


            void AbandonMutex()
            {
                signal = _orphan.WaitOne(TimeSpan.FromMinutes(1), false);
                // Abandon the mutex by exiting the method without releasing
            }
        }

        private void AssertSingleMigrationDirectory(string migrationsDirectory)
        {
            Directory.Exists(migrationsDirectory).Should().BeTrue();
            string[] files = Directory.GetFiles(migrationsDirectory);
            files.Length.Should().Be(1);
            files[0].Should().Be(Path.Combine(migrationsDirectory, "1"));
        }
    }
}
