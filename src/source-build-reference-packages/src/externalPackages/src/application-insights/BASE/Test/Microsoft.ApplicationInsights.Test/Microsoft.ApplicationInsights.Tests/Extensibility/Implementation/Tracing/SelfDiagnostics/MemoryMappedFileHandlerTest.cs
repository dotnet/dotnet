namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.SelfDiagnostics
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MemoryMappedFileHandlerTest
    {
        public static readonly byte[] MessageOnNewFile = MemoryMappedFileHandler.MessageOnNewFile;

        [TestMethod]
        public void MemoryMappedFileHandler_Success()
        {
            string filePath;
            var fileSize = 1024;
            using (var handler = new MemoryMappedFileHandler())
            {
                handler.CreateLogFile(".", fileSize);

                filePath = handler.CurrentFilePath;
            }

            var actualBytes = ReadFile(filePath, MessageOnNewFile.Length);

            CollectionAssert.AreEqual(MessageOnNewFile, actualBytes);
        }

        [TestMethod]
        public void MemoryMappedFileHandler_Circular_Success()
        {
            var fileSize = 1024;
            var buffer = new byte[1024];
            var messageToOverflow = Encoding.UTF8.GetBytes("1234567");
            var expectedBytesAtEnd = Encoding.UTF8.GetBytes("1234");
            var expectedBytesAtStart = Encoding.UTF8.GetBytes("567cessfully opened file.\n");
            string filePath;

            using (var handler = new MemoryMappedFileHandler())
            {
                handler.CreateLogFile(".", fileSize);

                handler.Write(buffer, fileSize - MessageOnNewFile.Length - expectedBytesAtEnd.Length);

                handler.Write(messageToOverflow, messageToOverflow.Length);

                filePath = handler.CurrentFilePath;
            }

            var actualBytes = ReadFile(filePath, buffer.Length);

            CollectionAssert.AreEqual(expectedBytesAtStart, SubArray(actualBytes, 0, expectedBytesAtStart.Length));
            CollectionAssert.AreEqual(expectedBytesAtEnd, SubArray(actualBytes, actualBytes.Length - expectedBytesAtEnd.Length, expectedBytesAtEnd.Length));
        }

        private static byte[] ReadFile(string filePath, int byteCount)
        {
            byte[] actualBytes = new byte[byteCount];
            using (var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                file.Read(actualBytes, 0, byteCount);
            }
            return actualBytes;
        }

        private static byte[] SubArray(byte[] array, int offset, int length)
        {
            byte[] result = new byte[length];
            Array.Copy(array, offset, result, 0, length);
            return result;
        }
    }
}
