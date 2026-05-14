// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.TestUtilities;

public static class OSUtils
{
    public static bool IsWindows { get; } = System.Environment.OSVersion.Platform.ToString().StartsWith("Win");
}
