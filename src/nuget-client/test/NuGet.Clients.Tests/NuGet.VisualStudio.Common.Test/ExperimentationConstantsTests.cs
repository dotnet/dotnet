// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using FluentAssertions;
using Xunit;

namespace NuGet.VisualStudio.Common.Test
{
    public class ExperimentationConstantsTests
    {
        [Fact]
        public void Constructor_WithNullFlightFlag_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new ExperimentationConstants(null, "value"));
        }

        [Fact]
        public void Constructor_WithNullFlightExperimentalVariable_DoesNotThrow()
        {
            var constant = new ExperimentationConstants("value", null);

            constant.FlightEnvironmentVariable.Should().BeNull();
        }
    }
}
