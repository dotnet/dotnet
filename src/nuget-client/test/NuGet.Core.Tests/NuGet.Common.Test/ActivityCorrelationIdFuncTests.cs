// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using Xunit;

namespace NuGet.Core.FuncTest
{
    public class ActivityCorrelationIdFuncTests
    {
        [Fact]
        public async Task ActivityCorrelationId_StressTest()
        {
            // Arrange
            var taskCount = 8;
            var iterationCount = 250;

            var tasks = Enumerable
                .Range(0, taskCount)
                .Select(t => Task.Run(async () =>
                {
                    for (var i = 0; i < iterationCount; i++)
                    {
                        await ActivityCorrelationId_FlowsDownAsyncCalls();
                    }
                }))
                .ToArray();

            // Act & Assert
            await Task.WhenAll(tasks);
        }

        private async Task ActivityCorrelationId_FlowsDownAsyncCalls()
        {
            // Arrange
            ActivityCorrelationId.StartNew();
            var expected = ActivityCorrelationId.Current;

            // Act
            var actual = await AsyncCallA(TimeSpan.FromMilliseconds(1));

            // Assert
            Assert.Equal(expected, actual);
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
    }
}
