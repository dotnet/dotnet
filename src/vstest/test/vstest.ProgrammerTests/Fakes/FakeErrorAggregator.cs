// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace vstest.ProgrammerTests.Fakes;

internal class FakeErrorAggregator
{
    public List<object> Errors { get; } = new();

    public void Add(object error)
    {
        Errors.Add(error);
    }
}
