// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using Xunit;

namespace OpenTelemetry.Extensions.Tests;

public sealed class ActivityEventAttachingLogProcessorTests : IDisposable
{
    private readonly ActivitySource activitySource = new ActivitySource("Test");
    private readonly ActivityListener activityListener = new ActivityListener
    {
        ShouldListenTo = source => true,
    };

    private bool sampled;

    public ActivityEventAttachingLogProcessorTests()
    {
        this.activityListener.Sample = (ref ActivityCreationOptions<ActivityContext> options) =>
        {
            return this.sampled
                ? ActivitySamplingResult.AllDataAndRecorded
                : ActivitySamplingResult.PropagationData;
        };

        ActivitySource.AddActivityListener(this.activityListener);
    }

    public void Dispose()
    {
        this.activitySource.Dispose();
        this.activityListener.Dispose();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(true, 18, true, true)]
    [InlineData(true, 0, false, true, true)]
    [InlineData(true, 18, true, true, false, true)]
    [InlineData(true, 0, false, true, true, true)]
    public void AttachLogsToActivityEventTest(
        bool sampled,
        int eventId = 0,
        bool includeFormattedMessage = false,
        bool includeScopes = false,
        bool recordException = false,
        bool? filter = null)
    {
        this.sampled = sampled;

        using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddOpenTelemetry(options =>
                {
                    options.IncludeScopes = includeScopes;
                    options.IncludeFormattedMessage = includeFormattedMessage;
                    options.AttachLogsToActivityEvent(x =>
                    {
                        x.Filter = filter switch
                        {
                            true => _ => true,
                            false => _ => false,
                            null => null,
                        };
                    });
                });
                builder.AddFilter(typeof(ActivityEventAttachingLogProcessorTests).FullName, LogLevel.Trace);
            });

        ILogger logger = loggerFactory.CreateLogger<ActivityEventAttachingLogProcessorTests>();
        var activity = this.activitySource.StartActivity("Test");
        Assert.NotNull(activity);

        using var scope = logger.BeginScope("{NodeId}", 99);

        logger.LogInformation(eventId, "Hello OpenTelemetry {UserId}!", 8);

        Activity? innerActivity = null;
        if (recordException)
        {
            innerActivity = this.activitySource.StartActivity("InnerTest");
            Assert.NotNull(innerActivity);

            using var innerScope = logger.BeginScope("{RequestId}", "1234");

            logger.LogError(new InvalidOperationException("Goodbye OpenTelemetry."), "Exception event.");

            innerActivity.Dispose();
        }

        activity.Dispose();

        if (sampled)
        {
            ActivityEvent? logEvent = activity.Events.FirstOrDefault();

            Assert.NotNull(logEvent);
            Assert.Equal("log", logEvent.Value.Name);

            var tags = logEvent.Value.Tags?.ToDictionary(i => i.Key, i => i.Value);
            Assert.NotNull(tags);

            Assert.Equal("OpenTelemetry.Extensions.Tests.ActivityEventAttachingLogProcessorTests", tags[nameof(LogRecord.CategoryName)]);
            Assert.Equal(LogLevel.Information, tags[nameof(LogRecord.LogLevel)]);

            if (eventId != 0)
            {
                Assert.Equal((EventId)eventId, tags[nameof(LogRecord.EventId)]);
            }
            else
            {
                Assert.DoesNotContain(tags, kvp => kvp.Key == nameof(LogRecord.EventId));
            }

            if (includeFormattedMessage)
            {
                Assert.Equal("Hello OpenTelemetry 8!", tags[nameof(LogRecord.FormattedMessage)]);
            }
            else
            {
                Assert.DoesNotContain(tags, kvp => kvp.Key == nameof(LogRecord.FormattedMessage));
            }

            Assert.Equal(8, tags["state.UserId"]);

            if (includeScopes)
            {
                Assert.Equal(99, tags["scope[0].NodeId"]);
            }
            else
            {
                Assert.DoesNotContain(tags, kvp => kvp.Key == "scope[0].NodeId");
            }

            if (recordException)
            {
                Assert.NotNull(innerActivity);
                ActivityEvent? exLogEvent = innerActivity.Events.FirstOrDefault();

                Assert.NotNull(exLogEvent);
                Assert.Equal("log", exLogEvent.Value.Name);

                var exLogTags = exLogEvent.Value.Tags?.ToDictionary(i => i.Key, i => i.Value);
                Assert.NotNull(exLogTags);

                Assert.Equal(99, exLogTags["scope[0].NodeId"]);
                Assert.Equal("1234", exLogTags["scope[1].RequestId"]);

                ActivityEvent? exEvent = innerActivity.Events.Skip(1).FirstOrDefault();

                Assert.NotNull(exEvent);
                Assert.Equal("exception", exEvent.Value.Name);

                var exTags = exEvent.Value.Tags?.ToDictionary(i => i.Key, i => i.Value);
                Assert.NotNull(exTags);

                Assert.Equal("System.InvalidOperationException", exTags["exception.type"]);
                Assert.Equal("Goodbye OpenTelemetry.", exTags["exception.message"]);
                Assert.Contains(exTags, kvp => kvp.Key == "exception.stacktrace");
            }
        }
        else
        {
            Assert.Empty(activity.Events);
        }
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, true)]
    [InlineData(true, true, 18, true, true, true)]
    [InlineData(true, true, 0, false, false, true, true)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    [InlineData(true, false, 18, true, true, true)]
    [InlineData(true, false, 0, false, false, true, true)]
    public void AttachLogsToActivityEventTest_Filter(
        bool sampled,
        bool filterThrows,
        int eventId = 0,
        bool includeFormattedMessage = false,
        bool parseStateValues = false,
        bool includeScopes = false,
        bool recordException = false)
    {
        this.sampled = sampled;

        using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddOpenTelemetry(options =>
                {
                    options.IncludeScopes = includeScopes;
                    options.IncludeFormattedMessage = includeFormattedMessage;
                    options.ParseStateValues = parseStateValues;
                    options.AttachLogsToActivityEvent(x => x.Filter = _ => filterThrows
                        ? throw new Exception()
                        : false);
                });
                builder.AddFilter(typeof(ActivityEventAttachingLogProcessorTests).FullName, LogLevel.Trace);
            });

        ILogger logger = loggerFactory.CreateLogger<ActivityEventAttachingLogProcessorTests>();
        var activity = this.activitySource.StartActivity("Test");
        Assert.NotNull(activity);

        using var scope = logger.BeginScope("{NodeId}", 99);

        logger.LogInformation(eventId, "Hello OpenTelemetry {UserId}!", 8);

        if (recordException)
        {
            var innerActivity = this.activitySource.StartActivity("InnerTest");
            Assert.NotNull(innerActivity);

            using var innerScope = logger.BeginScope("{RequestId}", "1234");

            logger.LogError(new InvalidOperationException("Goodbye OpenTelemetry."), "Exception event.");

            innerActivity.Dispose();
        }

        activity.Dispose();

        if (sampled)
        {
            Assert.DoesNotContain(activity.Events, x => x.Name == "log");
        }
        else
        {
            Assert.Empty(activity.Events);
        }
    }
}
