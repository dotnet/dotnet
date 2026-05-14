// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace vstest.ProgrammerTests.Fakes;

/// <summary>
/// Contains information about something that happened, e.g. when a runtime provider resolve was called.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class ActionRecord<T>
{
    public T Value { get; }
    public string StackTrace { get; }
    public ActionRecord(T value)
    {
        StackTrace = Environment.StackTrace;
        Value = value;
    }
}
