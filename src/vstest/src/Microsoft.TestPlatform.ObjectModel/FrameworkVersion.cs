// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// This holds the major desktop framework versions. This is just to maintain compatibility with older runsettings files.
/// </summary>
public enum FrameworkVersion
{
    None,
    Framework35,
    Framework40,
    Framework45,
    FrameworkCore10,
    FrameworkUap10
}
