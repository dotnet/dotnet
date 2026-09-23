// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.ScenarioTests.SdkTemplateTests;

internal static class TestCategories
{
    // Temporarily excluded from VMR runs until runtime targets net12.0: https://github.com/dotnet/dotnet/issues/9637
    public const string RequiresNet12RuntimeTargetingPack = nameof(RequiresNet12RuntimeTargetingPack);
}
