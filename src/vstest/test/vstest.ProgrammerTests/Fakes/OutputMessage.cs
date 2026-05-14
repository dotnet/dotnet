// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace vstest.ProgrammerTests.Fakes;

internal class OutputMessage
{
    public OutputMessage(string? message, OutputLevel level, bool isNewLine)
    {
        Message = message;
        Level = level;
        IsNewLine = isNewLine;
    }

    public string? Message { get; }
    public OutputLevel Level { get; }
    public bool IsNewLine { get; }
}
