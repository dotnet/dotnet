// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Composition;

namespace Microsoft.VisualStudio.Editor.Razor;

[Shared]
[Export(typeof(AggregateProjectCapabilityResolver))]
internal class AggregateProjectCapabilityResolver : ProjectCapabilityResolver
{
    private readonly IEnumerable<ProjectCapabilityResolver> _projectCapabilityResolvers;

    [ImportingConstructor]
    public AggregateProjectCapabilityResolver([ImportMany] IEnumerable<ProjectCapabilityResolver> projectCapabilityResolvers)
    {
        _projectCapabilityResolvers = projectCapabilityResolvers;
    }

    public override bool HasCapability(object project, string capability)
    {
        foreach (var capabilityResolver in _projectCapabilityResolvers)
        {
            if (capabilityResolver.HasCapability(project, capability))
            {
                return true;
            }
        }

        return false;
    }

    public override bool HasCapability(string documentFilePath, object project, string capability)
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
