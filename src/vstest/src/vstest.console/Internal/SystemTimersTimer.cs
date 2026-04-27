// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Timer = System.Timers.Timer;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;

internal sealed class SystemTimersTimer : ISystemTimersTimer
{
    private readonly Timer _timer;
    public SystemTimersTimer(int interval)
    {
        _timer = new Timer(interval);
    }
    public event System.Timers.ElapsedEventHandler? Elapsed
    {
        add => _timer.Elapsed += value;
        remove => _timer.Elapsed -= value;
    }
    public void Start() => _timer.Start();
    public void Stop() => _timer.Stop();
    public void Dispose() => _timer.Dispose();
}
