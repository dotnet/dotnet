// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

namespace OpenTelemetry.Exporter.Geneva.Tests;

[Collection("OtlpProtobufMetricExporterTests")]
public class OtlpProtobufMetricExporterWithPrefixTests : OtlpProtobufMetricExporterTests
{
    protected override bool PrefixBufferWithUInt32LittleEndianLength => true;
}
