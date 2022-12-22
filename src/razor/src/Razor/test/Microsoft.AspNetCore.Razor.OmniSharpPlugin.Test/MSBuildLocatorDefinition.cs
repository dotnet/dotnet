// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Xunit;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

[CollectionDefinition("MSBuildLocator")]
public class MSBuildLocatorDefinition : ICollectionFixture<MSBuildLocatorFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
