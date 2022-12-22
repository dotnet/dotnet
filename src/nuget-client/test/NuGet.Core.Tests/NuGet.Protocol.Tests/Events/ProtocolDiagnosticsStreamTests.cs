// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using Moq;
using NuGet.Protocol.Events;
using Xunit;

namespace NuGet.Protocol.Tests.Events
{
    public class ProtocolDiagnosticsStreamTests
    {
        [Fact]
        public void Dispose_CompletedStream_SendsSuccessfulEvent()
        {
            // Arrange
            var memoryStream = new MemoryStream(capacity: 100);
            ProtocolDiagnosticInProgressHttpEvent incompleteEvent = CreateInProgressEvent();
            var stopwatch = Stopwatch.StartNew();
            ProtocolDiagnosticHttpEvent completedEvent = null;
            void GotEvent(ProtocolDiagnosticHttpEvent @event)
            {
                Assert.Null(completedEvent);
                completedEvent = @event;
            }

            // Act
            using (var target = new ProtocolDiagnosticsStream(memoryStream, incompleteEvent, stopwatch, GotEvent))
            {
                ReadStream(target);
            }

            // Assert
            Assert.NotNull(completedEvent);
            Assert.True(completedEvent.IsSuccess);
        }

        [Fact]
        public void Dispose_IncompleteStream_SendsSuccessfulEvent()
        {
            // Arrange
            var memoryStream = new MemoryStream(capacity: 100);
            ProtocolDiagnosticInProgressHttpEvent inProgressEvent = CreateInProgressEvent();
            var stopwatch = Stopwatch.StartNew();
            ProtocolDiagnosticHttpEvent completedEvent = null;
            void GotEvent(ProtocolDiagnosticHttpEvent @event)
            {
                Assert.Null(completedEvent);
                completedEvent = @event;
            }

            // Act
            using (var target = new ProtocolDiagnosticsStream(memoryStream, inProgressEvent, stopwatch, GotEvent))
            {
            }

            // Assert
            Assert.NotNull(completedEvent);
            Assert.True(completedEvent.IsSuccess);
        }

        // This really should return a successful event, because the failure was outside the context of reading the stream.
        // If a source has bad data, we download it successfully, even if the contents are unusable.
        [Fact]
        public void Dispose_ExceptionDuringStreamProcessing_SendsSuccessfulEvent()
        {
            // Arrange
            var memoryStream = new MemoryStream(capacity: 100);
            ProtocolDiagnosticInProgressHttpEvent inProgressEvent = CreateInProgressEvent();
            var stopwatch = Stopwatch.StartNew();
            ProtocolDiagnosticHttpEvent completedEvent = null;
            void GotEvent(ProtocolDiagnosticHttpEvent @event)
            {
                Assert.Null(completedEvent);
                completedEvent = @event;
            }

            // Act
            Action action = () =>
            {
                using (var target = new ProtocolDiagnosticsStream(memoryStream, inProgressEvent, stopwatch, GotEvent))
                {
                    throw new InvalidOperationException();
                }
            };
            Assert.Throws<InvalidOperationException>(action);

            // Assert
            Assert.NotNull(completedEvent);
            Assert.True(completedEvent.IsSuccess);
        }

        [Fact]
        public void Dispose_ExceptionDuringStreamRead_SendsFailedEvent()
        {
            // Arrange
            var stream = new Mock<Stream>();
            stream.Setup(s => s.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws<TimeoutException>();
            ProtocolDiagnosticInProgressHttpEvent inProgressEvent = CreateInProgressEvent();
            var stopwatch = Stopwatch.StartNew();
            ProtocolDiagnosticHttpEvent completedEvent = null;
            void GotEvent(ProtocolDiagnosticHttpEvent @event)
            {
                Assert.Null(completedEvent);
                completedEvent = @event;
            }

            // Act
            Action action = () =>
            {
                using (var target = new ProtocolDiagnosticsStream(stream.Object, inProgressEvent, stopwatch, GotEvent))
                {
                    ReadStream(target);
                }
            };
            Assert.Throws<TimeoutException>(action);

            // Assert
            Assert.NotNull(completedEvent);
            Assert.False(completedEvent.IsSuccess);
        }

        private ProtocolDiagnosticInProgressHttpEvent CreateInProgressEvent()
        {
            return new ProtocolDiagnosticInProgressHttpEvent(
                source: "https://source.test/",
                url: new Uri("https://source.test/resource"),
                headerDuration: null,
                httpStatusCode: null,
                isRetry: false,
                isCancelled: false,
                isLastAttempt: false);
        }

        private void ReadStream(ProtocolDiagnosticsStream target)
        {
            var buffer = new byte[8 * 1024];
            while (target.Read(buffer, 0, buffer.Length) > 0)
            {
            }
        }
    }
}
