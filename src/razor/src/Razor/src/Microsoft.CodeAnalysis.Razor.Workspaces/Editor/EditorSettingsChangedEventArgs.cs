// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.CodeAnalysis.Razor.Editor;

internal sealed class EditorSettingsChangedEventArgs(EditorSettings settings) : EventArgs
{
    public EditorSettings Settings { get; } = settings ?? throw new ArgumentNullException(nameof(settings));
}
