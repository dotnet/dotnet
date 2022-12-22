// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace vstest.ProgrammerTests.Fakes;

internal class TestMessage
{
    public TestMessage(TestMessageLevel level, string? message)
    {
        Level = level;
        Message = message;
    }

    public TestMessageLevel Level { get; }
    public string? Message { get; }
}
