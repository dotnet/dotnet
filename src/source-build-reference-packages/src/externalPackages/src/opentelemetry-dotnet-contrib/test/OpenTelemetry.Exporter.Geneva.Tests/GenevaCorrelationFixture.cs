// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

namespace OpenTelemetry.Exporter.Geneva.Tests;

[CollectionDefinition(nameof(GenevaCorrelationFixture))]
public class GenevaCorrelationFixture : ICollectionFixture<GenevaCorrelationFixture>
{
}
