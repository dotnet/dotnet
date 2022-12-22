// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.Build.Locator;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

public sealed class MSBuildLocatorFixture : IDisposable
{
    public MSBuildLocatorFixture()
    {
        if (!MSBuildLocator.IsRegistered)
        {
            MSBuildLocator.RegisterDefaults();
        }
    }

    public void Dispose()
    {
        MSBuildLocator.Unregister();
    }
}
