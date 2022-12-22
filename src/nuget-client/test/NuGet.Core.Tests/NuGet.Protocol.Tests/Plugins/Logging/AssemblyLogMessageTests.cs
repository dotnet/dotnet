// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class AssemblyLogMessageTests : LogMessageTests
    {
        [Fact]
        public void ToString_ReturnsJson()
        {
            var now = DateTimeOffset.UtcNow;
            var logMessage = new AssemblyLogMessage(now);

            var message = VerifyOuterMessageAndReturnInnerMessage(logMessage, now, "assembly");

            Assert.Equal(4, message.Count);

            var actualAssemblyFullName = message.Value<string>("assembly full name");
            var actualFileVersion = message.Value<string>("file version");
            var actualInformationalVersion = message.Value<string>("informational version");
            var actualEntryAssemblyFullName = message.Value<string>("entry assembly full name");

            GetExpectedValues(
                out var expectedAssemblyFullName,
                out var expectedFileVersion,
                out var expectedActualInformationalVersion,
                out var expectedEntryAssemblyFullName);

            Assert.Equal(expectedAssemblyFullName, actualAssemblyFullName);
            Assert.Equal(expectedFileVersion, actualFileVersion);
            Assert.Equal(expectedActualInformationalVersion, actualInformationalVersion);
            Assert.Equal(expectedEntryAssemblyFullName, actualEntryAssemblyFullName);

        }

        private static void GetExpectedValues(
            out object expectedAssemblyFullName,
            out object expectedFileVersion,
            out object expectedActualInformationalVersion,
            out object expectedEntryAssemblyFullName)
        {
            var assembly = typeof(PluginFactory).Assembly;
            var informationalVersionAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            var fileVersionAttribute = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();

            expectedAssemblyFullName = assembly.FullName;
            expectedFileVersion = fileVersionAttribute.Version;
            expectedActualInformationalVersion = informationalVersionAttribute.InformationalVersion;
            expectedEntryAssemblyFullName = Assembly.GetEntryAssembly()?.FullName;
        }
    }
}
