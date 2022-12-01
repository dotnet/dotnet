// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.CodeAnalysis.Razor;

internal abstract class RazorLogger
{
    public abstract void LogError(string message);

    public abstract void LogWarning(string message);

    public abstract void LogVerbose(string message);
}
