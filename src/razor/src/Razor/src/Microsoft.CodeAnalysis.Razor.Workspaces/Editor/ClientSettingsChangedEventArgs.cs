// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.CodeAnalysis.Razor.Editor;

internal sealed class ClientSettingsChangedEventArgs(ClientSettings settings) : EventArgs
{
    public ClientSettings Settings { get; } = settings ?? throw new ArgumentNullException(nameof(settings));
}
