// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.Editor.Razor;

internal abstract class EditorSettingsChangedTrigger
{
    public abstract void Initialize(EditorSettingsManager editorSettingsManager);
}
