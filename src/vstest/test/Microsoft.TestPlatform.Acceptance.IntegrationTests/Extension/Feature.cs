// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.AcceptanceTests;

public class Feature
{
    public Feature(string version, string issue)
    {
        Version = version;
        Issue = issue;
    }

    public string Version { get; }
    public string Issue { get; }
}
