// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Timers;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Internal;

internal class SteppableTimer : ISystemTimersTimer
{
    private bool _isStarted;

    public void Step()
    {
        if (_isStarted)
        {
            // Some craziness with no constructor here.
#if NET
            var elapsed = new System.Timers.ElapsedEventArgs(DateTime.UtcNow);
#else
            var elapsed = ElapsedEventArgs.Empty as ElapsedEventArgs;
#endif
            Elapsed?.Invoke(this, elapsed);
        }
    }

    private event ElapsedEventHandler? Elapsed = delegate { };

    event ElapsedEventHandler? ISystemTimersTimer.Elapsed
    {
        add
        {
            Elapsed += value;
        }

        remove
        {
            Elapsed -= value;
        }
    }

    void IDisposable.Dispose()
    {
    }

    void ISystemTimersTimer.Start()
    {
        _isStarted = true;
    }

    void ISystemTimersTimer.Stop()
    {
        _isStarted = false;
    }
}
