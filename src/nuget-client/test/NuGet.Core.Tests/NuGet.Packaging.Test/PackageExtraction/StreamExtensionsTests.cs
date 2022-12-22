// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Text;
using Moq;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test.PackageExtraction
{
    public class StreamExtensionsTests
    {
        private const string TestText = "Hello world";

        class TestableProxy : NuGet.Packaging.StreamExtensions.Testable
        {
            public bool MmapCopyWasCalled { get; set; }
            public bool FileStreamCopyWasCalled { get; set; }
            internal TestableProxy(IEnvironmentVariableReader environmentVariableReader) : base(environmentVariableReader)
            {
            }

            internal override void MmapCopy(Stream inputStream, string fileFullPath, long size)
            {
                MmapCopyWasCalled = true;
                base.MmapCopy(inputStream, fileFullPath, size);
            }

            internal override void FileStreamCopy(Stream inputStream, string fileFullPath)
            {
                FileStreamCopyWasCalled = true;
                base.FileStreamCopy(inputStream, fileFullPath);
            }
        }

        [PlatformFact(Platform.Windows)]
        public void CopyToFile_Windows_CallsMmapCopy()
        {
            using (var directory = TestDirectory.Create())
            {
                var testPath = Path.Combine(directory, Path.GetRandomFileName());
                var environmentVariableReader = new Mock<IEnvironmentVariableReader>();
                var uut = new TestableProxy(environmentVariableReader.Object);
                uut.CopyToFile(new MemoryStream(Encoding.UTF8.GetBytes(TestText)), testPath);
                Assert.True(uut.MmapCopyWasCalled);
                Assert.False(uut.FileStreamCopyWasCalled);
                Assert.Equal(TestText, File.ReadAllText(testPath));
            }
        }

        [PlatformFact(SkipPlatform = Platform.Windows)]
        public void CopyToFile_NonWindows_CallsFileStreamCopy()
        {
            using (var directory = TestDirectory.Create())
            {
                var testPath = Path.Combine(directory, Path.GetRandomFileName());
                var environmentVariableReader = new Mock<IEnvironmentVariableReader>();
                var uut = new TestableProxy(environmentVariableReader.Object);
                uut.CopyToFile(new MemoryStream(Encoding.UTF8.GetBytes(TestText)), testPath);
                Assert.False(uut.MmapCopyWasCalled);
                Assert.True(uut.FileStreamCopyWasCalled);
                Assert.Equal(TestText, File.ReadAllText(testPath));
            }
        }

        [Theory]
        [InlineData("0", false)]
        [InlineData("1", true)]
        public void CopyToFile_EnvironmentVariable_IsRespected(string env, bool expectedMMap)
        {
            using (var directory = TestDirectory.Create())
            {
                var environmentVariableReader = new Mock<IEnvironmentVariableReader>();
                environmentVariableReader.Setup(x => x.GetEnvironmentVariable("NUGET_PACKAGE_EXTRACTION_USE_MMAP"))
                    .Returns(env);
                var uut = new TestableProxy(environmentVariableReader.Object);
                var testPath = Path.Combine(directory, Path.GetRandomFileName());
                uut.CopyToFile(new MemoryStream(Encoding.UTF8.GetBytes(TestText)), testPath);
                Assert.Equal(uut.MmapCopyWasCalled, expectedMMap);
                Assert.Equal(uut.FileStreamCopyWasCalled, !uut.MmapCopyWasCalled);
                Assert.Equal(TestText, File.ReadAllText(testPath));
            }
        }
    }
}
