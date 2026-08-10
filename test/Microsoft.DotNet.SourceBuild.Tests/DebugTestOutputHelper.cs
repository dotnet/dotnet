// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Text;
using Xunit;

namespace Microsoft.DotNet.SourceBuild.Tests;

internal class DebugTestOutputHelper : ITestOutputHelper
{
    private readonly StringBuilder _output = new();

    public string Output => _output.ToString();

    public void Write(string message)
    {
        _output.Append(message);
        Debug.Write(message);
    }

    public void Write(string format, params object[] args)
    {
        Write(string.Format(format, args));
    }

    public void WriteLine(string message)
    {
        _output.AppendLine(message);
        Debug.WriteLine(message);
    }

    public void WriteLine(string format, params object[] args)
    {
        WriteLine(string.Format(format, args));
    }
}
