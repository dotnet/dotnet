// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics.Tracing;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.WsFederation;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml;
using Microsoft.IdentityModel.Tokens.Saml2;
using Microsoft.IdentityModel.Xml;
using Xunit;

namespace Microsoft.IdentityModel.Logging.Tests
{
    public class PIITests
    {
        // Used for formatting a message for testing with one parameter.
        private const string TestMessageOneParam = "This is the parameter: '{0}'.";
        // Used for formatting a message for testing with two parameters.
        private const string TestMessageTwoParams = "This is the first parameter '{0}'. This is the second parameter '{1}'.";

        [Fact]
        public void LogOpenIdConnectProtocolExceptions()
        {
            SampleListener listener = new SampleListener();
            IdentityModelEventSource.Logger.LogLevel = EventLevel.Error;
            listener.EnableEvents(IdentityModelEventSource.Logger, EventLevel.Error);

            var exception1 = LogHelper.LogExceptionMessage(new ArgumentNullException("test1"));
            var exception2 = LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant(TestMessageTwoParams, "test2", LogHelper.MarkAsNonPII("test3")), exception1));
            var exception3 = LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidAtHashException(LogHelper.FormatInvariant(TestMessageOneParam, "test4"), exception2));
            var exception4 = LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidCHashException(LogHelper.FormatInvariant(TestMessageOneParam, "test5"), exception3));
            var exception5 = LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant(TestMessageOneParam, "test6"), exception4));
            var exception6 = LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidStateException(LogHelper.FormatInvariant(TestMessageOneParam, "test7"), exception5));

            Assert.Contains("test1", listener.TraceBuffer);
            Assert.DoesNotContain("test2", listener.TraceBuffer);
            Assert.Contains("test3", listener.TraceBuffer);
            Assert.DoesNotContain("test4", listener.TraceBuffer);
            Assert.DoesNotContain("test5", listener.TraceBuffer);
            Assert.DoesNotContain("test6", listener.TraceBuffer);
            Assert.DoesNotContain("test7", listener.TraceBuffer);
        }

        [Fact]
        public void LogWsFederationExceptions()
        {
            SampleListener listener = new SampleListener();
            IdentityModelEventSource.Logger.LogLevel = EventLevel.Error;
            listener.EnableEvents(IdentityModelEventSource.Logger, EventLevel.Error);

            var exception1 = LogHelper.LogExceptionMessage(new ArgumentNullException("test1"));
            var exception2 = LogHelper.LogExceptionMessage(new WsFederationException(LogHelper.FormatInvariant(TestMessageTwoParams, "test2", LogHelper.MarkAsNonPII("test3")), exception1));
            var exception3 = LogHelper.LogExceptionMessage(new WsFederationReadException(LogHelper.FormatInvariant(TestMessageOneParam, "test4"), exception2));

            Assert.Contains("test1", listener.TraceBuffer);
            Assert.DoesNotContain("test2", listener.TraceBuffer);
            Assert.Contains("test3", listener.TraceBuffer);
            Assert.DoesNotContain("test4", listener.TraceBuffer);
        }

        [Fact]
        public void LogTokenExceptions()
        {
            SampleListener listener = new SampleListener();
            IdentityModelEventSource.Logger.LogLevel = EventLevel.Error;
            listener.EnableEvents(IdentityModelEventSource.Logger, EventLevel.Error);

            var exception1 = LogHelper.LogExceptionMessage(new ArgumentNullException("test1"));
            var exception2 = LogHelper.LogExceptionMessage(new SecurityTokenDecryptionFailedException(LogHelper.FormatInvariant(TestMessageTwoParams, "test2", LogHelper.MarkAsNonPII("test3")), exception1));
            var exception3 = LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogHelper.FormatInvariant(TestMessageOneParam, "test4"), exception2));
            var exception4 = LogHelper.LogExceptionMessage(new SecurityTokenEncryptionKeyNotFoundException(LogHelper.FormatInvariant(TestMessageOneParam, "test5"), exception3));
            var exception5 = LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(TestMessageOneParam, "test6"), exception4));
            var exception6 = LogHelper.LogExceptionMessage(new SecurityTokenExpiredException(LogHelper.FormatInvariant(TestMessageOneParam, "test7"), exception5));
            var exception7 = LogHelper.LogExceptionMessage(new SecurityTokenInvalidAudienceException(LogHelper.FormatInvariant(TestMessageOneParam, "test8"), exception6));
            var exception8 = LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException(LogHelper.FormatInvariant(TestMessageOneParam, "test9"), exception7));
            var exception9 = LogHelper.LogExceptionMessage(new SecurityTokenInvalidLifetimeException(LogHelper.FormatInvariant(TestMessageOneParam, "test10"), exception8));
            var exception10 = LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TestMessageOneParam, "test11"), exception9));
            var exception11 = LogHelper.LogExceptionMessage(new SecurityTokenInvalidSigningKeyException(LogHelper.FormatInvariant(TestMessageOneParam, "test12"), exception10));
            var exception12 = LogHelper.LogExceptionMessage(new SecurityTokenKeyWrapException(LogHelper.FormatInvariant(TestMessageOneParam, "test13"), exception11));
            var exception13 = LogHelper.LogExceptionMessage(new SecurityTokenNoExpirationException(LogHelper.FormatInvariant(TestMessageOneParam, "test14"), exception12));
            var exception14 = LogHelper.LogExceptionMessage(new SecurityTokenNotYetValidException(LogHelper.FormatInvariant(TestMessageOneParam, "test15"), exception13));
            var exception15 = LogHelper.LogExceptionMessage(new SecurityTokenReplayAddFailedException(LogHelper.FormatInvariant(TestMessageOneParam, "test16"), exception14));
            var exception16 = LogHelper.LogExceptionMessage(new SecurityTokenReplayDetectedException(LogHelper.FormatInvariant(TestMessageOneParam, "test17"), exception15));
            var exception17 = LogHelper.LogExceptionMessage(new SecurityTokenSignatureKeyNotFoundException(LogHelper.FormatInvariant(TestMessageOneParam, "test18"), exception16));
            var exception18 = LogHelper.LogExceptionMessage(new SecurityTokenValidationException(LogHelper.FormatInvariant(TestMessageOneParam, "test19"), exception17));

            Assert.Contains("test1", listener.TraceBuffer);
            Assert.DoesNotContain("test2", listener.TraceBuffer);
            Assert.Contains("test3", listener.TraceBuffer);
            Assert.DoesNotContain("test4", listener.TraceBuffer);
            Assert.DoesNotContain("test5", listener.TraceBuffer);
            Assert.DoesNotContain("test6", listener.TraceBuffer);
            Assert.DoesNotContain("test7", listener.TraceBuffer);
            Assert.DoesNotContain("test8", listener.TraceBuffer);
            Assert.DoesNotContain("test9", listener.TraceBuffer);
            Assert.DoesNotContain("test10", listener.TraceBuffer);
            Assert.DoesNotContain("test11", listener.TraceBuffer);
            Assert.DoesNotContain("test12", listener.TraceBuffer);
            Assert.DoesNotContain("test13", listener.TraceBuffer);
            Assert.DoesNotContain("test14", listener.TraceBuffer);
            Assert.DoesNotContain("test15", listener.TraceBuffer);
            Assert.DoesNotContain("test16", listener.TraceBuffer);
            Assert.DoesNotContain("test17", listener.TraceBuffer);
            Assert.DoesNotContain("test18", listener.TraceBuffer);
            Assert.DoesNotContain("test19", listener.TraceBuffer);
        }

        [Fact]
        public void LogSamlExceptions()
        {
            SampleListener listener = new SampleListener();
            IdentityModelEventSource.Logger.LogLevel = EventLevel.Error;
            listener.EnableEvents(IdentityModelEventSource.Logger, EventLevel.Error);

            var exception1 = LogHelper.LogExceptionMessage(new ArgumentNullException("test1"));
            var exception2 = LogHelper.LogExceptionMessage(new SamlSecurityTokenException(LogHelper.FormatInvariant(TestMessageTwoParams, "test2", LogHelper.MarkAsNonPII("test3")), exception1));
            var exception3 = LogHelper.LogExceptionMessage(new SamlSecurityTokenReadException(LogHelper.FormatInvariant(TestMessageOneParam, "test4"), exception2));
            var exception4 = LogHelper.LogExceptionMessage(new SamlSecurityTokenWriteException(LogHelper.FormatInvariant(TestMessageOneParam, "test5"), exception3));

            Assert.Contains("test1", listener.TraceBuffer);
            Assert.DoesNotContain("test2", listener.TraceBuffer);
            Assert.Contains("test3", listener.TraceBuffer);
            Assert.DoesNotContain("test4", listener.TraceBuffer);
            Assert.DoesNotContain("test5", listener.TraceBuffer);
        }

        [Fact]
        public void LogSaml2Exceptions()
        {
            SampleListener listener = new SampleListener();
            IdentityModelEventSource.Logger.LogLevel = EventLevel.Error;
            listener.EnableEvents(IdentityModelEventSource.Logger, EventLevel.Error);

            var exception1 = LogHelper.LogExceptionMessage(new ArgumentNullException("test1"));
            var exception2 = LogHelper.LogExceptionMessage(new Saml2SecurityTokenException(LogHelper.FormatInvariant(TestMessageTwoParams, "test2", LogHelper.MarkAsNonPII("test3")), exception1));
            var exception3 = LogHelper.LogExceptionMessage(new Saml2SecurityTokenReadException(LogHelper.FormatInvariant(TestMessageOneParam, "test4"), exception2));
            var exception4 = LogHelper.LogExceptionMessage(new Saml2SecurityTokenWriteException(LogHelper.FormatInvariant(TestMessageOneParam, "test5"), exception3));

            Assert.Contains("test1", listener.TraceBuffer);
            Assert.DoesNotContain("test2", listener.TraceBuffer);
            Assert.Contains("test3", listener.TraceBuffer);
            Assert.DoesNotContain("test4", listener.TraceBuffer);
            Assert.DoesNotContain("test5", listener.TraceBuffer);
        }

        [Fact]
        public void LogXmlExceptions()
        {
            SampleListener listener = new SampleListener();
            IdentityModelEventSource.Logger.LogLevel = EventLevel.Error;
            listener.EnableEvents(IdentityModelEventSource.Logger, EventLevel.Error);

            var exception1 = LogHelper.LogExceptionMessage(new ArgumentNullException("test1"));
            var exception2 = LogHelper.LogExceptionMessage(new XmlException(LogHelper.FormatInvariant(TestMessageTwoParams, "test2", LogHelper.MarkAsNonPII("test3")), exception1));
            var exception3 = LogHelper.LogExceptionMessage(new XmlReadException(LogHelper.FormatInvariant(TestMessageOneParam, "test4"), exception2));
            var exception4 = LogHelper.LogExceptionMessage(new XmlValidationException(LogHelper.FormatInvariant(TestMessageOneParam, "test5"), exception3));
            var exception5 = LogHelper.LogExceptionMessage(new XmlWriteException(LogHelper.FormatInvariant(TestMessageOneParam, "test6"), exception4));

            Assert.Contains("test1", listener.TraceBuffer);
            Assert.DoesNotContain("test2", listener.TraceBuffer);
            Assert.Contains("test3", listener.TraceBuffer);
            Assert.DoesNotContain("test4", listener.TraceBuffer);
            Assert.DoesNotContain("test5", listener.TraceBuffer);
        }

        [Fact]
        public void LogNullArgument()
        {
            SampleListener listener = new SampleListener();
            IdentityModelEventSource.Logger.LogLevel = EventLevel.Error;
            listener.EnableEvents(IdentityModelEventSource.Logger, EventLevel.Error);

            string algorithm = null;

            LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("Algorithm not supported exception 1: {0}", algorithm)));
            LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("Algorithm not supported exception 2: {0}", LogHelper.MarkAsNonPII(algorithm))));

            Assert.Contains("Algorithm not supported exception 1: [PII of type 'Null' is hidden. For more details, see https://aka.ms/IdentityModel/PII.]", listener.TraceBuffer);
            Assert.Contains("Algorithm not supported exception 2: Null", listener.TraceBuffer);
        }

        
        [Fact]
        public void LogExceptionAsArgument()
        {
            SampleListener listener = new SampleListener();
            IdentityModelEventSource.Logger.LogLevel = EventLevel.Error;
            listener.EnableEvents(IdentityModelEventSource.Logger, EventLevel.Error);

            LogHelper.LogExceptionMessage(EventLevel.Error, new ArgumentException(LogHelper.FormatInvariant("Main exception 1: {0}", new SecurityTokenCompressionFailedException("custom inner exception"))));
            LogHelper.LogExceptionMessage(EventLevel.Error, new ArgumentException(LogHelper.FormatInvariant("Main exception 2: {0}", new InvalidOperationException("system exception"))));

            Assert.Contains("Main exception 1: Microsoft.IdentityModel.Tokens.SecurityTokenCompressionFailedException: custom inner exception", listener.TraceBuffer);
            Assert.Contains("Main exception 2: [PII of type 'System.InvalidOperationException' is hidden. For more details, see https://aka.ms/IdentityModel/PII.]", listener.TraceBuffer);
        }
    }
}
