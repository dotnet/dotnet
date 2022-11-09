// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.TestPlatform.TestUtilities;

// For data source to serialize correctly to enable splitting testcases to one per test in VS,
// this must be serializable. This is NOT sealed because we need this for adapters and testSdk.
// But be aware that the exact type must be used, not any child type for the data on data source object (RunnerInfo).
// Otherwise it works, but silently does not split the test cases anymore.
[Serializable]
public class DllInfo
{
    public string? Name { get; set; }
    public string? PropertyName { get; set; }
    public string? VersionType { get; set; }
    public string? Version { get; set; }
    public string? Path { get; set; }

    public override string ToString() => $" {Name} = {Version} [{VersionType}]";
}
