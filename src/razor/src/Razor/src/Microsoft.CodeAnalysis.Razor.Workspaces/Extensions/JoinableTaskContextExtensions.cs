// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;

internal static class JoinableTaskContextExtensions
{
    public static void AssertUIThread(this JoinableTaskContext joinableTaskContext, [CallerMemberName] string? caller = null)
    {
        if (!joinableTaskContext.IsOnMainThread)
        {
            caller = caller is null ? "The method" : $"'{caller}'";
            throw new InvalidOperationException($"{caller} must be called on the UI thread.");
        }
    }
}
