// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace TestPlatform.Playground;

internal class EnvironmentVariables
{
    public static readonly Dictionary<string, string?> Variables = new()
    {
        ["VSTEST_CONNECTION_TIMEOUT"] = "999",
        ["VSTEST_DEBUG_NOBP"] = "1",
        ["VSTEST_RUNNER_DEBUG_ATTACHVS"] = "0",
        ["VSTEST_HOST_DEBUG_ATTACHVS"] = "0",
        ["VSTEST_DATACOLLECTOR_DEBUG_ATTACHVS"] = "0",
    };

}
