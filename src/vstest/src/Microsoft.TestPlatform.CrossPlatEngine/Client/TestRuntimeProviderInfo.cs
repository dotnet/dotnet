// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;

public class TestRuntimeProviderInfo
{
    public Type? Type { get; }
    public bool Shared { get; }
    public string? RunSettings { get; }
    public List<SourceDetail> SourceDetails { get; }

    public TestRuntimeProviderInfo(Type? type, bool shared, string? runSettings, List<SourceDetail> sourceDetails)
    {
        Type = type;
        Shared = shared;
        RunSettings = runSettings;
        SourceDetails = sourceDetails;
    }
}
