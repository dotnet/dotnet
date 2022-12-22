// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Media;
using NuGet.VisualStudio;

namespace NuGetConsole
{
    /// <summary>
    /// Contains a shared implementation of <see cref="IOutputConsole"/>. This class declares the methods that need implemented as abstract.
    /// </summary>
    internal abstract class SharedOutputConsole : IOutputConsole
    {
        public int ConsoleWidth => 120;

        // The next 3 methods are the ones that need to be overriden
        public abstract Task ActivateAsync();

        public abstract Task ClearAsync();

        public abstract Task WriteAsync(string text);

        public Task WriteAsync(string text, Color? foreground, Color? background)
        {
            // the output window doesn't allow setting text color
            return WriteAsync(text);
        }

        public Task WriteBackspaceAsync()
        {
            throw new NotSupportedException();
        }

        public Task WriteLineAsync(string text)
        {
            return WriteAsync(text + Environment.NewLine);
        }

        public Task WriteLineAsync(string format, params object[] args)
        {
            return WriteLineAsync(string.Format(CultureInfo.CurrentCulture, format, args));
        }

        public Task WriteProgressAsync(string operation, int percentComplete)
        {
            return Task.CompletedTask;
        }
    }
}
