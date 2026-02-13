// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net;
using AwesomeAssertions;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests
{
    public class PublishBuildToMaestroTests
    {
        [Fact]
        public void GetTimeToWait_ReadsGitHubRateLimitingHeaders()
        {
            // Unauthenticated requests to github that are rate limited don't use the Retry-After header,
            // so we need to be able to handle their X-RateLimit-Reset header.

            DateTime resetTime = DateTime.UtcNow.AddSeconds(5.0);
            long unixTime = (long)(resetTime - DateTime.UnixEpoch).TotalSeconds;
            var response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Headers =
                {
                    { "X-RateLimit-Remaining", "0" },
                    { "X-RateLimit-Reset", unixTime.ToString() },
                }
            };

            TimeSpan? actual = PublishBuildToMaestro.GetTimeToWait(response);

            actual.Should().NotBeNull();
            actual.Should().BeCloseTo(resetTime - DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void GetTimeToWait_NotWaitWhenNotLimited()
        {
            DateTime resetTime = DateTime.UtcNow.AddSeconds(5.0);
            long unixTime = (long)(resetTime - DateTime.UnixEpoch).TotalSeconds;
            var response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Headers =
                {
                    { "X-RateLimit-Remaining", "1" },
                    { "X-RateLimit-Reset", unixTime.ToString() },
                }
            };

            TimeSpan? actual = PublishBuildToMaestro.GetTimeToWait(response);

            actual.Should().BeNull();
        }

        [Fact]
        public void GetTimeToWait_UsesRetryAfterDuration()
        {
            var response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Headers =
                {
                    { "Retry-After", "5" },
                }
            };
            TimeSpan? actual = PublishBuildToMaestro.GetTimeToWait(response);
            actual.Should().NotBeNull();
            actual.Should().BeCloseTo(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void GetTimeToWait_UsesRetryAfterDate()
        {
            DateTime resetTime = DateTime.UtcNow.AddSeconds(5.0);
            var response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Headers =
                {
                    { "Retry-After", resetTime.ToString("R") },
                }
            };
            TimeSpan? actual = PublishBuildToMaestro.GetTimeToWait(response);
            actual.Should().NotBeNull();
            actual.Should().BeCloseTo(resetTime - DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }
    }
}
