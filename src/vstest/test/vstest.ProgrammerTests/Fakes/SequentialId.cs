// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace vstest.ProgrammerTests.Fakes;

/// <summary>
/// A sequential Id that starts from 0 or a given number. Put this in a static field in your class, and call Next to get the next Id.
/// </summary>
internal class SequentialId
{
    private int _id;

    public SequentialId() : this(0)
    {
    }

    public SequentialId(int firstId)
    {
        _id = firstId;
    }

    public int Next()
    {
        return Interlocked.Increment(ref _id);
    }
}
