// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Common.Test
{
    public class EnvironmentVariableWrapperTests
    {
        [Fact]
        public void Instance_WhenCalledMultipleTimes_ReturnsSameInstance()
        {
            var value0 = EnvironmentVariableWrapper.Instance;
            var value1 = EnvironmentVariableWrapper.Instance;

            Assert.Same(value0, value1);
        }

        [Fact]
        public void GetEnvironmentVariable_WhenVariableDoesNotExist_ReturnsNull()
        {
            var instance = EnvironmentVariableWrapper.Instance;

            var value = instance.GetEnvironmentVariable(Guid.NewGuid().ToString());

            Assert.Null(value);
        }

        [Fact]
        public void GetEnvironmentVariable_WhenVariableExists_ReturnsValue()
        {
            var instance = EnvironmentVariableWrapper.Instance;

            var name = Guid.NewGuid().ToString();
            var expectedValue = Guid.NewGuid().ToString();

            Environment.SetEnvironmentVariable(name, expectedValue);

            var actualValue = instance.GetEnvironmentVariable(name);

            Assert.Equal(expectedValue, actualValue);
        }
    }
}
