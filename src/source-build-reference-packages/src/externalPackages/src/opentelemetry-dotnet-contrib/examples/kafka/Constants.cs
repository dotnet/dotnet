// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace Examples.ConfluentKafka;

internal static class Constants
{
    public static readonly string Topic = $"test-topic-{Guid.NewGuid()}";
}
