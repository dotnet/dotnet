// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

public class SourceDetail
{
    public string? Source { get; internal set; }
    public Architecture Architecture { get; internal set; }
    public Framework? Framework { get; internal set; }
}
