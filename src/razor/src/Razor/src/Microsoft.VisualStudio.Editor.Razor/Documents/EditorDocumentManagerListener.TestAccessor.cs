// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Editor.Razor.Documents;

internal partial class EditorDocumentManagerListener
{
    internal TestAccessor GetTestAccessor() => new(this);

    internal sealed class TestAccessor(EditorDocumentManagerListener instance)
    {
        public Task ProjectChangedTask => instance._projectChangedTask;

        public event EventHandler OnChangedOnDisk
        {
            add => instance._onChangedOnDisk += value;
            remove => instance._onChangedOnDisk -= value;
        }

        public event EventHandler OnChangedInEditor
        {
            add => instance._onChangedInEditor += value;
            remove => instance._onChangedInEditor -= value;
        }

        public event EventHandler OnOpened
        {
            add => instance._onOpened += value;
            remove => instance._onOpened -= value;
        }

        public event EventHandler OnClosed
        {
            add => instance._onClosed += value;
            remove => instance._onClosed -= value;
        }
    }
}
