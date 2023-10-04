// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class SettingsLoadingContextTests
    {
        /// <summary>
        /// Verifies that <see cref="SettingsLoadingContext.GetOrCreateSettingsFile(string, bool, bool)" /> can read and cache settings files when they already exist on disk.
        /// </summary>
        [Fact]
        public void GetOrCreateSettingsFile_OnlyReadsFileOnce_WhenAlreadyExists()
        {
            using var testPathContext = new SimpleTestPathContext();
            using var settingsLoadingContext = new SettingsLoadingContext();

            ConcurrentBag<string> filesThatWereRead = new ConcurrentBag<string>();

            settingsLoadingContext.FileRead += (_, filePath) => filesThatWereRead.Add(filePath);

            Parallel.For(0, 10, (i) =>
            {
                SettingsFile settingsFile = settingsLoadingContext.GetOrCreateSettingsFile(testPathContext.NuGetConfig);

                settingsFile.ConfigFilePath.Should().Be(testPathContext.NuGetConfig);
            });

            filesThatWereRead
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .Be(testPathContext.NuGetConfig);
        }

        /// <summary>
        /// Verifies that <see cref="SettingsLoadingContext.GetOrCreateSettingsFile(string, bool, bool)" /> can create and cache a settings file when one does not exist already.
        /// </summary>
        [Fact]
        public void GetOrCreateSettingsFile_OnlyReadsFileOnce_WhenDoesNotExists()
        {
            using var testPathContext = new SimpleTestPathContext();
            using var settingsLoadingContext = new SettingsLoadingContext();

            File.Delete(testPathContext.NuGetConfig);

            ConcurrentBag<string> filesThatWereRead = new ConcurrentBag<string>();

            settingsLoadingContext.FileRead += (_, filePath) => filesThatWereRead.Add(filePath);

            Parallel.For(0, 10, (i) =>
            {
                SettingsFile settingsFile = settingsLoadingContext.GetOrCreateSettingsFile(testPathContext.NuGetConfig);

                settingsFile.ConfigFilePath.Should().Be(testPathContext.NuGetConfig);
            });

            filesThatWereRead
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .Be(testPathContext.NuGetConfig);
        }

        /// <summary>
        /// Verifies that <see cref="SettingsLoadingContext.GetOrCreateSettingsFile(string, bool, bool)" /> throws an <see cref="NuGetConfigurationException"/> when a settings file is unreadable.
        /// </summary>
        [Fact]
        public void GetOrCreateSettingsFile_ThrowNuGetConfigurationException_WhenConfigIsUnreadable()
        {
            using var testPathContext = new SimpleTestPathContext();

            using var settingsLoadingContext = new SettingsLoadingContext();

            File.WriteAllText(testPathContext.NuGetConfig, string.Empty);

            Action action = () => _ = settingsLoadingContext.GetOrCreateSettingsFile(testPathContext.NuGetConfig);

            action.Should()
                .Throw<NuGetConfigurationException>()
                .Which
                .Message
                .Should()
                .Contain("NuGet.Config is not valid XML");
        }

        /// <summary>
        /// Verifies that <see cref="SettingsLoadingContext.GetOrCreateSettingsFile(string, bool, bool)" /> throws an <see cref="ArgumentNullException"/> when passed a <see langword="null" /> value for the file path.
        /// </summary>
        [Fact]
        public void GetOrCreateSettingsFile_ThrowsArgumentNullException_WhenFilePathIsNull()
        {
            var settingsLoadingContext = new SettingsLoadingContext();

            Action action = () => settingsLoadingContext.GetOrCreateSettingsFile(filePath: null);

            action.Should()
                .Throw<ArgumentNullException>()
                .Which
                .ParamName
                .Should()
                .Be("filePath");
        }

        /// <summary>
        /// Verifies that <see cref="SettingsLoadingContext.GetOrCreateSettingsFile(string, bool, bool)" /> throws an <see cref="ObjectDisposedException"/> when it has been disposed.
        /// </summary>
        [Fact]
        public void GetOrCreateSettingsFile_ThrowsObjectDisposedException_WhenDisposed()
        {
            var settingsLoadingContext = new SettingsLoadingContext();

            settingsLoadingContext.Dispose();

            Action action = () => settingsLoadingContext.GetOrCreateSettingsFile(filePath: null);

            action.Should()
                .Throw<ObjectDisposedException>()
                .Which
                .ObjectName
                .Should()
                .Be(nameof(SettingsLoadingContext));
        }

        /// <summary>
        /// Verifies that <see cref="SettingsLoadingContext.GetOrCreateSettingsFile(string, bool, bool)" /> throws an <see cref="ArgumentException"/> when a non-rooted path is specified (ie just a filename).
        /// </summary>
        [Fact]
        public void GetOrCreateSettingsFile_ThrowsArgumentException_WhenNonRootedPathSpecified()
        {
            var settingsLoadingContext = new SettingsLoadingContext();

            Action action = () => settingsLoadingContext.GetOrCreateSettingsFile(filePath: "Something.config");

            action.Should()
                .Throw<ArgumentException>()
                .Which
                .ParamName
                .Should()
                .Be("filePath");
        }
    }
}
