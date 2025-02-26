// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentAssertions;
using Microsoft.Arcade.Common;
using Microsoft.Arcade.Test.Common;
using Microsoft.DotNet.Build.Tasks.Feed.Model;
using Microsoft.DotNet.Internal.DependencyInjection.Testing;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Microsoft.DotNet.Build.Tasks.Feed.GeneralUtils;
using static Microsoft.DotNet.Build.CloudTestTasks.AzureStorageUtils;
using MsBuildUtils = Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests
{
    public class PublishArtifactsInManifestTests
    {
        private const string GeneralTestingChannelId = "529";
        private const string RandomToken = "abcd";
        private const string AzDOFeedUrl = "https://pkgs.dev.azure.com/dnceng/public/_packaging/a-dotnet-feed/nuget/v3/index.json";
        private const string StorageUrl = "https://dotnetstorageaccount.blob.core.windows.net/placewherethingsarepublished/index.json";

        // This test should be refactored: https://github.com/dotnet/arcade/issues/6715
        [Fact]
        public void ConstructV3PublishingTask()
        {
            var manifestFullPath = TestInputs.GetFullPath(Path.Combine("Manifests", "SampleV3.xml"));

            var buildEngine = new MockBuildEngine();
            var task = new PublishArtifactsInManifest()
            {
                BuildEngine = buildEngine,
                TargetChannels = GeneralTestingChannelId
            };

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton<IFileSystem, FileSystem>()
                .AddSingleton<IBuildModelFactory, BuildModelFactory>();
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act and Assert
            task.InvokeExecute(provider);

            var which = task.WhichPublishingTask(manifestFullPath);
            which.Should().BeOfType<PublishArtifactsInManifestV3>();
        }

        [Fact]
        public void ConstructV4PublishingTask()
        {
            var manifestFullPath = TestInputs.GetFullPath(Path.Combine("Manifests", "SampleV4.xml"));

            var buildEngine = new MockBuildEngine();
            var task = new PublishArtifactsInManifest()
            {
                BuildEngine = buildEngine,
                TargetChannels = GeneralTestingChannelId
            };

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton<IFileSystem, FileSystem>()
                .AddSingleton<IBuildModelFactory, BuildModelFactory>();
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act and Assert
            task.InvokeExecute(provider);

            var which = task.WhichPublishingTask(manifestFullPath);
            which.Should().BeOfType<PublishArtifactsInManifestV4>();
        }

        [Theory]
        [InlineData("https://pkgs.dev.azure.com/dnceng/public/_packaging/mmitche-test-transport/nuget/v3/index.json", "dnceng", "public/", "mmitche-test-transport")]
        [InlineData("https://pkgs.dev.azure.com/DevDiv/public/_packaging/1234.5/nuget/v3/index.json", "DevDiv", "public/", "1234.5")]
        [InlineData("https://pkgs.dev.azure.com/DevDiv/_packaging/1234.5/nuget/v3/index.json", "DevDiv", "", "1234.5")]
        public void NugetFeedParseTests(string uri, string account, string visibility, string feed)
        {
            var matches = Regex.Match(uri, PublishingConstants.AzDoNuGetFeedPattern);
            matches.Groups["account"]?.Value.Should().Be(account);
            matches.Groups["visibility"]?.Value.Should().Be(visibility);
            matches.Groups["feed"]?.Value.Should().Be(feed);
        }


        [Theory]
        // Test cases:
        // Succeeds on first try, not already on feed
        [InlineData(1, false, true)]
        // Succeeds on second try, turns out to be already on the feed
        [InlineData(2, true, true)]
        // Succeeds on last possible try (for retry logic)
        [InlineData(5, false, true)]
        // Succeeds by determining streams match and takes no action.
        [InlineData(1, true, true)]
        // Fails due to too many retries
        [InlineData(7, false, true, true)]
        // Fails and gives up due to non-matching streams (CompareLocalPackageToFeedPackage says no match)
        [InlineData(10, true, false, true)]
        public async Task PushNugetPackageTestsAsync(int pushAttemptsBeforeSuccess, bool packageAlreadyOnFeed,  bool localPackageMatchesFeed, bool expectedFailure = false)
        {
            // Setup
            var buildEngine = new MockBuildEngine();
            // May as well check that the exe is plumbed through from the task.
            string fakeNugetExeName = $"{Path.GetRandomFileName()}.exe";
            int timesNugetExeCalled = 0;

            // Functionality is the same as this is in the base class, create a v2 object to test. 
            var task = new PublishArtifactsInManifestV3
            {
                InternalBuild = true,
                BuildEngine = buildEngine,
                NugetPath = fakeNugetExeName,
                MaxRetryCount = 5, // In case the default changes, lock to 5 so the test data works
                RetryDelayMilliseconds = 10 // retry faster in test
            };
            TargetFeedConfig config = new TargetFeedConfig(TargetFeedContentType.Package, "testUrl", FeedType.AzDoNugetFeed, "tokenValue");

            Func<string, string, HttpClient, MsBuildUtils.TaskLoggingHelper, Task<PackageFeedStatus>> testCompareLocalPackage = async (string localPackageFullPath, string packageContentUrl, HttpClient client, MsBuildUtils.TaskLoggingHelper log) =>
            {
                await (Task.Delay(10)); // To make this actually async
                Debug.WriteLine($"Called mocked CompareLocalPackageToFeedPackage() :  localPackageFullPath = {localPackageFullPath}, packageContentUrl = {packageContentUrl}");
                if (packageAlreadyOnFeed)
                {
                    return localPackageMatchesFeed ? PackageFeedStatus.ExistsAndIdenticalToLocal : PackageFeedStatus.ExistsAndDifferent;
                }
                else
                {
                    return PackageFeedStatus.DoesNotExist;
                }
            };

            Func<string, string, Task<ProcessExecutionResult>> testRunAndLogProcess = (string fakeExePath, string fakeExeArgs) =>
            {
                Debug.WriteLine($"Called mocked RunProcessAndGetOutputs() :  ExePath = {fakeExePath}, ExeArgs = {fakeExeArgs}");
                fakeNugetExeName.Should().Be(fakeExePath);
                ProcessExecutionResult result = new ProcessExecutionResult() { StandardError = "fake stderr", StandardOut = "fake stdout" };
                timesNugetExeCalled++;
                if (timesNugetExeCalled >= pushAttemptsBeforeSuccess)
                {
                    result.ExitCode = 0;
                }
                else
                {
                    result.ExitCode = 1;
                }
                return Task.FromResult(result);
            };

            await task.PushNugetPackageAsync(
                config, 
                null, 
                "localPackageLocation", 
                "1234", 
                "version", 
                "feedaccount", 
                "feedvisibility", 
                "feedname",
                testCompareLocalPackage,
                testRunAndLogProcess);
            if (!expectedFailure && localPackageMatchesFeed)
            {
                // Successful retry scenario; make sure we ran the # of retries we thought.
                timesNugetExeCalled.Should().BeLessOrEqualTo(task.MaxRetryCount);
            }
            expectedFailure.Should().Be(task.Log.HasLoggedErrors);
        }


        [Theory]
        // Simple case where we fill the whole buffer on each stream call and the streams match
        [InlineData("QXJjYWRl", "QXJjYWRl", new int[] { int.MaxValue }, new int[] { int.MaxValue }, 1024)]
        // Simple case where we fill the whole buffer on each stream call and the streams don't match
        [InlineData("QXJjYWRl", "QXJjYWRm", new int[] { int.MaxValue }, new int[] { int.MaxValue }, 1024)]
        // Case where the first stream returns everything initially, but the second returns one byte at a time.
        [InlineData("QXJjYWRl", "QXJjYWRl", new int[] { int.MaxValue }, new int[] { 1, 1, 1, 1, 1 }, 1024)]
        // Case where the first stream returns everything initially, but the second returns one byte at a time and they are not equal
        [InlineData("QXJjYWRl", "QXJjYWRm", new int[] { int.MaxValue }, new int[] { 1, 1, 1, 1, 1 }, 1024)]
        // Case where both streams return one byte at a time
        [InlineData("QXJjYWRl", "QXJjYWRl", new int[] { 1, 1, 1, 1, 1 }, new int[] { 1, 1, 1, 1, 1 }, 1024)]
        // Case where both streams return one byte at a time
        [InlineData("QXJjYWRl", "QXJjYWQ=", new int[] { 1, 1, 1, 1, 1 }, new int[] { 1, 1, 1, 1, 1 }, 1024)]
        // Case where the buffer must wrap around and one stream returns faster than the other, equal streams
        [InlineData("VGhlIHF1aWNrIGJyb3JuIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5aXNoIGRvZ2dv", "VGhlIHF1aWNrIGJyb3JuIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5aXNoIGRvZ2dv", new int[] { 16, 16, 16, 16, 16, 16 }, new int[] { 1, 1, 1, 1, 1 }, 8)]
        // Case where the buffer must wrap around and one stream returns faster than the other, unequal streams
        [InlineData("VGhpcyBpcyBhIHNlbnRlbmNlIHRoYXQgaXMgYSBsaXR0bGUgbG9uZ2Vy", "VGhpcyBpcyBhIHNlbnRlbmNlIHRoYXQgaXMgYSBsb25nZXI=", new int[] { 7, 3, 5, 16, 16, 16 }, new int[] { 1, 1, 1, 1, 1 }, 8)]
        public async Task StreamComparisonTestsAsync(string streamA, string streamB, int[] maxStreamABytesReturnedEachCall, int[] maxStreamBBytesReturnedEachCall, int bufferSize)
        {
            byte[] streamABytes = Convert.FromBase64String(streamA);
            byte[] streamBBytes = Convert.FromBase64String(streamB);

            FakeStream fakeStreamA = new FakeStream(streamABytes, maxStreamABytesReturnedEachCall);
            FakeStream fakeStreamB = new FakeStream(streamBBytes, maxStreamBBytesReturnedEachCall);

            bool streamsShouldBeSame = streamA == streamB;
            bool streamsAreSame = await GeneralUtils.CompareStreamsAsync(fakeStreamA, fakeStreamB, bufferSize);
            streamsAreSame.Should().Be(streamsShouldBeSame, "Stream comparison failed");
        }

        class FakeStream : Stream
        {
            public FakeStream(byte[] streamBytes, int[] maxStreamBytesReturned)
            {
                _streamBytes = streamBytes;
                _maxStreamBytesReturned = maxStreamBytesReturned;
            }

            byte[] _streamBytes;
            int[] _maxStreamBytesReturned;
            int _callIndex = 0;
            int _position = 0;

            public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                count.Should().BeGreaterThan(0);

                // If we reach the end of the _maxStreamBytesReturned array, just use max int.
                int maxStreamBytesThisCall = int.MaxValue;
                if (_callIndex < _maxStreamBytesReturned.Length)
                {
                    maxStreamBytesThisCall = _maxStreamBytesReturned[_callIndex];
                    _callIndex++;
                }
                int bytesToWrite = Math.Min(Math.Min(_streamBytes.Length - _position, count), maxStreamBytesThisCall);

                for (int i = 0; i < bytesToWrite; i++)
                {
                    buffer[offset + i] = _streamBytes[_position + i];
                }
                _position += bytesToWrite;

                return Task.FromResult(bytesToWrite);
            }

            #region Unused

            public override bool CanRead => throw new NotImplementedException();

            public override bool CanSeek => throw new NotImplementedException();

            public override bool CanWrite => throw new NotImplementedException();

            public override long Length => throw new NotImplementedException();

            public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }
            #endregion
        }

        [Fact]
        public void AreDependenciesRegistered()
        {
            PublishArtifactsInManifest task = new PublishArtifactsInManifest();

            var collection = new ServiceCollection();
            task.ConfigureServices(collection);
            var provider = collection.BuildServiceProvider();

            DependencyInjectionValidation.IsDependencyResolutionCoherent(
                    s =>
                    {
                        task.ConfigureServices(s);
                    },
                    out string message,
                    additionalSingletonTypes: task.GetExecuteParameterTypes()
                )
                .Should()
                .BeTrue(message);
        }        
    }
}
