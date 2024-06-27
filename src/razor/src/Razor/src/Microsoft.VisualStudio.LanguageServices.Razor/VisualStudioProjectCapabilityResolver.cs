// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.VisualStudio.Razor;

[Export(typeof(IProjectCapabilityResolver))]
[method: ImportingConstructor]
internal sealed class VisualStudioProjectCapabilityResolver(ILoggerFactory loggerFactory) : IProjectCapabilityResolver
{
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<VisualStudioProjectCapabilityResolver>();

    public bool HasCapability(string documentFilePath, object project, string capability)
    {
        if (project is not IVsHierarchy vsHierarchy)
        {
            return false;
        }

        var localHasCapability = LocalHasCapability(vsHierarchy, capability);
        return localHasCapability;
    }

    private bool LocalHasCapability(IVsHierarchy hierarchy, string capability)
    {
        try
        {
            var hasCapability = hierarchy.IsCapabilityMatch(capability);
            return hasCapability;
        }
        catch (NotSupportedException)
        {
            // IsCapabilityMatch throws a NotSupportedException if it can't create a
            // BooleanSymbolExpressionEvaluator COM object
            _logger.LogWarning($"Could not resolve project capability for hierarchy due to NotSupportedException.");
            return false;
        }
        catch (ObjectDisposedException)
        {
            // IsCapabilityMatch throws an ObjectDisposedException if the underlying hierarchy has been disposed
            _logger.LogWarning($"Could not resolve project capability for hierarchy due to hierarchy being disposed.");
            return false;
        }
    }
}
