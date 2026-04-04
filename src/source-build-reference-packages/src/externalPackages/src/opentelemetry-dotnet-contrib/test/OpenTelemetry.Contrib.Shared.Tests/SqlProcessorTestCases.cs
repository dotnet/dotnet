// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace OpenTelemetry.Instrumentation.Tests;

public static class SqlProcessorTestCases
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() },
    };

    private static readonly HashSet<string> DbSystemTestCasesToExecute = ["other_sql", "microsoft.sql_server"];

    public static TheoryData<TestCase> GetSemanticConventionsTestCases()
    {
        var data = new TheoryData<TestCase>();

        var assembly = Assembly.GetExecutingAssembly();
        var input = JsonSerializer.Deserialize<TestCase[]>(
            assembly.GetManifestResourceStream("SqlProcessorTestCases.json")!,
            JsonSerializerOptions)!;

        if (input is not null)
        {
            foreach (var testCase in input)
            {
                // Skipping this for now as we need to discuss the expected output.
                if (testCase.Name == "alter_table")
                {
                    continue;
                }

                if (DbSystemTestCasesToExecute.Contains(testCase.Input.DbSystemName))
                {
                    data.Add(testCase);
                }
            }
        }

        // Reintroduce "alter_table" with a revised expected output.
        // We don't expect the number '255' to be redacted when defining the size of a column.
        data.Add(new()
        {
            Name = "alter_table",
            Input = new()
            {
                DbSystemName = "other_sql",
                Query = "ALTER  TABLE MyTable ADD Name varchar(255)",
            },
            Expected = new()
            {
                SanitizedQueryText = ["ALTER  TABLE MyTable ADD Name varchar(?)", "ALTER  TABLE MyTable ADD Name varchar(255)"],
                Summary = "ALTER TABLE MyTable",
            },
        });

        input = JsonSerializer.Deserialize<TestCase[]>(
            assembly.GetManifestResourceStream("SqlProcessorAdditionalTestCases.json")!,
            JsonSerializerOptions)!;

        if (input is not null)
        {
            foreach (var testCase in input)
            {
                if (DbSystemTestCasesToExecute.Contains(testCase.Input.DbSystemName))
                {
                    data.Add(testCase);
                }
            }
        }

        return data;
    }

#pragma warning disable CA1034 // Nested types should not be visible
    public class TestCase : IXunitSerializable
    {
        public string Name { get; set; } = string.Empty;

        public string? Skip { get; set; }

        public TestCaseInput Input { get; set; } = new();

        public TestCaseExpected Expected { get; set; } = new();

        public void Deserialize(IXunitSerializationInfo info)
        {
            var json = info.GetValue<string>("json") ?? string.Empty;

            var proxy = JsonSerializer.Deserialize<TestCase>(json, JsonSerializerOptions);

            if (proxy is not null)
            {
                this.Name = proxy.Name;
                this.Input = proxy.Input;
                this.Expected = proxy.Expected;
                this.Skip = proxy.Skip;
            }
            else
            {
                throw new InvalidOperationException("Deserialized TestCase is null.");
            }
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            var json = JsonSerializer.Serialize(this, JsonSerializerOptions);
            info.AddValue("json", json);
        }

        public override string ToString() => this.Name;
    }

    public class TestCaseInput
    {
        [JsonPropertyName("db.system.name")]
        public string DbSystemName { get; set; } = string.Empty;

        public string Query { get; set; } = string.Empty;
    }

    public class TestCaseExpected
    {
        [JsonPropertyName("db.query.text")]
#pragma warning disable CA1819 // Properties should not return arrays
        public string[] SanitizedQueryText { get; set; } = [];
#pragma warning restore CA1819

        [JsonPropertyName("db.query.summary")]
        public string? Summary { get; set; }
    }
#pragma warning restore CA1034
}
