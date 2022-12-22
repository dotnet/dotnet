// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

[Export(typeof(ProjectCapabilityResolver))]
internal class VisualStudioWindowsProjectCapabilityResolver : ProjectCapabilityResolver
{
    private readonly RazorLogger _razorLogger;

    [ImportingConstructor]
    public VisualStudioWindowsProjectCapabilityResolver(RazorLogger razorLogger)
    {
        if (razorLogger is null)
        {
            throw new ArgumentNullException(nameof(razorLogger));
        }

        _razorLogger = razorLogger;
    }

    public override bool HasCapability(object project, string capability)
    {
        if (project is not IVsHierarchy vsHierarchy)
        {
            return false;
        }

        var localHasCapability = LocalHasCapability(vsHierarchy, capability);
        return localHasCapability;
    }

    public override bool HasCapability(string documentFilePath, object project, string capability) => HasCapability(project, capability);

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
            _razorLogger.LogWarning("Could not resolve project capability for hierarchy due to NotSupportedException.");
            return false;
        }
        catch (ObjectDisposedException)
        {
            // IsCapabilityMatch throws an ObjectDisposedException if the underlying hierarchy has been disposed
            _razorLogger.LogWarning("Could not resolve project capability for hierarchy due to hierarchy being disposed.");
            return false;
        }
    }
}
