namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.IO;
    using System.Security.AccessControl;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.ApplicationInsights.TestFramework;


    /// <summary>
    /// We use these tests to understand actual behavior of <see cref="FileInfo"/> and ensure that 
    /// <see cref="PlatformFile"/> behaves consistently on all platforms we support.
    /// </summary>
    [TestClass]
    public partial class PlatformFileTest : FileSystemTest, IDisposable
    {
        private FileInfo platformFile;

        public PlatformFileTest()
        {
            string uniqueFileName = GetUniqueFileName();
            this.platformFile = FileSystemTest.CreatePlatformFile(uniqueFileName);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                try
                {
                    FileSystemTest.DeletePlatformItem(this.platformFile);
                }
                catch (IOException)
                {
                    // File already deleted
                }
            }
        }

        private static void WriteBytesAndDispose(Stream stream, byte[] bytes)
        {
            using (stream)
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        private static byte[] ReadBytesAndDispose(Stream stream)
        {
            using (stream)
            {
                var readBytes = new byte[stream.Length];
                stream.Read(readBytes, 0, readBytes.Length);
                return readBytes;
            }
        }

        [TestClass]
        public class Class : PlatformFileTest
        {
            [TestMethod]
            public void ConstructorThrowsArgumentNullExceptionGivenNullStorageFileToPreventUsageErrors()
            {
                AssertEx.Throws<ArgumentNullException>(() => new PlatformFile(null));
            }

            [TestMethod]
            public void ImplementsIFileSystemFileInterfaceForCompatibilityWithIPlatform()
            {
                Assert.IsTrue(typeof(IPlatformFile).IsAssignableFrom(typeof(PlatformFile)));
            }
        }

        [TestClass]
        public class Name : PlatformFileTest
        {
            [TestMethod]
            public void ReturnsNameOfGivenPlatformFile()
            {
                var file = new PlatformFile(this.platformFile);
                Assert.AreEqual(FileSystemTest.GetPlatformFileName(this.platformFile), file.Name);
            }
        }

        [TestClass]
        public class DateCreated : PlatformFileTest
        {
            [TestMethod]
            public void ReturnsDateCreatedOfGivenPlatformFile()
            {
                var file = new PlatformFile(this.platformFile);
                Assert.AreEqual(FileSystemTest.GetPlatformFileDateCreated(this.platformFile), file.DateCreated);
            }
        }

        [TestClass]
        public class DeleteAsync : PlatformFileTest
        {
            [TestMethod]
            public void DeletesFileFromFileSystem()
            {
                var file = new PlatformFile(this.platformFile);
                file.Delete();
                AssertEx.Throws<FileNotFoundException>(() => FileSystemTest.GetPlatformFile(FileSystemTest.GetPlatformFileName(this.platformFile)));
            }

            [TestMethod]
            public void ThrowsFileNotFoundExceptionWhenFileIsAlreadyDeleted()
            {
                var file = new PlatformFile(this.platformFile);
                FileSystemTest.DeletePlatformItem(this.platformFile);
                AssertEx.Throws<FileNotFoundException>(() => file.Delete());
            }
        }

        [TestClass]
        public class GetSizeAsync : PlatformFileTest
        {
            [TestMethod]
            public void ThrowsFileNotFoundExceptionWhenFileIsDeleted()
            {
                var file = new PlatformFile(this.platformFile);
                FileSystemTest.DeletePlatformItem(this.platformFile);
                AssertEx.Throws<FileNotFoundException>(() => { var length = file.Length; });
            }
        }

        [TestClass]
        public class OpenAsync : PlatformFileTest
        {
            [TestMethod]
            public void ReturnsStreamWithThatCanBeUsedToReadFileContents()
            {
                var writtenBytes = new byte[] { 4, 2 };
                PlatformFileTest.WriteBytesAndDispose(FileSystemTest.OpenPlatformFile(this.platformFile), writtenBytes);

                var file = new PlatformFile(this.platformFile);
                byte[] readBytes = ReadBytesAndDispose(file.Open());

                AssertEx.AreEqual(writtenBytes, readBytes);
            }

            [TestMethod]
            public void ReturnsStreamThatCanBeUsedToModifyFileContents()
            {
                var file = new PlatformFile(this.platformFile);

                var writtenBytes = new byte[] { 4, 2 };
                PlatformFileTest.WriteBytesAndDispose(file.Open(), writtenBytes);

                byte[] readBytes = ReadBytesAndDispose(FileSystemTest.OpenPlatformFile(this.platformFile));
                AssertEx.AreEqual(writtenBytes, readBytes);
            }

            [TestMethod]
            public void ThrowsFileNotFoundExceptionWhenFileIsAlreadyDeleted()
            {
                var file = new PlatformFile(this.platformFile);
                FileSystemTest.DeletePlatformItem(this.platformFile);
                AssertEx.Throws<FileNotFoundException>(() => file.Open());
            }

            [TestMethod]
            public void ThrowsIOExceptionWhenFileIsAlreadyOpen()
            {
                var file = new PlatformFile(this.platformFile);
                using (Stream previouslyOpenedStream = FileSystemTest.OpenPlatformFile(this.platformFile))
                {
                    AssertEx.Throws<IOException>(() => file.Open());
                }
            }

            [TestMethod]
            [TestCategory("WindowsOnly")]
            public void ThrowsUnauthorizedAccessExceptionWhenProcessHasNoRightToWriteToFile()
            {
                // Only on Windows as the APIs are not available in Linux.                
                using (new FileAccessDenier(this.platformFile, FileSystemRights.Write))
                { 
                    var file = new PlatformFile(this.platformFile);
                    AssertEx.Throws<UnauthorizedAccessException>(() => file.Open());
                }
            }
        }

        [TestClass]
        public class RenameAsync : PlatformFileTest
        {
            [TestMethod]
            public void ThrowsArgumentNullExceptionWhenDesiredNameIsNull()
            {
                var file = new PlatformFile(this.platformFile);
                AssertEx.Throws<ArgumentNullException>(() => file.Rename(null));
            }

            [TestMethod]
            public void RenamesFileInFileSystem()
            {
                var file = new PlatformFile(this.platformFile);
                string oldName = GetPlatformFileName(this.platformFile);
                string newName = Guid.NewGuid().ToString("N");

                file.Rename(newName);

                AssertEx.Throws<FileNotFoundException>(() => FileSystemTest.GetPlatformFile(oldName));
                Assert.IsNotNull(FileSystemTest.GetPlatformFile(newName));
            }

            [TestMethod]
            public void UpdatesNamePropertyToReflectChange()
            {
                var file = new PlatformFile(this.platformFile);

                string newName = GetUniqueFileName();
                file.Rename(newName);

                Assert.AreEqual(newName, file.Name);
            }

            [TestMethod]
            public void ThrowsFileNotFoundExceptionWhenFileIsAlreadyDeleted()
            {
                var file = new PlatformFile(this.platformFile);

                FileSystemTest.DeletePlatformItem(this.platformFile);

                string newName = GetUniqueFileName();
                AssertEx.Throws<FileNotFoundException>(() => file.Rename(newName));
            }

            [TestMethod]
            public void ThrowsIOExceptionWhenFileWithDesiredNameAlreadyExists()
            {
                var file = new PlatformFile(this.platformFile);

                string newName = GetUniqueFileName();
                var conflictingFile = FileSystemTest.CreatePlatformFile(newName);

                AssertEx.Throws<IOException>(() => file.Rename(newName));

                FileSystemTest.DeletePlatformItem(conflictingFile);
            }

            [TestMethod]
            public void ThrowsIOExceptionWhenDesiredFileNameIsTooLong()
            {
                bool expectedException = false;

                try
                {
                    var file = new PlatformFile(this.platformFile);
                    file.Rename(new string('F', 1024));
                }
                catch (PathTooLongException)
                {
                    expectedException = true;
                }
                catch (IOException)
                {
                    // NET CORE 3.0 changed the exception that can be thrown.
                    expectedException = true;
                }

                Assert.IsTrue(expectedException);
            }

            [TestMethod]
            public void ThrowsArgumentExceptionWhenDesiredFileNameIsEmpty()
            {
                var file = new PlatformFile(this.platformFile);
                AssertEx.Throws<ArgumentException>(() => file.Rename(string.Empty));
            }
        }
    }
}