// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Microsoft.VisualStudio.Razor;

[Export(typeof(IAggregateProjectCapabilityResolver))]
internal sealed class AggregateProjectCapabilityResolver : IAggregateProjectCapabilityResolver
{
    private readonly IEnumerable<IProjectCapabilityResolver> _projectCapabilityResolvers;

    [ImportingConstructor]
    public AggregateProjectCapabilityResolver([ImportMany] IEnumerable<IProjectCapabilityResolver> projectCapabilityResolvers)
    {
        _projectCapabilityResolvers = projectCapabilityResolvers;
    }

    public bool HasCapability(string documentFilePath, object project, string capability)
    {
        foreach (var capabilityResolver in _projectCapabilityResolvers)
        {
            if (capabilityResolver.HasCapability(documentFilePath, project, capability))
            {
                return true;
            }
        }

        return false;
    }
}
