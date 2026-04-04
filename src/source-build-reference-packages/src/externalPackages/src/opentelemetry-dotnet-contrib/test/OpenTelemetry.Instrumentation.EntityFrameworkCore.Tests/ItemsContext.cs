// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.EntityFrameworkCore;

namespace OpenTelemetry.Instrumentation.EntityFrameworkCore.Tests;

internal class ItemsContext(DbContextOptions<ItemsContext> options) : DbContext(options)
{
    public DbSet<Item> Items { get; set; } = null!;
}
