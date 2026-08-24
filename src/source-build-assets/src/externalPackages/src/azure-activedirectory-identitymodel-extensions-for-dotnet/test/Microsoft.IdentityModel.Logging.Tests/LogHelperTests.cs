// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Logging.Tests
{
    using System;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Xunit;

    // Leveraging Xunit collections: https://xunit.net/docs/running-tests-in-parallel#parallelism-in-test-frameworks
    // Currently this does nothing but if we face issues with tests colliding trying to access ShowPII and LogCompleteSecurityArtifact
    // this collection can be used to prevent race conditions.
    [Collection("Relying on ShowPII and LogCompleteSecurityArtifact")]
    public class LogHelperTests
    {
        private const string SafeFormat = "Safe {0}";
        private const string UnsafeFormat = "Unsafe {0}";

        [Fact]
        public void MarkAsSecurityArtifact_ReturnsSecurityArtifactInstance()
        {
            object arg = "argument";
            var result = LogHelper.MarkAsSecurityArtifact(arg,
                obj => string.Format(SafeFormat, obj),
                obj => string.Format(UnsafeFormat, obj));

            Assert.IsType<SecurityArtifact>(result);
        }

        [Fact]
        public void MarkAsUnsafeOnlySecurityArtifact_ReturnsSecurityArtifactInstance()
        {
            object arg = "argument";
            var result = LogHelper.MarkAsUnsafeSecurityArtifact(arg,
                obj => string.Format(UnsafeFormat, obj));

            Assert.IsType<SecurityArtifact>(result);
        }

        [Fact]
        public void MarkAsSecurityArtifact_ArgumentIsNull_NoException()
        {
            // Asserting no exception is thrown for a null argument
            LogHelper.MarkAsSecurityArtifact(null,
               obj => string.Format(SafeFormat, obj),
               obj => string.Format(UnsafeFormat, obj));
        }

        [Fact]
        public void MarkAsSecurityArtifactSafeCallbackIsNull_NoException()
        {
            object arg = "argument";
            LogHelper.MarkAsSecurityArtifact(arg,
                null,
                obj => string.Format(UnsafeFormat, obj));
        }

        [Fact]
        public void MarkAsSecurityArtifactUnsafeCallbackIsNull_NoException()
        {
            object arg = "argument";
            LogHelper.MarkAsSecurityArtifact(arg,
                obj => string.Format(SafeFormat, obj),
                null);
        }

        [Fact]
        public void MarkAsUnsafeOnlySecurityArtifact_ArgumentIsNull_NoException()
        {
            LogHelper.MarkAsUnsafeSecurityArtifact(
                null,
                obj => string.Format(UnsafeFormat, obj));
        }

        [Fact]
        public void MarkAsUnsafeOnlySecurityArtifactUnsafeCallbackIsNull_NoException()
        {
            object arg = "argument";
            LogHelper.MarkAsUnsafeSecurityArtifact(arg, null);
        }

        [Fact]
        public void FormatInvariant_NullFormat_ReturnsEmptyString()
        {
            // Arrange
            string format = null;
            object[] args = new object[] { "arg1", "arg2" };

            // Act
            var result = LogHelper.FormatInvariant(format, args);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void FormatInvariant_NullArgs_ReturnsFormatString()
        {
            // Arrange
            string format = "This is a {0} string.";
            object[] args = null;

            // Act
            var result = LogHelper.FormatInvariant(format, args);

            // Assert
            Assert.Equal(format, result);
        }

        [Fact]
        public void FormatInvariant_ShowPIIEnabled_ReturnsPIIData()
        {
            // Arrange
            string format = "This is a {0} string.";
            object[] args = new object[] { "sensitive data" };
            IdentityModelEventSource.ShowPII = true;

            // Act
            var result = LogHelper.FormatInvariant(format, args);

            // Assert
            Assert.Equal("This is a sensitive data string.", result);

            // Reset for other tests
            IdentityModelEventSource.ShowPII = false;
        }

        [Fact]
        public void FormatInvariant_ShowPIIDisabled_ReturnsSanitizedData()
        {
            // Arrange
            string format = "This is a {0} string.";
            object[] args = new object[] { "sensitive data" };
            IdentityModelEventSource.ShowPII = false;

            // Act
            var result = LogHelper.FormatInvariant(format, args);

            // Assert
            Assert.Equal(
                string.Format(format, string.Format(IdentityModelEventSource.HiddenPIIString, args[0].GetType().ToString())),
                result);

            // Reset for other tests
            IdentityModelEventSource.ShowPII = false;
        }

        [Fact]
        public void FormatInvariant_ShowPIIDisabled_ArtifactDisabled_ReturnsSanitizedData()
        {
            // Arrange
            string format = "PII Data: {0} and Token Data: {1}.";
            object[] args = new object[] { "sensitive data", new MockSecurityToken() };
            IdentityModelEventSource.ShowPII = false;
            IdentityModelEventSource.LogCompleteSecurityArtifact = false;

            // Act
            var result = LogHelper.FormatInvariant(format, args);

            // Assert
            // NOTE: Here that we scrub the token showing PII hidden not SecurityArtifact hidden for the token.
            Assert.Equal(
                string.Format(
                    format,
                    string.Format(IdentityModelEventSource.HiddenPIIString, args[0].GetType().ToString()),
                    string.Format(IdentityModelEventSource.HiddenPIIString, args[1].GetType().ToString())),
                result);
        }

        [Fact]
        public void FormatInvariant_ShowPIIEnabled_ArtifactEnabled_ReturnsUnscrubbedData()
        {
            // Arrange
            string format = "PII Data: {0} and Token Data: {1}.";
            object[] args = new object[] { "sensitive data", new MockSecurityToken() };
            IdentityModelEventSource.ShowPII = true;
            IdentityModelEventSource.LogCompleteSecurityArtifact = true;

            // Act
            var result = LogHelper.FormatInvariant(format, args);

            // Assert
            Assert.Equal(
                string.Format(
                    format,
                    args[0].ToString(),
                    ((ISafeLogSecurityArtifact)args[1]).UnsafeToString()),
                result);

            // Reset for other tests
            IdentityModelEventSource.ShowPII = false;
            IdentityModelEventSource.LogCompleteSecurityArtifact = false;
        }

        [Fact]
        public void FormatInvariant_ArtifactEnabled_ShowPIIDisabled_ExplicitlyMarkedProperty_ReturnsSanitizedData()
        {
            // Arrange
            string format = "PII Data: {0} and Token Data: {1}.";
            object[] args = new object[] { LogHelper.MarkAsNonPII("data"), LogHelper.MarkAsSecurityArtifact("token", t => "safe token") };
            IdentityModelEventSource.LogCompleteSecurityArtifact = true;

            // Act
            var result = LogHelper.FormatInvariant(format, args);

            // Assert
            // NOTE: here the security artifact is scrubbed for PII even though the LogCompleteSecurityArtifact is true.
            // artifacts often can and do have PII in them so if PII is off, never log non-specific token data.
            Assert.Equal(
                string.Format(
                    format,
                    "data",
                    string.Format(IdentityModelEventSource.HiddenPIIString, args[1].GetType().ToString())),
                result);

            // Reset for other tests
            IdentityModelEventSource.LogCompleteSecurityArtifact = false;
        }

        [Fact]
        public void FormatInvariant_ArtifactEnabled_ShowPIIEnabled_ExplicitlyMarkedProperty_ReturnsDisarmedToken()
        {
            // Arrange
            string format = "PII Data: {0} and Token Data: {1}.";
            object[] args = new object[] { LogHelper.MarkAsNonPII("data"), LogHelper.MarkAsSecurityArtifact("token", t => "safe token") };
            IdentityModelEventSource.ShowPII = true;
            IdentityModelEventSource.LogCompleteSecurityArtifact = true;

            // Act
            var result = LogHelper.FormatInvariant(format, args);

            // Assert
            // NOTE: here the security artifact logs the disarmed token EVEN THOUGH LogCompleteSecurityArtifact is true. This is because no
            // callback was provided to return an unsafe string so we default to the safe string.
            Assert.Equal(
                string.Format(
                    format,
                    "data",
                    "safe token"),
                result);

            // Reset for other tests
            IdentityModelEventSource.ShowPII = false;
            IdentityModelEventSource.LogCompleteSecurityArtifact = false;
        }

        [Fact]
        public void FormatInvariant_ArtifactEnabled_ShowPIIEnabled_ExplicitlyMarkedProperty_NullArgument()
        {
            // Arrange
            string format = "PII Data: {0} and Token Data: {1}.";
            object[] args = new object[] { LogHelper.MarkAsNonPII("data"), LogHelper.MarkAsSecurityArtifact(null, t => "safe token") };
            IdentityModelEventSource.ShowPII = true;
            IdentityModelEventSource.LogCompleteSecurityArtifact = true;

            // Act
            var result = LogHelper.FormatInvariant(format, args);

            // Assert
            Assert.Equal(
                string.Format(
                    format,
                    "data",
                    "null"),
                result);

            // Reset for other tests
            IdentityModelEventSource.ShowPII = false;
            IdentityModelEventSource.LogCompleteSecurityArtifact = false;
        }

        [Fact]
        public void FormatInvariant_ArtifactEnabled_ShowPIIEnabled_ExplicitlyMarkedPropertyNullCallback_ReturnsDefaultScrub()
        {
            // Arrange
            string format = "PII Data: {0} and Token Data: {1}.";
            object[] args = new object[] { LogHelper.MarkAsNonPII("data"), LogHelper.MarkAsSecurityArtifact("token", null) };
            IdentityModelEventSource.ShowPII = true;
            IdentityModelEventSource.LogCompleteSecurityArtifact = true;

            // Act
            var result = LogHelper.FormatInvariant(format, args);

            // Assert
            // NOTE: here the security artifact logs the disarmed token EVEN THOUGH LogCompleteSecurityArtifact is true. This is because no
            // callback was provided to return an unsafe string so we default to the safe string.
            Assert.Equal(
                string.Format(
                    format,
                    "data",
                    "#ScrubbedArtifact#"),
                result);

            // Reset for other tests
            IdentityModelEventSource.ShowPII = false;
            IdentityModelEventSource.LogCompleteSecurityArtifact = false;
        }

        [Fact]
        public void FormatInvariant_ArtifactEnabled_ShowPIIEnabled_ExplicitlyMarkedUnsafePropertyNullCallback_ReturnsDefaultScrub()
        {
            // Arrange
            string format = "PII Data: {0} and Token Data: {1}.";
            object[] args = new object[] { LogHelper.MarkAsNonPII("data"), LogHelper.MarkAsUnsafeSecurityArtifact("token", null) };
            IdentityModelEventSource.ShowPII = true;
            IdentityModelEventSource.LogCompleteSecurityArtifact = true;

            // Act
            var result = LogHelper.FormatInvariant(format, args);

            // Assert
            // NOTE: here it logs a generic scrubbed string EVEN THOUGH LogCompleteSecurityArtifact is true. This is because no
            // callback was provided to return an unsafe string so we default to the safe string.
            Assert.Equal(
                string.Format(
                    format,
                    "data",
                    "#ScrubbedArtifact#"),
                result);

            // Reset for other tests
            IdentityModelEventSource.ShowPII = false;
            IdentityModelEventSource.LogCompleteSecurityArtifact = false;
        }

        [Fact]
        public void FormatInvariant_ArtifactEnabled_ShowPIIEnabled_ExplicitlyMarkedProperty_ReturnsTokenData()
        {
            // Arrange
            string format = "PII Data: {0} and Token Data: {1}.";
            object[] args = new object[] { LogHelper.MarkAsNonPII("data"), LogHelper.MarkAsSecurityArtifact("token", t => "safe token", t => t.ToString()) };
            IdentityModelEventSource.ShowPII = true;
            IdentityModelEventSource.LogCompleteSecurityArtifact = true;

            // Act
            var result = LogHelper.FormatInvariant(format, args);

            // Assert
            Assert.Equal(
                string.Format(
                    format,
                    "data",
                    "token"),
                result);

            // Reset for other tests
            IdentityModelEventSource.ShowPII = false;
            IdentityModelEventSource.LogCompleteSecurityArtifact = false;
        }

        [Fact]
        public void FormatInvariant_ArtifactEnabled_ShowPIIEnabled_ExplicitlyMarkedUnsafeProperty_ReturnsTokenData()
        {
            // Arrange
            string format = "PII Data: {0} and Token Data: {1}.";
            object[] args = new object[] { LogHelper.MarkAsNonPII("data"), LogHelper.MarkAsUnsafeSecurityArtifact("token", t => t.ToString()) };
            IdentityModelEventSource.ShowPII = true;
            IdentityModelEventSource.LogCompleteSecurityArtifact = true;

            // Act
            var result = LogHelper.FormatInvariant(format, args);

            // Assert
            Assert.Equal(
                string.Format(
                    format,
                    "data",
                    "token"),
                result);

            // Reset for other tests
            IdentityModelEventSource.ShowPII = false;
            IdentityModelEventSource.LogCompleteSecurityArtifact = false;
        }

        [Fact]
        public void FormatInvariant_MultipleNonPIIArgs_NoPII_ReturnsFormattedString()
        {
            // Arrange
            string format = "This is a {0} and {1} string.";
            object[] args = new object[] { LogHelper.MarkAsNonPII("first"), LogHelper.MarkAsNonPII("second") };

            // Act
            var result = LogHelper.FormatInvariant(format, args);

            // Assert
            Assert.Equal("This is a first and second string.", result);
        }

        [Fact]
        public void FormatInvariant_MixedArgs_ShowPII_ReturnsFormattedString()
        {
            // Arrange
            string format = "This is a {0} and {1} string.";
            object[] args = new object[] { LogHelper.MarkAsNonPII("first"), "second" };
            IdentityModelEventSource.ShowPII = true;

            // Act
            var result = LogHelper.FormatInvariant(format, args);

            // Assert
            Assert.Equal("This is a first and second string.", result);

            // Reset for other tests
            IdentityModelEventSource.ShowPII = false;
        }

        [Fact]
        public void FormatInvariant_MixedArgs_NoPII_ReturnsFormattedString()
        {
            // Arrange
            string format = "This is a {0} and {1} string.";
            object[] args = new object[] { LogHelper.MarkAsNonPII("first"), "second" };

            // Act
            var result = LogHelper.FormatInvariant(format, args);

            // Assert
            Assert.Equal(
                string.Format(
                    format,
                    args[0].ToString(),
                    string.Format(IdentityModelEventSource.HiddenPIIString, args[1].GetType().ToString())),
                result);
        }

        [Fact]
        public void FormatInvariant_NoArgs_ReturnsFormatString()
        {
            // Arrange
            string format = "This is a string with no arguments.";

            // Act
            var result = LogHelper.FormatInvariant(format);

            // Assert
            Assert.Equal("This is a string with no arguments.", result);
        }

        [Fact]
        public void FormatInvariant_NonPIIArgument_SanitizesSpecialCharacters()
        {
            string format = "Value: {0}";
            object[] args = new object[] { LogHelper.MarkAsNonPII("A\rB\nC\tD\r\nE" + (char)2) };

            string result = LogHelper.FormatInvariant(format, args);

            Assert.Equal("Value: A\\rB\\nC\\tD\\r\\nE\\u0002", result);
        }

        [Fact]
        public void FormatInvariant_PIIArgument_DoesNotSanitizeWhenShowPIIDisabled()
        {
            string format = "Value: {0}";
            object[] args = new object[] { "A\rB\nC\tD\r\nE" + (char)2 };
            IdentityModelEventSource.ShowPII = false;

            string result = LogHelper.FormatInvariant(format, args);

            Assert.Equal($"Value: {string.Format(IdentityModelEventSource.HiddenPIIString, typeof(string).ToString())}", result);

            IdentityModelEventSource.ShowPII = false;
        }

        [Fact]
        public void FormatInvariant_PIIArgument_SanitizeWhenShowPIIEnabled()
        {
            string format = "Value: {0}";
            object[] args = new object[] { "A\rB\nC\tD\r\nE" + (char)2 };
            IdentityModelEventSource.ShowPII = true;

            string result = LogHelper.FormatInvariant(format, args);

            Assert.Equal("Value: A\\rB\\nC\\tD\\r\\nE\\u0002", result);

            IdentityModelEventSource.ShowPII = false;
        }

        [Fact]
        public void FormatInvariant_NonPIIArgument_SanitizesUnicodeFormatCharacters()
        {
            // U+200B ZERO WIDTH SPACE and U+2060 WORD JOINER are Unicode format characters
            string format = "Value: {0}";
            string input = "A" + '\u200B' + "B" + '\u2060' + "C";

            string result = LogHelper.FormatInvariant(format, LogHelper.MarkAsNonPII(input));

            // Both format characters should be replaced with their \uXXXX representation
            Assert.Equal("Value: A\\u200BB\\u2060C", result);
        }

        [Fact]
        public void FormatInvariant_EmptyString_ReturnsEmptyString()
        {
            string format = "Value: {0}";
            string input = "";

            string result = LogHelper.FormatInvariant(format, LogHelper.MarkAsNonPII(input));

            Assert.Equal("Value: ", result);
        }

        [Fact]
        public void FormatInvariant_NoSpecialCharacters_ReturnsUnchanged()
        {
            string format = "Value: {0}";
            string input = "This is a normal string with no special characters.";

            string result = LogHelper.FormatInvariant(format, LogHelper.MarkAsNonPII(input));

            Assert.Equal("Value: This is a normal string with no special characters.", result);
        }

        [Fact]
        public void FormatInvariant_OnlyControlCharacters_SanitizesAll()
        {
            string format = "Value: {0}";
            string input = "\u0000\u0001\u0002\u0003";

            string result = LogHelper.FormatInvariant(format, LogHelper.MarkAsNonPII(input));

            Assert.Equal("Value: \\u0000\\u0001\\u0002\\u0003", result);
        }

        [Fact]
        public void FormatInvariant_MixedNormalAndControlCharacters()
        {
            string format = "Value: {0}";
            string input = "Start\u0000Middle\u0001End";

            string result = LogHelper.FormatInvariant(format, LogHelper.MarkAsNonPII(input));

            Assert.Equal("Value: Start\\u0000Middle\\u0001End", result);
        }

        [Fact]
        public void FormatInvariant_AllASCIIControlCharacters()
        {
            string format = "Value: {0}";
            // Test all ASCII control characters (0x00-0x1F and 0x7F-0x9F)
            string input = "A\u007FB\u0080C\u009FD";

            string result = LogHelper.FormatInvariant(format, LogHelper.MarkAsNonPII(input));

            Assert.Equal("Value: A\\u007FB\\u0080C\\u009FD", result);
        }

        [Fact]
        public void FormatInvariant_DirectionalFormatCharacters()
        {
            string format = "Value: {0}";
            // U+200E (Left-to-Right Mark), U+200F (Right-to-Left Mark), U+202A-U+202E (Directional formatting)
            string input = "A\u200EB\u200FC\u202AD\u202EE";

            string result = LogHelper.FormatInvariant(format, LogHelper.MarkAsNonPII(input));

            Assert.Equal("Value: A\\u200EB\\u200FC\\u202AD\\u202EE", result);
        }

        [Fact]
        public void FormatInvariant_ZeroWidthCharacters()
        {
            string format = "Value: {0}";
            // U+200B (Zero Width Space), U+200C (Zero Width Non-Joiner), U+200D (Zero Width Joiner), U+FEFF (Zero Width No-Break Space)
            string input = "A\u200BB\u200CC\u200DD\uFEFFE";

            string result = LogHelper.FormatInvariant(format, LogHelper.MarkAsNonPII(input));

            Assert.Equal("Value: A\\u200BB\\u200CC\\u200DD\\uFEFFE", result);
        }

        [Fact]
        public void FormatInvariant_MultipleConsecutiveSpecialCharacters()
        {
            string format = "Value: {0}";
            string input = "A\r\n\tB";

            string result = LogHelper.FormatInvariant(format, LogHelper.MarkAsNonPII(input));

            Assert.Equal("Value: A\\r\\n\\tB", result);
        }

        [Fact]
        public void FormatInvariant_SpecialCharactersAtStartAndEnd()
        {
            string format = "Value: {0}";
            string input = "\rStart and End\n";

            string result = LogHelper.FormatInvariant(format, LogHelper.MarkAsNonPII(input));

            Assert.Equal("Value: \\rStart and End\\n", result);
        }

        [Fact]
        public void FormatInvariant_LongStringWithSparseSpecialCharacters()
        {
            string format = "Value: {0}";
            string input = "This is a long string " + '\u200B' + " with some " + '\r' + " special " + '\n' + " characters " + '\t' + " scattered throughout.";

            string result = LogHelper.FormatInvariant(format, LogHelper.MarkAsNonPII(input));

            Assert.Equal("Value: This is a long string \\u200B with some \\r special \\n characters \\t scattered throughout.", result);
        }

        [Fact]
        public void FormatInvariant_ArabicFormatCharacters()
        {
            string format = "Value: {0}";
            // U+0600-U+0605, U+061C (Arabic format characters)
            string input = "A\u0600B\u061CC";

            string result = LogHelper.FormatInvariant(format, LogHelper.MarkAsNonPII(input));

            Assert.Equal("Value: A\\u0600B\\u061CC", result);
        }

        [Fact]
        public void FormatInvariant_SoftHyphen()
        {
            string format = "Value: {0}";
            // U+00AD (Soft Hyphen) is a format character
            string input = "A\u00ADB";

            string result = LogHelper.FormatInvariant(format, LogHelper.MarkAsNonPII(input));

            Assert.Equal("Value: A\\u00ADB", result);
        }

        [Fact]
        public void FormatInvariant_VariousFormatCharacters()
        {
            string format = "Value: {0}";
            // U+2060-U+206F (Various format characters)
            string input = "A\u2060B\u2061C\u206FD";

            string result = LogHelper.FormatInvariant(format, LogHelper.MarkAsNonPII(input));

            Assert.Equal("Value: A\\u2060B\\u2061C\\u206FD", result);
        }

        [Fact]
        public void FormatInvariant_InterlinearAnnotationCharacters()
        {
            string format = "Value: {0}";
            // U+FFF9-U+FFFB (Interlinear annotation characters)
            string input = "A\uFFF9B\uFFFAC\uFFFBD";

            string result = LogHelper.FormatInvariant(format, LogHelper.MarkAsNonPII(input));

            Assert.Equal("Value: A\\uFFF9B\\uFFFAC\\uFFFBD", result);
        }

        [Fact]
        public void FormatInvariant_PIIArgument_SanitizesWhenShowPIIEnabledWithAllCharacterTypes()
        {
            string format = "Value: {0}";
            // Mix of all character types: control, format, zero-width, directional
            string input = "A\rB\nC\tD\u0000E\u200BF\u200EG\u202AH\uFEFFI";
            IdentityModelEventSource.ShowPII = true;

            string result = LogHelper.FormatInvariant(format, input);

            Assert.Equal("Value: A\\rB\\nC\\tD\\u0000E\\u200BF\\u200EG\\u202AH\\uFEFFI", result);

            IdentityModelEventSource.ShowPII = false;
        }
    }

    public class MockSecurityToken : SecurityToken
    {
        public override string Id { get; }
        public override DateTime ValidFrom { get; }
        public override DateTime ValidTo { get; }

        public override string Issuer => throw new NotImplementedException();

        public override SecurityKey SecurityKey => throw new NotImplementedException();

        public override SecurityKey SigningKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override string UnsafeToString() => "#SECURITY TOKEN#";
    }
}
