// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

public abstract class ProjectConfigurationProvider
{
    public abstract bool TryResolveConfiguration(ProjectConfigurationProviderContext context, out ProjectConfiguration configuration);
}
