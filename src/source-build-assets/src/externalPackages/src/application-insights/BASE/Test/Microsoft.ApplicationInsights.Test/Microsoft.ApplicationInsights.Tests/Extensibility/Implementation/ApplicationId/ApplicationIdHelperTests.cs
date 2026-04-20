namespace Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class ApplicationIdHelperTests
    {
        List<char> asciiCharacters = new List<char>(); // US-ASCII characters (hex: 0x00 - 0x7F) (decimal: 0-127) (PARTIALLY ALLOWED)
        List<char> asciiExtendedCharacters = new List<char>(); // ASCII Extended characters (hex: 0x80 - 0xFF) (decimal: 128-255) (NOT ALLOWED)
        List<char> nonPrintableCharacters = new List<char>(); // Non-Printable ASCII characters are (hex: 0x00 - 0x1F) (decimal: 0-31) (NOT ALLOWED)
        List<char> asciiPrintableCharacters = new List<char>(); // Printable ASCII characters are (hex: 0x20 - 0xFF) (decimal: 32-255) (PARTIALLY ALLOWED)
        List<char> headerSafeCharacters = new List<char>(); // Printable ASCII characters are (hex: 0x20 - 0xFF) (decimal: 32-255) (PARTIALLY ALLOWED)

        public ApplicationIdHelperTests()
        {
            asciiCharacters.AddRange(Enumerable.Range(0, 128).Select(x => Convert.ToChar(x)));
            asciiExtendedCharacters.AddRange(Enumerable.Range(128, 128).Select(x => Convert.ToChar(x)));
            nonPrintableCharacters.AddRange(Enumerable.Range(0, 32).Select(x => Convert.ToChar(x)));
            asciiPrintableCharacters.AddRange(Enumerable.Range(32, 224).Select(x => Convert.ToChar(x)));
            headerSafeCharacters.AddRange(Enumerable.Range(32, 96).Select(x => Convert.ToChar(x)));
        }

        [TestMethod]
        public void VerifySafeCharactersAreAllowed()
        {
            string testSet = new String(headerSafeCharacters.ToArray());

            Verify(testSet, testSet);
        }

        [TestMethod]
        public void VerifyNonPrintableCharactersAreNotAllowed()
        {
            string testSet = new string(nonPrintableCharacters.ToArray());

            Verify(testSet, string.Empty);
        }

        [TestMethod]
        public void VerifyExtendedCharactersAreNotAllowed()
        {
            string testSet = new string(asciiExtendedCharacters.ToArray());

            Verify(testSet, string.Empty);
        }

        [TestMethod]
        public void VerifyVarious()
        {
            Verify(null, null);
            Guid guid = Guid.NewGuid();
            Verify(guid.ToString(), guid.ToString());
            Verify("Hello, World!", "Hello, World!");
            Verify("Test\r\n\tTest", "TestTest"); // Non-printable ASCII
            Verify("Test\u20acTest", "TestTest"); // Euro sign (non-ASCII above 256)
            Verify("Test\u00a0Test", "TestTest"); // No-break space (non-ASCII below 256)
        }

        private void Verify(string input, string expectedSanitizedOutput) => Assert.AreEqual(expectedSanitizedOutput, ApplicationIdHelper.SanitizeString(input));
    }
}
