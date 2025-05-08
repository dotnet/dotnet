namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.SelfDiagnostics
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class SelfDiagnosticsEventListenerTest
    {
        private const string Ellipses = "...\n";
        private const string EllipsesWithBrackets = "{...}\n";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SelfDiagnosticsEventListener_constructor_Invalid_Input()
        {
            // no configRefresher object
            _ = new SelfDiagnosticsEventListener(EventLevel.Error, null);
        }

        [TestMethod]
        public void SelfDiagnosticsEventListener_EventSourceSetup_LowerSeverity()
        {
            var fileHandlerMock = new Mock<MemoryMappedFileHandler>();
            var listener = new SelfDiagnosticsEventListener(EventLevel.Error, fileHandlerMock.Object);

            // Emitting a Warning event. Or any EventSource event with lower severity than Error.
            CoreEventSource.Log.OperationIsNullWarning();
            fileHandlerMock.Verify(fileHandler => fileHandler.Write(It.IsAny<byte[]>(), It.IsAny<int>()), Times.Never());
        }

        [TestMethod]
        public void SelfDiagnosticsEventListener_EventSourceSetup_HigherSeverity()
        {
            var fileHandlerMock = new Mock<MemoryMappedFileHandler>();
            fileHandlerMock.Setup(fileHandler => fileHandler.Write(It.IsAny<byte[]>(), It.IsAny<int>()));
            var listener = new SelfDiagnosticsEventListener(EventLevel.Error, fileHandlerMock.Object);

            // Emitting an Error event. Or any EventSource event with higher than or equal to to Error severity.
            CoreEventSource.Log.InvalidOperationToStopError();
            fileHandlerMock.Verify(fileHandler => fileHandler.Write(It.IsAny<byte[]>(), It.IsAny<int>()));
        }

        [TestMethod]
        public void SelfDiagnosticsEventListener_DateTimeGetBytes()
        {
            var fileHandlerMock = new Mock<MemoryMappedFileHandler>();
            var listener = new SelfDiagnosticsEventListener(EventLevel.Error, fileHandlerMock.Object);

            // Check DateTimeKind of Utc, Local, and Unspecified
            DateTime[] datetimes = new DateTime[]
            {
                DateTime.SpecifyKind(DateTime.Parse("1996-12-01T14:02:31.1234567-08:00", CultureInfo.InvariantCulture), DateTimeKind.Utc),
                DateTime.SpecifyKind(DateTime.Parse("1996-12-01T14:02:31.1234567-08:00", CultureInfo.InvariantCulture), DateTimeKind.Local),
                DateTime.SpecifyKind(DateTime.Parse("1996-12-01T14:02:31.1234567-08:00", CultureInfo.InvariantCulture), DateTimeKind.Unspecified),
                DateTime.UtcNow,
                DateTime.Now,
            };

            // Expect to match output string from DateTime.ToString("O")
            string[] expected = new string[datetimes.Length];
            for (int i = 0; i < datetimes.Length; i++)
            {
                expected[i] = datetimes[i].ToString("O");
            }

            byte[] buffer = new byte[40 * datetimes.Length];
            int pos = 0;

            // Get string after DateTimeGetBytes() write into a buffer
            string[] results = new string[datetimes.Length];
            for (int i = 0; i < datetimes.Length; i++)
            {
                int len = SelfDiagnosticsEventListener.DateTimeGetBytes(datetimes[i], buffer, pos);
                results[i] = Encoding.Default.GetString(buffer, pos, len);
                pos += len;
            }

            CollectionAssert.AreEqual(expected, results);
        }

        [TestMethod]
        public void SelfDiagnosticsEventListener_EncodeInBuffer_Null()
        {
            byte[] buffer = new byte[20];
            int startPos = 0;
            int endPos = SelfDiagnosticsEventListener.EncodeInBuffer(null, false, buffer, startPos);
            Assert.AreEqual(startPos, endPos);
        }

        [TestMethod]
        public void SelfDiagnosticsEventListener_EncodeInBuffer_Empty()
        {
            byte[] buffer = new byte[20];
            int startPos = 0;
            int endPos = SelfDiagnosticsEventListener.EncodeInBuffer(string.Empty, false, buffer, startPos);
            byte[] expected = Encoding.UTF8.GetBytes(string.Empty);
            AssertBufferOutput(expected, buffer, startPos, endPos);
        }

        [TestMethod]
        public void SelfDiagnosticsEventListener_EncodeInBuffer_EnoughSpace()
        {
            byte[] buffer = new byte[20];
            int startPos = buffer.Length - Ellipses.Length - 6;  // Just enough space for "abc" even if "...\n" needs to be added.
            int endPos = SelfDiagnosticsEventListener.EncodeInBuffer("abc", false, buffer, startPos);

            // '\n' will be appended to the original string "abc" after EncodeInBuffer is called.
            // The byte where '\n' will be placed should not be touched within EncodeInBuffer, so it stays as '\0'.
            byte[] expected = Encoding.UTF8.GetBytes("abc\0");
            AssertBufferOutput(expected, buffer, startPos, endPos + 1);
        }

        [TestMethod]
        public void SelfDiagnosticsEventListener_EncodeInBuffer_NotEnoughSpaceForFullString()
        {
            byte[] buffer = new byte[20];
            int startPos = buffer.Length - Ellipses.Length - 5;  // Just not space for "abc" if "...\n" needs to be added.

            // It's a quick estimate by assumption that most Unicode characters takes up to 2 16-bit UTF-16 chars,
            // which can be up to 4 bytes when encoded in UTF-8.
            int endPos = SelfDiagnosticsEventListener.EncodeInBuffer("abc", false, buffer, startPos);
            byte[] expected = Encoding.UTF8.GetBytes("ab...\0");
            AssertBufferOutput(expected, buffer, startPos, endPos + 1);
        }

        [TestMethod]
        public void SelfDiagnosticsEventListener_EncodeInBuffer_NotEvenSpaceForTruncatedString()
        {
            byte[] buffer = new byte[20];
            int startPos = buffer.Length - Ellipses.Length;  // Just enough space for "...\n".
            int endPos = SelfDiagnosticsEventListener.EncodeInBuffer("abc", false, buffer, startPos);
            byte[] expected = Encoding.UTF8.GetBytes("...\0");
            AssertBufferOutput(expected, buffer, startPos, endPos + 1);
        }

        [TestMethod]
        public void SelfDiagnosticsEventListener_EncodeInBuffer_NotEvenSpaceForTruncationEllipses()
        {
            byte[] buffer = new byte[20];
            int startPos = buffer.Length - Ellipses.Length + 1;  // Not enough space for "...\n".
            int endPos = SelfDiagnosticsEventListener.EncodeInBuffer("abc", false, buffer, startPos);
            Assert.AreEqual(startPos, endPos);
        }

        [TestMethod]
        public void SelfDiagnosticsEventListener_EncodeInBuffer_IsParameter_EnoughSpace()
        {
            byte[] buffer = new byte[20];
            int startPos = buffer.Length - EllipsesWithBrackets.Length - 6;  // Just enough space for "abc" even if "...\n" need to be added.
            int endPos = SelfDiagnosticsEventListener.EncodeInBuffer("abc", true, buffer, startPos);
            byte[] expected = Encoding.UTF8.GetBytes("{abc}\0");
            AssertBufferOutput(expected, buffer, startPos, endPos + 1);
        }

        [TestMethod]
        public void SelfDiagnosticsEventListener_EncodeInBuffer_IsParameter_NotEnoughSpaceForFullString()
        {
            byte[] buffer = new byte[20];
            int startPos = buffer.Length - EllipsesWithBrackets.Length - 5;  // Just not space for "...\n".
            int endPos = SelfDiagnosticsEventListener.EncodeInBuffer("abc", true, buffer, startPos);
            byte[] expected = Encoding.UTF8.GetBytes("{ab...}\0");
            AssertBufferOutput(expected, buffer, startPos, endPos + 1);
        }

        [TestMethod]
        public void SelfDiagnosticsEventListener_EncodeInBuffer_IsParameter_NotEvenSpaceForTruncatedString()
        {
            byte[] buffer = new byte[20];
            int startPos = buffer.Length - EllipsesWithBrackets.Length;  // Just enough space for "{...}\n".
            int endPos = SelfDiagnosticsEventListener.EncodeInBuffer("abc", true, buffer, startPos);
            byte[] expected = Encoding.UTF8.GetBytes("{...}\0");
            AssertBufferOutput(expected, buffer, startPos, endPos + 1);
        }

        [TestMethod]
        public void SelfDiagnosticsEventListener_EncodeInBuffer_IsParameter_NotEvenSpaceForTruncationEllipses()
        {
            byte[] buffer = new byte[20];
            int startPos = buffer.Length - EllipsesWithBrackets.Length + 1;  // Not enough space for "{...}\n".
            int endPos = SelfDiagnosticsEventListener.EncodeInBuffer("abc", true, buffer, startPos);
            Assert.AreEqual(startPos, endPos);
        }

        private static void AssertBufferOutput(byte[] expected, byte[] buffer, int startPos, int endPos)
        {
            Assert.AreEqual(expected.Length, endPos - startPos);
            for (int i = 0, j = startPos; j < endPos; ++i, ++j)
            {
                Assert.AreEqual(expected[i], buffer[j]);
            }
        }
    }
}
