// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OpenTelemetry.Instrumentation.EntityFrameworkCore.Implementation;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Instrumentation.EntityFrameworkCore.Tests;

public class EntityFrameworkDiagnosticListenerTests : IDisposable
{
    private readonly DbContextOptions<ItemsContext> contextOptions;
    private readonly DbConnection connection;

    public EntityFrameworkDiagnosticListenerTests()
    {
        this.contextOptions = new DbContextOptionsBuilder<ItemsContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;

        this.connection = RelationalOptionsExtension.Extract(this.contextOptions).Connection!;

        this.Seed();
    }

    public static TheoryData<string, string, string> DbSystemTestCases()
    {
        var testCases = new TheoryData<string, string, string>()
        {
            { "Microsoft.EntityFrameworkCore.Cosmos", "cosmosdb", "azure.cosmosdb" },
            { "MongoDB.EntityFrameworkCore", "mongodb", "mongodb" },
        };

        // Couchbase
        string[] names =
        [
            "Couchbase.EntityFrameworkCore",
            "Couchbase.EntityFrameworkCore.Storage.Internal",
        ];

        foreach (string name in names)
        {
            testCases.Add(name, "couchbase", "couchbase");
        }

        // DB2
        names =
        [
            "IBM.EntityFrameworkCore",
            "IBM.EntityFrameworkCore-lnx",
            "IBM.EntityFrameworkCore-osx",
        ];

        foreach (string name in names)
        {
            testCases.Add(name, "db2", "ibm.db2");
        }

        // Firebird
        names =
        [
            "FirebirdSql.Data.FirebirdClient.FbCommand",
            "FirebirdSql.EntityFrameworkCore.Firebird",
        ];

        foreach (string name in names)
        {
            testCases.Add(name, "firebird", "firebirdsql");
        }

        // Microsoft SQL Server
        names =
        [
            "Microsoft.Data.SqlClient.SqlCommand",
            "Microsoft.EntityFrameworkCore.SqlServer",
        ];

        foreach (string name in names)
        {
            testCases.Add(name, "mssql", "microsoft.sql_server");
        }

        // MySQL
        names =
        [
            "Devart.Data.MySql.Entity.EFCore",
            "Devart.Data.MySql.MySqlCommand",
            "MySql.Data.EntityFrameworkCore",
            "MySql.Data.MySqlClient.MySqlCommand",
            "MySql.EntityFrameworkCore",
            "Pomelo.EntityFrameworkCore.MySql",
        ];

        foreach (string name in names)
        {
            testCases.Add(name, "mysql", "mysql");
        }

        // Oracle Database
        names =
        [
            "Devart.Data.Oracle.Entity.EFCore",
            "Devart.Data.Oracle.OracleCommand",
            "Oracle.EntityFrameworkCore",
            "Oracle.ManagedDataAccess.Client.OracleCommand",
        ];

        foreach (string name in names)
        {
            testCases.Add(name, "oracle", "oracle.db");
        }

        // PostgreSQL
        names =
        [
            "Devart.Data.PostgreSql.Entity.EFCore",
            "Devart.Data.PostgreSql.PgSqlCommand",
            "Npgsql.EntityFrameworkCore.PostgreSQL",
            "Npgsql.NpgsqlCommand",
        ];

        foreach (string name in names)
        {
            testCases.Add(name, "postgresql", "postgresql");
        }

        // SQLite
        names =
        [
            "Devart.Data.SQLite.Entity.EFCore",
            "Microsoft.Data.Sqlite.SqliteCommand",
            "Microsoft.EntityFrameworkCore.Sqlite",
        ];

        foreach (string name in names)
        {
            testCases.Add(name, "sqlite", "sqlite");
        }

        // Spanner
        names =
        [
            "Google.Cloud.EntityFrameworkCore.Spanner",
            "Google.Cloud.Spanner.Data.SpannerCommand",
        ];

        foreach (string name in names)
        {
            testCases.Add(name, "spanner", "gcp.spanner");
        }

        // Teradata
        names =
        [
            "Teradata.Client.Provider.TdCommand",
            "Teradata.EntityFrameworkCore",
        ];

        foreach (string name in names)
        {
            testCases.Add(name, "teradata", "teradata");
        }

        // Unknown providers
        names =
        [
            "foo",
            "Contoso.BusinessLogic.DataAccess.Command",
        ];

        foreach (string name in names)
        {
            testCases.Add(name, "other_sql", "other_sql");
        }

        return testCases;
    }

    public static TheoryData<string, bool> IsSqlLikeProviderTestCases()
    {
        // Get all the possible names and assume they are false
        var values = DbSystemTestCases().ToDictionary((k) => (string)k[0], (v) => false);

        // Override specific entries to be true
        string[] supported =
        [
            "Devart.Data.MySql.Entity.EFCore",
            "Devart.Data.MySql.MySqlCommand",
            "Devart.Data.Oracle.Entity.EFCore",
            "Devart.Data.Oracle.OracleCommand",
            "Devart.Data.PostgreSql.Entity.EFCore",
            "Devart.Data.PostgreSql.PgSqlCommand",
            "Devart.Data.SQLite.Entity.EFCore",
            "FirebirdSql.Data.FirebirdClient.FbCommand",
            "FirebirdSql.EntityFrameworkCore.Firebird",
            "Google.Cloud.EntityFrameworkCore.Spanner",
            "Google.Cloud.Spanner.Data.SpannerCommand",
            "IBM.EntityFrameworkCore",
            "IBM.EntityFrameworkCore-lnx",
            "IBM.EntityFrameworkCore-osx",
            "Microsoft.Data.SqlClient.SqlCommand",
            "Microsoft.Data.Sqlite.SqliteCommand",
            "Microsoft.EntityFrameworkCore.Sqlite",
            "Microsoft.EntityFrameworkCore.SqlServer",
            "MySql.Data.EntityFrameworkCore",
            "MySql.Data.MySqlClient.MySqlCommand",
            "MySql.EntityFrameworkCore",
            "Npgsql.EntityFrameworkCore.PostgreSQL",
            "Npgsql.NpgsqlCommand",
            "Oracle.EntityFrameworkCore",
            "Oracle.ManagedDataAccess.Client.OracleCommand",
            "Pomelo.EntityFrameworkCore.MySql",
            "Teradata.Client.Provider.TdCommand",
            "Teradata.EntityFrameworkCore",
        ];

        foreach (var name in supported)
        {
            values[name] = true;
        }

        var testCases = new TheoryData<string, bool>();

        foreach ((var name, var expected) in values)
        {
            testCases.Add(name, expected);
        }

        return testCases;
    }

    [Theory]
    [MemberData(nameof(DbSystemTestCases))]
    public void ShouldReturnCorrectAttributeValuesProviderOrCommandName(string name, string expectedDbSystem, string expectedDbSystemName)
    {
        (var actualDbSystem, var actualDbSystemName) = EntityFrameworkDiagnosticListener.GetDbSystemNames(name);

        Assert.Equal(expectedDbSystem, actualDbSystem);
        Assert.Equal(expectedDbSystemName, actualDbSystemName);
    }

    [Theory]
    [MemberData(nameof(IsSqlLikeProviderTestCases))]
    public void ShouldReturnCorrectValueForSqlLikeProviderOrCommandName(string name, bool expected)
    {
        var actual = EntityFrameworkDiagnosticListener.IsSqlLikeProvider(name);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EntityFrameworkContextEventsInstrumentedTest()
    {
        var exportedItems = new List<Activity>();

        using (Sdk.CreateTracerProviderBuilder()
                  .AddInMemoryExporter(exportedItems)
                  .AddEntityFrameworkCoreInstrumentation()
                  .Build())
        {
            using var context = new ItemsContext(this.contextOptions);
            var items = context.Set<Item>().OrderBy(e => e.Name).ToList();

            Assert.Equal(3, items.Count);
            Assert.Equal("ItemOne", items[0].Name);
            Assert.Equal("ItemThree", items[1].Name);
            Assert.Equal("ItemTwo", items[2].Name);
        }

        Assert.Single(exportedItems);
        var activity = exportedItems[0];

        VerifyActivityData(activity);
    }

    [Fact]
    public void EntityFrameworkEnrichDisplayNameWithEnrichWithIDbCommand()
    {
        var exportedItems = new List<Activity>();
        var expectedDisplayName = "Text main";

        using (Sdk.CreateTracerProviderBuilder()
                  .AddInMemoryExporter(exportedItems)
                  .AddEntityFrameworkCoreInstrumentation(options =>
                  {
                      options.EnrichWithIDbCommand = (activity1, command) =>
                      {
                          var stateDisplayName = $"{command.CommandType} main";
                          activity1.DisplayName = stateDisplayName;
                          activity1.SetTag("db.name", stateDisplayName);
                      };
                  })
                  .Build())
        {
            using var context = new ItemsContext(this.contextOptions);
            var items = context.Set<Item>().OrderBy(e => e.Name).ToList();

            Assert.Equal(3, items.Count);
            Assert.Equal("ItemOne", items[0].Name);
            Assert.Equal("ItemThree", items[1].Name);
            Assert.Equal("ItemTwo", items[2].Name);
        }

        Assert.Single(exportedItems);
        var activity = exportedItems[0];

        VerifyActivityData(activity, altDisplayName: $"{expectedDisplayName}");
    }

    [Fact]
    public void EntityFrameworkEnrichDisplayNameWithEnrichWithIDbCommand_New()
    {
        var exportedItems = new List<Activity>();
        var expectedDisplayName = "Text main";

        using (Sdk.CreateTracerProviderBuilder()
                  .AddInMemoryExporter(exportedItems)
                  .AddEntityFrameworkCoreInstrumentation(options =>
                  {
                      options.EnrichWithIDbCommand = (activity1, command) =>
                      {
                          var stateDisplayName = $"{command.CommandType} main";
                          activity1.DisplayName = stateDisplayName;
                          activity1.SetTag("db.namespace", stateDisplayName);
                      };
                      options.EmitNewAttributes = true;
                  })
                  .Build())
        {
            using var context = new ItemsContext(this.contextOptions);

            var items = context.Set<Item>().OrderBy(e => e.Name).ToList();

            Assert.Equal(3, items.Count);
            Assert.Equal("ItemOne", items[0].Name);
            Assert.Equal("ItemThree", items[1].Name);
            Assert.Equal("ItemTwo", items[2].Name);
        }

        Assert.Single(exportedItems);
        var activity = exportedItems[0];

        VerifyActivityData(activity, altDisplayName: $"{expectedDisplayName}", emitNewAttributes: true);
    }

    [Fact]
    public void EntityFrameworkContextExceptionEventsInstrumentedTest()
    {
        var exportedItems = new List<Activity>();

        using (Sdk.CreateTracerProviderBuilder()
                  .AddInMemoryExporter(exportedItems)
                  .AddEntityFrameworkCoreInstrumentation()
                  .Build())
        {
            using var context = new ItemsContext(this.contextOptions);

            try
            {
                context.Database.ExecuteSqlRaw("select * from no_table");
            }
            catch
            {
                // intentional empty catch
            }
        }

        Assert.Single(exportedItems);
        var activity = exportedItems[0];

        VerifyActivityData(activity, isError: true);
    }

    [Fact]
    public void ShouldNotCollectTelemetryWhenFilterEvaluatesToFalseByDbCommand()
    {
        var exportedItems = new List<Activity>();

        using (Sdk.CreateTracerProviderBuilder()
                  .AddInMemoryExporter(exportedItems)
                  .AddEntityFrameworkCoreInstrumentation(options => options.Filter = (_, command) => !command.CommandText.Contains("Item", StringComparison.OrdinalIgnoreCase))
                  .Build())
        {
            using var context = new ItemsContext(this.contextOptions);
            _ = context.Set<Item>().OrderBy(e => e.Name).ToList();
        }

        Assert.Empty(exportedItems);
    }

    [Fact]
    public void ShouldCollectTelemetryWhenFilterEvaluatesToTrueByDbCommand()
    {
        var exportedItems = new List<Activity>();

        using (Sdk.CreateTracerProviderBuilder()
                  .AddInMemoryExporter(exportedItems)
                  .AddEntityFrameworkCoreInstrumentation(options => options.Filter = (_, command) => command.CommandText.Contains("Item", StringComparison.OrdinalIgnoreCase))
                  .Build())
        {
            using var context = new ItemsContext(this.contextOptions);
            _ = context.Set<Item>().OrderBy(e => e.Name).ToList();
        }

        Assert.Single(exportedItems);
        var activity = exportedItems[0];

        Assert.True(activity.IsAllDataRequested);
        Assert.True(activity.ActivityTraceFlags.HasFlag(ActivityTraceFlags.Recorded));
    }

    [Theory]
    [InlineData("Microsoft.EntityFrameworkCore.SqlServer")]
    [InlineData("Microsoft.EntityFrameworkCore.Cosmos")]
    [InlineData("Devart.Data.SQLite.Entity.EFCore")]
    [InlineData("MySql.Data.EntityFrameworkCore")]
    [InlineData("Pomelo.EntityFrameworkCore.MySql")]
    [InlineData("Devart.Data.MySql.Entity.EFCore")]
    [InlineData("Npgsql.EntityFrameworkCore.PostgreSQL")]
    [InlineData("Devart.Data.PostgreSql.Entity.EFCore")]
    [InlineData("Oracle.EntityFrameworkCore")]
    [InlineData("Devart.Data.Oracle.Entity.EFCore")]
    [InlineData("Microsoft.EntityFrameworkCore.InMemory")]
    [InlineData("FirebirdSql.EntityFrameworkCore.Firebird")]
    [InlineData("FileContextCore")]
    [InlineData("EntityFrameworkCore.SqlServerCompact35")]
    [InlineData("EntityFrameworkCore.SqlServerCompact40")]
    [InlineData("EntityFrameworkCore.OpenEdge")]
    [InlineData("EntityFrameworkCore.Jet")]
    [InlineData("Google.Cloud.EntityFrameworkCore.Spanner")]
    [InlineData("Teradata.EntityFrameworkCore")]
    public void ShouldNotCollectTelemetryWhenFilterEvaluatesToFalseByProviderName(string provider)
    {
        var exportedItems = new List<Activity>();

        using (Sdk.CreateTracerProviderBuilder()
                  .AddInMemoryExporter(exportedItems)
                  .AddEntityFrameworkCoreInstrumentation(options => options.Filter = (providerName, _) => providerName != null && providerName.Equals(provider, StringComparison.OrdinalIgnoreCase))
                  .Build())
        {
            using var context = new ItemsContext(this.contextOptions);
            _ = context.Set<Item>().OrderBy(e => e.Name).ToList();
        }

        Assert.Empty(exportedItems);
    }

    [Fact]
    public void ShouldCollectTelemetryWhenFilterEvaluatesToTrueByProviderName()
    {
        var exportedItems = new List<Activity>();

        using (Sdk.CreateTracerProviderBuilder()
                  .AddInMemoryExporter(exportedItems)
                  .AddEntityFrameworkCoreInstrumentation(options => options.Filter = (providerName, _) => providerName != null && providerName.Equals("Microsoft.EntityFrameworkCore.Sqlite", StringComparison.OrdinalIgnoreCase))
                  .Build())
        {
            using var context = new ItemsContext(this.contextOptions);
            _ = context.Set<Item>().OrderBy(e => e.Name).ToList();
        }

        Assert.Single(exportedItems);
        var activity = exportedItems[0];

        Assert.True(activity.IsAllDataRequested);
        Assert.True(activity.ActivityTraceFlags.HasFlag(ActivityTraceFlags.Recorded));
    }

    public void Dispose() => this.connection.Dispose();

    private static SqliteConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("Filename=:memory:");

        connection.Open();

        return connection;
    }

    private static void VerifyActivityData(Activity activity, bool isError = false, string? altDisplayName = null, bool emitNewAttributes = false)
    {
        Assert.Equal(altDisplayName ?? "main", activity.DisplayName);

        Assert.Equal(ActivityKind.Client, activity.Kind);
        Assert.Equal("sqlite", activity.Tags.FirstOrDefault(t => t.Key == SemanticConventions.AttributeDbSystem).Value);

        // TBD: SqlLite not setting the DataSource so it doesn't get set.
        Assert.DoesNotContain(activity.Tags, t => t.Key == "peer.service");
        Assert.DoesNotContain(activity.Tags, t => t.Key == "server.address");
        Assert.DoesNotContain(activity.Tags, t => t.Key == "server.port");

        if (!emitNewAttributes)
        {
            Assert.Equal(altDisplayName ?? "main", activity.Tags.FirstOrDefault(t => t.Key == SemanticConventions.AttributeDbName).Value);
        }

        if (emitNewAttributes)
        {
            Assert.Equal(altDisplayName ?? "main", activity.Tags.FirstOrDefault(t => t.Key == SemanticConventions.AttributeDbNamespace).Value);
        }

        if (!isError)
        {
            Assert.Equal(ActivityStatusCode.Unset, activity.Status);
        }
        else
        {
            Assert.Equal(ActivityStatusCode.Error, activity.Status);
            Assert.Equal("SQLite Error 1: 'no such table: no_table'.", activity.StatusDescription);
        }
    }

    private void Seed()
    {
        using var context = new ItemsContext(this.contextOptions);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var one = new Item() { Name = "ItemOne" };

        var two = new Item() { Name = "ItemTwo" };

        var three = new Item() { Name = "ItemThree" };

        context.AddRange(one, two, three);

        context.SaveChanges();
    }
}
