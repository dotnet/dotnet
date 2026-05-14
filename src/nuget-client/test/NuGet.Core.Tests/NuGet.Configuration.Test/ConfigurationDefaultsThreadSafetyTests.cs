// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    // Since these tests try to hit multi-threaded timing bugs, ensure other tests are not running in prallel.
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class ConfigurationDefaultsThreadSafetyTests
    {
        [Fact]
        public void DefaultPackageSources_CalledConcurrently_ReturnsCorrectPackageSources()
        {
            // Arrange
            using SimpleTestPathContext testContext = new();

            var packageSource1 = Path.Combine(testContext.WorkingDirectory, "1");
            var packageSource2 = Path.Combine(testContext.WorkingDirectory, "2");
            var packageSource3 = Path.Combine(testContext.WorkingDirectory, "3");

            testContext.Settings.AddSource("1", packageSource1);
            testContext.Settings.AddSource("2", packageSource2);
            testContext.Settings.AddSource("3", packageSource3);

            var expectedPackageSources = new[]
            {
                testContext.PackageSource,
                packageSource1,
                packageSource2,
                packageSource3
            };

            for (int attempt = 0; attempt < 10; attempt++)
            {
                ConfigurationDefaults configurationDefaults = new(Path.GetDirectoryName(testContext.NuGetConfig)!, Path.GetFileName(testContext.NuGetConfig));

                // Act
                RunInParallel(EnumerateDefaultSources, attempt);

                // Assert
                configurationDefaults.DefaultPackageSources.Count().Should().Be(expectedPackageSources.Length);

                configurationDefaults.DefaultPackageSources.Select(s => s.Source).Should().BeEquivalentTo(expectedPackageSources);

                void EnumerateDefaultSources()
                {
                    IEnumerable<PackageSource> sources = configurationDefaults.DefaultPackageSources;
                    foreach (PackageSource source in sources)
                    {
                    }
                }
            }
        }

        [Fact]
        public void DefaultPushSource_CalledConcurrently_ReturnsCorrectPackageSource()
        {
            for (int attempt = 0; attempt < 10; attempt++)
            {
                // Arrange
                using SimpleTestPathContext testContext = new();
                testContext.Settings.SetDefaultPushSource(testContext.PackageSource);

                ConfigurationDefaults configurationDefaults = new(Path.GetDirectoryName(testContext.NuGetConfig)!, Path.GetFileName(testContext.NuGetConfig));

                // Act
                RunInParallel(CheckDefaultPushSource, attempt);

                // Assert
                configurationDefaults.DefaultPushSource.Should().Be(testContext.PackageSource);

                void CheckDefaultPushSource()
                {
                    string? source = configurationDefaults.DefaultPushSource;
                    if (source == null)
                    {
                        throw new Exception("Default package source is null");
                    }
                }
            }
        }

        private void RunInParallel(Action action, int attempt)
        {
            ConcurrentQueue<Exception> exceptions = new();
            ManualResetEventSlim resetEvent = new(initialState: false);
            Thread[] threads = new Thread[Environment.ProcessorCount];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(Run);
                threads[i].Start();
            }

            resetEvent.Set();
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }

            if (exceptions.TryDequeue(out Exception? ex))
            {
                throw new Exception("At least one thread did not complete successfully on attempt " + (attempt + 1), ex);
            }

            void Run()
            {
                try
                {
                    resetEvent.Wait();
                    action();
                }
                catch (Exception ex)
                {
                    exceptions.Enqueue(ex);
                }
            }
        }
    }
}
