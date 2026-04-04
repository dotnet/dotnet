// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Testcontainers.PostgreSql;
using Xunit;

namespace OpenTelemetry.Instrumentation.EntityFrameworkCore.Tests;

public sealed class PostgresIntegrationTestsFixture : IAsyncLifetime
{
    private static readonly string PostgresImage = GetPostgresImage();

    public PostgreSqlContainer DatabaseContainer { get; } = CreatePostgres();

    public Task InitializeAsync()
        => this.DatabaseContainer.StartAsync();

    public Task DisposeAsync()
        => this.DatabaseContainer.DisposeAsync().AsTask();

    private static PostgreSqlContainer CreatePostgres()
        => new PostgreSqlBuilder(PostgresImage).Build();

    private static string GetPostgresImage()
    {
        var assembly = typeof(PostgresIntegrationTestsFixture).Assembly;

        using var stream = assembly.GetManifestResourceStream("postgres.Dockerfile");
        using var reader = new StreamReader(stream!);

        var raw = reader.ReadToEnd();

        // Exclude FROM
        return raw.Substring(4).Trim();
    }
}
