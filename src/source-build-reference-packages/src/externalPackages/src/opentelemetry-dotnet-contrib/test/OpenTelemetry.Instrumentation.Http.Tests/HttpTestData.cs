// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Reflection;
using System.Text.Json;

namespace OpenTelemetry.Instrumentation.Http.Tests;

internal static class HttpTestData
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public static IEnumerable<object[]> ReadTestCases()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var input = JsonSerializer.Deserialize<HttpOutTestCase[]>(
            assembly.GetManifestResourceStream("OpenTelemetry.Instrumentation.Http.Tests.http-out-test-cases.json")!,
            JsonSerializerOptions);
        return GetArgumentsFromTestCaseObject(input);
    }

    public static IEnumerable<object[]> GetArgumentsFromTestCaseObject(IEnumerable<HttpOutTestCase>? input)
    {
        var result = new List<object[]>();

        if (input == null)
        {
            return result;
        }

        foreach (var testCase in input)
        {
            result.Add([
                testCase
            ]);
        }

        return result;
    }

    public static string NormalizeValues(string value, string host, int port)
    {
        return value
            .Replace("{host}", host)
            .Replace("{port}", port.ToString())
            .Replace("{flavor}", "1.1");
    }
}
