// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Moq;
using NuGet.ProjectManagement;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.Test
{
    public class XdtTransformerTests
    {
        private readonly XdtTransformer _transformer;

        public XdtTransformerTests()
        {
            _transformer = new XdtTransformer();
        }

        [Fact]
        public async Task TransformFileAsync_ThrowsForNullStreamTaskFactory()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _transformer.TransformFileAsync(
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
                () => _transformer.TransformFileAsync(
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
                () => _transformer.TransformFileAsync(
                    () => Task.FromResult(Stream.Null),
                    targetPath: "a",
                    projectSystem: Mock.Of<IMSBuildProjectSystem>(),
                    cancellationToken: new CancellationToken(canceled: true)));
        }

        [Fact]
        public async Task TransformFileAsync_TransformsFile()
        {
            using (var test = new XdtTransformerTest("<a xmlns:xdt=\"http://schemas.microsoft.com/XML-Document-Transform\"><x><y xdt:Transform=\"Insert\"><z>$c$</z></y></x></a>"))
            {
                var projectFileOriginalContent = "<a><x /></a>";

                File.WriteAllText(test.TargetFile.FullName, projectFileOriginalContent);

                test.ProjectSystem.SetupGet(x => x.ProjectFullPath)
                    .Returns(test.TargetFile.DirectoryName);
                test.ProjectSystem.SetupGet(x => x.ProjectName)
                    .Returns("ProjectName");
                test.ProjectSystem.Setup(x => x.GetPropertyValue(It.IsNotNull<string>()))
                    .Returns("d");
                test.ProjectSystem.Setup(x => x.AddFile(It.IsNotNull<string>(), It.IsNotNull<Stream>()))
                    .Callback<string, Stream>(
                        (targetFilePath, stream) =>
                        {
                            Assert.Equal(test.TargetFile.Name, targetFilePath);

                            stream.Seek(offset: 0, origin: SeekOrigin.Begin);

                            using (var reader = new StreamReader(stream))
                            {
                                var actualResult = reader.ReadToEnd();
                                var expectedResult = "<a>\r\n  <x>\r\n    <y>\r\n      <z>d</z>\r\n    </y>\r\n  </x>\r\n</a>";

                                Assert.Equal(expectedResult, actualResult);
                            }
                        });

                await test.Transformer.TransformFileAsync(
                    test.StreamTaskFactory,
                    test.TargetFile.Name,
                    test.ProjectSystem.Object,
                    CancellationToken.None);
            }
        }

        [Fact]
        public async Task PerformXdtTransformAsync_InSecureXmlFailsToTransform_Throws()
        {
            using (var test = new XdtTransformerTest("<a xmlns:xdt=\"http://schemas.microsoft.com/XML-Document-Transform\"><x><y xdt:Transform=\"Insert\"><z>$c$</z></y></x></a>"))
            {
                var projectFileOriginalContent =
                @"<?xml version=""1.0""?>
<!DOCTYPE a [
   <!ENTITY greeting ""Hello"">
   <!ENTITY name ""NuGet Client "">
   <!ENTITY sayhello ""&greeting; &name;"">
]>
<a><x name=""&sayhello;"" /></a>";

                File.WriteAllText(test.TargetFile.FullName, projectFileOriginalContent);

                test.ProjectSystem.SetupGet(x => x.ProjectFullPath)
                    .Returns(test.TargetFile.DirectoryName);
                test.ProjectSystem.SetupGet(x => x.ProjectName)
                    .Returns("ProjectName");
                test.ProjectSystem.Setup(x => x.GetPropertyValue(It.IsNotNull<string>()))
                    .Returns("d");

                var exception = await Assert.ThrowsAsync<InvalidDataException>(
                    () => XdtTransformer.PerformXdtTransformAsync(
                        test.StreamTaskFactory,
                        test.TargetFile.Name,
                        test.ProjectSystem.Object,
                        CancellationToken.None));

                Assert.IsType<XmlException>(exception.InnerException);
            }
        }

        [Fact]
        public async Task TransformFileAsync_WithPUACharactersInPathTransformsFile_Success()
        {
            using var test = new XdtTransformerTest("<a xmlns:xdt=\"http://schemas.microsoft.com/XML-Document-Transform\"><x><y xdt:Transform=\"Insert\"><z>$c$</z></y></x></a>");

            var projectFileOriginalContent = "<a><x /></a>";
            File.WriteAllText(test.TargetFile.FullName, projectFileOriginalContent);

            string pathWithPAUChars = Path.Combine(test.TargetFile.DirectoryName, "U1[]U2[]U3[]");
            Directory.CreateDirectory(pathWithPAUChars);
            string newTargetFile = Path.Combine(pathWithPAUChars, "target.file");
            File.Move(test.TargetFile.FullName, newTargetFile);

            test.ProjectSystem.SetupGet(x => x.ProjectFullPath)
                .Returns(test.TargetFile.DirectoryName);
            test.ProjectSystem.SetupGet(x => x.ProjectName)
                .Returns("ProjectName");
            test.ProjectSystem.Setup(x => x.GetPropertyValue(It.IsNotNull<string>()))
                .Returns("d");
            test.ProjectSystem.Setup(x => x.AddFile(It.IsNotNull<string>(), It.IsNotNull<Stream>()))
                .Callback<string, Stream>(
                    (targetFilePath, stream) =>
                    {
                        Assert.Equal(newTargetFile, targetFilePath);

                        stream.Seek(offset: 0, origin: SeekOrigin.Begin);

                        using (var reader = new StreamReader(stream))
                        {
                            var actualResult = reader.ReadToEnd();
                            var expectedResult = "<a>\r\n  <x>\r\n    <y>\r\n      <z>d</z>\r\n    </y>\r\n  </x>\r\n</a>";

                            Assert.Equal(expectedResult, actualResult);
                        }
                    });

            await XdtTransformer.PerformXdtTransformAsync(
                test.StreamTaskFactory,
                newTargetFile,
                test.ProjectSystem.Object,
                CancellationToken.None);
        }

        [Fact]
        public async Task RevertFileAsync_ThrowsForNullStreamTaskFactory()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _transformer.RevertFileAsync(
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
                () => _transformer.RevertFileAsync(
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
                () => _transformer.RevertFileAsync(
                    () => Task.FromResult(Stream.Null),
                    targetPath: "a",
                    matchingFiles: Enumerable.Empty<InternalZipFileInfo>(),
                    projectSystem: Mock.Of<IMSBuildProjectSystem>(),
                    cancellationToken: new CancellationToken(canceled: true)));
        }

        [Fact]
        public async Task RevertFileAsync_RevertsFile()
        {
            var projectFileContent = string.Format(
                CultureInfo.InvariantCulture,
                "<a>{0}  <x>{0}    <b>d</b>{0}    <y>{0}      <z>d</z>{0}    </y>{0}  </x>{0}</a>",
                Environment.NewLine);

            using (var test = new XdtTransformerTest("<a xmlns:xdt=\"http://schemas.microsoft.com/XML-Document-Transform\"><x><y xdt:Transform=\"Remove\"><z>$c$</z></y></x></a>"))
            {
                var zipArchiveFilePath = Path.Combine(test.TestDirectory.Path, "archive.zip");
                var zipFileInfo = new InternalZipFileInfo(zipArchiveFilePath, "install.xdt");

                using (var zipFileStream = File.OpenWrite(zipArchiveFilePath))
                using (var zipArchive = new ZipArchive(zipFileStream, ZipArchiveMode.Create))
                {
                    var content = Encoding.UTF8.GetBytes("<a xmlns:xdt=\"http://schemas.microsoft.com/XML-Document-Transform\"><x><b xdt:Transform=\"Insert\">$c$</b></x></a>");

                    zipArchive.AddEntry(zipFileInfo.ZipArchiveEntryFullName, content);
                }

                File.WriteAllText(test.TargetFile.FullName, projectFileContent);

                test.ProjectSystem.SetupGet(x => x.ProjectFullPath)
                    .Returns(test.TargetFile.DirectoryName);
                test.ProjectSystem.SetupGet(x => x.ProjectName)
                    .Returns("ProjectName");
                test.ProjectSystem.Setup(x => x.GetPropertyValue(It.IsNotNull<string>()))
                    .Returns("d");
                test.ProjectSystem.Setup(x => x.AddFile(It.IsNotNull<string>(), It.IsNotNull<Stream>()))
                    .Callback<string, Stream>(
                        (targetFilePath, stream) =>
                        {
                            Assert.Equal(test.TargetFile.Name, targetFilePath);

                            stream.Seek(offset: 0, origin: SeekOrigin.Begin);

                            using (var reader = new StreamReader(stream))
                            {
                                var actualResult = reader.ReadToEnd();
                                var expectedResult = string.Format(
                                    CultureInfo.InvariantCulture,
                                    "<a>{0}  <x>{0}    <b>d</b>{0}  </x>{0}</a>",
                                    Environment.NewLine);

                                Assert.Equal(expectedResult, actualResult);
                            }
                        });

                await test.Transformer.RevertFileAsync(
                    test.StreamTaskFactory,
                    test.TargetFile.Name,
                    new[] { zipFileInfo },
                    test.ProjectSystem.Object,
                    CancellationToken.None);
            }
        }

        private sealed class XdtTransformerTest : IDisposable
        {
            private readonly MemoryStream _stream;

            internal Mock<IMSBuildProjectSystem> ProjectSystem { get; }
            internal Func<Task<Stream>> StreamTaskFactory { get; }
            internal FileInfo TargetFile { get; }
            internal TestDirectory TestDirectory { get; }
            internal XdtTransformer Transformer { get; }
            internal string TransformStreamContent { get; }

            internal XdtTransformerTest(string transformStreamContent)
            {
                _stream = new MemoryStream(Encoding.UTF8.GetBytes(transformStreamContent));

                TransformStreamContent = transformStreamContent;
                StreamTaskFactory = () => Task.FromResult<Stream>(_stream);
                Transformer = new XdtTransformer();
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
