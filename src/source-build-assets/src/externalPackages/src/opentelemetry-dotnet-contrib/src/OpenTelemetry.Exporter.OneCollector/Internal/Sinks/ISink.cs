// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Resources;

namespace OpenTelemetry.Exporter.OneCollector;

internal interface ISink<T>
    where T : class
{
    string Description { get; }

    ITransport? Transport { get; }

    int Write(Resource resource, in Batch<T> batch);
}
