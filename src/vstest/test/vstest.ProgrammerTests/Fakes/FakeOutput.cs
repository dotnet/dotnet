// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace vstest.ProgrammerTests.Fakes;

internal class FakeOutput : IOutput
{
    public List<OutputMessage> Messages { get; } = new();
    public StringBuilder CurrentLine { get; } = new();
    public List<string> Lines { get; } = new();

    public void Write(string? message, OutputLevel level)
    {
        Messages.Add(new OutputMessage(message, level, isNewLine: false));
        CurrentLine.Append(message);
    }

    public void WriteLine(string? message, OutputLevel level)
    {
        Lines.Add(CurrentLine + message);
        CurrentLine.Clear();
    }
}
