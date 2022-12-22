// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Common.Hosting;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace TestPlatform.CrossPlatEngine.UnitTests;

public class TestableTestEngine : TestEngine
{
    public TestableTestEngine(IProcessHelper processHelper)
        : base(TestRuntimeProviderManager.Instance, processHelper)
    {
    }
}
