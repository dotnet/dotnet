// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

internal sealed class ContextChangeEventArgs(ContextChangeKind kind) : EventArgs
{
    public ContextChangeKind Kind { get; } = kind;
}
