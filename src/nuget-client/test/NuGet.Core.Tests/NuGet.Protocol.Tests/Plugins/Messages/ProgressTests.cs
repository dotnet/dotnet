// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class ProgressTests
    {
        [Theory]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData(-0.1)]
        [InlineData(2D)]
        public void Constructor_ThrowsForInvalidPercentage(double percentage)
        {
            var exception = Assert.Throws<ArgumentException>(() => new Progress(percentage));

            Assert.Equal("percentage", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0D)]
        [InlineData(0.5)]
        [InlineData(1D)]
        public void Constructor_InitializesPercentageProperty(double? percentage)
        {
            var progress = new Progress(percentage);

            Assert.Equal(percentage, progress.Percentage);
        }

        [Theory]
        [InlineData(null, "{}")]
        [InlineData(0.5, "{\"Percentage\":0.5}")]
        public void JsonSerialization_ReturnsCorrectJson(double? percentage, string expectedJson)
        {
            var progress = new Progress(percentage);
            var actualJson = TestUtilities.Serialize(progress);

            Assert.Equal(expectedJson, actualJson);
        }

        [Theory]
        [InlineData("{}", null)]
        [InlineData("{\"Percentage\":0}", 0D)]
        [InlineData("{\"Percentage\":0.5}", 0.5)]
        [InlineData("{\"Percentage\":1}", 1D)]
        public void JsonDeserialization_ReturnsCorrectObject(string json, double? percentage)
        {
            var progress = JsonSerializationUtilities.Deserialize<Progress>(json);

            Assert.Equal(percentage, progress.Percentage);
        }
    }
}
