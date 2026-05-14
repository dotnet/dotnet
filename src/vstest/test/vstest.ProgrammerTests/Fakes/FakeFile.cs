// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace vstest.ProgrammerTests.Fakes;

internal class FakeFile
{
    public string Path { get; }

    public FakeFile(string path)
    {
        Path = path;
    }
}
