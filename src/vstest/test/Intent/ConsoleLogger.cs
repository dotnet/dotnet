// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

using static System.Console;
using static System.ConsoleColor;

namespace Intent.Console;

internal class ConsoleLogger : IRunLogger
{
    public void WriteTestPassed(MethodInfo m, TimeSpan t)
    {
        var currentColor = ForegroundColor;
        ForegroundColor = Green;
        WriteLine($"[+] {FormatMethodName(m)} {(int)t.TotalMilliseconds} ms");
        ForegroundColor = currentColor;
    }

    public void WriteTestFailure(MethodInfo m, Exception ex, TimeSpan t)
    {
        var currentColor = ForegroundColor;
        ForegroundColor = Red;
        WriteLine($"[-] {FormatMethodName(m)} {(int)t.TotalMilliseconds} ms{Environment.NewLine}{ex}");
        ForegroundColor = currentColor;
    }

    public void WriteFrameworkError(Exception ex)
    {
        var currentColor = ForegroundColor;
        ForegroundColor = DarkRed;
        WriteLine($"[-] framework failed{Environment.NewLine}{ex}{Environment.NewLine}{Environment.NewLine}");
        ForegroundColor = currentColor;
    }

    private static string FormatMethodName(MethodInfo method)
    {
        var methodName = method.GetCustomAttribute<TestAttribute>() is TestAttribute test ? test.Name : method.Name;
        if (!methodName.Contains('\n'))
        {
            return methodName;
        }

        var lines = methodName.Split('\n').Select(line => line.Trim());
        var first = lines.Take(1).ToList();
        var rest = lines.Skip(1).Select(l => $"{Environment.NewLine}     {l}").ToList();

        return string.Join(null, first.Concat(rest));
    }

    public void WriteSummary(int passed, List<(MethodInfo method, Exception exception, TimeSpan time)> failures, TimeSpan duration)
    {
        WriteLine();
        WriteLine();
        if (failures.Count > 0)
        {
            WriteLine($"There were {failures.Count} failures:");
        }
        failures.ForEach(t => { WriteTestFailure(t.method, t.exception, t.time); WriteLine(); });
        WriteLine();
        WriteLine($"Test run finished: Total: {passed + failures.Count} Passed: {passed} Failed: {failures.Count} Duration: {(int)duration.TotalMilliseconds} ms");
    }
}
