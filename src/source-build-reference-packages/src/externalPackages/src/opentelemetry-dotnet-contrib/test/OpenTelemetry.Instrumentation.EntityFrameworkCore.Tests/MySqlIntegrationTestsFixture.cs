// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Testcontainers.MySql;
using Xunit;

namespace OpenTelemetry.Instrumentation.EntityFrameworkCore.Tests;

public sealed class MySqlIntegrationTestsFixture : IAsyncLifetime
{
    private static readonly string MySqlImage = GetMySqlImage();

    public MySqlContainer DatabaseContainer { get; } = CreateMySql();

    public Task InitializeAsync()
        => this.DatabaseContainer.StartAsync();

    public Task DisposeAsync()
        => this.DatabaseContainer.DisposeAsync().AsTask();

    private static MySqlContainer CreateMySql()
        => new MySqlBuilder(MySqlImage).Build();

    private static string GetMySqlImage()
    {
        var assembly = typeof(MySqlIntegrationTestsFixture).Assembly;

        using var stream = assembly.GetManifestResourceStream("mysql.Dockerfile");
        using var reader = new StreamReader(stream!);

        var raw = reader.ReadToEnd();

        // Exclude FROM
        return raw.Substring(4).Trim();
    }
}
