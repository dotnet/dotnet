// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.Test.Common.Logging;

public partial class TestOutputLogger : ILogger
{
    [ThreadStatic]
    private static StringBuilder? g_builder;

    private readonly ITestOutputHelper _testOutput;

    public string? CategoryName { get; }
    public LogLevel LogLevel { get; }

    public TestOutputLogger(
        ITestOutputHelper testOutput,
        string? categoryName = null,
        LogLevel logLevel = LogLevel.Trace)
    {
        _testOutput = testOutput ?? throw new ArgumentNullException(nameof(testOutput));
        CategoryName = categoryName;
        LogLevel = logLevel;
    }

    public IDisposable BeginScope<TState>(TState state)
        => NoOpDisposable.Instance;

    public bool IsEnabled(LogLevel logLevel)
        => logLevel >= LogLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var message = formatter(state, exception);

        var builder = GetEmptyBuilder();

        var time = DateTime.Now.TimeOfDay;
        var leadingTimeStamp = $"[{time:hh\\:mm\\:ss\\.fffffff}] ";
        var leadingSpaces = new string(' ', leadingTimeStamp.Length);
        var lines = message.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        var isFirstLine = true;

        builder.Append(leadingTimeStamp);

        if (CategoryName is { } categoryName)
        {
            builder.Append(categoryName);
            isFirstLine = false;
        }

        foreach (var line in lines)
        {
            if (!isFirstLine)
            {
                builder.AppendLine();
                builder.Append(leadingSpaces);
            }

            builder.Append(line);

            isFirstLine = false;
        }

        var finalMessage = builder.ToString();

        try
        {
            _testOutput.WriteLine(finalMessage);
        }
        catch (Exception ex)
        {
            // If an exception is thrown while writing a message, throw an AggregateException that includes
            // the message that was being logged, along with the exception that was thrown and any exception
            // that was being logged. This might provide clues to the cause.

            var innerExceptions = new List<Exception>
            {
                ex
            };

            // Were we logging an exception? If so, add that too.
            if (exception is not null)
            {
                innerExceptions.Add(exception);
            }

            var aggregateException = new AggregateException($"An exception occurred while logging: {finalMessage}", innerExceptions);
            throw aggregateException.Flatten();
        }
    }

    private static StringBuilder GetEmptyBuilder()
    {
        if (g_builder is null)
        {
            g_builder = new();
        }
        else
        {
            g_builder.Clear();
        }

        return g_builder;
    }
}
