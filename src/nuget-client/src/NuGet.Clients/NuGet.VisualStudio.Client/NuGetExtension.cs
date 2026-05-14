// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.Extensibility;

namespace NuGet.VisualStudio.Client
{
    [VisualStudioContribution]
    public class NuGetExtension : Extension
    {
        public override ExtensionConfiguration ExtensionConfiguration => new()
        {
            RequiresInProcessHosting = true,
        };
    }
}
