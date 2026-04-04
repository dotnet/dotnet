// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#nullable disable

using System.Text;
using Xunit;

namespace OpenTelemetry.Exporter.Geneva.Tests;

public class TableNameSerializerTests
{
    [Theory]
    [InlineData("Unknown", "DefaultLogs")]
    [InlineData("Unknown", "LogTableName", "LogTableName")]
    public void DefaultResolutionTests(string categoryName, string tableName, string defaultTableName = null)
    {
        var mappings = new Dictionary<string, string>();

        if (defaultTableName != null)
        {
            mappings["*"] = defaultTableName;
        }

        var options = new GenevaExporterOptions
        {
            TableNameMappings = mappings,
        };

        RunTableNameSerializerTest(categoryName, tableName, options);
    }

    [Theory]
    [InlineData("Unknown", "Unknown")]
    [InlineData("unknown.table", "Unknowntable")]
    public void PassthroughResolutionTests(string categoryName, string tableName)
    {
        var options = new GenevaExporterOptions
        {
            TableNameMappings = new Dictionary<string, string>()
            {
                ["*"] = "*",
            },
        };

        RunTableNameSerializerTest(categoryName, tableName, options);
    }

    [Theory]
    [InlineData("Unknown", "DefaultLogs")]
    [InlineData("Prefix.Nonmatch", "PrefixNonmatch")]
    [InlineData("Prefix.Sub.Nonmatch", "SubTableName")]
    [InlineData("Prefix.Sub.Final", "FinalTableName")]
    public void PrefixResolutionTests(string categoryName, string tableName)
    {
        var options = new GenevaExporterOptions
        {
            TableNameMappings = new Dictionary<string, string>()
            {
                ["Prefix"] = "*",
                ["Prefix.Sub"] = "SubTableName",
                ["Prefix.Sub.Final"] = "FinalTableName",
            },
        };

        RunTableNameSerializerTest(categoryName, tableName, options);
    }

    [Fact]
    public void ResolvedTableNameCacheTest()
    {
        var options = new GenevaExporterOptions();

        var buffer = new byte[1024];

        var tableNameSerializer = new TableNameSerializer(options, "DefaultLogs");

        Assert.Empty(tableNameSerializer.TableNameCache);

        tableNameSerializer.ResolveAndSerializeTableNameForCategoryName(buffer, 0, "MyCategory", out _);

        Assert.Single(tableNameSerializer.TableNameCache);

        tableNameSerializer.ResolveAndSerializeTableNameForCategoryName(buffer, 0, "MyCategory", out _);

        Assert.Single(tableNameSerializer.TableNameCache);

        tableNameSerializer.ResolveAndSerializeTableNameForCategoryName(buffer, 0, "MyCategory2", out _);

        Assert.Equal(2, tableNameSerializer.TableNameCache.Count);

        tableNameSerializer.ResolveAndSerializeTableNameForCategoryName(buffer, 0, "MyCategory2", out _);

        Assert.Equal(2, tableNameSerializer.TableNameCache.Count);
    }

    [Fact]
    public void TableNameCacheTest()
    {
        var options = new GenevaExporterOptions
        {
            TableNameMappings = new Dictionary<string, string>()
            {
                ["*"] = "*",
            },
        };

        var buffer = new byte[1024];

        var tableNameSerializer = new TableNameSerializer(options, "DefaultLogs");

        var numberOfCategoryNames = TableNameSerializer.MaxCachedSanitizedTableNames * 2;

        for (var i = 0; i < numberOfCategoryNames; i++)
        {
            var categoryName = $"category.{i}-test";
            var sanitizedCategoryName = $"Category{i}test";

            for (var c = 0; c < 10; c++)
            {
                var bytesWritten = tableNameSerializer.ResolveAndSerializeTableNameForCategoryName(buffer, 0, categoryName, out var tableName);

                Assert.Equal(sanitizedCategoryName.Length + 2, bytesWritten);
                Assert.Equal(sanitizedCategoryName, Encoding.ASCII.GetString(tableName.ToArray(), 2, sanitizedCategoryName.Length));
            }
        }

        var tableNameCache = tableNameSerializer.TableNameCache;

        Assert.NotNull(tableNameCache);
        Assert.Equal(numberOfCategoryNames, tableNameCache.Count);
        Assert.Equal(TableNameSerializer.MaxCachedSanitizedTableNames, tableNameCache.CachedSanitizedTableNameCount);
    }

    private static void RunTableNameSerializerTest(string categoryName, string tableName, GenevaExporterOptions options)
    {
        var buffer = new byte[1024];

        var tableNameSerializer = new TableNameSerializer(options, "DefaultLogs");

        var bytesWritten = tableNameSerializer.ResolveAndSerializeTableNameForCategoryName(buffer, 0, categoryName, out var resolvedTableName);

        Assert.Equal(tableName.Length + 2, bytesWritten);
        Assert.Equal(tableName, Encoding.ASCII.GetString(resolvedTableName.ToArray(), 2, tableName.Length));
    }
}
