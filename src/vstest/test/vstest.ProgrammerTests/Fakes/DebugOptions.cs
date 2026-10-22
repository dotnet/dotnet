// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#pragma warning disable IDE1006 // Naming Styles
// For some reason only this occurence of vstest is flagged in build, and no other.
namespace vstest.ProgrammerTests.Fakes;
#pragma warning restore IDE1006 // Naming Styles

internal class DebugOptions
{
    public const int DefaultTimeout = 5;
    // TODO: This setting is actually quite pointless, because I cannot come up with
    // a useful way to abort quickly enough when debugger is attached and I am just running my tests (pressing F5)
    // but at the same time not abort when I am in the middle of debugging some behavior. Maybe looking at debugger,
    // and asking it if any breakpoints were hit / are set. But that is difficult.
    //
    // So normally I press F5 to investigate, but Ctrl+F5 (run without debugger), to run tests.
    public const int DefaultDebugTimeout = 30 * 60;
    public const bool DefaultBreakOnAbort = true;
    public int Timeout { get; init; } = DefaultTimeout;
    public int DebugTimeout { get; init; } = DefaultDebugTimeout;
    public bool BreakOnAbort { get; init; } = DefaultBreakOnAbort;
}
