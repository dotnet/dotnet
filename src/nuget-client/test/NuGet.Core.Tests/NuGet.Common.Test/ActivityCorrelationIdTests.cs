// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Xunit;

namespace NuGet.Common.Test
{
    public class ActivityCorrelationIdTests
    {
        [Fact]
        public void ActivityCorrelationId_StartNewChangesCorrelationId()
        {
            // Arrange
            ActivityCorrelationId.StartNew();
            var original = ActivityCorrelationId.Current;

            // Act
            ActivityCorrelationId.StartNew();

            // Assert
            Assert.NotEqual(original, ActivityCorrelationId.Current);
        }

        [Fact]
        public async Task ActivityCorrelationId_FlowsDownAsyncCalls()
        {
            // Arrange
            ActivityCorrelationId.StartNew();
            var expected = ActivityCorrelationId.Current;

            // Act
            var actual = await AsyncCallA(TimeSpan.FromMilliseconds(1));

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ActivityCorrelationId_DoesNotFlowUpAsyncCalls()
        {
            // Arrange
            ActivityCorrelationId.StartNew();
            var expected = ActivityCorrelationId.Current;

            // Act & Assert
            var changed = await ChangeActivityCorrelationIdAsync();
            Assert.Equal(expected, ActivityCorrelationId.Current);
            Assert.NotEqual(expected, changed);
        }

        [Fact]
        public void ActivityCorrelationId_FlowsUpSyncCalls()
        {
            // Arrange
            ActivityCorrelationId.StartNew();
            var original = ActivityCorrelationId.Current;

            // Act & Assert
            var expected = ChangeActivityCorrelationId();
            Assert.Equal(expected, ActivityCorrelationId.Current);
            Assert.NotEqual(expected, original);
        }

        [Fact]
        public async Task ActivityCorrelationId_ClearSetsCorrelationIdToEmptyGuid()
        {
            // Arrange
            await Task.Yield();
            ActivityCorrelationId.StartNew();

            // Act
            ActivityCorrelationId.Clear();

            // Assert
            Assert.Equal("00000000-0000-0000-0000-000000000000", ActivityCorrelationId.Current);
        }

        private async Task<string> AsyncCallA(TimeSpan delay)
        {
            await Task.Yield();
            await Task.Delay(delay);
            return await AsyncCallB(delay);
        }

        private async Task<string> AsyncCallB(TimeSpan delay)
        {
            await Task.Yield();
            await Task.Delay(delay);
            return await AsyncCallC(delay);
        }

        private async Task<string> AsyncCallC(TimeSpan delay)
        {
            await Task.Yield();
            await Task.Delay(delay);
            return ActivityCorrelationId.Current;
        }

        private async Task<string> ChangeActivityCorrelationIdAsync()
        {
            await Task.Yield();
            ActivityCorrelationId.StartNew();
            return ActivityCorrelationId.Current;
        }

        private string ChangeActivityCorrelationId()
        {
            ActivityCorrelationId.StartNew();
            return ActivityCorrelationId.Current;
        }
    }
}
