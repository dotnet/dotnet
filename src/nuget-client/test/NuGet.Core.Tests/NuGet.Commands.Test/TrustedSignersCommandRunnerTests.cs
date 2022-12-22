// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Configuration.Test;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;
using static NuGet.Commands.TrustedSignersArgs;

namespace NuGet.Commands.Test
{
    public class TrustedSignersCommandRunnerTests
    {
        [Fact]
        public async Task ExecuteCommandAsync_ListAction_ShowsListOfTrustedSignersAsync()
        {
            // Arrange
            var trustedSignersProvider = new Mock<ITrustedSignersProvider>();
            trustedSignersProvider
                .Setup(p => p.GetTrustedSigners())
                .Returns(new List<TrustedSignerItem>()
                {
                    new AuthorItem("author", new CertificateItem("abc", HashAlgorithmName.SHA256, allowUntrustedRoot: true)),
                    new RepositoryItem("repo", "https://serviceindex.test/v3/index.json", new CertificateItem("def", HashAlgorithmName.SHA384))
                });

            using (var test = Test.Create(TrustedSignersAction.List, trustedSignersProvider.Object))
            {
                // Act
                var result = await test.Runner.ExecuteCommandAsync(test.Args);
                result.Should().Be(0);

                var logs = test.Logger.Messages.ToList();
                logs[0].Should().Contain("Registered trusted signers:");
                logs[2].Should().Contain("author [author]");
                logs[2].Should().Contain("[U] SHA256 - abc");
                logs[3].Should().Contain("repo [repository]");
                logs[3].Should().Contain("Service Index: https://serviceindex.test/v3/index.json");
                logs[3].Should().Contain("SHA384 - def");
            }
        }

        [Fact]
        public async Task ExecuteCommandAsync_RemoveAction_WithoutTrustedSigners_LogsAndSucceedsAsync()
        {
            // Arrange
            var trustedSignersProvider = new Mock<ITrustedSignersProvider>();
            trustedSignersProvider
                .Setup(p => p.GetTrustedSigners())
                .Returns(new List<TrustedSignerItem>()
                {
                });

            using (var test = Test.Create(TrustedSignersAction.Remove, trustedSignersProvider.Object))
            {
                test.Args.Name = "signer";

                // Act
                var result = await test.Runner.ExecuteCommandAsync(test.Args);
                result.Should().Be(0);

                test.Logger.Messages.Should().Contain("No trusted signers with the name: 'signer' were found.");
            }
        }

        [Fact]
        public async Task ExecuteCommandAsync_RemoveAction_WithTrustedSigners_NonExistantSigner_LogsAndSucceedsAsync()
        {
            // Arrange
            var trustedSignersProvider = new Mock<ITrustedSignersProvider>();
            trustedSignersProvider
                .Setup(p => p.GetTrustedSigners())
                .Returns(new List<TrustedSignerItem>()
                {
                    new AuthorItem("author", new CertificateItem("abc", HashAlgorithmName.SHA256))
                });

            using (var test = Test.Create(TrustedSignersAction.Remove, trustedSignersProvider.Object))
            {
                test.Args.Name = "signer";

                // Act
                var result = await test.Runner.ExecuteCommandAsync(test.Args);
                result.Should().Be(0);

                test.Logger.Messages.Should().Contain("No trusted signers with the name: 'signer' were found.");
            }
        }

        [Fact]
        public async Task ExecuteCommandAsync_RemoveAction_WithTrustedSigner_ExistingSigner_RemovesItSuccesfullyAsync()
        {
            // Arrange
            var trustedSignersProvider = new Mock<ITrustedSignersProvider>();
            var expectedItem = new AuthorItem("author", new CertificateItem("abc", HashAlgorithmName.SHA256));
            trustedSignersProvider
                .Setup(p => p.GetTrustedSigners())
                .Returns(new List<TrustedSignerItem>()
                {
                    expectedItem
                });

            using (var test = Test.Create(TrustedSignersAction.Remove, trustedSignersProvider.Object))
            {
                test.Args.Name = "author";

                // Act
                var result = await test.Runner.ExecuteCommandAsync(test.Args);
                result.Should().Be(0);

                test.Logger.Messages.Should().Contain("Successfully removed the trusted signer 'author'.");

                trustedSignersProvider.Verify(p =>
                    p.Remove(It.Is<IReadOnlyList<TrustedSignerItem>>(l =>
                        l.Count == 1 &&
                        SettingsTestUtils.DeepEquals(l.First(), expectedItem))));
            }
        }

        [Fact]
        public async Task ExecuteCommandAsync_AddAction_WithCertificateFingerprint_WithoutHashAlgorithm_DefaultsToSHA256Async()
        {
            // Arrange
            var trustedSignersProvider = new Mock<ITrustedSignersProvider>();
            trustedSignersProvider
                .Setup(p => p.GetTrustedSigners())
                .Returns(new List<TrustedSignerItem>()
                {
                });

            using (var test = Test.Create(TrustedSignersAction.Add, trustedSignersProvider.Object))
            {
                test.Args.Name = "author";
                test.Args.CertificateFingerprint = "abc";

                // Act
                var result = await test.Runner.ExecuteCommandAsync(test.Args);
                result.Should().Be(0);

                test.Logger.Messages.Should().Contain("Successfully added a trusted author 'author'.");

                var expectedItem = new AuthorItem("author", new CertificateItem("abc", HashAlgorithmName.SHA256));
                trustedSignersProvider.Verify(p =>
                    p.AddOrUpdateTrustedSigner(It.Is<TrustedSignerItem>(i =>
                        SettingsTestUtils.DeepEquals(i, expectedItem))));
            }
        }

        private sealed class Test : IDisposable
        {
            private bool _isDisposed;

            internal TrustedSignersArgs Args { get; }
            internal TestDirectory Directory { get; }
            internal TestLogger Logger { get; }
            internal TrustedSignersCommandRunner Runner { get; }

            internal Test(
                ITrustedSignersProvider provider,
                IPackageSourceProvider sources,
                TrustedSignersArgs args,
                TestDirectory directory,
                TestLogger logger)
            {
                Args = args;
                Directory = directory;
                Runner = new TrustedSignersCommandRunner(provider, sources);
                Logger = logger;
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    Directory.Dispose();

                    GC.SuppressFinalize(this);

                    _isDisposed = true;
                }
            }

            internal static Test Create(
                TrustedSignersAction action,
                ITrustedSignersProvider trustedSignersProvider,
                IPackageSourceProvider sourcesProvider = null)
            {
                var directory = TestDirectory.Create();
                var logger = new TestLogger();

                var args = new TrustedSignersArgs()
                {
                    Action = action,
                    Logger = logger,
                };

                return new Test(trustedSignersProvider, sourcesProvider, args, directory, logger);
            }
        }
    }
}
