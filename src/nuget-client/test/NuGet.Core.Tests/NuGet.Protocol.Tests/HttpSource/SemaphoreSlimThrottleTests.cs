// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class SemaphoreSlimThrottleTests
    {
        [Fact]
        public async Task SemaphoreSlimThrottle_RespectsInnerSemaphoreAsync()
        {
            // Arrange
            var semaphoreSlim = new SemaphoreSlim(2);
            var target = new SemaphoreSlimThrottle(semaphoreSlim);

            // Act
            await target.WaitAsync();
            var countA = target.CurrentCount;
            await target.WaitAsync();
            var countB = target.CurrentCount;
            target.Release();
            var countC = target.CurrentCount;
            target.Release();
            var countD = target.CurrentCount;

            // Assert
            Assert.Equal(1, countA);
            Assert.Equal(0, countB);
            Assert.Equal(1, countC);
            Assert.Equal(2, countD);
        }

        [Fact]
        public async Task SemaphoreSlimThrottle_CreateBinarySemaphore_HasInitialCountOfOneAsync()
        {
            // Arrange
            var target = SemaphoreSlimThrottle.CreateBinarySemaphore();

            // Act
            var beforeAcquiringCount = target.CurrentCount;
            await target.WaitAsync();
            var afterAcquiringCount = target.CurrentCount;
            target.Release();
            var afterReleaseCount = target.CurrentCount;

            // Assert
            Assert.True(beforeAcquiringCount == 1, "The binary semaphore should only allow a count of one.");
            Assert.True(afterAcquiringCount == 0, "The binary semaphore should not allow more than one thread to enter.");
            Assert.True(afterReleaseCount == 1, "The binary semaphore should have released back to a count of one.");
        }
    }
}
