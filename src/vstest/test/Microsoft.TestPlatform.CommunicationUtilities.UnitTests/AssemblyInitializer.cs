// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests;

[TestClass]
public static class AssemblyInitializer
{
    [AssemblyInitialize]
    public static void SetTimeout(TestContext testContext)
    {
        // Set the value to 1 to avoid big timeouts for these unit tests. If you change the value you
        // must do it at the start of a doNotParallelize test, to ensure you don't clean the default,
        // because after it resets to 90 seconds.
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, "1");
    }
}
