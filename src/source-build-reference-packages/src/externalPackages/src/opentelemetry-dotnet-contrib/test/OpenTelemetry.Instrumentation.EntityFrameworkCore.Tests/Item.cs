// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.EntityFrameworkCore.Tests;

internal class Item
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
}
