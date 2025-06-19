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
