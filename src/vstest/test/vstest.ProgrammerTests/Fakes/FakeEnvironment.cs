// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace vstest.ProgrammerTests.Fakes;

internal class FakeEnvironment : IEnvironment
{
    private readonly IEnvironment _environment = new PlatformEnvironment();

    public PlatformArchitecture Architecture => _environment.Architecture;

    public PlatformOperatingSystem OperatingSystem => _environment.OperatingSystem;

    public string OperatingSystemVersion => _environment.OperatingSystemVersion;

    public int ProcessorCount => _environment.ProcessorCount;

    public void Exit(int exitcode) => _environment.Exit(exitcode);

    public int GetCurrentManagedThreadId() => _environment.GetCurrentManagedThreadId();
}
