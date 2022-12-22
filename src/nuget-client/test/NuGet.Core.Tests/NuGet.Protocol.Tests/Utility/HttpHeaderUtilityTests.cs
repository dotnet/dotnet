// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net.Http;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.Protocol.Tests.Utility
{
    public class HttpResponseMessageExtensionsTests
    {
        private readonly ITestOutputHelper _output;

        public HttpResponseMessageExtensionsTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void LogsServerWarningWhenNuGetWarningHeaderPresent()
        {
            var testLogger = new TestLogger(_output);
            var response = new HttpResponseMessage();
            response.Headers.Add(ProtocolConstants.ServerWarningHeader, "test");

            response.LogServerWarning(testLogger);

            Assert.Equal(1, testLogger.Warnings);

            string warning;
            Assert.True(testLogger.Messages.TryDequeue(out warning));
            Assert.Equal("test", warning);
        }
    }
}
