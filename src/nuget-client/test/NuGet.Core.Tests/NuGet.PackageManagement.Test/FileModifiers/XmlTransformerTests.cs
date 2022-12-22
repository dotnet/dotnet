// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Moq;
using NuGet.ProjectManagement;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.Test
{
    public class XmlTransformerTests
    {
        private readonly XmlTransformer _transformer;

        public XmlTransformerTests()
        {
            _transformer = new XmlTransformer(new Dictionary<XName, Action<XElement, XElement>>
                {
                    { "x", (parent, element) => parent.AddFirst(element) }
                });
        }

        [Fact]
        public void Constructor_ThrowsForNullNodeActions()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new XmlTransformer(nodeActions: null));

            Assert.Equal("nodeActions", exception.ParamName);
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
            using (var test = new XmlTransformerTest("<a><x><y /></x><b>$c$</b></a>"))
            {
                var projectFileOriginalContent = "<a><x /></a>";

                File.WriteAllText(test.TargetFile.FullName, projectFileOriginalContent);

                test.ProjectSystem.Setup(x => x.FileExistsInProject(It.IsNotNull<string>()))
                    .Returns(false);
                test.ProjectSystem.SetupGet(x => x.ProjectFullPath)
                    .Returns(test.TargetFile.DirectoryName);
                test.ProjectSystem.SetupGet(x => x.NuGetProjectContext)
                    .Returns(Mock.Of<INuGetProjectContext>());
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
                                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>{0}<a>{0}  <x>{0}    <y />{0}  </x>{0}  <b>d</b>{0}</a>",
                                    Environment.NewLine);

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
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>{0}<a>{0}  <x>{0}    <y />{0}    <z>d</z>{0}  </x>{0}  <b>d</b>{0}</a>",
                Environment.NewLine);

            using (var test = new XmlTransformerTest(projectFileContent))
            {
                var zipArchiveFilePath = Path.Combine(test.TestDirectory.Path, "archive.zip");
                var zipFileInfo = new InternalZipFileInfo(zipArchiveFilePath, "xml.transform");

                using (var zipFileStream = File.OpenWrite(zipArchiveFilePath))
                using (var zipArchive = new ZipArchive(zipFileStream, ZipArchiveMode.Create))
                {
                    var content = Encoding.UTF8.GetBytes("<a><x><z>$c$</z></x></a>");

                    zipArchive.AddEntry(zipFileInfo.ZipArchiveEntryFullName, content);
                }

                File.WriteAllText(test.TargetFile.FullName, projectFileContent);

                test.ProjectSystem.Setup(x => x.FileExistsInProject(It.IsNotNull<string>()))
                    .Returns(true);
                test.ProjectSystem.SetupGet(x => x.ProjectFullPath)
                    .Returns(test.TargetFile.DirectoryName);
                test.ProjectSystem.SetupGet(x => x.NuGetProjectContext)
                    .Returns(Mock.Of<INuGetProjectContext>());
                test.ProjectSystem.Setup(x => x.GetPropertyValue(It.IsNotNull<string>()))
                    .Returns("d");
                test.ProjectSystem.Setup(x => x.RemoveFile(It.IsNotNull<string>()));

                await test.Transformer.RevertFileAsync(
                    test.StreamTaskFactory,
                    test.TargetFile.Name,
                    new[] { zipFileInfo },
                    test.ProjectSystem.Object,
                    CancellationToken.None);

                var expectedResult = string.Format(
                    CultureInfo.InvariantCulture,
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>{0}<a>{0}  <x>{0}    {0}    <z>d</z>{0}  </x>{0}  {0}</a>",
                    Environment.NewLine);
                var actualResult = File.ReadAllText(test.TargetFile.FullName);

                Assert.Equal(expectedResult, actualResult);
            }
        }

        private sealed class XmlTransformerTest : IDisposable
        {
            private readonly MemoryStream _stream;

            internal Mock<IMSBuildProjectSystem> ProjectSystem { get; }
            internal Func<Task<Stream>> StreamTaskFactory { get; }
            internal FileInfo TargetFile { get; }
            internal TestDirectory TestDirectory { get; }
            internal XmlTransformer Transformer { get; }
            internal string TransformStreamContent { get; }

            internal XmlTransformerTest(string transformStreamContent)
            {
                _stream = new MemoryStream(Encoding.UTF8.GetBytes(transformStreamContent));

                TransformStreamContent = transformStreamContent;
                StreamTaskFactory = () => Task.FromResult<Stream>(_stream);
                Transformer = new XmlTransformer(new Dictionary<XName, Action<XElement, XElement>>
                    {
                        { "x", (parent, element) => parent.AddFirst(element) }
                    });
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
