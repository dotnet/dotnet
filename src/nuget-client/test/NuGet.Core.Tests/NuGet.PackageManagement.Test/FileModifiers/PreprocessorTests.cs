// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.ProjectManagement;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.Test
{
    public class PreprocessorTests
    {
        private readonly Preprocessor _processor;

        public PreprocessorTests()
        {
            _processor = new Preprocessor();
        }

        [Fact]
        public async Task TransformFileAsync_ThrowsForNullStreamTaskFactory()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _processor.TransformFileAsync(
                    streamTaskFactory: null,
                    targetPath: "a",
                    projectSystem: Mock.Of<IMSBuildProjectSystem>(),
                    cancellationToken: CancellationToken.None));

            Assert.Equal("streamTaskFactory", exception.ParamName);
        }

        [Fact]
        public async Task TransformFileAsync_ThrowsForNullProjectSystem()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _processor.TransformFileAsync(
                    () => Task.FromResult(Stream.Null),
                    targetPath: "a",
                    projectSystem: null,
                    cancellationToken: CancellationToken.None));

            Assert.Equal("projectSystem", exception.ParamName);
        }

        [Fact]
        public async Task TransformFileAsync_ThrowsIfCancelled()
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => _processor.TransformFileAsync(
                    () => Task.FromResult(Stream.Null),
                    targetPath: "a",
                    projectSystem: Mock.Of<IMSBuildProjectSystem>(),
                    cancellationToken: new CancellationToken(canceled: true)));
        }

        [Fact]
        public async Task TransformFileAsync_AddsFileToProject()
        {
            using (var test = new PreprocessorTest("a"))
            {
                test.ProjectSystem.Setup(x => x.FileExistsInProject(It.IsNotNull<string>()))
                    .Returns(false);

                test.ProjectSystem.Setup(x => x.AddFile(It.IsNotNull<string>(), It.IsNotNull<Stream>()))
                    .Callback<string, Stream>(
                        (filePath, stream) =>
                        {
                            Assert.Equal(test.TargetFile.FullName, filePath);

                            stream.Seek(offset: 0, origin: SeekOrigin.Begin);

                            using (var reader = new StreamReader(stream))
                            {
                                var actualStreamContent = reader.ReadToEnd();

                                Assert.Equal(test.StreamContent, actualStreamContent);
                            }
                        });

                await test.Processor.TransformFileAsync(
                    test.StreamTaskFactory,
                    test.TargetFile.FullName,
                    test.ProjectSystem.Object,
                    CancellationToken.None);
            }
        }

        [Fact]
        public async Task RevertFileAsync_ThrowsForNullStreamTaskFactory()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _processor.RevertFileAsync(
                    streamTaskFactory: null,
                    targetPath: "a",
                    matchingFiles: Enumerable.Empty<InternalZipFileInfo>(),
                    projectSystem: Mock.Of<IMSBuildProjectSystem>(),
                    cancellationToken: CancellationToken.None));

            Assert.Equal("streamTaskFactory", exception.ParamName);
        }

        [Fact]
        public async Task RevertFileAsync_ThrowsForNullProjectSystem()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _processor.RevertFileAsync(
                    () => Task.FromResult(Stream.Null),
                    targetPath: "a",
                    matchingFiles: Enumerable.Empty<InternalZipFileInfo>(),
                    projectSystem: null,
                    cancellationToken: CancellationToken.None));

            Assert.Equal("projectSystem", exception.ParamName);
        }

        [Fact]
        public async Task RevertFileAsync_ThrowsIfCancelled()
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => _processor.RevertFileAsync(
                    () => Task.FromResult(Stream.Null),
                    targetPath: "a",
                    matchingFiles: Enumerable.Empty<InternalZipFileInfo>(),
                    projectSystem: Mock.Of<IMSBuildProjectSystem>(),
                    cancellationToken: new CancellationToken(canceled: true)));
        }

        [Fact]
        public async Task RevertFileAsync_RemovesFileFromProject()
        {
            using (var test = new PreprocessorTest("a"))
            {
                File.WriteAllText(test.TargetFile.FullName, test.StreamContent);

                test.ProjectSystem.Setup(x => x.FileExistsInProject(It.IsNotNull<string>()))
                    .Returns(true);

                test.ProjectSystem.SetupGet(x => x.ProjectFullPath)
                    .Returns(test.TargetFile.DirectoryName);

                test.ProjectSystem.SetupGet(x => x.NuGetProjectContext)
                    .Returns((INuGetProjectContext)null);

                test.ProjectSystem.Setup(x => x.RemoveFile(It.IsNotNull<string>()))
                    .Callback<string>(
                        (filePath) =>
                        {
                            Assert.Equal(test.TargetFile.FullName, filePath);
                        });

                await test.Processor.RevertFileAsync(
                    test.StreamTaskFactory,
                    test.TargetFile.FullName,
                    Enumerable.Empty<InternalZipFileInfo>(),
                    test.ProjectSystem.Object,
                    CancellationToken.None);
            }
        }

        private sealed class PreprocessorTest : IDisposable
        {
            private readonly MemoryStream _stream;

            internal Preprocessor Processor { get; }
            internal Mock<IMSBuildProjectSystem> ProjectSystem { get; }
            internal string StreamContent { get; }
            internal Func<Task<Stream>> StreamTaskFactory { get; }
            internal FileInfo TargetFile { get; }
            internal TestDirectory TestDirectory { get; }

            internal PreprocessorTest(string streamContent)
            {
                _stream = new MemoryStream(Encoding.UTF8.GetBytes(streamContent));

                StreamContent = streamContent;
                StreamTaskFactory = () => Task.FromResult<Stream>(_stream);
                Processor = new Preprocessor();
                ProjectSystem = new Mock<IMSBuildProjectSystem>(MockBehavior.Strict);
                TestDirectory = TestDirectory.Create();
                TargetFile = new FileInfo(Path.Combine(TestDirectory.Path, "target.file"));
            }

            public void Dispose()
            {
                _stream.Dispose();
                TestDirectory.Dispose();

                GC.SuppressFinalize(this);

                ProjectSystem.Verify();
            }
        }
    }
}
