// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.VisualStudio;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class TelemetryOnceEmitterTests
    {
        private readonly ITestOutputHelper _output;
        private readonly ConcurrentQueue<TelemetryEvent> _telemetryEvents;

        public TelemetryOnceEmitterTests(ITestOutputHelper output)
        {
            _output = output;
            var telemetrySession = new Mock<ITelemetrySession>();
            _telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => _telemetryEvents.Enqueue(x));
            TelemetryActivity.NuGetTelemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void TelemetryOnceEmitter_NullOrEmptyEventName_Throws(string eventName)
        {
            Assert.Throws<ArgumentException>(() => new TelemetryOnceEmitter(eventName));
        }

        [Fact]
        public void EmitIfNeeded_TwoCalls_EmitsTelemetryOnce()
        {
            // Arrange
            TelemetryOnceEmitter logger = new("TestEvent");

            // Act and Assert I 
            logger.EmitIfNeeded();
            Assert.NotEmpty(_telemetryEvents);
            Assert.Contains(_telemetryEvents, e => e.Name == logger.EventName);

            // Act and Assert II
            logger.EmitIfNeeded();
            Assert.Equal(1, _telemetryEvents.Count);
        }

        [Fact]
        public async Task EmitIfNeeded_MultipleThreads_EmitsOnceAsync()
        {
            // Arrange
            TelemetryOnceEmitter logger = new("TestEvent");
            Task[] tasks = new[]
            {
                new Task(() => logger.EmitIfNeeded()),
                new Task(() => logger.EmitIfNeeded()),
                new Task(() => logger.EmitIfNeeded()),
                new Task(() => logger.EmitIfNeeded()),
                new Task(() => logger.EmitIfNeeded())
            };

            // Act
            Parallel.ForEach(tasks, t =>
            {
                if (t.Status == TaskStatus.Created)
                {
                    _output.WriteLine($"{t.Id} {t.Status}");
                    t.Start();
                }
            });
            await Task.WhenAll(tasks);

            // Assert
            Assert.Equal(1, _telemetryEvents.Count);
            Assert.Contains(_telemetryEvents, e => e.Name == logger.EventName);
        }

        [Fact]
        public void Reset_WithAlreadyEmitted_Restarts()
        {
            // Arrange
            TelemetryOnceEmitter logger = new("TestEvent");
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
