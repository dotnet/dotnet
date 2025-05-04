// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class TelemetryOnceEmitterTests
    {
        private readonly ConcurrentQueue<TelemetryEvent> _telemetryEvents;
        private readonly NuGetVSTelemetryService _telemetryService;

        public TelemetryOnceEmitterTests()
        {
            var telemetrySession = new Mock<ITelemetrySession>();
            _telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => _telemetryEvents.Enqueue(x));
            _telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void TelemetryOnceEmitter_NullOrEmptyEventName_Throws(string eventName)
        {
            Assert.Throws<ArgumentException>(() => new TelemetryOnceEmitter(eventName, _telemetryService));
        }

        [Fact]
        public void EmitIfNeeded_TwoCalls_EmitsTelemetryOnce()
        {
            // Arrange
            TelemetryOnceEmitter logger = new("TestEvent", _telemetryService);

            // Act and Assert I 
            logger.EmitIfNeeded();
            Assert.NotEmpty(_telemetryEvents);
            Assert.Contains(_telemetryEvents, e => e.Name == logger.EventName);

            // Act and Assert II
            logger.EmitIfNeeded();
            Assert.Equal(1, _telemetryEvents.Count);
        }

        [Fact]
        public void EmitIfNeeded_MultipleThreads_EmitsOnce()
        {
            // Arrange
            TelemetryOnceEmitter logger = new("TestEvent", _telemetryService);

            // Act
            Parallel.ForEach(Enumerable.Range(1, 5), t =>
            {
                logger.EmitIfNeeded();
            });

            // Assert
            Assert.Equal(1, _telemetryEvents.Count);
            Assert.Contains(_telemetryEvents, e => e.Name == logger.EventName);
        }

        [Fact]
        public void Reset_WithAlreadyEmitted_Restarts()
        {
            // Arrange
            TelemetryOnceEmitter logger = new("TestEvent", _telemetryService);
            logger.EmitIfNeeded();

            // Act
            logger.Reset();
            logger.EmitIfNeeded();

            // Assert
            Assert.Equal(2, _telemetryEvents.Count);
            Assert.All(_telemetryEvents, e => Assert.Equal(logger.EventName, e.Name));
        }
    }
}
