// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Protocol.Plugins;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Credentials.Test
{
    public class SecurePluginCredentialProviderBuilderTests : IDisposable
    {
        private readonly TestDirectory _testDirectory;

        public SecurePluginCredentialProviderBuilderTests()
        {
            _testDirectory = TestDirectory.Create();
        }

        public void Dispose()
        {
            _testDirectory.Dispose();
        }

        [Fact]
        public void Constructor_WhenLoggerIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new SecurePluginCredentialProviderBuilder(CreateDefaultPluginManager(), canShowDialog: true, logger: null));
            Assert.Equal("logger", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenPluginManagerIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new SecurePluginCredentialProviderBuilder(pluginManager: null, canShowDialog: true, logger: NullLogger.Instance));
            Assert.Equal("pluginManager", exception.ParamName);
        }

        [Fact]
        public async Task BuildAllAsync_BuildsZero()
        {
            var plugins = new List<KeyValuePair<string, PluginFileState>>();

            using (var pluginManagerBuilder = new PluginManagerBuilderMock(plugins))
            {
                var providerBuilder = new SecurePluginCredentialProviderBuilder(pluginManagerBuilder.PluginManager, canShowDialog: true, logger: NullLogger.Instance);

                var credentialProviders = await providerBuilder.BuildAllAsync();
                Assert.Equal(0, credentialProviders.Count());
            }
        }

        [Fact]
        public async Task BuildAllAsync_BuildsPluginsInCorrectOrder()
        {
            var plugins = new List<KeyValuePair<string, PluginFileState>>();
            plugins.Add(new KeyValuePair<string, PluginFileState>("a", PluginFileState.Valid));
            plugins.Add(new KeyValuePair<string, PluginFileState>("b", PluginFileState.Valid));
            plugins.Add(new KeyValuePair<string, PluginFileState>("c", PluginFileState.Valid));

            using (var pluginManagerBuilder = new PluginManagerBuilderMock(plugins))
            {
                var providerBuilder = new SecurePluginCredentialProviderBuilder(pluginManagerBuilder.PluginManager, canShowDialog: true, logger: NullLogger.Instance);

                var credentialProviders = (await providerBuilder.BuildAllAsync()).ToArray();
                Assert.Equal(3, credentialProviders.Count());
                Assert.StartsWith(nameof(SecurePluginCredentialProvider) + "_a", credentialProviders[0].Id);
                Assert.StartsWith(nameof(SecurePluginCredentialProvider) + "_b", credentialProviders[1].Id);
                Assert.StartsWith(nameof(SecurePluginCredentialProvider) + "_c", credentialProviders[2].Id);
            }
        }

        [Fact]
        public async Task BuildAllAsync_BuildsAllPlugins()
        {
            var plugins = new List<KeyValuePair<string, PluginFileState>>();
            plugins.Add(new KeyValuePair<string, PluginFileState>("a", PluginFileState.Valid));
            plugins.Add(new KeyValuePair<string, PluginFileState>("b", PluginFileState.InvalidEmbeddedSignature));
            plugins.Add(new KeyValuePair<string, PluginFileState>("c", PluginFileState.InvalidFilePath));
            plugins.Add(new KeyValuePair<string, PluginFileState>("d", PluginFileState.NotFound));

            using (var pluginManagerBuilder = new PluginManagerBuilderMock(plugins))
            {
                var providerBuilder = new SecurePluginCredentialProviderBuilder(pluginManagerBuilder.PluginManager, canShowDialog: true, logger: NullLogger.Instance);

                var credentialProviders = (await providerBuilder.BuildAllAsync()).ToArray();
                Assert.Equal(4, credentialProviders.Count());
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task BuildAllAsync_PassesCorrectCanShowDialogValue(bool canShowDialog)
        {
            var plugins = new List<KeyValuePair<string, PluginFileState>>();
            plugins.Add(new KeyValuePair<string, PluginFileState>("a", PluginFileState.Valid));

            using (var pluginManagerBuilder = new PluginManagerBuilderMock(plugins))
            {
                var providerBuilder = new SecurePluginCredentialProviderBuilder(pluginManagerBuilder.PluginManager, canShowDialog, logger: NullLogger.Instance);

                var credentialProviders = (await providerBuilder.BuildAllAsync()).ToArray();
                Assert.Equal(1, credentialProviders.Count());
                var bla = typeof(SecurePluginCredentialProvider).GetField("_canShowDialog", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
                Assert.Equal(canShowDialog, bla.GetValue(credentialProviders.Single()));
            }
        }

        private sealed class PluginManagerBuilderMock : IDisposable
        {
            private readonly TestDirectory _testDirectory;

            internal PluginManager PluginManager { get; }

            internal PluginManagerBuilderMock(List<KeyValuePair<string, PluginFileState>> plugins)
            {
                _testDirectory = TestDirectory.Create();

                var reader = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);

                reader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.PluginPaths)))
                    .Returns(string.Join(";", plugins.Select(e => e.Key)));
                reader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.CorePluginPaths)))
                    .Returns((string)null);
                reader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.DesktopPluginPaths)))
                    .Returns((string)null);
                reader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.RequestTimeout)))
                    .Returns("b");
                reader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.IdleTimeout)))
                    .Returns("c");
                reader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.HandshakeTimeout)))
                    .Returns("d");

                var pluginDiscoverer = new Mock<IPluginDiscoverer>(MockBehavior.Strict);
                var pluginDiscoveryResults = GetPluginDiscoveryResults(plugins);

                pluginDiscoverer.Setup(x => x.DiscoverAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(pluginDiscoveryResults);

                PluginManager = new PluginManager(
                    reader.Object,
                    new Lazy<IPluginDiscoverer>(() => pluginDiscoverer.Object),
                    (TimeSpan idleTimeout) => Mock.Of<PluginFactory>(),
                    new Lazy<string>(() => _testDirectory.Path));
            }

            public void Dispose()
            {
                _testDirectory.Dispose();
            }

            private static IEnumerable<PluginDiscoveryResult> GetPluginDiscoveryResults(List<KeyValuePair<string, PluginFileState>> plugins)
            {
                var results = new List<PluginDiscoveryResult>();
                foreach (var plugin in plugins)
                {
                    var file = new PluginFile(plugin.Key, new Lazy<PluginFileState>(() => plugin.Value));
                    results.Add(new PluginDiscoveryResult(file));
                }

                return results;
            }
        }

        private PluginManager CreateDefaultPluginManager()
        {
            return new PluginManager(
                Mock.Of<IEnvironmentVariableReader>(),
                new Lazy<IPluginDiscoverer>(),
                (TimeSpan idleTimeout) => Mock.Of<PluginFactory>(),
                new Lazy<string>(() => _testDirectory.Path));
        }
    }
}
