// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace NuGet.Commands.Test;

public sealed class ContentFileUtilsTests
{
    [Theory]
    [InlineData("contentFiles/cs/net45/config/config.xml", "config/config.xml")]
    [InlineData("contentFiles/any/any/config/config.xml", "config/config.xml")]
    [InlineData("", "")]
    [InlineData("hello", "hello")]
    [InlineData("a/b/", "a/b/")]
    [InlineData("a/b/c/", "")]
    [InlineData("a/b/c", "a/b/c")]
    [InlineData("///config/config.xml", "config/config.xml")]
    [InlineData("/a/b/config/config.xml", "config/config.xml")]
    public void GetContentFileFolderRelativeToFramework(string input, string expected)
    {
        using var _ = new SuppressAsserts();

        string actual = ContentFileUtils.GetContentFileFolderRelativeToFramework(input.AsSpan());

        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Suppresses the default trace listeners for a specific duration. Useful to ensure that
    /// debug asserts don't block unit tests indefinitely by showing modal dialog messages
    /// about assertion failures.
    /// </summary>
    private sealed class SuppressAsserts : IDisposable
    {
        private readonly TraceListener[] _suppressedListeners;

        public SuppressAsserts()
        {
            _suppressedListeners = Trace.Listeners.Cast<TraceListener>().ToArray();

            Trace.Listeners.Clear();
        }

        public void Dispose()
        {
            Trace.Listeners.AddRange(_suppressedListeners);
        }
    }
}
