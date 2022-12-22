// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor;
using MonoDevelop.Core;
using MonoDevelop.Core.Logging;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor;

[Export(typeof(RazorLogger))]
internal class VisualStudioMacLogger : RazorLogger
{
    public override void LogError(string message)
    {
        LoggingService.LogError(
            SR.RazorLanguageServiceGeneralError,
            message);
    }

    public override void LogVerbose(string message)
    {
        LoggingService.Log(LogLevel.Debug, message);
    }

    public override void LogWarning(string message)
    {
        LoggingService.LogWarning(message);
    }
}
