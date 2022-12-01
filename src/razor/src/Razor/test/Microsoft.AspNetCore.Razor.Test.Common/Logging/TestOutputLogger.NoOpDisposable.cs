// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.Test.Common.Logging;

internal sealed class NoOpDisposable : IDisposable
{
    public static IDisposable Instance { get; } = new NoOpDisposable();

    public void Dispose()
    {
    }
}
