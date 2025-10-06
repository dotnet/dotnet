// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Razor.Documents;

internal partial class EditorDocumentManagerListener
{
    internal TestAccessor GetTestAccessor() => new(this);

    internal sealed class TestAccessor(EditorDocumentManagerListener instance)
    {
        public Task WaitUntilCurrentBatchCompletesAsync()
            => instance._workQueue.WaitUntilCurrentBatchCompletesAsync();

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
