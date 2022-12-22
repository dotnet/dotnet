// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class MonitorNuGetProcessExitRequestTests
    {
        private static readonly int _processId = 7;

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        public void Constructor_AcceptsAnyProcessId(int processId)
        {
            new MonitorNuGetProcessExitRequest(processId);
        }

        [Fact]
        public void Constructor_InitializesProcessIdProperty()
        {
            var request = new MonitorNuGetProcessExitRequest(_processId);

            Assert.Equal(_processId, request.ProcessId);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var request = new MonitorNuGetProcessExitRequest(_processId);

            var json = TestUtilities.Serialize(request);

            Assert.Equal($"{{\"ProcessId\":{_processId}}}", json);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObject()
        {
            var json = $"{{\"ProcessId\":{_processId}}}";
            var request = JsonSerializationUtilities.Deserialize<MonitorNuGetProcessExitRequest>(json);

            Assert.Equal(_processId, request.ProcessId);
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("{\"ProcessId\":null}")]
        [InlineData("{\"ProcessId\":\"\"}")]
        public void JsonDeserialization_ThrowsForInvalidProcessId(string json)
        {
            Assert.Throws<JsonSerializationException>(
                () => JsonSerializationUtilities.Deserialize<MonitorNuGetProcessExitRequest>(json));
        }
    }
}
