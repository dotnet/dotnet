// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.TestPlatform.TestUtilities;

[Serializable]
// For data source to serialize correctly to enable splitting testcases to one per test in VS,
// this must be serializable. This is sealed because the exact type must be used, not any child type.
// Otherwise it works, but silently does not split the test cases anymore.
public sealed class DebugInfo
{
    public bool DebugVSTestConsole { get; set; }
    public bool DebugTestHost { get; set; }
    public bool DebugDataCollector { get; set; }
    public bool DebugStopAtEntrypoint { get; set; }
}
