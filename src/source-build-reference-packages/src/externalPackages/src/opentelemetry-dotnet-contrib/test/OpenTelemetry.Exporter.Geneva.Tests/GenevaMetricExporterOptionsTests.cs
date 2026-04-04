// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#nullable disable

using Xunit;

namespace OpenTelemetry.Exporter.Geneva.Tests;

public class GenevaMetricExporterOptionsTests
{
    [Fact]
    public void InvalidPrepopulatedDimensions()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            var exporterOptions = new GenevaMetricExporterOptions { PrepopulatedMetricDimensions = null };
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var exporterOptions = new GenevaMetricExporterOptions
            {
                PrepopulatedMetricDimensions = new Dictionary<string, object>
                {
                    ["DimensionKey"] = null,
                },
            };
        });

        var invalidDimensionNameException = Assert.Throws<ArgumentException>(() =>
        {
            var exporterOptions = new GenevaMetricExporterOptions
            {
                PrepopulatedMetricDimensions = new Dictionary<string, object>
                {
                    [new string('a', GenevaMetricExporter.MaxDimensionNameSize + 1)] = "DimensionValue",
                },
            };
        });

        var expectedErrorMessage = $"The dimension: {new string('a', GenevaMetricExporter.MaxDimensionNameSize + 1)} exceeds the maximum allowed limit of {GenevaMetricExporter.MaxDimensionNameSize} characters for a dimension name.";
        Assert.Equal(expectedErrorMessage, invalidDimensionNameException.Message);

        var invalidDimensionValueException = Assert.Throws<ArgumentException>(() =>
        {
            var exporterOptions = new GenevaMetricExporterOptions
            {
                PrepopulatedMetricDimensions = new Dictionary<string, object>
                {
                    ["DimensionKey"] = new string('a', GenevaMetricExporter.MaxDimensionValueSize + 1),
                },
            };
        });

        expectedErrorMessage = $"Value provided for the dimension: DimensionKey exceeds the maximum allowed limit of {GenevaMetricExporter.MaxDimensionValueSize} characters for dimension value.";
        Assert.Equal(expectedErrorMessage, invalidDimensionValueException.Message);
    }

    [Fact]
    public void MetricExportIntervalValidationTest()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var exporterOptions = new GenevaMetricExporterOptions
            {
                MetricExportIntervalMilliseconds = 999,
            };
        });

        var exception = Record.Exception(() =>
        {
            var exporterOptions = new GenevaMetricExporterOptions
            {
                MetricExportIntervalMilliseconds = 1000,
            };
        });

        Assert.Null(exception);
    }
}
