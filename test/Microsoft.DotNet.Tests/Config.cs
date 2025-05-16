// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Runtime.Versioning;

namespace Microsoft.DotNet.Tests;

internal static class Config
{
    private const string ConfigSwitchPrefix = "Microsoft.DotNet.Tests.";

    public static string? BinariesReportFile => (string)AppContext.GetData(ConfigSwitchPrefix + nameof(BinariesReportFile))!;
}
