// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System
{
    // Helper that could be part of the BCL
    internal class DisposeAction : IDisposable
    {
        private Action _action;
        public DisposeAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            if (_action != null)
            {
                _action();
                _action = null;
            }
        }
    }
}
